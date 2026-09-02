using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPSalesOrderSalesPersonModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all SalesOrderSalesPeople with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderSalesPeople to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving SalesOrderSalesPerson information based on the specified SalesOrderSalesPerson Unique Id.
	/// </summary>
	/// <param name="salesOrderSalesPersonId">The Unique Id of the SalesOrderSalesPerson.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderSalesPerson(Guid salesOrderSalesPersonId);

	/// <summary>
	/// Validates the PUT request for creating or updating SalesOrderSalesPerson information based on the specified SalesOrderSalesPerson.
	/// </summary>
	/// <param name="salesOrderSalesPerson">The SalesOrderSalesPerson details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderSalesPerson(ERPSalesOrderSalesPersonDto salesOrderSalesPerson);

	/// <summary>
	/// Processes the request to retrieve all SalesOrderSalesPeople with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderSalesPeople to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderSalesPeople DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPSalesOrderSalesPersonDto>>> Process_GetAllSalesOrderSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific SalesOrderSalesPerson.
	/// </summary>
	/// <param name="salesOrderSalesPersonId">The Unique Id of the SalesOrderSalesPerson to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the SalesOrderSalesPerson DTO.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderSalesPersonDto>> Process_GetSalesOrderSalesPerson(Guid salesOrderSalesPersonId);

	/// <summary>
	/// Processes the creating or updating of a SalesOrderSalesPerson record.
	/// </summary>
	/// <param name="salesOrderSalesPerson">The SalesOrderSalesPerson data transfer object (DTO) containing the details of the SalesOrderSalesPerson to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the SalesOrderSalesPerson details.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderSalesPersonDto>> Process_PutSalesOrderSalesPerson(ERPSalesOrderSalesPersonDto salesOrderSalesPerson);

	/// <summary>
	/// Validates the request for deleting a SalesOrderSalesPerson record.
	/// </summary>
	/// <param name="salesOrderSalesPersonId">The Unique Id of the SalesOrderSalesPerson.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderSalesPerson(Guid salesOrderSalesPersonId);

	/// <summary>
	/// Processes the request to delete a SalesOrderSalesPerson record.
	/// </summary>
	/// <param name="salesOrderSalesPersonId">The Unique Id of the SalesOrderSalesPerson.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPSalesOrderSalesPersonDto>> Process_DeleteSalesOrderSalesPerson(Guid salesOrderSalesPersonId);
}
