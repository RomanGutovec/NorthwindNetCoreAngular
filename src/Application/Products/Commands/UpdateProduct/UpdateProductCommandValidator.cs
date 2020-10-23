using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(product => product.ProductName).Length(1, 10).WithMessage("Product name length should be from 1 to 10 characters.");
            RuleFor(product => product.QuantityPerUnit).NotEmpty().WithMessage("Quantity is required.");
            RuleFor(product => product.UnitsInStock).GreaterThan((short)0).WithMessage("Units amount should be positive and greater than 0.");
            RuleFor(product => product.QuantityPerUnit).MaximumLength(20).WithMessage("Amount of units is too big.");
        }
    }
}
