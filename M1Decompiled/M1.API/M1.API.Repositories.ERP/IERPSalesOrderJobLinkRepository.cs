using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSalesOrderJobLinkRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SalesOrderJobLink with the specified Unique Id exists.
	/// </summary>
	/// <param name="salesOrderJobLinkId">The Unique Id of the SalesOrderJobLink to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SalesOrderJobLink exists or not.</returns>
	Task<bool> DoesSalesOrderJobLinkExist(Guid salesOrderJobLinkId);

	/// <summary>
	/// Retrieves all SalesOrderJobLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderJobLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderJobLinks DTOs.</returns>
	Task<ICollection<ERPSalesOrderJobLinkInformationDto>> GetAllSalesOrderJobLinks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific SalesOrderJobLink.
	/// </summary>
	/// <param name="salesOrderJobLinkId">The Unique Id of the SalesOrderJobLink to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SalesOrderJobLink DTO.</returns>
	Task<ERPSalesOrderJobLinkInformationDto> GetSalesOrderJobLink(Guid salesOrderJobLinkId);

	/// <summary>
	/// Saves the provided ERP salesOrderJobLink.
	/// </summary>
	/// <param name="salesOrderJobLink">The ERP salesOrderJobLink to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveSalesOrderJobLink(ERPSalesOrderJobLinkDto salesOrderJobLink);
}
