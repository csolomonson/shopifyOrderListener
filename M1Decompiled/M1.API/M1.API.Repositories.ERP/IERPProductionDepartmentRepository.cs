using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPProductionDepartmentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ProductionDepartment with the specified Unique Id exists.
	/// </summary>
	/// <param name="productionDepartmentId">The Unique Id of the ProductionDepartment to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ProductionDepartment exists or not.</returns>
	Task<bool> DoesProductionDepartmentExist(Guid productionDepartmentId);

	/// <summary>
	/// Retrieves all ProductionDepartments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductionDepartments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProductionDepartments DTOs.</returns>
	Task<ICollection<ERPProductionDepartmentInformationDto>> GetAllProductionDepartments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ProductionDepartment.
	/// </summary>
	/// <param name="productionDepartmentId">The Unique Id of the ProductionDepartment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ProductionDepartment DTO.</returns>
	Task<ERPProductionDepartmentInformationDto> GetProductionDepartment(Guid productionDepartmentId);

	/// <summary>
	/// Saves the provided ERP productionDepartment.
	/// </summary>
	/// <param name="productionDepartment">The ERP productionDepartment to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveProductionDepartment(ERPProductionDepartmentDto productionDepartment);
}
