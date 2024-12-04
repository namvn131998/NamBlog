using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DataAccess.Model;
using ShoppingCart.Business.Repositories;
using ShoppingCart.Models;
using System.Diagnostics;
using ShoppingCart.DataAccess.ViewModels;
using ShoppingCart.DataAccess.Constants.Enums;

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