using Application.Suppliers;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping
{
    public class SupplierMappingProfile : Profile
    {
        public SupplierMappingProfile()
        {
            CreateMap<Supplier, SupplierDto>()
                .ForMember(c => c.Id,
                    opt => opt.MapFrom(src => src.SupplierId));
        }
    }
}
