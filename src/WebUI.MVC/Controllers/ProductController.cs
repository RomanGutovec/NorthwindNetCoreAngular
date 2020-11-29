using System.Linq;
using System.Threading.Tasks;
using Application.Categories.Queries;
using Application.Categories.Queries.CategoriesList;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Queries.ProductDetail;
using Application.Products.Queries.ProductsList;
using Application.Suppliers;
using Application.Suppliers.Queries.SuppliersList;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUI.MVC.Helpers;
using WebUI.MVC.Models;

namespace WebUI.MVC.Controllers
{

    public class ProductController : Controller
    {
        private readonly IMediator _mediator;
        private readonly INorthwindConfig _northwindConfig;
        private readonly IMapper _mapper;


        public ProductController(IMediator mediator, INorthwindConfig northwindConfig, IMapper mapper)
        {
            _mediator = mediator;
            _northwindConfig = northwindConfig;
            _mapper = mapper;
        }

        // GET: ProductController
        [Authorize]
        public async Task<IActionResult> Index([FromQuery] int max = 0)
        {
            _northwindConfig.AmountOfProductsFromQuery = max;
            var products = await _mediator.Send(new GetProductsListQuery {AmountOfProducts = _northwindConfig.AmountOfProducts});
            return View(products);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Create()
        {
            await SetUpSelectLists();

            return View(new CreateProductCommand());
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductCommand createProduct)
        {
            if (!ModelState.IsValid) {

                return BadRequest(ModelState);
            }

            try {
                await _mediator.Send(createProduct);
                TempData.Put("UserMessage", new MessageViewModel { Title = "Success", CssClassName = "alert-success", Message = "Operation done" });
            } catch (ValidationException) {
                TempData.Put("UserMessage", new MessageViewModel { Title = "Error", CssClassName = "alert alert-warning", Message = "Operation failed" });
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var productToUpdateCommand = new UpdateProductCommand();
            try {
                var productDetail = await _mediator.Send(new GetProductDetailQuery { ProductId = id });
                _mapper.Map(productDetail, productToUpdateCommand);
            } catch (NotFoundException) {
                return View("ProductNotFound", id);
            }

            await SetUpSelectLists(productToUpdateCommand.CategoryId, productToUpdateCommand.SupplierId);

            return View(productToUpdateCommand);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateProductCommand updateProduct)
        {
            if (!ModelState.IsValid || id != updateProduct.ProductId) {

                return BadRequest(ModelState);
            }

            try {
                await _mediator.Send(updateProduct);
                TempData.Put("UserMessage", new MessageViewModel { Title = "Success", CssClassName = "alert-success", Message = "Operation done" });
            } catch (ValidationException) {
                TempData.Put("UserMessage", new MessageViewModel { Title = "Error", CssClassName = "alert alert-warning", Message = "Operation failed" });
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task SetUpSelectLists(int? selectedCategoryId = null, int? selectedSupplierId = null)
        {
            var categories = await _mediator.Send(new GetCategoriesListQuery());
            var suppliers = await _mediator.Send(new GetSuppliersListQuery());
            ViewBag.Categories = new SelectList(categories.Categories, nameof(CategoryDto.Id),
                nameof(CategoryDto.Description),
                selectedValue: categories.Categories.Where(x => x.Id == selectedCategoryId));
            ViewBag.Suppliers = new SelectList(suppliers.Suppliers, nameof(SupplierDto.Id),
                nameof(SupplierDto.CompanyName),
                selectedValue: suppliers.Suppliers.Where(x => x.Id == selectedSupplierId));
        }
    }
}
