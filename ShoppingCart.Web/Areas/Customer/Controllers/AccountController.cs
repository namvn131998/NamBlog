using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingCart.Business.Repositories;
using ShoppingCart.Business.Utilities;
using ShoppingCart.DataAccess.Helper;
using ShoppingCart.DataAccess.Model;

namespace ShoppingCart.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AccountController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Account_Settings()
        {
            var id = HttpContext.Session.Get<LoggedUser>(SessionUtilities.SessionCurrentUserkey)?.UserId;
            if (id == null)
            {
                // Handle the case where the user is not logged in or the session is null
                return RedirectToAction("Index");
            }

            var user = _unitOfWork.RegistrationRepository.GetUserByID(id.Value);
            return PartialView(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // protect the CSRF's attack
        public void UpdateUserAccount(Registration reg)
        {
            _unitOfWork.RegistrationRepository.Update(reg);
            _unitOfWork.Save();

            TempData["SuccessMessage"] = "Updated successful!";
        }
    }
}
