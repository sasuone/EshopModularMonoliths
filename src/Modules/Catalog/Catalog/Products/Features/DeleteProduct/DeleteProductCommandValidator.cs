namespace Catalog.Products.Features.DeleteProduct;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
	public DeleteProductCommandValidator()
	{
		RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product Id is required");
	}
}