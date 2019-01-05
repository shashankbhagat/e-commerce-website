﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XYZShoppingMVC.DataLayer;

namespace XYZShoppingMVC.BusinessLayer
{
    public class BusinessAuth
    {
        public static bool VerifyLogin(string Username, string Password)
        {
            return RepositoryAuth.VerifyLogin(Username, Password);
        }

        public static string GetRolesForUser(string Username)
        {
            return RepositoryAuth.GetRolesForUser(Username);
        }
    }
}