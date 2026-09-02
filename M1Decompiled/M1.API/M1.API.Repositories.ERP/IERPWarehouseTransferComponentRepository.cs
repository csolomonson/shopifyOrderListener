using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPWarehouseTransferComponentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a WarehouseTransferComponent with the specified Unique Id exists.
	/// </summary>
	/// <param name="warehouseTransferComponentId">The Unique Id of the WarehouseTransferComponent to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the WarehouseTransferComponent exists or not.</returns>
	Task<bool> DoesWarehouseTransferComponentExist(Guid warehouseTransferComponentId);

	/// <summary>
	/// Retrieves all WarehouseTransferComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseTransferComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseTransferComponents DTOs.</returns>
	Task<ICollection<ERPWarehouseTransferComponentInformationDto>> GetAllWarehouseTransferComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific WarehouseTransferComponent.
	/// </summary>
	/// <param name="warehouseTransferComponentId">The Unique Id of the WarehouseTransferComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the WarehouseTransferComponent DTO.</returns>
	Task<ERPWarehouseTransferComponentInformationDto> GetWarehouseTransferComponent(Guid warehouseTransferComponentId);

	/// <summary>
	/// Saves the provided ERP warehouseTransferComponent.
	/// </summary>
	/// <param name="warehouseTransferComponent">The ERP warehouseTransferComponent to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveWarehouseTransferComponent(ERPWarehouseTransferComponentDto warehouseTransferComponent);
}
