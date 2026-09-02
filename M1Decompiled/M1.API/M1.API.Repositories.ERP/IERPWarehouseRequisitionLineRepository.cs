using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPWarehouseRequisitionLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a WarehouseRequisitionLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="warehouseRequisitionLineId">The Unique Id of the WarehouseRequisitionLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the WarehouseRequisitionLine exists or not.</returns>
	Task<bool> DoesWarehouseRequisitionLineExist(Guid warehouseRequisitionLineId);

	/// <summary>
	/// Retrieves all WarehouseRequisitionLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseRequisitionLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseRequisitionLines DTOs.</returns>
	Task<ICollection<ERPWarehouseRequisitionLineInformationDto>> GetAllWarehouseRequisitionLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific WarehouseRequisitionLine.
	/// </summary>
	/// <param name="warehouseRequisitionLineId">The Unique Id of the WarehouseRequisitionLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the WarehouseRequisitionLine DTO.</returns>
	Task<ERPWarehouseRequisitionLineInformationDto> GetWarehouseRequisitionLine(Guid warehouseRequisitionLineId);

	/// <summary>
	/// Saves the provided ERP warehouseRequisitionLine.
	/// </summary>
	/// <param name="warehouseRequisitionLine">The ERP warehouseRequisitionLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveWarehouseRequisitionLine(ERPWarehouseRequisitionLineDto warehouseRequisitionLine);
}
