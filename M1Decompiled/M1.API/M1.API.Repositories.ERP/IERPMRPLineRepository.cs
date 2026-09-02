using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPMRPLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a MRPLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="mRPLineId">The Unique Id of the MRPLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the MRPLine exists or not.</returns>
	Task<bool> DoesMRPLineExist(Guid mRPLineId);

	/// <summary>
	/// Retrieves all MRPLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MRPLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MRPLines DTOs.</returns>
	Task<ICollection<ERPMRPLineInformationDto>> GetAllMRPLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific MRPLine.
	/// </summary>
	/// <param name="mRPLineId">The Unique Id of the MRPLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the MRPLine DTO.</returns>
	Task<ERPMRPLineInformationDto> GetMRPLine(Guid mRPLineId);

	/// <summary>
	/// Saves the provided ERP mRPLine.
	/// </summary>
	/// <param name="mRPLine">The ERP mRPLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveMRPLine(ERPMRPLineDto mRPLine);
}
