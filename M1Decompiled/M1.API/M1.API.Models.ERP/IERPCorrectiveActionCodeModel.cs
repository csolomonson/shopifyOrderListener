using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPCorrectiveActionCodeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all CorrectiveActionCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CorrectiveActionCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllCorrectiveActionCodes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving CorrectiveActionCode information based on the specified CorrectiveActionCode Unique Id.
	/// </summary>
	/// <param name="correctiveActionCodeId">The Unique Id of the CorrectiveActionCode.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetCorrectiveActionCode(Guid correctiveActionCodeId);

	/// <summary>
	/// Validates the PUT request for creating or updating CorrectiveActionCode information based on the specified CorrectiveActionCode.
	/// </summary>
	/// <param name="correctiveActionCode">The CorrectiveActionCode details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutCorrectiveActionCode(ERPCorrectiveActionCodeDto correctiveActionCode);

	/// <summary>
	/// Processes the request to retrieve all CorrectiveActionCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CorrectiveActionCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CorrectiveActionCodes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPCorrectiveActionCodeDto>>> Process_GetAllCorrectiveActionCodes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific CorrectiveActionCode.
	/// </summary>
	/// <param name="correctiveActionCodeId">The Unique Id of the CorrectiveActionCode to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the CorrectiveActionCode DTO.</returns>
	Task<ERPResponseMessageDto<ERPCorrectiveActionCodeDto>> Process_GetCorrectiveActionCode(Guid correctiveActionCodeId);

	/// <summary>
	/// Processes the creating or updating of a CorrectiveActionCode record.
	/// </summary>
	/// <param name="correctiveActionCode">The CorrectiveActionCode data transfer object (DTO) containing the details of the CorrectiveActionCode to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the CorrectiveActionCode details.</returns>
	Task<ERPResponseMessageDto<ERPCorrectiveActionCodeDto>> Process_PutCorrectiveActionCode(ERPCorrectiveActionCodeDto correctiveActionCode);

	/// <summary>
	/// Validates the request for deleting a CorrectiveActionCode record.
	/// </summary>
	/// <param name="correctiveActionCodeId">The Unique Id of the CorrectiveActionCode.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteCorrectiveActionCode(Guid correctiveActionCodeId);

	/// <summary>
	/// Processes the request to delete a CorrectiveActionCode record.
	/// </summary>
	/// <param name="correctiveActionCodeId">The Unique Id of the CorrectiveActionCode.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPCorrectiveActionCodeDto>> Process_DeleteCorrectiveActionCode(Guid correctiveActionCodeId);
}
