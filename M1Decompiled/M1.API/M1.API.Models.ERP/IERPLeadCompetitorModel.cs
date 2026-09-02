using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPLeadCompetitorModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all LeadCompetitors with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LeadCompetitors to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllLeadCompetitors(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving LeadCompetitor information based on the specified LeadCompetitor Unique Id.
	/// </summary>
	/// <param name="leadCompetitorId">The Unique Id of the LeadCompetitor.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetLeadCompetitor(Guid leadCompetitorId);

	/// <summary>
	/// Validates the PUT request for creating or updating LeadCompetitor information based on the specified LeadCompetitor.
	/// </summary>
	/// <param name="leadCompetitor">The LeadCompetitor details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutLeadCompetitor(ERPLeadCompetitorDto leadCompetitor);

	/// <summary>
	/// Processes the request to retrieve all LeadCompetitors with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LeadCompetitors to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LeadCompetitors DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPLeadCompetitorDto>>> Process_GetAllLeadCompetitors(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific LeadCompetitor.
	/// </summary>
	/// <param name="leadCompetitorId">The Unique Id of the LeadCompetitor to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the LeadCompetitor DTO.</returns>
	Task<ERPResponseMessageDto<ERPLeadCompetitorDto>> Process_GetLeadCompetitor(Guid leadCompetitorId);

	/// <summary>
	/// Processes the creating or updating of a LeadCompetitor record.
	/// </summary>
	/// <param name="leadCompetitor">The LeadCompetitor data transfer object (DTO) containing the details of the LeadCompetitor to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the LeadCompetitor details.</returns>
	Task<ERPResponseMessageDto<ERPLeadCompetitorDto>> Process_PutLeadCompetitor(ERPLeadCompetitorDto leadCompetitor);

	/// <summary>
	/// Validates the request for deleting a LeadCompetitor record.
	/// </summary>
	/// <param name="leadCompetitorId">The Unique Id of the LeadCompetitor.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteLeadCompetitor(Guid leadCompetitorId);

	/// <summary>
	/// Processes the request to delete a LeadCompetitor record.
	/// </summary>
	/// <param name="leadCompetitorId">The Unique Id of the LeadCompetitor.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPLeadCompetitorDto>> Process_DeleteLeadCompetitor(Guid leadCompetitorId);
}
