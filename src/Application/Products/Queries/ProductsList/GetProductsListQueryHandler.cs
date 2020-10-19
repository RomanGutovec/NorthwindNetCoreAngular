using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Categories.Queries;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Queries.ProductsList
{
    public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, ProductsListViewModel>
    {
        private readonly IMapper _mapper;
        private readonly INorthwindDbContext _dbContext;

        public GetProductsListQueryHandler(IMapper mapper, INorthwindDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<ProductsListViewModel> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            var products = await _dbContext.Products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new ProductsListViewModel { Products = products };
        }
    }
}
