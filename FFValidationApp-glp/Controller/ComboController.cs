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
            Console.WriteLine("Does the customer have an specific Diet?\n(NonGluten, Vegan, Halal, None)[1 -2 - 3 -4]\nPress B to go back and X to close");
            var res = Console.ReadLine();
            while (res != "X" || res != "x")
            {
                if (res == "X" || res == "x")
                {
                    return res;
                }
                else if (res == "B" || res == "b")
                {
                    return res;
                }
                else
                {
                    switch (res)
                    {




                    }
                }
            }
            return default;
        }
    }
}