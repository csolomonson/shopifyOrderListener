using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPProjectRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Project with the specified Unique Id exists.
	/// </summary>
	/// <param name="projectId">The Unique Id of the Project to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Project exists or not.</returns>
	Task<bool> DoesProjectExist(Guid projectId);

	/// <summary>
	/// Retrieves all Projects with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Projects to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Projects DTOs.</returns>
	Task<ICollection<ERPProjectInformationDto>> GetAllProjects(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Project.
	/// </summary>
	/// <param name="projectId">The Unique Id of the Project to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Project DTO.</returns>
	Task<ERPProjectInformationDto> GetProject(Guid projectId);

	/// <summary>
	/// Saves the provided ERP project.
	/// </summary>
	/// <param name="project">The ERP project to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveProject(ERPProjectDto project);
}
