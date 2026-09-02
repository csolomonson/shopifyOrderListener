using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPWarehouseTransferLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a WarehouseTransferLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="warehouseTransferLineId">The Unique Id of the WarehouseTransferLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the WarehouseTransferLine exists or not.</returns>
	Task<bool> DoesWarehouseTransferLineExist(Guid warehouseTransferLineId);

	/// <summary>
	/// Retrieves all WarehouseTransferLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseTransferLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseTransferLines DTOs.</returns>
	Task<ICollection<ERPWarehouseTransferLineInformationDto>> GetAllWarehouseTransferLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific WarehouseTransferLine.
	/// </summary>
	/// <param name="warehouseTransferLineId">The Unique Id of the WarehouseTransferLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the WarehouseTransferLine DTO.</returns>
	Task<ERPWarehouseTransferLineInformationDto> GetWarehouseTransferLine(Guid warehouseTransferLineId);

	/// <summary>
	/// Saves the provided ERP warehouseTransferLine.
	/// </summary>
	/// <param name="warehouseTransferLine">The ERP warehouseTransferLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveWarehouseTransferLine(ERPWarehouseTransferLineDto warehouseTransferLine);
}
