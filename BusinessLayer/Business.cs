using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XYZShoppingMVC.DataLayer;
using XYZShoppingMVC.Models;

namespace XYZShoppingMVC.BusinessLayer
{
    public class Business
    {
        public List<SelectListItem> getCategory()
        {
            return Repository.getCategory();
        }

        public List<Products> getProductsbyCategory(int category)
        {
            return Repository.getProductsbyCategory(category);
        }

        public bool AddToCart(ProductsDetailVM pdvm)
        {
            if (Repository.ProductInStock(pdvm.Quantity, pdvm.Prod.ProductId) == false)
                throw new Exception("Product Out of Stock");
            return Repository.AddToCart(pdvm);
        }

        public Products GetProducts(int pid)
        {
            return Repository.GetProducts(pid);
        }

        public List<OrderDetails> PopulateCart()
        {
            return Repository.PopulateCart();
        }

        public bool UpdateOrder()
        {
            return Repository.UpdateOrder();
        }
    }
}