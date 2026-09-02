using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPShipmentPackageDetailRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ShipmentPackageDetail with the specified Unique Id exists.
	/// </summary>
	/// <param name="shipmentPackageDetailId">The Unique Id of the ShipmentPackageDetail to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ShipmentPackageDetail exists or not.</returns>
	Task<bool> DoesShipmentPackageDetailExist(Guid shipmentPackageDetailId);

	/// <summary>
	/// Retrieves all ShipmentPackageDetails with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentPackageDetails to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShipmentPackageDetails DTOs.</returns>
	Task<ICollection<ERPShipmentPackageDetailInformationDto>> GetAllShipmentPackageDetails(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ShipmentPackageDetail.
	/// </summary>
	/// <param name="shipmentPackageDetailId">The Unique Id of the ShipmentPackageDetail to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ShipmentPackageDetail DTO.</returns>
	Task<ERPShipmentPackageDetailInformationDto> GetShipmentPackageDetail(Guid shipmentPackageDetailId);

	/// <summary>
	/// Saves the provided ERP shipmentPackageDetail.
	/// </summary>
	/// <param name="shipmentPackageDetail">The ERP shipmentPackageDetail to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveShipmentPackageDetail(ERPShipmentPackageDetailDto shipmentPackageDetail);
}
