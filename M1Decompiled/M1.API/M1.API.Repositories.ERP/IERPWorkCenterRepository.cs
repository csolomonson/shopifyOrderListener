using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPWorkCenterRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a WorkCenter with the specified Unique Id exists.
	/// </summary>
	/// <param name="workCenterId">The Unique Id of the WorkCenter to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the WorkCenter exists or not.</returns>
	Task<bool> DoesWorkCenterExist(Guid workCenterId);

	/// <summary>
	/// Retrieves all WorkCenters with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WorkCenters to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WorkCenters DTOs.</returns>
	Task<ICollection<ERPWorkCenterInformationDto>> GetAllWorkCenters(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific WorkCenter.
	/// </summary>
	/// <param name="workCenterId">The Unique Id of the WorkCenter to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the WorkCenter DTO.</returns>
	Task<ERPWorkCenterInformationDto> GetWorkCenter(Guid workCenterId);

	/// <summary>
	/// Saves the provided ERP workCenter.
	/// </summary>
	/// <param name="workCenter">The ERP workCenter to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveWorkCenter(ERPWorkCenterDto workCenter);
}
