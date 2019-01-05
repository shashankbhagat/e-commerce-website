using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XYZShoppingMVC.Utils
{
    public class MyBasePage:System.Web.UI.Page
    {
        public bool requireSSL = false;

        public MyBasePage()
        {
        }

        void ChangeURL()
        {
            if (requireSSL && (Request.IsSecureConnection == false))
                Response.Redirect(Request.Url.ToString().Replace("http://", "https://"));
            if (!requireSSL && (Request.IsSecureConnection == true))
                Response.Redirect(Request.Url.ToString().Replace("https://", "http://"));
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ChangeURL();
        }
    }
}