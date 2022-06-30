namespace WebApi.Middlewares
{
    // este es un metodo de extencion el cual me permite hacer que esta clase
    // y su logica se puedan implementar como un middleware dentro de configure en startup
    public static class LoguearRespuestaHTTPMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoguearRespuestaHTTP(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
        }
    }


    public class LoguearRespuestaHTTPMiddleware
    {
        // a traves de este delegate podemos indicar los middleware que queremos invocar
        private readonly RequestDelegate siguiente;
        // inyectamos logger para poder imprimir los logs y poder ver la imformacion que capturamos
        private readonly ILogger<LoguearRespuestaHTTPMiddleware> logger;

        public LoguearRespuestaHTTPMiddleware(RequestDelegate siguiente, 
            ILogger<LoguearRespuestaHTTPMiddleware> logger)
        {
            this.siguiente = siguiente;
            this.logger = logger;
        }

        // Invoke o InvokeAsync
        // esta clase es inresindible para eque la clase sea interpretada como un middleware
        public async Task InvokeAsync(HttpContext contexto)
        {
            //jpor medio del memory sting vamos a imprimir la informacion que recibimos
            using (var ms = new MemoryStream())
            {
                var cuerpoOriginalRespuesta = contexto.Response.Body;
                contexto.Response.Body = ms;

                await siguiente(contexto);

                ms.Seek(0, SeekOrigin.Begin);
                string respuesta = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);

                await ms.CopyToAsync(cuerpoOriginalRespuesta);
                contexto.Response.Body = cuerpoOriginalRespuesta;

                logger.LogInformation(respuesta);
            }
        }
    }
}