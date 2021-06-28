using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Turnos.Models;

namespace Turnos
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
            services.AddSession(options => 
            {
                options.IdleTimeout = TimeSpan.FromSeconds(300); //la duración de la sesión será 5min, si en este tiempo no se registra actividad, se cierra
                options.Cookie.HttpOnly = true;//almacenará la cookie en el navegador unicamente
                /*Middleware para el manejo de sesiones
                atraves de la función lambda establecemos las propiedades requeridas*/
            });
            services.AddControllersWithViews(options => 
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())
            ); //Middleware de MVC, acá irá la validación de tokens automática para cada endpoint
            //Startup es nuestro contenedor, EntityFramework nos permite hacer inyecciones a nuestro contenedor
            //Startup es la que dispara todos los servicios para iniciar la app, es como el main de la app
            /*El método AddDbContext necesita dos partes para funcionar,
            una de ellas es el tipo de contexto (TurnosContext), además, necesita el motor de DB
            en este caso, le decimos que usará SQL Server, en este caso definimos a opciones como parámetro
            y se le asigna UseSqlServer*/ 
            
            services.AddDbContext<TurnosContext>(opciones => opciones.UseSqlServer(Configuration.GetConnectionString("TurnosContext")));
            /*Accedemos con el objeto configuration a  nuestro archivo .json, con el método GetConnectionString, obtenemos la propiedad
            turnos context (SE ENCUENTRA EN appsettings.json)*/


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if(env.IsProduction())//si entra por acá, es porque estamos en ambiente de producción
            {
                app.UseExceptionHandler("/Home/Error");//middleware para mostrar al usuario el error en un entorno de producción
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
