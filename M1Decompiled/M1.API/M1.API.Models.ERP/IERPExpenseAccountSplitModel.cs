using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPExpenseAccountSplitModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ExpenseAccountSplits with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ExpenseAccountSplits to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllExpenseAccountSplits(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ExpenseAccountSplit information based on the specified ExpenseAccountSplit Unique Id.
	/// </summary>
	/// <param name="expenseAccountSplitId">The Unique Id of the ExpenseAccountSplit.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetExpenseAccountSplit(Guid expenseAccountSplitId);

	/// <summary>
	/// Validates the PUT request for creating or updating ExpenseAccountSplit information based on the specified ExpenseAccountSplit.
	/// </summary>
	/// <param name="expenseAccountSplit">The ExpenseAccountSplit details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutExpenseAccountSplit(ERPExpenseAccountSplitDto expenseAccountSplit);

	/// <summary>
	/// Processes the request to retrieve all ExpenseAccountSplits with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ExpenseAccountSplits to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ExpenseAccountSplits DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPExpenseAccountSplitDto>>> Process_GetAllExpenseAccountSplits(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ExpenseAccountSplit.
	/// </summary>
	/// <param name="expenseAccountSplitId">The Unique Id of the ExpenseAccountSplit to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ExpenseAccountSplit DTO.</returns>
	Task<ERPResponseMessageDto<ERPExpenseAccountSplitDto>> Process_GetExpenseAccountSplit(Guid expenseAccountSplitId);

	/// <summary>
	/// Processes the creating or updating of a ExpenseAccountSplit record.
	/// </summary>
	/// <param name="expenseAccountSplit">The ExpenseAccountSplit data transfer object (DTO) containing the details of the ExpenseAccountSplit to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ExpenseAccountSplit details.</returns>
	Task<ERPResponseMessageDto<ERPExpenseAccountSplitDto>> Process_PutExpenseAccountSplit(ERPExpenseAccountSplitDto expenseAccountSplit);

	/// <summary>
	/// Validates the request for deleting a ExpenseAccountSplit record.
	/// </summary>
	/// <param name="expenseAccountSplitId">The Unique Id of the ExpenseAccountSplit.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteExpenseAccountSplit(Guid expenseAccountSplitId);

	/// <summary>
	/// Processes the request to delete a ExpenseAccountSplit record.
	/// </summary>
	/// <param name="expenseAccountSplitId">The Unique Id of the ExpenseAccountSplit.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPExpenseAccountSplitDto>> Process_DeleteExpenseAccountSplit(Guid expenseAccountSplitId);
}
