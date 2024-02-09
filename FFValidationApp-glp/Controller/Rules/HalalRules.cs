using FFValidationApp_glp.Models;
using NRules.Fluent.Dsl;
using NRules.RuleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FFValidationApp_glp.Controller.Rules
{
    public class HalalRules : Rule
    {
        public override void Define()
        {
            MenuItemModel item = null;

            When().Match<MenuItemModel>(() => item, i => !i.IsHalal);
            Then().Do(ctx => HandleValidationErrors(item, new Errors() { Model = item}));
        }
        private static void HandleValidationErrors(MenuItemModel item, Errors errors)
        {
           
        }

    }
}
