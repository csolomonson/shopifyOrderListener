using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPServiceContractLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ServiceContractLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ServiceContractLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllServiceContractLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ServiceContractLine information based on the specified ServiceContractLine Unique Id.
	/// </summary>
	/// <param name="serviceContractLineId">The Unique Id of the ServiceContractLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetServiceContractLine(Guid serviceContractLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating ServiceContractLine information based on the specified ServiceContractLine.
	/// </summary>
	/// <param name="serviceContractLine">The ServiceContractLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutServiceContractLine(ERPServiceContractLineDto serviceContractLine);

	/// <summary>
	/// Processes the request to retrieve all ServiceContractLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ServiceContractLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ServiceContractLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPServiceContractLineDto>>> Process_GetAllServiceContractLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ServiceContractLine.
	/// </summary>
	/// <param name="serviceContractLineId">The Unique Id of the ServiceContractLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ServiceContractLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPServiceContractLineDto>> Process_GetServiceContractLine(Guid serviceContractLineId);

	/// <summary>
	/// Processes the creating or updating of a ServiceContractLine record.
	/// </summary>
	/// <param name="serviceContractLine">The ServiceContractLine data transfer object (DTO) containing the details of the ServiceContractLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ServiceContractLine details.</returns>
	Task<ERPResponseMessageDto<ERPServiceContractLineDto>> Process_PutServiceContractLine(ERPServiceContractLineDto serviceContractLine);

	/// <summary>
	/// Validates the request for deleting a ServiceContractLine record.
	/// </summary>
	/// <param name="serviceContractLineId">The Unique Id of the ServiceContractLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteServiceContractLine(Guid serviceContractLineId);

	/// <summary>
	/// Processes the request to delete a ServiceContractLine record.
	/// </summary>
	/// <param name="serviceContractLineId">The Unique Id of the ServiceContractLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPServiceContractLineDto>> Process_DeleteServiceContractLine(Guid serviceContractLineId);
}
