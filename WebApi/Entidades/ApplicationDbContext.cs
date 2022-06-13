using Microsoft.EntityFrameworkCore;

namespace WebApi.Entidades
{
    public class ApplicationDbContext : DbContext
    {
        // generamos contructor al cual podemos pasarle distintas configuraciones par entity framework
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }


        // configuramos las tablas que vamos a generar a partir de un esquema ya establecido en entidades
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
    }
}