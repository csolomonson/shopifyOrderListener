using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPInspectionLineApprovalModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all InspectionLineApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of InspectionLineApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllInspectionLineApprovals(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving InspectionLineApproval information based on the specified InspectionLineApproval Unique Id.
	/// </summary>
	/// <param name="inspectionLineApprovalId">The Unique Id of the InspectionLineApproval.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetInspectionLineApproval(Guid inspectionLineApprovalId);

	/// <summary>
	/// Validates the PUT request for creating or updating InspectionLineApproval information based on the specified InspectionLineApproval.
	/// </summary>
	/// <param name="inspectionLineApproval">The InspectionLineApproval details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutInspectionLineApproval(ERPInspectionLineApprovalDto inspectionLineApproval);

	/// <summary>
	/// Processes the request to retrieve all InspectionLineApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of InspectionLineApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of InspectionLineApprovals DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPInspectionLineApprovalDto>>> Process_GetAllInspectionLineApprovals(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific InspectionLineApproval.
	/// </summary>
	/// <param name="inspectionLineApprovalId">The Unique Id of the InspectionLineApproval to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the InspectionLineApproval DTO.</returns>
	Task<ERPResponseMessageDto<ERPInspectionLineApprovalDto>> Process_GetInspectionLineApproval(Guid inspectionLineApprovalId);

	/// <summary>
	/// Processes the creating or updating of a InspectionLineApproval record.
	/// </summary>
	/// <param name="inspectionLineApproval">The InspectionLineApproval data transfer object (DTO) containing the details of the InspectionLineApproval to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the InspectionLineApproval details.</returns>
	Task<ERPResponseMessageDto<ERPInspectionLineApprovalDto>> Process_PutInspectionLineApproval(ERPInspectionLineApprovalDto inspectionLineApproval);

	/// <summary>
	/// Validates the request for deleting a InspectionLineApproval record.
	/// </summary>
	/// <param name="inspectionLineApprovalId">The Unique Id of the InspectionLineApproval.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteInspectionLineApproval(Guid inspectionLineApprovalId);

	/// <summary>
	/// Processes the request to delete a InspectionLineApproval record.
	/// </summary>
	/// <param name="inspectionLineApprovalId">The Unique Id of the InspectionLineApproval.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPInspectionLineApprovalDto>> Process_DeleteInspectionLineApproval(Guid inspectionLineApprovalId);
}
