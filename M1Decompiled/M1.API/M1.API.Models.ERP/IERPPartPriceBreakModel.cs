using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartPriceBreakModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartPriceBreaks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartPriceBreaks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartPriceBreaks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartPriceBreak information based on the specified PartPriceBreak Unique Id.
	/// </summary>
	/// <param name="partPriceBreakId">The Unique Id of the PartPriceBreak.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartPriceBreak(Guid partPriceBreakId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartPriceBreak information based on the specified PartPriceBreak.
	/// </summary>
	/// <param name="partPriceBreak">The PartPriceBreak details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartPriceBreak(ERPPartPriceBreakDto partPriceBreak);

	/// <summary>
	/// Processes the request to retrieve all PartPriceBreaks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartPriceBreaks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartPriceBreaks DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartPriceBreakDto>>> Process_GetAllPartPriceBreaks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartPriceBreak.
	/// </summary>
	/// <param name="partPriceBreakId">The Unique Id of the PartPriceBreak to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartPriceBreak DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartPriceBreakDto>> Process_GetPartPriceBreak(Guid partPriceBreakId);

	/// <summary>
	/// Processes the creating or updating of a PartPriceBreak record.
	/// </summary>
	/// <param name="partPriceBreak">The PartPriceBreak data transfer object (DTO) containing the details of the PartPriceBreak to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartPriceBreak details.</returns>
	Task<ERPResponseMessageDto<ERPPartPriceBreakDto>> Process_PutPartPriceBreak(ERPPartPriceBreakDto partPriceBreak);

	/// <summary>
	/// Validates the request for deleting a PartPriceBreak record.
	/// </summary>
	/// <param name="partPriceBreakId">The Unique Id of the PartPriceBreak.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartPriceBreak(Guid partPriceBreakId);

	/// <summary>
	/// Processes the request to delete a PartPriceBreak record.
	/// </summary>
	/// <param name="partPriceBreakId">The Unique Id of the PartPriceBreak.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartPriceBreakDto>> Process_DeletePartPriceBreak(Guid partPriceBreakId);
}
