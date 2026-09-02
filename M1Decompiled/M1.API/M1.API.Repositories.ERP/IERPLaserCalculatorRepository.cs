using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPLaserCalculatorRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a LaserCalculator with the specified Unique Id exists.
	/// </summary>
	/// <param name="laserCalculatorId">The Unique Id of the LaserCalculator to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the LaserCalculator exists or not.</returns>
	Task<bool> DoesLaserCalculatorExist(Guid laserCalculatorId);

	/// <summary>
	/// Retrieves all LaserCalculators with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LaserCalculators to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LaserCalculators DTOs.</returns>
	Task<ICollection<ERPLaserCalculatorInformationDto>> GetAllLaserCalculators(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific LaserCalculator.
	/// </summary>
	/// <param name="laserCalculatorId">The Unique Id of the LaserCalculator to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the LaserCalculator DTO.</returns>
	Task<ERPLaserCalculatorInformationDto> GetLaserCalculator(Guid laserCalculatorId);

	/// <summary>
	/// Saves the provided ERP laserCalculator.
	/// </summary>
	/// <param name="laserCalculator">The ERP laserCalculator to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveLaserCalculator(ERPLaserCalculatorDto laserCalculator);
}
