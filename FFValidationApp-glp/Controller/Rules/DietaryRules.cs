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
    public class DietaryRules : Rule
    {
        public override void Define()
        {
            MenuItemModel item = null;
            Errors errors = null;

            When().Match<MenuItemModel>(() => item, i => !i.IsHalal || !i.IsVegan || !i.IsNonGluten)
                  .Match<Errors>(() => errors);

            Then().Do(ctx => errors.AddItem(item));
        }


    }
}
