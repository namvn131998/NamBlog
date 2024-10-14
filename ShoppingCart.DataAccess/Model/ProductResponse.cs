using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Model
{
    public class ProductResponse
    {
        public List<Product> products { get; set; }
        public int pageCount { get; set; }  
        public int currentPage { get; set; }
    }
}
