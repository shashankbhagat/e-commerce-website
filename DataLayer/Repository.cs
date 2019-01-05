using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XYZShoppingMVC.Models;
using XYZShoppingMVC.Utils;

namespace XYZShoppingMVC.DataLayer
{
    public static class Repository
    {

        public static List<Products> getProductsbyCategory(int category)
        {
            List<Products> prodList = new List<Products>();
            try
            {
                string sql= "SELECT ProductId,productSDesc,ProductImage,Price,Instock,Inventory FROM  Products WHERE CATID = @category";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@category", System.Data.SqlDbType.Int);
                p1.Value = category;
                pList.Add(p1);
                DataTable dt = DataAccess.GetManyRowsCols(sql, pList);
                foreach(DataRow dr in dt.Rows)
                {
                    Products pr = new Products();
                    pr.ProductId = (int)dr["ProductId"];
                    pr.ProductSDesc = dr["ProductSDesc"].ToString();
                    pr.ProductImage = dr["ProductImage"].ToString();
                    pr.Price = (decimal)dr["Price"];
                    pr.InStock = (bool)dr["InStock"];
                    pr.Inventory = (int)dr["Inventory"];
                    prodList.Add(pr);
                }
            }
            catch(Exception)
            {
                throw;
            }
            return prodList;
        }

        public static List<SelectListItem> getCategory()
        {
            List<SelectListItem> catList = new List<SelectListItem>();
            try
            {
                string sql = "select * from ProductCategories";
                DataTable dt = DataAccess.GetManyRowsCols(sql,null);
                foreach (DataRow dr in dt.Rows)
                {
                    SelectListItem si = new SelectListItem();
                    si.Value = dr["CatId"].ToString();
                    si.Text = dr["CatDesc"].ToString();
                    catList.Add(si);
                }
            }
            catch(Exception)
            {
                throw;
            }
            return catList;
        }

        public static bool AddOrder()
        {
            string user = SessionFacade.LOGGEDIN;
            int UID = RepositoryAuth.GetUserID(user);
            bool res = false;
            try
            {
                string sql = "if not exists(select 1 from Orders ord inner join (select max(orderNo) maxOr from Orders where UserID="+UID + ")or1 on (or1.maxor=ord.orderno) where TotalCost=0 and TotalQty=0) insert into Orders(OrderDate,TotalQty,TotalCost,UserID) select getdate(),0,0," + UID;
                int rows = DataAccess.InsertUpdateDelete(sql, null);
                if (rows > 0)
                    res = true;
            }
            catch(Exception)
            {
                throw;
            }
            return res;
        }

        public static bool AddToCart(ProductsDetailVM pr)
        {
            bool res = false;
            int orderNo=0;
            try
            {
                orderNo = GetOrderNo();
                string sql = "insert into OrderDetails(OrderNo,ItemNo,ItemDesc,Qty,Price) select @OrderNo, @ItemNo, @ItemDesc, @qty,@Price";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@OrderNo", SqlDbType.Int);
                SqlParameter p2 = new SqlParameter("@ItemNo", SqlDbType.Int);
                SqlParameter p3 = new SqlParameter("@ItemDesc", SqlDbType.VarChar);
                SqlParameter p4 = new SqlParameter("@Qty", SqlDbType.Int);
                SqlParameter p5 = new SqlParameter("@Price", SqlDbType.Money);

                p1.Value = orderNo;
                p2.Value = pr.Prod.CatID;
                p3.Value = pr.Prod.ProductSDesc;
                p4.Value = pr.Quantity;
                p5.Value = pr.Prod.Price;

                pList.Add(p1);
                pList.Add(p2);
                pList.Add(p3);
                pList.Add(p4);
                pList.Add(p5);

                int rows = DataAccess.InsertUpdateDelete(sql, pList);
                if (rows > 0)
                    res = true;
            }
            catch(Exception)
            {
                throw;
            }
            return res;
        }

        public static bool ProductInStock(int quantity,int ProductId)
        {
            bool res = false;
            try
            {
                string sql = "select 1 from Products where ProductId=@ProductId and Inventory>=@quantity";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@ProductId", SqlDbType.Int);
                SqlParameter p2 = new SqlParameter("@quantity", SqlDbType.Int);
                p1.Value = ProductId;
                p2.Value = quantity;
                pList.Add(p1);
                pList.Add(p2);
                object obj = DataAccess.GetSingleAnswer(sql, pList);
                if (obj != null)
                    res = true;
            }
            catch(Exception)
            {
                throw;
            }
            return res;
        }

        public static int GetOrderNo()
        {
            Object obj = null;
            try
            {
                string sql = "select max(orderNo) maxOr from Orders where UserID=@UserID";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@UserID", SqlDbType.Int);
                p1.Value = Convert.ToInt32(RepositoryAuth.GetUserID(SessionFacade.LOGGEDIN));
                pList.Add(p1);
                obj = DataAccess.GetSingleAnswer(sql, pList);
            }
            catch(Exception)
            {
                throw;
            }
            return (int)obj;
        }

        public static Products GetProducts(int pid)
        {
            Products pr = new Products();
            try
            {
                string sql = "select * from Products where ProductId=@ProductId";
                List<SqlParameter> pList = new List<SqlParameter>();
                SqlParameter p1 = new SqlParameter("@ProductId", SqlDbType.Int);
                p1.Value = pid;
                pList.Add(p1);
                DataTable dt = DataAccess.GetManyRowsCols(sql, pList);
                foreach(DataRow dr in dt.Rows)
                {
                    pr.ProductId = (int)dr["ProductId"];
                    pr.CatID = (int)dr["CatID"];
                    pr.ProductSDesc = dr["ProductSDesc"].ToString();
                    pr.ProductLDesc = dr["ProductLDesc"].ToString();
                    pr.ProductImage = dr["ProductImage"].ToString();
                    pr.Price = (decimal)dr["Price"];
                    pr.InStock = (bool)dr["InStock"];
                    pr.Inventory = (int)dr["Inventory"];
                }
            }
            catch(Exception)
            {
                throw;
            }
            return pr;
        }

        public static List<OrderDetails> PopulateCart()
        {
            List<OrderDetails> odList = new List<OrderDetails>();
            try
            {
                int OrderNo = GetOrderNo();
                string sql = "select * from orderdetails where OrderNo=" + OrderNo;
                DataTable dt = DataAccess.GetManyRowsCols(sql, null);
                foreach(DataRow dr in dt.Rows)
                {
                    OrderDetails ordet = new OrderDetails();
                    ordet.OrderDetailID = (long)dr["OrderDetailID"];
                    ordet.OrderNo = (int)dr["OrderNo"];
                    ordet.ItemNo = (int)dr["ItemNo"];
                    ordet.ItemDesc = dr["ItemDesc"].ToString();
                    ordet.Qty = (int)dr["Qty"];
                    ordet.Price = (decimal)dr["Price"];
                    odList.Add(ordet);
                }
            }
            catch(Exception)
            {
                throw;
            }
            return odList;
        }

        public static bool UpdateOrder()
        {
            bool res = false;
            string user = SessionFacade.LOGGEDIN;
            int UID = RepositoryAuth.GetUserID(user);
            int OrderNo = GetOrderNo();
            try
            {
                string sql = "update ord set ord.totalqty=ordet.Qty, ord.totalcost=ordet.Price from orders ord inner join (select sum(Qty) Qty,sum(Price) Price,OrderNo from OrderDetails group by OrderNo)ordet on (ord.orderNo=ordet.OrderNo) where ord.UserID=" + UID + " and ord.OrderNo=" + OrderNo;
                int rows = DataAccess.InsertUpdateDelete(sql, null);
                if (rows > 0)
                    res = true;
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }
    }
}