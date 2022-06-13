namespace WebApi.Entidades
{
    public class Autor
    {
        
        public int Id { get; set; } 

        public string Nombre { get; set; }

        // variable de navegacion donde decimos que un autor tiene muchos libros
        public List<Libro> Libros { get; set; }

        // esta notacion se vuelve innecesaria al habilitar la opcion de permiitir referencias nulas en el csproj
        // public string? Nombre2 { get; set; }

    }
}