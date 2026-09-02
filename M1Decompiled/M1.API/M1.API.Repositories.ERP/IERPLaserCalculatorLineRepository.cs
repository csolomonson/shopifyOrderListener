using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPLaserCalculatorLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a LaserCalculatorLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="laserCalculatorLineId">The Unique Id of the LaserCalculatorLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the LaserCalculatorLine exists or not.</returns>
	Task<bool> DoesLaserCalculatorLineExist(Guid laserCalculatorLineId);

	/// <summary>
	/// Retrieves all LaserCalculatorLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LaserCalculatorLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LaserCalculatorLines DTOs.</returns>
	Task<ICollection<ERPLaserCalculatorLineInformationDto>> GetAllLaserCalculatorLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific LaserCalculatorLine.
	/// </summary>
	/// <param name="laserCalculatorLineId">The Unique Id of the LaserCalculatorLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the LaserCalculatorLine DTO.</returns>
	Task<ERPLaserCalculatorLineInformationDto> GetLaserCalculatorLine(Guid laserCalculatorLineId);

	/// <summary>
	/// Saves the provided ERP laserCalculatorLine.
	/// </summary>
	/// <param name="laserCalculatorLine">The ERP laserCalculatorLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveLaserCalculatorLine(ERPLaserCalculatorLineDto laserCalculatorLine);
}
