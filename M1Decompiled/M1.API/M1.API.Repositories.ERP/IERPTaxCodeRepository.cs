using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPTaxCodeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a TaxCode with the specified Unique Id exists.
	/// </summary>
	/// <param name="taxCodeId">The Unique Id of the TaxCode to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the TaxCode exists or not.</returns>
	Task<bool> DoesTaxCodeExist(Guid taxCodeId);

	/// <summary>
	/// Retrieves all TaxCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of TaxCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of TaxCodes DTOs.</returns>
	Task<ICollection<ERPTaxCodeInformationDto>> GetAllTaxCodes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific TaxCode.
	/// </summary>
	/// <param name="taxCodeId">The Unique Id of the TaxCode to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the TaxCode DTO.</returns>
	Task<ERPTaxCodeInformationDto> GetTaxCode(Guid taxCodeId);

	/// <summary>
	/// Saves the provided ERP taxCode.
	/// </summary>
	/// <param name="taxCode">The ERP taxCode to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveTaxCode(ERPTaxCodeDto taxCode);
}
