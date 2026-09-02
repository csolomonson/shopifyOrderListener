using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartGroupModel : ERPBaseModel, IERPPartGroupModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartGroups(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartGroupRepository iERPPartGroupRepository = (base.ERPPartGroupRepository = new ERPPartGroupRepository(base.ApiClientContext));
		using (iERPPartGroupRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartGroupRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartGroupRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartGroupRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartGroupRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartGroup(Guid partGroupId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartGroupRepository iERPPartGroupRepository = (base.ERPPartGroupRepository = new ERPPartGroupRepository(base.ApiClientContext));
		using (iERPPartGroupRepository)
		{
			if (!(await base.ERPPartGroupRepository.DoesPartGroupExist(partGroupId)))
			{
				errorsList.Add($"PartGroup [{partGroupId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartGroup(ERPPartGroupDto partGroup)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartGroupRepository iERPPartGroupRepository = (base.ERPPartGroupRepository = new ERPPartGroupRepository(base.ApiClientContext));
		using (iERPPartGroupRepository)
		{
			if (!string.IsNullOrWhiteSpace(partGroup.imuSalesGlAccountID) && !(await base.ERPPartGroupRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { partGroup.imuSalesGlAccountID })))
			{
				errorsList.Add("imuSalesGlAccountID [" + partGroup.imuSalesGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partGroup.imuDiscountGlAccountID) && !(await base.ERPPartGroupRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { partGroup.imuDiscountGlAccountID })))
			{
				errorsList.Add("imuDiscountGlAccountID [" + partGroup.imuDiscountGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partGroup.imuParentPartGroupID) && !(await base.ERPPartGroupRepository.DoesRecordExistInTableUsingKeys("PartGroups", new object[1] { "IMUPARTGROUPID" }, new object[1] { partGroup.imuParentPartGroupID })))
			{
				errorsList.Add("imuParentPartGroupID [" + partGroup.imuParentPartGroupID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partGroup.imuCogsLaborGlAccountID) && !(await base.ERPPartGroupRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { partGroup.imuCogsLaborGlAccountID })))
			{
				errorsList.Add("imuCogsLaborGlAccountID [" + partGroup.imuCogsLaborGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partGroup.imuCogsMaterialGlAccountID) && !(await base.ERPPartGroupRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { partGroup.imuCogsMaterialGlAccountID })))
			{
				errorsList.Add("imuCogsMaterialGlAccountID [" + partGroup.imuCogsMaterialGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partGroup.imuCogsSubcontractGlAccountID) && !(await base.ERPPartGroupRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { partGroup.imuCogsSubcontractGlAccountID })))
			{
				errorsList.Add("imuCogsSubcontractGlAccountID [" + partGroup.imuCogsSubcontractGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partGroup.imuCogsOverheadGlAccountID) && !(await base.ERPPartGroupRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { partGroup.imuCogsOverheadGlAccountID })))
			{
				errorsList.Add("imuCogsOverheadGlAccountID [" + partGroup.imuCogsOverheadGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partGroup.imuArDepositGlAccountID) && !(await base.ERPPartGroupRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { partGroup.imuArDepositGlAccountID })))
			{
				errorsList.Add("imuArDepositGlAccountID [" + partGroup.imuArDepositGlAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartGroupDto>>> Process_GetAllPartGroups(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartGroupDto> allPartGroupsDto = new List<ERPPartGroupDto>();
		ERPResponseMessageDto<IList<ERPPartGroupDto>> result;
		try
		{
			IERPPartGroupRepository iERPPartGroupRepository = (base.ERPPartGroupRepository = new ERPPartGroupRepository(base.ApiClientContext));
			using (iERPPartGroupRepository)
			{
				foreach (ERPPartGroupInformationDto item2 in await base.ERPPartGroupRepository.GetAllPartGroups(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartGroupDto item = new ERPPartGroupDto
					{
						imuArDepositGlAccountID = item2.imuArDepositGlAccountID,
						imuAvalaraTaxCodeID = item2.imuAvalaraTaxCodeID,
						imuPartGroupID = item2.imuPartGroupID,
						imuCogsLaborGlAccountID = item2.imuCogsLaborGlAccountID,
						imuCogsMaterialGlAccountID = item2.imuCogsMaterialGlAccountID,
						imuCogsOverheadGlAccountID = item2.imuCogsOverheadGlAccountID,
						imuCogsSubcontractGlAccountID = item2.imuCogsSubcontractGlAccountID,
						imuCommissionRate = item2.imuCommissionRate,
						imuCommissionType = item2.imuCommissionType,
						imuCreatedBy = item2.imuCreatedBy,
						imuCreatedDate = item2.imuCreatedDate,
						imuDescription = item2.imuDescription,
						imuDiscountGlAccountID = item2.imuDiscountGlAccountID,
						imuUniqueID = item2.imuUniqueID,
						imuInactiveDate = item2.imuInactiveDate,
						imuInactive = item2.imuInactive,
						imuNextSerialNumberIDFormula = item2.imuNextSerialNumberIDFormula,
						imuNextSerialNumberOption = item2.imuNextSerialNumberOption,
						imuNextSerialNumberValue = item2.imuNextSerialNumberValue,
						imuParentPartGroupID = item2.imuParentPartGroupID,
						imuPartImageFileName = item2.imuPartImageFileName,
						imuQmLaborMarkup = item2.imuQmLaborMarkup,
						imuQmMarkupOption = item2.imuQmMarkupOption,
						imuQmMaterialMarkup = item2.imuQmMaterialMarkup,
						imuQmOverHeadMarkup = item2.imuQmOverHeadMarkup,
						imuQmPurchaseToOrderMarkup = item2.imuQmPurchaseToOrderMarkup,
						imuQmQuoteMarkupType = item2.imuQmQuoteMarkupType,
						imuQmQuotingMarkup = item2.imuQmQuotingMarkup,
						imuQmSubcontractMarkup = item2.imuQmSubcontractMarkup,
						imuRowVersion = item2.imuRowVersion,
						imuSalesGlAccountID = item2.imuSalesGlAccountID,
						CustomFields = item2.CustomFields
					};
					allPartGroupsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartGroups]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartGroupDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartGroupsDto,
				RecordCount = allPartGroupsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartGroupDto>> Process_GetPartGroup(Guid partGroupId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartGroupDto partGroupDto = null;
		ERPResponseMessageDto<ERPPartGroupDto> result;
		try
		{
			IERPPartGroupRepository iERPPartGroupRepository = (base.ERPPartGroupRepository = new ERPPartGroupRepository(base.ApiClientContext));
			using (iERPPartGroupRepository)
			{
				ERPPartGroupInformationDto eRPPartGroupInformationDto = await base.ERPPartGroupRepository.GetPartGroup(partGroupId);
				partGroupDto = new ERPPartGroupDto
				{
					imuArDepositGlAccountID = eRPPartGroupInformationDto.imuArDepositGlAccountID,
					imuAvalaraTaxCodeID = eRPPartGroupInformationDto.imuAvalaraTaxCodeID,
					imuPartGroupID = eRPPartGroupInformationDto.imuPartGroupID,
					imuCogsLaborGlAccountID = eRPPartGroupInformationDto.imuCogsLaborGlAccountID,
					imuCogsMaterialGlAccountID = eRPPartGroupInformationDto.imuCogsMaterialGlAccountID,
					imuCogsOverheadGlAccountID = eRPPartGroupInformationDto.imuCogsOverheadGlAccountID,
					imuCogsSubcontractGlAccountID = eRPPartGroupInformationDto.imuCogsSubcontractGlAccountID,
					imuCommissionRate = eRPPartGroupInformationDto.imuCommissionRate,
					imuCommissionType = eRPPartGroupInformationDto.imuCommissionType,
					imuCreatedBy = eRPPartGroupInformationDto.imuCreatedBy,
					imuCreatedDate = eRPPartGroupInformationDto.imuCreatedDate,
					imuDescription = eRPPartGroupInformationDto.imuDescription,
					imuDiscountGlAccountID = eRPPartGroupInformationDto.imuDiscountGlAccountID,
					imuUniqueID = eRPPartGroupInformationDto.imuUniqueID,
					imuInactiveDate = eRPPartGroupInformationDto.imuInactiveDate,
					imuInactive = eRPPartGroupInformationDto.imuInactive,
					imuNextSerialNumberIDFormula = eRPPartGroupInformationDto.imuNextSerialNumberIDFormula,
					imuNextSerialNumberOption = eRPPartGroupInformationDto.imuNextSerialNumberOption,
					imuNextSerialNumberValue = eRPPartGroupInformationDto.imuNextSerialNumberValue,
					imuParentPartGroupID = eRPPartGroupInformationDto.imuParentPartGroupID,
					imuPartImageFileName = eRPPartGroupInformationDto.imuPartImageFileName,
					imuQmLaborMarkup = eRPPartGroupInformationDto.imuQmLaborMarkup,
					imuQmMarkupOption = eRPPartGroupInformationDto.imuQmMarkupOption,
					imuQmMaterialMarkup = eRPPartGroupInformationDto.imuQmMaterialMarkup,
					imuQmOverHeadMarkup = eRPPartGroupInformationDto.imuQmOverHeadMarkup,
					imuQmPurchaseToOrderMarkup = eRPPartGroupInformationDto.imuQmPurchaseToOrderMarkup,
					imuQmQuoteMarkupType = eRPPartGroupInformationDto.imuQmQuoteMarkupType,
					imuQmQuotingMarkup = eRPPartGroupInformationDto.imuQmQuotingMarkup,
					imuQmSubcontractMarkup = eRPPartGroupInformationDto.imuQmSubcontractMarkup,
					imuRowVersion = eRPPartGroupInformationDto.imuRowVersion,
					imuSalesGlAccountID = eRPPartGroupInformationDto.imuSalesGlAccountID,
					CustomFields = eRPPartGroupInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartGroups []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartGroupDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partGroupDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartGroupDto>> Process_PutPartGroup(ERPPartGroupDto partGroup)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartGroupDto createdObject = null;
		ERPResponseMessageDto<ERPPartGroupDto> result;
		try
		{
			IERPPartGroupRepository iERPPartGroupRepository = (base.ERPPartGroupRepository = new ERPPartGroupRepository(base.ApiClientContext));
			using (iERPPartGroupRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartGroupRepository.SavePartGroup(partGroup);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartGroupInformationDto eRPPartGroupInformationDto = await base.ERPPartGroupRepository.GetPartGroup(partGroup.imuUniqueID);
					createdObject = new ERPPartGroupDto
					{
						imuArDepositGlAccountID = eRPPartGroupInformationDto.imuArDepositGlAccountID,
						imuAvalaraTaxCodeID = eRPPartGroupInformationDto.imuAvalaraTaxCodeID,
						imuPartGroupID = eRPPartGroupInformationDto.imuPartGroupID,
						imuCogsLaborGlAccountID = eRPPartGroupInformationDto.imuCogsLaborGlAccountID,
						imuCogsMaterialGlAccountID = eRPPartGroupInformationDto.imuCogsMaterialGlAccountID,
						imuCogsOverheadGlAccountID = eRPPartGroupInformationDto.imuCogsOverheadGlAccountID,
						imuCogsSubcontractGlAccountID = eRPPartGroupInformationDto.imuCogsSubcontractGlAccountID,
						imuCommissionRate = eRPPartGroupInformationDto.imuCommissionRate,
						imuCommissionType = eRPPartGroupInformationDto.imuCommissionType,
						imuCreatedBy = eRPPartGroupInformationDto.imuCreatedBy,
						imuCreatedDate = eRPPartGroupInformationDto.imuCreatedDate,
						imuDescription = eRPPartGroupInformationDto.imuDescription,
						imuDiscountGlAccountID = eRPPartGroupInformationDto.imuDiscountGlAccountID,
						imuUniqueID = eRPPartGroupInformationDto.imuUniqueID,
						imuInactiveDate = eRPPartGroupInformationDto.imuInactiveDate,
						imuInactive = eRPPartGroupInformationDto.imuInactive,
						imuNextSerialNumberIDFormula = eRPPartGroupInformationDto.imuNextSerialNumberIDFormula,
						imuNextSerialNumberOption = eRPPartGroupInformationDto.imuNextSerialNumberOption,
						imuNextSerialNumberValue = eRPPartGroupInformationDto.imuNextSerialNumberValue,
						imuParentPartGroupID = eRPPartGroupInformationDto.imuParentPartGroupID,
						imuPartImageFileName = eRPPartGroupInformationDto.imuPartImageFileName,
						imuQmLaborMarkup = eRPPartGroupInformationDto.imuQmLaborMarkup,
						imuQmMarkupOption = eRPPartGroupInformationDto.imuQmMarkupOption,
						imuQmMaterialMarkup = eRPPartGroupInformationDto.imuQmMaterialMarkup,
						imuQmOverHeadMarkup = eRPPartGroupInformationDto.imuQmOverHeadMarkup,
						imuQmPurchaseToOrderMarkup = eRPPartGroupInformationDto.imuQmPurchaseToOrderMarkup,
						imuQmQuoteMarkupType = eRPPartGroupInformationDto.imuQmQuoteMarkupType,
						imuQmQuotingMarkup = eRPPartGroupInformationDto.imuQmQuotingMarkup,
						imuQmSubcontractMarkup = eRPPartGroupInformationDto.imuQmSubcontractMarkup,
						imuRowVersion = eRPPartGroupInformationDto.imuRowVersion,
						imuSalesGlAccountID = eRPPartGroupInformationDto.imuSalesGlAccountID,
						CustomFields = eRPPartGroupInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartGroup [{partGroup.imuUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartGroupDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartGroup(Guid partGroupId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartGroupRepository iERPPartGroupRepository = (base.ERPPartGroupRepository = new ERPPartGroupRepository(base.ApiClientContext));
		using (iERPPartGroupRepository)
		{
			if (!(await base.ERPPartGroupRepository.DoesPartGroupExist(partGroupId)))
			{
				base.ErrorsList.Add($"PartGroup [{partGroupId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartGroupInformationDto eRPPartGroupInformationDto = await base.ERPPartGroupRepository.GetPartGroup(partGroupId);
				string text = await base.ERPPartGroupRepository.WhereUsed("PartGroups", new object[1] { eRPPartGroupInformationDto.imuPartGroupID }, new object[1] { "imuPartGroupID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartGroup cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartGroupDto>> Process_DeletePartGroup(Guid partGroupId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartGroupDto> result;
		try
		{
			IERPPartGroupRepository iERPPartGroupRepository = (base.ERPPartGroupRepository = new ERPPartGroupRepository(base.ApiClientContext));
			using (iERPPartGroupRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartGroupRepository.DeleteRowFromTable("PartGroups", "imu", partGroupId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartGroup [{partGroupId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartGroupDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartGroupDto()
			};
		}
		return result;
	}
}
