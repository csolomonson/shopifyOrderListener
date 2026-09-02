using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPProjectTypeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ProjectType with the specified Unique Id exists.
	/// </summary>
	/// <param name="projectTypeId">The Unique Id of the ProjectType to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ProjectType exists or not.</returns>
	Task<bool> DoesProjectTypeExist(Guid projectTypeId);

	/// <summary>
	/// Retrieves all ProjectTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProjectTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProjectTypes DTOs.</returns>
	Task<ICollection<ERPProjectTypeInformationDto>> GetAllProjectTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ProjectType.
	/// </summary>
	/// <param name="projectTypeId">The Unique Id of the ProjectType to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ProjectType DTO.</returns>
	Task<ERPProjectTypeInformationDto> GetProjectType(Guid projectTypeId);

	/// <summary>
	/// Saves the provided ERP projectType.
	/// </summary>
	/// <param name="projectType">The ERP projectType to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveProjectType(ERPProjectTypeDto projectType);
}
