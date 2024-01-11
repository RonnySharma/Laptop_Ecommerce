using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laptop_Ecommerce.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Laptop_Ecommerce.Models.ViewModels
{
    public class SearchVM
    {
        public Laptop Laptop { get; set; }

        public List<Laptop>? Laptops { get; set; }
        public SelectList? GetGraphics { get; set; }
        public SelectList? Processors { get; set; }
        public SelectList? LaptopCompanies { get; set; }
        public string? LaptopsName { get; set; }
        public string? GetGraphicsName { get; set; }
        public string? ProcessorsName { get; set; }
        public string? LaptopCompaniesName { get; set; }
    }
}
