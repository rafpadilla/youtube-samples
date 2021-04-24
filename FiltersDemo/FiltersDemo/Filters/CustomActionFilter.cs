using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace FiltersDemo.Filters
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        private readonly ILogger<CustomActionFilter> _logger;

        public CustomActionFilter(ILogger<CustomActionFilter> logger)
        {
            _logger = logger;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = new List<KeyValuePair<string, string>>();
                var modelStateKeys = context.ModelState.Keys.ToArray();
                var modelStateErrors = context.ModelState.Values.SelectMany(a => a.Errors.Select(b => b.ErrorMessage)).ToArray();

                //for (int i = 0; i < context.ModelState.ErrorCount; i++)
                //{
                //    errors.Add(new KeyValuePair<string, string>(modelStateKeys[i], modelStateErrors[i]));
                //}

                context.Result = new BadRequestObjectResult(errors);

                _logger.LogError(new Exception("Validation Error"), JsonSerializer.Serialize(errors));
            }
        }
    }
}