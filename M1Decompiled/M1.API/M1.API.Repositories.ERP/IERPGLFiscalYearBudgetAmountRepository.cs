using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPGLFiscalYearBudgetAmountRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a GLFiscalYearBudgetAmount with the specified Unique Id exists.
	/// </summary>
	/// <param name="gLFiscalYearBudgetAmountId">The Unique Id of the GLFiscalYearBudgetAmount to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the GLFiscalYearBudgetAmount exists or not.</returns>
	Task<bool> DoesGLFiscalYearBudgetAmountExist(Guid gLFiscalYearBudgetAmountId);

	/// <summary>
	/// Retrieves all GLFiscalYearBudgetAmounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearBudgetAmounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLFiscalYearBudgetAmounts DTOs.</returns>
	Task<ICollection<ERPGLFiscalYearBudgetAmountInformationDto>> GetAllGLFiscalYearBudgetAmounts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific GLFiscalYearBudgetAmount.
	/// </summary>
	/// <param name="gLFiscalYearBudgetAmountId">The Unique Id of the GLFiscalYearBudgetAmount to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the GLFiscalYearBudgetAmount DTO.</returns>
	Task<ERPGLFiscalYearBudgetAmountInformationDto> GetGLFiscalYearBudgetAmount(Guid gLFiscalYearBudgetAmountId);

	/// <summary>
	/// Saves the provided ERP gLFiscalYearBudgetAmount.
	/// </summary>
	/// <param name="gLFiscalYearBudgetAmount">The ERP gLFiscalYearBudgetAmount to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveGLFiscalYearBudgetAmount(ERPGLFiscalYearBudgetAmountDto gLFiscalYearBudgetAmount);
}
