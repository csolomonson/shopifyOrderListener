using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAssetScheduleModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all AssetSchedules with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetSchedules to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAssetSchedules(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving AssetSchedule information based on the specified AssetSchedule Unique Id.
	/// </summary>
	/// <param name="assetScheduleId">The Unique Id of the AssetSchedule.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAssetSchedule(Guid assetScheduleId);

	/// <summary>
	/// Validates the PUT request for creating or updating AssetSchedule information based on the specified AssetSchedule.
	/// </summary>
	/// <param name="assetSchedule">The AssetSchedule details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutAssetSchedule(ERPAssetScheduleDto assetSchedule);

	/// <summary>
	/// Processes the request to retrieve all AssetSchedules with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetSchedules to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AssetSchedules DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAssetScheduleDto>>> Process_GetAllAssetSchedules(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific AssetSchedule.
	/// </summary>
	/// <param name="assetScheduleId">The Unique Id of the AssetSchedule to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the AssetSchedule DTO.</returns>
	Task<ERPResponseMessageDto<ERPAssetScheduleDto>> Process_GetAssetSchedule(Guid assetScheduleId);

	/// <summary>
	/// Processes the creating or updating of a AssetSchedule record.
	/// </summary>
	/// <param name="assetSchedule">The AssetSchedule data transfer object (DTO) containing the details of the AssetSchedule to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the AssetSchedule details.</returns>
	Task<ERPResponseMessageDto<ERPAssetScheduleDto>> Process_PutAssetSchedule(ERPAssetScheduleDto assetSchedule);

	/// <summary>
	/// Validates the request for deleting a AssetSchedule record.
	/// </summary>
	/// <param name="assetScheduleId">The Unique Id of the AssetSchedule.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteAssetSchedule(Guid assetScheduleId);

	/// <summary>
	/// Processes the request to delete a AssetSchedule record.
	/// </summary>
	/// <param name="assetScheduleId">The Unique Id of the AssetSchedule.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPAssetScheduleDto>> Process_DeleteAssetSchedule(Guid assetScheduleId);
}
