using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPBankEntryModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all BankEntries with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of BankEntries to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllBankEntries(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving BankEntry information based on the specified BankEntry Unique Id.
	/// </summary>
	/// <param name="bankEntryId">The Unique Id of the BankEntry.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetBankEntry(Guid bankEntryId);

	/// <summary>
	/// Validates the PUT request for creating or updating BankEntry information based on the specified BankEntry.
	/// </summary>
	/// <param name="bankEntry">The BankEntry details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutBankEntry(ERPBankEntryDto bankEntry);

	/// <summary>
	/// Processes the request to retrieve all BankEntries with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of BankEntries to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of BankEntries DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPBankEntryDto>>> Process_GetAllBankEntries(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific BankEntry.
	/// </summary>
	/// <param name="bankEntryId">The Unique Id of the BankEntry to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the BankEntry DTO.</returns>
	Task<ERPResponseMessageDto<ERPBankEntryDto>> Process_GetBankEntry(Guid bankEntryId);

	/// <summary>
	/// Processes the creating or updating of a BankEntry record.
	/// </summary>
	/// <param name="bankEntry">The BankEntry data transfer object (DTO) containing the details of the BankEntry to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the BankEntry details.</returns>
	Task<ERPResponseMessageDto<ERPBankEntryDto>> Process_PutBankEntry(ERPBankEntryDto bankEntry);

	/// <summary>
	/// Validates the request for deleting a BankEntry record.
	/// </summary>
	/// <param name="bankEntryId">The Unique Id of the BankEntry.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteBankEntry(Guid bankEntryId);

	/// <summary>
	/// Processes the request to delete a BankEntry record.
	/// </summary>
	/// <param name="bankEntryId">The Unique Id of the BankEntry.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPBankEntryDto>> Process_DeleteBankEntry(Guid bankEntryId);
}
