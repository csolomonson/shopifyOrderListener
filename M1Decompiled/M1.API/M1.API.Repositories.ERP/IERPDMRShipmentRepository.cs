using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPDMRShipmentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a DMRShipment with the specified Unique Id exists.
	/// </summary>
	/// <param name="dMRShipmentId">The Unique Id of the DMRShipment to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the DMRShipment exists or not.</returns>
	Task<bool> DoesDMRShipmentExist(Guid dMRShipmentId);

	/// <summary>
	/// Retrieves all DMRShipments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRShipments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of DMRShipments DTOs.</returns>
	Task<ICollection<ERPDMRShipmentInformationDto>> GetAllDMRShipments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific DMRShipment.
	/// </summary>
	/// <param name="dMRShipmentId">The Unique Id of the DMRShipment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the DMRShipment DTO.</returns>
	Task<ERPDMRShipmentInformationDto> GetDMRShipment(Guid dMRShipmentId);

	/// <summary>
	/// Saves the provided ERP dMRShipment.
	/// </summary>
	/// <param name="dMRShipment">The ERP dMRShipment to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveDMRShipment(ERPDMRShipmentDto dMRShipment);
}
