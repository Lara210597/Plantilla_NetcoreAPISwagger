using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Plantilla.NetCoreSwaggerBasicAuth.API;

namespace Plantilla.NetCoreSwaggerBasicAuth.API
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            services.AddVersionedApiExplorer(options =>
            {                
                options.GroupNameFormat = string.Empty;
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

         
            services.AddSwaggerGen(options =>
            {
                const string url_exceltec = "https://www.excelteccr.com/";

                options.EnableAnnotations();

                options.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = $"API - Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}",
                    Description = $" {Environment.NewLine} {Environment.NewLine} ",
                    TermsOfService = new Uri(url_exceltec),
                    Contact = new OpenApiContact()
                    {
                        Name = "TI",
                        Email = "",
                        Url = new Uri(url_exceltec)
                    },
                    License = new OpenApiLicense
                    {
                        Name = "©",
                        Url = new Uri(url_exceltec)
                    },
                });

                using (var serviceProvider = services.BuildServiceProvider())
                {
                    var provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
                    String assemblyDescription = typeof(Startup).Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo()
                        {
                            Title = $"{typeof(Startup).Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product} {description.ApiVersion}",
                            Version = description.ApiVersion.ToString(),
                            Description = description.IsDeprecated ? $"{assemblyDescription} - DEPRECATED" : $"{assemblyDescription}"
                        });
                    }
                }

                options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Authorization"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });

            });

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddScoped<IUserService, UserService>();
            services.AddControllers();
            services.AddRouting();

            IoC.IoC_Injection(services);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();

            app.UseSwagger();
            var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
