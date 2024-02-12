using FFValidationApp_glp.Controller.Rules;
using FFValidationApp_glp.Models;
using FFValidationApp_glp.Utils;
using NRules;
using NRules.Fluent;
using Xunit.Abstractions;

namespace FFValidationApp_glp.Test
{
    public class FFValTest
    {
        [Fact]
        public void Test_Dietary_Rule()
        {
            //Arrange
                Errors err = new Errors();
                var repository = new RuleRepository();
                repository.Load(x => x.From(typeof(DietaryRules).Assembly));
                var factory = repository.Compile();
                var session = factory.CreateSession();
                var menuItem = new MenuItemModel() { itemName = "Falafel Wrap", IsVegan = false, IsHalal = false, IsNonGluten = false };
                session.Insert(menuItem);
                session.Insert(err);
            //Act
                session.Fire();
            //Assert
                Assert.Equal(menuItem.itemName, err.Model.First().itemName);
        }

        [Fact]
        public void Test_Data_Fetch()
        {
            //Act
            var data = DataBase.getMenuData("items");
            //Assert
            Assert.NotNull(data);   
        }
       
    }
}