using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPEmployeeSkillCompetencyModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all EmployeeSkillCompetencies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeSkillCompetencies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeeSkillCompetencies(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving EmployeeSkillCompetency information based on the specified EmployeeSkillCompetency Unique Id.
	/// </summary>
	/// <param name="employeeSkillCompetencyId">The Unique Id of the EmployeeSkillCompetency.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetEmployeeSkillCompetency(Guid employeeSkillCompetencyId);

	/// <summary>
	/// Processes the request to retrieve all EmployeeSkillCompetencies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeSkillCompetencies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeeSkillCompetencies DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPEmployeeSkillCompetencyDto>>> Process_GetAllEmployeeSkillCompetencies(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific EmployeeSkillCompetency.
	/// </summary>
	/// <param name="employeeSkillCompetencyId">The Unique Id of the EmployeeSkillCompetency to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the EmployeeSkillCompetency DTO.</returns>
	Task<ERPResponseMessageDto<ERPEmployeeSkillCompetencyDto>> Process_GetEmployeeSkillCompetency(Guid employeeSkillCompetencyId);
}
