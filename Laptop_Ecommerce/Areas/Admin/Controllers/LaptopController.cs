using Laptop_Ecommerce.DataAccess.Rpository.IRepository;
using Laptop_Ecommerce.Models;
using Laptop_Ecommerce.Models.ViewModels;
using Laptop_Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Laptop_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class LaptopController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LaptopController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            LaptopVM laptopVM = new LaptopVM()
            {
                laptop = new Laptop(),
                LaptopCompanyList = _unitOfWork.LaptopCompany.GetAll().Select(lcl => new SelectListItem
                {
                    Text = lcl.Name,
                    Value = lcl.Id.ToString()
                }),
                ProcessorList = _unitOfWork.Processor.GetAll().Select(pl => new SelectListItem
                {
                    Text = pl.Name,
                    Value = pl.Id.ToString()
                }),
                GraphicsCardList = _unitOfWork.GraphicsCard.GetAll().Select(gcl => new SelectListItem
                {
                    Text = gcl.Name,
                    Value = gcl.Id.ToString()
                })
            };
            if (id == null) return View(laptopVM);
            laptopVM.laptop = _unitOfWork.Laptop.Get(id.GetValueOrDefault());
            if (laptopVM.laptop == null) return BadRequest();
            return View(laptopVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Upsert(LaptopVM laptopVM)
        {
            if(!ModelState.IsValid)
            {
                var webRootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if(files.Count()>0)
                {
                    var fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(files[0].FileName);
                    var uploads = Path.Combine(webRootPath, @"images\laptops");
                    if(laptopVM.laptop.Id!=0)
                    {
                        var imageExists = _unitOfWork.Laptop.Get(laptopVM.laptop.Id).ImageUrl;
                        laptopVM.laptop.ImageUrl = imageExists;
                    }
                    if(laptopVM.laptop.ImageUrl!=null)
                    {
                        var imagePath = Path.Combine(webRootPath, laptopVM.laptop.ImageUrl.Trim('\\'));
                        if(System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using(var fileStream=new FileStream(Path.Combine(uploads,fileName+extension),FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    laptopVM.laptop.ImageUrl=@"\images\laptops\"+fileName+extension;
                }
                else
                {
                    if(laptopVM.laptop.Id!=0)
                    {
                        var imageExists = _unitOfWork.Laptop.Get(laptopVM.laptop.Id).ImageUrl;
                        laptopVM.laptop.ImageUrl = imageExists;
                    }
                }
                if (laptopVM.laptop.Id == 0)
                    _unitOfWork.Laptop.Add(laptopVM.laptop);
                else
                    _unitOfWork.Laptop.Update(laptopVM.laptop);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                 laptopVM = new LaptopVM()
                {
                    laptop = new Laptop(),
                    LaptopCompanyList = _unitOfWork.LaptopCompany.GetAll().Select(lcl => new SelectListItem
                    {
                        Text = lcl.Name,
                        Value = lcl.Id.ToString()
                    }),
                    ProcessorList = _unitOfWork.Processor.GetAll().Select(pl => new SelectListItem
                    {
                        Text = pl.Name,
                        Value = pl.Id.ToString()
                    }),
                    GraphicsCardList = _unitOfWork.GraphicsCard.GetAll().Select(gcl => new SelectListItem
                    {
                        Text = gcl.Name,
                        Value = gcl.Id.ToString()
                    })
                };
                if(laptopVM.laptop.Id!=0)
                {
                    laptopVM.laptop = _unitOfWork.Laptop.Get(laptopVM.laptop.Id);
                }
                return View(laptopVM);
            }
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Laptop.GetAll(includePropties: "LaptopCompany,Processor,GraphicsCard") });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var laptopInDb = _unitOfWork.Laptop.Get(id);
            if (laptopInDb == null)
                return Json(new { success = false, message = "Something Went Wrong While Deleting Data!!!" });
            var webRootPath = _webHostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, laptopInDb.ImageUrl.Trim('\\'));
            if(System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _unitOfWork.Laptop.Remove(laptopInDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Data Deleted Successfully!!!" });
        }
        #endregion
    }
}
