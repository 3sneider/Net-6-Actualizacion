using System.ComponentModel.DataAnnotations;

namespace WebApi.Validaciones
{
    // de la forma en la que un controlador lo identificamos por la palabra Conrtroller, los atributos tambien
    // son identificados por tener la palabra attribute, ademas de que deben heredar de la clase ValidationAttribute
    public class PrimeraLetraMayusculaAttribute: ValidationAttribute
    {
        // sobreescribimnos el metodo de la clase ValidatioAttribute
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // inicialmente validamos si el campo es nuli, esto para saber si operamos o no
            // no agregamos mayor logica por que para ello ya contamos con una validacion Required
            // la cual valida si el camopo es nulo o no
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            // aplicamos la logica que queremos que tenga nuestra validacion
            var primeraLetra = value.ToString()[0].ToString();

            if (primeraLetra != primeraLetra.ToUpper())
            {
                // en caso de que la validacion no se cumpla indicamos el error por defectoque queremos manejar
                // en la validacion
                return new ValidationResult("La primera letra debe ser mayúscula");
            }

            // si la validacion se cumple la damos por correcta
            return ValidationResult.Success;

            //terminada la validacion la podemos implementar como un dataannotation en las propiedades de mi entidad
        }
    }
}