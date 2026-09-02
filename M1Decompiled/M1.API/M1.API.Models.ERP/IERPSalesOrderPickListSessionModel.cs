using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPSalesOrderPickListSessionModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all SalesOrderPickListSessions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderPickListSessions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderPickListSessions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving SalesOrderPickListSession information based on the specified SalesOrderPickListSession Unique Id.
	/// </summary>
	/// <param name="salesOrderPickListSessionId">The Unique Id of the SalesOrderPickListSession.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderPickListSession(Guid salesOrderPickListSessionId);

	/// <summary>
	/// Validates the PUT request for creating or updating SalesOrderPickListSession information based on the specified SalesOrderPickListSession.
	/// </summary>
	/// <param name="salesOrderPickListSession">The SalesOrderPickListSession details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderPickListSession(ERPSalesOrderPickListSessionDto salesOrderPickListSession);

	/// <summary>
	/// Processes the request to retrieve all SalesOrderPickListSessions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderPickListSessions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderPickListSessions DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPSalesOrderPickListSessionDto>>> Process_GetAllSalesOrderPickListSessions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific SalesOrderPickListSession.
	/// </summary>
	/// <param name="salesOrderPickListSessionId">The Unique Id of the SalesOrderPickListSession to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the SalesOrderPickListSession DTO.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderPickListSessionDto>> Process_GetSalesOrderPickListSession(Guid salesOrderPickListSessionId);

	/// <summary>
	/// Processes the creating or updating of a SalesOrderPickListSession record.
	/// </summary>
	/// <param name="salesOrderPickListSession">The SalesOrderPickListSession data transfer object (DTO) containing the details of the SalesOrderPickListSession to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the SalesOrderPickListSession details.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderPickListSessionDto>> Process_PutSalesOrderPickListSession(ERPSalesOrderPickListSessionDto salesOrderPickListSession);

	/// <summary>
	/// Validates the request for deleting a SalesOrderPickListSession record.
	/// </summary>
	/// <param name="salesOrderPickListSessionId">The Unique Id of the SalesOrderPickListSession.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderPickListSession(Guid salesOrderPickListSessionId);

	/// <summary>
	/// Processes the request to delete a SalesOrderPickListSession record.
	/// </summary>
	/// <param name="salesOrderPickListSessionId">The Unique Id of the SalesOrderPickListSession.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPSalesOrderPickListSessionDto>> Process_DeleteSalesOrderPickListSession(Guid salesOrderPickListSessionId);
}
