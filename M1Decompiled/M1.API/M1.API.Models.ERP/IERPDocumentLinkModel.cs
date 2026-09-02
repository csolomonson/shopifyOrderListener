using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPDocumentLinkModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all DocumentLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DocumentLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllDocumentLinks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving DocumentLink information based on the specified DocumentLink Unique Id.
	/// </summary>
	/// <param name="documentLinkId">The Unique Id of the DocumentLink.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetDocumentLink(Guid documentLinkId);

	/// <summary>
	/// Validates the PUT request for creating or updating DocumentLink information based on the specified DocumentLink.
	/// </summary>
	/// <param name="documentLink">The DocumentLink details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutDocumentLink(ERPDocumentLinkDto documentLink);

	/// <summary>
	/// Processes the request to retrieve all DocumentLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DocumentLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of DocumentLinks DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPDocumentLinkDto>>> Process_GetAllDocumentLinks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific DocumentLink.
	/// </summary>
	/// <param name="documentLinkId">The Unique Id of the DocumentLink to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the DocumentLink DTO.</returns>
	Task<ERPResponseMessageDto<ERPDocumentLinkDto>> Process_GetDocumentLink(Guid documentLinkId);

	/// <summary>
	/// Processes the creating or updating of a DocumentLink record.
	/// </summary>
	/// <param name="documentLink">The DocumentLink data transfer object (DTO) containing the details of the DocumentLink to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the DocumentLink details.</returns>
	Task<ERPResponseMessageDto<ERPDocumentLinkDto>> Process_PutDocumentLink(ERPDocumentLinkDto documentLink);

	/// <summary>
	/// Validates the request for deleting a DocumentLink record.
	/// </summary>
	/// <param name="documentLinkId">The Unique Id of the DocumentLink.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteDocumentLink(Guid documentLinkId);

	/// <summary>
	/// Processes the request to delete a DocumentLink record.
	/// </summary>
	/// <param name="documentLinkId">The Unique Id of the DocumentLink.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPDocumentLinkDto>> Process_DeleteDocumentLink(Guid documentLinkId);
}
