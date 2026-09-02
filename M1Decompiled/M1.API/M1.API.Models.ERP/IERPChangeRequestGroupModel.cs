using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPChangeRequestGroupModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ChangeRequestGroups with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ChangeRequestGroups to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllChangeRequestGroups(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ChangeRequestGroup information based on the specified ChangeRequestGroup Unique Id.
	/// </summary>
	/// <param name="changeRequestGroupId">The Unique Id of the ChangeRequestGroup.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetChangeRequestGroup(Guid changeRequestGroupId);

	/// <summary>
	/// Processes the request to retrieve all ChangeRequestGroups with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ChangeRequestGroups to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ChangeRequestGroups DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPChangeRequestGroupDto>>> Process_GetAllChangeRequestGroups(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ChangeRequestGroup.
	/// </summary>
	/// <param name="changeRequestGroupId">The Unique Id of the ChangeRequestGroup to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ChangeRequestGroup DTO.</returns>
	Task<ERPResponseMessageDto<ERPChangeRequestGroupDto>> Process_GetChangeRequestGroup(Guid changeRequestGroupId);
}
