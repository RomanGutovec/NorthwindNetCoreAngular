using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebUI.MVC.HtmlHelpers
{
    public static class NorthwindImageExtensions
    {
        public static HtmlString NorthwindImageLink(this IHtmlHelper htmlHelper, int imageId, string linkText)
        {
            var result = $"<a href=images/{imageId}>{linkText}</a>";
            return new HtmlString(result);
        }
    }
}