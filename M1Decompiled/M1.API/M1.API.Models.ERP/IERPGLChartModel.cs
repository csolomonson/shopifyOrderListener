using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPGLChartModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all GLCharts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLCharts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllGLCharts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving GLChart information based on the specified GLChart Unique Id.
	/// </summary>
	/// <param name="gLChartId">The Unique Id of the GLChart.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetGLChart(Guid gLChartId);

	/// <summary>
	/// Validates the PUT request for creating or updating GLChart information based on the specified GLChart.
	/// </summary>
	/// <param name="gLChart">The GLChart details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutGLChart(ERPGLChartDto gLChart);

	/// <summary>
	/// Processes the request to retrieve all GLCharts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLCharts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLCharts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPGLChartDto>>> Process_GetAllGLCharts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific GLChart.
	/// </summary>
	/// <param name="gLChartId">The Unique Id of the GLChart to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the GLChart DTO.</returns>
	Task<ERPResponseMessageDto<ERPGLChartDto>> Process_GetGLChart(Guid gLChartId);

	/// <summary>
	/// Processes the creating or updating of a GLChart record.
	/// </summary>
	/// <param name="gLChart">The GLChart data transfer object (DTO) containing the details of the GLChart to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the GLChart details.</returns>
	Task<ERPResponseMessageDto<ERPGLChartDto>> Process_PutGLChart(ERPGLChartDto gLChart);

	/// <summary>
	/// Validates the request for deleting a GLChart record.
	/// </summary>
	/// <param name="gLChartId">The Unique Id of the GLChart.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteGLChart(Guid gLChartId);

	/// <summary>
	/// Processes the request to delete a GLChart record.
	/// </summary>
	/// <param name="gLChartId">The Unique Id of the GLChart.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPGLChartDto>> Process_DeleteGLChart(Guid gLChartId);
}
