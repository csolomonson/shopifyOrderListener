using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPWorkCenterSkillCompetencyRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a WorkCenterSkillCompetency with the specified Unique Id exists.
	/// </summary>
	/// <param name="workCenterSkillCompetencyId">The Unique Id of the WorkCenterSkillCompetency to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the WorkCenterSkillCompetency exists or not.</returns>
	Task<bool> DoesWorkCenterSkillCompetencyExist(Guid workCenterSkillCompetencyId);

	/// <summary>
	/// Retrieves all WorkCenterSkillCompetencies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WorkCenterSkillCompetencies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WorkCenterSkillCompetencies DTOs.</returns>
	Task<ICollection<ERPWorkCenterSkillCompetencyInformationDto>> GetAllWorkCenterSkillCompetencies(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific WorkCenterSkillCompetency.
	/// </summary>
	/// <param name="workCenterSkillCompetencyId">The Unique Id of the WorkCenterSkillCompetency to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the WorkCenterSkillCompetency DTO.</returns>
	Task<ERPWorkCenterSkillCompetencyInformationDto> GetWorkCenterSkillCompetency(Guid workCenterSkillCompetencyId);
}
