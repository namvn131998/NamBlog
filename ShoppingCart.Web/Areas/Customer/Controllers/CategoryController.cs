using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Business.Repositories;
using ShoppingCart.Models.Category;
using ShoppingCart.Models.Product;

namespace ShoppingCart.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int categoryId)
        {
            ViewBag.CategoryId = categoryId;
            return View();
        }
        public IActionResult _Index(ProductListRequestModel product)
        {
            var products = _unitOfWork.ProductRepository.GetProducts(product);
            ViewBag.CategoryName = _unitOfWork.CategoryRepository.GetT(c => c.Id == product.CategoryId).Name;
            ViewBag.CategoryId = product.CategoryId;
            ViewBag.PageSize = product.PageSize;
            ViewBag.minPrice = product.minPrice;
            ViewBag.maxPrice = product.maxPrice;
            return PartialView(products);
        }
    }
}
