using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPLeadLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all LeadLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LeadLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllLeadLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving LeadLine information based on the specified LeadLine Unique Id.
	/// </summary>
	/// <param name="leadLineId">The Unique Id of the LeadLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetLeadLine(Guid leadLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating LeadLine information based on the specified LeadLine.
	/// </summary>
	/// <param name="leadLine">The LeadLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutLeadLine(ERPLeadLineDto leadLine);

	/// <summary>
	/// Processes the request to retrieve all LeadLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LeadLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LeadLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPLeadLineDto>>> Process_GetAllLeadLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific LeadLine.
	/// </summary>
	/// <param name="leadLineId">The Unique Id of the LeadLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the LeadLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPLeadLineDto>> Process_GetLeadLine(Guid leadLineId);

	/// <summary>
	/// Processes the creating or updating of a LeadLine record.
	/// </summary>
	/// <param name="leadLine">The LeadLine data transfer object (DTO) containing the details of the LeadLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the LeadLine details.</returns>
	Task<ERPResponseMessageDto<ERPLeadLineDto>> Process_PutLeadLine(ERPLeadLineDto leadLine);

	/// <summary>
	/// Validates the request for deleting a LeadLine record.
	/// </summary>
	/// <param name="leadLineId">The Unique Id of the LeadLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteLeadLine(Guid leadLineId);

	/// <summary>
	/// Processes the request to delete a LeadLine record.
	/// </summary>
	/// <param name="leadLineId">The Unique Id of the LeadLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPLeadLineDto>> Process_DeleteLeadLine(Guid leadLineId);
}
