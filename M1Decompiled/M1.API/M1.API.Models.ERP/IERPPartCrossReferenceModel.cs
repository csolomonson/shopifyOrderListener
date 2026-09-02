using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartCrossReferenceModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartCrossReferences with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartCrossReferences to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartCrossReferences(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartCrossReference information based on the specified PartCrossReference Unique Id.
	/// </summary>
	/// <param name="partCrossReferenceId">The Unique Id of the PartCrossReference.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartCrossReference(Guid partCrossReferenceId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartCrossReference information based on the specified PartCrossReference.
	/// </summary>
	/// <param name="partCrossReference">The PartCrossReference details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartCrossReference(ERPPartCrossReferenceDto partCrossReference);

	/// <summary>
	/// Processes the request to retrieve all PartCrossReferences with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartCrossReferences to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartCrossReferences DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartCrossReferenceDto>>> Process_GetAllPartCrossReferences(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartCrossReference.
	/// </summary>
	/// <param name="partCrossReferenceId">The Unique Id of the PartCrossReference to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartCrossReference DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartCrossReferenceDto>> Process_GetPartCrossReference(Guid partCrossReferenceId);

	/// <summary>
	/// Processes the creating or updating of a PartCrossReference record.
	/// </summary>
	/// <param name="partCrossReference">The PartCrossReference data transfer object (DTO) containing the details of the PartCrossReference to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartCrossReference details.</returns>
	Task<ERPResponseMessageDto<ERPPartCrossReferenceDto>> Process_PutPartCrossReference(ERPPartCrossReferenceDto partCrossReference);

	/// <summary>
	/// Validates the request for deleting a PartCrossReference record.
	/// </summary>
	/// <param name="partCrossReferenceId">The Unique Id of the PartCrossReference.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartCrossReference(Guid partCrossReferenceId);

	/// <summary>
	/// Processes the request to delete a PartCrossReference record.
	/// </summary>
	/// <param name="partCrossReferenceId">The Unique Id of the PartCrossReference.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartCrossReferenceDto>> Process_DeletePartCrossReference(Guid partCrossReferenceId);
}
