using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPSalesOrderApprovalModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all SalesOrderApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderApprovals(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving SalesOrderApproval information based on the specified SalesOrderApproval Unique Id.
	/// </summary>
	/// <param name="salesOrderApprovalId">The Unique Id of the SalesOrderApproval.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderApproval(Guid salesOrderApprovalId);

	/// <summary>
	/// Validates the PUT request for creating or updating SalesOrderApproval information based on the specified SalesOrderApproval.
	/// </summary>
	/// <param name="salesOrderApproval">The SalesOrderApproval details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderApproval(ERPSalesOrderApprovalDto salesOrderApproval);

	/// <summary>
	/// Processes the request to retrieve all SalesOrderApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderApprovals DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPSalesOrderApprovalDto>>> Process_GetAllSalesOrderApprovals(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific SalesOrderApproval.
	/// </summary>
	/// <param name="salesOrderApprovalId">The Unique Id of the SalesOrderApproval to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the SalesOrderApproval DTO.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderApprovalDto>> Process_GetSalesOrderApproval(Guid salesOrderApprovalId);

	/// <summary>
	/// Processes the creating or updating of a SalesOrderApproval record.
	/// </summary>
	/// <param name="salesOrderApproval">The SalesOrderApproval data transfer object (DTO) containing the details of the SalesOrderApproval to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the SalesOrderApproval details.</returns>
	Task<ERPResponseMessageDto<ERPSalesOrderApprovalDto>> Process_PutSalesOrderApproval(ERPSalesOrderApprovalDto salesOrderApproval);

	/// <summary>
	/// Validates the request for deleting a SalesOrderApproval record.
	/// </summary>
	/// <param name="salesOrderApprovalId">The Unique Id of the SalesOrderApproval.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderApproval(Guid salesOrderApprovalId);

	/// <summary>
	/// Processes the request to delete a SalesOrderApproval record.
	/// </summary>
	/// <param name="salesOrderApprovalId">The Unique Id of the SalesOrderApproval.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPSalesOrderApprovalDto>> Process_DeleteSalesOrderApproval(Guid salesOrderApprovalId);
}
