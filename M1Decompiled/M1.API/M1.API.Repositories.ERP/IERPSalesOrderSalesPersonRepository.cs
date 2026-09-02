using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSalesOrderSalesPersonRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SalesOrderSalesPerson with the specified Unique Id exists.
	/// </summary>
	/// <param name="salesOrderSalesPersonId">The Unique Id of the SalesOrderSalesPerson to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SalesOrderSalesPerson exists or not.</returns>
	Task<bool> DoesSalesOrderSalesPersonExist(Guid salesOrderSalesPersonId);

	/// <summary>
	/// Retrieves all SalesOrderSalesPeople with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderSalesPeople to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderSalesPeople DTOs.</returns>
	Task<ICollection<ERPSalesOrderSalesPersonInformationDto>> GetAllSalesOrderSalesPeople(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific SalesOrderSalesPerson.
	/// </summary>
	/// <param name="salesOrderSalesPersonId">The Unique Id of the SalesOrderSalesPerson to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SalesOrderSalesPerson DTO.</returns>
	Task<ERPSalesOrderSalesPersonInformationDto> GetSalesOrderSalesPerson(Guid salesOrderSalesPersonId);

	/// <summary>
	/// Saves the provided ERP salesOrderSalesPerson.
	/// </summary>
	/// <param name="salesOrderSalesPerson">The ERP salesOrderSalesPerson to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveSalesOrderSalesPerson(ERPSalesOrderSalesPersonDto salesOrderSalesPerson);
}
