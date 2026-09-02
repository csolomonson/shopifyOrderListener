using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPCustomerGroupRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a CustomerGroup with the specified Unique Id exists.
	/// </summary>
	/// <param name="customerGroupId">The Unique Id of the CustomerGroup to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the CustomerGroup exists or not.</returns>
	Task<bool> DoesCustomerGroupExist(Guid customerGroupId);

	/// <summary>
	/// Retrieves all CustomerGroups with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CustomerGroups to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CustomerGroups DTOs.</returns>
	Task<ICollection<ERPCustomerGroupInformationDto>> GetAllCustomerGroups(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific CustomerGroup.
	/// </summary>
	/// <param name="customerGroupId">The Unique Id of the CustomerGroup to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the CustomerGroup DTO.</returns>
	Task<ERPCustomerGroupInformationDto> GetCustomerGroup(Guid customerGroupId);

	/// <summary>
	/// Saves the provided ERP customerGroup.
	/// </summary>
	/// <param name="customerGroup">The ERP customerGroup to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveCustomerGroup(ERPCustomerGroupDto customerGroup);
}
