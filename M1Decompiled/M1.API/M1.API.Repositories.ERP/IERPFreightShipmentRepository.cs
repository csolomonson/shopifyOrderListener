using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPFreightShipmentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a FreightShipment with the specified Unique Id exists.
	/// </summary>
	/// <param name="freightShipmentId">The Unique Id of the FreightShipment to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the FreightShipment exists or not.</returns>
	Task<bool> DoesFreightShipmentExist(Guid freightShipmentId);

	/// <summary>
	/// Retrieves all FreightShipments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FreightShipments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of FreightShipments DTOs.</returns>
	Task<ICollection<ERPFreightShipmentInformationDto>> GetAllFreightShipments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific FreightShipment.
	/// </summary>
	/// <param name="freightShipmentId">The Unique Id of the FreightShipment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the FreightShipment DTO.</returns>
	Task<ERPFreightShipmentInformationDto> GetFreightShipment(Guid freightShipmentId);

	/// <summary>
	/// Saves the provided ERP freightShipment.
	/// </summary>
	/// <param name="freightShipment">The ERP freightShipment to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveFreightShipment(ERPFreightShipmentDto freightShipment);
}
