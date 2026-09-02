using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Transaction;
using M1.API.DTOs.Core;
using M1.API.Utilities;

namespace M1.API.Models.BOM.Transaction;

public interface IBOMMaterialIssueModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	Task<APIValidationInfoDto> ValidateRequest_GetMaterialIssue(string materialIssueId);

	Task<APIValidationInfoDto> ValidateRequest_GetMaterialIssue(string materialIssueId, APIClientContext context);

	Task<BOMResponseMessageDto<IList<BOMMaterialIssueDto>>> Process_GetAllMaterialIssues(int pageSize, int pageNumber);

	Task<BOMResponseMessageDto<BOMMaterialIssueDto>> Process_GetMaterialIssue(string materialIssueId);
}
