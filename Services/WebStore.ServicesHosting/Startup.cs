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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Logger;
using WebStore.Services.Data;
using WebStore.Services.Services.InMemory;
using WebStore.Services.Services.InSQL;

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

            services.AddIdentity<User, Role>(/*opt => { }*/)
                .AddEntityFrameworkStores<WebStoreDB>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequiredLength = 3;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredUniqueChars = 3;
#endif

                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

            });

            services.AddTransient<IEmployeesData, InMemoryEmployeesData>();
            services.AddTransient<IProductData, SqlProductData>();
            services.AddTransient<IOrderService, SqlOrderService>();


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebStore.ServiceHosting", Version = "v1" });

                const string webstore_api_xml = "WebStore.ServicesHosting.xml";
                const string webstore_domain_xml = "WebStore.Domain.xml";
                const string debug_path = "bin/debug/net5.0";

                //c.IncludeXmlComments("WebStore.ServiceHosting.xml");
                //c.IncludeXmlComments("WebStore.Domain.xml");

                if (File.Exists(webstore_api_xml))
                    c.IncludeXmlComments(webstore_api_xml);
                else if (File.Exists(Path.Combine(debug_path, webstore_api_xml)))
                    c.IncludeXmlComments(Path.Combine(debug_path, webstore_api_xml));

                if (File.Exists(webstore_domain_xml))
                    c.IncludeXmlComments(webstore_domain_xml);
                else if (File.Exists(Path.Combine(debug_path, webstore_domain_xml)))
                    c.IncludeXmlComments(Path.Combine(debug_path, webstore_domain_xml));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WebStoreDbInitializer db, ILoggerFactory log)
        {
            log.AddLog4Net();

            db.Initialize();

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
