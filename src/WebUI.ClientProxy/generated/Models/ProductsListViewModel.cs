// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace NrothwindClientProxy.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class ProductsListViewModel
    {
        /// <summary>
        /// Initializes a new instance of the ProductsListViewModel class.
        /// </summary>
        public ProductsListViewModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ProductsListViewModel class.
        /// </summary>
        public ProductsListViewModel(IList<ProductDto> products = default(IList<ProductDto>))
        {
            Products = products;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "products")]
        public IList<ProductDto> Products { get; set; }

    }
}
