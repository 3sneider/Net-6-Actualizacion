using WebApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Tests.PruebasUnitarias;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void PrimeraLetraMinusculaRetornaError()
    {
        // preparacion
        var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
        var valor = "felipe";
        var ValContext = new ValidationContext(new { Nombre = valor });

        // ejecucion
        var resultado = primeraLetraMayuscula.GetValidationResult(valor, ValContext);

        // verificacion
        Assert.AreEqual("La primera letra debe ser mayúscula", resultado.ErrorMessage);
    }

    [TestMethod]
    public void ValorNulo_NoDevuelveError()
    {
        // Preparación
        var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
        string valor = null;
        var valContext = new ValidationContext(new { Nombre = valor });

        // Ejecución
        var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);

        // Verificación
        Assert.IsNull(resultado);
    }

    [TestMethod]
    public void ValorConPrimeraLetraMayuscula_NoDevuelveError()
    {
        // Preparación
        var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
        string valor = "Felipe";
        var valContext = new ValidationContext(new { Nombre = valor });

        // Ejecución
        var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);

        // Verificación
        Assert.IsNull(resultado);
    }
}