using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using NuGet.Protocol.Core.Types;
using Project_Ecomm.DataAccess.Data;
using Project_Ecomm_DataAccess.Repository.IRepository;
using Project_Ecomm_Models;
using Project_Ecomm_Uitility;
using System.Text.RegularExpressions;

namespace Project_Ecomm.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin + "," + SD.Role_Employe)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        public OrderController(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ByDateTime(DateTime? dateTime1,DateTime? dateTime2)
        {
            if (dateTime1 == null && dateTime2 == null)
            {
                return RedirectToAction("Index");
            }
            if (dateTime2 == null)
            {
                var date = _unitOfWork.orderHeader.GetAll(u => u.OrderDate.Date == dateTime1);
                return View(date);
            }
            if (dateTime1 == null)
            {
                var date=_unitOfWork.orderHeader.GetAll(u=>u.OrderDate.Date == dateTime2);
                return View(date);
            }
            IQueryable<OrderHeader> query = from o in _context.orderHeaders select o;
            if (dateTime1 != null && dateTime2 != null)
            {
                query = from o in _context.orderHeaders where o.OrderDate.Date > dateTime1 && o.OrderDate.Date <= dateTime2 select o;
            }
            return View(query);
        }
        public IActionResult Details(int id)
        {
            var allDetails = _unitOfWork.orderHeader.FristOrDefault(u => u.Id == id, IncludeProperties: "ApplicationUser");
            return View(allDetails);
        }
        public IActionResult StatusApproved()
        {
            var approved = _unitOfWork.orderHeader.GetAll().Where(os => os.OrderStatus == "Approved");
            return View(approved);
        }
        public IActionResult StatusPending()
        {
            var approved = _unitOfWork.orderHeader.GetAll().Where(os => os.OrderStatus == "Pending");
            return View(approved);
        }
        public IActionResult StatusCancelled()
        {
            var approved = _unitOfWork.orderHeader.GetAll().Where(os => os.OrderStatus == "Cancelled");
            return View(approved);
        }
        public IActionResult StatusRefunded()
        {
            var approved = _unitOfWork.orderHeader.GetAll().Where(os => os.OrderStatus == "Refunded");
            return View(approved);
        }
        public IActionResult StatusProcessing()
        {
            var approved = _unitOfWork.orderHeader.GetAll().Where(os => os.OrderStatus == "Processing");
            return View(approved);
        }
        public IActionResult StatusShipped()
        {
            var approved = _unitOfWork.orderHeader.GetAll().Where(os => os.OrderStatus == "Shipped");
            return View(approved);
        }
        #region APIs
        [HttpGet]

        //public IActionResult TopOrder()
        //{
        //    var TopProduct = _context.products.Join(_context.products, Product => Product.Id,
        //        View => View.CategoryId, (Product, View) => new { ProductId = Product.Id, ViewId = View.Id });
        //    SearchOrderBy.Id(g => g.ProductId).select(s => new
        //    {
        //        id = s.key,
        //        viewCount = s.count()
        //    });
        //    return View(TopOrder);
        //}
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.orderHeader.GetAll() });
        }
        #endregion
    }
}
