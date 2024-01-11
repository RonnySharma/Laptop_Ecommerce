using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Ecomm_DataAccess.Repository.IRepository;
using Project_Ecomm_Models;
using Project_Ecomm_Uitility;
using System.Data;

namespace Project_Ecomm.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? Id)
        {
            Category category=new Category();
            if(Id==null)return View(category);
            //category = _unitOfWork.category.Get(Id.GetValueOrDefault());
            var param = new DynamicParameters();
            param.Add("@id", Id.GetValueOrDefault());
            category = _unitOfWork.SPCALL.OneRecord<Category>(SD.Proc_GetCategory, param);
            if (category == null) return NotFound();
            return View(category);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (category == null) return NotFound();
            if (!ModelState.IsValid) return View(category);
            var param = new DynamicParameters();
            param.Add("@name", category.Name);
            if (category.Id == 0)
                _unitOfWork.SPCALL.Execute(SD.Proc_CreateCategory, param);
               // _unitOfWork.category.Add(category);
            else
            {
                param.Add("@id", category.Id);
                _unitOfWork.SPCALL.Execute(SD.Proc_UpdateCategory, param);
            }
               // _unitOfWork.category.Update(category);
            //_unitOfWork.Save();
            return RedirectToAction("Index");
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            //  var CategoryList = _unitOfWork.category.GetAll();
            // return Json(new { data = CategoryList }); 
            return Json(new { data = _unitOfWork.SPCALL.List<Category>(SD.Proc_GetCategories) });
        }
        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            var param = new DynamicParameters();
            param.Add("id", Id);
            //var CategoryInDb=_unitOfWork.category.Get(Id);
            var CategoryInDb = _unitOfWork.SPCALL.OneRecord<Category>(SD.Proc_GetCategory, param);
            if (CategoryInDb == null)
                return Json(new { success = false, message = "Somthing Went Wrong While Delete Data!!!" });
            _unitOfWork.SPCALL.Execute(SD.Proc_DeleteCategory, param);
           // _unitOfWork.category.Remove(CategoryInDb);
           // _unitOfWork.Save();
            return Json(new { success = true, message = "Data Deleted Successfully!!" });
        }
        #endregion
    }
}
