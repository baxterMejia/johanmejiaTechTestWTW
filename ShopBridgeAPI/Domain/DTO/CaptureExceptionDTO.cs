
namespace Domain.DTO
{
    public class CaptureExceptionDTO
    {
        public DateTime Timestamp { get; set; }
        public string Evento { get; set; }
        public string Mensaje { get; set; }
        public string DatosEvento { get; set; }
        public string UsuarioAsociado { get; set; }
        public DateTime FechaEjecucion { get; set; }
    }
}
