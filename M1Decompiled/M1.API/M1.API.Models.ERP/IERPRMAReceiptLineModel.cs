using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPRMAReceiptLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all RMAReceiptLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAReceiptLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllRMAReceiptLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving RMAReceiptLine information based on the specified RMAReceiptLine Unique Id.
	/// </summary>
	/// <param name="rMAReceiptLineId">The Unique Id of the RMAReceiptLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetRMAReceiptLine(Guid rMAReceiptLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating RMAReceiptLine information based on the specified RMAReceiptLine.
	/// </summary>
	/// <param name="rMAReceiptLine">The RMAReceiptLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutRMAReceiptLine(ERPRMAReceiptLineDto rMAReceiptLine);

	/// <summary>
	/// Processes the request to retrieve all RMAReceiptLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAReceiptLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RMAReceiptLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPRMAReceiptLineDto>>> Process_GetAllRMAReceiptLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific RMAReceiptLine.
	/// </summary>
	/// <param name="rMAReceiptLineId">The Unique Id of the RMAReceiptLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the RMAReceiptLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPRMAReceiptLineDto>> Process_GetRMAReceiptLine(Guid rMAReceiptLineId);

	/// <summary>
	/// Processes the creating or updating of a RMAReceiptLine record.
	/// </summary>
	/// <param name="rMAReceiptLine">The RMAReceiptLine data transfer object (DTO) containing the details of the RMAReceiptLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the RMAReceiptLine details.</returns>
	Task<ERPResponseMessageDto<ERPRMAReceiptLineDto>> Process_PutRMAReceiptLine(ERPRMAReceiptLineDto rMAReceiptLine);

	/// <summary>
	/// Validates the request for deleting a RMAReceiptLine record.
	/// </summary>
	/// <param name="rMAReceiptLineId">The Unique Id of the RMAReceiptLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteRMAReceiptLine(Guid rMAReceiptLineId);

	/// <summary>
	/// Processes the request to delete a RMAReceiptLine record.
	/// </summary>
	/// <param name="rMAReceiptLineId">The Unique Id of the RMAReceiptLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPRMAReceiptLineDto>> Process_DeleteRMAReceiptLine(Guid rMAReceiptLineId);
}
