using System.IO;
using System.Threading.Tasks;
using Application.Categories.Commands.UpdateCategory;
using Application.Categories.Queries.CategoriesList;
using Application.Categories.Queries.CategoryDetail;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebUI.MVC.Filters;

namespace WebUI.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [TypeFilter(typeof(LogActionFilter))]
        public async Task<IActionResult> Index()
        {
            var categories = await _mediator.Send(new GetCategoriesListQuery());
            return View(categories);
        }

        [HttpGet]
        [TypeFilter(typeof(LogActionFilter), Arguments = new object[] { true })]
        public async Task<FileContentResult> GetImage(int id)
        {
            var categoryDetail = await _mediator.Send(new GetCategoryDetailQuery { Id = id });

            return File(categoryDetail.Picture, "image/bmp");
        }

        [HttpGet]
        public IActionResult Images(int id)
        {
            return View("CategoryImageEdit", id);
            
        }

        [HttpPost]
        public async Task<IActionResult> Images(int categoryId, IFormFile uploadedFile)
        {

            var categoryToUpdate = new UpdateCategoryCommand();
            categoryToUpdate.Id = categoryId;

            await using (var memoryStream = new MemoryStream()) {
                await uploadedFile.CopyToAsync(memoryStream);

                if (memoryStream.Length < 2097152) {
                    categoryToUpdate.Picture = new byte[memoryStream.Length];
                    memoryStream.ToArray().CopyTo(categoryToUpdate.Picture, 0);

                    await _mediator.Send(categoryToUpdate);
                } else {
                    ModelState.AddModelError("File", "The file is too large.");
                }
            }

            return RedirectToAction("Index");
        }
    }
}
