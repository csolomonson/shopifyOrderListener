using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPRMAActionTypeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a RMAActionType with the specified Unique Id exists.
	/// </summary>
	/// <param name="rMAActionTypeId">The Unique Id of the RMAActionType to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the RMAActionType exists or not.</returns>
	Task<bool> DoesRMAActionTypeExist(Guid rMAActionTypeId);

	/// <summary>
	/// Retrieves all RMAActionTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAActionTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RMAActionTypes DTOs.</returns>
	Task<ICollection<ERPRMAActionTypeInformationDto>> GetAllRMAActionTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific RMAActionType.
	/// </summary>
	/// <param name="rMAActionTypeId">The Unique Id of the RMAActionType to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the RMAActionType DTO.</returns>
	Task<ERPRMAActionTypeInformationDto> GetRMAActionType(Guid rMAActionTypeId);
}
