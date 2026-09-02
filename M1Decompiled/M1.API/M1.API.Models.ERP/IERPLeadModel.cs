using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPLeadModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Leads with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Leads to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllLeads(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Lead information based on the specified Lead Unique Id.
	/// </summary>
	/// <param name="leadId">The Unique Id of the Lead.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetLead(Guid leadId);

	/// <summary>
	/// Validates the PUT request for creating or updating Lead information based on the specified Lead.
	/// </summary>
	/// <param name="lead">The Lead details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutLead(ERPLeadDto lead);

	/// <summary>
	/// Processes the request to retrieve all Leads with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Leads to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Leads DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPLeadDto>>> Process_GetAllLeads(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Lead.
	/// </summary>
	/// <param name="leadId">The Unique Id of the Lead to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Lead DTO.</returns>
	Task<ERPResponseMessageDto<ERPLeadDto>> Process_GetLead(Guid leadId);

	/// <summary>
	/// Processes the creating or updating of a Lead record.
	/// </summary>
	/// <param name="lead">The Lead data transfer object (DTO) containing the details of the Lead to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Lead details.</returns>
	Task<ERPResponseMessageDto<ERPLeadDto>> Process_PutLead(ERPLeadDto lead);

	/// <summary>
	/// Validates the request for deleting a Lead record.
	/// </summary>
	/// <param name="leadId">The Unique Id of the Lead.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteLead(Guid leadId);

	/// <summary>
	/// Processes the request to delete a Lead record.
	/// </summary>
	/// <param name="leadId">The Unique Id of the Lead.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPLeadDto>> Process_DeleteLead(Guid leadId);
}
