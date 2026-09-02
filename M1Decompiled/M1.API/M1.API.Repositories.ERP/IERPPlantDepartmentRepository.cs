using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPlantDepartmentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PlantDepartment with the specified Unique Id exists.
	/// </summary>
	/// <param name="plantDepartmentId">The Unique Id of the PlantDepartment to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PlantDepartment exists or not.</returns>
	Task<bool> DoesPlantDepartmentExist(Guid plantDepartmentId);

	/// <summary>
	/// Retrieves all PlantDepartments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PlantDepartments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PlantDepartments DTOs.</returns>
	Task<ICollection<ERPPlantDepartmentInformationDto>> GetAllPlantDepartments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PlantDepartment.
	/// </summary>
	/// <param name="plantDepartmentId">The Unique Id of the PlantDepartment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PlantDepartment DTO.</returns>
	Task<ERPPlantDepartmentInformationDto> GetPlantDepartment(Guid plantDepartmentId);

	/// <summary>
	/// Saves the provided ERP plantDepartment.
	/// </summary>
	/// <param name="plantDepartment">The ERP plantDepartment to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePlantDepartment(ERPPlantDepartmentDto plantDepartment);
}
