using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Entidades;

namespace WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/T1Autores")] 
    // api/[Controller] esta regla de ruteo se aplica en el remoto caso de que el nombre del 
    // controlador pueda llegar a cambiar en el futuro cercano
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
        // podemos tener mas de una ruta para el mismo endpoin
        [HttpGet("todos")]
        // podemos hacer la ruta absoluta para obiar la parte princiapal de la ruta establecida por el controlador
        [HttpGet("/todos")]
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
        // las regals de ruteo son las que me permite asociar un endpoint a un metodo
        // las variables de ruta nos permiten especificar un parametro al metodo
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
        // la diferencia entre pasar  variable de ruta o no hacerlo es la forma en la que se crea la ruta
        [HttpDelete] // sin variable seria // api/autores?id=1
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

        // para retornar datos debemos tener encuenta lo siguiente
        // primero se puede retornar el dato puro que queremos reguresar, pero me limita a las respuestas http
        // osea podemos retornar un Autor pero no podiramos retornar el satus 200
        // pero un action result me permite retornar el objeto ademas de el estado
        // una opcion adicional es retornar la interfaz de action result pero esto nos obliga a retornar el objeto
        // dentro de una respuesta hhttp, ok(autor)

        // es recomendable que los endpoints sean de tipo async, esto por que cuando usamos bases de atos
        // el servidor de bases de datos puede tardar mas de lo requerido, entonces para que la aplicacion 
        // pueda ser mas eficiente la programacion asyncrona me permitira procesar otras tareas mientras e
        // esperamos la respuesta del otro servidor, una peticion asyncrona debe contar con dos
        // paramertos oblligados, async y await
        
    }
}