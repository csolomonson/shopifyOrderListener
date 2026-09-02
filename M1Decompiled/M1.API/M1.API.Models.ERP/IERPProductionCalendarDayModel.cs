using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPProductionCalendarDayModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ProductionCalendarDays with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductionCalendarDays to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllProductionCalendarDays(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ProductionCalendarDay information based on the specified ProductionCalendarDay Unique Id.
	/// </summary>
	/// <param name="productionCalendarDayId">The Unique Id of the ProductionCalendarDay.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetProductionCalendarDay(Guid productionCalendarDayId);

	/// <summary>
	/// Processes the request to retrieve all ProductionCalendarDays with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductionCalendarDays to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProductionCalendarDays DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPProductionCalendarDayDto>>> Process_GetAllProductionCalendarDays(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ProductionCalendarDay.
	/// </summary>
	/// <param name="productionCalendarDayId">The Unique Id of the ProductionCalendarDay to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ProductionCalendarDay DTO.</returns>
	Task<ERPResponseMessageDto<ERPProductionCalendarDayDto>> Process_GetProductionCalendarDay(Guid productionCalendarDayId);
}
