using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPTaxCodePlantModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all TaxCodePlants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of TaxCodePlants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllTaxCodePlants(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving TaxCodePlant information based on the specified TaxCodePlant Unique Id.
	/// </summary>
	/// <param name="taxCodePlantId">The Unique Id of the TaxCodePlant.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetTaxCodePlant(Guid taxCodePlantId);

	/// <summary>
	/// Validates the PUT request for creating or updating TaxCodePlant information based on the specified TaxCodePlant.
	/// </summary>
	/// <param name="taxCodePlant">The TaxCodePlant details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutTaxCodePlant(ERPTaxCodePlantDto taxCodePlant);

	/// <summary>
	/// Processes the request to retrieve all TaxCodePlants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of TaxCodePlants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of TaxCodePlants DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPTaxCodePlantDto>>> Process_GetAllTaxCodePlants(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific TaxCodePlant.
	/// </summary>
	/// <param name="taxCodePlantId">The Unique Id of the TaxCodePlant to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the TaxCodePlant DTO.</returns>
	Task<ERPResponseMessageDto<ERPTaxCodePlantDto>> Process_GetTaxCodePlant(Guid taxCodePlantId);

	/// <summary>
	/// Processes the creating or updating of a TaxCodePlant record.
	/// </summary>
	/// <param name="taxCodePlant">The TaxCodePlant data transfer object (DTO) containing the details of the TaxCodePlant to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the TaxCodePlant details.</returns>
	Task<ERPResponseMessageDto<ERPTaxCodePlantDto>> Process_PutTaxCodePlant(ERPTaxCodePlantDto taxCodePlant);

	/// <summary>
	/// Validates the request for deleting a TaxCodePlant record.
	/// </summary>
	/// <param name="taxCodePlantId">The Unique Id of the TaxCodePlant.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteTaxCodePlant(Guid taxCodePlantId);

	/// <summary>
	/// Processes the request to delete a TaxCodePlant record.
	/// </summary>
	/// <param name="taxCodePlantId">The Unique Id of the TaxCodePlant.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPTaxCodePlantDto>> Process_DeleteTaxCodePlant(Guid taxCodePlantId);
}
