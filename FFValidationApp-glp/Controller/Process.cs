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
            var close = "";
            while (close.ToLower() != "x")
            {
                CustomerModel customer = (CustomerModel)CustomerModel.CustomerCreation();
                OrdersModel order = new OrdersModel()
                {
                    customerId = customer.customerId
                };
                bool keepOrdering;
                var message = "Combo or Single Menu item?\n[grey](valid input 1 or 2 Press X to exit)[/]\n";
                var res = AnsiConsole.Ask<string>(message);
                while (res.ToLower() != "z" || res.ToLower() != "x" || res.ToLower() != "b")
                {
                    switch (res)
                    {
                        case "1":
                            order.comboItems = Combo.Show(customer, order, out keepOrdering);
                            if (keepOrdering)
                            {
                                order.menuItems = Menu.Show(customer, order, out keepOrdering);
                                if (!keepOrdering)
                                {
                                    res = OrdersController.processOrders(_logger, customer, order, Menu.ShowOrderDetails(order), keepOrdering);
                                }
                                else
                                {
                                    res = "1";
                                }
                            }
                            else
                            {
                                res = OrdersController.processOrders(_logger, customer, order, Menu.ShowOrderDetails(order), keepOrdering);
                            }
                            break;
                        case "2":

                            order.menuItems = Menu.Show(customer, order, out keepOrdering);
                            if (keepOrdering)
                            {
                                order.comboItems = Combo.Show(customer, order, out keepOrdering);
                                if (!keepOrdering)
                                {
                                    res = OrdersController.processOrders(_logger, customer, order, Menu.ShowOrderDetails(order), keepOrdering);
                                }
                                else
                                {
                                    res = "2";
                                }
                            }
                            else
                            {
                                res = OrdersController.processOrders(_logger, customer, order, Menu.ShowOrderDetails(order), keepOrdering);
                            }
                            break;
                        default:
                            _logger.LogError("Please enter a valid input: (1 or 2)");
                            break;
                    }
                    if (res.ToLower() == "x" || res.ToLower() == "z")
                    {
                        close = res.ToLower() == "x" ? "x" : "z";
                        break;
                    }
                    if (res.ToUpper() == "B" || res == "1" || res == "2")
                    {
                        res = AnsiConsole.Ask<string>(message);
                    }
                }
            }


            

        }

        

      
    }
}
