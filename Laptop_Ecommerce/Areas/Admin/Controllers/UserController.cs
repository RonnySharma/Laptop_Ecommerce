using Laptop_Ecommerce.DataAccess.Data;
using Laptop_Ecommerce.DataAccess.Rpository.IRepository;
using Laptop_Ecommerce.Models;
using Laptop_Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laptop_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _context = context;
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
            var userList = _context.ApplicationUsers.ToList();
            var roles = _context.Roles.ToList ();
            var userRole = _context.UserRoles.ToList();
            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(rl => rl.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;
                if(user.CompanyId!=null)
                {
                    user.Company = new Company()
                    {
                        Name = _unitOfWork.Company.Get(Convert.ToInt32(user.CompanyId)).Name
                    };
                }
                if (user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
            }
            var adminUser = userList.FirstOrDefault(au => au.Role == SD.Role_Admin);
            userList.Remove(adminUser);
            return Json(new { data = userList });
        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            bool isLocked = false;
            var userInDb = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (userInDb == null)
                return Json(new { success = false, message = "Something Went Wrong While Locking/Unlocking The User!!!" });
            if(userInDb!=null&&userInDb.LockoutEnd>DateTime.Now)
            {
                userInDb.LockoutEnd = DateTime.Now;
                isLocked = false;
            }
            else
            {
                userInDb.LockoutEnd = DateTime.Now.AddYears(100);
                isLocked=true;
            }
            _context.SaveChanges();
            return Json(new { success = true, message = isLocked == true ? "User Locked Successfully" : "User Unlocked Successfully" });
        }
        #endregion
    }
}
