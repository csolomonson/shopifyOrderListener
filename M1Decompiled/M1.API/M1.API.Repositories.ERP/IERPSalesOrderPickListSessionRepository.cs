using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSalesOrderPickListSessionRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SalesOrderPickListSession with the specified Unique Id exists.
	/// </summary>
	/// <param name="salesOrderPickListSessionId">The Unique Id of the SalesOrderPickListSession to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SalesOrderPickListSession exists or not.</returns>
	Task<bool> DoesSalesOrderPickListSessionExist(Guid salesOrderPickListSessionId);

	/// <summary>
	/// Retrieves all SalesOrderPickListSessions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderPickListSessions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderPickListSessions DTOs.</returns>
	Task<ICollection<ERPSalesOrderPickListSessionInformationDto>> GetAllSalesOrderPickListSessions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific SalesOrderPickListSession.
	/// </summary>
	/// <param name="salesOrderPickListSessionId">The Unique Id of the SalesOrderPickListSession to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SalesOrderPickListSession DTO.</returns>
	Task<ERPSalesOrderPickListSessionInformationDto> GetSalesOrderPickListSession(Guid salesOrderPickListSessionId);

	/// <summary>
	/// Saves the provided ERP salesOrderPickListSession.
	/// </summary>
	/// <param name="salesOrderPickListSession">The ERP salesOrderPickListSession to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveSalesOrderPickListSession(ERPSalesOrderPickListSessionDto salesOrderPickListSession);
}
