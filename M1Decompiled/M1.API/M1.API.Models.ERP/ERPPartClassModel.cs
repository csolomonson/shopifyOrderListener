using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartClassModel : ERPBaseModel, IERPPartClassModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartClasses(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartClassRepository iERPPartClassRepository = (base.ERPPartClassRepository = new ERPPartClassRepository(base.ApiClientContext));
		using (iERPPartClassRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartClassRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartClassRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartClassRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartClassRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartClass(Guid partClassId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartClassRepository iERPPartClassRepository = (base.ERPPartClassRepository = new ERPPartClassRepository(base.ApiClientContext));
		using (iERPPartClassRepository)
		{
			if (!(await base.ERPPartClassRepository.DoesPartClassExist(partClassId)))
			{
				errorsList.Add($"PartClass [{partClassId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartClass(ERPPartClassDto partClass)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartClassRepository iERPPartClassRepository = (base.ERPPartClassRepository = new ERPPartClassRepository(base.ApiClientContext));
		using (iERPPartClassRepository)
		{
			if (!string.IsNullOrWhiteSpace(partClass.imcParentPartClassID) && !(await base.ERPPartClassRepository.DoesRecordExistInTableUsingKeys("PartClasses", new object[1] { "IMCPARTCLASSID" }, new object[1] { partClass.imcParentPartClassID })))
			{
				errorsList.Add("imcParentPartClassID [" + partClass.imcParentPartClassID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partClass.imcInventoryGlAccountID) && !(await base.ERPPartClassRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { partClass.imcInventoryGlAccountID })))
			{
				errorsList.Add("imcInventoryGlAccountID [" + partClass.imcInventoryGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partClass.imcInvInInspectionGlAccountID) && !(await base.ERPPartClassRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { partClass.imcInvInInspectionGlAccountID })))
			{
				errorsList.Add("imcInvInInspectionGlAccountID [" + partClass.imcInvInInspectionGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partClass.imcInvToReturnGlAccountID) && !(await base.ERPPartClassRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { partClass.imcInvToReturnGlAccountID })))
			{
				errorsList.Add("imcInvToReturnGlAccountID [" + partClass.imcInvToReturnGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partClass.imcInvInTransferGlAccountID) && !(await base.ERPPartClassRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { partClass.imcInvInTransferGlAccountID })))
			{
				errorsList.Add("imcInvInTransferGlAccountID [" + partClass.imcInvInTransferGlAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartClassDto>>> Process_GetAllPartClasses(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartClassDto> allPartClassesDto = new List<ERPPartClassDto>();
		ERPResponseMessageDto<IList<ERPPartClassDto>> result;
		try
		{
			IERPPartClassRepository iERPPartClassRepository = (base.ERPPartClassRepository = new ERPPartClassRepository(base.ApiClientContext));
			using (iERPPartClassRepository)
			{
				foreach (ERPPartClassInformationDto item2 in await base.ERPPartClassRepository.GetAllPartClasses(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartClassDto item = new ERPPartClassDto
					{
						imcPartClassID = item2.imcPartClassID,
						imcCreatedBy = item2.imcCreatedBy,
						imcCreatedDate = item2.imcCreatedDate,
						imcDescription = item2.imcDescription,
						imcUniqueID = item2.imcUniqueID,
						imcFdxHandlingCost = item2.imcFdxHandlingCost,
						imcFdxPackageHeight = item2.imcFdxPackageHeight,
						imcFdxPackageLength = item2.imcFdxPackageLength,
						imcFdxPackageWidth = item2.imcFdxPackageWidth,
						imcFdxPackaging = item2.imcFdxPackaging,
						imcFdxPackagingCost = item2.imcFdxPackagingCost,
						imcFdxShipCostMarkupPct = item2.imcFdxShipCostMarkupPct,
						imcInactiveDate = item2.imcInactiveDate,
						imcInventoryGlAccountID = item2.imcInventoryGlAccountID,
						imcInvInInspectionGlAccountID = item2.imcInvInInspectionGlAccountID,
						imcInvInTransferGlAccountID = item2.imcInvInTransferGlAccountID,
						imcInvToReturnGlAccountID = item2.imcInvToReturnGlAccountID,
						imcInactive = item2.imcInactive,
						imcFdxNonstandardContainer = item2.imcFdxNonstandardContainer,
						imcFdxOneItemPerShipment = item2.imcFdxOneItemPerShipment,
						imcRequiresInspection = item2.imcRequiresInspection,
						imcParentPartClassID = item2.imcParentPartClassID,
						imcPartImageFileName = item2.imcPartImageFileName,
						imcPickingMethod = item2.imcPickingMethod,
						imcReorderMethod = item2.imcReorderMethod,
						imcRowVersion = item2.imcRowVersion,
						imcWeight = item2.imcWeight,
						CustomFields = item2.CustomFields
					};
					allPartClassesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartClasses]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartClassDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartClassesDto,
				RecordCount = allPartClassesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartClassDto>> Process_GetPartClass(Guid partClassId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartClassDto partClassDto = null;
		ERPResponseMessageDto<ERPPartClassDto> result;
		try
		{
			IERPPartClassRepository iERPPartClassRepository = (base.ERPPartClassRepository = new ERPPartClassRepository(base.ApiClientContext));
			using (iERPPartClassRepository)
			{
				ERPPartClassInformationDto eRPPartClassInformationDto = await base.ERPPartClassRepository.GetPartClass(partClassId);
				partClassDto = new ERPPartClassDto
				{
					imcPartClassID = eRPPartClassInformationDto.imcPartClassID,
					imcCreatedBy = eRPPartClassInformationDto.imcCreatedBy,
					imcCreatedDate = eRPPartClassInformationDto.imcCreatedDate,
					imcDescription = eRPPartClassInformationDto.imcDescription,
					imcUniqueID = eRPPartClassInformationDto.imcUniqueID,
					imcFdxHandlingCost = eRPPartClassInformationDto.imcFdxHandlingCost,
					imcFdxPackageHeight = eRPPartClassInformationDto.imcFdxPackageHeight,
					imcFdxPackageLength = eRPPartClassInformationDto.imcFdxPackageLength,
					imcFdxPackageWidth = eRPPartClassInformationDto.imcFdxPackageWidth,
					imcFdxPackaging = eRPPartClassInformationDto.imcFdxPackaging,
					imcFdxPackagingCost = eRPPartClassInformationDto.imcFdxPackagingCost,
					imcFdxShipCostMarkupPct = eRPPartClassInformationDto.imcFdxShipCostMarkupPct,
					imcInactiveDate = eRPPartClassInformationDto.imcInactiveDate,
					imcInventoryGlAccountID = eRPPartClassInformationDto.imcInventoryGlAccountID,
					imcInvInInspectionGlAccountID = eRPPartClassInformationDto.imcInvInInspectionGlAccountID,
					imcInvInTransferGlAccountID = eRPPartClassInformationDto.imcInvInTransferGlAccountID,
					imcInvToReturnGlAccountID = eRPPartClassInformationDto.imcInvToReturnGlAccountID,
					imcInactive = eRPPartClassInformationDto.imcInactive,
					imcFdxNonstandardContainer = eRPPartClassInformationDto.imcFdxNonstandardContainer,
					imcFdxOneItemPerShipment = eRPPartClassInformationDto.imcFdxOneItemPerShipment,
					imcRequiresInspection = eRPPartClassInformationDto.imcRequiresInspection,
					imcParentPartClassID = eRPPartClassInformationDto.imcParentPartClassID,
					imcPartImageFileName = eRPPartClassInformationDto.imcPartImageFileName,
					imcPickingMethod = eRPPartClassInformationDto.imcPickingMethod,
					imcReorderMethod = eRPPartClassInformationDto.imcReorderMethod,
					imcRowVersion = eRPPartClassInformationDto.imcRowVersion,
					imcWeight = eRPPartClassInformationDto.imcWeight,
					CustomFields = eRPPartClassInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartClasses []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartClassDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partClassDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartClassDto>> Process_PutPartClass(ERPPartClassDto partClass)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartClassDto createdObject = null;
		ERPResponseMessageDto<ERPPartClassDto> result;
		try
		{
			IERPPartClassRepository iERPPartClassRepository = (base.ERPPartClassRepository = new ERPPartClassRepository(base.ApiClientContext));
			using (iERPPartClassRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartClassRepository.SavePartClass(partClass);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartClassInformationDto eRPPartClassInformationDto = await base.ERPPartClassRepository.GetPartClass(partClass.imcUniqueID);
					createdObject = new ERPPartClassDto
					{
						imcPartClassID = eRPPartClassInformationDto.imcPartClassID,
						imcCreatedBy = eRPPartClassInformationDto.imcCreatedBy,
						imcCreatedDate = eRPPartClassInformationDto.imcCreatedDate,
						imcDescription = eRPPartClassInformationDto.imcDescription,
						imcUniqueID = eRPPartClassInformationDto.imcUniqueID,
						imcFdxHandlingCost = eRPPartClassInformationDto.imcFdxHandlingCost,
						imcFdxPackageHeight = eRPPartClassInformationDto.imcFdxPackageHeight,
						imcFdxPackageLength = eRPPartClassInformationDto.imcFdxPackageLength,
						imcFdxPackageWidth = eRPPartClassInformationDto.imcFdxPackageWidth,
						imcFdxPackaging = eRPPartClassInformationDto.imcFdxPackaging,
						imcFdxPackagingCost = eRPPartClassInformationDto.imcFdxPackagingCost,
						imcFdxShipCostMarkupPct = eRPPartClassInformationDto.imcFdxShipCostMarkupPct,
						imcInactiveDate = eRPPartClassInformationDto.imcInactiveDate,
						imcInventoryGlAccountID = eRPPartClassInformationDto.imcInventoryGlAccountID,
						imcInvInInspectionGlAccountID = eRPPartClassInformationDto.imcInvInInspectionGlAccountID,
						imcInvInTransferGlAccountID = eRPPartClassInformationDto.imcInvInTransferGlAccountID,
						imcInvToReturnGlAccountID = eRPPartClassInformationDto.imcInvToReturnGlAccountID,
						imcInactive = eRPPartClassInformationDto.imcInactive,
						imcFdxNonstandardContainer = eRPPartClassInformationDto.imcFdxNonstandardContainer,
						imcFdxOneItemPerShipment = eRPPartClassInformationDto.imcFdxOneItemPerShipment,
						imcRequiresInspection = eRPPartClassInformationDto.imcRequiresInspection,
						imcParentPartClassID = eRPPartClassInformationDto.imcParentPartClassID,
						imcPartImageFileName = eRPPartClassInformationDto.imcPartImageFileName,
						imcPickingMethod = eRPPartClassInformationDto.imcPickingMethod,
						imcReorderMethod = eRPPartClassInformationDto.imcReorderMethod,
						imcRowVersion = eRPPartClassInformationDto.imcRowVersion,
						imcWeight = eRPPartClassInformationDto.imcWeight,
						CustomFields = eRPPartClassInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartClass [{partClass.imcUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartClassDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartClass(Guid partClassId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartClassRepository iERPPartClassRepository = (base.ERPPartClassRepository = new ERPPartClassRepository(base.ApiClientContext));
		using (iERPPartClassRepository)
		{
			if (!(await base.ERPPartClassRepository.DoesPartClassExist(partClassId)))
			{
				base.ErrorsList.Add($"PartClass [{partClassId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartClassInformationDto eRPPartClassInformationDto = await base.ERPPartClassRepository.GetPartClass(partClassId);
				string text = await base.ERPPartClassRepository.WhereUsed("PartClasses", new object[1] { eRPPartClassInformationDto.imcPartClassID }, new object[1] { "imcPartClassID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartClass cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartClassDto>> Process_DeletePartClass(Guid partClassId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartClassDto> result;
		try
		{
			IERPPartClassRepository iERPPartClassRepository = (base.ERPPartClassRepository = new ERPPartClassRepository(base.ApiClientContext));
			using (iERPPartClassRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartClassRepository.DeleteRowFromTable("PartClasses", "imc", partClassId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartClass [{partClassId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartClassDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartClassDto()
			};
		}
		return result;
	}
}
