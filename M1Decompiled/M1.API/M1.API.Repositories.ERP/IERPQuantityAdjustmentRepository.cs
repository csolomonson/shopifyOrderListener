using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPQuantityAdjustmentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a QuantityAdjustment with the specified Unique Id exists.
	/// </summary>
	/// <param name="quantityAdjustmentId">The Unique Id of the QuantityAdjustment to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the QuantityAdjustment exists or not.</returns>
	Task<bool> DoesQuantityAdjustmentExist(Guid quantityAdjustmentId);

	/// <summary>
	/// Retrieves all QuantityAdjustments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuantityAdjustments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of QuantityAdjustments DTOs.</returns>
	Task<ICollection<ERPQuantityAdjustmentInformationDto>> GetAllQuantityAdjustments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific QuantityAdjustment.
	/// </summary>
	/// <param name="quantityAdjustmentId">The Unique Id of the QuantityAdjustment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the QuantityAdjustment DTO.</returns>
	Task<ERPQuantityAdjustmentInformationDto> GetQuantityAdjustment(Guid quantityAdjustmentId);

	/// <summary>
	/// Saves the provided ERP quantityAdjustment.
	/// </summary>
	/// <param name="quantityAdjustment">The ERP quantityAdjustment to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveQuantityAdjustment(ERPQuantityAdjustmentDto quantityAdjustment);
}
