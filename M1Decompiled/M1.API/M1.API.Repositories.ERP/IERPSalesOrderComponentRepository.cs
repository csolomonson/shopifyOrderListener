using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSalesOrderComponentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SalesOrderComponent with the specified Unique Id exists.
	/// </summary>
	/// <param name="salesOrderComponentId">The Unique Id of the SalesOrderComponent to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SalesOrderComponent exists or not.</returns>
	Task<bool> DoesSalesOrderComponentExist(Guid salesOrderComponentId);

	/// <summary>
	/// Retrieves all SalesOrderComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderComponents DTOs.</returns>
	Task<ICollection<ERPSalesOrderComponentInformationDto>> GetAllSalesOrderComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific SalesOrderComponent.
	/// </summary>
	/// <param name="salesOrderComponentId">The Unique Id of the SalesOrderComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SalesOrderComponent DTO.</returns>
	Task<ERPSalesOrderComponentInformationDto> GetSalesOrderComponent(Guid salesOrderComponentId);

	/// <summary>
	/// Saves the provided ERP salesOrderComponent.
	/// </summary>
	/// <param name="salesOrderComponent">The ERP salesOrderComponent to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveSalesOrderComponent(ERPSalesOrderComponentDto salesOrderComponent);
}
