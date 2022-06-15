namespace WebApi.Entidades
{
    public class Autor
    {
        
        public int Id { get; set; } 

        public string Nombre { get; set; }        

        // variable de navegacion
        public List<Libro> Libros { get; set; }

    }
}