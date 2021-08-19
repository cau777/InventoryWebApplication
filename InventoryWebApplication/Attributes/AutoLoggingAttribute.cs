using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace InventoryWebApplication.Attributes
{
    public class AutoLoggingAttribute : TypeFilterAttribute
    {
        public AutoLoggingAttribute() : base(typeof(AutoLoggingImpl))
        {
            
        }
        
        private class AutoLoggingImpl : IActionFilter
        {
            private readonly ILogger<AutoLoggingImpl> _logger;
            public AutoLoggingImpl(ILogger<AutoLoggingImpl> logger)
            {
                _logger = logger;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                _logger.LogInformation($"{context.HttpContext.Request.Method} -> {context.HttpContext.Request.Path}");
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                
            }
        }
    }
}