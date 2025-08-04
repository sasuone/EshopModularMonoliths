namespace Basket.Basket.Models;

public class ShoppingCart : Aggregate<Guid>
{
	public string UserName { get; private set; } = default!;
	private readonly List<ShoppingCartItem> _items = new();
	public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();
	private decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);

	public static ShoppingCart Create(Guid id, string userName)
	{
		ArgumentNullException.ThrowIfNull(userName);

		ShoppingCart shoppingCart = new ShoppingCart
		{
			Id = id,
			UserName = userName
		};
		
		return shoppingCart;
	}

	public void AddItem(Guid productId, int quantity, string color, decimal price, string productName)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
		
		ShoppingCartItem? existingItem = Items.FirstOrDefault(x => x.ProductId == productId);

		if (existingItem != null)
		{
			existingItem.Quantity += quantity;
		}
		else
		{
			ShoppingCartItem newItem = new ShoppingCartItem(Id, productId, quantity, color, price, productName);
			_items.Add(newItem);
		}
	}

	public void RemoveItem(Guid productId)
	{
		ShoppingCartItem? existingItem = Items.FirstOrDefault(x => x.ProductId == productId);
		if (existingItem != null)
		{
			_items.Remove(existingItem);
		}
	}
}