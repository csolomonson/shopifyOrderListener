using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPRMAReceiptComponentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all RMAReceiptComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAReceiptComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllRMAReceiptComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving RMAReceiptComponent information based on the specified RMAReceiptComponent Unique Id.
	/// </summary>
	/// <param name="rMAReceiptComponentId">The Unique Id of the RMAReceiptComponent.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetRMAReceiptComponent(Guid rMAReceiptComponentId);

	/// <summary>
	/// Validates the PUT request for creating or updating RMAReceiptComponent information based on the specified RMAReceiptComponent.
	/// </summary>
	/// <param name="rMAReceiptComponent">The RMAReceiptComponent details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutRMAReceiptComponent(ERPRMAReceiptComponentDto rMAReceiptComponent);

	/// <summary>
	/// Processes the request to retrieve all RMAReceiptComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAReceiptComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RMAReceiptComponents DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPRMAReceiptComponentDto>>> Process_GetAllRMAReceiptComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific RMAReceiptComponent.
	/// </summary>
	/// <param name="rMAReceiptComponentId">The Unique Id of the RMAReceiptComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the RMAReceiptComponent DTO.</returns>
	Task<ERPResponseMessageDto<ERPRMAReceiptComponentDto>> Process_GetRMAReceiptComponent(Guid rMAReceiptComponentId);

	/// <summary>
	/// Processes the creating or updating of a RMAReceiptComponent record.
	/// </summary>
	/// <param name="rMAReceiptComponent">The RMAReceiptComponent data transfer object (DTO) containing the details of the RMAReceiptComponent to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the RMAReceiptComponent details.</returns>
	Task<ERPResponseMessageDto<ERPRMAReceiptComponentDto>> Process_PutRMAReceiptComponent(ERPRMAReceiptComponentDto rMAReceiptComponent);

	/// <summary>
	/// Validates the request for deleting a RMAReceiptComponent record.
	/// </summary>
	/// <param name="rMAReceiptComponentId">The Unique Id of the RMAReceiptComponent.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteRMAReceiptComponent(Guid rMAReceiptComponentId);

	/// <summary>
	/// Processes the request to delete a RMAReceiptComponent record.
	/// </summary>
	/// <param name="rMAReceiptComponentId">The Unique Id of the RMAReceiptComponent.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPRMAReceiptComponentDto>> Process_DeleteRMAReceiptComponent(Guid rMAReceiptComponentId);
}
