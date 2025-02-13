using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Business.Repositories;
using ShoppingCart.DataAccess.ViewModels;
using ShoppingCart.DataAccess.Model;
using ShoppingCart.Models.Product;
using ShoppingCart.DataAccess.Constants.Enums;
using ShoppingCart.Business.Utilities;
using ShoppingCart.DataAccess.Helper;
using X.PagedList;
using System.IO;

namespace ShoppingCart.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var user = HttpContext.Session.Get<LoggedUser>(SessionUtilities.SessionCurrentUserkey);
            return View();
        }
        [HttpGet]
        public IActionResult CreateOrUpdate(int? id)
        {
            Product product = new Product();
            if (id == null || id == 0)
            {
                return View(product);
            }
            else
            {
                product = _unitOfWork.ProductRepository.GetT(x => x.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(product);
                }
            }
        }
        [HttpPost]
        public IActionResult CreateOrUpdate(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.Id == 0)
                {
                    _unitOfWork.ProductRepository.Add(product);
                }
                else
                {
                    _unitOfWork.ProductRepository.Update(product);
                }
                _unitOfWork.Save();
                int productID = product.Id;
                return Json(new { result = "OK",productID = productID });
            }
            return Json(new { result = "Fail" });
        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var product = _unitOfWork.ProductRepository.GetT(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Save();
            return Json(new { result = "OK" });
        }
        [HttpGet]
        public IActionResult _List(CartListRequestModel product)
        {
            ProductVM productVM = new ProductVM();
            if (string.IsNullOrEmpty(product.searchValue))
                productVM.Products = _unitOfWork.ProductRepository.GetAll();
            else
                productVM.Products = _unitOfWork.ProductRepository.GetAll(s => s.Name.Contains(product.searchValue));
            switch (product.sortBy)
            {
                case "Name":
                    if (product.sortDirection == "DESC")
                        productVM.Products = productVM.Products.OrderByDescending(x => x.Name);
                    else
                        productVM.Products = productVM.Products.OrderBy(x => x.Name);
                    break;
                default:
                    break;
            }
            return PartialView(productVM.Products.ToPagedList(product.Page, product.PageSize));
        }
    }
}
