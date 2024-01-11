using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ecomm_Models.ViewModels
{
    public class ProductVM
    { // multipule table se data lane k leye step 1   step 2 productController m
        public Product product { get; set; }
        public  IEnumerable<SelectListItem> CategoryList { get; set; }
        public  IEnumerable<SelectListItem> CoverTypeList { get; set; }
    }
}
