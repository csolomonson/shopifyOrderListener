using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartGroupModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartGroups with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartGroups to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartGroups(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartGroup information based on the specified PartGroup Unique Id.
	/// </summary>
	/// <param name="partGroupId">The Unique Id of the PartGroup.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartGroup(Guid partGroupId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartGroup information based on the specified PartGroup.
	/// </summary>
	/// <param name="partGroup">The PartGroup details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartGroup(ERPPartGroupDto partGroup);

	/// <summary>
	/// Processes the request to retrieve all PartGroups with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartGroups to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartGroups DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartGroupDto>>> Process_GetAllPartGroups(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartGroup.
	/// </summary>
	/// <param name="partGroupId">The Unique Id of the PartGroup to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartGroup DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartGroupDto>> Process_GetPartGroup(Guid partGroupId);

	/// <summary>
	/// Processes the creating or updating of a PartGroup record.
	/// </summary>
	/// <param name="partGroup">The PartGroup data transfer object (DTO) containing the details of the PartGroup to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartGroup details.</returns>
	Task<ERPResponseMessageDto<ERPPartGroupDto>> Process_PutPartGroup(ERPPartGroupDto partGroup);

	/// <summary>
	/// Validates the request for deleting a PartGroup record.
	/// </summary>
	/// <param name="partGroupId">The Unique Id of the PartGroup.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartGroup(Guid partGroupId);

	/// <summary>
	/// Processes the request to delete a PartGroup record.
	/// </summary>
	/// <param name="partGroupId">The Unique Id of the PartGroup.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartGroupDto>> Process_DeletePartGroup(Guid partGroupId);
}
