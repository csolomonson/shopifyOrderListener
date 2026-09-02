using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartRuleModel : ERPBaseModel, IERPPartRuleModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartRules(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartRuleRepository iERPPartRuleRepository = (base.ERPPartRuleRepository = new ERPPartRuleRepository(base.ApiClientContext));
		using (iERPPartRuleRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartRuleRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartRuleRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartRuleRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartRuleRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartRule(Guid partRuleId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartRuleRepository iERPPartRuleRepository = (base.ERPPartRuleRepository = new ERPPartRuleRepository(base.ApiClientContext));
		using (iERPPartRuleRepository)
		{
			if (!(await base.ERPPartRuleRepository.DoesPartRuleExist(partRuleId)))
			{
				errorsList.Add($"PartRule [{partRuleId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartRule(ERPPartRuleDto partRule)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartRuleRepository iERPPartRuleRepository = (base.ERPPartRuleRepository = new ERPPartRuleRepository(base.ApiClientContext));
		using (iERPPartRuleRepository)
		{
			if (!string.IsNullOrWhiteSpace(partRule.pcrMethodID) && !(await base.ERPPartRuleRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partRule.pcrMethodID })))
			{
				errorsList.Add("pcrMethodID [" + partRule.pcrMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partRule.pcrMethodRevisionID) && !(await base.ERPPartRuleRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partRule.pcrMethodID, partRule.pcrMethodRevisionID })))
			{
				errorsList.Add("pcrMethodRevisionID [" + partRule.pcrMethodRevisionID + "] not found.");
			}
			if (partRule.pcrMethodAssemblyID > 0 && !(await base.ERPPartRuleRepository.DoesRecordExistInTableUsingKeys("PartAssemblies", new object[3] { "IMAMETHODID", "IMAMETHODREVISIONID", "IMAMETHODASSEMBLYID" }, new object[3] { partRule.pcrMethodID, partRule.pcrMethodRevisionID, partRule.pcrMethodAssemblyID })))
			{
				errorsList.Add($"pcrMethodAssemblyID [{partRule.pcrMethodAssemblyID}] not found.");
			}
			if (partRule.pcrMethodMaterialID > 0 && !(await base.ERPPartRuleRepository.DoesRecordExistInTableUsingKeys("PartMaterials", new object[4] { "IMMMETHODID", "IMMMETHODREVISIONID", "IMMMETHODASSEMBLYID", "IMMMETHODMATERIALID" }, new object[4] { partRule.pcrMethodID, partRule.pcrMethodRevisionID, partRule.pcrMethodAssemblyID, partRule.pcrMethodMaterialID })))
			{
				errorsList.Add($"pcrMethodMaterialID [{partRule.pcrMethodMaterialID}] not found.");
			}
			if (partRule.pcrMethodOperationID > 0 && !(await base.ERPPartRuleRepository.DoesRecordExistInTableUsingKeys("PartOperations", new object[4] { "IMOMETHODID", "IMOMETHODREVISIONID", "IMOMETHODASSEMBLYID", "IMOMETHODOPERATIONID" }, new object[4] { partRule.pcrMethodID, partRule.pcrMethodRevisionID, partRule.pcrMethodAssemblyID, partRule.pcrMethodOperationID })))
			{
				errorsList.Add($"pcrMethodOperationID [{partRule.pcrMethodOperationID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartRuleDto>>> Process_GetAllPartRules(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartRuleDto> allPartRulesDto = new List<ERPPartRuleDto>();
		ERPResponseMessageDto<IList<ERPPartRuleDto>> result;
		try
		{
			IERPPartRuleRepository iERPPartRuleRepository = (base.ERPPartRuleRepository = new ERPPartRuleRepository(base.ApiClientContext));
			using (iERPPartRuleRepository)
			{
				foreach (ERPPartRuleInformationDto item2 in await base.ERPPartRuleRepository.GetAllPartRules(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartRuleDto item = new ERPPartRuleDto
					{
						pcrCode = item2.pcrCode,
						pcrCreatedBy = item2.pcrCreatedBy,
						pcrCreatedDate = item2.pcrCreatedDate,
						pcrUniqueID = item2.pcrUniqueID,
						pcrField = item2.pcrField,
						pcrMethodAssemblyID = item2.pcrMethodAssemblyID,
						pcrMethodID = item2.pcrMethodID,
						pcrMethodMaterialID = item2.pcrMethodMaterialID,
						pcrMethodOperationID = item2.pcrMethodOperationID,
						pcrMethodRevisionID = item2.pcrMethodRevisionID,
						pcrMethodType = item2.pcrMethodType,
						pcrProcessSequence = item2.pcrProcessSequence,
						pcrRowVersion = item2.pcrRowVersion,
						CustomFields = item2.CustomFields
					};
					allPartRulesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartRules]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartRuleDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartRulesDto,
				RecordCount = allPartRulesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartRuleDto>> Process_GetPartRule(Guid partRuleId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartRuleDto partRuleDto = null;
		ERPResponseMessageDto<ERPPartRuleDto> result;
		try
		{
			IERPPartRuleRepository iERPPartRuleRepository = (base.ERPPartRuleRepository = new ERPPartRuleRepository(base.ApiClientContext));
			using (iERPPartRuleRepository)
			{
				ERPPartRuleInformationDto eRPPartRuleInformationDto = await base.ERPPartRuleRepository.GetPartRule(partRuleId);
				partRuleDto = new ERPPartRuleDto
				{
					pcrCode = eRPPartRuleInformationDto.pcrCode,
					pcrCreatedBy = eRPPartRuleInformationDto.pcrCreatedBy,
					pcrCreatedDate = eRPPartRuleInformationDto.pcrCreatedDate,
					pcrUniqueID = eRPPartRuleInformationDto.pcrUniqueID,
					pcrField = eRPPartRuleInformationDto.pcrField,
					pcrMethodAssemblyID = eRPPartRuleInformationDto.pcrMethodAssemblyID,
					pcrMethodID = eRPPartRuleInformationDto.pcrMethodID,
					pcrMethodMaterialID = eRPPartRuleInformationDto.pcrMethodMaterialID,
					pcrMethodOperationID = eRPPartRuleInformationDto.pcrMethodOperationID,
					pcrMethodRevisionID = eRPPartRuleInformationDto.pcrMethodRevisionID,
					pcrMethodType = eRPPartRuleInformationDto.pcrMethodType,
					pcrProcessSequence = eRPPartRuleInformationDto.pcrProcessSequence,
					pcrRowVersion = eRPPartRuleInformationDto.pcrRowVersion,
					CustomFields = eRPPartRuleInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartRules []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartRuleDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partRuleDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartRuleDto>> Process_PutPartRule(ERPPartRuleDto partRule)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartRuleDto createdObject = null;
		ERPResponseMessageDto<ERPPartRuleDto> result;
		try
		{
			IERPPartRuleRepository iERPPartRuleRepository = (base.ERPPartRuleRepository = new ERPPartRuleRepository(base.ApiClientContext));
			using (iERPPartRuleRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartRuleRepository.SavePartRule(partRule);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartRuleInformationDto eRPPartRuleInformationDto = await base.ERPPartRuleRepository.GetPartRule(partRule.pcrUniqueID);
					createdObject = new ERPPartRuleDto
					{
						pcrCode = eRPPartRuleInformationDto.pcrCode,
						pcrCreatedBy = eRPPartRuleInformationDto.pcrCreatedBy,
						pcrCreatedDate = eRPPartRuleInformationDto.pcrCreatedDate,
						pcrUniqueID = eRPPartRuleInformationDto.pcrUniqueID,
						pcrField = eRPPartRuleInformationDto.pcrField,
						pcrMethodAssemblyID = eRPPartRuleInformationDto.pcrMethodAssemblyID,
						pcrMethodID = eRPPartRuleInformationDto.pcrMethodID,
						pcrMethodMaterialID = eRPPartRuleInformationDto.pcrMethodMaterialID,
						pcrMethodOperationID = eRPPartRuleInformationDto.pcrMethodOperationID,
						pcrMethodRevisionID = eRPPartRuleInformationDto.pcrMethodRevisionID,
						pcrMethodType = eRPPartRuleInformationDto.pcrMethodType,
						pcrProcessSequence = eRPPartRuleInformationDto.pcrProcessSequence,
						pcrRowVersion = eRPPartRuleInformationDto.pcrRowVersion,
						CustomFields = eRPPartRuleInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartRule [{partRule.pcrUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartRuleDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartRule(Guid partRuleId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartRuleRepository iERPPartRuleRepository = (base.ERPPartRuleRepository = new ERPPartRuleRepository(base.ApiClientContext));
		using (iERPPartRuleRepository)
		{
			if (!(await base.ERPPartRuleRepository.DoesPartRuleExist(partRuleId)))
			{
				base.ErrorsList.Add($"PartRule [{partRuleId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartRuleInformationDto eRPPartRuleInformationDto = await base.ERPPartRuleRepository.GetPartRule(partRuleId);
				string text = await base.ERPPartRuleRepository.WhereUsed("PartRules", new object[1] { eRPPartRuleInformationDto.pcrUniqueID }, new object[1] { "pcrUniqueID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartRule cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartRuleDto>> Process_DeletePartRule(Guid partRuleId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartRuleDto> result;
		try
		{
			IERPPartRuleRepository iERPPartRuleRepository = (base.ERPPartRuleRepository = new ERPPartRuleRepository(base.ApiClientContext));
			using (iERPPartRuleRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartRuleRepository.DeleteRowFromTable("PartRules", "pcr", partRuleId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartRule [{partRuleId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartRuleDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartRuleDto()
			};
		}
		return result;
	}
}
