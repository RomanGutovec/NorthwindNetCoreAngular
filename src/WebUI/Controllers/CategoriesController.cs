using System;
using System.IO;
using System.Threading.Tasks;
using Application.Categories.Commands.UpdateCategory;
using Application.Categories.Queries.CategoriesList;
using Application.Categories.Queries.CategoryDetail;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<CategoriesListViewModel>> GetCategories()
        {
            var categories = await _mediator.Send(new GetCategoriesListQuery());
            return Ok(categories);
        }

        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var category = await _mediator.Send(new GetCategoryDetailQuery { Id = id });
            return File(category.Picture, "image/bmp");
        }

        [HttpPut("uploadimage/{id}")]
        public async Task<IActionResult> UpdateImage(int id, [FromForm] IFormFile uploadedFile)
        {
            var updateCommand = new UpdateCategoryCommand { Id = id };
            await using var stream = new MemoryStream();
            await uploadedFile.CopyToAsync(stream);
            updateCommand.Picture = stream.ToArray();

            try {
                await _mediator.Send(updateCommand);
                return NoContent();
            } catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}