using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPShipmentFreightLinkRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ShipmentFreightLink with the specified Unique Id exists.
	/// </summary>
	/// <param name="shipmentFreightLinkId">The Unique Id of the ShipmentFreightLink to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ShipmentFreightLink exists or not.</returns>
	Task<bool> DoesShipmentFreightLinkExist(Guid shipmentFreightLinkId);

	/// <summary>
	/// Retrieves all ShipmentFreightLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentFreightLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShipmentFreightLinks DTOs.</returns>
	Task<ICollection<ERPShipmentFreightLinkInformationDto>> GetAllShipmentFreightLinks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ShipmentFreightLink.
	/// </summary>
	/// <param name="shipmentFreightLinkId">The Unique Id of the ShipmentFreightLink to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ShipmentFreightLink DTO.</returns>
	Task<ERPShipmentFreightLinkInformationDto> GetShipmentFreightLink(Guid shipmentFreightLinkId);

	/// <summary>
	/// Saves the provided ERP shipmentFreightLink.
	/// </summary>
	/// <param name="shipmentFreightLink">The ERP shipmentFreightLink to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveShipmentFreightLink(ERPShipmentFreightLinkDto shipmentFreightLink);
}
