using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XYZShoppingMVC.Models
{
    public class OrderDetails
    {
        public long OrderDetailID { get; set; }
        public int OrderNo { get; set; }
        public int ItemNo { get; set; }
        public string ItemDesc { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }
}