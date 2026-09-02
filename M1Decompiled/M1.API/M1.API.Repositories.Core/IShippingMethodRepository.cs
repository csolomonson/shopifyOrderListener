using System.Threading.Tasks;

namespace M1.API.Repositories.Core;

public interface IShippingMethodRepository
{
	/// <summary>
	/// Checks if the specified shipping method code exists.
	/// </summary>
	/// <param name="shippingMethodId">The ID of the shipping method to check.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains
	/// a boolean value indicating whether the shipping method code exists (true) or not (false).
	/// </returns>
	Task<bool> DoesShippingMethodExistsAsync(string shippingMethodId);
}
