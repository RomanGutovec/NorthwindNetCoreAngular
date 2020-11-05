using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Categories.Queries;
using Application.Categories.Queries.CategoriesList;
using Application.Common.Interfaces;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Queries;
using Application.Products.Queries.ProductsList;
using Application.Suppliers;
using Application.Suppliers.Queries.SuppliersList;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using WebUI.MVC.Controllers;
using Xunit;

namespace WebUI.MVC.Tests
{
    public class ProductControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<INorthwindConfig> _configMock;
        private readonly Mock<IMapper> _mapperMock;

        public ProductControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductsListQuery>(), It.IsAny<CancellationToken>()))
                .Returns(GetTestProducts());
            _configMock = new Mock<INorthwindConfig>();
            _configMock.Setup(c => c.AmountOfProducts)
                .Returns(10);
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfProducts()
        {
            //Arrange
            var controller = new ProductController(_mediatorMock.Object, _configMock.Object, _mapperMock.Object);

            //Act
            var result = await controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductsListViewModel>(viewResult.ViewData.Model);
            _mediatorMock.Verify(mocks => mocks.Send(It.IsAny<GetProductsListQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(4, model.Products.Count);
        }

        [Fact]
        public async Task CreateProductGet_ReturnsAViewResult_WithAListOfProductsAndSelectList()
        {
            //Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCategoriesListQuery>(), It.IsAny<CancellationToken>()))
                .Returns(GetTestCategories());
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetSuppliersListQuery>(), It.IsAny<CancellationToken>()))
                .Returns(GetTestSuppliers());
            var controller = new ProductController(_mediatorMock.Object, _configMock.Object, _mapperMock.Object);

            //Act
            var result = await controller.Create();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<CreateProductCommand>(viewResult.ViewData.Model);
            _mediatorMock.Verify(mocks => mocks.Send(It.IsAny<GetProductsListQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(mocks => mocks.Send(It.IsAny<GetCategoriesListQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(mocks => mocks.Send(It.IsAny<GetSuppliersListQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            var categoriesCount = ((IList<CategoryDto>)((SelectList)viewResult.ViewData["Categories"]).Items).Count;
            var suppliersCount = ((IList<SupplierDto>)((SelectList)viewResult.ViewData["Suppliers"]).Items).Count;
            Assert.Equal(2, categoriesCount);
            Assert.Equal(3, suppliersCount);
        }


        [Fact]
        public async Task CreateProductPost_RedirectToIndex_WhenModelValid()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var controller = new ProductController(_mediatorMock.Object, _configMock.Object, _mapperMock.Object) {
                TempData = tempData
            };

            //Act
            var result = await controller.Create(new CreateProductCommand());

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(viewResult.ControllerName);
            Assert.Equal(nameof(ProductController.Index), viewResult.ActionName);
        }

        [Fact]
        public async Task CreateProductPost_ReturnBadRequest_WhenModelInValid()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var controller = new ProductController(_mediatorMock.Object, _configMock.Object, _mapperMock.Object) {
                TempData = tempData
            };

            controller.ModelState.AddModelError("Test field", "Required");

            //Act
            var result = await controller.Create(new CreateProductCommand());

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }


        [Fact]
        public async Task EditProductGet_ReturnsAViewResult_WithAListOfProductsAndSelectList()
        {
            //Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCategoriesListQuery>(), It.IsAny<CancellationToken>()))
                .Returns(GetTestCategories());
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetSuppliersListQuery>(), It.IsAny<CancellationToken>()))
                .Returns(GetTestSuppliers());
            var controller = new ProductController(_mediatorMock.Object, _configMock.Object, _mapperMock.Object);
            var id = 5;

            //Act
            var result = await controller.Edit(id);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<UpdateProductCommand>(viewResult.ViewData.Model);
            _mediatorMock.Verify(mocks => mocks.Send(It.IsAny<GetProductsListQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(mocks => mocks.Send(It.IsAny<GetCategoriesListQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(mocks => mocks.Send(It.IsAny<GetSuppliersListQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            var categoriesCount = ((IList<CategoryDto>)((SelectList)viewResult.ViewData["Categories"]).Items).Count;
            var suppliersCount = ((IList<SupplierDto>)((SelectList)viewResult.ViewData["Suppliers"]).Items).Count;
            Assert.Equal(2, categoriesCount);
            Assert.Equal(3, suppliersCount);
        }


        [Fact]
        public async Task EditProductPost_RedirectToIndex_WhenModelValid()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var controller = new ProductController(_mediatorMock.Object, _configMock.Object, _mapperMock.Object) {
                TempData = tempData
            };
            var id = 6;

            //Act
            var result = await controller.Edit(id, new UpdateProductCommand { ProductId = id });

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(viewResult.ControllerName);
            Assert.Equal(nameof(ProductController.Index), viewResult.ActionName);
        }

        [Fact]
        public async Task EditProductPost_ReturnBadRequest_WhenIdsInconsistent()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var controller = new ProductController(_mediatorMock.Object, _configMock.Object, _mapperMock.Object) {
                TempData = tempData
            };
            var id = 11;

            //Act
            var result = await controller.Edit(id, new UpdateProductCommand { ProductId = 12 });

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }


        private Task<ProductsListViewModel> GetTestProducts()
        {
            var products = CreateProductsEntities();
            return Task.Run(() => products);
        }

        private ProductsListViewModel CreateProductsEntities()
        {
            return new ProductsListViewModel {
                Products = new List<ProductDto>
                {
                        new ProductDto {ProductId = 1, ProductName = "FirstProduct"},
                        new ProductDto {ProductId = 2, ProductName = "SecondProduct"},
                        new ProductDto {ProductId = 3, ProductName = "ThirdProduct"},
                        new ProductDto {ProductId = 4, ProductName = "FourthProduct"}
                }
            };
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
                }
            };
        }

        private Task<SuppliersListViewModel> GetTestSuppliers()
        {
            var suppliers = CreateSuppliersEntities();
            return Task.Run(() => suppliers);
        }


        private SuppliersListViewModel CreateSuppliersEntities()
        {
            return new SuppliersListViewModel {
                Suppliers = new List<SupplierDto>
                {
                    new SupplierDto {Id = 1, CompanyName = "Apple"},
                    new SupplierDto {Id = 2, CompanyName = "Microsoft"},
                    new SupplierDto {Id = 3, CompanyName = "Soft"}
                }
            };
        }
    }
}