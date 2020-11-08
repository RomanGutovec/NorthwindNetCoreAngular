using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebUI.MVC.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "northwind-id")]
    public class NorthwindImageTagHelper : TagHelper
    {
        // PascalCase gets translated into kebab-case.
        public int NorthwindId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("href", $"images/{NorthwindId}");
        }
    }
}