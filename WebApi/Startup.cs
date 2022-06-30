using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using WebApi.Entidades;
using WebApi.Filtros;
using WebApi.Middlewares;
using WebApi.Servicios;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // aqui se configuran los servicios
        public void ConfigurationServices(IServiceCollection services){
            
            // configuracion para evitar data ciclica
            services.AddControllers(opciones => {
                opciones.Filters.Add(typeof(FiltroDeExcepcion));
            }).AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);   

            // inyectamos el dbcontext
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            // ciclos de vida e inyeccion de dependencias

            // cuando una clase requiera un IServicio yo le voy a mandar una instancia de ServicioA
            // que implementa la interfaz Iservicio
            services.AddTransient<IServicio, ServicioA>();
            // aunque no es recomendado, si yo quisiera recibir un dato concreto en mis clases, lo inyectaria de la siguiente forma
            //services.AddTransient<ServicioA>()

            // para ejemplificar el tiempo de vida de los servicios vamos a hacer uno de cada tipo
            services.AddTransient<ServicioTransient>();
            services.AddScoped<ServicioScoped>();
            services.AddSingleton<ServicioSingleton>();

            // inyectando el filtro MiFiltroDeAccion en el sistema de inyeccion de dependencia, el tipo de vida util
            // dependera de la complegidad de la logica del filtro
            services.AddSingleton<MiFiltroDeAccion>();

            // inyectamos nuestro servicio de tarea recurrente pero este tiene un agregado especial
            // lo agregamos al host, aunque tambien se podria agregar de tipo singleton
            services.AddHostedService<EscribirEnArchivo>();

            // inyectamos los servicios de responseCaching  para que puedan ser usados como filtros
            services.AddResponseCaching();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        // aqui se configuran los middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env){
            // esta seria la forma de invocar el middleware creado si no hacemos uso de la extencion static
            //app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
            // al usar este middleware desde este punto vamos a interceptar las repuestas antes de responderle al cliente            
            app.UseLoguearRespuestaHTTP();

            // un ejemplo de un middleware anidado a una ruta especifica
            app.Map("/ruta1", app =>
            {
                // el middleware que simplemente ejecuta una logica tras cumplir la condicion generando una nueva rama    
                app.Run(async contexto =>
                {
                    await contexto.Response.WriteAsync("Estoy interceptando la tubería");
                });
            });
            
            // en envairoment lo evaluamos desde el [IwebHostedEnviroment] ahora
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // este lo agregamos por que no lo trae el program
            app.UseRouting();

            // este es un filtro de cache predefinido por el framework
            // se utiliza en este punto para poder capturar la informacion que se le va a retornar a los usuarios
            // desde los controladores
            app.UseResponseCaching();

            app.UseAuthorization();

            // MapControllers lo remplazamos por UseEndpoints con la siguiente configuracion
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}