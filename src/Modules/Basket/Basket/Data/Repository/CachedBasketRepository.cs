using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;
using Basket.Data.JsonConverters;

namespace Basket.Data.Repository;

public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache)
	: IBasketRepository
{
	private readonly JsonSerializerOptions _options = new JsonSerializerOptions
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		Converters =
		{
			new ShoppingCartConverter(), 
			new ShoppingCartItemConverter()
		}
	};
	
	public async Task<ShoppingCart> GetBasket(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
	{
		if (!asNoTracking)
		{
			return await repository.GetBasket(userName, false, cancellationToken);
		}
		
		string? cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
		if (!string.IsNullOrEmpty(cachedBasket))
		{
			return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket, _options)!;
		}
		
		ShoppingCart shoppingCart = await repository.GetBasket(userName, asNoTracking, cancellationToken);
		await cache.SetStringAsync(userName, JsonSerializer.Serialize(shoppingCart, _options), cancellationToken);
		
		return shoppingCart;
	}

	public async Task<ShoppingCart> CreateBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
	{
		await repository.CreateBasket(shoppingCart, cancellationToken);
		
		await cache.SetStringAsync(shoppingCart.UserName, JsonSerializer.Serialize(shoppingCart, _options), cancellationToken);
		
		return shoppingCart;
	}

	public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
	{
		await repository.DeleteBasket(userName, cancellationToken);
		
		await cache.RemoveAsync(userName, cancellationToken);
		
		return true;
	}

	public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
	{
		int result = await repository.SaveChangesAsync(userName, cancellationToken);

		if (userName is not null)
		{
			await cache.RemoveAsync(userName, cancellationToken);
		}
		
		return result;
	}
}