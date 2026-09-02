using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Transaction;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Repositories.Core.Transaction;
using M1.API.Utilities;

namespace M1.API.Models.BOM.Transaction;

public class BOMMaterialIssueModel : BOMBaseModel, IBOMMaterialIssueModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public IDictionary<string, object> MaterialIssueKeyDictionary { get; set; }

	public BOMMaterialIssueModel()
	{
		MaterialIssueKeyDictionary = new Dictionary<string, object>();
	}

	public async Task<BOMResponseMessageDto<IList<BOMMaterialIssueDto>>> Process_GetAllMaterialIssues(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IList<BOMMaterialIssueDto> list = new List<BOMMaterialIssueDto>();
		BOMResponseMessageDto<IList<BOMMaterialIssueDto>> result;
		try
		{
			using MaterialIssueRepository materialIssueRepository = new MaterialIssueRepository(base.ApiClientContext);
			foreach (MaterialIssueDto item2 in materialIssueRepository.GetAllMaterialIssues(pageSize, pageNumber).Result)
			{
				BOMMaterialIssueDto item = new BOMMaterialIssueDto
				{
					MaterialIssueID = item2.MaterialIssueID,
					CreatedBy = item2.CreatedBy,
					CreatedDate = item2.CreatedDate,
					UniqueID = item2.UniqueID,
					Posted = item2.Posted,
					ReversalEntry = item2.ReversalEntry,
					Reversed = item2.Reversed,
					MaterialIssueDate = item2.MaterialIssueDate,
					PostedDate = item2.PostedDate,
					RowVersion = item2.RowVersion
				};
				list.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all MaterialIssues]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<IList<BOMMaterialIssueDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = list
			};
		}
		return result;
	}

	public Task<APIValidationInfoDto> ValidateRequest_GetMaterialIssue(string materialIssueId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		APIValidationInfoDto aPIValidationInfoDto = null;
		try
		{
			string result = GetM1MaterialIssueIdFromGuid(materialIssueId).Result;
			if (string.IsNullOrWhiteSpace(result))
			{
				base.ErrorsList.Add("Invalid material issue Id/Guid");
			}
			if (base.ErrorsList.Count > 0)
			{
				httpValidationStatusCode = HttpStatusCode.BadRequest;
			}
			else
			{
				MaterialIssueKeyDictionary.Add("iniMaterialIssueID", result);
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the parameters]");
			throw;
		}
		finally
		{
			aPIValidationInfoDto = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(aPIValidationInfoDto);
	}

	private async Task<string> GetM1MaterialIssueIdFromGuid(string materialIssueId)
	{
		using (MaterialIssueRepository materialIssueRepository = new MaterialIssueRepository(base.ApiClientContext))
		{
			if (Guid.TryParse(materialIssueId, out var _))
			{
				return materialIssueRepository.GetMaterialIssueIdFromGuid(materialIssueId).Result;
			}
			if (materialIssueRepository.DoesMaterialIssueExists(materialIssueId).Result)
			{
				return materialIssueId;
			}
		}
		return string.Empty;
	}

	public async Task<BOMResponseMessageDto<BOMMaterialIssueDto>> Process_GetMaterialIssue(string materialIssueId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		BOMMaterialIssueDto returnObject = null;
		BOMResponseMessageDto<BOMMaterialIssueDto> result2;
		try
		{
			using MaterialIssueRepository materialIssueRepository = new MaterialIssueRepository(base.ApiClientContext);
			MaterialIssueDto result = materialIssueRepository.GetMaterialIssue(materialIssueId).Result;
			returnObject = new BOMMaterialIssueDto
			{
				MaterialIssueID = result.MaterialIssueID,
				CreatedBy = result.CreatedBy,
				CreatedDate = result.CreatedDate,
				UniqueID = result.UniqueID,
				Posted = result.Posted,
				ReversalEntry = result.ReversalEntry,
				Reversed = result.Reversed,
				MaterialIssueDate = result.MaterialIssueDate,
				PostedDate = result.PostedDate,
				RowVersion = result.RowVersion
			};
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Material Issue [" + materialIssueId + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result2 = new BOMResponseMessageDto<BOMMaterialIssueDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = returnObject
			};
		}
		return result2;
	}

	public Task<APIValidationInfoDto> ValidateRequest_GetMaterialIssue(string materialIssueId, APIClientContext context)
	{
		if (base.ApiClientContext == null)
		{
			base.ApiClientContext = context;
		}
		return ValidateRequest_GetMaterialIssue(materialIssueId);
	}
}
