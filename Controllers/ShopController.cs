using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XYZShoppingMVC.DataLayer;
using XYZShoppingMVC.Models;
using XYZShoppingMVC.Utils;

namespace XYZShoppingMVC.Controllers
{
    public class ShopController : Controller
    {
        BusinessLayer.Business _bus = new BusinessLayer.Business();
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }

        //[Authorize(Roles ="Customer")]
        public ActionResult ShowProducts()
        {
            string custId = SessionFacade.LOGGEDIN;
            Console.WriteLine(custId);
            //if (custId == null)
            //    return RedirectToAction("Login", "Auth");
            ProductsVM pvm = new ProductsVM();
            pvm.cList = _bus.getCategory();
            pvm.catselected = Convert.ToInt32(pvm.cList[0].Value);
            pvm.pList = _bus.getProductsbyCategory(pvm.catselected);
            return View(pvm);
        }

        [HttpPost]
        public ActionResult ShowProducts(ProductsVM pvm)
        {
            string custId = SessionFacade.LOGGEDIN;
            Console.WriteLine(custId);
            if (custId == null)
                return RedirectToAction("Login", "Auth");
            pvm.cList = _bus.getCategory();            
            pvm.pList = _bus.getProductsbyCategory(pvm.catselected);
            return View(pvm);
        }

        public ActionResult Details(int id)
        {
            //bring in product details  using id
            ProductsDetailVM pdvm = new ProductsDetailVM();
            pdvm.Prod = _bus.GetProducts(id);
            Repository.AddOrder();
            return View(pdvm);
        }

        [HttpPost]
        public ActionResult Details(ProductsDetailVM pdvm,int id)
        {
            pdvm.Prod = _bus.GetProducts(id);
            bool res = false;
            if(ModelState.IsValid)
            {
                res = _bus.AddToCart(pdvm);
                if(res==true)
                {
                    ViewBag.msg = "Product added to cart";
                    bool result = false;
                    result = _bus.UpdateOrder();
                    if (result == true)
                        return RedirectToAction("ShowProducts");
                    else
                        ViewBag.msg = "Unable to update order";
                }
                else
                    ViewBag.msg = "Unable to add to cart";
            }
            return View(pdvm);
        }

        public ActionResult Cart()
        {
            OrderDetails ordet = new OrderDetails();
            List<OrderDetails> odList = new List<OrderDetails>();
            odList = _bus.PopulateCart();
            return View(odList);
        }

        [HttpPost]
        public ActionResult Cart(List<OrderDetails> odList)
        {
            odList = _bus.PopulateCart();
            return RedirectToAction("ConfirmOrder", "Shop");
        }

        public ActionResult ConfirmOrder()
        {
            OrderDetails ordet = new OrderDetails();
            List<OrderDetails> odList = new List<OrderDetails>();
            odList = _bus.PopulateCart();
            return View(odList);
        }

        [HttpPost]
        public ActionResult ConfirmOrder(List<OrderDetails> odList)
        {   
            odList = _bus.PopulateCart();
            return RedirectToAction("Thankyou", "Shop");
        }

        public ActionResult Thankyou()
        {
            return View();
        }
    }
}