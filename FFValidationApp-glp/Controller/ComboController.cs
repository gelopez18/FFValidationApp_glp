using FFValidationApp_glp.Models;
using FFValidationApp_glp.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.Collections.Generic;
using System.Text;

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
        public List<ComboModel> Show(CustomerModel customer, OrdersModel order, out bool KeepOrdering) 
        {
            
            List<MenuItemModel> menuItems = DataBase.getMenuData("items");
            List<ComboModel> Combos = DataBase.getMenuData("Combos");
            getComboDetails(menuItems, Combos);
            List<ComboModel> comboPicked = HandleCombosPicked(Combos, out KeepOrdering);
            if (order.comboItems != null)
            {
                foreach (var combo in order.comboItems) { comboPicked.Add(combo); }
            }
            return comboPicked;
        }
        public Dictionary<ComboModel, List<MenuItemModel>> getComboDetails(List<MenuItemModel> menuItems, List<ComboModel> Combos)
        {
            Dictionary<ComboModel ,List<MenuItemModel>>  Details = new Dictionary<ComboModel, List<MenuItemModel>>();
            foreach (var c in Combos)
            {
                List<MenuItemModel> items = new List<MenuItemModel>();
                
                foreach (var item in c.MenuItemId)
                {
                    items.Add(menuItems.Where( i => i.itemId.Equals(item)).First());    
                }
                Details.Add(c, items);
            }
            displayComboMenu(Details);
            return Details;
        }
        public void displayComboMenu(Dictionary<ComboModel, List<MenuItemModel>> Details)
        {
            var table = new Table();
            table.AddColumn(new TableColumn(new Markup("[green]Combo Id[/]")));
            table.AddColumn(new TableColumn("[white]Items[/]"));
            table.AddColumn(new TableColumn("[white]Items Description[/]"));
            table.Columns[1].Width(35);
            foreach (var combo in Details)
            {
                StringBuilder items = new StringBuilder();
                StringBuilder itemsDesc = new StringBuilder();
                foreach (var item in combo.Value)
                {
                    items.Append(item.itemName.ToString());
                    itemsDesc.AppendLine(item.itemDescription+" ");
                    items.Append(", ");
                }
                table.AddRow(combo.Key.comboId.ToString(), items.ToString().Remove(items.Length - 2), itemsDesc.ToString());
               
            }
            AnsiConsole.Write(table);
        }

        public static List<ComboModel> HandleCombosPicked(List<ComboModel> combos, out bool KeepOrdering)
        {
            string res = "";
            List<ComboModel> listOfItems = new List<ComboModel>();
            while (res != "d" && res != "c")
            {
                List<int> itemsName = combos.Select(combo => combo.comboId).ToList();
                var menuPick = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                .Title("Please Choose an item from the[green] Combo Menu[/]?\n[grey](Move up and down to reveal more Items and Enter to pick one)[/]")
                .PageSize(10)
                .AddChoices(itemsName));
                listOfItems.Add(combos.Where(c => c.comboId.Equals(menuPick)).First());
                res = AnsiConsole.Confirm($"[yellow]{menuPick} has been added to the list[/].\nWould you like to add to the order?") ? AnsiConsole.Confirm($"From the Combo Menu?") ? res = "" : res = "c" : res = "d";
            }
            KeepOrdering = res.ToLower() == "c" ? true : false;
            return listOfItems;
        }
    }
}