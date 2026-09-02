using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSalesOrderDeliveryRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SalesOrderDelivery with the specified Unique Id exists.
	/// </summary>
	/// <param name="salesOrderDeliveryId">The Unique Id of the SalesOrderDelivery to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SalesOrderDelivery exists or not.</returns>
	Task<bool> DoesSalesOrderDeliveryExist(Guid salesOrderDeliveryId);

	/// <summary>
	/// Retrieves all SalesOrderDeliveries with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SalesOrderDeliveries to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SalesOrderDeliveries DTOs.</returns>
	Task<ICollection<ERPSalesOrderDeliveryInformationDto>> GetAllSalesOrderDeliveries(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific SalesOrderDelivery.
	/// </summary>
	/// <param name="salesOrderDeliveryId">The Unique Id of the SalesOrderDelivery to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SalesOrderDelivery DTO.</returns>
	Task<ERPSalesOrderDeliveryInformationDto> GetSalesOrderDelivery(Guid salesOrderDeliveryId);

	/// <summary>
	/// Saves the provided ERP salesOrderDelivery.
	/// </summary>
	/// <param name="salesOrderDelivery">The ERP salesOrderDelivery to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveSalesOrderDelivery(ERPSalesOrderDeliveryDto salesOrderDelivery);
}
