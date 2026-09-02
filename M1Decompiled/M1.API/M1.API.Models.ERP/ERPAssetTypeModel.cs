using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAssetTypeModel : ERPBaseModel, IERPAssetTypeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAssetTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAssetTypeRepository iERPAssetTypeRepository = (base.ERPAssetTypeRepository = new ERPAssetTypeRepository(base.ApiClientContext));
		using (iERPAssetTypeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAssetTypeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAssetTypeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAssetTypeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAssetTypeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAssetType(Guid assetTypeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetTypeRepository iERPAssetTypeRepository = (base.ERPAssetTypeRepository = new ERPAssetTypeRepository(base.ApiClientContext));
		using (iERPAssetTypeRepository)
		{
			if (!(await base.ERPAssetTypeRepository.DoesAssetTypeExist(assetTypeId)))
			{
				errorsList.Add($"AssetType [{assetTypeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAssetTypeDto>>> Process_GetAllAssetTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAssetTypeDto> allAssetTypesDto = new List<ERPAssetTypeDto>();
		ERPResponseMessageDto<IList<ERPAssetTypeDto>> result;
		try
		{
			IERPAssetTypeRepository iERPAssetTypeRepository = (base.ERPAssetTypeRepository = new ERPAssetTypeRepository(base.ApiClientContext));
			using (iERPAssetTypeRepository)
			{
				foreach (ERPAssetTypeInformationDto item2 in await base.ERPAssetTypeRepository.GetAllAssetTypes(pageSize, pageNumber, filter, orderBy))
				{
					ERPAssetTypeDto item = new ERPAssetTypeDto
					{
						fatAccumDeprGlAccountID = item2.fatAccumDeprGlAccountID,
						fatAssetGlAccountID = item2.fatAssetGlAccountID,
						fatAssetTypeID = item2.fatAssetTypeID,
						fatCreatedBy = item2.fatCreatedBy,
						fatCreatedDate = item2.fatCreatedDate,
						fatDepreciationGlAccountID = item2.fatDepreciationGlAccountID,
						fatDescription = item2.fatDescription,
						fatUniqueID = item2.fatUniqueID,
						fatExpenseGlAccountID = item2.fatExpenseGlAccountID,
						fatLossGlAccountID = item2.fatLossGlAccountID,
						fatProfitGlAccountID = item2.fatProfitGlAccountID,
						fatRepairsGlAccountID = item2.fatRepairsGlAccountID,
						fatRevaluationGlAccountID = item2.fatRevaluationGlAccountID,
						fatRowVersion = item2.fatRowVersion,
						CustomFields = item2.CustomFields
					};
					allAssetTypesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all AssetTypes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAssetTypeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAssetTypesDto,
				RecordCount = allAssetTypesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAssetTypeDto>> Process_GetAssetType(Guid assetTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAssetTypeDto assetTypeDto = null;
		ERPResponseMessageDto<ERPAssetTypeDto> result;
		try
		{
			IERPAssetTypeRepository iERPAssetTypeRepository = (base.ERPAssetTypeRepository = new ERPAssetTypeRepository(base.ApiClientContext));
			using (iERPAssetTypeRepository)
			{
				ERPAssetTypeInformationDto eRPAssetTypeInformationDto = await base.ERPAssetTypeRepository.GetAssetType(assetTypeId);
				assetTypeDto = new ERPAssetTypeDto
				{
					fatAccumDeprGlAccountID = eRPAssetTypeInformationDto.fatAccumDeprGlAccountID,
					fatAssetGlAccountID = eRPAssetTypeInformationDto.fatAssetGlAccountID,
					fatAssetTypeID = eRPAssetTypeInformationDto.fatAssetTypeID,
					fatCreatedBy = eRPAssetTypeInformationDto.fatCreatedBy,
					fatCreatedDate = eRPAssetTypeInformationDto.fatCreatedDate,
					fatDepreciationGlAccountID = eRPAssetTypeInformationDto.fatDepreciationGlAccountID,
					fatDescription = eRPAssetTypeInformationDto.fatDescription,
					fatUniqueID = eRPAssetTypeInformationDto.fatUniqueID,
					fatExpenseGlAccountID = eRPAssetTypeInformationDto.fatExpenseGlAccountID,
					fatLossGlAccountID = eRPAssetTypeInformationDto.fatLossGlAccountID,
					fatProfitGlAccountID = eRPAssetTypeInformationDto.fatProfitGlAccountID,
					fatRepairsGlAccountID = eRPAssetTypeInformationDto.fatRepairsGlAccountID,
					fatRevaluationGlAccountID = eRPAssetTypeInformationDto.fatRevaluationGlAccountID,
					fatRowVersion = eRPAssetTypeInformationDto.fatRowVersion,
					CustomFields = eRPAssetTypeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the AssetTypes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = assetTypeDto
			};
		}
		return result;
	}
}
