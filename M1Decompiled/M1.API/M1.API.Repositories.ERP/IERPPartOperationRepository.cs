using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartOperationRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartOperation with the specified Unique Id exists.
	/// </summary>
	/// <param name="partOperationId">The Unique Id of the PartOperation to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartOperation exists or not.</returns>
	Task<bool> DoesPartOperationExist(Guid partOperationId);

	/// <summary>
	/// Retrieves all PartOperations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartOperations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartOperations DTOs.</returns>
	Task<ICollection<ERPPartOperationInformationDto>> GetAllPartOperations(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartOperation.
	/// </summary>
	/// <param name="partOperationId">The Unique Id of the PartOperation to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartOperation DTO.</returns>
	Task<ERPPartOperationInformationDto> GetPartOperation(Guid partOperationId);

	/// <summary>
	/// Saves the provided ERP partOperation.
	/// </summary>
	/// <param name="partOperation">The ERP partOperation to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartOperation(ERPPartOperationDto partOperation);
}
