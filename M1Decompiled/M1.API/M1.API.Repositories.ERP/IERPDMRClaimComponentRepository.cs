using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPDMRClaimComponentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a DMRClaimComponent with the specified Unique Id exists.
	/// </summary>
	/// <param name="dMRClaimComponentId">The Unique Id of the DMRClaimComponent to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the DMRClaimComponent exists or not.</returns>
	Task<bool> DoesDMRClaimComponentExist(Guid dMRClaimComponentId);

	/// <summary>
	/// Retrieves all DMRClaimComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRClaimComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of DMRClaimComponents DTOs.</returns>
	Task<ICollection<ERPDMRClaimComponentInformationDto>> GetAllDMRClaimComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific DMRClaimComponent.
	/// </summary>
	/// <param name="dMRClaimComponentId">The Unique Id of the DMRClaimComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the DMRClaimComponent DTO.</returns>
	Task<ERPDMRClaimComponentInformationDto> GetDMRClaimComponent(Guid dMRClaimComponentId);

	/// <summary>
	/// Saves the provided ERP dMRClaimComponent.
	/// </summary>
	/// <param name="dMRClaimComponent">The ERP dMRClaimComponent to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveDMRClaimComponent(ERPDMRClaimComponentDto dMRClaimComponent);
}
