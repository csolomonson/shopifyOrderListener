using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartMaterialModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartMaterials with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartMaterials to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartMaterials(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartMaterial information based on the specified PartMaterial Unique Id.
	/// </summary>
	/// <param name="partMaterialId">The Unique Id of the PartMaterial.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartMaterial(Guid partMaterialId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartMaterial information based on the specified PartMaterial.
	/// </summary>
	/// <param name="partMaterial">The PartMaterial details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartMaterial(ERPPartMaterialDto partMaterial);

	/// <summary>
	/// Processes the request to retrieve all PartMaterials with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartMaterials to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartMaterials DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartMaterialDto>>> Process_GetAllPartMaterials(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartMaterial.
	/// </summary>
	/// <param name="partMaterialId">The Unique Id of the PartMaterial to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartMaterial DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartMaterialDto>> Process_GetPartMaterial(Guid partMaterialId);

	/// <summary>
	/// Processes the creating or updating of a PartMaterial record.
	/// </summary>
	/// <param name="partMaterial">The PartMaterial data transfer object (DTO) containing the details of the PartMaterial to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartMaterial details.</returns>
	Task<ERPResponseMessageDto<ERPPartMaterialDto>> Process_PutPartMaterial(ERPPartMaterialDto partMaterial);

	/// <summary>
	/// Validates the request for deleting a PartMaterial record.
	/// </summary>
	/// <param name="partMaterialId">The Unique Id of the PartMaterial.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartMaterial(Guid partMaterialId);

	/// <summary>
	/// Processes the request to delete a PartMaterial record.
	/// </summary>
	/// <param name="partMaterialId">The Unique Id of the PartMaterial.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartMaterialDto>> Process_DeletePartMaterial(Guid partMaterialId);
}
