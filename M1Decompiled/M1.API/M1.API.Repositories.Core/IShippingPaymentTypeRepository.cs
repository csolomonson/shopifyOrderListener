using System.Threading.Tasks;

namespace M1.API.Repositories.Core;

public interface IShippingPaymentTypeRepository
{
	/// <summary>
	/// Checks if a specific shipping payment type exists asynchronously.
	/// </summary>
	/// <param name="shippingPaymentTypeCode">The code of the shipping payment type to check.</param>
	/// <returns>A task that represents the asynchronous operation. 
	/// The task result contains a boolean value indicating whether the shipping payment type exists.</returns>
	Task<bool> DoesShippingPaymentTypeExistsAsync(string shippingPaymentTypeCode);
}
