using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartUnitSalePriceModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartUnitSalePrices with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartUnitSalePrices to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartUnitSalePrices(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartUnitSalePrice information based on the specified PartUnitSalePrice Unique Id.
	/// </summary>
	/// <param name="partUnitSalePriceId">The Unique Id of the PartUnitSalePrice.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartUnitSalePrice(Guid partUnitSalePriceId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartUnitSalePrice information based on the specified PartUnitSalePrice.
	/// </summary>
	/// <param name="partUnitSalePrice">The PartUnitSalePrice details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartUnitSalePrice(ERPPartUnitSalePriceDto partUnitSalePrice);

	/// <summary>
	/// Processes the request to retrieve all PartUnitSalePrices with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartUnitSalePrices to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartUnitSalePrices DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartUnitSalePriceDto>>> Process_GetAllPartUnitSalePrices(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartUnitSalePrice.
	/// </summary>
	/// <param name="partUnitSalePriceId">The Unique Id of the PartUnitSalePrice to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartUnitSalePrice DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartUnitSalePriceDto>> Process_GetPartUnitSalePrice(Guid partUnitSalePriceId);

	/// <summary>
	/// Processes the creating or updating of a PartUnitSalePrice record.
	/// </summary>
	/// <param name="partUnitSalePrice">The PartUnitSalePrice data transfer object (DTO) containing the details of the PartUnitSalePrice to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartUnitSalePrice details.</returns>
	Task<ERPResponseMessageDto<ERPPartUnitSalePriceDto>> Process_PutPartUnitSalePrice(ERPPartUnitSalePriceDto partUnitSalePrice);

	/// <summary>
	/// Validates the request for deleting a PartUnitSalePrice record.
	/// </summary>
	/// <param name="partUnitSalePriceId">The Unique Id of the PartUnitSalePrice.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartUnitSalePrice(Guid partUnitSalePriceId);

	/// <summary>
	/// Processes the request to delete a PartUnitSalePrice record.
	/// </summary>
	/// <param name="partUnitSalePriceId">The Unique Id of the PartUnitSalePrice.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartUnitSalePriceDto>> Process_DeletePartUnitSalePrice(Guid partUnitSalePriceId);
}
