using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPReasonPlantModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ReasonPlants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ReasonPlants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllReasonPlants(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ReasonPlant information based on the specified ReasonPlant Unique Id.
	/// </summary>
	/// <param name="reasonPlantId">The Unique Id of the ReasonPlant.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetReasonPlant(Guid reasonPlantId);

	/// <summary>
	/// Processes the request to retrieve all ReasonPlants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ReasonPlants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ReasonPlants DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPReasonPlantDto>>> Process_GetAllReasonPlants(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ReasonPlant.
	/// </summary>
	/// <param name="reasonPlantId">The Unique Id of the ReasonPlant to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ReasonPlant DTO.</returns>
	Task<ERPResponseMessageDto<ERPReasonPlantDto>> Process_GetReasonPlant(Guid reasonPlantId);
}
