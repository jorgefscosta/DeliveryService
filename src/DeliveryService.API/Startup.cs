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

namespace DeliveryService.API
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
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton(new DataContext(Configuration["ConnectionStrings:Main:URI"], 
                Configuration["ConnectionStrings:Main:User"], Configuration["ConnectionStrings:Main:Pass"]));

            services.AddTransient<IBaseRepository<Warehouse>, BaseRepository<Warehouse>>();
            services.AddTransient<IBaseService<Warehouse>, BaseService<Warehouse>>();
            services.AddTransient<IWarehouseService, WarehouseService>();
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
