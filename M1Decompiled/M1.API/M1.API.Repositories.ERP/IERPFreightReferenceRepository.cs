using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPFreightReferenceRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a FreightReference with the specified Unique Id exists.
	/// </summary>
	/// <param name="freightReferenceId">The Unique Id of the FreightReference to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the FreightReference exists or not.</returns>
	Task<bool> DoesFreightReferenceExist(Guid freightReferenceId);

	/// <summary>
	/// Retrieves all FreightReferences with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FreightReferences to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of FreightReferences DTOs.</returns>
	Task<ICollection<ERPFreightReferenceInformationDto>> GetAllFreightReferences(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific FreightReference.
	/// </summary>
	/// <param name="freightReferenceId">The Unique Id of the FreightReference to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the FreightReference DTO.</returns>
	Task<ERPFreightReferenceInformationDto> GetFreightReference(Guid freightReferenceId);

	/// <summary>
	/// Saves the provided ERP freightReference.
	/// </summary>
	/// <param name="freightReference">The ERP freightReference to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveFreightReference(ERPFreightReferenceDto freightReference);
}
