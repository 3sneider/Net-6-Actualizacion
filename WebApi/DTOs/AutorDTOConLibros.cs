namespace WebApi.DTOs
{
    // hereda de Libro DTO para tener la relacion aislada en caso de que los libros sean requeridos
    public class AutorDTOConLibros: AutorDTO
    {
        public List<LibroDTO> Libros { get; set; }
    }
}