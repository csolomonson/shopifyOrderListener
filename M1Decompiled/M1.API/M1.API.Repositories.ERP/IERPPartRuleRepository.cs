using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartRuleRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartRule with the specified Unique Id exists.
	/// </summary>
	/// <param name="partRuleId">The Unique Id of the PartRule to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartRule exists or not.</returns>
	Task<bool> DoesPartRuleExist(Guid partRuleId);

	/// <summary>
	/// Retrieves all PartRules with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartRules to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartRules DTOs.</returns>
	Task<ICollection<ERPPartRuleInformationDto>> GetAllPartRules(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartRule.
	/// </summary>
	/// <param name="partRuleId">The Unique Id of the PartRule to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartRule DTO.</returns>
	Task<ERPPartRuleInformationDto> GetPartRule(Guid partRuleId);

	/// <summary>
	/// Saves the provided ERP partRule.
	/// </summary>
	/// <param name="partRule">The ERP partRule to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartRule(ERPPartRuleDto partRule);
}
