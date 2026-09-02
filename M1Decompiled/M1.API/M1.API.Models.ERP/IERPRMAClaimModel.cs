using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPRMAClaimModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all RMAClaims with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAClaims to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllRMAClaims(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving RMAClaim information based on the specified RMAClaim Unique Id.
	/// </summary>
	/// <param name="rMAClaimId">The Unique Id of the RMAClaim.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetRMAClaim(Guid rMAClaimId);

	/// <summary>
	/// Validates the PUT request for creating or updating RMAClaim information based on the specified RMAClaim.
	/// </summary>
	/// <param name="rMAClaim">The RMAClaim details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutRMAClaim(ERPRMAClaimDto rMAClaim);

	/// <summary>
	/// Processes the request to retrieve all RMAClaims with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAClaims to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RMAClaims DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPRMAClaimDto>>> Process_GetAllRMAClaims(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific RMAClaim.
	/// </summary>
	/// <param name="rMAClaimId">The Unique Id of the RMAClaim to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the RMAClaim DTO.</returns>
	Task<ERPResponseMessageDto<ERPRMAClaimDto>> Process_GetRMAClaim(Guid rMAClaimId);

	/// <summary>
	/// Processes the creating or updating of a RMAClaim record.
	/// </summary>
	/// <param name="rMAClaim">The RMAClaim data transfer object (DTO) containing the details of the RMAClaim to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the RMAClaim details.</returns>
	Task<ERPResponseMessageDto<ERPRMAClaimDto>> Process_PutRMAClaim(ERPRMAClaimDto rMAClaim);

	/// <summary>
	/// Validates the request for deleting a RMAClaim record.
	/// </summary>
	/// <param name="rMAClaimId">The Unique Id of the RMAClaim.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteRMAClaim(Guid rMAClaimId);

	/// <summary>
	/// Processes the request to delete a RMAClaim record.
	/// </summary>
	/// <param name="rMAClaimId">The Unique Id of the RMAClaim.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPRMAClaimDto>> Process_DeleteRMAClaim(Guid rMAClaimId);
}
