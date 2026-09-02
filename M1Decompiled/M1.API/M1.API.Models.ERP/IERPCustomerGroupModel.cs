using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPCustomerGroupModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all CustomerGroups with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CustomerGroups to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllCustomerGroups(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving CustomerGroup information based on the specified CustomerGroup Unique Id.
	/// </summary>
	/// <param name="customerGroupId">The Unique Id of the CustomerGroup.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetCustomerGroup(Guid customerGroupId);

	/// <summary>
	/// Validates the PUT request for creating or updating CustomerGroup information based on the specified CustomerGroup.
	/// </summary>
	/// <param name="customerGroup">The CustomerGroup details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutCustomerGroup(ERPCustomerGroupDto customerGroup);

	/// <summary>
	/// Processes the request to retrieve all CustomerGroups with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CustomerGroups to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CustomerGroups DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPCustomerGroupDto>>> Process_GetAllCustomerGroups(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific CustomerGroup.
	/// </summary>
	/// <param name="customerGroupId">The Unique Id of the CustomerGroup to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the CustomerGroup DTO.</returns>
	Task<ERPResponseMessageDto<ERPCustomerGroupDto>> Process_GetCustomerGroup(Guid customerGroupId);

	/// <summary>
	/// Processes the creating or updating of a CustomerGroup record.
	/// </summary>
	/// <param name="customerGroup">The CustomerGroup data transfer object (DTO) containing the details of the CustomerGroup to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the CustomerGroup details.</returns>
	Task<ERPResponseMessageDto<ERPCustomerGroupDto>> Process_PutCustomerGroup(ERPCustomerGroupDto customerGroup);

	/// <summary>
	/// Validates the request for deleting a CustomerGroup record.
	/// </summary>
	/// <param name="customerGroupId">The Unique Id of the CustomerGroup.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteCustomerGroup(Guid customerGroupId);

	/// <summary>
	/// Processes the request to delete a CustomerGroup record.
	/// </summary>
	/// <param name="customerGroupId">The Unique Id of the CustomerGroup.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPCustomerGroupDto>> Process_DeleteCustomerGroup(Guid customerGroupId);
}
