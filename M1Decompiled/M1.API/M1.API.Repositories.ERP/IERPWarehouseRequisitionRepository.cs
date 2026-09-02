using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPWarehouseRequisitionRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a WarehouseRequisition with the specified Unique Id exists.
	/// </summary>
	/// <param name="warehouseRequisitionId">The Unique Id of the WarehouseRequisition to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the WarehouseRequisition exists or not.</returns>
	Task<bool> DoesWarehouseRequisitionExist(Guid warehouseRequisitionId);

	/// <summary>
	/// Retrieves all WarehouseRequisitions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseRequisitions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseRequisitions DTOs.</returns>
	Task<ICollection<ERPWarehouseRequisitionInformationDto>> GetAllWarehouseRequisitions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific WarehouseRequisition.
	/// </summary>
	/// <param name="warehouseRequisitionId">The Unique Id of the WarehouseRequisition to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the WarehouseRequisition DTO.</returns>
	Task<ERPWarehouseRequisitionInformationDto> GetWarehouseRequisition(Guid warehouseRequisitionId);

	/// <summary>
	/// Saves the provided ERP warehouseRequisition.
	/// </summary>
	/// <param name="warehouseRequisition">The ERP warehouseRequisition to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveWarehouseRequisition(ERPWarehouseRequisitionDto warehouseRequisition);
}
