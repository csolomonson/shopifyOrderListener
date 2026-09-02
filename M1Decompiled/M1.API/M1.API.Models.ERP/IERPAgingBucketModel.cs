using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAgingBucketModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all AgingBuckets with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AgingBuckets to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAgingBuckets(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving AgingBucket information based on the specified AgingBucket Unique Id.
	/// </summary>
	/// <param name="agingBucketId">The Unique Id of the AgingBucket.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAgingBucket(Guid agingBucketId);

	/// <summary>
	/// Processes the request to retrieve all AgingBuckets with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AgingBuckets to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AgingBuckets DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAgingBucketDto>>> Process_GetAllAgingBuckets(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific AgingBucket.
	/// </summary>
	/// <param name="agingBucketId">The Unique Id of the AgingBucket to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the AgingBucket DTO.</returns>
	Task<ERPResponseMessageDto<ERPAgingBucketDto>> Process_GetAgingBucket(Guid agingBucketId);
}
