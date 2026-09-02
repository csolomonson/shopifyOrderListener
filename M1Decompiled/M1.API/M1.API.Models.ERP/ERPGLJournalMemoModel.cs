using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPGLJournalMemoModel : ERPBaseModel, IERPGLJournalMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllGLJournalMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLJournalMemoRepository iERPGLJournalMemoRepository = (base.ERPGLJournalMemoRepository = new ERPGLJournalMemoRepository(base.ApiClientContext));
		using (iERPGLJournalMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPGLJournalMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPGLJournalMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPGLJournalMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPGLJournalMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetGLJournalMemo(Guid gLJournalMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLJournalMemoRepository iERPGLJournalMemoRepository = (base.ERPGLJournalMemoRepository = new ERPGLJournalMemoRepository(base.ApiClientContext));
		using (iERPGLJournalMemoRepository)
		{
			if (!(await base.ERPGLJournalMemoRepository.DoesGLJournalMemoExist(gLJournalMemoId)))
			{
				errorsList.Add($"GLJournalMemo [{gLJournalMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutGLJournalMemo(ERPGLJournalMemoDto gLJournalMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLJournalMemoRepository iERPGLJournalMemoRepository = (base.ERPGLJournalMemoRepository = new ERPGLJournalMemoRepository(base.ApiClientContext));
		using (iERPGLJournalMemoRepository)
		{
			if (gLJournalMemo.glmGlJournalID > 0 && !(await base.ERPGLJournalMemoRepository.DoesRecordExistInTableUsingKeys("GLJournals", new object[1] { "GLPGLJOURNALID" }, new object[1] { gLJournalMemo.glmGlJournalID })))
			{
				errorsList.Add($"glmGlJournalID [{gLJournalMemo.glmGlJournalID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPGLJournalMemoDto>>> Process_GetAllGLJournalMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPGLJournalMemoDto> allGLJournalMemosDto = new List<ERPGLJournalMemoDto>();
		ERPResponseMessageDto<IList<ERPGLJournalMemoDto>> result;
		try
		{
			IERPGLJournalMemoRepository iERPGLJournalMemoRepository = (base.ERPGLJournalMemoRepository = new ERPGLJournalMemoRepository(base.ApiClientContext));
			using (iERPGLJournalMemoRepository)
			{
				foreach (ERPGLJournalMemoInformationDto item2 in await base.ERPGLJournalMemoRepository.GetAllGLJournalMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPGLJournalMemoDto item = new ERPGLJournalMemoDto
					{
						glmCreatedBy = item2.glmCreatedBy,
						glmCreatedDate = item2.glmCreatedDate,
						glmUniqueID = item2.glmUniqueID,
						glmGlJournalID = item2.glmGlJournalID,
						glmClosed = item2.glmClosed,
						glmLongDescriptionRtf = item2.glmLongDescriptionRtf,
						glmLongDescriptionText = item2.glmLongDescriptionText,
						glmMemoDate = item2.glmMemoDate,
						glmRowVersion = item2.glmRowVersion,
						glmGlJournalMemoID = item2.glmGlJournalMemoID,
						glmShortDescription = item2.glmShortDescription,
						CustomFields = item2.CustomFields
					};
					allGLJournalMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all GLJournalMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPGLJournalMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allGLJournalMemosDto,
				RecordCount = allGLJournalMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLJournalMemoDto>> Process_GetGLJournalMemo(Guid gLJournalMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPGLJournalMemoDto gLJournalMemoDto = null;
		ERPResponseMessageDto<ERPGLJournalMemoDto> result;
		try
		{
			IERPGLJournalMemoRepository iERPGLJournalMemoRepository = (base.ERPGLJournalMemoRepository = new ERPGLJournalMemoRepository(base.ApiClientContext));
			using (iERPGLJournalMemoRepository)
			{
				ERPGLJournalMemoInformationDto eRPGLJournalMemoInformationDto = await base.ERPGLJournalMemoRepository.GetGLJournalMemo(gLJournalMemoId);
				gLJournalMemoDto = new ERPGLJournalMemoDto
				{
					glmCreatedBy = eRPGLJournalMemoInformationDto.glmCreatedBy,
					glmCreatedDate = eRPGLJournalMemoInformationDto.glmCreatedDate,
					glmUniqueID = eRPGLJournalMemoInformationDto.glmUniqueID,
					glmGlJournalID = eRPGLJournalMemoInformationDto.glmGlJournalID,
					glmClosed = eRPGLJournalMemoInformationDto.glmClosed,
					glmLongDescriptionRtf = eRPGLJournalMemoInformationDto.glmLongDescriptionRtf,
					glmLongDescriptionText = eRPGLJournalMemoInformationDto.glmLongDescriptionText,
					glmMemoDate = eRPGLJournalMemoInformationDto.glmMemoDate,
					glmRowVersion = eRPGLJournalMemoInformationDto.glmRowVersion,
					glmGlJournalMemoID = eRPGLJournalMemoInformationDto.glmGlJournalMemoID,
					glmShortDescription = eRPGLJournalMemoInformationDto.glmShortDescription,
					CustomFields = eRPGLJournalMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the GLJournalMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLJournalMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = gLJournalMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLJournalMemoDto>> Process_PutGLJournalMemo(ERPGLJournalMemoDto gLJournalMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPGLJournalMemoDto createdObject = null;
		ERPResponseMessageDto<ERPGLJournalMemoDto> result;
		try
		{
			IERPGLJournalMemoRepository iERPGLJournalMemoRepository = (base.ERPGLJournalMemoRepository = new ERPGLJournalMemoRepository(base.ApiClientContext));
			using (iERPGLJournalMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPGLJournalMemoRepository.SaveGLJournalMemo(gLJournalMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPGLJournalMemoInformationDto eRPGLJournalMemoInformationDto = await base.ERPGLJournalMemoRepository.GetGLJournalMemo(gLJournalMemo.glmUniqueID);
					createdObject = new ERPGLJournalMemoDto
					{
						glmCreatedBy = eRPGLJournalMemoInformationDto.glmCreatedBy,
						glmCreatedDate = eRPGLJournalMemoInformationDto.glmCreatedDate,
						glmUniqueID = eRPGLJournalMemoInformationDto.glmUniqueID,
						glmGlJournalID = eRPGLJournalMemoInformationDto.glmGlJournalID,
						glmClosed = eRPGLJournalMemoInformationDto.glmClosed,
						glmLongDescriptionRtf = eRPGLJournalMemoInformationDto.glmLongDescriptionRtf,
						glmLongDescriptionText = eRPGLJournalMemoInformationDto.glmLongDescriptionText,
						glmMemoDate = eRPGLJournalMemoInformationDto.glmMemoDate,
						glmRowVersion = eRPGLJournalMemoInformationDto.glmRowVersion,
						glmGlJournalMemoID = eRPGLJournalMemoInformationDto.glmGlJournalMemoID,
						glmShortDescription = eRPGLJournalMemoInformationDto.glmShortDescription,
						CustomFields = eRPGLJournalMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing GLJournalMemo [{gLJournalMemo.glmUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLJournalMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteGLJournalMemo(Guid gLJournalMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLJournalMemoRepository iERPGLJournalMemoRepository = (base.ERPGLJournalMemoRepository = new ERPGLJournalMemoRepository(base.ApiClientContext));
		using (iERPGLJournalMemoRepository)
		{
			if (!(await base.ERPGLJournalMemoRepository.DoesGLJournalMemoExist(gLJournalMemoId)))
			{
				base.ErrorsList.Add($"GLJournalMemo [{gLJournalMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPGLJournalMemoInformationDto eRPGLJournalMemoInformationDto = await base.ERPGLJournalMemoRepository.GetGLJournalMemo(gLJournalMemoId);
				string text = await base.ERPGLJournalMemoRepository.WhereUsed("GLJournalMemos", new object[2] { eRPGLJournalMemoInformationDto.glmGlJournalID, eRPGLJournalMemoInformationDto.glmGlJournalMemoID }, new object[2] { "glmGlJournalID", "glmGlJournalMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("GLJournalMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPGLJournalMemoDto>> Process_DeleteGLJournalMemo(Guid gLJournalMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPGLJournalMemoDto> result;
		try
		{
			IERPGLJournalMemoRepository iERPGLJournalMemoRepository = (base.ERPGLJournalMemoRepository = new ERPGLJournalMemoRepository(base.ApiClientContext));
			using (iERPGLJournalMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPGLJournalMemoRepository.DeleteRowFromTable("GLJournalMemos", "glm", gLJournalMemoId);
				((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
				IList<string> errorsList = base.ErrorsList;
				if (errorsList != null && errorsList.Count > 0)
				{
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of GLJournalMemo [{gLJournalMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLJournalMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPGLJournalMemoDto()
			};
		}
		return result;
	}
}
