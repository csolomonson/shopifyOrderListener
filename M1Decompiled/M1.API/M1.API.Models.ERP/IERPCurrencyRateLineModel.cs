using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPCurrencyRateLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all CurrencyRateLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CurrencyRateLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllCurrencyRateLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving CurrencyRateLine information based on the specified CurrencyRateLine Unique Id.
	/// </summary>
	/// <param name="currencyRateLineId">The Unique Id of the CurrencyRateLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetCurrencyRateLine(Guid currencyRateLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating CurrencyRateLine information based on the specified CurrencyRateLine.
	/// </summary>
	/// <param name="currencyRateLine">The CurrencyRateLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutCurrencyRateLine(ERPCurrencyRateLineDto currencyRateLine);

	/// <summary>
	/// Processes the request to retrieve all CurrencyRateLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CurrencyRateLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CurrencyRateLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPCurrencyRateLineDto>>> Process_GetAllCurrencyRateLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific CurrencyRateLine.
	/// </summary>
	/// <param name="currencyRateLineId">The Unique Id of the CurrencyRateLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the CurrencyRateLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPCurrencyRateLineDto>> Process_GetCurrencyRateLine(Guid currencyRateLineId);

	/// <summary>
	/// Processes the creating or updating of a CurrencyRateLine record.
	/// </summary>
	/// <param name="currencyRateLine">The CurrencyRateLine data transfer object (DTO) containing the details of the CurrencyRateLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the CurrencyRateLine details.</returns>
	Task<ERPResponseMessageDto<ERPCurrencyRateLineDto>> Process_PutCurrencyRateLine(ERPCurrencyRateLineDto currencyRateLine);

	/// <summary>
	/// Validates the request for deleting a CurrencyRateLine record.
	/// </summary>
	/// <param name="currencyRateLineId">The Unique Id of the CurrencyRateLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteCurrencyRateLine(Guid currencyRateLineId);

	/// <summary>
	/// Processes the request to delete a CurrencyRateLine record.
	/// </summary>
	/// <param name="currencyRateLineId">The Unique Id of the CurrencyRateLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPCurrencyRateLineDto>> Process_DeleteCurrencyRateLine(Guid currencyRateLineId);
}
