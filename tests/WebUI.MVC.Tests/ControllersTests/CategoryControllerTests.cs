using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebUI.MVC.Controllers;
using Xunit;

namespace WebUI.MVC.Tests
{
    public class CategoryControllerTests
    {
        public class CategoriesControllerTests
        {
            private readonly Mock<IMediator> _mediatorMock;

            public CategoriesControllerTests()
            {
                _mediatorMock = new Mock<IMediator>();
                _mediatorMock.Setup(m =>
                        m.Send(It.IsAny<GetCategoriesListQuery>(),
                            It.IsAny<CancellationToken>()))
                    .Returns(GetTestCategories());
            }

            [Fact]
            public async Task Index_ReturnsAViewResult_WithAListOfCategories()
            {
                //Arrange
                var controller = new CategoryController(_mediatorMock.Object);

                //Act
                var result = await controller.Index();

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<CategoriesListViewModel>(viewResult.ViewData.Model);
                Assert.Equal(3, model.Categories.Count());
            }

            private Task<CategoriesListViewModel> GetTestCategories()
            {
                var categories = CreateCategoriesEntities();
                return Task.Run(() => categories);
            }


            private CategoriesListViewModel CreateCategoriesEntities()
            {
                return new CategoriesListViewModel {
                    Categories = new List<CategoryDto>
                    {
                        new CategoryDto {Id = 1, Name = "First", Description = "Some description here"},
                        new CategoryDto {Id = 2, Name = "Second", Description = "Some description here"},
                        new CategoryDto {Id = 3, Name = "Third", Description = "Some description here"}
                    }
                };
            }
        }
    }
}
