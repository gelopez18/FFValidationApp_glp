using FFValidationApp_glp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FFValidationApp_glp.Utils
{
    public class DataBase
    {
        public static dynamic getMenuData(string type)
        {
            //TODO This can be replaced for Sqlserver entity Framework 
            string typeMenu = type.ToUpper() == "ITEMS" ? "menu_items.json" : "combos.json"; ;
            var MenuItems = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName);
            StringBuilder path = new StringBuilder()
                .Append(MenuItems)
                .Append("\\FFValidationApp-glp.Test\\Data")
                .Append($"\\{typeMenu}");
            dynamic res = type.ToUpper() == "ITEMS" ? JsonConvert.DeserializeObject<List<MenuItemModel>>(File.ReadAllText(path.ToString())) : JsonConvert.DeserializeObject<List<ComboModel>>(File.ReadAllText(path.ToString()));
            return res;
        }
        public static void addOrderData(OrdersModel order)
        {
            //TODO process adding the data to a db.
        }
        public static void addErrorsData(OrdersModel order)
        {
            //TODO process adding the data to a db, this will help to acknowledge the customer that was inform the food had specific ingridients, to prevent miss understandings.
        }
    }
}
