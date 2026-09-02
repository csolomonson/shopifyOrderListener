using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPWarehouseRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Warehouse with the specified Unique Id exists.
	/// </summary>
	/// <param name="warehouseId">The Unique Id of the Warehouse to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Warehouse exists or not.</returns>
	Task<bool> DoesWarehouseExist(Guid warehouseId);

	/// <summary>
	/// Retrieves all Warehouses with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Warehouses to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Warehouses DTOs.</returns>
	Task<ICollection<ERPWarehouseInformationDto>> GetAllWarehouses(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Warehouse.
	/// </summary>
	/// <param name="warehouseId">The Unique Id of the Warehouse to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Warehouse DTO.</returns>
	Task<ERPWarehouseInformationDto> GetWarehouse(Guid warehouseId);

	/// <summary>
	/// Saves the provided ERP warehouse.
	/// </summary>
	/// <param name="warehouse">The ERP warehouse to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveWarehouse(ERPWarehouseDto warehouse);
}
