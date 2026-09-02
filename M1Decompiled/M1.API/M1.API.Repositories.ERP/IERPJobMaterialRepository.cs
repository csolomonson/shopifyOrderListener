using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPJobMaterialRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a JobMaterial with the specified Unique Id exists.
	/// </summary>
	/// <param name="jobMaterialId">The Unique Id of the JobMaterial to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the JobMaterial exists or not.</returns>
	Task<bool> DoesJobMaterialExist(Guid jobMaterialId);

	/// <summary>
	/// Retrieves all JobMaterials with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobMaterials to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of JobMaterials DTOs.</returns>
	Task<ICollection<ERPJobMaterialInformationDto>> GetAllJobMaterials(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific JobMaterial.
	/// </summary>
	/// <param name="jobMaterialId">The Unique Id of the JobMaterial to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the JobMaterial DTO.</returns>
	Task<ERPJobMaterialInformationDto> GetJobMaterial(Guid jobMaterialId);

	/// <summary>
	/// Saves the provided ERP jobMaterial.
	/// </summary>
	/// <param name="jobMaterial">The ERP jobMaterial to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveJobMaterial(ERPJobMaterialDto jobMaterial);
}
