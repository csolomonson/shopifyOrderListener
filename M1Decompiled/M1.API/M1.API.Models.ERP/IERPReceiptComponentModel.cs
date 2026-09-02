using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPReceiptComponentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ReceiptComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ReceiptComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllReceiptComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ReceiptComponent information based on the specified ReceiptComponent Unique Id.
	/// </summary>
	/// <param name="receiptComponentId">The Unique Id of the ReceiptComponent.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetReceiptComponent(Guid receiptComponentId);

	/// <summary>
	/// Validates the PUT request for creating or updating ReceiptComponent information based on the specified ReceiptComponent.
	/// </summary>
	/// <param name="receiptComponent">The ReceiptComponent details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutReceiptComponent(ERPReceiptComponentDto receiptComponent);

	/// <summary>
	/// Processes the request to retrieve all ReceiptComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ReceiptComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ReceiptComponents DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPReceiptComponentDto>>> Process_GetAllReceiptComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ReceiptComponent.
	/// </summary>
	/// <param name="receiptComponentId">The Unique Id of the ReceiptComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ReceiptComponent DTO.</returns>
	Task<ERPResponseMessageDto<ERPReceiptComponentDto>> Process_GetReceiptComponent(Guid receiptComponentId);

	/// <summary>
	/// Processes the creating or updating of a ReceiptComponent record.
	/// </summary>
	/// <param name="receiptComponent">The ReceiptComponent data transfer object (DTO) containing the details of the ReceiptComponent to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ReceiptComponent details.</returns>
	Task<ERPResponseMessageDto<ERPReceiptComponentDto>> Process_PutReceiptComponent(ERPReceiptComponentDto receiptComponent);

	/// <summary>
	/// Validates the request for deleting a ReceiptComponent record.
	/// </summary>
	/// <param name="receiptComponentId">The Unique Id of the ReceiptComponent.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteReceiptComponent(Guid receiptComponentId);

	/// <summary>
	/// Processes the request to delete a ReceiptComponent record.
	/// </summary>
	/// <param name="receiptComponentId">The Unique Id of the ReceiptComponent.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPReceiptComponentDto>> Process_DeleteReceiptComponent(Guid receiptComponentId);
}
