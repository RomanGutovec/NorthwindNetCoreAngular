using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.MVC.Models.Identity
{
    public class UserRegistrationModel
    {
        [Display(Name ="First name")]
        public string FirstName { get; set; }
        
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name ="Confirm password")]
        public string PasswordConfirmation { get; set; }
    }
}
