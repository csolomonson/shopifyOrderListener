using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPJobAssemblyModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all JobAssemblies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobAssemblies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllJobAssemblies(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving JobAssembly information based on the specified JobAssembly Unique Id.
	/// </summary>
	/// <param name="jobAssemblyId">The Unique Id of the JobAssembly.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetJobAssembly(Guid jobAssemblyId);

	/// <summary>
	/// Validates the PUT request for creating or updating JobAssembly information based on the specified JobAssembly.
	/// </summary>
	/// <param name="jobAssembly">The JobAssembly details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutJobAssembly(ERPJobAssemblyDto jobAssembly);

	/// <summary>
	/// Processes the request to retrieve all JobAssemblies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobAssemblies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of JobAssemblies DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPJobAssemblyDto>>> Process_GetAllJobAssemblies(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific JobAssembly.
	/// </summary>
	/// <param name="jobAssemblyId">The Unique Id of the JobAssembly to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the JobAssembly DTO.</returns>
	Task<ERPResponseMessageDto<ERPJobAssemblyDto>> Process_GetJobAssembly(Guid jobAssemblyId);

	/// <summary>
	/// Processes the creating or updating of a JobAssembly record.
	/// </summary>
	/// <param name="jobAssembly">The JobAssembly data transfer object (DTO) containing the details of the JobAssembly to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the JobAssembly details.</returns>
	Task<ERPResponseMessageDto<ERPJobAssemblyDto>> Process_PutJobAssembly(ERPJobAssemblyDto jobAssembly);

	/// <summary>
	/// Validates the request for deleting a JobAssembly record.
	/// </summary>
	/// <param name="jobAssemblyId">The Unique Id of the JobAssembly.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteJobAssembly(Guid jobAssemblyId);

	/// <summary>
	/// Processes the request to delete a JobAssembly record.
	/// </summary>
	/// <param name="jobAssemblyId">The Unique Id of the JobAssembly.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPJobAssemblyDto>> Process_DeleteJobAssembly(Guid jobAssemblyId);
}
