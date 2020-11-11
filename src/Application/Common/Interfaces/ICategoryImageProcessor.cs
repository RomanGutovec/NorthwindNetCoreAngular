using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Application.Categories.Queries;
using Application.Categories.Queries.CategoryDetail;

namespace Application.Common.Interfaces
{
    public interface ICategoryImageProcessor
    {
        void Process(CategoryDetailViewModel category);
    }
}
