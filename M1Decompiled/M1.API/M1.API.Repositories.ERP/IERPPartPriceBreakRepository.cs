using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartPriceBreakRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartPriceBreak with the specified Unique Id exists.
	/// </summary>
	/// <param name="partPriceBreakId">The Unique Id of the PartPriceBreak to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartPriceBreak exists or not.</returns>
	Task<bool> DoesPartPriceBreakExist(Guid partPriceBreakId);

	/// <summary>
	/// Retrieves all PartPriceBreaks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartPriceBreaks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartPriceBreaks DTOs.</returns>
	Task<ICollection<ERPPartPriceBreakInformationDto>> GetAllPartPriceBreaks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartPriceBreak.
	/// </summary>
	/// <param name="partPriceBreakId">The Unique Id of the PartPriceBreak to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartPriceBreak DTO.</returns>
	Task<ERPPartPriceBreakInformationDto> GetPartPriceBreak(Guid partPriceBreakId);

	/// <summary>
	/// Saves the provided ERP partPriceBreak.
	/// </summary>
	/// <param name="partPriceBreak">The ERP partPriceBreak to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartPriceBreak(ERPPartPriceBreakDto partPriceBreak);
}
