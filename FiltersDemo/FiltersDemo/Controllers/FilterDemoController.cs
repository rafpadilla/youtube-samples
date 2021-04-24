using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FiltersDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilterDemoController : ControllerBase
    {
        private readonly ILogger<FilterDemoController> _logger;

        public FilterDemoController(ILogger<FilterDemoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            throw new NotImplementedException("this method is not implemented");
        }
    }
}
