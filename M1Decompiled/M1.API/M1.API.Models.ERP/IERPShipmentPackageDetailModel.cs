using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPShipmentPackageDetailModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ShipmentPackageDetails with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentPackageDetails to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllShipmentPackageDetails(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ShipmentPackageDetail information based on the specified ShipmentPackageDetail Unique Id.
	/// </summary>
	/// <param name="shipmentPackageDetailId">The Unique Id of the ShipmentPackageDetail.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetShipmentPackageDetail(Guid shipmentPackageDetailId);

	/// <summary>
	/// Validates the PUT request for creating or updating ShipmentPackageDetail information based on the specified ShipmentPackageDetail.
	/// </summary>
	/// <param name="shipmentPackageDetail">The ShipmentPackageDetail details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutShipmentPackageDetail(ERPShipmentPackageDetailDto shipmentPackageDetail);

	/// <summary>
	/// Processes the request to retrieve all ShipmentPackageDetails with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentPackageDetails to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShipmentPackageDetails DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPShipmentPackageDetailDto>>> Process_GetAllShipmentPackageDetails(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ShipmentPackageDetail.
	/// </summary>
	/// <param name="shipmentPackageDetailId">The Unique Id of the ShipmentPackageDetail to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ShipmentPackageDetail DTO.</returns>
	Task<ERPResponseMessageDto<ERPShipmentPackageDetailDto>> Process_GetShipmentPackageDetail(Guid shipmentPackageDetailId);

	/// <summary>
	/// Processes the creating or updating of a ShipmentPackageDetail record.
	/// </summary>
	/// <param name="shipmentPackageDetail">The ShipmentPackageDetail data transfer object (DTO) containing the details of the ShipmentPackageDetail to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ShipmentPackageDetail details.</returns>
	Task<ERPResponseMessageDto<ERPShipmentPackageDetailDto>> Process_PutShipmentPackageDetail(ERPShipmentPackageDetailDto shipmentPackageDetail);

	/// <summary>
	/// Validates the request for deleting a ShipmentPackageDetail record.
	/// </summary>
	/// <param name="shipmentPackageDetailId">The Unique Id of the ShipmentPackageDetail.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteShipmentPackageDetail(Guid shipmentPackageDetailId);

	/// <summary>
	/// Processes the request to delete a ShipmentPackageDetail record.
	/// </summary>
	/// <param name="shipmentPackageDetailId">The Unique Id of the ShipmentPackageDetail.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPShipmentPackageDetailDto>> Process_DeleteShipmentPackageDetail(Guid shipmentPackageDetailId);
}
