using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XYZShoppingMVC.Models
{
    public class ProductsVM
    {
        public List<Products> pList { get; set; }
        public List<SelectListItem> cList { get; set; }
        public int catselected { get; set; }
    }
}