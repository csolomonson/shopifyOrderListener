using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPSkillModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Skills with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Skills to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllSkills(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Skill information based on the specified Skill Unique Id.
	/// </summary>
	/// <param name="skillId">The Unique Id of the Skill.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSkill(Guid skillId);

	/// <summary>
	/// Processes the request to retrieve all Skills with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Skills to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Skills DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPSkillDto>>> Process_GetAllSkills(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Skill.
	/// </summary>
	/// <param name="skillId">The Unique Id of the Skill to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Skill DTO.</returns>
	Task<ERPResponseMessageDto<ERPSkillDto>> Process_GetSkill(Guid skillId);
}
