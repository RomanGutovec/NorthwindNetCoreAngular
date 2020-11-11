using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            var filteredProducts = request.AmountOfProducts == 0
                ? products
                : products.Take(request.AmountOfProducts).ToList();
            
            return new ProductsListViewModel { Products = filteredProducts };
        }
    }
}