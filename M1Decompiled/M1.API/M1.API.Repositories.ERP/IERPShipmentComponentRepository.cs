using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPShipmentComponentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ShipmentComponent with the specified Unique Id exists.
	/// </summary>
	/// <param name="shipmentComponentId">The Unique Id of the ShipmentComponent to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ShipmentComponent exists or not.</returns>
	Task<bool> DoesShipmentComponentExist(Guid shipmentComponentId);

	/// <summary>
	/// Retrieves all ShipmentComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShipmentComponents DTOs.</returns>
	Task<ICollection<ERPShipmentComponentInformationDto>> GetAllShipmentComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ShipmentComponent.
	/// </summary>
	/// <param name="shipmentComponentId">The Unique Id of the ShipmentComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ShipmentComponent DTO.</returns>
	Task<ERPShipmentComponentInformationDto> GetShipmentComponent(Guid shipmentComponentId);

	/// <summary>
	/// Saves the provided ERP shipmentComponent.
	/// </summary>
	/// <param name="shipmentComponent">The ERP shipmentComponent to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveShipmentComponent(ERPShipmentComponentDto shipmentComponent);
}
