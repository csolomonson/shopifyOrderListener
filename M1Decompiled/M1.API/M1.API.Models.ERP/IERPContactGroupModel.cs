using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPContactGroupModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ContactGroups with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ContactGroups to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllContactGroups(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ContactGroup information based on the specified ContactGroup Unique Id.
	/// </summary>
	/// <param name="contactGroupId">The Unique Id of the ContactGroup.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetContactGroup(Guid contactGroupId);

	/// <summary>
	/// Validates the PUT request for creating or updating ContactGroup information based on the specified ContactGroup.
	/// </summary>
	/// <param name="contactGroup">The ContactGroup details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutContactGroup(ERPContactGroupDto contactGroup);

	/// <summary>
	/// Processes the request to retrieve all ContactGroups with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ContactGroups to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ContactGroups DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPContactGroupDto>>> Process_GetAllContactGroups(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ContactGroup.
	/// </summary>
	/// <param name="contactGroupId">The Unique Id of the ContactGroup to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ContactGroup DTO.</returns>
	Task<ERPResponseMessageDto<ERPContactGroupDto>> Process_GetContactGroup(Guid contactGroupId);

	/// <summary>
	/// Processes the creating or updating of a ContactGroup record.
	/// </summary>
	/// <param name="contactGroup">The ContactGroup data transfer object (DTO) containing the details of the ContactGroup to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ContactGroup details.</returns>
	Task<ERPResponseMessageDto<ERPContactGroupDto>> Process_PutContactGroup(ERPContactGroupDto contactGroup);

	/// <summary>
	/// Validates the request for deleting a ContactGroup record.
	/// </summary>
	/// <param name="contactGroupId">The Unique Id of the ContactGroup.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteContactGroup(Guid contactGroupId);

	/// <summary>
	/// Processes the request to delete a ContactGroup record.
	/// </summary>
	/// <param name="contactGroupId">The Unique Id of the ContactGroup.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPContactGroupDto>> Process_DeleteContactGroup(Guid contactGroupId);
}
