using Laptop_Ecommerce.DataAccess.Rpository.IRepository;
using Laptop_Ecommerce.Models;
using Laptop_Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laptop_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProcessorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProcessorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Processor processor = new Processor();
            if (id == null) return View(processor);
            processor = _unitOfWork.Processor.Get(id.GetValueOrDefault());
            if (processor == null) return BadRequest();
            return View(processor);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Upsert(Processor processor)
        {
            if (processor == null) return BadRequest();
            if (!ModelState.IsValid) return View();
            if (processor.Id == 0)
                _unitOfWork.Processor.Add(processor);
            else
                _unitOfWork.Processor.Update(processor);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Processor.GetAll() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var cpugpuInDB = _unitOfWork.Processor.Get(id);
            if (cpugpuInDB == null)
                return Json(new { success = false, message = "Something Went Wrong While Deleting Data!!!" });
            _unitOfWork.Processor.Remove(cpugpuInDB);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Data Deleted Successfully!!!" });
        }
        #endregion
    }
}
