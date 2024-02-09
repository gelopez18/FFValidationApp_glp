using FFValidationApp_glp.Controller.NewFolder;
using FFValidationApp_glp.Controller.Rules;
using FFValidationApp_glp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NRules;
using NRules.Fluent;
using NRules.RuleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFValidationApp_glp.Controller
{
    public class OrdersController
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        public OrdersController(ILoggerFactory loggerFact, IConfiguration config)
        {
            _config = config;
            _logger = loggerFact.CreateLogger<OrdersController>();
        }
        public static void addOrder(CustomerModel customer, ref OrdersModel order)
        {
            foreach (var items in order.menuItems)
            {
                ValidateMenuItems(items);
            }
            
        }
 
        private static void ValidateMenuItems(MenuItemModel item)
        {
            var repository = new RuleRepository();
                repository.Load(x => x.From(typeof(HalalRules).Assembly));
                repository.Load(x => x.From(typeof(NonGlutenRules).Assembly));
                repository.Load(x => x.From(typeof(VeganRules).Assembly));
            var factory = repository.Compile();
            var session = factory.CreateSession();
                session.Insert(item);
            session.Fire();
        }

       
    }
}
