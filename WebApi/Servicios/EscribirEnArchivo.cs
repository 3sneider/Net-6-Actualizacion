// este servicio  recurrente lo que hara sera ejecutar un log del estado de la aplicacion cada cierto tiempo
namespace WebApi.Servicios
{
    // para tareas recurrentes vamos a implementar la interfaz IHostedService
    public class EscribirEnArchivo : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string nombreArchivo = "Archivo 1.txt";
        private Timer timer;

        // recibimos un IWebHostEnvironment que nos va a permitir acceder al hambiente en el cual nos encontramos
        public EscribirEnArchivo(IWebHostEnvironment env)
        {
            this.env = env;
        }

        // esta funcionalidad se va a ejecutar una vez cuando inciemos nuestro web api
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Escribir("Proceso iniciado");
            return Task.CompletedTask;
        }

        // este metodo se ejecutaria cuando apaguemos nuestra web api
        // cabe anotar que si la aplicacion se interrumpe inesperadamente este metodo
        // puede queno se ejecute
        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            Escribir("Proceso finalizado");
            return Task.CompletedTask;
        }

        // metodo que ejecuta una tarea o manda a realizar una tarea
        private void DoWork(object state)
        {
            Escribir("Proceso en ejecución: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
        }

        // metodo con la logica de escritura de archivo
        private void Escribir(string mensaje)
        {
            // el enviroment nos toma la ruta dle proyecto por ente el folder wwwroot debe ser creado 
            // para poder almacenar esa data estatica de la aplicacion
            var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true))
            {
                writer.WriteLine(mensaje);
            }
        }
    }
}