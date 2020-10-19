using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Products.Queries.ProductsList
{
    public class GetProductsListQuery : IRequest<ProductsListViewModel> 
    {
    }
}
