using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPProjectAreaRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ProjectArea with the specified Unique Id exists.
	/// </summary>
	/// <param name="projectAreaId">The Unique Id of the ProjectArea to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ProjectArea exists or not.</returns>
	Task<bool> DoesProjectAreaExist(Guid projectAreaId);

	/// <summary>
	/// Retrieves all ProjectAreas with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProjectAreas to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProjectAreas DTOs.</returns>
	Task<ICollection<ERPProjectAreaInformationDto>> GetAllProjectAreas(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ProjectArea.
	/// </summary>
	/// <param name="projectAreaId">The Unique Id of the ProjectArea to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ProjectArea DTO.</returns>
	Task<ERPProjectAreaInformationDto> GetProjectArea(Guid projectAreaId);

	/// <summary>
	/// Saves the provided ERP projectArea.
	/// </summary>
	/// <param name="projectArea">The ERP projectArea to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveProjectArea(ERPProjectAreaDto projectArea);
}
