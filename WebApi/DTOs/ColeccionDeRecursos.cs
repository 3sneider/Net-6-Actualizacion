namespace WebApi.DTOs
{
    // clase generia para la generacion de hateous
    public class ColeccionDeRecursos<T>: Recurso where T: Recurso
    {
        public List<T> Valores { get; set; }
    }
}