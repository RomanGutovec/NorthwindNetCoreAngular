using MediatR;

namespace Application.Products.Queries.ProductDetail
{
    public class GetProductDetailQuery : IRequest<ProductDetailViewModel>
    {
        public int ProductId { get; set; }
    }
}
