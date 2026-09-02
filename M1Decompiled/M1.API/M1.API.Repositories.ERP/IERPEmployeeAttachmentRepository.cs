using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPEmployeeAttachmentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a EmployeeAttachment with the specified Unique Id exists.
	/// </summary>
	/// <param name="employeeAttachmentId">The Unique Id of the EmployeeAttachment to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the EmployeeAttachment exists or not.</returns>
	Task<bool> DoesEmployeeAttachmentExist(Guid employeeAttachmentId);

	/// <summary>
	/// Retrieves all EmployeeAttachments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeAttachments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeeAttachments DTOs.</returns>
	Task<ICollection<ERPEmployeeAttachmentInformationDto>> GetAllEmployeeAttachments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific EmployeeAttachment.
	/// </summary>
	/// <param name="employeeAttachmentId">The Unique Id of the EmployeeAttachment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the EmployeeAttachment DTO.</returns>
	Task<ERPEmployeeAttachmentInformationDto> GetEmployeeAttachment(Guid employeeAttachmentId);

	/// <summary>
	/// Saves the provided ERP employeeAttachment.
	/// </summary>
	/// <param name="employeeAttachment">The ERP employeeAttachment to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveEmployeeAttachment(ERPEmployeeAttachmentDto employeeAttachment);
}
