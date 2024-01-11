using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laptop_Ecommerce.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }

        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public OrderHeader OrderHeader { get; set; }

        public int LaptopId { get; set; }
        [ForeignKey("LaptopId")]
        public Laptop Laptop { get; set; }

        public int Count { get; set; }
        public double Price { get; set; }

    }
}
