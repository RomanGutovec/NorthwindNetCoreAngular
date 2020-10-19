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
                .ForMember(p => p.SupplierName, opt => opt.MapFrom(src => src.Supplier.ContactName));
        }
    }
}
