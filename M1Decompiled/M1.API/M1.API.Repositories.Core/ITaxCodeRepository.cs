using System;
using System.Threading.Tasks;

namespace M1.API.Repositories.Core;

public interface ITaxCodeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if the specified tax code exists.
	/// </summary>
	/// <param name="taxCodeId">The ID of the tax code to check.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains
	/// a boolean value indicating whether the tax code exists (true) or not (false).
	/// </returns>
	Task<bool> DoesTaxCodeExistAsync(string taxCodeId);
}
