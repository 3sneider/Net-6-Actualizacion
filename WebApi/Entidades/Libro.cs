namespace WebApi.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        // id del autor
        public int AutorId { get; set; }
        // propiedad de navegacion que me indica el id del autor 
        public Autor Autor { get; set; }
    }
}