using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Redis.Commander.Blazor.Shared;
using Redis.Commander.Blazor.Shared.State;
using Redis.Commander.Client;
using Redis.Commander.Client.Contracts;
using Redis.Commander.Data;
using Redis.Commander.Data.Contracts;

namespace Redis.Commander.Blazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddSingleton<IConnectionRepository, ConnectionRepository>();
            services.AddSingleton<ICommandRepository, CommandRepository>();
            services.AddSingleton<IRedisClient, RedisClient>();

            services.AddSingleton<AppState>();
            services.Configure<DbOptions>(Configuration.GetSection(nameof(DbOptions)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            // Before opening any window, generate and populate the SQLite database
            Task.Run(async () =>
            {
                var dbConfigInstance = new DbOptions();
                Configuration.GetSection(nameof(DbOptions)).Bind(dbConfigInstance);
                var dbOptions = Options.Create<DbOptions>(dbConfigInstance);

                var bootstrapper = new DataBootstrapper(dbOptions);
                await bootstrapper.BootstrapAsync();
            })
            .Wait();

            if (HybridSupport.IsElectronActive)
            {
                ElectronBootstrap();
            }
        }

        public async void ElectronBootstrap()
        {
            var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = 1920,
                Height = 1080,
                Show = false
            });

            await browserWindow.WebContents.Session.ClearCacheAsync();

            browserWindow.OnReadyToShow += () => browserWindow.Show();
            browserWindow.SetTitle("Redis Commander");
        }
    }
}
