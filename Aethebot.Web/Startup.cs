using Aethebot.Worker;
using Aethebot.Worker.Commands;
using Discord.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Aethebot.Web
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
            if (string.IsNullOrWhiteSpace(Configuration.GetValue("DISCORD_TOKEN", string.Empty)))
            {
                throw new InvalidOperationException("Missing DISCORD_TOKEN! Did you forget to read the README?");
            }

            if (Configuration.GetValue<bool>("RunWebsite"))
            {
                services.AddControllers();
            }

            if (Configuration.GetValue<bool>("RunBot"))
            {
                services.AddHostedService<WorkerService>();
            }

            services.AddControllersWithViews();
            services.AddHttpClient("reddit");

            services.AddSingleton(new PingModule());
#pragma warning disable CA2000 // Dispose objects before losing scope
            services.AddSingleton(new CommandService());
#pragma warning restore CA2000 // Dispose objects before losing scope
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
