using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laptop_Ecommerce.Models
{
    public class Processor
    {
        public int Id { get; set; }
        [Display(Name="Processor")]
        public string Name { get; set; }
    }
}
