using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laptop_Ecommerce.Models.ViewModels
{
    public class LaptopByCountVM
    {
        public IEnumerable<Laptop> laptops { get; set; }

        public IEnumerable<OrderDetails> orderDetails { get; set; }
    }
}
