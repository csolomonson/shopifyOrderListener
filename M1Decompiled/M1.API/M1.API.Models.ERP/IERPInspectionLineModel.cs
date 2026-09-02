using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPInspectionLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all InspectionLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of InspectionLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllInspectionLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving InspectionLine information based on the specified InspectionLine Unique Id.
	/// </summary>
	/// <param name="inspectionLineId">The Unique Id of the InspectionLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetInspectionLine(Guid inspectionLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating InspectionLine information based on the specified InspectionLine.
	/// </summary>
	/// <param name="inspectionLine">The InspectionLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutInspectionLine(ERPInspectionLineDto inspectionLine);

	/// <summary>
	/// Processes the request to retrieve all InspectionLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of InspectionLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of InspectionLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPInspectionLineDto>>> Process_GetAllInspectionLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific InspectionLine.
	/// </summary>
	/// <param name="inspectionLineId">The Unique Id of the InspectionLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the InspectionLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPInspectionLineDto>> Process_GetInspectionLine(Guid inspectionLineId);

	/// <summary>
	/// Processes the creating or updating of a InspectionLine record.
	/// </summary>
	/// <param name="inspectionLine">The InspectionLine data transfer object (DTO) containing the details of the InspectionLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the InspectionLine details.</returns>
	Task<ERPResponseMessageDto<ERPInspectionLineDto>> Process_PutInspectionLine(ERPInspectionLineDto inspectionLine);

	/// <summary>
	/// Validates the request for deleting a InspectionLine record.
	/// </summary>
	/// <param name="inspectionLineId">The Unique Id of the InspectionLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteInspectionLine(Guid inspectionLineId);

	/// <summary>
	/// Processes the request to delete a InspectionLine record.
	/// </summary>
	/// <param name="inspectionLineId">The Unique Id of the InspectionLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPInspectionLineDto>> Process_DeleteInspectionLine(Guid inspectionLineId);
}
