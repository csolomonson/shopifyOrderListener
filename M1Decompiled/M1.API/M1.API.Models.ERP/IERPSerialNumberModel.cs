using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPSerialNumberModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all SerialNumbers with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SerialNumbers to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllSerialNumbers(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving SerialNumber information based on the specified SerialNumber Unique Id.
	/// </summary>
	/// <param name="serialNumberId">The Unique Id of the SerialNumber.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSerialNumber(Guid serialNumberId);

	/// <summary>
	/// Validates the PUT request for creating or updating SerialNumber information based on the specified SerialNumber.
	/// </summary>
	/// <param name="serialNumber">The SerialNumber details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutSerialNumber(ERPSerialNumberDto serialNumber);

	/// <summary>
	/// Processes the request to retrieve all SerialNumbers with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SerialNumbers to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SerialNumbers DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPSerialNumberDto>>> Process_GetAllSerialNumbers(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific SerialNumber.
	/// </summary>
	/// <param name="serialNumberId">The Unique Id of the SerialNumber to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the SerialNumber DTO.</returns>
	Task<ERPResponseMessageDto<ERPSerialNumberDto>> Process_GetSerialNumber(Guid serialNumberId);

	/// <summary>
	/// Processes the creating or updating of a SerialNumber record.
	/// </summary>
	/// <param name="serialNumber">The SerialNumber data transfer object (DTO) containing the details of the SerialNumber to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the SerialNumber details.</returns>
	Task<ERPResponseMessageDto<ERPSerialNumberDto>> Process_PutSerialNumber(ERPSerialNumberDto serialNumber);

	/// <summary>
	/// Validates the request for deleting a SerialNumber record.
	/// </summary>
	/// <param name="serialNumberId">The Unique Id of the SerialNumber.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteSerialNumber(Guid serialNumberId);

	/// <summary>
	/// Processes the request to delete a SerialNumber record.
	/// </summary>
	/// <param name="serialNumberId">The Unique Id of the SerialNumber.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPSerialNumberDto>> Process_DeleteSerialNumber(Guid serialNumberId);
}
