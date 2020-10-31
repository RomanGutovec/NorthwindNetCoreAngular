using Application.Categories.Commands.UpdateCategory;
using Application.Categories.Queries;
using Application.Categories.Queries.CategoryDetail;
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
            CreateMap<Category, CategoryDetailViewModel>()
                .ForMember(c => c.Id,
                    opt => opt.MapFrom(src => src.CategoryId));
            CreateMap<UpdateCategoryCommand, Category>();
        }
    }
}
