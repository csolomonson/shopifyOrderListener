using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPGLCategoryRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a GLCategory with the specified Unique Id exists.
	/// </summary>
	/// <param name="gLCategoryId">The Unique Id of the GLCategory to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the GLCategory exists or not.</returns>
	Task<bool> DoesGLCategoryExist(Guid gLCategoryId);

	/// <summary>
	/// Retrieves all GLCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLCategories DTOs.</returns>
	Task<ICollection<ERPGLCategoryInformationDto>> GetAllGLCategories(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific GLCategory.
	/// </summary>
	/// <param name="gLCategoryId">The Unique Id of the GLCategory to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the GLCategory DTO.</returns>
	Task<ERPGLCategoryInformationDto> GetGLCategory(Guid gLCategoryId);

	/// <summary>
	/// Saves the provided ERP gLCategory.
	/// </summary>
	/// <param name="gLCategory">The ERP gLCategory to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveGLCategory(ERPGLCategoryDto gLCategory);
}
