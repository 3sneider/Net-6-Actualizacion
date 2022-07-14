using WebApi.Entidades;

namespace WebApi.Entidades
{
    // clase intermedia para la generacion de relacion muchos a muchos
    public class AutorLibro
    {
        // referencia a tabla a
        public int LibroId { get; set; }

        // referencia a tabla b
        public int AutorId { get; set; }
        public int Orden { get; set; }
        // variables de navegagcion a los objetos de referencia
        public Libro Libro { get; set; }
        public Autor Autor { get; set; }
    }
}