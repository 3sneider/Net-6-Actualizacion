using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Entidades;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/Autores")]
    public class T1AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        // inyecta el contesto que instanciamos en el starup
        public T1AutoresController(ApplicationDbContext context)
        {            
            // lo encapsulamos en una propiedad
            this.context = context;
        }

        // este metodo trae todos los datos de una tabla
        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            // con include podemos agregar data de otras entidades que esten relacionadas
            return await context.Autores.Include(x => x.Libros).ToListAsync();
        }

        // insertamos un autor en una tabla
        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        // editamos un dato de un autor
        [HttpPut("{id:int}")] // api/autores/1 
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if (autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id de la URL");
            }

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        // Eliminamos un dato de una tabla
        [HttpDelete("{id:int}")] 
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
        
    }
}