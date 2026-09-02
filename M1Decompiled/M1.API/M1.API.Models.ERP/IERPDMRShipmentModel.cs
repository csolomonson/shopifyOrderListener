using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPDMRShipmentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all DMRShipments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRShipments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllDMRShipments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving DMRShipment information based on the specified DMRShipment Unique Id.
	/// </summary>
	/// <param name="dMRShipmentId">The Unique Id of the DMRShipment.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetDMRShipment(Guid dMRShipmentId);

	/// <summary>
	/// Validates the PUT request for creating or updating DMRShipment information based on the specified DMRShipment.
	/// </summary>
	/// <param name="dMRShipment">The DMRShipment details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutDMRShipment(ERPDMRShipmentDto dMRShipment);

	/// <summary>
	/// Processes the request to retrieve all DMRShipments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRShipments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of DMRShipments DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPDMRShipmentDto>>> Process_GetAllDMRShipments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific DMRShipment.
	/// </summary>
	/// <param name="dMRShipmentId">The Unique Id of the DMRShipment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the DMRShipment DTO.</returns>
	Task<ERPResponseMessageDto<ERPDMRShipmentDto>> Process_GetDMRShipment(Guid dMRShipmentId);

	/// <summary>
	/// Processes the creating or updating of a DMRShipment record.
	/// </summary>
	/// <param name="dMRShipment">The DMRShipment data transfer object (DTO) containing the details of the DMRShipment to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the DMRShipment details.</returns>
	Task<ERPResponseMessageDto<ERPDMRShipmentDto>> Process_PutDMRShipment(ERPDMRShipmentDto dMRShipment);

	/// <summary>
	/// Validates the request for deleting a DMRShipment record.
	/// </summary>
	/// <param name="dMRShipmentId">The Unique Id of the DMRShipment.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteDMRShipment(Guid dMRShipmentId);

	/// <summary>
	/// Processes the request to delete a DMRShipment record.
	/// </summary>
	/// <param name="dMRShipmentId">The Unique Id of the DMRShipment.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPDMRShipmentDto>> Process_DeleteDMRShipment(Guid dMRShipmentId);
}
