using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPGLFiscalYearOpeningBalanceRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a GLFiscalYearOpeningBalance with the specified Unique Id exists.
	/// </summary>
	/// <param name="gLFiscalYearOpeningBalanceId">The Unique Id of the GLFiscalYearOpeningBalance to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the GLFiscalYearOpeningBalance exists or not.</returns>
	Task<bool> DoesGLFiscalYearOpeningBalanceExist(Guid gLFiscalYearOpeningBalanceId);

	/// <summary>
	/// Retrieves all GLFiscalYearOpeningBalances with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearOpeningBalances to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLFiscalYearOpeningBalances DTOs.</returns>
	Task<ICollection<ERPGLFiscalYearOpeningBalanceInformationDto>> GetAllGLFiscalYearOpeningBalances(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific GLFiscalYearOpeningBalance.
	/// </summary>
	/// <param name="gLFiscalYearOpeningBalanceId">The Unique Id of the GLFiscalYearOpeningBalance to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the GLFiscalYearOpeningBalance DTO.</returns>
	Task<ERPGLFiscalYearOpeningBalanceInformationDto> GetGLFiscalYearOpeningBalance(Guid gLFiscalYearOpeningBalanceId);
}
