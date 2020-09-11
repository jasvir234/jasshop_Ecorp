using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jasshop.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string CartId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Seller { get; set; }
        public string Buyer { get; set; }
    }
}
