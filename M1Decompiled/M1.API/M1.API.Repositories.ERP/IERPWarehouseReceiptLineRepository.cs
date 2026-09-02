using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPWarehouseReceiptLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a WarehouseReceiptLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="warehouseReceiptLineId">The Unique Id of the WarehouseReceiptLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the WarehouseReceiptLine exists or not.</returns>
	Task<bool> DoesWarehouseReceiptLineExist(Guid warehouseReceiptLineId);

	/// <summary>
	/// Retrieves all WarehouseReceiptLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseReceiptLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseReceiptLines DTOs.</returns>
	Task<ICollection<ERPWarehouseReceiptLineInformationDto>> GetAllWarehouseReceiptLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific WarehouseReceiptLine.
	/// </summary>
	/// <param name="warehouseReceiptLineId">The Unique Id of the WarehouseReceiptLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the WarehouseReceiptLine DTO.</returns>
	Task<ERPWarehouseReceiptLineInformationDto> GetWarehouseReceiptLine(Guid warehouseReceiptLineId);

	/// <summary>
	/// Saves the provided ERP warehouseReceiptLine.
	/// </summary>
	/// <param name="warehouseReceiptLine">The ERP warehouseReceiptLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveWarehouseReceiptLine(ERPWarehouseReceiptLineDto warehouseReceiptLine);
}
