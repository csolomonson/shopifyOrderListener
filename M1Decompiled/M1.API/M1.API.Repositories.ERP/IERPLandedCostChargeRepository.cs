using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPLandedCostChargeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a LandedCostCharge with the specified Unique Id exists.
	/// </summary>
	/// <param name="landedCostChargeId">The Unique Id of the LandedCostCharge to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the LandedCostCharge exists or not.</returns>
	Task<bool> DoesLandedCostChargeExist(Guid landedCostChargeId);

	/// <summary>
	/// Retrieves all LandedCostCharges with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LandedCostCharges to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LandedCostCharges DTOs.</returns>
	Task<ICollection<ERPLandedCostChargeInformationDto>> GetAllLandedCostCharges(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific LandedCostCharge.
	/// </summary>
	/// <param name="landedCostChargeId">The Unique Id of the LandedCostCharge to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the LandedCostCharge DTO.</returns>
	Task<ERPLandedCostChargeInformationDto> GetLandedCostCharge(Guid landedCostChargeId);

	/// <summary>
	/// Saves the provided ERP landedCostCharge.
	/// </summary>
	/// <param name="landedCostCharge">The ERP landedCostCharge to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveLandedCostCharge(ERPLandedCostChargeDto landedCostCharge);
}
