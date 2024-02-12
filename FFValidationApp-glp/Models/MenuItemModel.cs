using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFValidationApp_glp.Models
{
    public class MenuItemModel 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int itemId { get; set; }
        public string itemName { get; set; }
        public string itemDescription { get; set; } 
        public string itemOption { get; set; }
        public double itemPrice { get; set; }
        public bool IsHalal { get; set; } = false;
        public bool IsVegan { get; set; } = false;
        public bool IsNonGluten { get; set; } = false;
        public bool IsRegular { get; set; } = false;
    }
}
