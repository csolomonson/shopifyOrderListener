using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPNonConformanceCategoryRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a NonConformanceCategory with the specified Unique Id exists.
	/// </summary>
	/// <param name="nonConformanceCategoryId">The Unique Id of the NonConformanceCategory to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the NonConformanceCategory exists or not.</returns>
	Task<bool> DoesNonConformanceCategoryExist(Guid nonConformanceCategoryId);

	/// <summary>
	/// Retrieves all NonConformanceCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of NonConformanceCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of NonConformanceCategories DTOs.</returns>
	Task<ICollection<ERPNonConformanceCategoryInformationDto>> GetAllNonConformanceCategories(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific NonConformanceCategory.
	/// </summary>
	/// <param name="nonConformanceCategoryId">The Unique Id of the NonConformanceCategory to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the NonConformanceCategory DTO.</returns>
	Task<ERPNonConformanceCategoryInformationDto> GetNonConformanceCategory(Guid nonConformanceCategoryId);

	/// <summary>
	/// Saves the provided ERP nonConformanceCategory.
	/// </summary>
	/// <param name="nonConformanceCategory">The ERP nonConformanceCategory to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveNonConformanceCategory(ERPNonConformanceCategoryDto nonConformanceCategory);
}
