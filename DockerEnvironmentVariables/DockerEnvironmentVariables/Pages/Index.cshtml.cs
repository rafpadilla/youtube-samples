using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DockerEnvironmentVariables.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public List<EnvVariable> EnvironmentVariables { get; set; } = new List<EnvVariable>();

        public IndexModel(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public void OnGet()
        {
            //ASPNETCORE_ENVIRONMENT
            EnvironmentVariables.Add(new EnvVariable(nameof(_webHostEnvironment.EnvironmentName), _webHostEnvironment.EnvironmentName));

            //SYSTEM
            EnvironmentVariables.Add(new EnvVariable("VARIABLE_IN_LAUNCHER", Environment.GetEnvironmentVariable("VARIABLE_IN_LAUNCHER")));
            EnvironmentVariables.Add(new EnvVariable("NUMBER_OF_PROCESSORS", Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS")));
            EnvironmentVariables.Add(new EnvVariable("AppSettings:Setting1", Environment.GetEnvironmentVariable("AppSettings:Setting1")));

            //Configuration
            EnvironmentVariables.Add(new EnvVariable("name", _configuration["name"]));
            EnvironmentVariables.Add(new EnvVariable("AppSettings:Setting1", _configuration["AppSettings:Setting1"]));
            EnvironmentVariables.Add(new EnvVariable("AppSettings:Setting2", _configuration["AppSettings:Setting2"]));
            EnvironmentVariables.Add(new EnvVariable("NUMBER_OF_PROCESSORS", _configuration["NUMBER_OF_PROCESSORS"]));

            //Docker
            EnvironmentVariables.Add(new EnvVariable("DOCKER_ENV_VARIABLE", _configuration["DOCKER_ENV_VARIABLE"]));
            EnvironmentVariables.Add(new EnvVariable("VAR_FOR_CONTAINER", _configuration["VAR_FOR_CONTAINER"]));
            EnvironmentVariables.Add(new EnvVariable("AppSettings:Setting1", _configuration["AppSettings:Setting1"]));
        }
    }

    public record EnvVariable(string Name, string? Value);
}