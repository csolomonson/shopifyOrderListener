using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartAssemblyModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartAssemblies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartAssemblies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartAssemblies(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartAssembly information based on the specified PartAssembly Unique Id.
	/// </summary>
	/// <param name="partAssemblyId">The Unique Id of the PartAssembly.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartAssembly(Guid partAssemblyId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartAssembly information based on the specified PartAssembly.
	/// </summary>
	/// <param name="partAssembly">The PartAssembly details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartAssembly(ERPPartAssemblyDto partAssembly);

	/// <summary>
	/// Processes the request to retrieve all PartAssemblies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartAssemblies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartAssemblies DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartAssemblyDto>>> Process_GetAllPartAssemblies(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartAssembly.
	/// </summary>
	/// <param name="partAssemblyId">The Unique Id of the PartAssembly to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartAssembly DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartAssemblyDto>> Process_GetPartAssembly(Guid partAssemblyId);

	/// <summary>
	/// Processes the creating or updating of a PartAssembly record.
	/// </summary>
	/// <param name="partAssembly">The PartAssembly data transfer object (DTO) containing the details of the PartAssembly to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartAssembly details.</returns>
	Task<ERPResponseMessageDto<ERPPartAssemblyDto>> Process_PutPartAssembly(ERPPartAssemblyDto partAssembly);

	/// <summary>
	/// Validates the request for deleting a PartAssembly record.
	/// </summary>
	/// <param name="partAssemblyId">The Unique Id of the PartAssembly.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartAssembly(Guid partAssemblyId);

	/// <summary>
	/// Processes the request to delete a PartAssembly record.
	/// </summary>
	/// <param name="partAssemblyId">The Unique Id of the PartAssembly.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartAssemblyDto>> Process_DeletePartAssembly(Guid partAssemblyId);
}
