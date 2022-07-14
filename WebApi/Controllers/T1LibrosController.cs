using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Entidades;
using WebApi.Filtros;
using WebApi.Servicios;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/T1libros")]
    public class T1LibrosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicio servicio;

        // encapsulamos los servicios recibidos en el sistema de inyeccion de dependencias
        // para poderlos usar como propiedades de nuestro controlador
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingleton servicioSingleton;

        // inyectamos servicio como un dato abstracto y lo encapsulamos en un campo
        public T1LibrosController(ApplicationDbContext context, 
            // esta dependencia recibe cualquier clase que implemente la interfaz Iservicio
            // como un dato abstracto, en el startup ya definimos cual clase iba aser  [ServicioA]
            IServicio servicio,
            // recibimos los servicios/dependencias inyectados en el sistea de inyeccion de dependencias
            ServicioTransient servicioTransient, 
            ServicioScoped servicioScoped,
            ServicioSingleton servicioSingleton)
        {
            this.context = context;
            this.servicio = servicio;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSingleton = servicioSingleton;
        }

        // con este metodo ejemplificamos el tiempo de vida de los diferentes tipos de
        // tiempo de vida de los servicios
        [HttpGet("GUID")]
        // podemos ver el ejemplo de cache aqui tambien al momento en el que esta informacion se guarde en cache
        // el servicio trasient que deberia ejecutar una nueva instancia en cada peticion, si la guardamos en cache
        // ninguno de estos se va a actualizar por los proximos 10 segundos cuando la cache se venza y el metodo se ejecute nuevamente
        [ResponseCache(Duration = 10)]
        // [ServiceFilter(typeof(MiFiltroDeAccion))]
        public ActionResult ObtenerGuids()
        {
            return Ok(new
            {                
                // este es el metodo llamado desde la dependencia
                AutoresController_Transient = servicioTransient.Guid,
                // este es el metodo llamado desde la clase [servicioA] 
                // que tambien cuenta con las dependencias
                ServicioA_Transient = servicio.ObtenerTransient(),

                // este es el metodo llamado desde la dependencia
                AutoresController_Scoped = servicioScoped.Guid,
                // este es el metodo llamado desde la clase [servicioA] 
                // que tambien cuenta con las dependencias
                ServicioA_Scoped = servicio.ObtenerScoped(),

                // este es el metodo llamado desde la dependencia
                AutoresController_Singleton = servicioSingleton.Guid,
                // este es el metodo llamado desde la clase [servicioA] 
                // que tambien cuenta con las dependencias
                ServicioA_Singleton = servicio.ObtenerSingleton()
            });

            // con esto que logramos, al momento de correr la aplicacion y ejecutar este metodo
            // vamos a generar multiples instancias de nuestras dependencias inyectadas en este controlador
            // y en el servicioA
            // y con ello podremos evidenciar cuales cambian y cuales no, su ciclo de vida

            // si entramos a sugger y ejecutamos este metodo vemos que
            // las dos primeras lineas siempre son diferentes en la misma peticion

            // las dos segundas son las mismas el la peticion, si hago una nueva peticion 
            // cambian pero seguiran siendo la misma que genere la peticion

            // las dos ultimas son las mismas en la misma peticion, e incluso en peticiones diefrente
            // no cambia hasta que yo pare y qjecute nuevamente la aplicacion
        }


        [HttpGet("{id:int}")]
        // si llega una peticion a una ruta se ejecuta un metodo pero esta info es guardada en cache
        // para posteriores consultas ya no llamamos ejecutamos el metodo nuevamente si no que la traemos del cache
        // se debe tener encuenta el tiempo de duracion del cache
        [ResponseCache(Duration = 10)]
        public ActionResult<Libro> Get(int id)
        {
            // como la relacion es bidireccional un autor puede tener muchos libros y asi mismo un libro pertenece a un autor
            // podemos incluir tambien la data del autor al que hace referencia

            // esta se comenta ya que no podemos tener dos relaxciones al mismo objeto, como en este caso que primero
            // trabajamos con una relacion uno a muchos para autores y libros pero ahora contamos con una muchos a muchoss
            // return await context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);
            return Ok();

        }

        [HttpPost]
        // forma de invocar un filtro personalizado de forma generuicaS
        [ServiceFilter(typeof(MiFiltroDeAccion))]
        public async Task<ActionResult> Post(Libro libro)
        {
            // esta se comenta ya que no podemos tener dos relaxciones al mismo objeto, como en este caso que primero
            // trabajamos con una relacion uno a muchos para autores y libros pero ahora contamos con una muchos a muchoss

            // validamos la existencia de un autor antes de generar una consulta que pueda reventar
            // var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);

            // if (!existeAutor)
            // {
            //     return BadRequest($"No existe el autor de Id: {libro.AutorId}");
            // }

            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}