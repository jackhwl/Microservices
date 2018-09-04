using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Globoma.Services;

namespace Globoma
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //services.AddSingleton<IConferenceService, ConferenceApiService>();
            services.AddSingleton<IProposalService, ProposalApiService>();
            services.AddHttpClient("GlobomanticsApi", c =>
                c.BaseAddress = new Uri("http://localhost:5000"));
            services.AddHttpClient<IConferenceService, ConferenceApiService>();

            services.Configure<GlobomanticsOptions>(configuration.GetSection("Globomantics"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Conference}/{action=Index}/{id?}");
            });
            //app.Use(async (context, next) =>
            //{
            //    //await context.Response.WriteAsync("Hello World!");
            //    logger.LogInformation("Before second");
            //    await next();
            //    logger.LogInformation("After second");
            //});

            //app.Run(async (context) =>
            //{
            //    logger.LogInformation("second");
            //    await context.Response.WriteAsync("Second text!");
            //});
        }
    }
}
