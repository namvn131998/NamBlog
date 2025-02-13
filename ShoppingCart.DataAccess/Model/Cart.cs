using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Model
{
    public class CartJson
    {
        [JsonProperty("Cart")]
        public Cart Cart { get; set; }
    }
    public class Cart
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Photo { get; set; }
        public int Quantity { get; set; }
        public string Weight { get; set; }
    }
}
