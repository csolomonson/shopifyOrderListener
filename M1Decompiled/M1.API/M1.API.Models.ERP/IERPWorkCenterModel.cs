using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPWorkCenterModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all WorkCenters with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WorkCenters to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllWorkCenters(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving WorkCenter information based on the specified WorkCenter Unique Id.
	/// </summary>
	/// <param name="workCenterId">The Unique Id of the WorkCenter.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetWorkCenter(Guid workCenterId);

	/// <summary>
	/// Validates the PUT request for creating or updating WorkCenter information based on the specified WorkCenter.
	/// </summary>
	/// <param name="workCenter">The WorkCenter details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutWorkCenter(ERPWorkCenterDto workCenter);

	/// <summary>
	/// Processes the request to retrieve all WorkCenters with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WorkCenters to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WorkCenters DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPWorkCenterDto>>> Process_GetAllWorkCenters(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific WorkCenter.
	/// </summary>
	/// <param name="workCenterId">The Unique Id of the WorkCenter to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the WorkCenter DTO.</returns>
	Task<ERPResponseMessageDto<ERPWorkCenterDto>> Process_GetWorkCenter(Guid workCenterId);

	/// <summary>
	/// Processes the creating or updating of a WorkCenter record.
	/// </summary>
	/// <param name="workCenter">The WorkCenter data transfer object (DTO) containing the details of the WorkCenter to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the WorkCenter details.</returns>
	Task<ERPResponseMessageDto<ERPWorkCenterDto>> Process_PutWorkCenter(ERPWorkCenterDto workCenter);

	/// <summary>
	/// Validates the request for deleting a WorkCenter record.
	/// </summary>
	/// <param name="workCenterId">The Unique Id of the WorkCenter.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteWorkCenter(Guid workCenterId);

	/// <summary>
	/// Processes the request to delete a WorkCenter record.
	/// </summary>
	/// <param name="workCenterId">The Unique Id of the WorkCenter.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPWorkCenterDto>> Process_DeleteWorkCenter(Guid workCenterId);
}
