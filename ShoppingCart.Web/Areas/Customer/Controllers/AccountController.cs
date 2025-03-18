using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index(int Type)
        {
             
            string customview = string.Empty;
            switch (Type)
            {
                case 1:
                    customview = "account_orders";
                    break;
                case 2:
                    customview = "account_settings";
                    var user = HttpContext.Session.Get<LoggedUser>(SessionUtilities.SessionCurrentUserkey);
                    return PartialView(customview, user);
                default:
                    break;
            }
            return PartialView(customview);
        }
        [HttpPost]
        public void UpdateUserAccount(Registration reg)
        {
            _unitOfWork.RegistrationRepository.Update(reg);
        }
    }
}
