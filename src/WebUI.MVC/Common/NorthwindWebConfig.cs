using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace WebUI.MVC.Common
{
    public class NorthwindWebConfig : INorthwindConfig
    {
        private readonly IConfiguration _configuration;

        public NorthwindWebConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int AmountOfProductsFromQuery { get; set; }
        public int AmountOfProducts =>
            AmountOfProductsFromQuery == 0
                ? _configuration.GetValue("NorthwindVariables:ProductsAmount", 0)
                : AmountOfProductsFromQuery;
    }
}