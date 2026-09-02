using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPGLDepartmentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a GLDepartment with the specified Unique Id exists.
	/// </summary>
	/// <param name="gLDepartmentId">The Unique Id of the GLDepartment to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the GLDepartment exists or not.</returns>
	Task<bool> DoesGLDepartmentExist(Guid gLDepartmentId);

	/// <summary>
	/// Retrieves all GLDepartments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLDepartments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLDepartments DTOs.</returns>
	Task<ICollection<ERPGLDepartmentInformationDto>> GetAllGLDepartments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific GLDepartment.
	/// </summary>
	/// <param name="gLDepartmentId">The Unique Id of the GLDepartment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the GLDepartment DTO.</returns>
	Task<ERPGLDepartmentInformationDto> GetGLDepartment(Guid gLDepartmentId);

	/// <summary>
	/// Saves the provided ERP gLDepartment.
	/// </summary>
	/// <param name="gLDepartment">The ERP gLDepartment to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveGLDepartment(ERPGLDepartmentDto gLDepartment);
}
