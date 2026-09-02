using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPWarehouseBinRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a WarehouseBin with the specified Unique Id exists.
	/// </summary>
	/// <param name="warehouseBinId">The Unique Id of the WarehouseBin to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the WarehouseBin exists or not.</returns>
	Task<bool> DoesWarehouseBinExist(Guid warehouseBinId);

	/// <summary>
	/// Retrieves all WarehouseBins with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseBins to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseBins DTOs.</returns>
	Task<ICollection<ERPWarehouseBinInformationDto>> GetAllWarehouseBins(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific WarehouseBin.
	/// </summary>
	/// <param name="warehouseBinId">The Unique Id of the WarehouseBin to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the WarehouseBin DTO.</returns>
	Task<ERPWarehouseBinInformationDto> GetWarehouseBin(Guid warehouseBinId);

	/// <summary>
	/// Saves the provided ERP warehouseBin.
	/// </summary>
	/// <param name="warehouseBin">The ERP warehouseBin to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveWarehouseBin(ERPWarehouseBinDto warehouseBin);
}
