﻿using System;
using System.Collections.Generic;
using System.Text;
using Application.Categories.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping
{
    public class CategoryMappingProfile : Profile
    {
        //TODO : use approach with IMapFrom interface in Dto and creating profiles using reflection
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(c => c.Id,
                    opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(c => c.Name,
                    opt => opt.MapFrom(src => src.CategoryName));
        }
    }
}
