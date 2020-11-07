using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUI.MVC.Models;

namespace WebUI.MVC.ViewComponents
{
    [ViewComponent(Name = "Breadcrumb")]
    public class BreadcrumbViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var breadcrumbModel = new BreadcrumbViewModel
            {
                Controller = ViewContext.RouteData.Values["controller"].ToString(),
                Action = ViewBag.Action = ViewContext.RouteData.Values["action"].ToString()
            };

            return await Task.FromResult(View("~/Views/Shared/Components/_Breadcrumb.cshtml", breadcrumbModel));
        }
    }
}
