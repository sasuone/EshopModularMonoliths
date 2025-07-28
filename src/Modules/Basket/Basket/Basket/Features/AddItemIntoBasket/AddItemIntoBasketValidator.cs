namespace Basket.Basket.Features.AddItemIntoBasket;

public class AddItemIntoBasketValidator :  AbstractValidator<AddItemIntoBasketCommand>
{
	public AddItemIntoBasketValidator()
	{
		RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
		RuleFor(x => x.ShoppingCartItem.ProductId).NotEmpty().WithMessage("ProductId is required");
		RuleFor(x => x.ShoppingCartItem.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
	}
}