using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPDMRShipmentComponentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a DMRShipmentComponent with the specified Unique Id exists.
	/// </summary>
	/// <param name="dMRShipmentComponentId">The Unique Id of the DMRShipmentComponent to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the DMRShipmentComponent exists or not.</returns>
	Task<bool> DoesDMRShipmentComponentExist(Guid dMRShipmentComponentId);

	/// <summary>
	/// Retrieves all DMRShipmentComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRShipmentComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of DMRShipmentComponents DTOs.</returns>
	Task<ICollection<ERPDMRShipmentComponentInformationDto>> GetAllDMRShipmentComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific DMRShipmentComponent.
	/// </summary>
	/// <param name="dMRShipmentComponentId">The Unique Id of the DMRShipmentComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the DMRShipmentComponent DTO.</returns>
	Task<ERPDMRShipmentComponentInformationDto> GetDMRShipmentComponent(Guid dMRShipmentComponentId);

	/// <summary>
	/// Saves the provided ERP dMRShipmentComponent.
	/// </summary>
	/// <param name="dMRShipmentComponent">The ERP dMRShipmentComponent to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveDMRShipmentComponent(ERPDMRShipmentComponentDto dMRShipmentComponent);
}
