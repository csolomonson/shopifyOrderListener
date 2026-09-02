using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPJobOperationRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a JobOperation with the specified Unique Id exists.
	/// </summary>
	/// <param name="jobOperationId">The Unique Id of the JobOperation to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the JobOperation exists or not.</returns>
	Task<bool> DoesJobOperationExist(Guid jobOperationId);

	/// <summary>
	/// Retrieves all JobOperations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobOperations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of JobOperations DTOs.</returns>
	Task<ICollection<ERPJobOperationInformationDto>> GetAllJobOperations(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific JobOperation.
	/// </summary>
	/// <param name="jobOperationId">The Unique Id of the JobOperation to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the JobOperation DTO.</returns>
	Task<ERPJobOperationInformationDto> GetJobOperation(Guid jobOperationId);

	/// <summary>
	/// Saves the provided ERP jobOperation.
	/// </summary>
	/// <param name="jobOperation">The ERP jobOperation to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveJobOperation(ERPJobOperationDto jobOperation);
}
