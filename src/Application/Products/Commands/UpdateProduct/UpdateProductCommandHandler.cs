using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Domain.Entities;

namespace Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly INorthwindDbContext _dbContext;

        public UpdateProductCommandHandler(INorthwindDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Products.FindAsync(request.ProductId);

            if (entity == null) {
                throw new NotFoundException(nameof(Product), request.ProductId);
            }

            entity.ProductId = request.ProductId;
            entity.ProductName = request.ProductName;
            entity.QuantityPerUnit = request.QuantityPerUnit;
            entity.UnitPrice = request.UnitPrice;
            entity.UnitsInStock = request.UnitsInStock;
            entity.UnitsOnOrder = request.UnitsOnOrder;
            entity.ReorderLevel = request.ReorderLevel;
            entity.Discontinued = request.Discontinued;
            entity.CategoryId = request.CategoryId;
            entity.SupplierId = request.SupplierId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}