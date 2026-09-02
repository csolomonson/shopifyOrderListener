using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPProductionCalendarWorkCenterModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ProductionCalendarWorkCenters with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductionCalendarWorkCenters to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllProductionCalendarWorkCenters(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ProductionCalendarWorkCenter information based on the specified ProductionCalendarWorkCenter Unique Id.
	/// </summary>
	/// <param name="productionCalendarWorkCenterId">The Unique Id of the ProductionCalendarWorkCenter.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetProductionCalendarWorkCenter(Guid productionCalendarWorkCenterId);

	/// <summary>
	/// Processes the request to retrieve all ProductionCalendarWorkCenters with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductionCalendarWorkCenters to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProductionCalendarWorkCenters DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPProductionCalendarWorkCenterDto>>> Process_GetAllProductionCalendarWorkCenters(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ProductionCalendarWorkCenter.
	/// </summary>
	/// <param name="productionCalendarWorkCenterId">The Unique Id of the ProductionCalendarWorkCenter to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ProductionCalendarWorkCenter DTO.</returns>
	Task<ERPResponseMessageDto<ERPProductionCalendarWorkCenterDto>> Process_GetProductionCalendarWorkCenter(Guid productionCalendarWorkCenterId);
}
