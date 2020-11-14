using System.Threading.Tasks;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.DeleteProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Queries.ProductsList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ProductsListViewModel>> GetProducts()
        {
            var products = await _mediator.Send(new GetProductsListQuery());
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateProduct(CreateProductCommand product)
        {
            var productId = await _mediator.Send(product);
            return Ok(productId);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductCommand product)
        {
            await _mediator.Send(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var products = await _mediator.Send(new DeleteProductCommand { ProductId = id });
            return Ok(products);
        }
    }
}