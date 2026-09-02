using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPDMRShipmentLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a DMRShipmentLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="dMRShipmentLineId">The Unique Id of the DMRShipmentLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the DMRShipmentLine exists or not.</returns>
	Task<bool> DoesDMRShipmentLineExist(Guid dMRShipmentLineId);

	/// <summary>
	/// Retrieves all DMRShipmentLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DMRShipmentLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of DMRShipmentLines DTOs.</returns>
	Task<ICollection<ERPDMRShipmentLineInformationDto>> GetAllDMRShipmentLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific DMRShipmentLine.
	/// </summary>
	/// <param name="dMRShipmentLineId">The Unique Id of the DMRShipmentLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the DMRShipmentLine DTO.</returns>
	Task<ERPDMRShipmentLineInformationDto> GetDMRShipmentLine(Guid dMRShipmentLineId);

	/// <summary>
	/// Saves the provided ERP dMRShipmentLine.
	/// </summary>
	/// <param name="dMRShipmentLine">The ERP dMRShipmentLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveDMRShipmentLine(ERPDMRShipmentLineDto dMRShipmentLine);
}
