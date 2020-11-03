using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Queries;
using Application.Products.Queries.ProductDetail;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(p => p.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(p => p.SupplierCompanyName, opt => opt.MapFrom(src => src.Supplier.CompanyName))
                .ReverseMap();

            CreateMap<CreateProductCommand, Product>().ReverseMap();
            CreateMap<UpdateProductCommand, Product>().ReverseMap();
            CreateMap<Product, ProductDetailViewModel>();
            CreateMap<ProductDetailViewModel, UpdateProductCommand>();
        }
    }
}
