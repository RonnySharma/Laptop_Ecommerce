using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Laptop_Ecommerce.Models
{
    public class GraphicsCard
    {
        public int Id { get; set; }
        [Display(Name = "Graphics Card")]
        public string Name { get; set; }
    }
}
