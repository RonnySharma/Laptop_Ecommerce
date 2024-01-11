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
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? Id)
        {
            CoverType coverType = new CoverType();
            if(Id==null) return View(coverType);
            var param = new DynamicParameters();
            param.Add("@id",Id.GetValueOrDefault());
            coverType = _unitOfWork.SPCALL.OneRecord<CoverType>(SD.Proc_GetCoverType, param);
           // coverType=_unitOfWork.coverType.Get(Id.GetValueOrDefault());
            if (coverType == null) return NotFound();
            return View(coverType);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (coverType == null) return BadRequest();
            if (!ModelState.IsValid) return View(coverType);
            var param = new DynamicParameters();
            param.Add("@name", coverType.Name);
            if (coverType.Id == 0)
                _unitOfWork.SPCALL.Execute(SD.Proc_CreateCoverType, param);
               // _unitOfWork.coverType.Add(coverType);
            else
            {
                param.Add("@id", coverType.Id);
                _unitOfWork.SPCALL.Execute(SD.Proc_UpdateCoverType, param);
            }
               // _unitOfWork.coverType.Update(coverType);
           // _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            //return Json(new { data = _unitOfWork.coverType.GetAll() });
            return Json(new { data = _unitOfWork.SPCALL.List<CoverType>(SD.Proc_GetCoverTypes)});
        }
        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            var param=new DynamicParameters();
            param.Add("id", Id);
            var CoverTypeInDb = _unitOfWork.SPCALL.OneRecord<CoverType>(SD.Proc_GetCoverType, param);
           // var CoverTypeInDb=_unitOfWork.coverType.Get(Id);
            if (CoverTypeInDb == null)
                return Json(new { success = false, Message = "Somthing Went Wrong While Delete Data" });
            _unitOfWork.SPCALL.Execute(SD.Proc_DeleteCoverType, param);
           // _unitOfWork.coverType.Remove(CoverTypeInDb);
           // _unitOfWork.Save();
            return Json(new { success = true, message = "Data Deleted Successfuly?" });
        }
        #endregion
    }
}
