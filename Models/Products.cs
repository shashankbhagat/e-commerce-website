using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XYZShoppingMVC.Models
{
    public class Products
    {
        public int ProductId { get; set; }
        public string ProductSDesc { get; set; }
        public string ProductLDesc { get; set; }
        public string ProductImage { get; set; } 
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public int Inventory { get; set; }
        public int Quantity { get; set; }
        public int CatID { get; set; }
    }
}