using MediatR;

namespace Application.Products.Queries.ProductsList
{
    public class GetProductsListQuery : IRequest<ProductsListViewModel> 
    {
    }
}
