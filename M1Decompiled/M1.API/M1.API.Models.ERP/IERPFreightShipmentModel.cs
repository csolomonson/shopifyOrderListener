using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPFreightShipmentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all FreightShipments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FreightShipments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllFreightShipments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving FreightShipment information based on the specified FreightShipment Unique Id.
	/// </summary>
	/// <param name="freightShipmentId">The Unique Id of the FreightShipment.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetFreightShipment(Guid freightShipmentId);

	/// <summary>
	/// Validates the PUT request for creating or updating FreightShipment information based on the specified FreightShipment.
	/// </summary>
	/// <param name="freightShipment">The FreightShipment details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutFreightShipment(ERPFreightShipmentDto freightShipment);

	/// <summary>
	/// Processes the request to retrieve all FreightShipments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FreightShipments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of FreightShipments DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPFreightShipmentDto>>> Process_GetAllFreightShipments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific FreightShipment.
	/// </summary>
	/// <param name="freightShipmentId">The Unique Id of the FreightShipment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the FreightShipment DTO.</returns>
	Task<ERPResponseMessageDto<ERPFreightShipmentDto>> Process_GetFreightShipment(Guid freightShipmentId);

	/// <summary>
	/// Processes the creating or updating of a FreightShipment record.
	/// </summary>
	/// <param name="freightShipment">The FreightShipment data transfer object (DTO) containing the details of the FreightShipment to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the FreightShipment details.</returns>
	Task<ERPResponseMessageDto<ERPFreightShipmentDto>> Process_PutFreightShipment(ERPFreightShipmentDto freightShipment);

	/// <summary>
	/// Validates the request for deleting a FreightShipment record.
	/// </summary>
	/// <param name="freightShipmentId">The Unique Id of the FreightShipment.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteFreightShipment(Guid freightShipmentId);

	/// <summary>
	/// Processes the request to delete a FreightShipment record.
	/// </summary>
	/// <param name="freightShipmentId">The Unique Id of the FreightShipment.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPFreightShipmentDto>> Process_DeleteFreightShipment(Guid freightShipmentId);
}
