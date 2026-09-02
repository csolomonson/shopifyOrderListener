using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPFollowupModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Followups with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Followups to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllFollowups(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Followup information based on the specified Followup Unique Id.
	/// </summary>
	/// <param name="followupId">The Unique Id of the Followup.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetFollowup(Guid followupId);

	/// <summary>
	/// Validates the PUT request for creating or updating Followup information based on the specified Followup.
	/// </summary>
	/// <param name="followup">The Followup details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutFollowup(ERPFollowupDto followup);

	/// <summary>
	/// Processes the request to retrieve all Followups with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Followups to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Followups DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPFollowupDto>>> Process_GetAllFollowups(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Followup.
	/// </summary>
	/// <param name="followupId">The Unique Id of the Followup to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Followup DTO.</returns>
	Task<ERPResponseMessageDto<ERPFollowupDto>> Process_GetFollowup(Guid followupId);

	/// <summary>
	/// Processes the creating or updating of a Followup record.
	/// </summary>
	/// <param name="followup">The Followup data transfer object (DTO) containing the details of the Followup to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Followup details.</returns>
	Task<ERPResponseMessageDto<ERPFollowupDto>> Process_PutFollowup(ERPFollowupDto followup);

	/// <summary>
	/// Validates the request for deleting a Followup record.
	/// </summary>
	/// <param name="followupId">The Unique Id of the Followup.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteFollowup(Guid followupId);

	/// <summary>
	/// Processes the request to delete a Followup record.
	/// </summary>
	/// <param name="followupId">The Unique Id of the Followup.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPFollowupDto>> Process_DeleteFollowup(Guid followupId);
}
