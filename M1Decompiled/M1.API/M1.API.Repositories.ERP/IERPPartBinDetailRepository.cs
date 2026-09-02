using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartBinDetailRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartBinDetail with the specified Unique Id exists.
	/// </summary>
	/// <param name="partBinDetailId">The Unique Id of the PartBinDetail to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartBinDetail exists or not.</returns>
	Task<bool> DoesPartBinDetailExist(Guid partBinDetailId);

	/// <summary>
	/// Retrieves all PartBinDetails with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartBinDetails to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartBinDetails DTOs.</returns>
	Task<ICollection<ERPPartBinDetailInformationDto>> GetAllPartBinDetails(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartBinDetail.
	/// </summary>
	/// <param name="partBinDetailId">The Unique Id of the PartBinDetail to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartBinDetail DTO.</returns>
	Task<ERPPartBinDetailInformationDto> GetPartBinDetail(Guid partBinDetailId);

	/// <summary>
	/// Saves the provided ERP partBinDetail.
	/// </summary>
	/// <param name="partBinDetail">The ERP partBinDetail to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartBinDetail(ERPPartBinDetailDto partBinDetail);
}
