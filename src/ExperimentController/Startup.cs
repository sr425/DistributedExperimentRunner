using System.Text;
using AutoMapper;
using ExperimentController.Model;
using ExperimentController.Services;
using ExperimentController.Services.TaskCreation;
using ExperimentController.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace ExperimentController
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite("Data Source=test.db")); //opt => opt.UseInMemoryDatabase ("Test"));

            services.AddAuthentication()
                /*.AddJwtBearer("Internal", opt =>
               {
                   opt.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidIssuer = Configuration["JwtIssuer"],
                       ValidateIssuer = true,
                       ValidAudience = Configuration["JwtAudience"],
                       ValidateAudience = false,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SigningKey"]))
                   };
               })*/
                .AddJwtBearer("Google", opt =>
               {
                   opt.Authority = Configuration["GoogleIssuer"];
                   opt.Audience = Configuration["GoogleAudience"];
                   opt.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true
                   };
               });

            services.AddAuthorization(options =>
           {
               options.DefaultPolicy = new AuthorizationPolicyBuilder()
                   .RequireAuthenticatedUser()
                   .AddAuthenticationSchemes("Google")
                   .Build();
           });

            services.AddTransient<ExperimentRunManager>();
            services.AddTransient<ExperimentEvaluation>();
            services.AddTransient<ITaskManager, TaskSchedulerTaskManager>();
            services.AddSingleton<TaskGenerator>();

            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, QueueResultFetcherService>();

            services.AddCors(opt => opt.AddPolicy("AllowAll", builder =>
          {
              builder.AllowAnyOrigin();
              builder.AllowAnyHeader();
              builder.AllowAnyMethod();
              builder.AllowCredentials();
          }));

            services.AddAutoMapper();

            services.AddMvc(opt =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Google")
                    .Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new Info { Title = "Optimizer Distributed", Version = "v1" });
           });
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IMapper autoMapper)
        {
            autoMapper.ConfigurationProvider.AssertConfigurationIsValid();

            app.UseCors("AllowAll");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection ();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
           {
               c.SwaggerEndpoint("/swagger/v1/swagger.json", "Distributed Optimizer API V1");
           });

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}