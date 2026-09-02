using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPurchasePlannerLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PurchasePlannerLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="purchasePlannerLineId">The Unique Id of the PurchasePlannerLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PurchasePlannerLine exists or not.</returns>
	Task<bool> DoesPurchasePlannerLineExist(Guid purchasePlannerLineId);

	/// <summary>
	/// Retrieves all PurchasePlannerLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchasePlannerLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchasePlannerLines DTOs.</returns>
	Task<ICollection<ERPPurchasePlannerLineInformationDto>> GetAllPurchasePlannerLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PurchasePlannerLine.
	/// </summary>
	/// <param name="purchasePlannerLineId">The Unique Id of the PurchasePlannerLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PurchasePlannerLine DTO.</returns>
	Task<ERPPurchasePlannerLineInformationDto> GetPurchasePlannerLine(Guid purchasePlannerLineId);

	/// <summary>
	/// Saves the provided ERP purchasePlannerLine.
	/// </summary>
	/// <param name="purchasePlannerLine">The ERP purchasePlannerLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePurchasePlannerLine(ERPPurchasePlannerLineDto purchasePlannerLine);
}
