using Domain.DTO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    /// <summary>
    /// Middleware para manejar errores y excepciones de manera centralizada.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        // Delegado para el siguiente middleware en el pipeline
        private readonly RequestDelegate next;

        /// <summary>
        /// Constructor que toma el siguiente middleware en el pipeline
        /// </summary>
        /// <param name="next">El siguiente middleware</param>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Método de invocación del middleware para manejar peticiones HTTP.
        /// </summary>
        /// <param name="context">El contexto HTTP de la petición</param>
        /// <returns>Una tarea que representa la ejecución asíncrona del middleware</returns>
        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {               
                await next(context);
            }
            catch (Exception ex)
            {                
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Maneja las excepciones y devuelve una respuesta HTTP adecuada.
        /// </summary>
        /// <param name="context">El contexto HTTP de la petición</param>
        /// <param name="ex">La excepción capturada</param>
        /// <returns>Una tarea que representa la escritura de la respuesta HTTP</returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
           
            var code = HttpStatusCode.InternalServerError;

         
            CaptureExceptionDTO captureException = new CaptureExceptionDTO
            {
                Timestamp = DateTime.UtcNow,
                Evento = "Error en Middleware",
                Mensaje = ex.Message,
                DatosEvento = ex.StackTrace,
                UsuarioAsociado = context.User?.Identity?.Name ?? "ErrorAutenticacion",  
                FechaEjecucion = DateTime.Now
            };
            
            var result = JsonConvert.SerializeObject(new { error = ex.Message });
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            
            return context.Response.WriteAsync(result);
        }
    }
}
