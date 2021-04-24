using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;

namespace FiltersDemo.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;
        private readonly IWebHostEnvironment _env;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }
        public void OnException(ExceptionContext context)
        {
            BuildResponse(context, HttpStatusCode.InternalServerError);
        }
        private void BuildResponse(ExceptionContext context, HttpStatusCode statusCode, string errorCode = "UnhandledError")
        {
            var trace = Guid.NewGuid();
            var listErrors = new List<KeyValuePair<string, string>>();
            var ex = context.Exception.GetBaseException().Message;

            listErrors.Add(new KeyValuePair<string, string>(errorCode, ex));

            if (_env.IsDevelopment())
            {
                var valuePairStackTrace = new KeyValuePair<string, string>("StackTrace", context.Exception.StackTrace);
                listErrors.Add(valuePairStackTrace);
            }

            context.Result = new ObjectResult(new { Trace = trace, Errors = listErrors });
            context.HttpContext.Response.StatusCode = (int)statusCode;

            _logger.LogError(ex, $"Trace->{trace}");
        }
    }
}
