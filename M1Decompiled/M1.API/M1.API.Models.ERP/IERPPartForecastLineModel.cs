using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartForecastLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartForecastLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartForecastLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartForecastLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartForecastLine information based on the specified PartForecastLine Unique Id.
	/// </summary>
	/// <param name="partForecastLineId">The Unique Id of the PartForecastLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartForecastLine(Guid partForecastLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartForecastLine information based on the specified PartForecastLine.
	/// </summary>
	/// <param name="partForecastLine">The PartForecastLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartForecastLine(ERPPartForecastLineDto partForecastLine);

	/// <summary>
	/// Processes the request to retrieve all PartForecastLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartForecastLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartForecastLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartForecastLineDto>>> Process_GetAllPartForecastLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartForecastLine.
	/// </summary>
	/// <param name="partForecastLineId">The Unique Id of the PartForecastLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartForecastLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartForecastLineDto>> Process_GetPartForecastLine(Guid partForecastLineId);

	/// <summary>
	/// Processes the creating or updating of a PartForecastLine record.
	/// </summary>
	/// <param name="partForecastLine">The PartForecastLine data transfer object (DTO) containing the details of the PartForecastLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartForecastLine details.</returns>
	Task<ERPResponseMessageDto<ERPPartForecastLineDto>> Process_PutPartForecastLine(ERPPartForecastLineDto partForecastLine);

	/// <summary>
	/// Validates the request for deleting a PartForecastLine record.
	/// </summary>
	/// <param name="partForecastLineId">The Unique Id of the PartForecastLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartForecastLine(Guid partForecastLineId);

	/// <summary>
	/// Processes the request to delete a PartForecastLine record.
	/// </summary>
	/// <param name="partForecastLineId">The Unique Id of the PartForecastLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartForecastLineDto>> Process_DeletePartForecastLine(Guid partForecastLineId);
}
