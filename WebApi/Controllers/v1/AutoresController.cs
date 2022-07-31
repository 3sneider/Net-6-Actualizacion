using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs;
using WebApi.Entidades;
using WebApi.Utilidades;

namespace WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/Autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")] // protegemos un controlador con una politica aplicada
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {            
            this.context = context;
            // inyectamos una intefaz de immaper 
            this.mapper = mapper;
        }

        [HttpGet(Name = "obtenreTodo")] // Metodo para traer datos de una tabla
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // protegemos un endpoint
        // en vez de traer una lista de dtos, puedo traer una colleccion de Recursos par que ya traigan toda su data integrada
        // Task<ActionResult<ColeccionDeRecursos<AutorDTO>>> recordemos que tambien podemos retornar un IactionResult para poder retornar diferrentes tipos de respuesta
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            // para paginar una lista y no traer todos los datos 
            // Como un get no recibe parametros de la queri vamos a tomar los necesarios para la paginacion
            var queryable = context.Autores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var autores = await queryable.OrderBy(autor => autor.Nombre).Paginar(paginacionDTO).ToListAsync();
            // var autores = await context.Autores.ToListAsync();
            // mapeamos el resultado de la consulta con el DTO
            return mapper.Map<List<AutorDTO>>(autores);            

        }

        [HttpGet("{id:int}", Name = "obtenerAutor")]
        // aplicamos el filtro una vez se de respuesta de la peticion agregaremos o no los enlaces segun sela el valor de header
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<AutorDTOConLibros>> Get(int id, [FromHeader] string incluirHATEOAS)
        {
            var autor = await context.Autores
                .Include(autorDB => autorDB.AutoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Libro)
                .FirstOrDefaultAsync(autorBD => autorBD.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            var autorDTO = mapper.Map<AutorDTOConLibros>(autor);            
            return autorDTO;
        }

        [HttpGet("{nombre}", Name = "obtenerAutores")]        
        public async Task<ActionResult<List<AutorDTO>>> Get([FromRoute] string nombre)
        {
            // tenemos una consulta con con una validacion linq
            var autores = await context.Autores.Where(autorBD => autorBD.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);
        }
        
        [HttpPost(Name = "crearAutor")] // metodo Create que toma su info del body de la peticion 
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO) // recibimos un dto para no traer informacion  basurra o sencible
        {
            // primero validamos que la informacion no este repetida o cumpla con nuestras necesidades
            var existeAutorConElMismoNombre = await context.Autores.AnyAsync(x => x.Nombre == autorCreacionDTO.Nombre);

            if (existeAutorConElMismoNombre)
            {
                // si no cumple nuestras necesidades podemos retornar un basRequest
                return BadRequest($"Ya existe un autor con el nombre {autorCreacionDTO.Nombre}");
            }

            // mapeamos a la entidad
            var autor = mapper.Map<Autor>(autorCreacionDTO);

            // si si cumple solo lo agregamos, para ello usamos Add con el cual marcamos un objeto
            // para insertarlo en la base de datos
            context.Add(autor);

            // una vex este marcado el dato lo que hacemos es guardar los cambios efectuados en el contexto
            await context.SaveChangesAsync();

            // y si todo salebien podemos retornar una respuesta Ok
            // return Ok();

            // mapeamos al objeto que vamos a retornar en la creacion de la url
            var autorDTO = mapper.Map<AutorDTO>(autor);
            // las buenas practicas dicen que si yo creo un recurso yo no retorno el recurso so no la url del recurso
            // el nombre de la ruta lo sacamos dedlnombre que le damos al crear el meodo get
            return CreatedAtRoute("obtenerAutor", new { id = autor.Id}, autorDTO);
        }

        [HttpPut("{id:int}")] // api/autores/1 
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id)
        {           
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id ;


            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")] // api/autores/2
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