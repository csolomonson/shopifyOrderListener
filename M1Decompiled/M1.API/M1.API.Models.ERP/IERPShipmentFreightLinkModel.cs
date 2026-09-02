using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPShipmentFreightLinkModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ShipmentFreightLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentFreightLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllShipmentFreightLinks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ShipmentFreightLink information based on the specified ShipmentFreightLink Unique Id.
	/// </summary>
	/// <param name="shipmentFreightLinkId">The Unique Id of the ShipmentFreightLink.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetShipmentFreightLink(Guid shipmentFreightLinkId);

	/// <summary>
	/// Validates the PUT request for creating or updating ShipmentFreightLink information based on the specified ShipmentFreightLink.
	/// </summary>
	/// <param name="shipmentFreightLink">The ShipmentFreightLink details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutShipmentFreightLink(ERPShipmentFreightLinkDto shipmentFreightLink);

	/// <summary>
	/// Processes the request to retrieve all ShipmentFreightLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentFreightLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShipmentFreightLinks DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPShipmentFreightLinkDto>>> Process_GetAllShipmentFreightLinks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ShipmentFreightLink.
	/// </summary>
	/// <param name="shipmentFreightLinkId">The Unique Id of the ShipmentFreightLink to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ShipmentFreightLink DTO.</returns>
	Task<ERPResponseMessageDto<ERPShipmentFreightLinkDto>> Process_GetShipmentFreightLink(Guid shipmentFreightLinkId);

	/// <summary>
	/// Processes the creating or updating of a ShipmentFreightLink record.
	/// </summary>
	/// <param name="shipmentFreightLink">The ShipmentFreightLink data transfer object (DTO) containing the details of the ShipmentFreightLink to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ShipmentFreightLink details.</returns>
	Task<ERPResponseMessageDto<ERPShipmentFreightLinkDto>> Process_PutShipmentFreightLink(ERPShipmentFreightLinkDto shipmentFreightLink);

	/// <summary>
	/// Validates the request for deleting a ShipmentFreightLink record.
	/// </summary>
	/// <param name="shipmentFreightLinkId">The Unique Id of the ShipmentFreightLink.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteShipmentFreightLink(Guid shipmentFreightLinkId);

	/// <summary>
	/// Processes the request to delete a ShipmentFreightLink record.
	/// </summary>
	/// <param name="shipmentFreightLinkId">The Unique Id of the ShipmentFreightLink.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPShipmentFreightLinkDto>> Process_DeleteShipmentFreightLink(Guid shipmentFreightLinkId);
}
