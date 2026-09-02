using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPMfgReceiptModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all MfgReceipts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MfgReceipts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllMfgReceipts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving MfgReceipt information based on the specified MfgReceipt Unique Id.
	/// </summary>
	/// <param name="mfgReceiptId">The Unique Id of the MfgReceipt.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetMfgReceipt(Guid mfgReceiptId);

	/// <summary>
	/// Validates the PUT request for creating or updating MfgReceipt information based on the specified MfgReceipt.
	/// </summary>
	/// <param name="mfgReceipt">The MfgReceipt details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutMfgReceipt(ERPMfgReceiptDto mfgReceipt);

	/// <summary>
	/// Processes the request to retrieve all MfgReceipts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MfgReceipts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MfgReceipts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPMfgReceiptDto>>> Process_GetAllMfgReceipts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific MfgReceipt.
	/// </summary>
	/// <param name="mfgReceiptId">The Unique Id of the MfgReceipt to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the MfgReceipt DTO.</returns>
	Task<ERPResponseMessageDto<ERPMfgReceiptDto>> Process_GetMfgReceipt(Guid mfgReceiptId);

	/// <summary>
	/// Processes the creating or updating of a MfgReceipt record.
	/// </summary>
	/// <param name="mfgReceipt">The MfgReceipt data transfer object (DTO) containing the details of the MfgReceipt to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the MfgReceipt details.</returns>
	Task<ERPResponseMessageDto<ERPMfgReceiptDto>> Process_PutMfgReceipt(ERPMfgReceiptDto mfgReceipt);

	/// <summary>
	/// Validates the request for deleting a MfgReceipt record.
	/// </summary>
	/// <param name="mfgReceiptId">The Unique Id of the MfgReceipt.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteMfgReceipt(Guid mfgReceiptId);

	/// <summary>
	/// Processes the request to delete a MfgReceipt record.
	/// </summary>
	/// <param name="mfgReceiptId">The Unique Id of the MfgReceipt.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPMfgReceiptDto>> Process_DeleteMfgReceipt(Guid mfgReceiptId);
}
