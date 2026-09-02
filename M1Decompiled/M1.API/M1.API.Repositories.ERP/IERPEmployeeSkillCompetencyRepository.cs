using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPEmployeeSkillCompetencyRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a EmployeeSkillCompetency with the specified Unique Id exists.
	/// </summary>
	/// <param name="employeeSkillCompetencyId">The Unique Id of the EmployeeSkillCompetency to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the EmployeeSkillCompetency exists or not.</returns>
	Task<bool> DoesEmployeeSkillCompetencyExist(Guid employeeSkillCompetencyId);

	/// <summary>
	/// Retrieves all EmployeeSkillCompetencies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeSkillCompetencies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeeSkillCompetencies DTOs.</returns>
	Task<ICollection<ERPEmployeeSkillCompetencyInformationDto>> GetAllEmployeeSkillCompetencies(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific EmployeeSkillCompetency.
	/// </summary>
	/// <param name="employeeSkillCompetencyId">The Unique Id of the EmployeeSkillCompetency to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the EmployeeSkillCompetency DTO.</returns>
	Task<ERPEmployeeSkillCompetencyInformationDto> GetEmployeeSkillCompetency(Guid employeeSkillCompetencyId);
}
