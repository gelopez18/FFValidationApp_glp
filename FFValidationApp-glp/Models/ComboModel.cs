using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFValidationApp_glp.Models
{
    public class ComboModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int comboId { get; set; }
        public List<int> MenuItemId { get; set; }
        public bool IsHalal { get; set; } = false;
        public bool IsVegan { get; set; } = false;
        public bool IsNonGluten { get; set; } = false;
        public bool IsRegular { get; set; } = false;
        public double Price {  get; set; }  
    }
}
