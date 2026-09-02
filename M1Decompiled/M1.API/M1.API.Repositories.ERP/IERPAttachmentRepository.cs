using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAttachmentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Attachment with the specified Unique Id exists.
	/// </summary>
	/// <param name="attachmentId">The Unique Id of the Attachment to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Attachment exists or not.</returns>
	Task<bool> DoesAttachmentExist(Guid attachmentId);

	/// <summary>
	/// Retrieves all Attachments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Attachments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Attachments DTOs.</returns>
	Task<ICollection<ERPAttachmentInformationDto>> GetAllAttachments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Attachment.
	/// </summary>
	/// <param name="attachmentId">The Unique Id of the Attachment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Attachment DTO.</returns>
	Task<ERPAttachmentInformationDto> GetAttachment(Guid attachmentId);

	/// <summary>
	/// Saves the provided ERP attachment.
	/// </summary>
	/// <param name="attachment">The ERP attachment to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveAttachment(ERPAttachmentDto attachment);
}
