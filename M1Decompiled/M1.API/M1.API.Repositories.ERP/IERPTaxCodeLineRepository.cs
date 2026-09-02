using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPTaxCodeLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a TaxCodeLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="taxCodeLineId">The Unique Id of the TaxCodeLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the TaxCodeLine exists or not.</returns>
	Task<bool> DoesTaxCodeLineExist(Guid taxCodeLineId);

	/// <summary>
	/// Retrieves all TaxCodeLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of TaxCodeLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of TaxCodeLines DTOs.</returns>
	Task<ICollection<ERPTaxCodeLineInformationDto>> GetAllTaxCodeLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific TaxCodeLine.
	/// </summary>
	/// <param name="taxCodeLineId">The Unique Id of the TaxCodeLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the TaxCodeLine DTO.</returns>
	Task<ERPTaxCodeLineInformationDto> GetTaxCodeLine(Guid taxCodeLineId);

	/// <summary>
	/// Saves the provided ERP taxCodeLine.
	/// </summary>
	/// <param name="taxCodeLine">The ERP taxCodeLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveTaxCodeLine(ERPTaxCodeLineDto taxCodeLine);
}
