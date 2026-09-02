using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPLeadRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Lead with the specified Unique Id exists.
	/// </summary>
	/// <param name="leadId">The Unique Id of the Lead to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Lead exists or not.</returns>
	Task<bool> DoesLeadExist(Guid leadId);

	/// <summary>
	/// Retrieves all Leads with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Leads to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Leads DTOs.</returns>
	Task<ICollection<ERPLeadInformationDto>> GetAllLeads(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Lead.
	/// </summary>
	/// <param name="leadId">The Unique Id of the Lead to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Lead DTO.</returns>
	Task<ERPLeadInformationDto> GetLead(Guid leadId);

	/// <summary>
	/// Saves the provided ERP lead.
	/// </summary>
	/// <param name="lead">The ERP lead to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveLead(ERPLeadDto lead);
}
