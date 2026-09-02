using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPContactGroupRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ContactGroup with the specified Unique Id exists.
	/// </summary>
	/// <param name="contactGroupId">The Unique Id of the ContactGroup to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ContactGroup exists or not.</returns>
	Task<bool> DoesContactGroupExist(Guid contactGroupId);

	/// <summary>
	/// Retrieves all ContactGroups with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ContactGroups to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ContactGroups DTOs.</returns>
	Task<ICollection<ERPContactGroupInformationDto>> GetAllContactGroups(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ContactGroup.
	/// </summary>
	/// <param name="contactGroupId">The Unique Id of the ContactGroup to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ContactGroup DTO.</returns>
	Task<ERPContactGroupInformationDto> GetContactGroup(Guid contactGroupId);

	/// <summary>
	/// Saves the provided ERP contactGroup.
	/// </summary>
	/// <param name="contactGroup">The ERP contactGroup to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveContactGroup(ERPContactGroupDto contactGroup);
}
