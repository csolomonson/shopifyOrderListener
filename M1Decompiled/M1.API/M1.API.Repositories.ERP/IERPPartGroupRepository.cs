using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartGroupRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartGroup with the specified Unique Id exists.
	/// </summary>
	/// <param name="partGroupId">The Unique Id of the PartGroup to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartGroup exists or not.</returns>
	Task<bool> DoesPartGroupExist(Guid partGroupId);

	/// <summary>
	/// Retrieves all PartGroups with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartGroups to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartGroups DTOs.</returns>
	Task<ICollection<ERPPartGroupInformationDto>> GetAllPartGroups(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartGroup.
	/// </summary>
	/// <param name="partGroupId">The Unique Id of the PartGroup to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartGroup DTO.</returns>
	Task<ERPPartGroupInformationDto> GetPartGroup(Guid partGroupId);

	/// <summary>
	/// Saves the provided ERP partGroup.
	/// </summary>
	/// <param name="partGroup">The ERP partGroup to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartGroup(ERPPartGroupDto partGroup);
}
