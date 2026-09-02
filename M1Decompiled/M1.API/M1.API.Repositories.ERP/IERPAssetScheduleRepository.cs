using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAssetScheduleRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a AssetSchedule with the specified Unique Id exists.
	/// </summary>
	/// <param name="assetScheduleId">The Unique Id of the AssetSchedule to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the AssetSchedule exists or not.</returns>
	Task<bool> DoesAssetScheduleExist(Guid assetScheduleId);

	/// <summary>
	/// Retrieves all AssetSchedules with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetSchedules to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AssetSchedules DTOs.</returns>
	Task<ICollection<ERPAssetScheduleInformationDto>> GetAllAssetSchedules(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific AssetSchedule.
	/// </summary>
	/// <param name="assetScheduleId">The Unique Id of the AssetSchedule to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the AssetSchedule DTO.</returns>
	Task<ERPAssetScheduleInformationDto> GetAssetSchedule(Guid assetScheduleId);

	/// <summary>
	/// Saves the provided ERP assetSchedule.
	/// </summary>
	/// <param name="assetSchedule">The ERP assetSchedule to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveAssetSchedule(ERPAssetScheduleDto assetSchedule);
}
