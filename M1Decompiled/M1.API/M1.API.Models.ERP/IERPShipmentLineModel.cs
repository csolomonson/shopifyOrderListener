using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPShipmentLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ShipmentLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllShipmentLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ShipmentLine information based on the specified ShipmentLine Unique Id.
	/// </summary>
	/// <param name="shipmentLineId">The Unique Id of the ShipmentLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetShipmentLine(Guid shipmentLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating ShipmentLine information based on the specified ShipmentLine.
	/// </summary>
	/// <param name="shipmentLine">The ShipmentLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutShipmentLine(ERPShipmentLineDto shipmentLine);

	/// <summary>
	/// Processes the request to retrieve all ShipmentLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShipmentLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPShipmentLineDto>>> Process_GetAllShipmentLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ShipmentLine.
	/// </summary>
	/// <param name="shipmentLineId">The Unique Id of the ShipmentLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ShipmentLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPShipmentLineDto>> Process_GetShipmentLine(Guid shipmentLineId);

	/// <summary>
	/// Processes the creating or updating of a ShipmentLine record.
	/// </summary>
	/// <param name="shipmentLine">The ShipmentLine data transfer object (DTO) containing the details of the ShipmentLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ShipmentLine details.</returns>
	Task<ERPResponseMessageDto<ERPShipmentLineDto>> Process_PutShipmentLine(ERPShipmentLineDto shipmentLine);

	/// <summary>
	/// Validates the request for deleting a ShipmentLine record.
	/// </summary>
	/// <param name="shipmentLineId">The Unique Id of the ShipmentLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteShipmentLine(Guid shipmentLineId);

	/// <summary>
	/// Processes the request to delete a ShipmentLine record.
	/// </summary>
	/// <param name="shipmentLineId">The Unique Id of the ShipmentLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPShipmentLineDto>> Process_DeleteShipmentLine(Guid shipmentLineId);
}
