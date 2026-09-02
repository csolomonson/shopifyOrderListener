using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPShipmentLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ShipmentLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="shipmentLineId">The Unique Id of the ShipmentLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ShipmentLine exists or not.</returns>
	Task<bool> DoesShipmentLineExist(Guid shipmentLineId);

	/// <summary>
	/// Retrieves all ShipmentLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShipmentLines DTOs.</returns>
	Task<ICollection<ERPShipmentLineInformationDto>> GetAllShipmentLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ShipmentLine.
	/// </summary>
	/// <param name="shipmentLineId">The Unique Id of the ShipmentLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ShipmentLine DTO.</returns>
	Task<ERPShipmentLineInformationDto> GetShipmentLine(Guid shipmentLineId);

	/// <summary>
	/// Saves the provided ERP shipmentLine.
	/// </summary>
	/// <param name="shipmentLine">The ERP shipmentLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveShipmentLine(ERPShipmentLineDto shipmentLine);
}
