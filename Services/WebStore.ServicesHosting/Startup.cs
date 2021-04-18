using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Services.Data;

namespace WebStore.ServicesHosting
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection_strng_name = Configuration["ConnectionString"];
            //services.AddDbContext<WebStoreDB>(opt => opt.UseSqlite(Configuration.GetConnectionString("Sqlite")));

            switch (connection_strng_name)
            {
                default: throw new InvalidOperationException($"Подключение {connection_strng_name} не поддерживается");
                case "SqlServer":
                    services.AddDbContext<WebStoreDB>(opt =>
                        opt.UseSqlServer(Configuration.GetConnectionString(connection_strng_name))
                            .UseLazyLoadingProxies()
                    );
                    break;
                case "Sqlite":
                    services.AddDbContext<WebStoreDB>(opt =>
                        opt.UseSqlite(Configuration.GetConnectionString(connection_strng_name), o => o.MigrationsAssembly("WebStore.DAL.Sqlite")));
                    break;
            }

            services.AddDbContext<WebStoreDB>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString(connection_strng_name))
                    .UseLazyLoadingProxies()
            );
            services.AddTransient<WebStoreDbInitializer>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebStore.ServicesHosting", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebStore.ServicesHosting v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
