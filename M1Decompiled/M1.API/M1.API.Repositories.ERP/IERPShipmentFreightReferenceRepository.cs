using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPShipmentFreightReferenceRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ShipmentFreightReference with the specified Unique Id exists.
	/// </summary>
	/// <param name="shipmentFreightReferenceId">The Unique Id of the ShipmentFreightReference to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ShipmentFreightReference exists or not.</returns>
	Task<bool> DoesShipmentFreightReferenceExist(Guid shipmentFreightReferenceId);

	/// <summary>
	/// Retrieves all ShipmentFreightReferences with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentFreightReferences to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShipmentFreightReferences DTOs.</returns>
	Task<ICollection<ERPShipmentFreightReferenceInformationDto>> GetAllShipmentFreightReferences(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ShipmentFreightReference.
	/// </summary>
	/// <param name="shipmentFreightReferenceId">The Unique Id of the ShipmentFreightReference to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ShipmentFreightReference DTO.</returns>
	Task<ERPShipmentFreightReferenceInformationDto> GetShipmentFreightReference(Guid shipmentFreightReferenceId);

	/// <summary>
	/// Saves the provided ERP shipmentFreightReference.
	/// </summary>
	/// <param name="shipmentFreightReference">The ERP shipmentFreightReference to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveShipmentFreightReference(ERPShipmentFreightReferenceDto shipmentFreightReference);
}
