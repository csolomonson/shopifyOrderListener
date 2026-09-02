using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPInventoryCountLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a InventoryCountLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="inventoryCountLineId">The Unique Id of the InventoryCountLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the InventoryCountLine exists or not.</returns>
	Task<bool> DoesInventoryCountLineExist(Guid inventoryCountLineId);

	/// <summary>
	/// Retrieves all InventoryCountLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of InventoryCountLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of InventoryCountLines DTOs.</returns>
	Task<ICollection<ERPInventoryCountLineInformationDto>> GetAllInventoryCountLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific InventoryCountLine.
	/// </summary>
	/// <param name="inventoryCountLineId">The Unique Id of the InventoryCountLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the InventoryCountLine DTO.</returns>
	Task<ERPInventoryCountLineInformationDto> GetInventoryCountLine(Guid inventoryCountLineId);

	/// <summary>
	/// Saves the provided ERP inventoryCountLine.
	/// </summary>
	/// <param name="inventoryCountLine">The ERP inventoryCountLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveInventoryCountLine(ERPInventoryCountLineDto inventoryCountLine);
}
