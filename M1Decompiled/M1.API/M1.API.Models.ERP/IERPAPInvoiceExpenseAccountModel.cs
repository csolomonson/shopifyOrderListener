using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAPInvoiceExpenseAccountModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all APInvoiceExpenseAccounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APInvoiceExpenseAccounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAPInvoiceExpenseAccounts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving APInvoiceExpenseAccount information based on the specified APInvoiceExpenseAccount Unique Id.
	/// </summary>
	/// <param name="aPInvoiceExpenseAccountId">The Unique Id of the APInvoiceExpenseAccount.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAPInvoiceExpenseAccount(Guid aPInvoiceExpenseAccountId);

	/// <summary>
	/// Validates the PUT request for creating or updating APInvoiceExpenseAccount information based on the specified APInvoiceExpenseAccount.
	/// </summary>
	/// <param name="aPInvoiceExpenseAccount">The APInvoiceExpenseAccount details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutAPInvoiceExpenseAccount(ERPAPInvoiceExpenseAccountDto aPInvoiceExpenseAccount);

	/// <summary>
	/// Processes the request to retrieve all APInvoiceExpenseAccounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APInvoiceExpenseAccounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of APInvoiceExpenseAccounts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAPInvoiceExpenseAccountDto>>> Process_GetAllAPInvoiceExpenseAccounts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific APInvoiceExpenseAccount.
	/// </summary>
	/// <param name="aPInvoiceExpenseAccountId">The Unique Id of the APInvoiceExpenseAccount to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the APInvoiceExpenseAccount DTO.</returns>
	Task<ERPResponseMessageDto<ERPAPInvoiceExpenseAccountDto>> Process_GetAPInvoiceExpenseAccount(Guid aPInvoiceExpenseAccountId);

	/// <summary>
	/// Processes the creating or updating of a APInvoiceExpenseAccount record.
	/// </summary>
	/// <param name="aPInvoiceExpenseAccount">The APInvoiceExpenseAccount data transfer object (DTO) containing the details of the APInvoiceExpenseAccount to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the APInvoiceExpenseAccount details.</returns>
	Task<ERPResponseMessageDto<ERPAPInvoiceExpenseAccountDto>> Process_PutAPInvoiceExpenseAccount(ERPAPInvoiceExpenseAccountDto aPInvoiceExpenseAccount);

	/// <summary>
	/// Validates the request for deleting a APInvoiceExpenseAccount record.
	/// </summary>
	/// <param name="aPInvoiceExpenseAccountId">The Unique Id of the APInvoiceExpenseAccount.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteAPInvoiceExpenseAccount(Guid aPInvoiceExpenseAccountId);

	/// <summary>
	/// Processes the request to delete a APInvoiceExpenseAccount record.
	/// </summary>
	/// <param name="aPInvoiceExpenseAccountId">The Unique Id of the APInvoiceExpenseAccount.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPAPInvoiceExpenseAccountDto>> Process_DeleteAPInvoiceExpenseAccount(Guid aPInvoiceExpenseAccountId);
}
