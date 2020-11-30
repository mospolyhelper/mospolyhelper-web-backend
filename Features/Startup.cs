using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Mospolyhelper.DI;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mospolyhelper
{
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object value)
        {
            // Slugify value
            return value == null ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }


    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:1427", "https://mospolyhelper.github.io/")
                       .AllowAnyMethod()
                       .AllowCredentials()
                       .AllowAnyHeader();
            }));
            services.AddControllers(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
            });

            services.AddSwaggerGen(c =>
            {
                //c.OperationFilter<AuthorizeOperationFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "mospolyhelper", Version = "v1" });
                //c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //{
                //    Type = SecuritySchemeType.OAuth2,
                //    Flows = new OpenApiOAuthFlows
                //    {
                //        Implicit = new OpenApiOAuthFlow
                //        {
                //            AuthorizationUrl = new Uri("auth", UriKind.Relative),
                //            Scopes = new Dictionary<string, string>
                //            {
                //                { "readAccess", "Access read operations" },
                //                { "writeAccess", "Access write operations" }
                //            }
                //        }
                //    }
                //});
            });


            //services
            //    .AddAuthentication(options =>
            //    {
            //        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(options =>
            //    {
            //        Configuration.Bind($"{nameof(AppSettings.Security)}:{nameof(AppSettings.Security.Jwt)}", options);
            //    })
            //    .Services
            //    .AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseHttpsRedirection();

            
            //app.UseCors(x => x.WithOrigins("http://localhost:1525").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseSwagger();
            
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;

                //c.OAuthClientId("cleint-id");
                //c.OAuthClientSecret("client-secret");
                //c.OAuthRealm("client-realm");
                //c.OAuthAppName("mospolyhelper API");
                //c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                //c.OAuthUsePkce();
            });
            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("MyPolicy");
            //app.UseAuthentication(); // --
            //app.UseAuthorization(); // --


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //TODO: Move this:
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new ScheduleModule());
            builder.RegisterModule(new AccountModule());
            builder.RegisterModule(new MapModule());
        }
    }
}
