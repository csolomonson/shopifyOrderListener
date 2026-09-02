using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPMaterialIssueModel : ERPBaseModel, IERPMaterialIssueModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllMaterialIssues(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPMaterialIssueRepository iERPMaterialIssueRepository = (base.ERPMaterialIssueRepository = new ERPMaterialIssueRepository(base.ApiClientContext));
		using (iERPMaterialIssueRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPMaterialIssueRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPMaterialIssueRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPMaterialIssueRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPMaterialIssueRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetMaterialIssue(Guid materialIssueId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMaterialIssueRepository iERPMaterialIssueRepository = (base.ERPMaterialIssueRepository = new ERPMaterialIssueRepository(base.ApiClientContext));
		using (iERPMaterialIssueRepository)
		{
			if (!(await base.ERPMaterialIssueRepository.DoesMaterialIssueExist(materialIssueId)))
			{
				errorsList.Add($"MaterialIssue [{materialIssueId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutMaterialIssue(ERPMaterialIssueDto materialIssue)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPMaterialIssueRepository iERPMaterialIssueRepository = (base.ERPMaterialIssueRepository = new ERPMaterialIssueRepository(base.ApiClientContext));
		using (iERPMaterialIssueRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPMaterialIssueDto>>> Process_GetAllMaterialIssues(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPMaterialIssueDto> allMaterialIssuesDto = new List<ERPMaterialIssueDto>();
		ERPResponseMessageDto<IList<ERPMaterialIssueDto>> result;
		try
		{
			IERPMaterialIssueRepository iERPMaterialIssueRepository = (base.ERPMaterialIssueRepository = new ERPMaterialIssueRepository(base.ApiClientContext));
			using (iERPMaterialIssueRepository)
			{
				foreach (ERPMaterialIssueInformationDto item2 in await base.ERPMaterialIssueRepository.GetAllMaterialIssues(pageSize, pageNumber, filter, orderBy))
				{
					ERPMaterialIssueDto item = new ERPMaterialIssueDto
					{
						iniMaterialIssueID = item2.iniMaterialIssueID,
						iniCreatedBy = item2.iniCreatedBy,
						iniCreatedDate = item2.iniCreatedDate,
						iniUniqueID = item2.iniUniqueID,
						iniPosted = item2.iniPosted,
						iniReversalEntry = item2.iniReversalEntry,
						iniReversed = item2.iniReversed,
						iniMaterialIssueDate = item2.iniMaterialIssueDate,
						iniPostedDate = item2.iniPostedDate,
						iniRowVersion = item2.iniRowVersion,
						iniSourceTableUniqueID = item2.iniSourceTableUniqueID,
						CustomFields = item2.CustomFields
					};
					allMaterialIssuesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all MaterialIssues]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPMaterialIssueDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allMaterialIssuesDto,
				RecordCount = allMaterialIssuesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMaterialIssueDto>> Process_GetMaterialIssue(Guid materialIssueId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPMaterialIssueDto materialIssueDto = null;
		ERPResponseMessageDto<ERPMaterialIssueDto> result;
		try
		{
			IERPMaterialIssueRepository iERPMaterialIssueRepository = (base.ERPMaterialIssueRepository = new ERPMaterialIssueRepository(base.ApiClientContext));
			using (iERPMaterialIssueRepository)
			{
				ERPMaterialIssueInformationDto eRPMaterialIssueInformationDto = await base.ERPMaterialIssueRepository.GetMaterialIssue(materialIssueId);
				materialIssueDto = new ERPMaterialIssueDto
				{
					iniMaterialIssueID = eRPMaterialIssueInformationDto.iniMaterialIssueID,
					iniCreatedBy = eRPMaterialIssueInformationDto.iniCreatedBy,
					iniCreatedDate = eRPMaterialIssueInformationDto.iniCreatedDate,
					iniUniqueID = eRPMaterialIssueInformationDto.iniUniqueID,
					iniPosted = eRPMaterialIssueInformationDto.iniPosted,
					iniReversalEntry = eRPMaterialIssueInformationDto.iniReversalEntry,
					iniReversed = eRPMaterialIssueInformationDto.iniReversed,
					iniMaterialIssueDate = eRPMaterialIssueInformationDto.iniMaterialIssueDate,
					iniPostedDate = eRPMaterialIssueInformationDto.iniPostedDate,
					iniRowVersion = eRPMaterialIssueInformationDto.iniRowVersion,
					iniSourceTableUniqueID = eRPMaterialIssueInformationDto.iniSourceTableUniqueID,
					CustomFields = eRPMaterialIssueInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the MaterialIssues []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMaterialIssueDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = materialIssueDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMaterialIssueDto>> Process_PutMaterialIssue(ERPMaterialIssueDto materialIssue)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPMaterialIssueDto createdObject = null;
		ERPResponseMessageDto<ERPMaterialIssueDto> result;
		try
		{
			IERPMaterialIssueRepository iERPMaterialIssueRepository = (base.ERPMaterialIssueRepository = new ERPMaterialIssueRepository(base.ApiClientContext));
			using (iERPMaterialIssueRepository)
			{
				APIValidationInfoDto postResult = await base.ERPMaterialIssueRepository.SaveMaterialIssue(materialIssue);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPMaterialIssueInformationDto eRPMaterialIssueInformationDto = await base.ERPMaterialIssueRepository.GetMaterialIssue(materialIssue.iniUniqueID);
					createdObject = new ERPMaterialIssueDto
					{
						iniMaterialIssueID = eRPMaterialIssueInformationDto.iniMaterialIssueID,
						iniCreatedBy = eRPMaterialIssueInformationDto.iniCreatedBy,
						iniCreatedDate = eRPMaterialIssueInformationDto.iniCreatedDate,
						iniUniqueID = eRPMaterialIssueInformationDto.iniUniqueID,
						iniPosted = eRPMaterialIssueInformationDto.iniPosted,
						iniReversalEntry = eRPMaterialIssueInformationDto.iniReversalEntry,
						iniReversed = eRPMaterialIssueInformationDto.iniReversed,
						iniMaterialIssueDate = eRPMaterialIssueInformationDto.iniMaterialIssueDate,
						iniPostedDate = eRPMaterialIssueInformationDto.iniPostedDate,
						iniRowVersion = eRPMaterialIssueInformationDto.iniRowVersion,
						iniSourceTableUniqueID = eRPMaterialIssueInformationDto.iniSourceTableUniqueID,
						CustomFields = eRPMaterialIssueInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing MaterialIssue [{materialIssue.iniUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMaterialIssueDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteMaterialIssue(Guid materialIssueId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMaterialIssueRepository iERPMaterialIssueRepository = (base.ERPMaterialIssueRepository = new ERPMaterialIssueRepository(base.ApiClientContext));
		using (iERPMaterialIssueRepository)
		{
			if (!(await base.ERPMaterialIssueRepository.DoesMaterialIssueExist(materialIssueId)))
			{
				base.ErrorsList.Add($"MaterialIssue [{materialIssueId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPMaterialIssueInformationDto eRPMaterialIssueInformationDto = await base.ERPMaterialIssueRepository.GetMaterialIssue(materialIssueId);
				string text = await base.ERPMaterialIssueRepository.WhereUsed("MaterialIssues", new object[1] { eRPMaterialIssueInformationDto.iniMaterialIssueID }, new object[1] { "iniMaterialIssueID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("MaterialIssue cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPMaterialIssueDto>> Process_DeleteMaterialIssue(Guid materialIssueId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPMaterialIssueDto> result;
		try
		{
			IERPMaterialIssueRepository iERPMaterialIssueRepository = (base.ERPMaterialIssueRepository = new ERPMaterialIssueRepository(base.ApiClientContext));
			using (iERPMaterialIssueRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPMaterialIssueRepository.DeleteRowFromTable("MaterialIssues", "ini", materialIssueId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of MaterialIssue [{materialIssueId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMaterialIssueDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPMaterialIssueDto()
			};
		}
		return result;
	}
}
