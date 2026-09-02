using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPInspectionModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Inspections with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Inspections to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllInspections(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Inspection information based on the specified Inspection Unique Id.
	/// </summary>
	/// <param name="inspectionId">The Unique Id of the Inspection.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetInspection(Guid inspectionId);

	/// <summary>
	/// Validates the PUT request for creating or updating Inspection information based on the specified Inspection.
	/// </summary>
	/// <param name="inspection">The Inspection details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutInspection(ERPInspectionDto inspection);

	/// <summary>
	/// Processes the request to retrieve all Inspections with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Inspections to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Inspections DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPInspectionDto>>> Process_GetAllInspections(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Inspection.
	/// </summary>
	/// <param name="inspectionId">The Unique Id of the Inspection to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Inspection DTO.</returns>
	Task<ERPResponseMessageDto<ERPInspectionDto>> Process_GetInspection(Guid inspectionId);

	/// <summary>
	/// Processes the creating or updating of a Inspection record.
	/// </summary>
	/// <param name="inspection">The Inspection data transfer object (DTO) containing the details of the Inspection to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Inspection details.</returns>
	Task<ERPResponseMessageDto<ERPInspectionDto>> Process_PutInspection(ERPInspectionDto inspection);

	/// <summary>
	/// Validates the request for deleting a Inspection record.
	/// </summary>
	/// <param name="inspectionId">The Unique Id of the Inspection.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteInspection(Guid inspectionId);

	/// <summary>
	/// Processes the request to delete a Inspection record.
	/// </summary>
	/// <param name="inspectionId">The Unique Id of the Inspection.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPInspectionDto>> Process_DeleteInspection(Guid inspectionId);
}
