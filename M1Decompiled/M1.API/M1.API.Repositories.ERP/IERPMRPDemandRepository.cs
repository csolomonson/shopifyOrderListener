using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPMRPDemandRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a MRPDemand with the specified Unique Id exists.
	/// </summary>
	/// <param name="mRPDemandId">The Unique Id of the MRPDemand to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the MRPDemand exists or not.</returns>
	Task<bool> DoesMRPDemandExist(Guid mRPDemandId);

	/// <summary>
	/// Retrieves all MRPDemands with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MRPDemands to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MRPDemands DTOs.</returns>
	Task<ICollection<ERPMRPDemandInformationDto>> GetAllMRPDemands(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific MRPDemand.
	/// </summary>
	/// <param name="mRPDemandId">The Unique Id of the MRPDemand to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the MRPDemand DTO.</returns>
	Task<ERPMRPDemandInformationDto> GetMRPDemand(Guid mRPDemandId);

	/// <summary>
	/// Saves the provided ERP mRPDemand.
	/// </summary>
	/// <param name="mRPDemand">The ERP mRPDemand to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveMRPDemand(ERPMRPDemandDto mRPDemand);
}
