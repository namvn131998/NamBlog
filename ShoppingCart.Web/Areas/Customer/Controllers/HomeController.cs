using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DataAccess.Model;
using ShoppingCart.Business.Repositories;
using ShoppingCart.Models;
using System.Diagnostics;
using ShoppingCart.DataAccess.ViewModels;
using ShoppingCart.DataAccess.Constants.Enums;
using ShoppingCart.Business.Utilities;
using ShoppingCart.DataAccess.Helper;

namespace ShoppingCart.Web.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult _QuickView(int productID)
        {
            Product model = new Product();
            model = _unitOfWork.ProductRepository.GetT(p => p.Id == productID);
            return PartialView(model);
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
        public IActionResult Login() 
        {
            return View(new Registration());
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel log)
        {
            string message = "InvalidUserName";
            Registration registration = new Registration();
            LoginStatus loginStatus = _unitOfWork.RegistrationRepository.Login(log.Username, log.Password, out registration);
            if (registration != null)
            {
                switch (loginStatus)
                {
                    case LoginStatus.Pending:
                        message = "Your account is pending";
                        break;
                    case LoginStatus.Disabled:
                        message = "Your account is disabled";
                        break;
                    case LoginStatus.InvalidUserName:
                        message = "InvalidUserName";
                        ModelState.AddModelError("UserName", message);
                        break;
                    case LoginStatus.InvalidPassword:
                        message = "InvalidPassword";
                        ModelState.AddModelError("Password", message);
                        break;
                    default:
                        message = "LoginSuccess";
                        break;
                }
            }
            if (loginStatus == LoginStatus.Success)
            {
                var user = new LoggedUser(registration);
                HttpContext.Session.Set(SessionUtilities.SessionCurrentUserkey, user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        public IActionResult Register()
        {
            return View(new Registration());
        }
        [HttpPost]
        public IActionResult Register(Registration reg)
        {
            if (ModelState.IsValid)
            {
                reg.CreatedDate = DateTime.Now;
                reg.UserType = (int)UserType.User;
                reg.Status = (int)RegistrationStatus.Activated;
                if (reg.UserID == 0)
                {
                    _unitOfWork.RegistrationRepository.Add(reg);
                }
                _unitOfWork.Save();
                return RedirectToAction("Login");
            }
            else
                return Json(new { message = "Fail!" });
        }
    }
}