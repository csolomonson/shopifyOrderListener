using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPGLFiscalYearPeriodMovementRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a GLFiscalYearPeriodMovement with the specified Unique Id exists.
	/// </summary>
	/// <param name="gLFiscalYearPeriodMovementId">The Unique Id of the GLFiscalYearPeriodMovement to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the GLFiscalYearPeriodMovement exists or not.</returns>
	Task<bool> DoesGLFiscalYearPeriodMovementExist(Guid gLFiscalYearPeriodMovementId);

	/// <summary>
	/// Retrieves all GLFiscalYearPeriodMovements with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLFiscalYearPeriodMovements to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLFiscalYearPeriodMovements DTOs.</returns>
	Task<ICollection<ERPGLFiscalYearPeriodMovementInformationDto>> GetAllGLFiscalYearPeriodMovements(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific GLFiscalYearPeriodMovement.
	/// </summary>
	/// <param name="gLFiscalYearPeriodMovementId">The Unique Id of the GLFiscalYearPeriodMovement to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the GLFiscalYearPeriodMovement DTO.</returns>
	Task<ERPGLFiscalYearPeriodMovementInformationDto> GetGLFiscalYearPeriodMovement(Guid gLFiscalYearPeriodMovementId);
}
