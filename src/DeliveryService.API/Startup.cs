using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.DAL.Contexts;
using DeliveryService.DAL.Models;
using DeliveryService.DL.Repositories;
using DeliveryService.DL.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;
using DeliveryService.DL.Infrastructure;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using DeliveryService.DL.Helpers;

namespace DeliveryService.API
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton(new DataContext(Configuration["ConnectionStrings:Main:URI"],
                Configuration["ConnectionStrings:Main:User"], Configuration["ConnectionStrings:Main:Pass"]));

            services.AddSingleton(new RouteSetup(Configuration["RouteSetup:MinHops"], Configuration["RouteSetup:MaxHops"]));
            services.AddTransient<IErrorHandler, ErrorHandler>();
            services.AddTransient<IBaseRepository<Warehouse>, BaseRepository<Warehouse>>();
            services.AddTransient<IBaseService<Warehouse>, BaseService<Warehouse>>();
            services.AddTransient<IWarehouseService, WarehouseService>();
            services.AddTransient<IRouteService, RouteService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(
              options =>
              {
                  options.Run(
                  async context =>
                  {
                      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                      context.Response.ContentType = "text/html";
                      var ex = context.Features.Get<IExceptionHandlerFeature>();
                      if (ex != null)
                      {
                          var err = $"<h1>Error: {ex.Error.Message}</h1>";
                          await context.Response.WriteAsync(err).ConfigureAwait(false);
                      }
                  });
              });
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            

        }
    }
}
