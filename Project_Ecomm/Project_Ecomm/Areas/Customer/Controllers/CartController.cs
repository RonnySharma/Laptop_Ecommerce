using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Project_Ecomm_DataAccess.Repository.IRepository;
using Project_Ecomm_Models;
using Project_Ecomm_Models.ViewModels;
using Project_Ecomm_Uitility;
using Stripe;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace Project_Ecomm.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private static bool isEmailConfirm = false;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        public CartController(IUnitOfWork unitOfWork,IEmailSender emailSender,UserManager<IdentityUser>userManager)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _userManager = userManager;
        }
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public IActionResult Index()
        {
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var Claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claim == null)
            {
                ShoppingCartVM =new ShoppingCartVM()
                {
                    ListCart=new List <ShoppingCart>()
                };
                return View(ShoppingCartVM);
            }
            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.shoppingCart.GetAll(sc => sc.ApplicationUserId == Claim.Value, IncludeProperties: "Product"),
                OrderHeader = new OrderHeader()
            };
            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.applicationUser.FristOrDefault(u => u.Id == Claim.Value);
            foreach (var list in ShoppingCartVM.ListCart)
            {
                list.Price = SD.GetPriceBasedOnQuantity(list.Count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (list.Price * list.Count);
                if (list.Product.Description.Length > 100)
                {
                    list.Product.Description = list.Product.Description.Substring(0, 99) + "....";
                }
            }
            if (!isEmailConfirm)
            {
                ViewBag.EmailMessage = "verify your email !!";
                ViewBag.EmailCss = "text-success";
                isEmailConfirm = false;
            }
            else
            {
                ViewBag.EmailMessage = "email Confirm for authorize customer";
                ViewBag.EmailCss = "text-danger";
            }
            return View(ShoppingCartVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ActionName("Index")]
        public async Task<IActionResult> Indexpost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.applicationUser.FristOrDefault(u => u.Id == Claim.Value);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email is Empty");
            }
            else
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,

                    values: new { area = "Identity", userId = userId, code = code},
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                isEmailConfirm = true;
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult plus(int id)
        {
            var Cart = _unitOfWork.shoppingCart.FristOrDefault(sc => sc.Id == id);
            Cart.Count += 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult minus(int id)
        {
            var Cart = _unitOfWork.shoppingCart.FristOrDefault(sc => sc.Id == id);
            if (Cart.Count == 1)
                Cart.Count = 1;
            else
                Cart.Count -= 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult delete(int id)
        {
            var Cart = _unitOfWork.shoppingCart.FristOrDefault(sc => sc.Id == id);
            _unitOfWork.shoppingCart.Remove(Cart);
            _unitOfWork.Save();

            //session
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var Claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claim != null)
            {
                var Count = _unitOfWork.shoppingCart.GetAll(sc => sc.ApplicationUserId == Claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, Count);
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Summary()
        {
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var Claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.shoppingCart.GetAll(sc => sc.ApplicationUserId == Claim.Value, IncludeProperties: "Product"),
                OrderHeader = new OrderHeader()
            };
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.applicationUser.FristOrDefault(u => u.Id == Claim.Value);
            foreach (var list in ShoppingCartVM.ListCart)
            {
                list.Price = SD.GetPriceBasedOnQuantity(list.Count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (list.Count * list.Price);
                list.Product.Description = SD.ConvertToRawHtml(list.Product.Description);
            }
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            return View(ShoppingCartVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost(string stripeToken)
        {
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var Claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.applicationUser.FristOrDefault(u => u.Id == Claim.Value);
            ShoppingCartVM.ListCart = _unitOfWork.shoppingCart.GetAll(sc => sc.ApplicationUserId == Claim.Value, IncludeProperties: "Product");

            ShoppingCartVM.OrderHeader.Carrier = "";
            ShoppingCartVM.OrderHeader.TrackingNumber = "";
            ShoppingCartVM.OrderHeader.TransactionId = "";
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusPending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = Claim.Value;
            _unitOfWork.orderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();
            foreach (var list in ShoppingCartVM.ListCart)
            {
                list.Price = SD.GetPriceBasedOnQuantity(list.Count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId=list.ProductId,
                    OrderHeaderId=ShoppingCartVM.OrderHeader.Id,
                    Price=list.Price,
                    Count=list.Count
                };
                ShoppingCartVM.OrderHeader.OrderTotal += (list.Price * list.Count);
                _unitOfWork.orderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }
            _unitOfWork.shoppingCart.RemoveRange(ShoppingCartVM.ListCart);
            _unitOfWork.Save();
            //session
            HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, 0);
            #region Stripe Process
            if (stripeToken == null)
            {
                ShoppingCartVM.OrderHeader.PaymentDueDate = DateTime.Today.AddDays(30);
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusApproved;
            }
            else
            {
                //payment process
                var option = new ChargeCreateOptions()
                {
                    Amount = Convert.ToInt32(ShoppingCartVM.OrderHeader.OrderTotal),
                    Currency = "usd",
                    Description = "Order Id:" + ShoppingCartVM.OrderHeader.Id,
                    Source = stripeToken
                };
                //payment
                var service = new ChargeService();
                Charge charge = service.Create(option);
                if (charge.BalanceTransactionId == null)
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                else
                    ShoppingCartVM.OrderHeader.TransactionId = charge.BalanceTransactionId;
                if (charge.Status.ToLower() == "succeeded")
                {
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    ShoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusApproved;
                    ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
                }
                _unitOfWork.Save();
            }

            #endregion
            return RedirectToAction("OrderConfirmation", "Cart", new { id = ShoppingCartVM.OrderHeader.Id });
        }
        public IActionResult OrderConfirmation(int Id)
        {
            return View(Id);
        }
    }
}
