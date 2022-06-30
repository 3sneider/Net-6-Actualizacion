using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filtros
{
    // interfaz que debe implementar para ser considerado un filtro
    public class MiFiltroDeAccion : IActionFilter
    {
        private readonly ILogger<MiFiltroDeAccion> logger;

        // inyectamos logguer para tener una funcion basica de nuestro filtro
        public MiFiltroDeAccion(ILogger<MiFiltroDeAccion> logger)
        {
            this.logger = logger;
        }

        // este metodo se ejecutar antes de ejecutar la accion
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Antes de ejecutar la acción");
        }

        // este filtro se ejecutara despues de eejcutada la accion
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Después de ejecutar la acción");

        }
    }
}