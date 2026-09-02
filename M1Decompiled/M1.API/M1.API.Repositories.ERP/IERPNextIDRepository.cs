using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPNextIDRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a NextID with the specified Unique Id exists.
	/// </summary>
	/// <param name="nextIDId">The Unique Id of the NextID to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the NextID exists or not.</returns>
	Task<bool> DoesNextIDExist(Guid nextIDId);

	/// <summary>
	/// Retrieves all NextIDs with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of NextIDs to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of NextIDs DTOs.</returns>
	Task<ICollection<ERPNextIDInformationDto>> GetAllNextIDs(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific NextID.
	/// </summary>
	/// <param name="nextIDId">The Unique Id of the NextID to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the NextID DTO.</returns>
	Task<ERPNextIDInformationDto> GetNextID(Guid nextIDId);

	/// <summary>
	/// Saves the provided ERP nextID.
	/// </summary>
	/// <param name="nextID">The ERP nextID to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveNextID(ERPNextIDDto nextID);
}
