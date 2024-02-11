using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFValidationApp_glp.Models
{
    public class OrdersModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ordersId {  get; set; }
        public long customerId { get; set; }
        public DateTime orderDate { get; set; }
        public List<MenuItemModel> menuItems { get; set; }
        public List<ComboModel> comboItems { get; set; }
        public double Total {  get; set; }

    }
}
