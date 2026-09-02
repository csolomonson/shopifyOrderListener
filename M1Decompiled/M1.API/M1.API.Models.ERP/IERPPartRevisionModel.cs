using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartRevisionModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartRevisions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartRevisions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartRevisions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartRevision information based on the specified PartRevision Unique Id.
	/// </summary>
	/// <param name="partRevisionId">The Unique Id of the PartRevision.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartRevision(Guid partRevisionId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartRevision information based on the specified PartRevision.
	/// </summary>
	/// <param name="partRevision">The PartRevision details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartRevision(ERPPartRevisionDto partRevision);

	/// <summary>
	/// Processes the request to retrieve all PartRevisions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartRevisions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartRevisions DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartRevisionDto>>> Process_GetAllPartRevisions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartRevision.
	/// </summary>
	/// <param name="partRevisionId">The Unique Id of the PartRevision to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartRevision DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartRevisionDto>> Process_GetPartRevision(Guid partRevisionId);

	/// <summary>
	/// Processes the creating or updating of a PartRevision record.
	/// </summary>
	/// <param name="partRevision">The PartRevision data transfer object (DTO) containing the details of the PartRevision to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartRevision details.</returns>
	Task<ERPResponseMessageDto<ERPPartRevisionDto>> Process_PutPartRevision(ERPPartRevisionDto partRevision);

	/// <summary>
	/// Validates the request for deleting a PartRevision record.
	/// </summary>
	/// <param name="partRevisionId">The Unique Id of the PartRevision.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartRevision(Guid partRevisionId);

	/// <summary>
	/// Processes the request to delete a PartRevision record.
	/// </summary>
	/// <param name="partRevisionId">The Unique Id of the PartRevision.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartRevisionDto>> Process_DeletePartRevision(Guid partRevisionId);
}
