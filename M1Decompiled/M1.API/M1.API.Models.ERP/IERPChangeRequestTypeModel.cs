using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPChangeRequestTypeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ChangeRequestTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ChangeRequestTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllChangeRequestTypes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ChangeRequestType information based on the specified ChangeRequestType Unique Id.
	/// </summary>
	/// <param name="changeRequestTypeId">The Unique Id of the ChangeRequestType.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetChangeRequestType(Guid changeRequestTypeId);

	/// <summary>
	/// Validates the PUT request for creating or updating ChangeRequestType information based on the specified ChangeRequestType.
	/// </summary>
	/// <param name="changeRequestType">The ChangeRequestType details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutChangeRequestType(ERPChangeRequestTypeDto changeRequestType);

	/// <summary>
	/// Processes the request to retrieve all ChangeRequestTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ChangeRequestTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ChangeRequestTypes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPChangeRequestTypeDto>>> Process_GetAllChangeRequestTypes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ChangeRequestType.
	/// </summary>
	/// <param name="changeRequestTypeId">The Unique Id of the ChangeRequestType to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ChangeRequestType DTO.</returns>
	Task<ERPResponseMessageDto<ERPChangeRequestTypeDto>> Process_GetChangeRequestType(Guid changeRequestTypeId);

	/// <summary>
	/// Processes the creating or updating of a ChangeRequestType record.
	/// </summary>
	/// <param name="changeRequestType">The ChangeRequestType data transfer object (DTO) containing the details of the ChangeRequestType to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ChangeRequestType details.</returns>
	Task<ERPResponseMessageDto<ERPChangeRequestTypeDto>> Process_PutChangeRequestType(ERPChangeRequestTypeDto changeRequestType);

	/// <summary>
	/// Validates the request for deleting a ChangeRequestType record.
	/// </summary>
	/// <param name="changeRequestTypeId">The Unique Id of the ChangeRequestType.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteChangeRequestType(Guid changeRequestTypeId);

	/// <summary>
	/// Processes the request to delete a ChangeRequestType record.
	/// </summary>
	/// <param name="changeRequestTypeId">The Unique Id of the ChangeRequestType.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPChangeRequestTypeDto>> Process_DeleteChangeRequestType(Guid changeRequestTypeId);
}
