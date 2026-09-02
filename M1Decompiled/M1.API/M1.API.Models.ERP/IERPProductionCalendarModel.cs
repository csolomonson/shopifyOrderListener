using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPProductionCalendarModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ProductionCalendars with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductionCalendars to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllProductionCalendars(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ProductionCalendar information based on the specified ProductionCalendar Unique Id.
	/// </summary>
	/// <param name="productionCalendarId">The Unique Id of the ProductionCalendar.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetProductionCalendar(Guid productionCalendarId);

	/// <summary>
	/// Processes the request to retrieve all ProductionCalendars with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductionCalendars to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProductionCalendars DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPProductionCalendarDto>>> Process_GetAllProductionCalendars(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ProductionCalendar.
	/// </summary>
	/// <param name="productionCalendarId">The Unique Id of the ProductionCalendar to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ProductionCalendar DTO.</returns>
	Task<ERPResponseMessageDto<ERPProductionCalendarDto>> Process_GetProductionCalendar(Guid productionCalendarId);
}
