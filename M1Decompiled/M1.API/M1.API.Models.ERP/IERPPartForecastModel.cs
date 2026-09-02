using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartForecastModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartForecasts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartForecasts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartForecasts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartForecast information based on the specified PartForecast Unique Id.
	/// </summary>
	/// <param name="partForecastId">The Unique Id of the PartForecast.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartForecast(Guid partForecastId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartForecast information based on the specified PartForecast.
	/// </summary>
	/// <param name="partForecast">The PartForecast details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartForecast(ERPPartForecastDto partForecast);

	/// <summary>
	/// Processes the request to retrieve all PartForecasts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartForecasts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartForecasts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartForecastDto>>> Process_GetAllPartForecasts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartForecast.
	/// </summary>
	/// <param name="partForecastId">The Unique Id of the PartForecast to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartForecast DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartForecastDto>> Process_GetPartForecast(Guid partForecastId);

	/// <summary>
	/// Processes the creating or updating of a PartForecast record.
	/// </summary>
	/// <param name="partForecast">The PartForecast data transfer object (DTO) containing the details of the PartForecast to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartForecast details.</returns>
	Task<ERPResponseMessageDto<ERPPartForecastDto>> Process_PutPartForecast(ERPPartForecastDto partForecast);

	/// <summary>
	/// Validates the request for deleting a PartForecast record.
	/// </summary>
	/// <param name="partForecastId">The Unique Id of the PartForecast.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartForecast(Guid partForecastId);

	/// <summary>
	/// Processes the request to delete a PartForecast record.
	/// </summary>
	/// <param name="partForecastId">The Unique Id of the PartForecast.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartForecastDto>> Process_DeletePartForecast(Guid partForecastId);
}
