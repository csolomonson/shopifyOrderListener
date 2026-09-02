using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPDMRShipmentLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all DMRShipmentLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRShipmentLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllDMRShipmentLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving DMRShipmentLine information based on the specified DMRShipmentLine Unique Id.
	/// </summary>
	/// <param name="dMRShipmentLineId">The Unique Id of the DMRShipmentLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetDMRShipmentLine(Guid dMRShipmentLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating DMRShipmentLine information based on the specified DMRShipmentLine.
	/// </summary>
	/// <param name="dMRShipmentLine">The DMRShipmentLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutDMRShipmentLine(ERPDMRShipmentLineDto dMRShipmentLine);

	/// <summary>
	/// Processes the request to retrieve all DMRShipmentLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRShipmentLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of DMRShipmentLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPDMRShipmentLineDto>>> Process_GetAllDMRShipmentLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific DMRShipmentLine.
	/// </summary>
	/// <param name="dMRShipmentLineId">The Unique Id of the DMRShipmentLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the DMRShipmentLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPDMRShipmentLineDto>> Process_GetDMRShipmentLine(Guid dMRShipmentLineId);

	/// <summary>
	/// Processes the creating or updating of a DMRShipmentLine record.
	/// </summary>
	/// <param name="dMRShipmentLine">The DMRShipmentLine data transfer object (DTO) containing the details of the DMRShipmentLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the DMRShipmentLine details.</returns>
	Task<ERPResponseMessageDto<ERPDMRShipmentLineDto>> Process_PutDMRShipmentLine(ERPDMRShipmentLineDto dMRShipmentLine);

	/// <summary>
	/// Validates the request for deleting a DMRShipmentLine record.
	/// </summary>
	/// <param name="dMRShipmentLineId">The Unique Id of the DMRShipmentLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteDMRShipmentLine(Guid dMRShipmentLineId);

	/// <summary>
	/// Processes the request to delete a DMRShipmentLine record.
	/// </summary>
	/// <param name="dMRShipmentLineId">The Unique Id of the DMRShipmentLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPDMRShipmentLineDto>> Process_DeleteDMRShipmentLine(Guid dMRShipmentLineId);
}
