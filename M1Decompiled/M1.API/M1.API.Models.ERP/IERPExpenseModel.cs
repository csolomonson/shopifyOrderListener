using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPExpenseModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Expenses with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Expenses to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllExpenses(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Expense information based on the specified Expense Unique Id.
	/// </summary>
	/// <param name="expenseId">The Unique Id of the Expense.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetExpense(Guid expenseId);

	/// <summary>
	/// Validates the PUT request for creating or updating Expense information based on the specified Expense.
	/// </summary>
	/// <param name="expense">The Expense details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutExpense(ERPExpenseDto expense);

	/// <summary>
	/// Processes the request to retrieve all Expenses with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Expenses to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Expenses DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPExpenseDto>>> Process_GetAllExpenses(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Expense.
	/// </summary>
	/// <param name="expenseId">The Unique Id of the Expense to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Expense DTO.</returns>
	Task<ERPResponseMessageDto<ERPExpenseDto>> Process_GetExpense(Guid expenseId);

	/// <summary>
	/// Processes the creating or updating of a Expense record.
	/// </summary>
	/// <param name="expense">The Expense data transfer object (DTO) containing the details of the Expense to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Expense details.</returns>
	Task<ERPResponseMessageDto<ERPExpenseDto>> Process_PutExpense(ERPExpenseDto expense);

	/// <summary>
	/// Validates the request for deleting a Expense record.
	/// </summary>
	/// <param name="expenseId">The Unique Id of the Expense.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteExpense(Guid expenseId);

	/// <summary>
	/// Processes the request to delete a Expense record.
	/// </summary>
	/// <param name="expenseId">The Unique Id of the Expense.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPExpenseDto>> Process_DeleteExpense(Guid expenseId);
}
