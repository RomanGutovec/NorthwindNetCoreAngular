using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.MVC.Models.Identity
{
    public class ForgotPasswordViewModel
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Code { get; set; }
    }
}
