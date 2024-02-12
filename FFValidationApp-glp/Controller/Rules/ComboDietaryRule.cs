using FFValidationApp_glp.Models;
using NRules.Fluent.Dsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFValidationApp_glp.Controller.Rules
{
    public class ComboDietaryRule : Rule
    {
        public override void Define()
        {
            ComboModel combo = null;
            Errors errors = null;
            When().Match<ComboModel>(() => combo, c => !c.IsHalal || !c.IsVegan || !c.IsNonGluten)
                .Match<Errors>(() => errors);
            Then().Do(ctx => errors.AddCombo(combo));
        }
    }
}
