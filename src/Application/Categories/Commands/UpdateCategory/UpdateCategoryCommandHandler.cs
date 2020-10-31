using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly INorthwindDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(INorthwindDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Categories.FindAsync(request.Id);

            if (entity == null) {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            entity.CategoryName = request.Name ?? entity.CategoryName;
            entity.Description = request.Description ?? entity.Description;
            entity.Picture = request.Picture ?? entity.Picture;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}