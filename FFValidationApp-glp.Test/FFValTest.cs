using FFValidationApp_glp.Controller.Rules;
using FFValidationApp_glp.Models;
using NRules;
using NRules.Fluent;

namespace FFValidationApp_glp.Test
{
    public class FFValTest
    {
        [Fact]
        public void Test_Halal_Food()
        {
            var repository = new RuleRepository();
            repository.Load(x => x.From(typeof(HalalRules).Assembly)); 
            var factory = repository.Compile();
            var session = factory.CreateSession();
            var menuItem = new MenuItemModel(){ itemName = "Falafel Wrap", IsVegan = true, IsHalal = true, IsNonGluten = false };
            session.Insert(menuItem);

        }
        [Fact]
        public void Test_Vegan_Food()
        {

        }
        [Fact]
        public void Test_NonGluten_Food()
        {

        }
    }
}