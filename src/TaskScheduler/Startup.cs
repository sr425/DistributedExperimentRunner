using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using TaskScheduler.Services;

namespace TaskScheduler
{
    public class Startup
    {
        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services)
        {
            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_1);
            /*string connectionString = Configuration.AsEnumerable ()
                .Where (pair => pair.Key == "ConnectionStrings:QueueDb:" && !string.IsNullOrWhiteSpace (pair.Value))
                .Select (p => p.Value)
                .FirstOrDefault ();*/

            services.AddDbContext<ApplicationDbContext> (opt => opt.UseSqlServer (Configuration.GetConnectionString ("QueueDb"))); //Configuration.GetConnectionString ("QueueDb"))); //opt.UseSqlite("Data Source=queue.db"));
            services.AddSingleton<StaticAuthentication> ();
            //services.AddTransient<IPersistenceProvider, CosmosDbPersistenceProvider>();
            services.AddScoped<IPersistenceProvider, SqlPersistenceProvider> ();
            services.AddSingleton<IHostedService, WorkpackageQueuerService> ();

            services.AddSwaggerGen (c =>
            {
                c.SwaggerDoc ("v1", new Info { Title = "Task Scheduler", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment ())
            {
                app.UseDeveloperExceptionPage ();
            }
            else
            {
                app.UseHsts ();
            }

            //app.UseHttpsRedirection();
            app.UseMvc ();

            app.UseSwagger ();
            app.UseSwaggerUI (c =>
            {
                c.SwaggerEndpoint ("/swagger/v1/swagger.json", "Task Scheduler API V1");
            });
        }
    }
}