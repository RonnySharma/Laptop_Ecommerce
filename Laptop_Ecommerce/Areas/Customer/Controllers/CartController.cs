using Laptop_Ecommerce.DataAccess.Rpository.IRepository;
using Laptop_Ecommerce.Models;
using Laptop_Ecommerce.Models.ViewModels;
using Laptop_Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;
using Laptop_Ecommerce.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Laptop_Ecommerce.Areas.Customer.Controllers
{
        [Area("Customer")]
        [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private static bool isEmailConfirm = false;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _userManager = userManager;
        }
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim==null)
            {
                ShoppingCartVM = new ShoppingCartVM()
                {
                    ListCart = new List<ShoppingCart>()
                };
                return View(ShoppingCartVM);
            }
            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == claim.Value, includePropties: "Laptop"),
                OrderHeader = new OrderHeader()
            };
            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value, includePropties: "Company");
            foreach (var list in ShoppingCartVM.ListCart)
            {
                list.Price = SD.GetPriceBasedOnQuentity(list.Count, list.Laptop.Price, list.Laptop.Price10, list.Laptop.ListPrice20);
                ShoppingCartVM.OrderHeader.OrderTotal += (list.Price * list.Count);
                if(list.Laptop.Description.Length>100)
                {
                    list.Laptop.Description = SD.ConverToRawHtml(list.Laptop.Description);
                    list.Laptop.Description = list.Laptop.Description.Substring(0, 99) + "...";
                }
            }
            //********
            if (!isEmailConfirm)
            {
                ViewBag.EmailMessage = "Email has been sent kindly varify your email address!!!";
                ViewBag.EmailCSS = "text-success";
                isEmailConfirm = false;
            }
            else
            {
                ViewBag.EmailMessage = "Email must be confirmed for authorized customer!!!";
                ViewBag.EmailCSS = "text-danger";
            }
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email is Empty!!!");
            }
            else
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                isEmailConfirm = true;
            }
            return RedirectToAction("Index");
        }

        public IActionResult plus(int id)
        {
            var cart = _unitOfWork.ShoppingCart.FirstOrDefault(c => c.Id == id);
            cart.Count += 1;
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        public IActionResult minus(int id)
        {
            var cart = _unitOfWork.ShoppingCart.FirstOrDefault(c => c.Id == id);
            if (cart.Count == 1)
                 cart.Count = 1;
            else
                cart.Count -= 1;
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var cart = _unitOfWork.ShoppingCart.FirstOrDefault(c => c.Id == id);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            //session
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim!=null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, count);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Summary(int[] isChecked)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            {
                ShoppingCartVM = new ShoppingCartVM()
                {
                    ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includePropties: "Laptop").Where(x=>isChecked.Contains(x.Id)),
                    OrderHeader = new OrderHeader()
                };
                ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value);
                foreach (var list in ShoppingCartVM.ListCart)
                {
                    list.Price = SD.GetPriceBasedOnQuentity(list.Count, list.Laptop.Price, list.Laptop.Price10, list.Laptop.ListPrice20);
                    ShoppingCartVM.OrderHeader.OrderTotal += (list.Count * list.Price);
                    list.Laptop.Description = SD.ConverToRawHtml(list.Laptop.Description);
                    if(list.Laptop.Description.Length>100)
                    {
                        list.Laptop.Description = list.Laptop.Description.Substring(0, 99) + "...";
                    }
                }
                ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
                ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
                ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
                ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
                ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
                ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            }
            return View(ShoppingCartVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost(string stripeToken, int[] Id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(au => au.Id == claim.Value);
            ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll(lc => lc.ApplicationUserId == claim.Value, includePropties: "Laptop").Where(x=>Id.Contains(x.Id));

            //OrderHeader Table
            ShoppingCartVM.OrderHeader.Carrier = "";
            ShoppingCartVM.OrderHeader.TransactionId = "";
            ShoppingCartVM.OrderHeader.TrackingNumber = "";

            ShoppingCartVM.OrderHeader.PaymentDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusPending;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;
            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var list in ShoppingCartVM.ListCart)
            {
                list.Price = SD.GetPriceBasedOnQuentity(list.Count, list.Laptop.Price, list.Laptop.Price10, list.Laptop.ListPrice20);
                OrderDetails orderDetails = new OrderDetails()
                {
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    LaptopId = list.LaptopId,
                    Price = list.Price,
                    Count = list.Count
                };
                ShoppingCartVM.OrderHeader.OrderTotal += (list.Price * list.Count);
                _unitOfWork.OrderDetails.Add(orderDetails);
                _unitOfWork.Save();
                //Quantity
                  var quantity=_unitOfWork.Laptop.Get(list.LaptopId);
                quantity.Quantity =quantity.Quantity-list.Count;
                _unitOfWork.Laptop.Update(quantity);
                _unitOfWork.Save();
            }
            _unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
            _unitOfWork.Save();
            
            //session
            HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, 0);

            #region Stripe Process
            if(stripeToken==null)
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayPayment;
                ShoppingCartVM.OrderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
                ShoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusApproved;
            }
            else
            {
                //PaymentProcess
                var options = new ChargeCreateOptions()
                {
                    Amount = Convert.ToInt32(ShoppingCartVM.OrderHeader.OrderTotal),
                    Currency = "USD",
                    Description = "Order Id:" + ShoppingCartVM.OrderHeader.Id,
                    Source = stripeToken
                };
                var service = new ChargeService();
                Charge charge = service.Create(options);

                //Payment
                if(charge.BalanceTransactionId==null)
                {
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                }
                else
                {
                    ShoppingCartVM.OrderHeader.TransactionId = charge.BalanceTransactionId;
                    ShoppingCartVM.OrderHeader.PaymentDate = DateTime.Now;
                }

                if(charge.Status.ToLower()=="succeeded")
                {
                    ShoppingCartVM.OrderHeader.OrderStatus = SD.OrderStatusApproved;
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
                }
                _unitOfWork.Save();
            }
            #endregion

            return RedirectToAction("OrderConfirmation", "Cart", new { id = ShoppingCartVM.OrderHeader.Id });
        }
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //session
            if (claim != null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, count);
            }
            var user = _unitOfWork.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value);
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            await _emailSender.SendEmailAsync(user.Email, "Your Order's Status:",
                $"Your Order Has Been Confirmed!!!");

            //Twilio
            string accountSid = "AC9c7384659ba40c79f002b30d5c535ff5";
            string authToken = "77aefeb5df4b0b1a779b522cbb040345";
            var phoneNumber1 = _unitOfWork.ApplicationUser.FirstOrDefault(x => x.Id == claim.Value);
            string phoneNumber = phoneNumber1.PhoneNumber;
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body:"Your Order is Confirmed And Your Order's id is:"+id,
                from:new Twilio.Types.PhoneNumber("+13203825494"),
                to:phoneNumber
                );

            return View(id);
        }
    }
}
