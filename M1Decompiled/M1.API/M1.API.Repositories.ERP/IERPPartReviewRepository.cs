using System;
using System.Threading.Tasks;

namespace M1.API.Repositories.ERP;

public interface IERPPartReviewRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartReview with the specified Unique Id exists.
	/// </summary>
	/// <param name="partReviewId">The Unique Id of the PartReview to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartReview exists or not.</returns>
	Task<bool> DoesPartReviewExist(Guid partReviewId);
}
