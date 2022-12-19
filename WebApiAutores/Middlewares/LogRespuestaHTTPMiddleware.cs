namespace WebApiAutores.Middlewares
{
    public static class LogRespuestaHTTPMiddlewareExtensions
    {
        /// <summary>
        /// Log Respuesta HTTP Middleware
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseLogRespuestaHTTP(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LogRespuestaHTTPMiddleware>();
        }
    
    }
    public class LogRespuestaHTTPMiddleware
    {
        private readonly RequestDelegate siguiente;
        private readonly ILogger<LogRespuestaHTTPMiddleware> logger;

        public LogRespuestaHTTPMiddleware(RequestDelegate siguiente,
            ILogger<LogRespuestaHTTPMiddleware> logger)
        {
            this.siguiente = siguiente;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext contexto)
        {
            using (var ms = new MemoryStream())
            {
                var cuerpooriginalrespuesta = contexto.Response.Body;
                contexto.Response.Body = ms;

                await siguiente(contexto);

                ms.Seek(0, SeekOrigin.Begin);
                string respuesta = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);

                await ms.CopyToAsync(cuerpooriginalrespuesta);
                contexto.Response.Body = cuerpooriginalrespuesta;
                logger.LogInformation(respuesta);
            }


        }
    }
}
