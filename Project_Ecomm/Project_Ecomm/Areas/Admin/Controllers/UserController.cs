using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Ecomm.DataAccess.Data;
using Project_Ecomm_DataAccess.Repository.IRepository;
using Project_Ecomm_Models;
using Project_Ecomm_Uitility;

namespace Project_Ecomm.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employe)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(ApplicationDbContext context,IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork=unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _context.applicationUsers.Include(u => u.company).ToList();
            var roles = _context.Roles.ToList();
            var userRoles = _context.UserRoles.ToList();
            foreach (var user in userList)
            {
                var roleId = userRoles.FirstOrDefault(r => r.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;
                if(user.company==null)
                {
                    user.company = new Company()
                    {
                        Name = ""
                    };
                }
            }
            var adminUser = userList.FirstOrDefault(u => u.Role == SD.Role_Admin);
            userList.Remove(adminUser);
            return Json(new { data = userList });
        }
        [HttpPost]
        public IActionResult LockUnLock([FromBody]string Id)
        {
            bool isLocked = false;
            var UserInDb=_context.applicationUsers.FirstOrDefault(u=>u.Id== Id);
            if (UserInDb == null)
                return Json(new { success = false, message = "somthint went wrong whilw lock unlock " });
            if(UserInDb!=null && UserInDb.LockoutEnd > DateTime.Now)
            {
                UserInDb.LockoutEnd = DateTime.Now;
                isLocked = false;
            }
            else
            {
                UserInDb.LockoutEnd = DateTime.Now.AddYears(100);
                isLocked = true;
            }
            _context.SaveChanges();
            return Json(new { success = true, message = isLocked == true ? "user successfully lock" : "user successfully unlock" });

        }
        #endregion
    }
}
