using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPJobCostModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all JobCosts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobCosts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllJobCosts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving JobCost information based on the specified JobCost Unique Id.
	/// </summary>
	/// <param name="jobCostId">The Unique Id of the JobCost.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetJobCost(Guid jobCostId);

	/// <summary>
	/// Validates the PUT request for creating or updating JobCost information based on the specified JobCost.
	/// </summary>
	/// <param name="jobCost">The JobCost details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutJobCost(ERPJobCostDto jobCost);

	/// <summary>
	/// Processes the request to retrieve all JobCosts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobCosts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of JobCosts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPJobCostDto>>> Process_GetAllJobCosts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific JobCost.
	/// </summary>
	/// <param name="jobCostId">The Unique Id of the JobCost to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the JobCost DTO.</returns>
	Task<ERPResponseMessageDto<ERPJobCostDto>> Process_GetJobCost(Guid jobCostId);

	/// <summary>
	/// Processes the creating or updating of a JobCost record.
	/// </summary>
	/// <param name="jobCost">The JobCost data transfer object (DTO) containing the details of the JobCost to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the JobCost details.</returns>
	Task<ERPResponseMessageDto<ERPJobCostDto>> Process_PutJobCost(ERPJobCostDto jobCost);

	/// <summary>
	/// Validates the request for deleting a JobCost record.
	/// </summary>
	/// <param name="jobCostId">The Unique Id of the JobCost.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteJobCost(Guid jobCostId);

	/// <summary>
	/// Processes the request to delete a JobCost record.
	/// </summary>
	/// <param name="jobCostId">The Unique Id of the JobCost.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPJobCostDto>> Process_DeleteJobCost(Guid jobCostId);
}
