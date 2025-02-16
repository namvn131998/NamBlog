using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using ShoppingCart.Business.Repositories;
using ShoppingCart.Business.Utilities;
using ShoppingCart.DataAccess.Model;



namespace ShoppingCart.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult ShopCart()
        {
            var cart = HttpContext.Session.GetCart<List<Cart>>(SessionUtilities.SessionCart) ?? new List<Cart>();
            return View(cart);
        }
        [HttpGet]
        public IActionResult AddTocart(Cart cart)
        {
            var product = _unitOfWork.ProductRepository.GetT(p => p.Id == cart.Id);
            var value = HttpContext.Session.GetCart<List<Cart>>(SessionUtilities.SessionCart);

            cart.Name = product.Name;
            cart.Price = product.Price;

            if (value == null)
            {
                HttpContext.Session.Set(SessionUtilities.SessionCart, cart);
            }
            else
            {
                for (int i = 0; i < value.Count; i++)
                {
                    if (value[i].Id != cart.Id)
                    {
                        value.Add(cart);
                        break;
                    }
                }
                HttpContext.Session.Set<List<Cart>>(SessionUtilities.SessionCart, value);
            }
            return Json(new { message = "OK" });
        }
        public IActionResult RemoveCartItem(int Id)
        {
            var value = HttpContext.Session.GetCart<List<Cart>>(SessionUtilities.SessionCart);
            for (int i = 0; i < value.Count; i++)
            {
                if (value[i].Id == Id)
                {
                    value.RemoveAt(i);
                    break;
                }
            }
            HttpContext.Session.Set<List<Cart>>(SessionUtilities.SessionCart, value);
            return Json(new { message = "OK" });
        }
    }
}
