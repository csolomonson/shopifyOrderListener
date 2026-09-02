using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPFreightReferenceModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all FreightReferences with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FreightReferences to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllFreightReferences(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving FreightReference information based on the specified FreightReference Unique Id.
	/// </summary>
	/// <param name="freightReferenceId">The Unique Id of the FreightReference.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetFreightReference(Guid freightReferenceId);

	/// <summary>
	/// Validates the PUT request for creating or updating FreightReference information based on the specified FreightReference.
	/// </summary>
	/// <param name="freightReference">The FreightReference details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutFreightReference(ERPFreightReferenceDto freightReference);

	/// <summary>
	/// Processes the request to retrieve all FreightReferences with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FreightReferences to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of FreightReferences DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPFreightReferenceDto>>> Process_GetAllFreightReferences(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific FreightReference.
	/// </summary>
	/// <param name="freightReferenceId">The Unique Id of the FreightReference to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the FreightReference DTO.</returns>
	Task<ERPResponseMessageDto<ERPFreightReferenceDto>> Process_GetFreightReference(Guid freightReferenceId);

	/// <summary>
	/// Processes the creating or updating of a FreightReference record.
	/// </summary>
	/// <param name="freightReference">The FreightReference data transfer object (DTO) containing the details of the FreightReference to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the FreightReference details.</returns>
	Task<ERPResponseMessageDto<ERPFreightReferenceDto>> Process_PutFreightReference(ERPFreightReferenceDto freightReference);

	/// <summary>
	/// Validates the request for deleting a FreightReference record.
	/// </summary>
	/// <param name="freightReferenceId">The Unique Id of the FreightReference.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteFreightReference(Guid freightReferenceId);

	/// <summary>
	/// Processes the request to delete a FreightReference record.
	/// </summary>
	/// <param name="freightReferenceId">The Unique Id of the FreightReference.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPFreightReferenceDto>> Process_DeleteFreightReference(Guid freightReferenceId);
}
