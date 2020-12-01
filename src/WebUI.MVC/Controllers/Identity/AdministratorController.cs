using System.Linq;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.MVC.Controllers.Identity
{
    [Authorize(Roles = "administrator")]
    public class AdministratorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministratorController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        
        public ActionResult Index()
        {
            var users = _userManager.Users.ToList<ApplicationUser>().Select(u=> (u.Id, u.Email));
            return View("~/Views/Identity/Administrator/Index.cshtml", users);
        }
    }
}
