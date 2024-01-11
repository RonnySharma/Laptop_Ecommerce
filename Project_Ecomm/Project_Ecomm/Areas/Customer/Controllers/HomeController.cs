using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project_Ecomm.Models;
using Project_Ecomm.Models.ViewModels;
using Project_Ecomm_DataAccess.Repository.IRepository;
using Project_Ecomm_Models;
using Project_Ecomm_Uitility;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace Project_Ecomm.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var Claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claim != null)
            {
                var Count = _unitOfWork.shoppingCart.GetAll(sc => sc.ApplicationUserId == Claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, Count);
            }
            var ProductList = _unitOfWork.product.GetAll(IncludeProperties: "category,coverType");
            return View(ProductList);
        }
        [HttpPost]
        public IActionResult index(string search)
        {
            var productlist = _unitOfWork.product.GetAll(IncludeProperties: "category,coverType");
            if (search != null)
            {
                productlist = _unitOfWork.product.GetAll().Where(c => c.Author.ToLower().Contains(search.ToLower())
                || (c.Title.ToLower().Contains(search.ToLower())));
            }
            return View(productlist);
        }
        public IActionResult Details(int id)
        {
            var ProductInDb = _unitOfWork.product.FristOrDefault(p => p.Id == id, IncludeProperties: "category,coverType");
            if (ProductInDb == null) return NotFound();
            var ShoppingCart = new ShoppingCart()
            {
                Product=ProductInDb,
                ProductId=ProductInDb.Id
            };
            //session
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claim != null)
            {
                var count = _unitOfWork.shoppingCart.GetAll(sc => sc.ApplicationUserId == Claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, count);
            }
            return View(ShoppingCart);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = 0;
            if (!ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null) return NotFound();
                shoppingCart.ApplicationUserId = claim.Value;
                var ShoppingCartFormDb = _unitOfWork.shoppingCart.FristOrDefault(sc => sc.ApplicationUserId == claim.Value && sc.ProductId == shoppingCart.ProductId);
                if (ShoppingCartFormDb == null)
                    _unitOfWork.shoppingCart.Add(shoppingCart);
                else
                    ShoppingCartFormDb.Count += shoppingCart.Count;
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                var ProductInDb = _unitOfWork.product.FristOrDefault(p => p.Id ==shoppingCart.ProductId, IncludeProperties: "category,coverType");
                if (ProductInDb == null) return NotFound();
                var ShoppingCartEdit = new ShoppingCart()
                {
                    Product = ProductInDb,
                    ProductId = ProductInDb.Id
                };
                return View(ShoppingCartEdit);
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}