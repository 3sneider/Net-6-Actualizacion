namespace WebApi.Entidades
{
    public class Autor
    {
        
        public int Id { get; set; } 

        public string Nombre { get; set; }     

        // variable de navegacion muchos a muchos
        public List<AutorLibro> AutoresLibros { get; set; }   

        // variable de navegacion
        public List<Libro> Libros { get; set; }

    }
}