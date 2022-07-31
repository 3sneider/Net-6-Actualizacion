using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs;
using WebApi.Entidades;

namespace WebApi.Controllers.v1
{
    // como esta es una clase dependiente de la clase libro, osea, si noexiste un libro
    // no eiste un comentario sobre ese lobro, para ello usamos esta nomenclatura de ruta
    [ApiController]
    [Route("api/v1/libros/{libroId:int}/comentarios")]
    public class ComentariosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> userManager;

        public ComentariosController(ApplicationDbContext context,
            IMapper mapper, IConfiguration configuration,
            UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId)
        {
            var existeLibro = await context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);

            if (!existeLibro)
            {
                return NotFound();
            }

            var comentarios = await context.Comentarios
                .Where(comentarioDB => comentarioDB.LibroId == libroId).ToListAsync();

            return mapper.Map<List<ComentarioDTO>>(comentarios);
        }

        [HttpGet("{id:int}", Name = "ObtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> GetPorId(int id)
        {
            var comentario = await context.Comentarios.FirstOrDefaultAsync(comentarioDB => comentarioDB.Id == id);

            if (comentario == null)
            {
                return NotFound();
            }

            return mapper.Map<ComentarioDTO>(comentario);
        }
 
        // metodo para la creacion de un comentario
        [HttpPost]
        // protegemos un endpoint para que los comenatrios soloo se puedan hacer por usuarios logueados
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {

            // como esta pasando por el sistema de autenticacin ahora tenemos acceso a los claims para complementar dadta, como en este caso el usuario quien hizo el comentario
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;

            // como ya tenemos el valor del email con el que el usuario se loggueo entonces ahora vamos a traer la data del usuario por medio de el email
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;           


            // validamos la existencia de el libro
            var existeLibro = await context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);

            if (!existeLibro)
            {
                // 404
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            comentario.UsuarioId = usuarioId;
            context.Add(comentario);
            await context.SaveChangesAsync();

            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);

            return CreatedAtRoute("ObtenerComentario", new { id = comentario.Id, libroId = libroId }, comentarioDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int libroId, int id, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var existeLibro = await context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);

            if (!existeLibro)
            {
                return NotFound();
            }

            var existeComentario = await context.Comentarios.AnyAsync(comentarioDB => comentarioDB.Id == id);

            if (!existeComentario)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.Id = id;
            comentario.LibroId = libroId;
            context.Update(comentario);
            await context.SaveChangesAsync();
            return NoContent();
        }
    
        // metodo para traer datos mediante el proveedor de configuraciones, para ello inyectamos
        // el iconfiguration el cual ya viene configurado 
        [HttpGet("configuraciones")]
        public ActionResult<string> ObtenerConfiguracion()
        {
            // existe una funcion especializada en jalar los datos de la ccadena de coneccion
            // Configuration.GetConnectionString("defaultConnecrtion")

            // para acceder a propiedades que estan dentro de propiedades lo unico que hacemos es 
            // bajar un nivel con : ejemplo [Configuration["propiedad:propiedadhija"]]

            // una ves inyectada la interfaz podemos conectar conlas propiedades del configuration
            return configuration["nombre"];
        }
    
    }
}