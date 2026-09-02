using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPWarehouseRequisitionComponentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a WarehouseRequisitionComponent with the specified Unique Id exists.
	/// </summary>
	/// <param name="warehouseRequisitionComponentId">The Unique Id of the WarehouseRequisitionComponent to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the WarehouseRequisitionComponent exists or not.</returns>
	Task<bool> DoesWarehouseRequisitionComponentExist(Guid warehouseRequisitionComponentId);

	/// <summary>
	/// Retrieves all WarehouseRequisitionComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseRequisitionComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseRequisitionComponents DTOs.</returns>
	Task<ICollection<ERPWarehouseRequisitionComponentInformationDto>> GetAllWarehouseRequisitionComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific WarehouseRequisitionComponent.
	/// </summary>
	/// <param name="warehouseRequisitionComponentId">The Unique Id of the WarehouseRequisitionComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the WarehouseRequisitionComponent DTO.</returns>
	Task<ERPWarehouseRequisitionComponentInformationDto> GetWarehouseRequisitionComponent(Guid warehouseRequisitionComponentId);

	/// <summary>
	/// Saves the provided ERP warehouseRequisitionComponent.
	/// </summary>
	/// <param name="warehouseRequisitionComponent">The ERP warehouseRequisitionComponent to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveWarehouseRequisitionComponent(ERPWarehouseRequisitionComponentDto warehouseRequisitionComponent);
}
