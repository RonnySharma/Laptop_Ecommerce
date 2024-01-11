using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Project_Ecomm_DataAccess.Repository.IRepository;
using Project_Ecomm_Models;
using Project_Ecomm_Models.ViewModels;
using Project_Ecomm_Uitility;
using System.Data;

namespace Project_Ecomm.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
            public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
                _unitOfWork= unitOfWork;
                _webHostEnvironment= webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? Id)
        {
            ProductVM productVM = new ProductVM()
            {  // multiple table se data lane ka 2 step  step 3 upsert m
                product = new Product(),
                CategoryList = _unitOfWork.category.GetAll().Select(Cl => new SelectListItem()
                {
                    Text = Cl.Name,
                    Value = Cl.Id.ToString()
                }),
                CoverTypeList=_unitOfWork.coverType.GetAll().Select(Ct=>new SelectListItem()
                {
                    Text=Ct.Name,
                    Value=Ct.Id.ToString()
                })
            };
            if (Id == null) return View(productVM);
            productVM.product = _unitOfWork.product.Get(Id.GetValueOrDefault());
            return View(productVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if(!ModelState.IsValid)
            {   // product m image upload ka code step 1 step 2 upsert m or wwroot m folder add krne h 2 
                var WebRootPath = _webHostEnvironment.WebRootPath;
                var Files = HttpContext.Request.Form.Files;
                if(Files.Count()>0)
                {
                    var FileName=Guid.NewGuid().ToString();
                    var Extension = Path.GetExtension(Files[0].FileName);
                    var Uploads = Path.Combine(WebRootPath, @"Image\Products");
                    if(productVM.product.Id!=0) //bina image upadate krne pe chlega or repository k update m ek line likhni h
                    {
                        var ImageExists = _unitOfWork.product.Get(productVM.product.Id).ImageUrl;
                        productVM.product.ImageUrl = ImageExists;
                    }
                    if(productVM.product.ImageUrl!=null)// image update krne pe chlega
                    {
                        var ImagePath = Path.Combine(WebRootPath, productVM.product.ImageUrl.Trim('\\'));
                        if(System.IO.File.Exists(ImagePath))
                        {
                            System.IO.File.Delete(ImagePath);
                        }
                    }
                    using(var FileStream=new FileStream(Path.Combine(Uploads,FileName+Extension),FileMode.Create))
                    {
                        Files[0].CopyTo(FileStream);
                    }
                    productVM.product.ImageUrl = @"\Image\Products\" + FileName + Extension;
                }
                else
                {
                    if (productVM.product.Id != 0)
                    {
                        var ImageExists = _unitOfWork.product.Get(productVM.product.Id).ImageUrl;
                        productVM.product.ImageUrl = ImageExists;
                    }
                }
                if (productVM.product.Id == 0)
                    _unitOfWork.product.Add(productVM.product);
                else
                    _unitOfWork.product.Update(productVM.product);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                productVM = new ProductVM()
                {
                    product = new Product(),
                    CategoryList = _unitOfWork.category.GetAll().Select(Cl => new SelectListItem()
                    {
                        Text = Cl.Name,
                        Value = Cl.Id.ToString()
                    }),
                    CoverTypeList = _unitOfWork.coverType.GetAll().Select(Ct => new SelectListItem()
                    {
                        Text = Ct.Name,
                        Value = Ct.Id.ToString()
                    })
                };
                if(productVM.product.Id!=0)
                {
                    productVM.product = _unitOfWork.product.Get(productVM.product.Id);
                }
                return View(productVM);
            }
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var ProductList = _unitOfWork.product.GetAll(IncludeProperties: "category,coverType");
            return Json(new { data = ProductList });
        }
        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            var ProductInDb = _unitOfWork.product.Get(Id);
            if (ProductInDb == null)
                return Json(new { success = false, message = "Somthing Went Wrong While Delete Data" });
            var WebRootPath = _webHostEnvironment.WebRootPath;
            var ImagePath = Path.Combine(WebRootPath, ProductInDb.ImageUrl.Trim('\\'));
            if(System.IO.File.Exists(ImagePath))
            {
                System.IO.File.Delete(ImagePath);
            }
            _unitOfWork.product.Remove(ProductInDb);
            _unitOfWork.Save();
            return Json(new { success = true, Message = "Data Delete Successfully" });
        }
        #endregion
    }
}
