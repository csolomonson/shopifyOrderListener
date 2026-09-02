using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPGLJournalLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all GLJournalLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLJournalLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllGLJournalLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving GLJournalLine information based on the specified GLJournalLine Unique Id.
	/// </summary>
	/// <param name="gLJournalLineId">The Unique Id of the GLJournalLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetGLJournalLine(Guid gLJournalLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating GLJournalLine information based on the specified GLJournalLine.
	/// </summary>
	/// <param name="gLJournalLine">The GLJournalLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutGLJournalLine(ERPGLJournalLineDto gLJournalLine);

	/// <summary>
	/// Processes the request to retrieve all GLJournalLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLJournalLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLJournalLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPGLJournalLineDto>>> Process_GetAllGLJournalLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific GLJournalLine.
	/// </summary>
	/// <param name="gLJournalLineId">The Unique Id of the GLJournalLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the GLJournalLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPGLJournalLineDto>> Process_GetGLJournalLine(Guid gLJournalLineId);

	/// <summary>
	/// Processes the creating or updating of a GLJournalLine record.
	/// </summary>
	/// <param name="gLJournalLine">The GLJournalLine data transfer object (DTO) containing the details of the GLJournalLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the GLJournalLine details.</returns>
	Task<ERPResponseMessageDto<ERPGLJournalLineDto>> Process_PutGLJournalLine(ERPGLJournalLineDto gLJournalLine);

	/// <summary>
	/// Validates the request for deleting a GLJournalLine record.
	/// </summary>
	/// <param name="gLJournalLineId">The Unique Id of the GLJournalLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteGLJournalLine(Guid gLJournalLineId);

	/// <summary>
	/// Processes the request to delete a GLJournalLine record.
	/// </summary>
	/// <param name="gLJournalLineId">The Unique Id of the GLJournalLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPGLJournalLineDto>> Process_DeleteGLJournalLine(Guid gLJournalLineId);
}
