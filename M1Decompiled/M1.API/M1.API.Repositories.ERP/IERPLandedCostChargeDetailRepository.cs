using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPLandedCostChargeDetailRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a LandedCostChargeDetail with the specified Unique Id exists.
	/// </summary>
	/// <param name="landedCostChargeDetailId">The Unique Id of the LandedCostChargeDetail to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the LandedCostChargeDetail exists or not.</returns>
	Task<bool> DoesLandedCostChargeDetailExist(Guid landedCostChargeDetailId);

	/// <summary>
	/// Retrieves all LandedCostChargeDetails with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LandedCostChargeDetails to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LandedCostChargeDetails DTOs.</returns>
	Task<ICollection<ERPLandedCostChargeDetailInformationDto>> GetAllLandedCostChargeDetails(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific LandedCostChargeDetail.
	/// </summary>
	/// <param name="landedCostChargeDetailId">The Unique Id of the LandedCostChargeDetail to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the LandedCostChargeDetail DTO.</returns>
	Task<ERPLandedCostChargeDetailInformationDto> GetLandedCostChargeDetail(Guid landedCostChargeDetailId);

	/// <summary>
	/// Saves the provided ERP landedCostChargeDetail.
	/// </summary>
	/// <param name="landedCostChargeDetail">The ERP landedCostChargeDetail to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveLandedCostChargeDetail(ERPLandedCostChargeDetailDto landedCostChargeDetail);
}
