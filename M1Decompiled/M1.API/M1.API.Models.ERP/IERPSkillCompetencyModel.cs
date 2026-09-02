using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPSkillCompetencyModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all SkillCompetencies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SkillCompetencies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllSkillCompetencies(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving SkillCompetency information based on the specified SkillCompetency Unique Id.
	/// </summary>
	/// <param name="skillCompetencyId">The Unique Id of the SkillCompetency.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSkillCompetency(Guid skillCompetencyId);

	/// <summary>
	/// Processes the request to retrieve all SkillCompetencies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SkillCompetencies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SkillCompetencies DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPSkillCompetencyDto>>> Process_GetAllSkillCompetencies(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific SkillCompetency.
	/// </summary>
	/// <param name="skillCompetencyId">The Unique Id of the SkillCompetency to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the SkillCompetency DTO.</returns>
	Task<ERPResponseMessageDto<ERPSkillCompetencyDto>> Process_GetSkillCompetency(Guid skillCompetencyId);
}
