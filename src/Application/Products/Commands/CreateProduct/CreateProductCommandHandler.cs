using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly INorthwindDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(INorthwindDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productEntity = _mapper.Map<Product>(request);
            _dbContext.Products.Add(productEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return productEntity.ProductId;
        }
    }
}
