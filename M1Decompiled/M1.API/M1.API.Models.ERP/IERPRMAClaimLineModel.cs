using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPRMAClaimLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all RMAClaimLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAClaimLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllRMAClaimLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving RMAClaimLine information based on the specified RMAClaimLine Unique Id.
	/// </summary>
	/// <param name="rMAClaimLineId">The Unique Id of the RMAClaimLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetRMAClaimLine(Guid rMAClaimLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating RMAClaimLine information based on the specified RMAClaimLine.
	/// </summary>
	/// <param name="rMAClaimLine">The RMAClaimLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutRMAClaimLine(ERPRMAClaimLineDto rMAClaimLine);

	/// <summary>
	/// Processes the request to retrieve all RMAClaimLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAClaimLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RMAClaimLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPRMAClaimLineDto>>> Process_GetAllRMAClaimLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific RMAClaimLine.
	/// </summary>
	/// <param name="rMAClaimLineId">The Unique Id of the RMAClaimLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the RMAClaimLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPRMAClaimLineDto>> Process_GetRMAClaimLine(Guid rMAClaimLineId);

	/// <summary>
	/// Processes the creating or updating of a RMAClaimLine record.
	/// </summary>
	/// <param name="rMAClaimLine">The RMAClaimLine data transfer object (DTO) containing the details of the RMAClaimLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the RMAClaimLine details.</returns>
	Task<ERPResponseMessageDto<ERPRMAClaimLineDto>> Process_PutRMAClaimLine(ERPRMAClaimLineDto rMAClaimLine);

	/// <summary>
	/// Validates the request for deleting a RMAClaimLine record.
	/// </summary>
	/// <param name="rMAClaimLineId">The Unique Id of the RMAClaimLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteRMAClaimLine(Guid rMAClaimLineId);

	/// <summary>
	/// Processes the request to delete a RMAClaimLine record.
	/// </summary>
	/// <param name="rMAClaimLineId">The Unique Id of the RMAClaimLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPRMAClaimLineDto>> Process_DeleteRMAClaimLine(Guid rMAClaimLineId);
}
