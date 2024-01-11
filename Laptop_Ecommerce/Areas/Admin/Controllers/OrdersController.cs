using Laptop_Ecommerce.DataAccess.Data;
using Laptop_Ecommerce.DataAccess.Rpository.IRepository;
using Laptop_Ecommerce.Models;
using Laptop_Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;

namespace Laptop_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public OrdersController(IUnitOfWork unitOfWork,ApplicationDbContext context,IEmailSender emailSender, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ByDateTime(DateTime? datetime1, DateTime? datetime2)
        {
            if(datetime1==null&&datetime2==null)
            {
                return RedirectToAction("Index");
            }
            if (datetime2 == null)
            {
                var date = _unitOfWork.OrderHeader.GetAll(u => u.OrderDate.Date == datetime1);
                return View(date);
            }
            if (datetime1 == null)
            {
                var date = _unitOfWork.OrderHeader.GetAll(u => u.OrderDate.Date == datetime2);
                return View(date);
            }
            IQueryable<OrderHeader> query = from o in _context.OrderHeaders select o;
            if(datetime1!=null&&datetime2!=null)
            {
                query = from o in _context.OrderHeaders where o.OrderDate.Date > datetime1 && o.OrderDate.Date <= datetime2 select o;
            }
            return View(query);
        }
        public IActionResult Details(int id)
        {
            var allDetails = _unitOfWork.OrderDetails.FirstOrDefault(u => u.OrderHeaderId == id, includePropties:"Laptop,OrderHeader,Laptop.LaptopCompany,Laptop.Processor,Laptop.GraphicsCard,OrderHeader.ApplicationUser");
            return View(allDetails);
        }
        public IActionResult StatusApproved()
        {
            var approved = _unitOfWork.OrderHeader.GetAll().Where(os => os.OrderStatus == "Approved");
            if(approved.ToList().Count==0)
            {
                return View("_NoRecord");
            }
            return View(approved);
        }
        public IActionResult StatusPending()
        {
            var approved = _unitOfWork.OrderHeader.GetAll().Where(os => os.OrderStatus == "Pending");
            if (approved.ToList().Count == 0)
            {
                return View("_NoRecord");
            }
            return View(approved);
        }
        public async Task<IActionResult> CancelOrder(int id)
        {
            var cancel = _unitOfWork.OrderHeader.Get(id);
            cancel.OrderStatus = "Cancelled";
            //
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //session
            //if (claim != null)
            //{
            //    var count = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value).ToList().Count;
            //    HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, count);
            //}

            var user = _unitOfWork.ApplicationUser.FirstOrDefault(u => u.Id == cancel.ApplicationUserId);

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            await _emailSender.SendEmailAsync(user.Email, $"Your Order's Has Been Cancelled for Order Id:{cancel.Id}",
                "Please Contact Us For More Information!!!");
            //
            _unitOfWork.OrderHeader.Update(cancel);
            _unitOfWork.Save();
            return View("Index");
        }
        public IActionResult StatusCancelled()
        {
            var approved = _unitOfWork.OrderHeader.GetAll().Where(os => os.OrderStatus == "Cancelled");
            if (approved.ToList().Count == 0)
            {
                return View("_NoRecord");
            }
            return View(approved);
        }
        public IActionResult StatusRefunded()
        {
            var approved = _unitOfWork.OrderHeader.GetAll().Where(os => os.OrderStatus == "Refunded");
            if (approved.ToList().Count == 0)
            {
                return View("_NoRecord");
            }
            return View(approved);
        }
        public IActionResult StatusProcessing()
        {
            var approved = _unitOfWork.OrderHeader.GetAll().Where(os => os.OrderStatus == "Processing");
            if (approved.ToList().Count == 0)
            {
                return View("_NoRecord");
            }
            return View(approved);
        }
        public IActionResult StatusShipped()
        {
            var approved = _unitOfWork.OrderHeader.GetAll().Where(os => os.OrderStatus == "Shipped");
            if (approved.ToList().Count == 0)
            {
                return View("_NoRecord");
            }
            return View(approved);
        }
     
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.OrderHeader.GetAll() });
        }
        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    return Json(new { data = _unitOfWork.OrderHeader.GetAll() });
        //}
        //[HttpGet]
        //public IActionResult StatusApproved()
        //{
        //    var approved = _unitOfWork.OrderHeader.GetAll().Where(os => os.OrderStatus == "Approved");
        //    return View(approved);
        //}
        //[HttpGet]

        //public IActionResult StatusPending()
        //{
        //    var approved = _unitOfWork.OrderHeader.GetAll().Where(os => os.OrderStatus == "Pending");
        //    return View(approved);
        //}
        //[HttpGet]

        //public IActionResult StatusCancelled()
        //{
        //    var approved = _unitOfWork.OrderHeader.GetAll().Where(os => os.OrderStatus == "Cancelled");
        //    return View(approved);
        //}
        //[HttpGet]

        //public IActionResult StatusRefunded()
        //{
        //    var approved = _unitOfWork.OrderHeader.GetAll().Where(os => os.OrderStatus == "Refunded");
        //    return View(approved);
        //}
        //[HttpGet]

        //public IActionResult StatusProcessing()
        //{
        //    var approved = _unitOfWork.OrderHeader.GetAll().Where(os => os.OrderStatus == "Processing");
        //    return View(approved);
        //}
        //[HttpGet]

        //public IActionResult StatusShipped()
        //{
        //    var approved = _unitOfWork.OrderHeader.GetAll().Where(os => os.OrderStatus == "Shipped");
        //    return View(approved);
        //}
        //[HttpGet]
        //public IActionResult AllDetails()
        //{
        //    var allDetails = _unitOfWork.OrderDetails.GetAll();
        //    return Json(new { data = allDetails });
        //}
        #endregion
    }
}
