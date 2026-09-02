using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartClassPlantModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartClassPlants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartClassPlants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartClassPlants(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartClassPlant information based on the specified PartClassPlant Unique Id.
	/// </summary>
	/// <param name="partClassPlantId">The Unique Id of the PartClassPlant.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartClassPlant(Guid partClassPlantId);

	/// <summary>
	/// Processes the request to retrieve all PartClassPlants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartClassPlants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartClassPlants DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartClassPlantDto>>> Process_GetAllPartClassPlants(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartClassPlant.
	/// </summary>
	/// <param name="partClassPlantId">The Unique Id of the PartClassPlant to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartClassPlant DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartClassPlantDto>> Process_GetPartClassPlant(Guid partClassPlantId);
}
