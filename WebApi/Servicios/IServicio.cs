namespace WebApi.Servicios
{
    // interfaz a implementar
    public interface IServicio
    {
        Guid ObtenerScoped();
        Guid ObtenerSingleton();
        Guid ObtenerTransient();
        void RealizarTarea();
    }

    // clase que implementa la interfaz 
    public class ServicioA : IServicio
    {
        // encapsulamos como un campo el servicio de logger
        private readonly ILogger<ServicioA> logger;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingleton servicioSingleton;

        // aqui aparte de inyectar los servicios de ejemplo de inyeccion de dependencias, tambien inyectamos el servicioLogger
        // el cual nos permitira capturar los diferentes logs que nos arroje la aplicacion
        public ServicioA(ILogger<ServicioA> logger, ServicioTransient servicioTransient,
            ServicioScoped servicioScoped, ServicioSingleton servicioSingleton)
        {
            this.logger = logger;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSingleton = servicioSingleton;
        }

        public Guid ObtenerTransient() { 
            // de esta menera podemos tener control sobre todos los tipos de log que nos ofrece Ilogger
            logger.LogInformation("Mensaje que ofrece una informacion");

            
            return servicioTransient.Guid; 
        }

        public Guid ObtenerScoped() { return servicioScoped.Guid; }
        public Guid ObtenerSingleton() { return servicioSingleton.Guid; }

        public void RealizarTarea()
        {
        }
    }

    // clase que implementa la interfaz
    public class ServicioB : IServicio
    {
        public Guid ObtenerScoped()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerSingleton()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerTransient()
        {
            throw new NotImplementedException();
        }

        public void RealizarTarea()
        {
        }
    }

    // clases que deceamos inyectar
    #region metodos

    // aqui contamos con tres clases diferentes que podemos inyectar en nuestras clases
    // para este propocito vamos a inyecttarlos en el sistema de inyeccion de dependencias
    

    // este sera un servicio el cual quiero que se genere un anueva instancia cada 
    // vez que una clase haga uso de ella
    public class ServicioTransient
    {
        public Guid Guid = Guid.NewGuid();
    }

    // este sera un servicio que me generara una instancia por peticion http, es la 
    // misma instancia para todo el proceso de un apeticion
    public class ServicioScoped
    {
        public Guid Guid = Guid.NewGuid();
    }

    // este sera un servicio que me generara una peticion con un tiempo de vida 
    // permanente el cual no cambiara a menos de que nosotros intervengamos de alguna manera
    // como generar un nuevo despliegue o reiniciar la instancia
    public class ServicioSingleton
    {
        public Guid Guid = Guid.NewGuid();
    }

    #endregion
}

// cuando yo recibo una clase como un parametro de un constructor estoy hablando de que estoy 
// inyectando dicha funcionalidad, esto se conoce como inyeccion de dependencias, ahora, 
// si yo no recibo en el constructor una clase si no que contrario a eso recibo una interfaz como 
// parametro entonces en el momento que yo haga la inyeccion en la clase startup me daria la flexibilidad 
// de inyectar cualquier clase que implemente la interfaz que recibe el constructor
// a esto tambien se le llama inyeccion de dependencias solo que en este caso puntual me ofrece una mayor 
// flexibilidad, esto con el fin de poder inyectar diferentes objetos sin afectar la funcionalidad del quien 
// lo implementa.

// para ejemplificar podemos ver el como en la clase servicioA recibimos como parametros 
// tres clases diferentes cada una con sus metodos diferentes, estas clases se encapsulan en un parametro 
// permitiendono asi tener control sobre los metodos de cada una de las clases

// aqui tenemos que tener encuenta uno de los principios solid, inyeccion de dependencias, que nos dice que nuestras
// clases deberian depender de objetos abstractos y no concretos, un objeto abstracto seria una interfaz, 
// y un objeto concreto seria una clase, por que razon, si la funcionalidad en mi clase cambia esto afectaria 
// todo el funcionamiento de nuestra aplicaion, mientras que si yo dependo de una interfaz, no importa si la clase 
// tiene cambios en su logica ya que la interfaz nos define los metodos con sus parametros y salida, la logica ya 
// dependera netamente de la clase que implemente la interfaz, yo lo unico que requier es una clase que 
// implemente la interfaz y me genere los metodos con los parametros y respuestas que se definieron en la interfaz
// esto me da la flexibilidad de usar cual sea la clase que implemente mi interfaz.


