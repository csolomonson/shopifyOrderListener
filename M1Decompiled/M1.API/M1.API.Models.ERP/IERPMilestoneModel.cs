using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPMilestoneModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Milestones with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Milestones to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllMilestones(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Milestone information based on the specified Milestone Unique Id.
	/// </summary>
	/// <param name="milestoneId">The Unique Id of the Milestone.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetMilestone(Guid milestoneId);

	/// <summary>
	/// Validates the PUT request for creating or updating Milestone information based on the specified Milestone.
	/// </summary>
	/// <param name="milestone">The Milestone details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutMilestone(ERPMilestoneDto milestone);

	/// <summary>
	/// Processes the request to retrieve all Milestones with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Milestones to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Milestones DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPMilestoneDto>>> Process_GetAllMilestones(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Milestone.
	/// </summary>
	/// <param name="milestoneId">The Unique Id of the Milestone to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Milestone DTO.</returns>
	Task<ERPResponseMessageDto<ERPMilestoneDto>> Process_GetMilestone(Guid milestoneId);

	/// <summary>
	/// Processes the creating or updating of a Milestone record.
	/// </summary>
	/// <param name="milestone">The Milestone data transfer object (DTO) containing the details of the Milestone to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Milestone details.</returns>
	Task<ERPResponseMessageDto<ERPMilestoneDto>> Process_PutMilestone(ERPMilestoneDto milestone);

	/// <summary>
	/// Validates the request for deleting a Milestone record.
	/// </summary>
	/// <param name="milestoneId">The Unique Id of the Milestone.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteMilestone(Guid milestoneId);

	/// <summary>
	/// Processes the request to delete a Milestone record.
	/// </summary>
	/// <param name="milestoneId">The Unique Id of the Milestone.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPMilestoneDto>> Process_DeleteMilestone(Guid milestoneId);
}
