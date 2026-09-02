using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAPPaymentHeaderModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all APPaymentHeaders with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APPaymentHeaders to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAPPaymentHeaders(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving APPaymentHeader information based on the specified APPaymentHeader Unique Id.
	/// </summary>
	/// <param name="aPPaymentHeaderId">The Unique Id of the APPaymentHeader.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAPPaymentHeader(Guid aPPaymentHeaderId);

	/// <summary>
	/// Validates the PUT request for creating or updating APPaymentHeader information based on the specified APPaymentHeader.
	/// </summary>
	/// <param name="aPPaymentHeader">The APPaymentHeader details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutAPPaymentHeader(ERPAPPaymentHeaderDto aPPaymentHeader);

	/// <summary>
	/// Processes the request to retrieve all APPaymentHeaders with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APPaymentHeaders to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of APPaymentHeaders DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAPPaymentHeaderDto>>> Process_GetAllAPPaymentHeaders(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific APPaymentHeader.
	/// </summary>
	/// <param name="aPPaymentHeaderId">The Unique Id of the APPaymentHeader to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the APPaymentHeader DTO.</returns>
	Task<ERPResponseMessageDto<ERPAPPaymentHeaderDto>> Process_GetAPPaymentHeader(Guid aPPaymentHeaderId);

	/// <summary>
	/// Processes the creating or updating of a APPaymentHeader record.
	/// </summary>
	/// <param name="aPPaymentHeader">The APPaymentHeader data transfer object (DTO) containing the details of the APPaymentHeader to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the APPaymentHeader details.</returns>
	Task<ERPResponseMessageDto<ERPAPPaymentHeaderDto>> Process_PutAPPaymentHeader(ERPAPPaymentHeaderDto aPPaymentHeader);

	/// <summary>
	/// Validates the request for deleting a APPaymentHeader record.
	/// </summary>
	/// <param name="aPPaymentHeaderId">The Unique Id of the APPaymentHeader.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteAPPaymentHeader(Guid aPPaymentHeaderId);

	/// <summary>
	/// Processes the request to delete a APPaymentHeader record.
	/// </summary>
	/// <param name="aPPaymentHeaderId">The Unique Id of the APPaymentHeader.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPAPPaymentHeaderDto>> Process_DeleteAPPaymentHeader(Guid aPPaymentHeaderId);
}
