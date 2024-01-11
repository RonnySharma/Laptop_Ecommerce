using Laptop_Ecommerce.DataAccess.Data;
using Laptop_Ecommerce.DataAccess.Rpository;
using Laptop_Ecommerce.DataAccess.Rpository.IRepository;
using Laptop_Ecommerce.Models;
using Laptop_Ecommerce.Models.ViewModels;
using Laptop_Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Laptop_Ecommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        public static Laptop Laptop1 { get; set; }
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _context = context;

        }
        public async Task<IActionResult> Index(string searchBy, string search)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, count);
            }
            if (_context.Laptops == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            if (searchBy == "company")
            {
                return View(_context.Laptops.Include("LaptopCompany").Where(x => x.LaptopCompany.Name.Contains(search)).ToList());
            }
            else if (searchBy == "modelName")
            {
                return View(_context.Laptops.Include("LaptopCompany").Where(x => x.ModelName.Contains(search)).ToList());
            }
            else if (searchBy == null && search == null)
            {
                //***
                var laptopsList = _unitOfWork.Laptop.GetAll(includePropties: "LaptopCompany,Processor,GraphicsCard").Where(x=>x.Active==true);
                return View(laptopsList);

                //var byCount = _context.OrderDetails.GroupBy(x => x.LaptopId);

                //var totalSales = byCount.Select(x => new
                //{
                //    itemname = x.Key,
                //    quantitySold = x.Sum(x => x.Count)
                //});
                //var ordersales = totalSales.OrderByDescending(x => x.quantitySold);
                //var highestSelled = ordersales.FirstOrDefault();
                //List<Laptop> laptops = new List<Laptop>();
                //foreach(var item in ordersales)
                //{
                //     Laptop1= _context.Laptops.FirstOrDefault(p => p.Id == item.itemname);
                //    laptops.Add(Laptop1);
                //}
                //laptopsList = laptops;
                //return View(laptopsList);
                //***

                //var productByCounts = _unitOfWork.OrderDetails.GetAll().OrderByDescending(x=>x.Count).GroupBy(x=>x.LaptopId);
                //List<Laptop> laptops = new List<Laptop>();
                //foreach (var laptop in productByCounts)
                //{
                //    var any = _unitOfWork.Laptop.FirstOrDefault(x => x.Id == laptop.Key);
                //    laptops.Add(any);
                //}
                //return View(laptops);


                //var productlists = _unitOfWork.Laptop.GetAll(includePropties: "LaptopCompany,Processor,GraphicsCard");
                //return View(productlists);


                //var productByCounts = _unitOfWork.OrderDetails.GetAll(includePropties: "Laptop.LaptopCompany,Laptop.Processor,Laptop.GraphicsCard").OrderByDescending(x => x.Count);
                //return View(productByCounts);

                //var counts = _unitOfWork.OrderDetails.GetAll();
                //var productByCounts = _unitOfWork.Laptop.GetAll(includePropties: "LaptopCompany,Processor,GraphicsCard").OrderByDescending(x=>x.ModelName);
                //return View(productByCounts);

                //var laptopByCount = _context.OrderDetails.Include("Laptop").GroupBy(x => x.LaptopId).Select(x=>x.OrderByDescending(x => x.Count).FirstOrDefault()).ToList();
                //var laptopByCount = _context.OrderDetails.Include("Laptop").GroupBy(x => x.LaptopId).Select(x => x.OrderByDescending(x => x.Count)).ToList();
                //var laptopByCount = _unitOfWork.OrderDetails.GetAll().GroupBy(x => x.LaptopId).Select(x => x.OrderByDescending(x => x.Count));
                //var laptopByCountList = laptopByCount.AsEnumerable().ToList();

                //var laptopByCoutList = _unitOfWork.OrderDetails.GetAll().OrderByDescending(x => x.Count);
                //var count = laptopByCoutList.GroupBy(x => x.LaptopId.ToString()).ToList();
                //return View(count);

                //var count = _unitOfWork.OrderDetails.GetAll().GroupBy(x => x.LaptopId).Select(x => x.OrderByDescending(x => x.Count)).ToList();
                //return View(count);

                //LaptopByCountVM laptopByCountVM = new LaptopByCountVM()
                //{
                //    laptops= _unitOfWork.Laptop.GetAll(includePropties: "LaptopCompany,Processor,GraphicsCard"),
                //    orderDetails=_unitOfWork.OrderDetails.GetAll()
                //};
                //return View(laptopByCountVM);
            }

            //var laptop = from l in _context.Laptops.Include("LaptopCompany")
            //             select l;
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    laptop = laptop.Where(s => s.ModelName!.Contains(searchString));
            //}


            //var productByCount = _unitOfWork.OrderDetails.GetAll(includePropties: "Laptop,Laptop.LaptopCompany,Laptop.Processor,Laptop.GraphicsCard").OrderByDescending(x=>x.Count).GroupBy(x=>x.LaptopId);
            
            
            //var productlist = _unitOfWork.Laptop.GetAll(includePropties: "LaptopCompany,Processor,GraphicsCard");
            //).OrderByDescending(x => _unitOfWork.ShoppingCart.GetAll(x => x.Count)
            //return View(productlist);
            //return View(productByCount);
            // return View(await laptop.ToListAsync());

            var laptopList = _unitOfWork.Laptop.GetAll(includePropties: "LaptopCompany,Processor,GraphicsCard");
            return View(laptopList);
        }
        public IActionResult Trending()
        {
            var productByCounts = _unitOfWork.OrderDetails.GetAll(includePropties:"Laptop").Where(x=>x.Laptop.Active==true).OrderByDescending(x => x.Count).GroupBy(x => x.LaptopId);
            List<Laptop> laptops = new List<Laptop>();
            foreach (var laptop in productByCounts)
            {
                var any = _unitOfWork.Laptop.FirstOrDefault(x =>x.Id == laptop.Key);
                laptops.Add(any);
            }
            return View("Index",laptops);
        }
        //public async Task<IActionResult> Index(HomeProductVM homeProductVM)
        //{
        //   
        //    var laptops = from m in _context.Laptops select m;
        //    var graphicName = _context.GraphicsCards.ToList();
        //    var processorName = _context.Processors.ToList();
        //    var companyName = _context.LaptopCompanies.ToList();

        //    if (!String.IsNullOrEmpty(homeProductVM.LaptopsName))
        //    {
        //        laptops = laptops.Where(m=>m.ModelName!.Contains(homeProductVM.LaptopsName));
        //    }

        //    if (homeProductVM.GetGraphicsName != null)
        //    {
        //        laptops = laptops.Where(m => m.GraphicsCard.Name== homeProductVM.GetGraphicsName);
        //    }

        //    if (homeProductVM.ProcessorsName != null)
        //    {
        //        laptops = laptops.Where(m => m.Processor.Name== homeProductVM.ProcessorsName);
        //    }

        //    if (!String.IsNullOrEmpty(homeProductVM.LaptopCompaniesName))
        //    {
        //        laptops = laptops.Where(m => m.LaptopCompany.Name== homeProductVM.LaptopCompaniesName);
        //    }
        //    HomeProductVM final = new HomeProductVM() {
        //        Laptops = laptops.ToList(),
        //        LaptopCompanies = new SelectList(companyName),
        //        GetGraphics = new SelectList(companyName),
        //        Processors = new SelectList(companyName)
        //    };

        //    return View(final);
        //}

        public IActionResult Details(int id)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //session
            if (claim != null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, count);
            }

            var laptopsInDb = _unitOfWork.Laptop.FirstOrDefault(l => l.Id == id, includePropties: "LaptopCompany,Processor,GraphicsCard");

            if (laptopsInDb == null) return NotFound();
            var shoppingCart = new ShoppingCart()
            {
                Laptop = laptopsInDb,
                LaptopId = laptopsInDb.Id

            };
            if (claim != null)
            {
                shoppingCart.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value);
            }

            return View(shoppingCart);
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
                var shoppingCartInDb = _unitOfWork.ShoppingCart.FirstOrDefault(sc => sc.ApplicationUserId == claim.Value && sc.LaptopId == shoppingCart.LaptopId);
                if (shoppingCartInDb == null)
                    _unitOfWork.ShoppingCart.Add(shoppingCart);
                else
                    shoppingCartInDb.Count += shoppingCart.Count;
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                var laptopInDb = _unitOfWork.Laptop.FirstOrDefault(l => l.Id == shoppingCart.Id, includePropties: "LaptopCompany,Processor,GraphicsCard");
                if (laptopInDb == null) return NotFound();
                var shoppingCartEdit = new ShoppingCart()
                {
                    Laptop = laptopInDb,
                    LaptopId = laptopInDb.Id
                };
                return View(shoppingCartEdit);
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