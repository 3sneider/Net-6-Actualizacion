using Microsoft.EntityFrameworkCore;

namespace WebApi.Entidades
{
    public class ApplicationDbContext : DbContext
    {
        // generamos contructor al cual podemos pasarle distintas configuraciones par entity framework
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        // ESTA ES UNA SOBRE ESCRITURA PARA OBJETOS ESPECIALES
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CONSTRUIMOS UN ID COMPUESTO PARA LA ENTIDAD aUTORlIBRO
            modelBuilder.Entity<AutorLibro>()
                .HasKey(al => new { al.AutorId, al.LibroId });

        }


        // configuramos las tablas que vamos a generar a partir de un esquema ya establecido en entidades
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<AutorLibro> AutoresLibros { get; set; }
    }
}