using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPDMRClaimModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all DMRClaims with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRClaims to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllDMRClaims(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving DMRClaim information based on the specified DMRClaim Unique Id.
	/// </summary>
	/// <param name="dMRClaimId">The Unique Id of the DMRClaim.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetDMRClaim(Guid dMRClaimId);

	/// <summary>
	/// Validates the PUT request for creating or updating DMRClaim information based on the specified DMRClaim.
	/// </summary>
	/// <param name="dMRClaim">The DMRClaim details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutDMRClaim(ERPDMRClaimDto dMRClaim);

	/// <summary>
	/// Processes the request to retrieve all DMRClaims with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRClaims to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of DMRClaims DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPDMRClaimDto>>> Process_GetAllDMRClaims(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific DMRClaim.
	/// </summary>
	/// <param name="dMRClaimId">The Unique Id of the DMRClaim to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the DMRClaim DTO.</returns>
	Task<ERPResponseMessageDto<ERPDMRClaimDto>> Process_GetDMRClaim(Guid dMRClaimId);

	/// <summary>
	/// Processes the creating or updating of a DMRClaim record.
	/// </summary>
	/// <param name="dMRClaim">The DMRClaim data transfer object (DTO) containing the details of the DMRClaim to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the DMRClaim details.</returns>
	Task<ERPResponseMessageDto<ERPDMRClaimDto>> Process_PutDMRClaim(ERPDMRClaimDto dMRClaim);

	/// <summary>
	/// Validates the request for deleting a DMRClaim record.
	/// </summary>
	/// <param name="dMRClaimId">The Unique Id of the DMRClaim.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteDMRClaim(Guid dMRClaimId);

	/// <summary>
	/// Processes the request to delete a DMRClaim record.
	/// </summary>
	/// <param name="dMRClaimId">The Unique Id of the DMRClaim.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPDMRClaimDto>> Process_DeleteDMRClaim(Guid dMRClaimId);
}
