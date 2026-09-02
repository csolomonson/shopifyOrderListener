using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPBankAccountModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all BankAccounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of BankAccounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllBankAccounts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving BankAccount information based on the specified BankAccount Unique Id.
	/// </summary>
	/// <param name="bankAccountId">The Unique Id of the BankAccount.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetBankAccount(Guid bankAccountId);

	/// <summary>
	/// Validates the PUT request for creating or updating BankAccount information based on the specified BankAccount.
	/// </summary>
	/// <param name="bankAccount">The BankAccount details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutBankAccount(ERPBankAccountDto bankAccount);

	/// <summary>
	/// Processes the request to retrieve all BankAccounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of BankAccounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of BankAccounts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPBankAccountDto>>> Process_GetAllBankAccounts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific BankAccount.
	/// </summary>
	/// <param name="bankAccountId">The Unique Id of the BankAccount to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the BankAccount DTO.</returns>
	Task<ERPResponseMessageDto<ERPBankAccountDto>> Process_GetBankAccount(Guid bankAccountId);

	/// <summary>
	/// Processes the creating or updating of a BankAccount record.
	/// </summary>
	/// <param name="bankAccount">The BankAccount data transfer object (DTO) containing the details of the BankAccount to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the BankAccount details.</returns>
	Task<ERPResponseMessageDto<ERPBankAccountDto>> Process_PutBankAccount(ERPBankAccountDto bankAccount);

	/// <summary>
	/// Validates the request for deleting a BankAccount record.
	/// </summary>
	/// <param name="bankAccountId">The Unique Id of the BankAccount.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteBankAccount(Guid bankAccountId);

	/// <summary>
	/// Processes the request to delete a BankAccount record.
	/// </summary>
	/// <param name="bankAccountId">The Unique Id of the BankAccount.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPBankAccountDto>> Process_DeleteBankAccount(Guid bankAccountId);
}
