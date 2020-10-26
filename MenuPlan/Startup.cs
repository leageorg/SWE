using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MenuPlan.Model;
using MenuPlan.Infrastruktur;



namespace MenuPlan
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

      
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.Configure<MenuplanDatabaseSettings>(
                Configuration.GetSection(nameof(MenuplanDatabaseSettings)));

            services.AddSingleton<IMenuplanDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<MenuplanDatabaseSettings>>().Value);

            services.AddSingleton<MenuPlanCRUDService>();
            
            services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());
           
        }

       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime, ILogger<Startup> loggerIn)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/errordev");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            applicationLifetime.ApplicationStopping.Register(
                () =>
                {
                    loggerIn.LogInformation($"Das Service wird heruntergefahren");
                }                
            );

            app.Use(async (context, next) =>
            {
                try
                {
                    await next().ConfigureAwait(false);
                }
                catch (ServiceException e)
                {
                    loggerIn.LogError($"Ernsthafter Fehler aufgetreten {e.InnerException.Message}");
                    loggerIn.LogError($"Das Service wird beendet");
                    applicationLifetime.StopApplication();
                }
                catch (Exception e)
                {
                    loggerIn.LogError($"Unbehandelter Fehler aufgetreten\n{e.Message}");
                }
            });



            app.UsePathBase("/");
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
