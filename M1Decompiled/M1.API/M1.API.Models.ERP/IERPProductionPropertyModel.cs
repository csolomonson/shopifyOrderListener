using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPProductionPropertyModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ProductionProperties with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductionProperties to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllProductionProperties(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ProductionProperty information based on the specified ProductionProperty Unique Id.
	/// </summary>
	/// <param name="productionPropertyId">The Unique Id of the ProductionProperty.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetProductionProperty(Guid productionPropertyId);

	/// <summary>
	/// Processes the request to retrieve all ProductionProperties with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductionProperties to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProductionProperties DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPProductionPropertyDto>>> Process_GetAllProductionProperties(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ProductionProperty.
	/// </summary>
	/// <param name="productionPropertyId">The Unique Id of the ProductionProperty to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ProductionProperty DTO.</returns>
	Task<ERPResponseMessageDto<ERPProductionPropertyDto>> Process_GetProductionProperty(Guid productionPropertyId);
}
