using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSkillCompetencyRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SkillCompetency with the specified Unique Id exists.
	/// </summary>
	/// <param name="skillCompetencyId">The Unique Id of the SkillCompetency to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SkillCompetency exists or not.</returns>
	Task<bool> DoesSkillCompetencyExist(Guid skillCompetencyId);

	/// <summary>
	/// Retrieves all SkillCompetencies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SkillCompetencies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SkillCompetencies DTOs.</returns>
	Task<ICollection<ERPSkillCompetencyInformationDto>> GetAllSkillCompetencies(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific SkillCompetency.
	/// </summary>
	/// <param name="skillCompetencyId">The Unique Id of the SkillCompetency to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SkillCompetency DTO.</returns>
	Task<ERPSkillCompetencyInformationDto> GetSkillCompetency(Guid skillCompetencyId);
}
