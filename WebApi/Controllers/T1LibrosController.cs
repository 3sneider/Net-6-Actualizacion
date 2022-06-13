using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Entidades;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class T1LibrosController: ControllerBase
    {
        private readonly ApplicationDbContext context;

        public T1LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            // como la relacion es bidireccional un autor puede tener muchos libros y asi mismo un libro pertenece a un autor
            // podemos incluir tambien la data del autor al que hace referencia
            return await context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            // validamos la existencia de un autor antes de generar una consulta que pueda reventar
            var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);

            if (!existeAutor)
            {
                return BadRequest($"No existe el autor de Id: {libro.AutorId}");
            }

            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}