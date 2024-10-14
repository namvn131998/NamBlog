using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.DataAccess.Model;
using ShoppingCart.Business.Repositories;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models.Product;
using X.PagedList;

namespace ShoppingCart.Business.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
        void UpdateMediaID(int productID, string mediaID);
        IPagedList<Product> GetProducts(ProductListRequestModel request);
    }
}
