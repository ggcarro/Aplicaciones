using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amigos.DataAccessLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Amigos.wwwroot.lib.signalr.Hub;     // NO DEBERÍA FALLAR ESTO!!!!!!!!!!!!!!!!!!!!!!!1

namespace Amigos
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
            services.AddCors();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR();

            services.AddDbContext<AmigoDBContext>(options => options.UseSqlite("Data Source=Amigos.db"));

            var supportedCultures = new[]
{
                "es-ES",
                "en-US"
            };

            var localizationOptions = new RequestLocalizationOptions();
            localizationOptions.AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures)
                .SetDefaultCulture(supportedCultures[0]);

            // Guardamos estas opciones en el contenedor de dependencias, pues luego las necesitaremos para satisfacer un parámetro del siguiente método y para pedirlo en uno de nuestros controladores, y así extraer la lista de idiomas soportados:
            services.AddSingleton(localizationOptions);

            // Añadimos todos los servicios necesarios para localizar nuestra aplicación al contenedor de inyección de dependencias, y le decimos que nuestros archivos de recursos estarán en la carpeta Resources
            services.AddLocalization(opt => opt.ResourcesPath = "Resources");
            // Añadimos los servicios de MVC, los servicios para localizar las vistas y las Data Anotations.
            services.AddMvc()
                /* con LanguageViewLocationExpanderFormat.Suffix le decimos al motor de localización que el identificador de idioma en un recurso de una vista, estará tras el nombre del mismo , de la manera: home.en.US.resx.
                */
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, RequestLocalizationOptions options)
        {
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRequestLocalization(options);


            /*
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");     //CORREGIR!!!!!!!!!!!!!!!!!!!!
            });
            */

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "Prueba",
                   template: "{controller}/{action}/{valor}/{veces}");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "Amigo",
                   template: "{controller}/{action}/{maxDist}/{lon}/{lat}");
            });
        }
    }
}
