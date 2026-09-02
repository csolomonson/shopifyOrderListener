using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPQuantityAdjustmentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all QuantityAdjustments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuantityAdjustments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllQuantityAdjustments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving QuantityAdjustment information based on the specified QuantityAdjustment Unique Id.
	/// </summary>
	/// <param name="quantityAdjustmentId">The Unique Id of the QuantityAdjustment.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetQuantityAdjustment(Guid quantityAdjustmentId);

	/// <summary>
	/// Validates the PUT request for creating or updating QuantityAdjustment information based on the specified QuantityAdjustment.
	/// </summary>
	/// <param name="quantityAdjustment">The QuantityAdjustment details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutQuantityAdjustment(ERPQuantityAdjustmentDto quantityAdjustment);

	/// <summary>
	/// Processes the request to retrieve all QuantityAdjustments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuantityAdjustments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of QuantityAdjustments DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPQuantityAdjustmentDto>>> Process_GetAllQuantityAdjustments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific QuantityAdjustment.
	/// </summary>
	/// <param name="quantityAdjustmentId">The Unique Id of the QuantityAdjustment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the QuantityAdjustment DTO.</returns>
	Task<ERPResponseMessageDto<ERPQuantityAdjustmentDto>> Process_GetQuantityAdjustment(Guid quantityAdjustmentId);

	/// <summary>
	/// Processes the creating or updating of a QuantityAdjustment record.
	/// </summary>
	/// <param name="quantityAdjustment">The QuantityAdjustment data transfer object (DTO) containing the details of the QuantityAdjustment to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the QuantityAdjustment details.</returns>
	Task<ERPResponseMessageDto<ERPQuantityAdjustmentDto>> Process_PutQuantityAdjustment(ERPQuantityAdjustmentDto quantityAdjustment);

	/// <summary>
	/// Validates the request for deleting a QuantityAdjustment record.
	/// </summary>
	/// <param name="quantityAdjustmentId">The Unique Id of the QuantityAdjustment.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteQuantityAdjustment(Guid quantityAdjustmentId);

	/// <summary>
	/// Processes the request to delete a QuantityAdjustment record.
	/// </summary>
	/// <param name="quantityAdjustmentId">The Unique Id of the QuantityAdjustment.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPQuantityAdjustmentDto>> Process_DeleteQuantityAdjustment(Guid quantityAdjustmentId);
}
