using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Transaction;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom.Transaction;

namespace M1.API.Models.BOM.Transaction;

public interface IBOMMaterialIssueLineModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	Task<BOMResponseMessageDto<IList<BOMMaterialIssueLineDto>>> Process_GetAllMaterialIssueLines(int pageSize, int pageNumber);

	Task<BOMResponseMessageDto<CTMBOMMaterialIssueLineDto>> Process_GetMaterialIssueLines(string materialIssueId);

	/// <summary>
	/// Validates the request to retrieve the material issue lines of a material issue by material issue ID.
	/// </summary>
	/// <param name="materialIssueId">The ID of the material issue to validate.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains validation information.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetMaterialIssue(string materialIssueId);
}
