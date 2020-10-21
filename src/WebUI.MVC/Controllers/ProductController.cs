using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Products.Queries.ProductsList;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: ProductController
        public async Task<ActionResult<ProductsListViewModel>> Index()
        {
            var products = await _mediator.Send(new GetProductsListQuery());
            return View(products);
        }
    }
}
