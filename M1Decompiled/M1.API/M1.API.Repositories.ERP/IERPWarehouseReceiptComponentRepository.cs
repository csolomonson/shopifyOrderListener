using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPWarehouseReceiptComponentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a WarehouseReceiptComponent with the specified Unique Id exists.
	/// </summary>
	/// <param name="warehouseReceiptComponentId">The Unique Id of the WarehouseReceiptComponent to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the WarehouseReceiptComponent exists or not.</returns>
	Task<bool> DoesWarehouseReceiptComponentExist(Guid warehouseReceiptComponentId);

	/// <summary>
	/// Retrieves all WarehouseReceiptComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseReceiptComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseReceiptComponents DTOs.</returns>
	Task<ICollection<ERPWarehouseReceiptComponentInformationDto>> GetAllWarehouseReceiptComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific WarehouseReceiptComponent.
	/// </summary>
	/// <param name="warehouseReceiptComponentId">The Unique Id of the WarehouseReceiptComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the WarehouseReceiptComponent DTO.</returns>
	Task<ERPWarehouseReceiptComponentInformationDto> GetWarehouseReceiptComponent(Guid warehouseReceiptComponentId);

	/// <summary>
	/// Saves the provided ERP warehouseReceiptComponent.
	/// </summary>
	/// <param name="warehouseReceiptComponent">The ERP warehouseReceiptComponent to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveWarehouseReceiptComponent(ERPWarehouseReceiptComponentDto warehouseReceiptComponent);
}
