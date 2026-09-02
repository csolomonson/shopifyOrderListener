using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartForecastLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartForecastLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="partForecastLineId">The Unique Id of the PartForecastLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartForecastLine exists or not.</returns>
	Task<bool> DoesPartForecastLineExist(Guid partForecastLineId);

	/// <summary>
	/// Retrieves all PartForecastLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartForecastLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartForecastLines DTOs.</returns>
	Task<ICollection<ERPPartForecastLineInformationDto>> GetAllPartForecastLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartForecastLine.
	/// </summary>
	/// <param name="partForecastLineId">The Unique Id of the PartForecastLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartForecastLine DTO.</returns>
	Task<ERPPartForecastLineInformationDto> GetPartForecastLine(Guid partForecastLineId);

	/// <summary>
	/// Saves the provided ERP partForecastLine.
	/// </summary>
	/// <param name="partForecastLine">The ERP partForecastLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartForecastLine(ERPPartForecastLineDto partForecastLine);
}
