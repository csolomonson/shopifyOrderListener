using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPInspectionComponentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all InspectionComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of InspectionComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllInspectionComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving InspectionComponent information based on the specified InspectionComponent Unique Id.
	/// </summary>
	/// <param name="inspectionComponentId">The Unique Id of the InspectionComponent.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetInspectionComponent(Guid inspectionComponentId);

	/// <summary>
	/// Validates the PUT request for creating or updating InspectionComponent information based on the specified InspectionComponent.
	/// </summary>
	/// <param name="inspectionComponent">The InspectionComponent details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutInspectionComponent(ERPInspectionComponentDto inspectionComponent);

	/// <summary>
	/// Processes the request to retrieve all InspectionComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of InspectionComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of InspectionComponents DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPInspectionComponentDto>>> Process_GetAllInspectionComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific InspectionComponent.
	/// </summary>
	/// <param name="inspectionComponentId">The Unique Id of the InspectionComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the InspectionComponent DTO.</returns>
	Task<ERPResponseMessageDto<ERPInspectionComponentDto>> Process_GetInspectionComponent(Guid inspectionComponentId);

	/// <summary>
	/// Processes the creating or updating of a InspectionComponent record.
	/// </summary>
	/// <param name="inspectionComponent">The InspectionComponent data transfer object (DTO) containing the details of the InspectionComponent to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the InspectionComponent details.</returns>
	Task<ERPResponseMessageDto<ERPInspectionComponentDto>> Process_PutInspectionComponent(ERPInspectionComponentDto inspectionComponent);

	/// <summary>
	/// Validates the request for deleting a InspectionComponent record.
	/// </summary>
	/// <param name="inspectionComponentId">The Unique Id of the InspectionComponent.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteInspectionComponent(Guid inspectionComponentId);

	/// <summary>
	/// Processes the request to delete a InspectionComponent record.
	/// </summary>
	/// <param name="inspectionComponentId">The Unique Id of the InspectionComponent.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPInspectionComponentDto>> Process_DeleteInspectionComponent(Guid inspectionComponentId);
}
