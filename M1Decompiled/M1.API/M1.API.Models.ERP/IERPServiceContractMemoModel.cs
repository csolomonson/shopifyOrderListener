using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPServiceContractMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ServiceContractMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ServiceContractMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllServiceContractMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ServiceContractMemo information based on the specified ServiceContractMemo Unique Id.
	/// </summary>
	/// <param name="serviceContractMemoId">The Unique Id of the ServiceContractMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetServiceContractMemo(Guid serviceContractMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating ServiceContractMemo information based on the specified ServiceContractMemo.
	/// </summary>
	/// <param name="serviceContractMemo">The ServiceContractMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutServiceContractMemo(ERPServiceContractMemoDto serviceContractMemo);

	/// <summary>
	/// Processes the request to retrieve all ServiceContractMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ServiceContractMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ServiceContractMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPServiceContractMemoDto>>> Process_GetAllServiceContractMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ServiceContractMemo.
	/// </summary>
	/// <param name="serviceContractMemoId">The Unique Id of the ServiceContractMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ServiceContractMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPServiceContractMemoDto>> Process_GetServiceContractMemo(Guid serviceContractMemoId);

	/// <summary>
	/// Processes the creating or updating of a ServiceContractMemo record.
	/// </summary>
	/// <param name="serviceContractMemo">The ServiceContractMemo data transfer object (DTO) containing the details of the ServiceContractMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ServiceContractMemo details.</returns>
	Task<ERPResponseMessageDto<ERPServiceContractMemoDto>> Process_PutServiceContractMemo(ERPServiceContractMemoDto serviceContractMemo);

	/// <summary>
	/// Validates the request for deleting a ServiceContractMemo record.
	/// </summary>
	/// <param name="serviceContractMemoId">The Unique Id of the ServiceContractMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteServiceContractMemo(Guid serviceContractMemoId);

	/// <summary>
	/// Processes the request to delete a ServiceContractMemo record.
	/// </summary>
	/// <param name="serviceContractMemoId">The Unique Id of the ServiceContractMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPServiceContractMemoDto>> Process_DeleteServiceContractMemo(Guid serviceContractMemoId);
}
