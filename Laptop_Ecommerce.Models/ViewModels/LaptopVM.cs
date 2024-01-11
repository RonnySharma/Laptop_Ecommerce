using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laptop_Ecommerce.Models.ViewModels
{
    public class LaptopVM
    {
        public Laptop laptop { get; set; }
        public IEnumerable<SelectListItem> LaptopCompanyList { get; set; }
        public IEnumerable<SelectListItem> ProcessorList { get; set; }
        public IEnumerable<SelectListItem> GraphicsCardList { get; set; }
    }
}
