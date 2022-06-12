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
            
            services.AddControllers();            
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