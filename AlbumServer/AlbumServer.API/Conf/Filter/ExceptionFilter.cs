using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AlbumServer.API.Conf.Filter
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Error in API");

            var errorResult = new
            {
                Message = context.Exception.Message,
                DevMessage = context.Exception.StackTrace,
            };

            var jsonResult = new JsonResult(errorResult)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.Result = jsonResult;
        }
    }
}
