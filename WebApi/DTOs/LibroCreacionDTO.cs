using System.ComponentModel.DataAnnotations;
using WebApi.Validaciones;

namespace WebApi.DTOs
{
    public class LibroCreacionDTO
    {
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 250)]
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        // le asgino los id de los autores del libero
        public List<int> AutoresIds { get; set; }
    }
}