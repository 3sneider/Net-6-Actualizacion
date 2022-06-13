using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using WebApi.Entidades;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigurationServices(IServiceCollection services){
            
            // configuracion para evitar data ciclica
            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);   

            // inyectamos el dbcontext
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env){
            
            // en envairoment lo evaluamos desde el [IwebHostedEnviroment] ahora
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // este lo agregamos por que no lo trae el program
            app.UseRouting();

            app.UseAuthorization();

            // MapControllers lo remplazamos por UseEndpoints con la siguiente configuracion
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}