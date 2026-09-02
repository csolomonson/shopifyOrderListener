using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPKnowledgeBasePageModel : ERPBaseModel, IERPKnowledgeBasePageModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllKnowledgeBasePages(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPKnowledgeBasePageRepository iERPKnowledgeBasePageRepository = (base.ERPKnowledgeBasePageRepository = new ERPKnowledgeBasePageRepository(base.ApiClientContext));
		using (iERPKnowledgeBasePageRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPKnowledgeBasePageRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPKnowledgeBasePageRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPKnowledgeBasePageRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPKnowledgeBasePageRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetKnowledgeBasePage(Guid knowledgeBasePageId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPKnowledgeBasePageRepository iERPKnowledgeBasePageRepository = (base.ERPKnowledgeBasePageRepository = new ERPKnowledgeBasePageRepository(base.ApiClientContext));
		using (iERPKnowledgeBasePageRepository)
		{
			if (!(await base.ERPKnowledgeBasePageRepository.DoesKnowledgeBasePageExist(knowledgeBasePageId)))
			{
				errorsList.Add($"KnowledgeBasePage [{knowledgeBasePageId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutKnowledgeBasePage(ERPKnowledgeBasePageDto knowledgeBasePage)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPKnowledgeBasePageRepository iERPKnowledgeBasePageRepository = (base.ERPKnowledgeBasePageRepository = new ERPKnowledgeBasePageRepository(base.ApiClientContext));
		using (iERPKnowledgeBasePageRepository)
		{
			if (!string.IsNullOrWhiteSpace(knowledgeBasePage.kbbPartID) && !(await base.ERPKnowledgeBasePageRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { knowledgeBasePage.kbbPartID })))
			{
				errorsList.Add("kbbPartID [" + knowledgeBasePage.kbbPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(knowledgeBasePage.kbbPartRevisionID) && !(await base.ERPKnowledgeBasePageRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { knowledgeBasePage.kbbPartID, knowledgeBasePage.kbbPartRevisionID })))
			{
				errorsList.Add("kbbPartRevisionID [" + knowledgeBasePage.kbbPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(knowledgeBasePage.kbbResolvedPartID) && !(await base.ERPKnowledgeBasePageRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { knowledgeBasePage.kbbResolvedPartID })))
			{
				errorsList.Add("kbbResolvedPartID [" + knowledgeBasePage.kbbResolvedPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(knowledgeBasePage.kbbResolvedPartRevisionID) && !(await base.ERPKnowledgeBasePageRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { knowledgeBasePage.kbbResolvedPartID, knowledgeBasePage.kbbResolvedPartRevisionID })))
			{
				errorsList.Add("kbbResolvedPartRevisionID [" + knowledgeBasePage.kbbResolvedPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(knowledgeBasePage.kbbOpenedByEmployeeID) && !(await base.ERPKnowledgeBasePageRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { knowledgeBasePage.kbbOpenedByEmployeeID })))
			{
				errorsList.Add("kbbOpenedByEmployeeID [" + knowledgeBasePage.kbbOpenedByEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(knowledgeBasePage.kbbClosedByEmployeeID) && !(await base.ERPKnowledgeBasePageRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { knowledgeBasePage.kbbClosedByEmployeeID })))
			{
				errorsList.Add("kbbClosedByEmployeeID [" + knowledgeBasePage.kbbClosedByEmployeeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPKnowledgeBasePageDto>>> Process_GetAllKnowledgeBasePages(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPKnowledgeBasePageDto> allKnowledgeBasePagesDto = new List<ERPKnowledgeBasePageDto>();
		ERPResponseMessageDto<IList<ERPKnowledgeBasePageDto>> result;
		try
		{
			IERPKnowledgeBasePageRepository iERPKnowledgeBasePageRepository = (base.ERPKnowledgeBasePageRepository = new ERPKnowledgeBasePageRepository(base.ApiClientContext));
			using (iERPKnowledgeBasePageRepository)
			{
				foreach (ERPKnowledgeBasePageInformationDto item2 in await base.ERPKnowledgeBasePageRepository.GetAllKnowledgeBasePages(pageSize, pageNumber, filter, orderBy))
				{
					ERPKnowledgeBasePageDto item = new ERPKnowledgeBasePageDto
					{
						kbbAccessedCount = item2.kbbAccessedCount,
						kbbClosedByEmployeeID = item2.kbbClosedByEmployeeID,
						kbbClosedDate = item2.kbbClosedDate,
						kbbKnowledgeBasePageID = item2.kbbKnowledgeBasePageID,
						kbbCreatedBy = item2.kbbCreatedBy,
						kbbCreatedDate = item2.kbbCreatedDate,
						kbbDescription = item2.kbbDescription,
						kbbUniqueID = item2.kbbUniqueID,
						kbbOpenedByEmployeeID = item2.kbbOpenedByEmployeeID,
						kbbOpenedDate = item2.kbbOpenedDate,
						kbbPartID = item2.kbbPartID,
						kbbPartRevisionID = item2.kbbPartRevisionID,
						kbbProblemDescriptionRtf = item2.kbbProblemDescriptionRtf,
						kbbProblemDescriptionText = item2.kbbProblemDescriptionText,
						kbbResolutionDescriptionRtf = item2.kbbResolutionDescriptionRtf,
						kbbResolutionDescriptionText = item2.kbbResolutionDescriptionText,
						kbbResolvedPartID = item2.kbbResolvedPartID,
						kbbResolvedPartRevisionID = item2.kbbResolvedPartRevisionID,
						kbbRowVersion = item2.kbbRowVersion,
						kbbStatus = item2.kbbStatus,
						kbbWorkAroundDescriptionRtf = item2.kbbWorkAroundDescriptionRtf,
						kbbWorkAroundDescriptionText = item2.kbbWorkAroundDescriptionText,
						CustomFields = item2.CustomFields
					};
					allKnowledgeBasePagesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all KnowledgeBasePages]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPKnowledgeBasePageDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allKnowledgeBasePagesDto,
				RecordCount = allKnowledgeBasePagesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPKnowledgeBasePageDto>> Process_GetKnowledgeBasePage(Guid knowledgeBasePageId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPKnowledgeBasePageDto knowledgeBasePageDto = null;
		ERPResponseMessageDto<ERPKnowledgeBasePageDto> result;
		try
		{
			IERPKnowledgeBasePageRepository iERPKnowledgeBasePageRepository = (base.ERPKnowledgeBasePageRepository = new ERPKnowledgeBasePageRepository(base.ApiClientContext));
			using (iERPKnowledgeBasePageRepository)
			{
				ERPKnowledgeBasePageInformationDto eRPKnowledgeBasePageInformationDto = await base.ERPKnowledgeBasePageRepository.GetKnowledgeBasePage(knowledgeBasePageId);
				knowledgeBasePageDto = new ERPKnowledgeBasePageDto
				{
					kbbAccessedCount = eRPKnowledgeBasePageInformationDto.kbbAccessedCount,
					kbbClosedByEmployeeID = eRPKnowledgeBasePageInformationDto.kbbClosedByEmployeeID,
					kbbClosedDate = eRPKnowledgeBasePageInformationDto.kbbClosedDate,
					kbbKnowledgeBasePageID = eRPKnowledgeBasePageInformationDto.kbbKnowledgeBasePageID,
					kbbCreatedBy = eRPKnowledgeBasePageInformationDto.kbbCreatedBy,
					kbbCreatedDate = eRPKnowledgeBasePageInformationDto.kbbCreatedDate,
					kbbDescription = eRPKnowledgeBasePageInformationDto.kbbDescription,
					kbbUniqueID = eRPKnowledgeBasePageInformationDto.kbbUniqueID,
					kbbOpenedByEmployeeID = eRPKnowledgeBasePageInformationDto.kbbOpenedByEmployeeID,
					kbbOpenedDate = eRPKnowledgeBasePageInformationDto.kbbOpenedDate,
					kbbPartID = eRPKnowledgeBasePageInformationDto.kbbPartID,
					kbbPartRevisionID = eRPKnowledgeBasePageInformationDto.kbbPartRevisionID,
					kbbProblemDescriptionRtf = eRPKnowledgeBasePageInformationDto.kbbProblemDescriptionRtf,
					kbbProblemDescriptionText = eRPKnowledgeBasePageInformationDto.kbbProblemDescriptionText,
					kbbResolutionDescriptionRtf = eRPKnowledgeBasePageInformationDto.kbbResolutionDescriptionRtf,
					kbbResolutionDescriptionText = eRPKnowledgeBasePageInformationDto.kbbResolutionDescriptionText,
					kbbResolvedPartID = eRPKnowledgeBasePageInformationDto.kbbResolvedPartID,
					kbbResolvedPartRevisionID = eRPKnowledgeBasePageInformationDto.kbbResolvedPartRevisionID,
					kbbRowVersion = eRPKnowledgeBasePageInformationDto.kbbRowVersion,
					kbbStatus = eRPKnowledgeBasePageInformationDto.kbbStatus,
					kbbWorkAroundDescriptionRtf = eRPKnowledgeBasePageInformationDto.kbbWorkAroundDescriptionRtf,
					kbbWorkAroundDescriptionText = eRPKnowledgeBasePageInformationDto.kbbWorkAroundDescriptionText,
					CustomFields = eRPKnowledgeBasePageInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the KnowledgeBasePages []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPKnowledgeBasePageDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = knowledgeBasePageDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPKnowledgeBasePageDto>> Process_PutKnowledgeBasePage(ERPKnowledgeBasePageDto knowledgeBasePage)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPKnowledgeBasePageDto createdObject = null;
		ERPResponseMessageDto<ERPKnowledgeBasePageDto> result;
		try
		{
			IERPKnowledgeBasePageRepository iERPKnowledgeBasePageRepository = (base.ERPKnowledgeBasePageRepository = new ERPKnowledgeBasePageRepository(base.ApiClientContext));
			using (iERPKnowledgeBasePageRepository)
			{
				APIValidationInfoDto postResult = await base.ERPKnowledgeBasePageRepository.SaveKnowledgeBasePage(knowledgeBasePage);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPKnowledgeBasePageInformationDto eRPKnowledgeBasePageInformationDto = await base.ERPKnowledgeBasePageRepository.GetKnowledgeBasePage(knowledgeBasePage.kbbUniqueID);
					createdObject = new ERPKnowledgeBasePageDto
					{
						kbbAccessedCount = eRPKnowledgeBasePageInformationDto.kbbAccessedCount,
						kbbClosedByEmployeeID = eRPKnowledgeBasePageInformationDto.kbbClosedByEmployeeID,
						kbbClosedDate = eRPKnowledgeBasePageInformationDto.kbbClosedDate,
						kbbKnowledgeBasePageID = eRPKnowledgeBasePageInformationDto.kbbKnowledgeBasePageID,
						kbbCreatedBy = eRPKnowledgeBasePageInformationDto.kbbCreatedBy,
						kbbCreatedDate = eRPKnowledgeBasePageInformationDto.kbbCreatedDate,
						kbbDescription = eRPKnowledgeBasePageInformationDto.kbbDescription,
						kbbUniqueID = eRPKnowledgeBasePageInformationDto.kbbUniqueID,
						kbbOpenedByEmployeeID = eRPKnowledgeBasePageInformationDto.kbbOpenedByEmployeeID,
						kbbOpenedDate = eRPKnowledgeBasePageInformationDto.kbbOpenedDate,
						kbbPartID = eRPKnowledgeBasePageInformationDto.kbbPartID,
						kbbPartRevisionID = eRPKnowledgeBasePageInformationDto.kbbPartRevisionID,
						kbbProblemDescriptionRtf = eRPKnowledgeBasePageInformationDto.kbbProblemDescriptionRtf,
						kbbProblemDescriptionText = eRPKnowledgeBasePageInformationDto.kbbProblemDescriptionText,
						kbbResolutionDescriptionRtf = eRPKnowledgeBasePageInformationDto.kbbResolutionDescriptionRtf,
						kbbResolutionDescriptionText = eRPKnowledgeBasePageInformationDto.kbbResolutionDescriptionText,
						kbbResolvedPartID = eRPKnowledgeBasePageInformationDto.kbbResolvedPartID,
						kbbResolvedPartRevisionID = eRPKnowledgeBasePageInformationDto.kbbResolvedPartRevisionID,
						kbbRowVersion = eRPKnowledgeBasePageInformationDto.kbbRowVersion,
						kbbStatus = eRPKnowledgeBasePageInformationDto.kbbStatus,
						kbbWorkAroundDescriptionRtf = eRPKnowledgeBasePageInformationDto.kbbWorkAroundDescriptionRtf,
						kbbWorkAroundDescriptionText = eRPKnowledgeBasePageInformationDto.kbbWorkAroundDescriptionText,
						CustomFields = eRPKnowledgeBasePageInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing KnowledgeBasePage [{knowledgeBasePage.kbbUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPKnowledgeBasePageDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteKnowledgeBasePage(Guid knowledgeBasePageId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPKnowledgeBasePageRepository iERPKnowledgeBasePageRepository = (base.ERPKnowledgeBasePageRepository = new ERPKnowledgeBasePageRepository(base.ApiClientContext));
		using (iERPKnowledgeBasePageRepository)
		{
			if (!(await base.ERPKnowledgeBasePageRepository.DoesKnowledgeBasePageExist(knowledgeBasePageId)))
			{
				base.ErrorsList.Add($"KnowledgeBasePage [{knowledgeBasePageId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPKnowledgeBasePageInformationDto eRPKnowledgeBasePageInformationDto = await base.ERPKnowledgeBasePageRepository.GetKnowledgeBasePage(knowledgeBasePageId);
				string text = await base.ERPKnowledgeBasePageRepository.WhereUsed("KnowledgeBasePages", new object[1] { eRPKnowledgeBasePageInformationDto.kbbKnowledgeBasePageID }, new object[1] { "kbbKnowledgeBasePageID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("KnowledgeBasePage cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPKnowledgeBasePageDto>> Process_DeleteKnowledgeBasePage(Guid knowledgeBasePageId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPKnowledgeBasePageDto> result;
		try
		{
			IERPKnowledgeBasePageRepository iERPKnowledgeBasePageRepository = (base.ERPKnowledgeBasePageRepository = new ERPKnowledgeBasePageRepository(base.ApiClientContext));
			using (iERPKnowledgeBasePageRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPKnowledgeBasePageRepository.DeleteRowFromTable("KnowledgeBasePages", "kbb", knowledgeBasePageId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of KnowledgeBasePage [{knowledgeBasePageId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPKnowledgeBasePageDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPKnowledgeBasePageDto()
			};
		}
		return result;
	}
}
