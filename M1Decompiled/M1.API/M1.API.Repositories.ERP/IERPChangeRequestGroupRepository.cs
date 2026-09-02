using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPChangeRequestGroupRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ChangeRequestGroup with the specified Unique Id exists.
	/// </summary>
	/// <param name="changeRequestGroupId">The Unique Id of the ChangeRequestGroup to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ChangeRequestGroup exists or not.</returns>
	Task<bool> DoesChangeRequestGroupExist(Guid changeRequestGroupId);

	/// <summary>
	/// Retrieves all ChangeRequestGroups with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ChangeRequestGroups to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ChangeRequestGroups DTOs.</returns>
	Task<ICollection<ERPChangeRequestGroupInformationDto>> GetAllChangeRequestGroups(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ChangeRequestGroup.
	/// </summary>
	/// <param name="changeRequestGroupId">The Unique Id of the ChangeRequestGroup to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ChangeRequestGroup DTO.</returns>
	Task<ERPChangeRequestGroupInformationDto> GetChangeRequestGroup(Guid changeRequestGroupId);
}
