using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPRMAClaimComponentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a RMAClaimComponent with the specified Unique Id exists.
	/// </summary>
	/// <param name="rMAClaimComponentId">The Unique Id of the RMAClaimComponent to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the RMAClaimComponent exists or not.</returns>
	Task<bool> DoesRMAClaimComponentExist(Guid rMAClaimComponentId);

	/// <summary>
	/// Retrieves all RMAClaimComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAClaimComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RMAClaimComponents DTOs.</returns>
	Task<ICollection<ERPRMAClaimComponentInformationDto>> GetAllRMAClaimComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific RMAClaimComponent.
	/// </summary>
	/// <param name="rMAClaimComponentId">The Unique Id of the RMAClaimComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the RMAClaimComponent DTO.</returns>
	Task<ERPRMAClaimComponentInformationDto> GetRMAClaimComponent(Guid rMAClaimComponentId);

	/// <summary>
	/// Saves the provided ERP rMAClaimComponent.
	/// </summary>
	/// <param name="rMAClaimComponent">The ERP rMAClaimComponent to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveRMAClaimComponent(ERPRMAClaimComponentDto rMAClaimComponent);
}
