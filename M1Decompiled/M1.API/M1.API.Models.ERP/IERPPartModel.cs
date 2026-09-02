using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Parts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Parts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllParts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Part information based on the specified Part Unique Id.
	/// </summary>
	/// <param name="partId">The Unique Id of the Part.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPart(Guid partId);

	/// <summary>
	/// Validates the PUT request for creating or updating Part information based on the specified Part.
	/// </summary>
	/// <param name="part">The Part details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPart(ERPPartDto part);

	/// <summary>
	/// Processes the request to retrieve all Parts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Parts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Parts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartDto>>> Process_GetAllParts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Part.
	/// </summary>
	/// <param name="partId">The Unique Id of the Part to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Part DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartDto>> Process_GetPart(Guid partId);

	/// <summary>
	/// Processes the creating or updating of a Part record.
	/// </summary>
	/// <param name="part">The Part data transfer object (DTO) containing the details of the Part to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Part details.</returns>
	Task<ERPResponseMessageDto<ERPPartDto>> Process_PutPart(ERPPartDto part);

	/// <summary>
	/// Validates the request for deleting a Part record.
	/// </summary>
	/// <param name="partId">The Unique Id of the Part.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePart(Guid partId);

	/// <summary>
	/// Processes the request to delete a Part record.
	/// </summary>
	/// <param name="partId">The Unique Id of the Part.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartDto>> Process_DeletePart(Guid partId);
}
