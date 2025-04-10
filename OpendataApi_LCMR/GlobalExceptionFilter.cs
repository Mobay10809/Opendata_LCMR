using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace OpendataApi_LCMR
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unhandled Exception");

            var errorResponse = new
            {
                Message = "系統發生未預期錯誤，請稍後再試。",
                // 開發顯示詳細錯誤，正式環境不顯示，未來可以寫進檔案LOG
                Error = _env.IsDevelopment() ? context.Exception.ToString() : null
            };

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = 500
            };
        }
    }

}
