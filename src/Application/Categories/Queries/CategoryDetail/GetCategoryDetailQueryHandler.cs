using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.Queries.CategoryDetail
{
    public class GetCategoryDetailQueryHandler : IRequestHandler<GetCategoryDetailQuery, CategoryDetailViewModel>
    {
        private readonly INorthwindDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICategoryImageProcessor _imageProcessor;

        public GetCategoryDetailQueryHandler(INorthwindDbContext dbContext, IMapper mapper, ICategoryImageProcessor imageProcessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _imageProcessor = imageProcessor;
        }

        public async Task<CategoryDetailViewModel> Handle(GetCategoryDetailQuery request, CancellationToken cancellationToken)
        {
            var categoryDetail = await _dbContext.Categories.ProjectTo<CategoryDetailViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (categoryDetail == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            _imageProcessor.Process(categoryDetail);

            return categoryDetail;
        }
    }
}
