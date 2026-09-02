using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPMfgReceiptComponentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all MfgReceiptComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MfgReceiptComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllMfgReceiptComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving MfgReceiptComponent information based on the specified MfgReceiptComponent Unique Id.
	/// </summary>
	/// <param name="mfgReceiptComponentId">The Unique Id of the MfgReceiptComponent.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetMfgReceiptComponent(Guid mfgReceiptComponentId);

	/// <summary>
	/// Validates the PUT request for creating or updating MfgReceiptComponent information based on the specified MfgReceiptComponent.
	/// </summary>
	/// <param name="mfgReceiptComponent">The MfgReceiptComponent details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutMfgReceiptComponent(ERPMfgReceiptComponentDto mfgReceiptComponent);

	/// <summary>
	/// Processes the request to retrieve all MfgReceiptComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MfgReceiptComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MfgReceiptComponents DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPMfgReceiptComponentDto>>> Process_GetAllMfgReceiptComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific MfgReceiptComponent.
	/// </summary>
	/// <param name="mfgReceiptComponentId">The Unique Id of the MfgReceiptComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the MfgReceiptComponent DTO.</returns>
	Task<ERPResponseMessageDto<ERPMfgReceiptComponentDto>> Process_GetMfgReceiptComponent(Guid mfgReceiptComponentId);

	/// <summary>
	/// Processes the creating or updating of a MfgReceiptComponent record.
	/// </summary>
	/// <param name="mfgReceiptComponent">The MfgReceiptComponent data transfer object (DTO) containing the details of the MfgReceiptComponent to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the MfgReceiptComponent details.</returns>
	Task<ERPResponseMessageDto<ERPMfgReceiptComponentDto>> Process_PutMfgReceiptComponent(ERPMfgReceiptComponentDto mfgReceiptComponent);

	/// <summary>
	/// Validates the request for deleting a MfgReceiptComponent record.
	/// </summary>
	/// <param name="mfgReceiptComponentId">The Unique Id of the MfgReceiptComponent.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteMfgReceiptComponent(Guid mfgReceiptComponentId);

	/// <summary>
	/// Processes the request to delete a MfgReceiptComponent record.
	/// </summary>
	/// <param name="mfgReceiptComponentId">The Unique Id of the MfgReceiptComponent.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPMfgReceiptComponentDto>> Process_DeleteMfgReceiptComponent(Guid mfgReceiptComponentId);
}
