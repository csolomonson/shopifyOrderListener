using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPSalesOrderJobLinkModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all SalesOrderJobLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderJobLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderJobLinks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving SalesOrderJobLink information based on the specified SalesOrderJobLink Unique Id.
	/// </summary>
	/// <param name="salesOrderJobLinkId">The Unique Id of the SalesOrderJobLink.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderJobLink(Guid salesOrderJobLinkId);

	/// <summary>
	/// Validates the PUT request for creating or updating SalesOrderJobLink information based on the specified SalesOrderJobLink.
	/// </summary>
	/// <param name="salesOrderJobLink">The SalesOrderJobLink details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderJobLink(ERPSalesOrderJobLinkDto salesOrderJobLink);

	/// <summary>
	/// Processes the request to retrieve all SalesOrderJobLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderJobLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderJobLinks DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPSalesOrderJobLinkDto>>> Process_GetAllSalesOrderJobLinks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific SalesOrderJobLink.
	/// </summary>
	/// <param name="salesOrderJobLinkId">The Unique Id of the SalesOrderJobLink to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the SalesOrderJobLink DTO.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderJobLinkDto>> Process_GetSalesOrderJobLink(Guid salesOrderJobLinkId);

	/// <summary>
	/// Processes the creating or updating of a SalesOrderJobLink record.
	/// </summary>
	/// <param name="salesOrderJobLink">The SalesOrderJobLink data transfer object (DTO) containing the details of the SalesOrderJobLink to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the SalesOrderJobLink details.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderJobLinkDto>> Process_PutSalesOrderJobLink(ERPSalesOrderJobLinkDto salesOrderJobLink);

	/// <summary>
	/// Validates the request for deleting a SalesOrderJobLink record.
	/// </summary>
	/// <param name="salesOrderJobLinkId">The Unique Id of the SalesOrderJobLink.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderJobLink(Guid salesOrderJobLinkId);

	/// <summary>
	/// Processes the request to delete a SalesOrderJobLink record.
	/// </summary>
	/// <param name="salesOrderJobLinkId">The Unique Id of the SalesOrderJobLink.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPSalesOrderJobLinkDto>> Process_DeleteSalesOrderJobLink(Guid salesOrderJobLinkId);
}
