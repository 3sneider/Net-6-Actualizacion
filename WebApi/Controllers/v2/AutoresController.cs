using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs;
using WebApi.Entidades;
using WebApi.Utilidades;

// este controlador es una version donde solo manejamos un metodo para el controlador autores, ahora podremos escoger con que version del api trabajar
namespace WebApi.Controllers.v2
{
    [ApiController]
    [Route("api/v2/Autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")] 
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {            
            this.context = context;            
            this.mapper = mapper;
        }

        [HttpGet("{nombre}", Name = "obtenerAutoresv2")]        
        public async Task<ActionResult<List<AutorDTO>>> Get([FromRoute] string nombre)
        {            
            var autores = await context.Autores.Where(autorBD => autorBD.Nombre.Contains(nombre)).ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }        
    }
}