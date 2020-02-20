using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using System.Net.Http;
using Npgsql;

namespace ContactApi2
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
            services.AddControllers();
            services
                    .AddControllersWithViews()
                    .AddNewtonsoftJson();

            Uri endpoint = new Uri("https://jsonplaceholder.typicode.com/");

            var httpClient = new HttpClient()
            {
                BaseAddress = endpoint,
            };
             var connection = new NpgsqlConnection("Host=127.0.0.1;Username=postgres;Password=docker;Database=postgres");

            // services.AddSingleton<HttpClient>(httpClient);
            services.AddSingleton<NpgsqlConnection>(connection);
            services.AddTransient<IDatabase, Database>();
        
            //services.AddContactContext
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
