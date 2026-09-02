using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartPriceModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartPrices with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartPrices to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartPrices(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartPrice information based on the specified PartPrice Unique Id.
	/// </summary>
	/// <param name="partPriceId">The Unique Id of the PartPrice.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartPrice(Guid partPriceId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartPrice information based on the specified PartPrice.
	/// </summary>
	/// <param name="partPrice">The PartPrice details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartPrice(ERPPartPriceDto partPrice);

	/// <summary>
	/// Processes the request to retrieve all PartPrices with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartPrices to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartPrices DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartPriceDto>>> Process_GetAllPartPrices(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartPrice.
	/// </summary>
	/// <param name="partPriceId">The Unique Id of the PartPrice to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartPrice DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartPriceDto>> Process_GetPartPrice(Guid partPriceId);

	/// <summary>
	/// Processes the creating or updating of a PartPrice record.
	/// </summary>
	/// <param name="partPrice">The PartPrice data transfer object (DTO) containing the details of the PartPrice to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartPrice details.</returns>
	Task<ERPResponseMessageDto<ERPPartPriceDto>> Process_PutPartPrice(ERPPartPriceDto partPrice);

	/// <summary>
	/// Validates the request for deleting a PartPrice record.
	/// </summary>
	/// <param name="partPriceId">The Unique Id of the PartPrice.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartPrice(Guid partPriceId);

	/// <summary>
	/// Processes the request to delete a PartPrice record.
	/// </summary>
	/// <param name="partPriceId">The Unique Id of the PartPrice.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartPriceDto>> Process_DeletePartPrice(Guid partPriceId);
}
