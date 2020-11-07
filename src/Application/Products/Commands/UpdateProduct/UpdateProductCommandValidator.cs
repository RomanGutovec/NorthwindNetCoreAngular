using FluentValidation;

namespace Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(product => product.ProductName).NotNull().MaximumLength(20).WithMessage("Product name length should be from 1 to 20 characters.");
            RuleFor(product => product.CategoryId).NotNull().WithMessage("Please choose category");
            RuleFor(product => product.SupplierId).NotNull().WithMessage("Please choose supplier");
            RuleFor(product => product.UnitPrice).NotNull().WithMessage("Price is required");
            RuleFor(product => product.QuantityPerUnit).NotEmpty().WithMessage("Quantity is required.");
            RuleFor(product => product.QuantityPerUnit).MaximumLength(40).WithMessage("Amount of units is too big.");
            RuleFor(product => product.UnitsInStock).NotEmpty().WithMessage("Units amount is required.");
            RuleFor(product => product.ReorderLevel).NotEmpty().WithMessage("Reorder level is required.");
        }
    }
}