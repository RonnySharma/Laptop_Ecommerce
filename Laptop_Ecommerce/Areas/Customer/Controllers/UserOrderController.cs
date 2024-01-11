using Laptop_Ecommerce.DataAccess.Rpository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Laptop_Ecommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class UserOrderController : Controller
    { private readonly IUnitOfWork _unitOfWork;
        public UserOrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var applicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(x => x.Id == claim.Value);
            var userAllOrders = _unitOfWork.OrderDetails.GetAll(includePropties:"OrderHeader,OrderHeader.ApplicationUser").Where(x => x.OrderHeader.ApplicationUserId == applicationUser.Id);
            return Json(new { data = userAllOrders });
        }
        #endregion
    }
}
