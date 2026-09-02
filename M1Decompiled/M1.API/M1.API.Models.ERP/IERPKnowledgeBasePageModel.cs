using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPKnowledgeBasePageModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all KnowledgeBasePages with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of KnowledgeBasePages to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllKnowledgeBasePages(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving KnowledgeBasePage information based on the specified KnowledgeBasePage Unique Id.
	/// </summary>
	/// <param name="knowledgeBasePageId">The Unique Id of the KnowledgeBasePage.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetKnowledgeBasePage(Guid knowledgeBasePageId);

	/// <summary>
	/// Validates the PUT request for creating or updating KnowledgeBasePage information based on the specified KnowledgeBasePage.
	/// </summary>
	/// <param name="knowledgeBasePage">The KnowledgeBasePage details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutKnowledgeBasePage(ERPKnowledgeBasePageDto knowledgeBasePage);

	/// <summary>
	/// Processes the request to retrieve all KnowledgeBasePages with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of KnowledgeBasePages to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of KnowledgeBasePages DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPKnowledgeBasePageDto>>> Process_GetAllKnowledgeBasePages(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific KnowledgeBasePage.
	/// </summary>
	/// <param name="knowledgeBasePageId">The Unique Id of the KnowledgeBasePage to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the KnowledgeBasePage DTO.</returns>
	Task<ERPResponseMessageDto<ERPKnowledgeBasePageDto>> Process_GetKnowledgeBasePage(Guid knowledgeBasePageId);

	/// <summary>
	/// Processes the creating or updating of a KnowledgeBasePage record.
	/// </summary>
	/// <param name="knowledgeBasePage">The KnowledgeBasePage data transfer object (DTO) containing the details of the KnowledgeBasePage to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the KnowledgeBasePage details.</returns>
	Task<ERPResponseMessageDto<ERPKnowledgeBasePageDto>> Process_PutKnowledgeBasePage(ERPKnowledgeBasePageDto knowledgeBasePage);

	/// <summary>
	/// Validates the request for deleting a KnowledgeBasePage record.
	/// </summary>
	/// <param name="knowledgeBasePageId">The Unique Id of the KnowledgeBasePage.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteKnowledgeBasePage(Guid knowledgeBasePageId);

	/// <summary>
	/// Processes the request to delete a KnowledgeBasePage record.
	/// </summary>
	/// <param name="knowledgeBasePageId">The Unique Id of the KnowledgeBasePage.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPKnowledgeBasePageDto>> Process_DeleteKnowledgeBasePage(Guid knowledgeBasePageId);
}
