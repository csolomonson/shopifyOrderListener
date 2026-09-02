using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAttachmentTypeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a AttachmentType with the specified Unique Id exists.
	/// </summary>
	/// <param name="attachmentTypeId">The Unique Id of the AttachmentType to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the AttachmentType exists or not.</returns>
	Task<bool> DoesAttachmentTypeExist(Guid attachmentTypeId);

	/// <summary>
	/// Retrieves all AttachmentTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AttachmentTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AttachmentTypes DTOs.</returns>
	Task<ICollection<ERPAttachmentTypeInformationDto>> GetAllAttachmentTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific AttachmentType.
	/// </summary>
	/// <param name="attachmentTypeId">The Unique Id of the AttachmentType to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the AttachmentType DTO.</returns>
	Task<ERPAttachmentTypeInformationDto> GetAttachmentType(Guid attachmentTypeId);

	/// <summary>
	/// Saves the provided ERP attachmentType.
	/// </summary>
	/// <param name="attachmentType">The ERP attachmentType to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveAttachmentType(ERPAttachmentTypeDto attachmentType);
}
