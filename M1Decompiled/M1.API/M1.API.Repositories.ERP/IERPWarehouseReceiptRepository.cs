using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPWarehouseReceiptRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a WarehouseReceipt with the specified Unique Id exists.
	/// </summary>
	/// <param name="warehouseReceiptId">The Unique Id of the WarehouseReceipt to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the WarehouseReceipt exists or not.</returns>
	Task<bool> DoesWarehouseReceiptExist(Guid warehouseReceiptId);

	/// <summary>
	/// Retrieves all WarehouseReceipts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseReceipts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseReceipts DTOs.</returns>
	Task<ICollection<ERPWarehouseReceiptInformationDto>> GetAllWarehouseReceipts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific WarehouseReceipt.
	/// </summary>
	/// <param name="warehouseReceiptId">The Unique Id of the WarehouseReceipt to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the WarehouseReceipt DTO.</returns>
	Task<ERPWarehouseReceiptInformationDto> GetWarehouseReceipt(Guid warehouseReceiptId);

	/// <summary>
	/// Saves the provided ERP warehouseReceipt.
	/// </summary>
	/// <param name="warehouseReceipt">The ERP warehouseReceipt to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveWarehouseReceipt(ERPWarehouseReceiptDto warehouseReceipt);
}
