using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFValidationApp_glp.Models
{
    public class Errors
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ErrorId{ get; set; }        
        public List<MenuItemModel> Model { get; set; } = new List<MenuItemModel>();

        public void AddItem(MenuItemModel item)
        {
            Model.Add(item);
        }
    }
}
