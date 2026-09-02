using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPDMRShipmentComponentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all DMRShipmentComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRShipmentComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllDMRShipmentComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving DMRShipmentComponent information based on the specified DMRShipmentComponent Unique Id.
	/// </summary>
	/// <param name="dMRShipmentComponentId">The Unique Id of the DMRShipmentComponent.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetDMRShipmentComponent(Guid dMRShipmentComponentId);

	/// <summary>
	/// Validates the PUT request for creating or updating DMRShipmentComponent information based on the specified DMRShipmentComponent.
	/// </summary>
	/// <param name="dMRShipmentComponent">The DMRShipmentComponent details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutDMRShipmentComponent(ERPDMRShipmentComponentDto dMRShipmentComponent);

	/// <summary>
	/// Processes the request to retrieve all DMRShipmentComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRShipmentComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of DMRShipmentComponents DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPDMRShipmentComponentDto>>> Process_GetAllDMRShipmentComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific DMRShipmentComponent.
	/// </summary>
	/// <param name="dMRShipmentComponentId">The Unique Id of the DMRShipmentComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the DMRShipmentComponent DTO.</returns>
	Task<ERPResponseMessageDto<ERPDMRShipmentComponentDto>> Process_GetDMRShipmentComponent(Guid dMRShipmentComponentId);

	/// <summary>
	/// Processes the creating or updating of a DMRShipmentComponent record.
	/// </summary>
	/// <param name="dMRShipmentComponent">The DMRShipmentComponent data transfer object (DTO) containing the details of the DMRShipmentComponent to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the DMRShipmentComponent details.</returns>
	Task<ERPResponseMessageDto<ERPDMRShipmentComponentDto>> Process_PutDMRShipmentComponent(ERPDMRShipmentComponentDto dMRShipmentComponent);

	/// <summary>
	/// Validates the request for deleting a DMRShipmentComponent record.
	/// </summary>
	/// <param name="dMRShipmentComponentId">The Unique Id of the DMRShipmentComponent.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteDMRShipmentComponent(Guid dMRShipmentComponentId);

	/// <summary>
	/// Processes the request to delete a DMRShipmentComponent record.
	/// </summary>
	/// <param name="dMRShipmentComponentId">The Unique Id of the DMRShipmentComponent.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPDMRShipmentComponentDto>> Process_DeleteDMRShipmentComponent(Guid dMRShipmentComponentId);
}
