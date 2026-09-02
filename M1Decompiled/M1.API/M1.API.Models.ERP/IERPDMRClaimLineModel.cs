using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPDMRClaimLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all DMRClaimLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRClaimLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllDMRClaimLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving DMRClaimLine information based on the specified DMRClaimLine Unique Id.
	/// </summary>
	/// <param name="dMRClaimLineId">The Unique Id of the DMRClaimLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetDMRClaimLine(Guid dMRClaimLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating DMRClaimLine information based on the specified DMRClaimLine.
	/// </summary>
	/// <param name="dMRClaimLine">The DMRClaimLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutDMRClaimLine(ERPDMRClaimLineDto dMRClaimLine);

	/// <summary>
	/// Processes the request to retrieve all DMRClaimLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRClaimLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of DMRClaimLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPDMRClaimLineDto>>> Process_GetAllDMRClaimLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific DMRClaimLine.
	/// </summary>
	/// <param name="dMRClaimLineId">The Unique Id of the DMRClaimLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the DMRClaimLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPDMRClaimLineDto>> Process_GetDMRClaimLine(Guid dMRClaimLineId);

	/// <summary>
	/// Processes the creating or updating of a DMRClaimLine record.
	/// </summary>
	/// <param name="dMRClaimLine">The DMRClaimLine data transfer object (DTO) containing the details of the DMRClaimLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the DMRClaimLine details.</returns>
	Task<ERPResponseMessageDto<ERPDMRClaimLineDto>> Process_PutDMRClaimLine(ERPDMRClaimLineDto dMRClaimLine);

	/// <summary>
	/// Validates the request for deleting a DMRClaimLine record.
	/// </summary>
	/// <param name="dMRClaimLineId">The Unique Id of the DMRClaimLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteDMRClaimLine(Guid dMRClaimLineId);

	/// <summary>
	/// Processes the request to delete a DMRClaimLine record.
	/// </summary>
	/// <param name="dMRClaimLineId">The Unique Id of the DMRClaimLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPDMRClaimLineDto>> Process_DeleteDMRClaimLine(Guid dMRClaimLineId);
}
