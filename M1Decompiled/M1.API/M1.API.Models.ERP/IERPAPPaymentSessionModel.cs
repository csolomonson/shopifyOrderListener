using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAPPaymentSessionModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all APPaymentSessions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APPaymentSessions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAPPaymentSessions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving APPaymentSession information based on the specified APPaymentSession Unique Id.
	/// </summary>
	/// <param name="aPPaymentSessionId">The Unique Id of the APPaymentSession.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAPPaymentSession(Guid aPPaymentSessionId);

	/// <summary>
	/// Validates the PUT request for creating or updating APPaymentSession information based on the specified APPaymentSession.
	/// </summary>
	/// <param name="aPPaymentSession">The APPaymentSession details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutAPPaymentSession(ERPAPPaymentSessionDto aPPaymentSession);

	/// <summary>
	/// Processes the request to retrieve all APPaymentSessions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APPaymentSessions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of APPaymentSessions DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAPPaymentSessionDto>>> Process_GetAllAPPaymentSessions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific APPaymentSession.
	/// </summary>
	/// <param name="aPPaymentSessionId">The Unique Id of the APPaymentSession to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the APPaymentSession DTO.</returns>
	Task<ERPResponseMessageDto<ERPAPPaymentSessionDto>> Process_GetAPPaymentSession(Guid aPPaymentSessionId);

	/// <summary>
	/// Processes the creating or updating of a APPaymentSession record.
	/// </summary>
	/// <param name="aPPaymentSession">The APPaymentSession data transfer object (DTO) containing the details of the APPaymentSession to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the APPaymentSession details.</returns>
	Task<ERPResponseMessageDto<ERPAPPaymentSessionDto>> Process_PutAPPaymentSession(ERPAPPaymentSessionDto aPPaymentSession);

	/// <summary>
	/// Validates the request for deleting a APPaymentSession record.
	/// </summary>
	/// <param name="aPPaymentSessionId">The Unique Id of the APPaymentSession.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteAPPaymentSession(Guid aPPaymentSessionId);

	/// <summary>
	/// Processes the request to delete a APPaymentSession record.
	/// </summary>
	/// <param name="aPPaymentSessionId">The Unique Id of the APPaymentSession.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPAPPaymentSessionDto>> Process_DeleteAPPaymentSession(Guid aPPaymentSessionId);
}
