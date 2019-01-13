using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Blazor.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Mime;
using LuhisBanking.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LuhisBanking.Server
{
    public class Startup
    {
        private readonly IConfiguration conf;

        public Startup(ILogger<Startup> logger, IConfiguration conf)
        {
            logger.LogInformation($"Starting up LuhisBanking {DateTime.UtcNow}");
            this.conf = conf;
        }

        private static readonly IEnumerable<Action<IServiceCollection>> DIModules = new Action<IServiceCollection>[]
        {
            LuhisBanking.Persistence.DiModule.Add
        };

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            foreach (var action in DIModules)
            {
                action(services);
            }
            services.AddMvc();

            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    MediaTypeNames.Application.Octet,
                    WasmMediaTypeNames.Application.Wasm,
                });
            });
            services.Configure<MyAppSettings>(conf.GetSection("MyAppSettings"));
            services.AddSingleton<ITrueLayerService, TrueLayerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller}/{action}/{id?}");
            });

            app.UseBlazor<Client.Startup>();
        }
    }
}
