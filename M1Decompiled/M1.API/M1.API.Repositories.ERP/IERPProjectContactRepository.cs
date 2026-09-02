using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPProjectContactRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ProjectContact with the specified Unique Id exists.
	/// </summary>
	/// <param name="projectContactId">The Unique Id of the ProjectContact to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ProjectContact exists or not.</returns>
	Task<bool> DoesProjectContactExist(Guid projectContactId);

	/// <summary>
	/// Retrieves all ProjectContacts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProjectContacts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProjectContacts DTOs.</returns>
	Task<ICollection<ERPProjectContactInformationDto>> GetAllProjectContacts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ProjectContact.
	/// </summary>
	/// <param name="projectContactId">The Unique Id of the ProjectContact to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ProjectContact DTO.</returns>
	Task<ERPProjectContactInformationDto> GetProjectContact(Guid projectContactId);

	/// <summary>
	/// Saves the provided ERP projectContact.
	/// </summary>
	/// <param name="projectContact">The ERP projectContact to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveProjectContact(ERPProjectContactDto projectContact);
}
