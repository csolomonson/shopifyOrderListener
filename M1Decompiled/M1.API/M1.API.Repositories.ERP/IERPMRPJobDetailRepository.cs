using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPMRPJobDetailRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a MRPJobDetail with the specified Unique Id exists.
	/// </summary>
	/// <param name="mRPJobDetailId">The Unique Id of the MRPJobDetail to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the MRPJobDetail exists or not.</returns>
	Task<bool> DoesMRPJobDetailExist(Guid mRPJobDetailId);

	/// <summary>
	/// Retrieves all MRPJobDetails with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MRPJobDetails to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MRPJobDetails DTOs.</returns>
	Task<ICollection<ERPMRPJobDetailInformationDto>> GetAllMRPJobDetails(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific MRPJobDetail.
	/// </summary>
	/// <param name="mRPJobDetailId">The Unique Id of the MRPJobDetail to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the MRPJobDetail DTO.</returns>
	Task<ERPMRPJobDetailInformationDto> GetMRPJobDetail(Guid mRPJobDetailId);

	/// <summary>
	/// Saves the provided ERP mRPJobDetail.
	/// </summary>
	/// <param name="mRPJobDetail">The ERP mRPJobDetail to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveMRPJobDetail(ERPMRPJobDetailDto mRPJobDetail);
}
