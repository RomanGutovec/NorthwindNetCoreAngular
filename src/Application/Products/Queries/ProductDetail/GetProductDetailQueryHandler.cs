using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Queries.ProductDetail
{
    public class GetProductDetailQueryHandler : IRequestHandler<GetProductDetailQuery, ProductDetailViewModel>
    {
        private readonly INorthwindDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetProductDetailQueryHandler(INorthwindDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ProductDetailViewModel> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
        {
            var productDetail = await _dbContext.Products.ProjectTo<ProductDetailViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.ProductId == request.ProductId, cancellationToken);

            if (productDetail == null) {
                throw new NotFoundException(nameof(Product), request.ProductId);
            }

            return productDetail;
        }
    }
}