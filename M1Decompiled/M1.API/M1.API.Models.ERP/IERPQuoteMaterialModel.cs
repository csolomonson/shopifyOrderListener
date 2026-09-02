using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPQuoteMaterialModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all QuoteMaterials with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteMaterials to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllQuoteMaterials(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving QuoteMaterial information based on the specified QuoteMaterial Unique Id.
	/// </summary>
	/// <param name="quoteMaterialId">The Unique Id of the QuoteMaterial.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetQuoteMaterial(Guid quoteMaterialId);

	/// <summary>
	/// Validates the PUT request for creating or updating QuoteMaterial information based on the specified QuoteMaterial.
	/// </summary>
	/// <param name="quoteMaterial">The QuoteMaterial details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutQuoteMaterial(ERPQuoteMaterialDto quoteMaterial);

	/// <summary>
	/// Processes the request to retrieve all QuoteMaterials with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteMaterials to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of QuoteMaterials DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPQuoteMaterialDto>>> Process_GetAllQuoteMaterials(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific QuoteMaterial.
	/// </summary>
	/// <param name="quoteMaterialId">The Unique Id of the QuoteMaterial to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the QuoteMaterial DTO.</returns>
	Task<ERPResponseMessageDto<ERPQuoteMaterialDto>> Process_GetQuoteMaterial(Guid quoteMaterialId);

	/// <summary>
	/// Processes the creating or updating of a QuoteMaterial record.
	/// </summary>
	/// <param name="quoteMaterial">The QuoteMaterial data transfer object (DTO) containing the details of the QuoteMaterial to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the QuoteMaterial details.</returns>
	Task<ERPResponseMessageDto<ERPQuoteMaterialDto>> Process_PutQuoteMaterial(ERPQuoteMaterialDto quoteMaterial);

	/// <summary>
	/// Validates the request for deleting a QuoteMaterial record.
	/// </summary>
	/// <param name="quoteMaterialId">The Unique Id of the QuoteMaterial.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteQuoteMaterial(Guid quoteMaterialId);

	/// <summary>
	/// Processes the request to delete a QuoteMaterial record.
	/// </summary>
	/// <param name="quoteMaterialId">The Unique Id of the QuoteMaterial.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPQuoteMaterialDto>> Process_DeleteQuoteMaterial(Guid quoteMaterialId);
}
