using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Application.Products.Queries.ProductsList
{
    public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, ProductsListViewModel>
    {
        private readonly IMapper _mapper;
        private readonly INorthwindDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public GetProductsListQueryHandler(IMapper mapper, INorthwindDbContext dbContext, IConfiguration configuration)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<ProductsListViewModel> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            var amountProductsToGet = _configuration.GetValue("NorthwindVariables:ProductsAmount", 0);
            var products = await _dbContext.Products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            var filteredProducts = amountProductsToGet == 0 ? products : products.Take(amountProductsToGet).ToList();
            
            return new ProductsListViewModel { Products = filteredProducts };
        }
    }
}
