using FFValidationApp_glp.Controller.Rules;
using FFValidationApp_glp.Models;
using FFValidationApp_glp.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NRules.RuleModel;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FFValidationApp_glp.Controller
{
    public class MenuController
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        public MenuController(ILogger logger, IConfiguration config)
        {
                _logger = logger;
                _config = config;
        }
        public string Show()
        {
            CustomerModel customer = (CustomerModel)CustomerModel.CustomerCreation();
            var picked = HandleMenuPicked(DisplayMenu());
            var total = ShowOrderDetails(picked);
            var res =  AnsiConsole.Ask<string>("Would you like to proceed?\n[grey](valid input X, B, P)[/]\n");
            if (res.ToUpper() == "X" || res.ToUpper() == "B")
            {
                return res;
            }          
            else if(res.ToUpper()=="P")
            {
                OrdersModel order = new OrdersModel()
                {
                    customerId = customer.customerId,
                    menuItems = picked,
                    Total = total
                };
                if (!OrdersController.addOrder(customer, ref order)) {
                    return "B";
                }
                else
                {
                    Payment.ProcessPayment(order);
                    var rule = new Rule("[red]Order Completed Successfully![/]");
                    AnsiConsole.Write(rule);
                    return "B";
                }
            }
            else
            {
                _logger.LogError("Please enter a valid input X - exit, B - go back, P - proceed");
            }                     
            return default;
        }

        public static List<MenuItemModel> HandleMenuPicked(List<MenuItemModel> menu)
        {
            string res="";
            List<MenuItemModel> listOfItems = new List<MenuItemModel>();
            while (res != "d" && res != "D")
            {
                List<string> itemsName = menu.Select(item => item.itemName).ToList();                
                var menuPick = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Please Choose an item from the[green] Menu[/]?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                .AddChoices(itemsName));
                listOfItems.Add(menu.Where(i => i.itemName.Equals(menuPick, StringComparison.OrdinalIgnoreCase)).First());
                res = AnsiConsole.Confirm($"[yellow]{menuPick} has been added to the list[/].\nWould you like to add another item?") ? res ="": res="d";
            } 
            return listOfItems;
        }
        private double ShowOrderDetails(List<MenuItemModel> picked)
        {
            var OrderDetails = new Table();
                OrderDetails.AddColumn(new TableColumn(new Markup("[green]Order Details[/]")));
                OrderDetails.AddColumn(new TableColumn(new Markup("[red3]Price[/]")));
            double total = 0.0;
            foreach (var item in picked)
            {
                OrderDetails.AddRow(item.itemName, item.itemPrice.ToString());
                total += item.itemPrice;    
            }
                OrderDetails.AddRow("[red]TOTAL[/]", total.ToString());  
            AnsiConsole.Write(OrderDetails);
            return total;
        }

        public static List<MenuItemModel> DisplayMenu()
        {
         
            List<MenuItemModel> menu = DataBase.getMenuData("Items");
            var table = new Table();
            table.AddColumn(new TableColumn(new Markup("[green]Item Id[/]")));
            table.AddColumn(new TableColumn("[white]Item Name[/]"));
            table.AddColumn(new TableColumn("[white]Item Description[/]"));
            table.AddColumn(new TableColumn("[white]Item Type[/]"));
            table.AddColumn(new TableColumn("[white]Item Option[/]"));
            var rows = new List<Rows>();
            foreach (var item in menu)
            {
                var type = item.IsHalal ? "Halal" : item.IsVegan ? "Vegan" : item.IsNonGluten ? "Gluten Free" : "Regular";
                table.AddRow(menu.IndexOf(item).ToString(), item.itemName, item.itemDescription, type, item.itemOption.ToString());
            }
            AnsiConsole.Write(table);

            return menu;
        }
    }
}
