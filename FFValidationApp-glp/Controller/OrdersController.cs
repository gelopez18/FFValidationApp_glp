
using FFValidationApp_glp.Controller.Rules;
using FFValidationApp_glp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NRules;
using NRules.Fluent;
using NRules.RuleModel;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FFValidationApp_glp.Controller
{
    public class OrdersController
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        public OrdersController(ILoggerFactory loggerFact, IConfiguration config)
        {
            _config = config;
            _logger = loggerFact.CreateLogger<OrdersController>();
        }
        public static bool addOrder(CustomerModel customer, ref OrdersModel order)
        {
            Errors errors = new Errors();
            foreach (var items in order.menuItems)
            {                
                ValidateMenuItems(items,ref errors);
            }
            RemoveItems(ref errors, ref order);
            if (order.menuItems.Count()==0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
 
        private static void ValidateMenuItems(MenuItemModel item, ref Errors errors)
        {
            var repository = new RuleRepository();
                repository.Load(x => x.From(typeof(DietaryRules).Assembly));                
            var factory = repository.Compile();
            var session = factory.CreateSession();
                session.Insert(item);
                session.Insert(errors);
            session.Fire();  
        }
        private static void handleCustomerFinalDecision(MenuItemModel item, ref Errors errors, ref OrdersModel orders)
        {
            if (AnsiConsole.Confirm($"Would the customer Like to remove the item?"))
            {
                orders.menuItems.Remove(item);
                if (AnsiConsole.Confirm($"Would the customer Like to pick something else?"))
                {
                    List<MenuItemModel> picked = MenuController.HandleMenuPicked(MenuController.DisplayMenu());
                    foreach (var i in picked)
                    {
                        orders.menuItems.Add(i);
                    }
                }                
            }            
        }

        private static void RemoveItems(ref Errors errors, ref OrdersModel order)
        {
            
            if (errors.Model.Any())
            {
                foreach (var result in errors.Model)
                {
                    var Halal = !result.IsHalal ? "Non-Halal" : "Halal";
                    var Vegan = !result.IsVegan ? "Not Vegan" : "Vegan";
                    var Gluten = result.IsNonGluten ? "Gluten free" : "Contains Gluten";

                    if (AnsiConsole.Confirm($"is the customer Aware [red]{result.itemName}[/] is [yellow]{Halal}[/], [yellow]{Vegan}[/] and [yellow]{Gluten}[/]?"))
                    {
                        handleCustomerFinalDecision(result, ref errors, ref order);
                    }
                    else
                    {
                        handleCustomerFinalDecision(result, ref errors, ref order);
                    }
                }
            }
        }

    }
}
