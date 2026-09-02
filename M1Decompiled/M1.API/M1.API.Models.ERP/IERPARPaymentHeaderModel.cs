using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPARPaymentHeaderModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ARPaymentHeaders with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARPaymentHeaders to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllARPaymentHeaders(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ARPaymentHeader information based on the specified ARPaymentHeader Unique Id.
	/// </summary>
	/// <param name="aRPaymentHeaderId">The Unique Id of the ARPaymentHeader.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetARPaymentHeader(Guid aRPaymentHeaderId);

	/// <summary>
	/// Validates the PUT request for creating or updating ARPaymentHeader information based on the specified ARPaymentHeader.
	/// </summary>
	/// <param name="aRPaymentHeader">The ARPaymentHeader details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutARPaymentHeader(ERPARPaymentHeaderDto aRPaymentHeader);

	/// <summary>
	/// Processes the request to retrieve all ARPaymentHeaders with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARPaymentHeaders to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ARPaymentHeaders DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPARPaymentHeaderDto>>> Process_GetAllARPaymentHeaders(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ARPaymentHeader.
	/// </summary>
	/// <param name="aRPaymentHeaderId">The Unique Id of the ARPaymentHeader to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ARPaymentHeader DTO.</returns>
	Task<ERPResponseMessageDto<ERPARPaymentHeaderDto>> Process_GetARPaymentHeader(Guid aRPaymentHeaderId);

	/// <summary>
	/// Processes the creating or updating of a ARPaymentHeader record.
	/// </summary>
	/// <param name="aRPaymentHeader">The ARPaymentHeader data transfer object (DTO) containing the details of the ARPaymentHeader to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ARPaymentHeader details.</returns>
	Task<ERPResponseMessageDto<ERPARPaymentHeaderDto>> Process_PutARPaymentHeader(ERPARPaymentHeaderDto aRPaymentHeader);

	/// <summary>
	/// Validates the request for deleting a ARPaymentHeader record.
	/// </summary>
	/// <param name="aRPaymentHeaderId">The Unique Id of the ARPaymentHeader.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteARPaymentHeader(Guid aRPaymentHeaderId);

	/// <summary>
	/// Processes the request to delete a ARPaymentHeader record.
	/// </summary>
	/// <param name="aRPaymentHeaderId">The Unique Id of the ARPaymentHeader.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPARPaymentHeaderDto>> Process_DeleteARPaymentHeader(Guid aRPaymentHeaderId);
}
