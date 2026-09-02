using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPLandedCostRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a LandedCost with the specified Unique Id exists.
	/// </summary>
	/// <param name="landedCostId">The Unique Id of the LandedCost to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the LandedCost exists or not.</returns>
	Task<bool> DoesLandedCostExist(Guid landedCostId);

	/// <summary>
	/// Retrieves all LandedCosts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LandedCosts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LandedCosts DTOs.</returns>
	Task<ICollection<ERPLandedCostInformationDto>> GetAllLandedCosts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific LandedCost.
	/// </summary>
	/// <param name="landedCostId">The Unique Id of the LandedCost to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the LandedCost DTO.</returns>
	Task<ERPLandedCostInformationDto> GetLandedCost(Guid landedCostId);

	/// <summary>
	/// Saves the provided ERP landedCost.
	/// </summary>
	/// <param name="landedCost">The ERP landedCost to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveLandedCost(ERPLandedCostDto landedCost);
}
