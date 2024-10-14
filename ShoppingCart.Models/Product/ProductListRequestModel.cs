using Microsoft.AspNetCore.Mvc.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Models.Product
{
    public class ProductListRequestModel : BaseListRequestModel
    {
        public ProductListRequestModel() :base()
        {
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public long minPrice { get; set; }
        public long maxPrice { get; set; }
        public string  ImageUrl { get; set; }
        public int  CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
