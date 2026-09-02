using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPGLFiscalYearPeriodRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a GLFiscalYearPeriod with the specified Unique Id exists.
	/// </summary>
	/// <param name="gLFiscalYearPeriodId">The Unique Id of the GLFiscalYearPeriod to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the GLFiscalYearPeriod exists or not.</returns>
	Task<bool> DoesGLFiscalYearPeriodExist(Guid gLFiscalYearPeriodId);

	/// <summary>
	/// Retrieves all GLFiscalYearPeriods with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearPeriods to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLFiscalYearPeriods DTOs.</returns>
	Task<ICollection<ERPGLFiscalYearPeriodInformationDto>> GetAllGLFiscalYearPeriods(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific GLFiscalYearPeriod.
	/// </summary>
	/// <param name="gLFiscalYearPeriodId">The Unique Id of the GLFiscalYearPeriod to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the GLFiscalYearPeriod DTO.</returns>
	Task<ERPGLFiscalYearPeriodInformationDto> GetGLFiscalYearPeriod(Guid gLFiscalYearPeriodId);

	/// <summary>
	/// Saves the provided ERP gLFiscalYearPeriod.
	/// </summary>
	/// <param name="gLFiscalYearPeriod">The ERP gLFiscalYearPeriod to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveGLFiscalYearPeriod(ERPGLFiscalYearPeriodDto gLFiscalYearPeriod);
}
