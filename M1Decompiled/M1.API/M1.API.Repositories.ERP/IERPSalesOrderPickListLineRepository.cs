using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSalesOrderPickListLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SalesOrderPickListLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="salesOrderPickListLineId">The Unique Id of the SalesOrderPickListLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SalesOrderPickListLine exists or not.</returns>
	Task<bool> DoesSalesOrderPickListLineExist(Guid salesOrderPickListLineId);

	/// <summary>
	/// Retrieves all SalesOrderPickListLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderPickListLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderPickListLines DTOs.</returns>
	Task<ICollection<ERPSalesOrderPickListLineInformationDto>> GetAllSalesOrderPickListLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific SalesOrderPickListLine.
	/// </summary>
	/// <param name="salesOrderPickListLineId">The Unique Id of the SalesOrderPickListLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SalesOrderPickListLine DTO.</returns>
	Task<ERPSalesOrderPickListLineInformationDto> GetSalesOrderPickListLine(Guid salesOrderPickListLineId);

	/// <summary>
	/// Saves the provided ERP salesOrderPickListLine.
	/// </summary>
	/// <param name="salesOrderPickListLine">The ERP salesOrderPickListLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveSalesOrderPickListLine(ERPSalesOrderPickListLineDto salesOrderPickListLine);
}
