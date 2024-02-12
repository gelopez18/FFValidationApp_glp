
using FFValidationApp_glp.Controller.Rules;
using FFValidationApp_glp.Models;
using FFValidationApp_glp.Utils;
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
        public static bool addOrder(CustomerModel customer, ref OrdersModel order, bool keepOrdering)
        {
            bool temp = true;
            Errors errors = new Errors();
            if (order.menuItems != null)
            {
                foreach (var items in order.menuItems)
                {
                    ValidateMenuItems(items, ref errors);
                }
                if (errors.Model != null)
                {
                    RemoveItems(ref errors, ref order, keepOrdering);
                }
                if (order.menuItems.Count()==0)
                {
                    temp = false;
                }
                else
                {
                    temp = true;
                }
            }
            if (order.comboItems != null)
            {
                foreach (var combo in order.comboItems)
                {
                    ValidateMenuItems(combo, ref errors);
                }
                if (errors.ComboModel != null)
                {
                    RemoveCombo(ref errors, ref order, keepOrdering);
                }

                if (order.comboItems.Count() == 0 && temp == false || (order.comboItems.Count() == 0 &&  order.menuItems == null))
                {
                    temp = false;
                }
                else
                {
                    temp = true;
                }
            }
            return temp;
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
        private static void ValidateMenuItems(ComboModel item, ref Errors errors)
        {
            var repository = new RuleRepository();
            repository.Load(x => x.From(typeof(ComboDietaryRule).Assembly));
            var factory = repository.Compile();
            var session = factory.CreateSession();
            session.Insert(item);
            session.Insert(errors);
            session.Fire();
        }
        private static void handleCustomerFinalDecision(MenuItemModel item, ref Errors errors, ref OrdersModel orders, bool keepOrdering)
        {            
                orders.menuItems.Remove(item);
                if (AnsiConsole.Confirm($"Would the customer Like to pick something else?"))
                {
                    if (AnsiConsole.Confirm($"From the Item Menu?"))
                    {
                        List<MenuItemModel> picked = MenuController.HandleMenuPicked(MenuController.DisplayMenu(), out keepOrdering);
                        foreach (var i in picked)
                        {
                            orders.menuItems.Add(i);
                        }
                    }
                    
                }                                        
        }
        private static void handleCustomerFinalDecision(ComboModel item, ref Errors errors, ref OrdersModel orders, bool keepOrdering)
        {
            orders.comboItems.Remove(item);
            if (AnsiConsole.Confirm($"Would the customer Like to pick something else?"))
            {
                if (AnsiConsole.Confirm($"From the Combo Menu?"))
                {
                    List<MenuItemModel> picked = MenuController.HandleMenuPicked(MenuController.DisplayMenu(), out keepOrdering);
                    foreach (var i in picked)
                    {
                        orders.menuItems.Add(i);
                    }
                }

            }
        }

        private static void RemoveItems(ref Errors errors, ref OrdersModel order, bool keepOrdering)
        {
            
            if (errors.Model.Any())
            {
                foreach (var result in errors.Model)
                {
                    var Halal = !result.IsHalal ? "Non-Halal" : "Halal";
                    var Vegan = !result.IsVegan ? "Not Vegan" : "Vegan";
                    var Gluten = result.IsNonGluten ? "Gluten free" : "Contains Gluten";

                    if (AnsiConsole.Confirm($"is the customer Aware [red]{result.itemName}[/] is [yellow]{Halal}[/], [yellow]{Vegan}[/] and [yellow]{Gluten}[/]\nWould the customer Like to remove the item?"))
                    {
                        handleCustomerFinalDecision(result, ref errors, ref order, keepOrdering);
                    }
                    
                }
            }
            
        }
        private static void RemoveCombo(ref Errors errors, ref OrdersModel order, bool keepOrdering)
        {
            List<MenuItemModel> menuData = DataBase.getMenuData("Items");
            if (errors.ComboModel.Any())
            {
                foreach (var c in errors.ComboModel)
                {
                    StringBuilder result = new StringBuilder();
                    foreach (var item in c.MenuItemId)
                    {
                        result.Append(menuData.Where(d => d.itemId.Equals(item)).First().itemName + " and ");
                    }

                    var Halal = !c.IsHalal ? "Non-Halal" : "Halal";
                    var Vegan = !c.IsVegan ? "Not Vegan" : "Vegan";
                    var Gluten = c.IsNonGluten ? "Gluten free" : "Contains Gluten";

                    if (AnsiConsole.Confirm($"is the customer Aware [red]{result.ToString().Remove(result.Length - 5)}[/] is [yellow]{Halal}[/], [yellow]{Vegan}[/] and [yellow]{Gluten}[/]\nWould the customer Like to remove the item?"))
                    {
                        handleCustomerFinalDecision(c, ref errors, ref order, keepOrdering);
                    }

                }
            }
        }
        public static string processOrders(ILogger logger, CustomerModel customer, OrdersModel order, double total, bool keepOdering)
        {
            var res = AnsiConsole.Ask<string>("Would you like to proceed?\n[grey](valid input X -Exit, B -Back, P -Process)[/]\n");
            if (res.ToUpper() == "X" || res.ToUpper() == "B")
            {
                return res;
            }
            else if (res.ToUpper() == "P")
            {
                
                if (!OrdersController.addOrder(customer, ref order, keepOdering))
                {
                    return "1";
                }
                else
                {
                    Payment.ProcessPayment(order);
                    var rule = new Rule("[Green]Order Completed Successfully![/]");
                    AnsiConsole.Write(rule);
                    return "z";
                }
            }
            else
            {
                logger.LogError("Please enter a valid input X - exit, B - go back, P - proceed");
            }
            return default;
        }
    }
}
