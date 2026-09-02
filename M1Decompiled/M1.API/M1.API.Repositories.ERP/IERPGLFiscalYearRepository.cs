using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPGLFiscalYearRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a GLFiscalYear with the specified Unique Id exists.
	/// </summary>
	/// <param name="gLFiscalYearId">The Unique Id of the GLFiscalYear to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the GLFiscalYear exists or not.</returns>
	Task<bool> DoesGLFiscalYearExist(Guid gLFiscalYearId);

	/// <summary>
	/// Retrieves all GLFiscalYears with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYears to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLFiscalYears DTOs.</returns>
	Task<ICollection<ERPGLFiscalYearInformationDto>> GetAllGLFiscalYears(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific GLFiscalYear.
	/// </summary>
	/// <param name="gLFiscalYearId">The Unique Id of the GLFiscalYear to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the GLFiscalYear DTO.</returns>
	Task<ERPGLFiscalYearInformationDto> GetGLFiscalYear(Guid gLFiscalYearId);

	/// <summary>
	/// Saves the provided ERP gLFiscalYear.
	/// </summary>
	/// <param name="gLFiscalYear">The ERP gLFiscalYear to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveGLFiscalYear(ERPGLFiscalYearDto gLFiscalYear);
}
