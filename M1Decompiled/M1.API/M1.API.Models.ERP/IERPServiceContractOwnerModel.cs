using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPServiceContractOwnerModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ServiceContractOwners with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ServiceContractOwners to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllServiceContractOwners(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ServiceContractOwner information based on the specified ServiceContractOwner Unique Id.
	/// </summary>
	/// <param name="serviceContractOwnerId">The Unique Id of the ServiceContractOwner.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetServiceContractOwner(Guid serviceContractOwnerId);

	/// <summary>
	/// Validates the PUT request for creating or updating ServiceContractOwner information based on the specified ServiceContractOwner.
	/// </summary>
	/// <param name="serviceContractOwner">The ServiceContractOwner details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutServiceContractOwner(ERPServiceContractOwnerDto serviceContractOwner);

	/// <summary>
	/// Processes the request to retrieve all ServiceContractOwners with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ServiceContractOwners to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ServiceContractOwners DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPServiceContractOwnerDto>>> Process_GetAllServiceContractOwners(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ServiceContractOwner.
	/// </summary>
	/// <param name="serviceContractOwnerId">The Unique Id of the ServiceContractOwner to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ServiceContractOwner DTO.</returns>
	Task<ERPResponseMessageDto<ERPServiceContractOwnerDto>> Process_GetServiceContractOwner(Guid serviceContractOwnerId);

	/// <summary>
	/// Processes the creating or updating of a ServiceContractOwner record.
	/// </summary>
	/// <param name="serviceContractOwner">The ServiceContractOwner data transfer object (DTO) containing the details of the ServiceContractOwner to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ServiceContractOwner details.</returns>
	Task<ERPResponseMessageDto<ERPServiceContractOwnerDto>> Process_PutServiceContractOwner(ERPServiceContractOwnerDto serviceContractOwner);

	/// <summary>
	/// Validates the request for deleting a ServiceContractOwner record.
	/// </summary>
	/// <param name="serviceContractOwnerId">The Unique Id of the ServiceContractOwner.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteServiceContractOwner(Guid serviceContractOwnerId);

	/// <summary>
	/// Processes the request to delete a ServiceContractOwner record.
	/// </summary>
	/// <param name="serviceContractOwnerId">The Unique Id of the ServiceContractOwner.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPServiceContractOwnerDto>> Process_DeleteServiceContractOwner(Guid serviceContractOwnerId);
}
