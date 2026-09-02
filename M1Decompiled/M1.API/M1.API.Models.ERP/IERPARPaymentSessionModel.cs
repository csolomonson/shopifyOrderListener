using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPARPaymentSessionModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ARPaymentSessions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARPaymentSessions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllARPaymentSessions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ARPaymentSession information based on the specified ARPaymentSession Unique Id.
	/// </summary>
	/// <param name="aRPaymentSessionId">The Unique Id of the ARPaymentSession.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetARPaymentSession(Guid aRPaymentSessionId);

	/// <summary>
	/// Validates the PUT request for creating or updating ARPaymentSession information based on the specified ARPaymentSession.
	/// </summary>
	/// <param name="aRPaymentSession">The ARPaymentSession details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutARPaymentSession(ERPARPaymentSessionDto aRPaymentSession);

	/// <summary>
	/// Processes the request to retrieve all ARPaymentSessions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARPaymentSessions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ARPaymentSessions DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPARPaymentSessionDto>>> Process_GetAllARPaymentSessions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ARPaymentSession.
	/// </summary>
	/// <param name="aRPaymentSessionId">The Unique Id of the ARPaymentSession to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ARPaymentSession DTO.</returns>
	Task<ERPResponseMessageDto<ERPARPaymentSessionDto>> Process_GetARPaymentSession(Guid aRPaymentSessionId);

	/// <summary>
	/// Processes the creating or updating of a ARPaymentSession record.
	/// </summary>
	/// <param name="aRPaymentSession">The ARPaymentSession data transfer object (DTO) containing the details of the ARPaymentSession to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ARPaymentSession details.</returns>
	Task<ERPResponseMessageDto<ERPARPaymentSessionDto>> Process_PutARPaymentSession(ERPARPaymentSessionDto aRPaymentSession);

	/// <summary>
	/// Validates the request for deleting a ARPaymentSession record.
	/// </summary>
	/// <param name="aRPaymentSessionId">The Unique Id of the ARPaymentSession.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteARPaymentSession(Guid aRPaymentSessionId);

	/// <summary>
	/// Processes the request to delete a ARPaymentSession record.
	/// </summary>
	/// <param name="aRPaymentSessionId">The Unique Id of the ARPaymentSession.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPARPaymentSessionDto>> Process_DeleteARPaymentSession(Guid aRPaymentSessionId);
}
