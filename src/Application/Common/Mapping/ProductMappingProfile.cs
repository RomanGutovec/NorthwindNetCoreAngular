using Application.Products.Commands.CreateProduct;
using Application.Products.Queries;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(p => p.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(p => p.SupplierCompanyName, opt => opt.MapFrom(src => src.Supplier.CompanyName));

            CreateMap<CreateProductCommand, Product>();
        }
    }
}
