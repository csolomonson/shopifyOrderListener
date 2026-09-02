using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartMaterialRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartMaterial with the specified Unique Id exists.
	/// </summary>
	/// <param name="partMaterialId">The Unique Id of the PartMaterial to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartMaterial exists or not.</returns>
	Task<bool> DoesPartMaterialExist(Guid partMaterialId);

	/// <summary>
	/// Retrieves all PartMaterials with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartMaterials to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartMaterials DTOs.</returns>
	Task<ICollection<ERPPartMaterialInformationDto>> GetAllPartMaterials(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartMaterial.
	/// </summary>
	/// <param name="partMaterialId">The Unique Id of the PartMaterial to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartMaterial DTO.</returns>
	Task<ERPPartMaterialInformationDto> GetPartMaterial(Guid partMaterialId);

	/// <summary>
	/// Saves the provided ERP partMaterial.
	/// </summary>
	/// <param name="partMaterial">The ERP partMaterial to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartMaterial(ERPPartMaterialDto partMaterial);
}
