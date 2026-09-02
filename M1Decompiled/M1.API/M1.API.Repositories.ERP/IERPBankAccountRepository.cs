using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPBankAccountRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a BankAccount with the specified Unique Id exists.
	/// </summary>
	/// <param name="bankAccountId">The Unique Id of the BankAccount to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the BankAccount exists or not.</returns>
	Task<bool> DoesBankAccountExist(Guid bankAccountId);

	/// <summary>
	/// Retrieves all BankAccounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of BankAccounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of BankAccounts DTOs.</returns>
	Task<ICollection<ERPBankAccountInformationDto>> GetAllBankAccounts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific BankAccount.
	/// </summary>
	/// <param name="bankAccountId">The Unique Id of the BankAccount to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the BankAccount DTO.</returns>
	Task<ERPBankAccountInformationDto> GetBankAccount(Guid bankAccountId);

	/// <summary>
	/// Saves the provided ERP bankAccount.
	/// </summary>
	/// <param name="bankAccount">The ERP bankAccount to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveBankAccount(ERPBankAccountDto bankAccount);
}
