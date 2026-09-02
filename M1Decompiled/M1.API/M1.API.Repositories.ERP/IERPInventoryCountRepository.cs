using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPInventoryCountRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a InventoryCount with the specified Unique Id exists.
	/// </summary>
	/// <param name="inventoryCountId">The Unique Id of the InventoryCount to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the InventoryCount exists or not.</returns>
	Task<bool> DoesInventoryCountExist(Guid inventoryCountId);

	/// <summary>
	/// Retrieves all InventoryCounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of InventoryCounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of InventoryCounts DTOs.</returns>
	Task<ICollection<ERPInventoryCountInformationDto>> GetAllInventoryCounts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific InventoryCount.
	/// </summary>
	/// <param name="inventoryCountId">The Unique Id of the InventoryCount to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the InventoryCount DTO.</returns>
	Task<ERPInventoryCountInformationDto> GetInventoryCount(Guid inventoryCountId);

	/// <summary>
	/// Saves the provided ERP inventoryCount.
	/// </summary>
	/// <param name="inventoryCount">The ERP inventoryCount to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveInventoryCount(ERPInventoryCountDto inventoryCount);
}
