using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPEmployeeSkillRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a EmployeeSkill with the specified Unique Id exists.
	/// </summary>
	/// <param name="employeeSkillId">The Unique Id of the EmployeeSkill to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the EmployeeSkill exists or not.</returns>
	Task<bool> DoesEmployeeSkillExist(Guid employeeSkillId);

	/// <summary>
	/// Retrieves all EmployeeSkills with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeSkills to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeeSkills DTOs.</returns>
	Task<ICollection<ERPEmployeeSkillInformationDto>> GetAllEmployeeSkills(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific EmployeeSkill.
	/// </summary>
	/// <param name="employeeSkillId">The Unique Id of the EmployeeSkill to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the EmployeeSkill DTO.</returns>
	Task<ERPEmployeeSkillInformationDto> GetEmployeeSkill(Guid employeeSkillId);
}
