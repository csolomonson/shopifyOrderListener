using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPCallTypeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a CallType with the specified Unique Id exists.
	/// </summary>
	/// <param name="callTypeId">The Unique Id of the CallType to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the CallType exists or not.</returns>
	Task<bool> DoesCallTypeExist(Guid callTypeId);

	/// <summary>
	/// Retrieves all CallTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CallTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CallTypes DTOs.</returns>
	Task<ICollection<ERPCallTypeInformationDto>> GetAllCallTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific CallType.
	/// </summary>
	/// <param name="callTypeId">The Unique Id of the CallType to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the CallType DTO.</returns>
	Task<ERPCallTypeInformationDto> GetCallType(Guid callTypeId);
}
