using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPWorkCenterSkillModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all WorkCenterSkills with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WorkCenterSkills to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllWorkCenterSkills(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving WorkCenterSkill information based on the specified WorkCenterSkill Unique Id.
	/// </summary>
	/// <param name="workCenterSkillId">The Unique Id of the WorkCenterSkill.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetWorkCenterSkill(Guid workCenterSkillId);

	/// <summary>
	/// Processes the request to retrieve all WorkCenterSkills with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WorkCenterSkills to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WorkCenterSkills DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPWorkCenterSkillDto>>> Process_GetAllWorkCenterSkills(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific WorkCenterSkill.
	/// </summary>
	/// <param name="workCenterSkillId">The Unique Id of the WorkCenterSkill to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the WorkCenterSkill DTO.</returns>
	Task<ERPResponseMessageDto<ERPWorkCenterSkillDto>> Process_GetWorkCenterSkill(Guid workCenterSkillId);
}
