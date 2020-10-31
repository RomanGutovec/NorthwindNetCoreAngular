using MediatR;

namespace Application.Categories.Queries.CategoryDetail
{
    public class GetCategoryDetailQuery : IRequest<CategoryDetailViewModel>
    {
        public int Id { get; set; }
    }
}
