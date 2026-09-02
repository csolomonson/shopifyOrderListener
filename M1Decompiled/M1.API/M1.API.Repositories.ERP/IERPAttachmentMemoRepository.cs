using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAttachmentMemoRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a AttachmentMemo with the specified Unique Id exists.
	/// </summary>
	/// <param name="attachmentMemoId">The Unique Id of the AttachmentMemo to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the AttachmentMemo exists or not.</returns>
	Task<bool> DoesAttachmentMemoExist(Guid attachmentMemoId);

	/// <summary>
	/// Retrieves all AttachmentMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AttachmentMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AttachmentMemos DTOs.</returns>
	Task<ICollection<ERPAttachmentMemoInformationDto>> GetAllAttachmentMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific AttachmentMemo.
	/// </summary>
	/// <param name="attachmentMemoId">The Unique Id of the AttachmentMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the AttachmentMemo DTO.</returns>
	Task<ERPAttachmentMemoInformationDto> GetAttachmentMemo(Guid attachmentMemoId);

	/// <summary>
	/// Saves the provided ERP attachmentMemo.
	/// </summary>
	/// <param name="attachmentMemo">The ERP attachmentMemo to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveAttachmentMemo(ERPAttachmentMemoDto attachmentMemo);
}
