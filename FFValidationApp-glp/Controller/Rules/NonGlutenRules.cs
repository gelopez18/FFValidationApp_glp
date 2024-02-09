using FFValidationApp_glp.Models;
using NRules.Fluent.Dsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFValidationApp_glp.Controller.NewFolder
{
    public class NonGlutenRules : Rule
    {
        public override void Define()
        {
            MenuItemModel item = null;
            When().Match<MenuItemModel>(() => item, i => !i.IsNonGluten);
            //Then().Do();
        }
    }
}