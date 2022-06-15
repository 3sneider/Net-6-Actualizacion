using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Validaciones;

namespace WebApi.Entidades
{
    // esta entidad no la vamos a mapear en el dbcontext por lo que no se va a crear como tabla en la base de datos
    public class testEntity : IValidatableObject
    {
        // para la validacion de modelos podemos agregar decoradores a nuestras propiedades que nos 
        // genera algun tipo de validacion sobre la propiedad
        

        public int Id { get; set; } 

        // con este dataannotation hacemos que estecampo sea obligatorio, adicional podemos personalizar 
        // el mensaje que queramos que se muestre en caso de que incurra en el error
        [Required(ErrorMessage = "El campo {0} es requerido")]
        // no estamos limitados a una unica regla de validacion
        // para este dataannotation podemos establecer el tamaño maximo de una cadena
        [StringLength(maximumLength:12, ErrorMessage = "el campo {0} debe tener un tamaño maximo de {1}")]
        // aqui implementamos nuestra validacion personalizada, al igual que un controlador no vamos a necesitar 
        // el uso de Attribute, es solo para identificarlos
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        // otro dataannotation muy comun es un rango para datos numericos, este tambien puede tener su errormessage
        [Range(18,120)]
        // en caso de que no queramos mapear un dato en nyuestra base de datos podemos usar notmapped 
        // aunque esta propiedad pierde toda su validacion si hacemos uso de DTO's
        [NotMapped]
        public int edad { get; set; }

        // la variedad de dataannotation es grande, incluso podemos ahorrarnos el tiempo para vaidar la estructura
        // de una tarjeta de credito
        [CreditCard]        
        [NotMapped]
        public string creditCard { get; set; }

        // podemos establecer y validar que un campo sea tomado como una url
        [Url]
        [NotMapped]
        public string url { get; set; }

        // variable de navegacion donde decimos que un autor tiene muchos libros
        public List<Libro> Libros { get; set; }
      
        // esta notacion se vuelve innecesaria al habilitar la opcion de permiitir referencias nulas en el csproj
        // public string? Nombre2 { get; set; }

        // existen multiples validaciones pero tambien podemos tener nuestras validaciones personalizadas, estas 
        // las podemos crear dentro del codigo o en clases independientes, por organizacion siempre es bueno tener 
        // todos nuestros objetos separados

        // otro tipo de validacion adicional son las validaciones por modelo, este tipo de validacion genera un
        // codigo dentro de la misma entidad, se hace en caso de que sea una validacion muy puntual de la entidad
        // que estamos trabajando, paar ello implementamos la interfaz IValidatableObject el cual nos genera 
        // el mismo metodo isValid que no otorga la clase validation en las validaciones personalizadas aisladas

        // para poder aplicar las validaciones por modelo debemos obisr las validaciones de dattaanotation

         public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // en este caso podemos trabajar la misma logica de nuestra validacion de letra mayuscula
            // con la unica excepcion de que ahora esto va a ser aplicable unicamente para esta entidad
            // ya no recibimos un value como parametro si no  que trabajamos directamente con las 
            // propiedades de nuestra entidad
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {

                    // sumamos las excepciones a la cola de respuesta
                    yield return new ValidationResult("La primera letra debe ser mayúscula", 
                            new string[] { nameof(Nombre) });
                }
            }

            // podemoa trabajar mas de una validacion, lirteralmente podemos validar todas las propiedades
            // de nuestra entidad

            //if (Menor > Mayor)
            //{
            //    yield return new ValidationResult("Este valor no puede ser más grande que el campo Mayor",
            //        new string[] { nameof(Menor) });
            //}
        }

        // existe una ultima forma de validacion que son a nivel de controladores pero ya son mas de logica, 
        // si exoste algo o si cumple cierta condicion haga una accion
    }
}