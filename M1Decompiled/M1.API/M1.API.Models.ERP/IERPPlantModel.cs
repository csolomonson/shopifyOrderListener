using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPlantModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Plants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Plants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPlants(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Plant information based on the specified Plant Unique Id.
	/// </summary>
	/// <param name="plantId">The Unique Id of the Plant.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPlant(Guid plantId);

	/// <summary>
	/// Validates the PUT request for creating or updating Plant information based on the specified Plant.
	/// </summary>
	/// <param name="plant">The Plant details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPlant(ERPPlantDto plant);

	/// <summary>
	/// Processes the request to retrieve all Plants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Plants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Plants DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPlantDto>>> Process_GetAllPlants(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Plant.
	/// </summary>
	/// <param name="plantId">The Unique Id of the Plant to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Plant DTO.</returns>
	Task<ERPResponseMessageDto<ERPPlantDto>> Process_GetPlant(Guid plantId);

	/// <summary>
	/// Processes the creating or updating of a Plant record.
	/// </summary>
	/// <param name="plant">The Plant data transfer object (DTO) containing the details of the Plant to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Plant details.</returns>
	Task<ERPResponseMessageDto<ERPPlantDto>> Process_PutPlant(ERPPlantDto plant);

	/// <summary>
	/// Validates the request for deleting a Plant record.
	/// </summary>
	/// <param name="plantId">The Unique Id of the Plant.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePlant(Guid plantId);

	/// <summary>
	/// Processes the request to delete a Plant record.
	/// </summary>
	/// <param name="plantId">The Unique Id of the Plant.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPlantDto>> Process_DeletePlant(Guid plantId);
}
