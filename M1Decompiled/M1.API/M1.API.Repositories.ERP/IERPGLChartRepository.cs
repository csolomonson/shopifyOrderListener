using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPGLChartRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a GLChart with the specified Unique Id exists.
	/// </summary>
	/// <param name="gLChartId">The Unique Id of the GLChart to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the GLChart exists or not.</returns>
	Task<bool> DoesGLChartExist(Guid gLChartId);

	/// <summary>
	/// Retrieves all GLCharts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLCharts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLCharts DTOs.</returns>
	Task<ICollection<ERPGLChartInformationDto>> GetAllGLCharts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific GLChart.
	/// </summary>
	/// <param name="gLChartId">The Unique Id of the GLChart to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the GLChart DTO.</returns>
	Task<ERPGLChartInformationDto> GetGLChart(Guid gLChartId);

	/// <summary>
	/// Saves the provided ERP gLChart.
	/// </summary>
	/// <param name="gLChart">The ERP gLChart to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveGLChart(ERPGLChartDto gLChart);
}
