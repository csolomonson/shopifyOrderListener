using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartBinRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartBin with the specified Unique Id exists.
	/// </summary>
	/// <param name="partBinId">The Unique Id of the PartBin to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartBin exists or not.</returns>
	Task<bool> DoesPartBinExist(Guid partBinId);

	/// <summary>
	/// Retrieves all PartBins with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartBins to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartBins DTOs.</returns>
	Task<ICollection<ERPPartBinInformationDto>> GetAllPartBins(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartBin.
	/// </summary>
	/// <param name="partBinId">The Unique Id of the PartBin to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartBin DTO.</returns>
	Task<ERPPartBinInformationDto> GetPartBin(Guid partBinId);

	/// <summary>
	/// Saves the provided ERP partBin.
	/// </summary>
	/// <param name="partBin">The ERP partBin to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartBin(ERPPartBinDto partBin);
}
