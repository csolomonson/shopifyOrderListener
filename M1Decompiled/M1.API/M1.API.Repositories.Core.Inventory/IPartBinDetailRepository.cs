using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Custom;

namespace M1.API.Repositories.Core.Inventory;

public interface IPartBinDetailRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Retrieves detailed information about all part bin details associated with the given part ID.
	/// </summary>
	/// <param name="partId">The ID of the part associated with the part bin details.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a list of part bin detail information DTOs.</returns>
	Task<IList<PartBinDetailInformationDto>> GetPartBinDetailsInfo(string partId);

	/// <summary>
	/// Retrieves all part bin details with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of part bin details to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a collection of part bin detail information DTOs.</returns>
	Task<ICollection<PartBinDetailInformationDto>> GetAllPartBinDetails(int? pageSize = null, int? pageNumber = null);
}
