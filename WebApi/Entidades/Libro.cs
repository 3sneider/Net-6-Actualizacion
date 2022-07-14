using System.ComponentModel.DataAnnotations;

namespace WebApi.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }

        // variable de navegacion  de la relacion uno a mñuchos donde un libro puede tener muchos comentarios
        public List<Comentario> Comentarios { get; set; }
        // variable de navegacion muchos a muchos
        public List<AutorLibro> AutoresLibros { get; set; }


        // en caso de una relacion uno a muchos entre libro yy autores donde un autor puede tener muchos libros
        // id del autor
        // public int AutorId { get; set; }
        // propiedad de navegacion que me indica el id del autor 
        // public Autor Autor { get; set; }
    }
}