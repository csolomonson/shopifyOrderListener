using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPScheduleTaskBucketRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ScheduleTaskBucket with the specified Unique Id exists.
	/// </summary>
	/// <param name="scheduleTaskBucketId">The Unique Id of the ScheduleTaskBucket to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ScheduleTaskBucket exists or not.</returns>
	Task<bool> DoesScheduleTaskBucketExist(Guid scheduleTaskBucketId);

	/// <summary>
	/// Retrieves all ScheduleTaskBuckets with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleTaskBuckets to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ScheduleTaskBuckets DTOs.</returns>
	Task<ICollection<ERPScheduleTaskBucketInformationDto>> GetAllScheduleTaskBuckets(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ScheduleTaskBucket.
	/// </summary>
	/// <param name="scheduleTaskBucketId">The Unique Id of the ScheduleTaskBucket to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ScheduleTaskBucket DTO.</returns>
	Task<ERPScheduleTaskBucketInformationDto> GetScheduleTaskBucket(Guid scheduleTaskBucketId);
}
