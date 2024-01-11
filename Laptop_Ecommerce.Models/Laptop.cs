using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laptop_Ecommerce.Models
{
    public class Laptop
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Brand")]
        public int LaptopCompanyId{ get; set; }
        public LaptopCompany LaptopCompany { get; set; }
        [Required]
        [Display(Name ="Model Name")]
        public string ModelName { get; set; }
        [Required]
        [Display(Name = "CPU Model")]
        public int ProcessorId { get; set; }
        public Processor Processor { get; set; }
        [Required]
        [Display(Name = "Graphics Card")]
        public int GraphicsCardId { get; set; }
        public GraphicsCard GraphicsCard { get; set; }
        [Required]
        [Display(Name ="Ram Memory")]
        public string RamMemory { get; set; }
        [Required]
        [Display(Name = "Storage Space")]
        public string StorageSpace { get; set; }
        [Required]
        [Display(Name = "Operating System")]
        public string OperatingSystem { get; set; }
        [Required]
        public string Colour { get; set; }
        public string Description { get; set; }
        [Display(Name ="Image Url")]
        public string ImageUrl { get; set; }
        [Required]
        public double ListPrice { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double Price10 { get; set; }
        [Required]
        public double ListPrice20 { get; set; }


        //Task
        public int Quantity { get; set; }
        public bool Active { get; set; }

    }
}
