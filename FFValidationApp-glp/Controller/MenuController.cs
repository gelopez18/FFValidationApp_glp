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
        public List<MenuItemModel> Show(CustomerModel customer, OrdersModel order, out bool KeepOrdering)
        {            
            List<MenuItemModel> picked = HandleMenuPicked(DisplayMenu(), out KeepOrdering);
            if (order.menuItems != null) { foreach (var item in order.menuItems) { picked.Add(item); } }           
            return picked;                    
        }
        public static List<MenuItemModel> HandleMenuPicked(List<MenuItemModel> menu, out bool keepOrdering)
        {
            string res="";
            List<MenuItemModel> listOfItems = new List<MenuItemModel>();
            while (res != "d" && res != "c")
            {
                List<string> itemsName = menu.Select(item => item.itemName).ToList();                
                var menuPick = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Please Choose an item from the[green] Menu[/]?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more Items[/]")
                .AddChoices(itemsName));
                listOfItems.Add(menu.Where(i => i.itemName.Equals(menuPick, StringComparison.OrdinalIgnoreCase)).First());
                res = AnsiConsole.Confirm($"[yellow]{menuPick} has been added to the list[/].\nWould you like to add another item?") ? AnsiConsole.Confirm($"From the Item Menu?") ? res = "" : res = "c" : res = "d";               
            }
            keepOrdering = res.ToLower() == "c" ? true : false;

            return listOfItems;
        }
        public double ShowOrderDetails(OrdersModel order)
        {
            var OrderDetails = new Table();
                OrderDetails.AddColumn(new TableColumn(new Markup("[green]Order Details[/]")));
                OrderDetails.AddColumn(new TableColumn(new Markup("[red3]Price[/]")));
            double total = 0.0;
            if (order.menuItems != null)
            {
                foreach (var item in order.menuItems)
                {
                    OrderDetails.AddRow(item.itemName, item.itemPrice.ToString());
                    total += item.itemPrice;
                }
            }
            if (order.comboItems != null)
            {
                foreach (var combo in order.comboItems)
                {
                    OrderDetails.AddRow(combo.comboId.ToString(), combo.Price.ToString());
                    total += combo.Price;
                }
            }            
                order.Total = total;
                OrderDetails.AddRow("[red]TOTAL[/]", order.Total.ToString());  
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
