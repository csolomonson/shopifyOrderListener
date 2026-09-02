using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSkillRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Skill with the specified Unique Id exists.
	/// </summary>
	/// <param name="skillId">The Unique Id of the Skill to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Skill exists or not.</returns>
	Task<bool> DoesSkillExist(Guid skillId);

	/// <summary>
	/// Retrieves all Skills with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Skills to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Skills DTOs.</returns>
	Task<ICollection<ERPSkillInformationDto>> GetAllSkills(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Skill.
	/// </summary>
	/// <param name="skillId">The Unique Id of the Skill to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Skill DTO.</returns>
	Task<ERPSkillInformationDto> GetSkill(Guid skillId);
}
