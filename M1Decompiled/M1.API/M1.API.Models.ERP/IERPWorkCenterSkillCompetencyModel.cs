using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPWorkCenterSkillCompetencyModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all WorkCenterSkillCompetencies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WorkCenterSkillCompetencies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllWorkCenterSkillCompetencies(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving WorkCenterSkillCompetency information based on the specified WorkCenterSkillCompetency Unique Id.
	/// </summary>
	/// <param name="workCenterSkillCompetencyId">The Unique Id of the WorkCenterSkillCompetency.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetWorkCenterSkillCompetency(Guid workCenterSkillCompetencyId);

	/// <summary>
	/// Processes the request to retrieve all WorkCenterSkillCompetencies with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WorkCenterSkillCompetencies to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WorkCenterSkillCompetencies DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPWorkCenterSkillCompetencyDto>>> Process_GetAllWorkCenterSkillCompetencies(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific WorkCenterSkillCompetency.
	/// </summary>
	/// <param name="workCenterSkillCompetencyId">The Unique Id of the WorkCenterSkillCompetency to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the WorkCenterSkillCompetency DTO.</returns>
	Task<ERPResponseMessageDto<ERPWorkCenterSkillCompetencyDto>> Process_GetWorkCenterSkillCompetency(Guid workCenterSkillCompetencyId);
}
