using Microsoft.AspNetCore.Identity;

namespace WebApi.Entidades
{
    public class Comentario
    {
         public int Id { get; set; }
        public string Contenido { get; set; }
        // referencia al objeto del que depende
        public int LibroId { get; set; }
        // variable de navegacion al objeto que referencia
        public Libro Libro { get; set; }

        // generamos una relacion con el usuario que hagrego un comentario
        public string UsuarioId { get; set; }
        public IdentityUser usuario { get; set; }
        
    }   
}