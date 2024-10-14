using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.DataAccess.Data;
using ShoppingCart.Models;
using ShoppingCart.DataAccess.Model;
using ShoppingCart.Business.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using ShoppingCart.Models.Product;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace ShoppingCart.Business.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Product product)
        {
            var objPro = _context.Products.FirstOrDefault(p => p.Id == product.Id);
            if (objPro != null)
            {
                objPro.Name = product.Name;
                objPro.Description = product.Description;
                objPro.Price = product.Price;
            }
        }
        public void UpdateMediaID(int productID, string mediaIDs)
        {
            var objPro = _context.Products.FirstOrDefault(p => p.Id == productID);
            if (objPro != null)
            {
                objPro.MediaIds = mediaIDs;
            }
        }
        public IPagedList<Product> GetProducts(ProductListRequestModel request)
        {
            var products =  _context.Products
                .Where(p => p.Price >= request.minPrice && p.Price <= request.maxPrice)
                .Where(p => p.CategoryId == request.CategoryId)
                .OrderBy(p => p.CreatedDate).ToPagedList(request.Page, request.PageSize);
            switch (request.sortBy)
            {
                case "1":
                    products = products.OrderBy(p => p.Price).ToPagedList(request.Page, request.PageSize);
                    break;
                case "2":
                    products = products.OrderByDescending(p => p.Price).ToPagedList(request.Page, request.PageSize);
                    break;
                case "3":
                    products = products.OrderByDescending(p => p.CreatedDate).ToPagedList(request.Page, request.PageSize);
                    break;
            }
            return products;
        }
    }
}
