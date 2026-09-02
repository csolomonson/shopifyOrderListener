using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPSerialNumberStatusModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all SerialNumberStatuses with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SerialNumberStatuses to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllSerialNumberStatuses(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving SerialNumberStatus information based on the specified SerialNumberStatus Unique Id.
	/// </summary>
	/// <param name="serialNumberStatusId">The Unique Id of the SerialNumberStatus.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSerialNumberStatus(Guid serialNumberStatusId);

	/// <summary>
	/// Validates the PUT request for creating or updating SerialNumberStatus information based on the specified SerialNumberStatus.
	/// </summary>
	/// <param name="serialNumberStatus">The SerialNumberStatus details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutSerialNumberStatus(ERPSerialNumberStatusDto serialNumberStatus);

	/// <summary>
	/// Processes the request to retrieve all SerialNumberStatuses with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SerialNumberStatuses to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SerialNumberStatuses DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPSerialNumberStatusDto>>> Process_GetAllSerialNumberStatuses(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific SerialNumberStatus.
	/// </summary>
	/// <param name="serialNumberStatusId">The Unique Id of the SerialNumberStatus to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the SerialNumberStatus DTO.</returns>
	Task<ERPResponseMessageDto<ERPSerialNumberStatusDto>> Process_GetSerialNumberStatus(Guid serialNumberStatusId);

	/// <summary>
	/// Processes the creating or updating of a SerialNumberStatus record.
	/// </summary>
	/// <param name="serialNumberStatus">The SerialNumberStatus data transfer object (DTO) containing the details of the SerialNumberStatus to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the SerialNumberStatus details.</returns>
	Task<ERPResponseMessageDto<ERPSerialNumberStatusDto>> Process_PutSerialNumberStatus(ERPSerialNumberStatusDto serialNumberStatus);

	/// <summary>
	/// Validates the request for deleting a SerialNumberStatus record.
	/// </summary>
	/// <param name="serialNumberStatusId">The Unique Id of the SerialNumberStatus.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteSerialNumberStatus(Guid serialNumberStatusId);

	/// <summary>
	/// Processes the request to delete a SerialNumberStatus record.
	/// </summary>
	/// <param name="serialNumberStatusId">The Unique Id of the SerialNumberStatus.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPSerialNumberStatusDto>> Process_DeleteSerialNumberStatus(Guid serialNumberStatusId);
}
