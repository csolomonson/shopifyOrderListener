using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartOrgReferenceRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartOrgReference with the specified Unique Id exists.
	/// </summary>
	/// <param name="partOrgReferenceId">The Unique Id of the PartOrgReference to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartOrgReference exists or not.</returns>
	Task<bool> DoesPartOrgReferenceExist(Guid partOrgReferenceId);

	/// <summary>
	/// Retrieves all PartOrgReferences with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartOrgReferences to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartOrgReferences DTOs.</returns>
	Task<ICollection<ERPPartOrgReferenceInformationDto>> GetAllPartOrgReferences(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartOrgReference.
	/// </summary>
	/// <param name="partOrgReferenceId">The Unique Id of the PartOrgReference to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartOrgReference DTO.</returns>
	Task<ERPPartOrgReferenceInformationDto> GetPartOrgReference(Guid partOrgReferenceId);

	/// <summary>
	/// Saves the provided ERP partOrgReference.
	/// </summary>
	/// <param name="partOrgReference">The ERP partOrgReference to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartOrgReference(ERPPartOrgReferenceDto partOrgReference);
}
