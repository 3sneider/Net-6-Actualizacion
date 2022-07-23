using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.Entidades;
using WebApi.Filtros;
using WebApi.Middlewares;
using WebApi.Servicios;
using WebApi.Utilidades;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // con esta configuracion limpiamos los cime
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // aqui se configuran los servicios
        public void ConfigurationServices(IServiceCollection services){
            
            // configuracion para evitar data ciclica
            services.AddControllers(opciones => {
                opciones.Filters.Add(typeof(FiltroDeExcepcion));
            }).AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();   // se jace la insercion de jsonpach 

            // inyectamos el dbcontext
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            // necesario para agregar un sistema de seguridad por jwtoken y configuracion del mismo
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opciones => opciones.TokenValidationParameters = new TokenValidationParameters { 
                  ValidateIssuer = false,
                  ValidateAudience = false,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(
                      Encoding.UTF8.GetBytes(Configuration["llavejwt"])),
                  ClockSkew = TimeSpan.Zero
                });


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
            // services.AddSingleton<MiFiltroDeAccion>(); // lo comentamos para que no nos siga llenando el log 

            // inyectamos nuestro servicio de tarea recurrente pero este tiene un agregado especial
            // lo agregamos al host, aunque tambien se podria agregar de tipo singleton
            // services.AddHostedService<EscribirEnArchivo>(); comentamos para que no siga escribiendo en el archivo plano

            // inyectamos los servicios de responseCaching  para que puedan ser usados como filtros
            services.AddResponseCaching();

            services.AddEndpointsApiExplorer();

            // configuracion de swagger para tener sistema de seguridad 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIAutores", Version = "v1" });
                // esto es para agregar los parametros en el swagger
                c.OperationFilter<AgregarParametroHATEOAS>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });

            });

            // servicio de automapper
            services.AddAutoMapper(typeof(Startup));

            // servicios necesarios para agregar identityframework para autenticacion y autorizacion

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // configuracion de claims y permisos con politicas de seguridad
            services.AddAuthorization(opciones =>
            {
                // en esta politica estamos diciendo que un usuario necesita una claim esAdmin para continuar con un endpoint
                opciones.AddPolicy("EsAdmin", politica => politica.RequireClaim("esAdmin"));
                opciones.AddPolicy("EsVendedor", politica => politica.RequireClaim("esVendedor"));
            });

            // servicio de proteccion de datos
            services.AddDataProtection();

            // agregamos el servicio de hash que creamos
            services.AddTransient<HashService>();

            // configuracion par evitar el problema de cors
            services.AddCors(opciones =>
            {
                opciones.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://www.apirequest.io").AllowAnyMethod().AllowAnyHeader(); // .WithExposedHeaders para exponer headers
                });
            });


            // agregamos los servicios para el generador de HATEOAS
            services.AddTransient<GeneradorEnlaces>();
            services.AddTransient<HATEOASFiltroAttribute>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
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

            // redirecciona las peticiones https
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