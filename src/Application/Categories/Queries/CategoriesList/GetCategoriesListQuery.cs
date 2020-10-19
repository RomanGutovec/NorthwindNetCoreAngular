using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Categories.Queries
{
    public class GetCategoriesListQuery : IRequest<CategoriesListViewModel>
    {
    }
}
