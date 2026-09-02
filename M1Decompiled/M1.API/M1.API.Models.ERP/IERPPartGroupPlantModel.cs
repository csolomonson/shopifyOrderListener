using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartGroupPlantModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartGroupPlants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartGroupPlants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartGroupPlants(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartGroupPlant information based on the specified PartGroupPlant Unique Id.
	/// </summary>
	/// <param name="partGroupPlantId">The Unique Id of the PartGroupPlant.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartGroupPlant(Guid partGroupPlantId);

	/// <summary>
	/// Processes the request to retrieve all PartGroupPlants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartGroupPlants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartGroupPlants DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartGroupPlantDto>>> Process_GetAllPartGroupPlants(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartGroupPlant.
	/// </summary>
	/// <param name="partGroupPlantId">The Unique Id of the PartGroupPlant to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartGroupPlant DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartGroupPlantDto>> Process_GetPartGroupPlant(Guid partGroupPlantId);
}
