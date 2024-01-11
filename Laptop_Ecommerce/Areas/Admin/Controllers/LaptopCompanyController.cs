using Laptop_Ecommerce.DataAccess.Rpository.IRepository;
using Laptop_Ecommerce.Models;
using Laptop_Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laptop_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class LaptopCompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public LaptopCompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            LaptopCompany laptopCompany = new LaptopCompany();
            if (id == null) return View(laptopCompany);
            laptopCompany = _unitOfWork.LaptopCompany.Get(id.GetValueOrDefault());
            if (laptopCompany == null) return BadRequest();
            return View(laptopCompany);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Upsert(LaptopCompany laptopCompany)
        {
            if (laptopCompany == null) return BadRequest();
            if (!ModelState.IsValid) return View();
            if (laptopCompany.Id == 0)
                _unitOfWork.LaptopCompany.Add(laptopCompany);
            else
                _unitOfWork.LaptopCompany.Update(laptopCompany);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.LaptopCompany.GetAll() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var lapcompInDb = _unitOfWork.LaptopCompany.Get(id);
            if (lapcompInDb == null)
                return Json(new { success = false, message = "Something Went Wrong While Deleting Data!!!" });
            _unitOfWork.LaptopCompany.Remove(lapcompInDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Data Deleted Successfully!!!" });
        }
        #endregion
    }
}
