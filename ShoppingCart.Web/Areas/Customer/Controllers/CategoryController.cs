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
            if (product == null)
            {
                return BadRequest("Invalid product request");
            }

            var products = _unitOfWork.ProductRepository.GetProducts(product);

            var category = _unitOfWork.CategoryRepository.GetT(c => c.Id == product.CategoryId);
            ViewBag.CategoryName = category?.Name ?? "Unknown Category";  // Kiểm tra null
            ViewBag.CategoryId = product.CategoryId;
            ViewBag.PageSize = product.PageSize;
            ViewBag.minPrice = product.minPrice;
            ViewBag.maxPrice = product.maxPrice;

            return PartialView(products);
        }

    }
}
