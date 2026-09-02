using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPServiceContractModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ServiceContracts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ServiceContracts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllServiceContracts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ServiceContract information based on the specified ServiceContract Unique Id.
	/// </summary>
	/// <param name="serviceContractId">The Unique Id of the ServiceContract.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetServiceContract(Guid serviceContractId);

	/// <summary>
	/// Validates the PUT request for creating or updating ServiceContract information based on the specified ServiceContract.
	/// </summary>
	/// <param name="serviceContract">The ServiceContract details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutServiceContract(ERPServiceContractDto serviceContract);

	/// <summary>
	/// Processes the request to retrieve all ServiceContracts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ServiceContracts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ServiceContracts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPServiceContractDto>>> Process_GetAllServiceContracts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ServiceContract.
	/// </summary>
	/// <param name="serviceContractId">The Unique Id of the ServiceContract to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ServiceContract DTO.</returns>
	Task<ERPResponseMessageDto<ERPServiceContractDto>> Process_GetServiceContract(Guid serviceContractId);

	/// <summary>
	/// Processes the creating or updating of a ServiceContract record.
	/// </summary>
	/// <param name="serviceContract">The ServiceContract data transfer object (DTO) containing the details of the ServiceContract to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ServiceContract details.</returns>
	Task<ERPResponseMessageDto<ERPServiceContractDto>> Process_PutServiceContract(ERPServiceContractDto serviceContract);

	/// <summary>
	/// Validates the request for deleting a ServiceContract record.
	/// </summary>
	/// <param name="serviceContractId">The Unique Id of the ServiceContract.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteServiceContract(Guid serviceContractId);

	/// <summary>
	/// Processes the request to delete a ServiceContract record.
	/// </summary>
	/// <param name="serviceContractId">The Unique Id of the ServiceContract.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPServiceContractDto>> Process_DeleteServiceContract(Guid serviceContractId);
}
