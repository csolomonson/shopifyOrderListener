using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPaymentMethodModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PaymentMethods with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PaymentMethods to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPaymentMethods(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PaymentMethod information based on the specified PaymentMethod Unique Id.
	/// </summary>
	/// <param name="paymentMethodId">The Unique Id of the PaymentMethod.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPaymentMethod(Guid paymentMethodId);

	/// <summary>
	/// Validates the PUT request for creating or updating PaymentMethod information based on the specified PaymentMethod.
	/// </summary>
	/// <param name="paymentMethod">The PaymentMethod details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPaymentMethod(ERPPaymentMethodDto paymentMethod);

	/// <summary>
	/// Processes the request to retrieve all PaymentMethods with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PaymentMethods to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PaymentMethods DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPaymentMethodDto>>> Process_GetAllPaymentMethods(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PaymentMethod.
	/// </summary>
	/// <param name="paymentMethodId">The Unique Id of the PaymentMethod to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PaymentMethod DTO.</returns>
	Task<ERPResponseMessageDto<ERPPaymentMethodDto>> Process_GetPaymentMethod(Guid paymentMethodId);

	/// <summary>
	/// Processes the creating or updating of a PaymentMethod record.
	/// </summary>
	/// <param name="paymentMethod">The PaymentMethod data transfer object (DTO) containing the details of the PaymentMethod to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PaymentMethod details.</returns>
	Task<ERPResponseMessageDto<ERPPaymentMethodDto>> Process_PutPaymentMethod(ERPPaymentMethodDto paymentMethod);

	/// <summary>
	/// Validates the request for deleting a PaymentMethod record.
	/// </summary>
	/// <param name="paymentMethodId">The Unique Id of the PaymentMethod.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePaymentMethod(Guid paymentMethodId);

	/// <summary>
	/// Processes the request to delete a PaymentMethod record.
	/// </summary>
	/// <param name="paymentMethodId">The Unique Id of the PaymentMethod.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPaymentMethodDto>> Process_DeletePaymentMethod(Guid paymentMethodId);
}
