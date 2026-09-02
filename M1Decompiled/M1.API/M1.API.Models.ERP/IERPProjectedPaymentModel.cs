using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPProjectedPaymentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ProjectedPayments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProjectedPayments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllProjectedPayments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ProjectedPayment information based on the specified ProjectedPayment Unique Id.
	/// </summary>
	/// <param name="projectedPaymentId">The Unique Id of the ProjectedPayment.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetProjectedPayment(Guid projectedPaymentId);

	/// <summary>
	/// Validates the PUT request for creating or updating ProjectedPayment information based on the specified ProjectedPayment.
	/// </summary>
	/// <param name="projectedPayment">The ProjectedPayment details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutProjectedPayment(ERPProjectedPaymentDto projectedPayment);

	/// <summary>
	/// Processes the request to retrieve all ProjectedPayments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProjectedPayments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProjectedPayments DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPProjectedPaymentDto>>> Process_GetAllProjectedPayments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ProjectedPayment.
	/// </summary>
	/// <param name="projectedPaymentId">The Unique Id of the ProjectedPayment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ProjectedPayment DTO.</returns>
	Task<ERPResponseMessageDto<ERPProjectedPaymentDto>> Process_GetProjectedPayment(Guid projectedPaymentId);

	/// <summary>
	/// Processes the creating or updating of a ProjectedPayment record.
	/// </summary>
	/// <param name="projectedPayment">The ProjectedPayment data transfer object (DTO) containing the details of the ProjectedPayment to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ProjectedPayment details.</returns>
	Task<ERPResponseMessageDto<ERPProjectedPaymentDto>> Process_PutProjectedPayment(ERPProjectedPaymentDto projectedPayment);

	/// <summary>
	/// Validates the request for deleting a ProjectedPayment record.
	/// </summary>
	/// <param name="projectedPaymentId">The Unique Id of the ProjectedPayment.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteProjectedPayment(Guid projectedPaymentId);

	/// <summary>
	/// Processes the request to delete a ProjectedPayment record.
	/// </summary>
	/// <param name="projectedPaymentId">The Unique Id of the ProjectedPayment.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPProjectedPaymentDto>> Process_DeleteProjectedPayment(Guid projectedPaymentId);
}
