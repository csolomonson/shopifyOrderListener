using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPGLFiscalYearBudgetLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a GLFiscalYearBudgetLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="gLFiscalYearBudgetLineId">The Unique Id of the GLFiscalYearBudgetLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the GLFiscalYearBudgetLine exists or not.</returns>
	Task<bool> DoesGLFiscalYearBudgetLineExist(Guid gLFiscalYearBudgetLineId);

	/// <summary>
	/// Retrieves all GLFiscalYearBudgetLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearBudgetLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLFiscalYearBudgetLines DTOs.</returns>
	Task<ICollection<ERPGLFiscalYearBudgetLineInformationDto>> GetAllGLFiscalYearBudgetLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific GLFiscalYearBudgetLine.
	/// </summary>
	/// <param name="gLFiscalYearBudgetLineId">The Unique Id of the GLFiscalYearBudgetLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the GLFiscalYearBudgetLine DTO.</returns>
	Task<ERPGLFiscalYearBudgetLineInformationDto> GetGLFiscalYearBudgetLine(Guid gLFiscalYearBudgetLineId);

	/// <summary>
	/// Saves the provided ERP gLFiscalYearBudgetLine.
	/// </summary>
	/// <param name="gLFiscalYearBudgetLine">The ERP gLFiscalYearBudgetLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveGLFiscalYearBudgetLine(ERPGLFiscalYearBudgetLineDto gLFiscalYearBudgetLine);
}
