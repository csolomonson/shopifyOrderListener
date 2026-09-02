using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPGLDivisionRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a GLDivision with the specified Unique Id exists.
	/// </summary>
	/// <param name="gLDivisionId">The Unique Id of the GLDivision to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the GLDivision exists or not.</returns>
	Task<bool> DoesGLDivisionExist(Guid gLDivisionId);

	/// <summary>
	/// Retrieves all GLDivisions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLDivisions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLDivisions DTOs.</returns>
	Task<ICollection<ERPGLDivisionInformationDto>> GetAllGLDivisions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific GLDivision.
	/// </summary>
	/// <param name="gLDivisionId">The Unique Id of the GLDivision to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the GLDivision DTO.</returns>
	Task<ERPGLDivisionInformationDto> GetGLDivision(Guid gLDivisionId);

	/// <summary>
	/// Saves the provided ERP gLDivision.
	/// </summary>
	/// <param name="gLDivision">The ERP gLDivision to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveGLDivision(ERPGLDivisionDto gLDivision);
}
