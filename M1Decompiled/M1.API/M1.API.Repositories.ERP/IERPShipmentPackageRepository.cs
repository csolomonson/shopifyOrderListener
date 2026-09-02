using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPShipmentPackageRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ShipmentPackage with the specified Unique Id exists.
	/// </summary>
	/// <param name="shipmentPackageId">The Unique Id of the ShipmentPackage to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ShipmentPackage exists or not.</returns>
	Task<bool> DoesShipmentPackageExist(Guid shipmentPackageId);

	/// <summary>
	/// Retrieves all ShipmentPackages with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentPackages to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShipmentPackages DTOs.</returns>
	Task<ICollection<ERPShipmentPackageInformationDto>> GetAllShipmentPackages(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ShipmentPackage.
	/// </summary>
	/// <param name="shipmentPackageId">The Unique Id of the ShipmentPackage to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ShipmentPackage DTO.</returns>
	Task<ERPShipmentPackageInformationDto> GetShipmentPackage(Guid shipmentPackageId);

	/// <summary>
	/// Saves the provided ERP shipmentPackage.
	/// </summary>
	/// <param name="shipmentPackage">The ERP shipmentPackage to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveShipmentPackage(ERPShipmentPackageDto shipmentPackage);
}
