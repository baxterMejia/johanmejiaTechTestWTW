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
    /// Middleware for centralized error and exception handling.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        // Delegate for the next middleware in the pipeline
        private readonly RequestDelegate next;

        /// <summary>
        /// Constructor that receives the next middleware in the pipeline.
        /// </summary>
        /// <param name="next">The next middleware delegate.</param>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Invokes the middleware to handle the incoming HTTP request.
        /// </summary>
        /// <param name="context">The HTTP context of the current request.</param>
        /// <returns>A task that represents the asynchronous execution of the middleware.</returns>
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
        /// Handles exceptions and returns an appropriate HTTP response.
        /// </summary>
        /// <param name="context">The HTTP context of the current request.</param>
        /// <param name="ex">The captured exception.</param>
        /// <returns>A task representing the writing of the HTTP response.</returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;

            CaptureExceptionDTO captureException = new CaptureExceptionDTO
            {
                Timestamp = DateTime.UtcNow,
                Evento = "Error in Middleware",
                Mensaje = ex.Message,
                DatosEvento = ex.StackTrace,
                UsuarioAsociado = context.User?.Identity?.Name ?? "AuthenticationError",
                FechaEjecucion = DateTime.Now
            };

            var result = JsonConvert.SerializeObject(new { error = ex.Message });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
