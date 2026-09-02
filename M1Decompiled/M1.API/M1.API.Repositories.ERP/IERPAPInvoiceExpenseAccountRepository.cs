using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAPInvoiceExpenseAccountRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a APInvoiceExpenseAccount with the specified Unique Id exists.
	/// </summary>
	/// <param name="aPInvoiceExpenseAccountId">The Unique Id of the APInvoiceExpenseAccount to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the APInvoiceExpenseAccount exists or not.</returns>
	Task<bool> DoesAPInvoiceExpenseAccountExist(Guid aPInvoiceExpenseAccountId);

	/// <summary>
	/// Retrieves all APInvoiceExpenseAccounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APInvoiceExpenseAccounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of APInvoiceExpenseAccounts DTOs.</returns>
	Task<ICollection<ERPAPInvoiceExpenseAccountInformationDto>> GetAllAPInvoiceExpenseAccounts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific APInvoiceExpenseAccount.
	/// </summary>
	/// <param name="aPInvoiceExpenseAccountId">The Unique Id of the APInvoiceExpenseAccount to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the APInvoiceExpenseAccount DTO.</returns>
	Task<ERPAPInvoiceExpenseAccountInformationDto> GetAPInvoiceExpenseAccount(Guid aPInvoiceExpenseAccountId);

	/// <summary>
	/// Saves the provided ERP aPInvoiceExpenseAccount.
	/// </summary>
	/// <param name="aPInvoiceExpenseAccount">The ERP aPInvoiceExpenseAccount to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveAPInvoiceExpenseAccount(ERPAPInvoiceExpenseAccountDto aPInvoiceExpenseAccount);
}
