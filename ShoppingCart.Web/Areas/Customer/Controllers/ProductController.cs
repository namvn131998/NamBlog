using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using ShoppingCart.Business.Repositories;
using ShoppingCart.DataAccess.Model;
using ShoppingCart.Models.Product;

namespace ShoppingCart.Web.Areas.Customer.Controllers
{
    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public  IPagedList<Product> GetProductsOfCategory(ProductListRequestModel request)
        {
            var products =  _unitOfWork.ProductRepository.GetProducts(request);
            return products;
        }
    }
}
