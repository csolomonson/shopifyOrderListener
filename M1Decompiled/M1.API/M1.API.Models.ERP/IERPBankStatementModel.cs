using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPBankStatementModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all BankStatements with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of BankStatements to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllBankStatements(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving BankStatement information based on the specified BankStatement Unique Id.
	/// </summary>
	/// <param name="bankStatementId">The Unique Id of the BankStatement.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetBankStatement(Guid bankStatementId);

	/// <summary>
	/// Validates the PUT request for creating or updating BankStatement information based on the specified BankStatement.
	/// </summary>
	/// <param name="bankStatement">The BankStatement details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutBankStatement(ERPBankStatementDto bankStatement);

	/// <summary>
	/// Processes the request to retrieve all BankStatements with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of BankStatements to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of BankStatements DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPBankStatementDto>>> Process_GetAllBankStatements(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific BankStatement.
	/// </summary>
	/// <param name="bankStatementId">The Unique Id of the BankStatement to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the BankStatement DTO.</returns>
	Task<ERPResponseMessageDto<ERPBankStatementDto>> Process_GetBankStatement(Guid bankStatementId);

	/// <summary>
	/// Processes the creating or updating of a BankStatement record.
	/// </summary>
	/// <param name="bankStatement">The BankStatement data transfer object (DTO) containing the details of the BankStatement to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the BankStatement details.</returns>
	Task<ERPResponseMessageDto<ERPBankStatementDto>> Process_PutBankStatement(ERPBankStatementDto bankStatement);

	/// <summary>
	/// Validates the request for deleting a BankStatement record.
	/// </summary>
	/// <param name="bankStatementId">The Unique Id of the BankStatement.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteBankStatement(Guid bankStatementId);

	/// <summary>
	/// Processes the request to delete a BankStatement record.
	/// </summary>
	/// <param name="bankStatementId">The Unique Id of the BankStatement.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPBankStatementDto>> Process_DeleteBankStatement(Guid bankStatementId);
}
