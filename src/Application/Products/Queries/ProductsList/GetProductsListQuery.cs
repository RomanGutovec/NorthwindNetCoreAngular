using System;
using MediatR;
using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Products.Queries.ProductsList
{
    public class GetProductsListQuery : IRequest<ProductsListViewModel>
    {
        public int AmountOfProducts { get; set; } 
    }
}
