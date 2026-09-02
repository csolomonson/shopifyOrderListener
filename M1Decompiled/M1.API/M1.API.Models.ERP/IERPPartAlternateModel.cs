using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartAlternateModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartAlternates with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartAlternates to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartAlternates(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartAlternate information based on the specified PartAlternate Unique Id.
	/// </summary>
	/// <param name="partAlternateId">The Unique Id of the PartAlternate.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartAlternate(Guid partAlternateId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartAlternate information based on the specified PartAlternate.
	/// </summary>
	/// <param name="partAlternate">The PartAlternate details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartAlternate(ERPPartAlternateDto partAlternate);

	/// <summary>
	/// Processes the request to retrieve all PartAlternates with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartAlternates to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartAlternates DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartAlternateDto>>> Process_GetAllPartAlternates(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartAlternate.
	/// </summary>
	/// <param name="partAlternateId">The Unique Id of the PartAlternate to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartAlternate DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartAlternateDto>> Process_GetPartAlternate(Guid partAlternateId);

	/// <summary>
	/// Processes the creating or updating of a PartAlternate record.
	/// </summary>
	/// <param name="partAlternate">The PartAlternate data transfer object (DTO) containing the details of the PartAlternate to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartAlternate details.</returns>
	Task<ERPResponseMessageDto<ERPPartAlternateDto>> Process_PutPartAlternate(ERPPartAlternateDto partAlternate);

	/// <summary>
	/// Validates the request for deleting a PartAlternate record.
	/// </summary>
	/// <param name="partAlternateId">The Unique Id of the PartAlternate.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartAlternate(Guid partAlternateId);

	/// <summary>
	/// Processes the request to delete a PartAlternate record.
	/// </summary>
	/// <param name="partAlternateId">The Unique Id of the PartAlternate.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartAlternateDto>> Process_DeletePartAlternate(Guid partAlternateId);
}
