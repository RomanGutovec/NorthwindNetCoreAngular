using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Suppliers.Queries.SuppliersList
{
    public class GetSuppliersListQueryHandler : IRequestHandler<GetSuppliersListQuery, SuppliersListViewModel>
    {
        private readonly INorthwindDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetSuppliersListQueryHandler(INorthwindDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<SuppliersListViewModel> Handle(GetSuppliersListQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await _dbContext.Suppliers.ProjectTo<SupplierDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new SuppliersListViewModel { Suppliers = suppliers };
        }
    }
}