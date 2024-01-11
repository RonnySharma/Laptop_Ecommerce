using Laptop_Ecommerce.DataAccess.Rpository.IRepository;
using Laptop_Ecommerce.Models;
using Laptop_Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laptop_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class GraphicsCardController : Controller
    { private readonly IUnitOfWork _unitOfWork;
        public GraphicsCardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            GraphicsCard graphicsCard = new GraphicsCard();
            if (id == null) return View(graphicsCard);
            graphicsCard = _unitOfWork.GraphicsCard.Get(id.GetValueOrDefault());
            if (graphicsCard == null) return BadRequest();
            return View(graphicsCard);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Upsert(GraphicsCard graphicsCard)
        {
            if (graphicsCard == null) return BadRequest();
            if (!ModelState.IsValid) return View();
            if (graphicsCard.Id == 0)
                _unitOfWork.GraphicsCard.Add(graphicsCard);
            else
                _unitOfWork.GraphicsCard.Update(graphicsCard);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.GraphicsCard.GetAll() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var graphicsCardInDb = _unitOfWork.GraphicsCard.Get(id);
            if (graphicsCardInDb == null)
                return Json(new { success = false, message = "Something Went Wrong While Deleting Data!!!" });
            _unitOfWork.GraphicsCard.Remove(graphicsCardInDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Data Deleted Successfully!!!" });
        }
        #endregion
    }
}
