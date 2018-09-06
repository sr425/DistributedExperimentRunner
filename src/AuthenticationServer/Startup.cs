using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AuthenticationServer.Model;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace AuthenticationServer
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
            services.AddDbContext<ApplicationDbContext> (opt => opt.UseSqlite ("Data Source=authentication.db")); //opt => opt.UseInMemoryDatabase ("Test"));

            {
                var builder = services.AddIdentityCore<ApplicationUser> (o =>
                {
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 6;
                });

                builder = new IdentityBuilder (builder.UserType, typeof (IdentityRole), builder.Services);
                builder.AddEntityFrameworkStores<ApplicationDbContext> ().AddDefaultTokenProviders ();
            }

            services.AddTransient<JwtCoder> ();

            services.AddCors (opt => opt.AddPolicy ("AllowAll", builder =>
            {
                builder.AllowAnyOrigin ();
                builder.AllowAnyHeader ();
                builder.AllowAnyMethod ();
                builder.AllowCredentials ();
            }));

            services.AddAutoMapper ();

            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen (c =>
            {
                c.SwaggerDoc ("v1", new Info { Title = "Authentication Server", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors ("AllowAll");
            if (env.IsDevelopment ())
            {
                app.UseDeveloperExceptionPage ();
            }
            else
            {
                app.UseHsts ();
            }

            app.UseSwagger ();
            app.UseSwaggerUI (c =>
            {
                c.SwaggerEndpoint ("/swagger/v1/swagger.json", "Authentication Server API V1");
            });

            //app.UseHttpsRedirection ();
            app.UseMvc ();
        }
    }
}