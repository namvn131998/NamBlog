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

namespace ShoppingCart.Business.Repositories
{
    public class CategoryRepository :  Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Category category)
        {
            var objCate = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (objCate != null)
            {
                objCate.Name = category.Name;
                objCate.Description = category.Description;
                objCate.DisplayOrder = category.DisplayOrder;
            }
        }
        public IEnumerable<Category> GetListCategory()
        {
            return _context.Categories.Select(cate => new Category 
            {
                Id = cate.Id,
                Name = cate.Name,
            });
        }
    }
}
