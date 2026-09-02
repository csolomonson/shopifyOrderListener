using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPLeadLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a LeadLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="leadLineId">The Unique Id of the LeadLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the LeadLine exists or not.</returns>
	Task<bool> DoesLeadLineExist(Guid leadLineId);

	/// <summary>
	/// Retrieves all LeadLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LeadLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LeadLines DTOs.</returns>
	Task<ICollection<ERPLeadLineInformationDto>> GetAllLeadLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific LeadLine.
	/// </summary>
	/// <param name="leadLineId">The Unique Id of the LeadLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the LeadLine DTO.</returns>
	Task<ERPLeadLineInformationDto> GetLeadLine(Guid leadLineId);

	/// <summary>
	/// Saves the provided ERP leadLine.
	/// </summary>
	/// <param name="leadLine">The ERP leadLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveLeadLine(ERPLeadLineDto leadLine);
}
