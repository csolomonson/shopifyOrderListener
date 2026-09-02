using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPGLAccountModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all GLAccounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLAccounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllGLAccounts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving GLAccount information based on the specified GLAccount Unique Id.
	/// </summary>
	/// <param name="gLAccountId">The Unique Id of the GLAccount.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetGLAccount(Guid gLAccountId);

	/// <summary>
	/// Validates the PUT request for creating or updating GLAccount information based on the specified GLAccount.
	/// </summary>
	/// <param name="gLAccount">The GLAccount details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutGLAccount(ERPGLAccountDto gLAccount);

	/// <summary>
	/// Processes the request to retrieve all GLAccounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLAccounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLAccounts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPGLAccountDto>>> Process_GetAllGLAccounts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific GLAccount.
	/// </summary>
	/// <param name="gLAccountId">The Unique Id of the GLAccount to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the GLAccount DTO.</returns>
	Task<ERPResponseMessageDto<ERPGLAccountDto>> Process_GetGLAccount(Guid gLAccountId);

	/// <summary>
	/// Processes the creating or updating of a GLAccount record.
	/// </summary>
	/// <param name="gLAccount">The GLAccount data transfer object (DTO) containing the details of the GLAccount to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the GLAccount details.</returns>
	Task<ERPResponseMessageDto<ERPGLAccountDto>> Process_PutGLAccount(ERPGLAccountDto gLAccount);

	/// <summary>
	/// Validates the request for deleting a GLAccount record.
	/// </summary>
	/// <param name="gLAccountId">The Unique Id of the GLAccount.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteGLAccount(Guid gLAccountId);

	/// <summary>
	/// Processes the request to delete a GLAccount record.
	/// </summary>
	/// <param name="gLAccountId">The Unique Id of the GLAccount.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPGLAccountDto>> Process_DeleteGLAccount(Guid gLAccountId);
}
