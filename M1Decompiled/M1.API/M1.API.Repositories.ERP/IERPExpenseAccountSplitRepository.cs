using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPExpenseAccountSplitRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ExpenseAccountSplit with the specified Unique Id exists.
	/// </summary>
	/// <param name="expenseAccountSplitId">The Unique Id of the ExpenseAccountSplit to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ExpenseAccountSplit exists or not.</returns>
	Task<bool> DoesExpenseAccountSplitExist(Guid expenseAccountSplitId);

	/// <summary>
	/// Retrieves all ExpenseAccountSplits with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ExpenseAccountSplits to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ExpenseAccountSplits DTOs.</returns>
	Task<ICollection<ERPExpenseAccountSplitInformationDto>> GetAllExpenseAccountSplits(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ExpenseAccountSplit.
	/// </summary>
	/// <param name="expenseAccountSplitId">The Unique Id of the ExpenseAccountSplit to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ExpenseAccountSplit DTO.</returns>
	Task<ERPExpenseAccountSplitInformationDto> GetExpenseAccountSplit(Guid expenseAccountSplitId);

	/// <summary>
	/// Saves the provided ERP expenseAccountSplit.
	/// </summary>
	/// <param name="expenseAccountSplit">The ERP expenseAccountSplit to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveExpenseAccountSplit(ERPExpenseAccountSplitDto expenseAccountSplit);
}
