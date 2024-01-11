using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Ecomm.DataAccess.Data;
using Project_Ecomm_DataAccess.Repository.IRepository;
using Project_Ecomm_Models;
using Project_Ecomm_Uitility;
using System.Data;

namespace Project_Ecomm.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employe)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? Id)
        {
            Company company = new Company();
            if (Id == null) return View(company);
            company = _unitOfWork.company.Get(Id.GetValueOrDefault());
            if (company == null) return NotFound();
            return View(company);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (company == null) return BadRequest();
            if (!ModelState.IsValid) return View(company);
            if (company.Id == 0)
                _unitOfWork.company.Add(company);
            else
                _unitOfWork.company.Update(company);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.company.GetAll() });
        }
        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            var CompanyInDb=_unitOfWork.company.Get(Id);
            if (CompanyInDb == null)
                return Json(new { success = false, message = "Somthing Went Wrong While Delete Data!!!" });
            _unitOfWork.company.Remove(CompanyInDb);
             _unitOfWork.Save();
            return Json(new { success = true, message = "Data Deleted Successfully!!" });
        }
        #endregion
    }
}
