using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartOrgReferenceModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartOrgReferences with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartOrgReferences to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartOrgReferences(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartOrgReference information based on the specified PartOrgReference Unique Id.
	/// </summary>
	/// <param name="partOrgReferenceId">The Unique Id of the PartOrgReference.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartOrgReference(Guid partOrgReferenceId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartOrgReference information based on the specified PartOrgReference.
	/// </summary>
	/// <param name="partOrgReference">The PartOrgReference details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartOrgReference(ERPPartOrgReferenceDto partOrgReference);

	/// <summary>
	/// Processes the request to retrieve all PartOrgReferences with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartOrgReferences to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartOrgReferences DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartOrgReferenceDto>>> Process_GetAllPartOrgReferences(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartOrgReference.
	/// </summary>
	/// <param name="partOrgReferenceId">The Unique Id of the PartOrgReference to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartOrgReference DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartOrgReferenceDto>> Process_GetPartOrgReference(Guid partOrgReferenceId);

	/// <summary>
	/// Processes the creating or updating of a PartOrgReference record.
	/// </summary>
	/// <param name="partOrgReference">The PartOrgReference data transfer object (DTO) containing the details of the PartOrgReference to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartOrgReference details.</returns>
	Task<ERPResponseMessageDto<ERPPartOrgReferenceDto>> Process_PutPartOrgReference(ERPPartOrgReferenceDto partOrgReference);

	/// <summary>
	/// Validates the request for deleting a PartOrgReference record.
	/// </summary>
	/// <param name="partOrgReferenceId">The Unique Id of the PartOrgReference.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartOrgReference(Guid partOrgReferenceId);

	/// <summary>
	/// Processes the request to delete a PartOrgReference record.
	/// </summary>
	/// <param name="partOrgReferenceId">The Unique Id of the PartOrgReference.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartOrgReferenceDto>> Process_DeletePartOrgReference(Guid partOrgReferenceId);
}
