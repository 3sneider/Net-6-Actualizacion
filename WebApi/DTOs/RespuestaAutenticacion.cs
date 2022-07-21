namespace WebApi.DTOs
{
    public class RespuestaAutenticacion
    {
        // token de autenticacion
        public string Token { get; set; }
        // expiracion del token
        public DateTime Expiracion { get; set; }
    }
}