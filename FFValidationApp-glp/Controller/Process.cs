using FFValidationApp_glp.Controller.NewFolder;
using FFValidationApp_glp.Controller.Rules;
using FFValidationApp_glp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NRules;
using NRules.Fluent;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFValidationApp_glp.Controller
{
    public class Process
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly MenuController Menu;
        private readonly ComboController Combo;

        public Process(Microsoft.Extensions.Logging.ILoggerFactory loggerFact, IConfiguration config)
        {
            _logger = loggerFact.CreateLogger<Process>();
            _config = config;    
            Menu = new MenuController(_logger, _config);
            Combo = new ComboController(_logger, _config);
        }

        public void Run()
        {
            var message = "Combo or Single Menu item?\n[grey](valid input 1 or 2 Press X to exit)[/]\n";            
            var res = AnsiConsole.Ask<string>(message);
            while (res != "X" || res != "x" || res != "B" || res != "b")
            {
                switch (res)
                {
                    case "1":
                       res = Combo.Show();
                        break;
                    case "2":
                        res = Menu.Show();
                        break;
                    default:
                        _logger.LogError("Please enter a valid input: (1 or 2)");
                        break;
                }
                if (res == "X" || res == "x")
                {
                    break;
                }
                if (res == "B" || res == "b")
                {
                    AnsiConsole.Ask<string>(message);
                }
                res = Console.ReadLine();
            }
            
        }

      
    }
}
