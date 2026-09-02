using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPShipmentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Shipment with the specified Unique Id exists.
	/// </summary>
	/// <param name="shipmentId">The Unique Id of the Shipment to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Shipment exists or not.</returns>
	Task<bool> DoesShipmentExist(Guid shipmentId);

	/// <summary>
	/// Retrieves all Shipments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Shipments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Shipments DTOs.</returns>
	Task<ICollection<ERPShipmentInformationDto>> GetAllShipments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Shipment.
	/// </summary>
	/// <param name="shipmentId">The Unique Id of the Shipment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Shipment DTO.</returns>
	Task<ERPShipmentInformationDto> GetShipment(Guid shipmentId);

	/// <summary>
	/// Saves the provided ERP shipment.
	/// </summary>
	/// <param name="shipment">The ERP shipment to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveShipment(ERPShipmentDto shipment);
}
