using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPServiceContractTypeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ServiceContractTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ServiceContractTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllServiceContractTypes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ServiceContractType information based on the specified ServiceContractType Unique Id.
	/// </summary>
	/// <param name="serviceContractTypeId">The Unique Id of the ServiceContractType.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetServiceContractType(Guid serviceContractTypeId);

	/// <summary>
	/// Processes the request to retrieve all ServiceContractTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ServiceContractTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ServiceContractTypes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPServiceContractTypeDto>>> Process_GetAllServiceContractTypes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ServiceContractType.
	/// </summary>
	/// <param name="serviceContractTypeId">The Unique Id of the ServiceContractType to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ServiceContractType DTO.</returns>
	Task<ERPResponseMessageDto<ERPServiceContractTypeDto>> Process_GetServiceContractType(Guid serviceContractTypeId);
}
