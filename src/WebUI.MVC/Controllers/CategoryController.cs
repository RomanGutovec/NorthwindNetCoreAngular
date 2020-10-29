using System.Threading.Tasks;
using Application.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: CategoryController
        public async Task<IActionResult> Index()
        {
            var categories =await _mediator.Send(new GetCategoriesListQuery());
            return View(categories);
        }
    }
}
