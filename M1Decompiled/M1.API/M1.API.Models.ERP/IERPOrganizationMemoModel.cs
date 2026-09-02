using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPOrganizationMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all OrganizationMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllOrganizationMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving OrganizationMemo information based on the specified OrganizationMemo Unique Id.
	/// </summary>
	/// <param name="organizationMemoId">The Unique Id of the OrganizationMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetOrganizationMemo(Guid organizationMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating OrganizationMemo information based on the specified OrganizationMemo.
	/// </summary>
	/// <param name="organizationMemo">The OrganizationMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutOrganizationMemo(ERPOrganizationMemoDto organizationMemo);

	/// <summary>
	/// Processes the request to retrieve all OrganizationMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of OrganizationMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of OrganizationMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPOrganizationMemoDto>>> Process_GetAllOrganizationMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific OrganizationMemo.
	/// </summary>
	/// <param name="organizationMemoId">The Unique Id of the OrganizationMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the OrganizationMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPOrganizationMemoDto>> Process_GetOrganizationMemo(Guid organizationMemoId);

	/// <summary>
	/// Processes the creating or updating of a OrganizationMemo record.
	/// </summary>
	/// <param name="organizationMemo">The OrganizationMemo data transfer object (DTO) containing the details of the OrganizationMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the OrganizationMemo details.</returns>
	Task<ERPResponseMessageDto<ERPOrganizationMemoDto>> Process_PutOrganizationMemo(ERPOrganizationMemoDto organizationMemo);

	/// <summary>
	/// Validates the request for deleting a OrganizationMemo record.
	/// </summary>
	/// <param name="organizationMemoId">The Unique Id of the OrganizationMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteOrganizationMemo(Guid organizationMemoId);

	/// <summary>
	/// Processes the request to delete a OrganizationMemo record.
	/// </summary>
	/// <param name="organizationMemoId">The Unique Id of the OrganizationMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPOrganizationMemoDto>> Process_DeleteOrganizationMemo(Guid organizationMemoId);
}
