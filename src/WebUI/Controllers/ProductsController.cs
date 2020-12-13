using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.DeleteProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Queries.ProductsList;
using FluentValidation;
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
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            try {
                var productId = await _mediator.Send(product);
                return Ok(productId);
            } catch (ValidationException ex) {
                return BadRequest(ex.Errors.ToList().Select(x => x.ErrorMessage));
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductCommand product)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            try {
                await _mediator.Send(product);
                return NoContent();
            } catch (NotFoundException ex) {
                return NotFound(ex.Message);
            } catch (ValidationException ex) {
                return BadRequest(ex.Errors.ToList().Select(x => x.ErrorMessage));
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try {
                var products = await _mediator.Send(new DeleteProductCommand { ProductId = id });
                return Ok(products);
            } catch (NotFoundException ex) {
                return NotFound(ex.Message);
            }
        }
    }
}