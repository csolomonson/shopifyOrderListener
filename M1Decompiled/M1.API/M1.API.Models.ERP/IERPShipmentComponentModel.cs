using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPShipmentComponentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ShipmentComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllShipmentComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ShipmentComponent information based on the specified ShipmentComponent Unique Id.
	/// </summary>
	/// <param name="shipmentComponentId">The Unique Id of the ShipmentComponent.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetShipmentComponent(Guid shipmentComponentId);

	/// <summary>
	/// Validates the PUT request for creating or updating ShipmentComponent information based on the specified ShipmentComponent.
	/// </summary>
	/// <param name="shipmentComponent">The ShipmentComponent details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutShipmentComponent(ERPShipmentComponentDto shipmentComponent);

	/// <summary>
	/// Processes the request to retrieve all ShipmentComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShipmentComponents DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPShipmentComponentDto>>> Process_GetAllShipmentComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ShipmentComponent.
	/// </summary>
	/// <param name="shipmentComponentId">The Unique Id of the ShipmentComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ShipmentComponent DTO.</returns>
	Task<ERPResponseMessageDto<ERPShipmentComponentDto>> Process_GetShipmentComponent(Guid shipmentComponentId);

	/// <summary>
	/// Processes the creating or updating of a ShipmentComponent record.
	/// </summary>
	/// <param name="shipmentComponent">The ShipmentComponent data transfer object (DTO) containing the details of the ShipmentComponent to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ShipmentComponent details.</returns>
	Task<ERPResponseMessageDto<ERPShipmentComponentDto>> Process_PutShipmentComponent(ERPShipmentComponentDto shipmentComponent);

	/// <summary>
	/// Validates the request for deleting a ShipmentComponent record.
	/// </summary>
	/// <param name="shipmentComponentId">The Unique Id of the ShipmentComponent.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteShipmentComponent(Guid shipmentComponentId);

	/// <summary>
	/// Processes the request to delete a ShipmentComponent record.
	/// </summary>
	/// <param name="shipmentComponentId">The Unique Id of the ShipmentComponent.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPShipmentComponentDto>> Process_DeleteShipmentComponent(Guid shipmentComponentId);
}
