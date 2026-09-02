using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPBankEntryRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a BankEntry with the specified Unique Id exists.
	/// </summary>
	/// <param name="bankEntryId">The Unique Id of the BankEntry to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the BankEntry exists or not.</returns>
	Task<bool> DoesBankEntryExist(Guid bankEntryId);

	/// <summary>
	/// Retrieves all BankEntries with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of BankEntries to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of BankEntries DTOs.</returns>
	Task<ICollection<ERPBankEntryInformationDto>> GetAllBankEntries(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific BankEntry.
	/// </summary>
	/// <param name="bankEntryId">The Unique Id of the BankEntry to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the BankEntry DTO.</returns>
	Task<ERPBankEntryInformationDto> GetBankEntry(Guid bankEntryId);

	/// <summary>
	/// Saves the provided ERP bankEntry.
	/// </summary>
	/// <param name="bankEntry">The ERP bankEntry to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveBankEntry(ERPBankEntryDto bankEntry);
}
