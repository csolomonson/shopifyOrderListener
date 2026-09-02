using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPWorkCenterSkillRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a WorkCenterSkill with the specified Unique Id exists.
	/// </summary>
	/// <param name="workCenterSkillId">The Unique Id of the WorkCenterSkill to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the WorkCenterSkill exists or not.</returns>
	Task<bool> DoesWorkCenterSkillExist(Guid workCenterSkillId);

	/// <summary>
	/// Retrieves all WorkCenterSkills with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WorkCenterSkills to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WorkCenterSkills DTOs.</returns>
	Task<ICollection<ERPWorkCenterSkillInformationDto>> GetAllWorkCenterSkills(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific WorkCenterSkill.
	/// </summary>
	/// <param name="workCenterSkillId">The Unique Id of the WorkCenterSkill to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the WorkCenterSkill DTO.</returns>
	Task<ERPWorkCenterSkillInformationDto> GetWorkCenterSkill(Guid workCenterSkillId);
}
