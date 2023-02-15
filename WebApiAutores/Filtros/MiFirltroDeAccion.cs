using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAutores.Filtros
{
    public class MiFirltroDeAccion : IActionFilter
    {
        private readonly ILogger<MiFirltroDeAccion> _logger;

        public MiFirltroDeAccion(ILogger<MiFirltroDeAccion> logger)
        {
            _logger = logger;

        }



        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Antes de Ejecutar la Accion");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Despues de Ejecutar la Accion");
        }

    }
}
