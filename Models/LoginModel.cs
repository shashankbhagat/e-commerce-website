using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XYZShoppingMVC.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="{0} required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "{0} required")]
        public string Password { get; set; }
    }
}