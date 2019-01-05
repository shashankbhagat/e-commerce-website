using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XYZShoppingMVC.Utils
{
    public class Utils
    {
        public Utils()
        {
        }

        public static string StripPunctuation(string inp)
        {
            string inp2 = inp.Replace("'", "''");
            inp2 = inp2.Replace(";", "");
            inp2 = inp2.Replace("-", "");
            inp2 = inp2.Replace("%", "");
            inp2 = inp2.Replace("*", "");
            return inp2;
        }
    }
}