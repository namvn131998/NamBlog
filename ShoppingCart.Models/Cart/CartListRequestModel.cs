using Microsoft.AspNetCore.Mvc.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Models.Product
{
    public class CartListRequestModel : BaseListRequestModel
    {
        public CartListRequestModel() :base()
        {
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public long Price { get; set; }
        public string  ImageUrl { get; set; }
        public int quantity { get; set; }
        public string Weight    { get; set; }

    }
}
