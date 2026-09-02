using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPExpenseRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Expense with the specified Unique Id exists.
	/// </summary>
	/// <param name="expenseId">The Unique Id of the Expense to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Expense exists or not.</returns>
	Task<bool> DoesExpenseExist(Guid expenseId);

	/// <summary>
	/// Retrieves all Expenses with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Expenses to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Expenses DTOs.</returns>
	Task<ICollection<ERPExpenseInformationDto>> GetAllExpenses(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Expense.
	/// </summary>
	/// <param name="expenseId">The Unique Id of the Expense to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Expense DTO.</returns>
	Task<ERPExpenseInformationDto> GetExpense(Guid expenseId);

	/// <summary>
	/// Saves the provided ERP expense.
	/// </summary>
	/// <param name="expense">The ERP expense to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveExpense(ERPExpenseDto expense);
}
