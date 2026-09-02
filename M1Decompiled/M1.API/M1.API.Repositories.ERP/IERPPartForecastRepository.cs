using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartForecastRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartForecast with the specified Unique Id exists.
	/// </summary>
	/// <param name="partForecastId">The Unique Id of the PartForecast to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartForecast exists or not.</returns>
	Task<bool> DoesPartForecastExist(Guid partForecastId);

	/// <summary>
	/// Retrieves all PartForecasts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartForecasts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartForecasts DTOs.</returns>
	Task<ICollection<ERPPartForecastInformationDto>> GetAllPartForecasts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartForecast.
	/// </summary>
	/// <param name="partForecastId">The Unique Id of the PartForecast to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartForecast DTO.</returns>
	Task<ERPPartForecastInformationDto> GetPartForecast(Guid partForecastId);

	/// <summary>
	/// Saves the provided ERP partForecast.
	/// </summary>
	/// <param name="partForecast">The ERP partForecast to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartForecast(ERPPartForecastDto partForecast);
}
