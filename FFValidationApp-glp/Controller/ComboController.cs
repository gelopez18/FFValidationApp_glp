using FFValidationApp_glp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FFValidationApp_glp.Controller
{
    internal class ComboController : ComboModel
    {
        private ILogger logger;
        private IConfiguration config;

        public ComboController(ILogger logger, IConfiguration config)
        {
            this.logger = logger;
            this.config = config;
        }
        public string Show() 
        {
            
            return default;
        }
    }
}