using System.Threading.Tasks;
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
    }
}
