using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartCrossReferenceRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartCrossReference with the specified Unique Id exists.
	/// </summary>
	/// <param name="partCrossReferenceId">The Unique Id of the PartCrossReference to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartCrossReference exists or not.</returns>
	Task<bool> DoesPartCrossReferenceExist(Guid partCrossReferenceId);

	/// <summary>
	/// Retrieves all PartCrossReferences with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartCrossReferences to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartCrossReferences DTOs.</returns>
	Task<ICollection<ERPPartCrossReferenceInformationDto>> GetAllPartCrossReferences(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartCrossReference.
	/// </summary>
	/// <param name="partCrossReferenceId">The Unique Id of the PartCrossReference to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartCrossReference DTO.</returns>
	Task<ERPPartCrossReferenceInformationDto> GetPartCrossReference(Guid partCrossReferenceId);

	/// <summary>
	/// Saves the provided ERP partCrossReference.
	/// </summary>
	/// <param name="partCrossReference">The ERP partCrossReference to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartCrossReference(ERPPartCrossReferenceDto partCrossReference);
}
