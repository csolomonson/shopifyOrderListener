using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPurchasePlannerLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PurchasePlannerLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchasePlannerLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPurchasePlannerLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PurchasePlannerLine information based on the specified PurchasePlannerLine Unique Id.
	/// </summary>
	/// <param name="purchasePlannerLineId">The Unique Id of the PurchasePlannerLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPurchasePlannerLine(Guid purchasePlannerLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating PurchasePlannerLine information based on the specified PurchasePlannerLine.
	/// </summary>
	/// <param name="purchasePlannerLine">The PurchasePlannerLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPurchasePlannerLine(ERPPurchasePlannerLineDto purchasePlannerLine);

	/// <summary>
	/// Processes the request to retrieve all PurchasePlannerLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchasePlannerLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchasePlannerLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPurchasePlannerLineDto>>> Process_GetAllPurchasePlannerLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PurchasePlannerLine.
	/// </summary>
	/// <param name="purchasePlannerLineId">The Unique Id of the PurchasePlannerLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PurchasePlannerLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPPurchasePlannerLineDto>> Process_GetPurchasePlannerLine(Guid purchasePlannerLineId);

	/// <summary>
	/// Processes the creating or updating of a PurchasePlannerLine record.
	/// </summary>
	/// <param name="purchasePlannerLine">The PurchasePlannerLine data transfer object (DTO) containing the details of the PurchasePlannerLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PurchasePlannerLine details.</returns>
	Task<ERPResponseMessageDto<ERPPurchasePlannerLineDto>> Process_PutPurchasePlannerLine(ERPPurchasePlannerLineDto purchasePlannerLine);

	/// <summary>
	/// Validates the request for deleting a PurchasePlannerLine record.
	/// </summary>
	/// <param name="purchasePlannerLineId">The Unique Id of the PurchasePlannerLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePurchasePlannerLine(Guid purchasePlannerLineId);

	/// <summary>
	/// Processes the request to delete a PurchasePlannerLine record.
	/// </summary>
	/// <param name="purchasePlannerLineId">The Unique Id of the PurchasePlannerLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPurchasePlannerLineDto>> Process_DeletePurchasePlannerLine(Guid purchasePlannerLineId);
}
