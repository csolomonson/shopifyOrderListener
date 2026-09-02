using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPJobScenarioModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all JobScenarios with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobScenarios to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllJobScenarios(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving JobScenario information based on the specified JobScenario Unique Id.
	/// </summary>
	/// <param name="jobScenarioId">The Unique Id of the JobScenario.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetJobScenario(Guid jobScenarioId);

	/// <summary>
	/// Validates the PUT request for creating or updating JobScenario information based on the specified JobScenario.
	/// </summary>
	/// <param name="jobScenario">The JobScenario details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutJobScenario(ERPJobScenarioDto jobScenario);

	/// <summary>
	/// Processes the request to retrieve all JobScenarios with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobScenarios to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of JobScenarios DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPJobScenarioDto>>> Process_GetAllJobScenarios(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific JobScenario.
	/// </summary>
	/// <param name="jobScenarioId">The Unique Id of the JobScenario to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the JobScenario DTO.</returns>
	Task<ERPResponseMessageDto<ERPJobScenarioDto>> Process_GetJobScenario(Guid jobScenarioId);

	/// <summary>
	/// Processes the creating or updating of a JobScenario record.
	/// </summary>
	/// <param name="jobScenario">The JobScenario data transfer object (DTO) containing the details of the JobScenario to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the JobScenario details.</returns>
	Task<ERPResponseMessageDto<ERPJobScenarioDto>> Process_PutJobScenario(ERPJobScenarioDto jobScenario);

	/// <summary>
	/// Validates the request for deleting a JobScenario record.
	/// </summary>
	/// <param name="jobScenarioId">The Unique Id of the JobScenario.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteJobScenario(Guid jobScenarioId);

	/// <summary>
	/// Processes the request to delete a JobScenario record.
	/// </summary>
	/// <param name="jobScenarioId">The Unique Id of the JobScenario.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPJobScenarioDto>> Process_DeleteJobScenario(Guid jobScenarioId);
}
