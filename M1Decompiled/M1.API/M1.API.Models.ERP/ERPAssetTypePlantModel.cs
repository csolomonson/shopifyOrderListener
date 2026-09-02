using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAssetTypePlantModel : ERPBaseModel, IERPAssetTypePlantModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAssetTypePlants(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAssetTypePlantRepository iERPAssetTypePlantRepository = (base.ERPAssetTypePlantRepository = new ERPAssetTypePlantRepository(base.ApiClientContext));
		using (iERPAssetTypePlantRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAssetTypePlantRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAssetTypePlantRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAssetTypePlantRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAssetTypePlantRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAssetTypePlant(Guid assetTypePlantId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetTypePlantRepository iERPAssetTypePlantRepository = (base.ERPAssetTypePlantRepository = new ERPAssetTypePlantRepository(base.ApiClientContext));
		using (iERPAssetTypePlantRepository)
		{
			if (!(await base.ERPAssetTypePlantRepository.DoesAssetTypePlantExist(assetTypePlantId)))
			{
				errorsList.Add($"AssetTypePlant [{assetTypePlantId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAssetTypePlantDto>>> Process_GetAllAssetTypePlants(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAssetTypePlantDto> allAssetTypePlantsDto = new List<ERPAssetTypePlantDto>();
		ERPResponseMessageDto<IList<ERPAssetTypePlantDto>> result;
		try
		{
			IERPAssetTypePlantRepository iERPAssetTypePlantRepository = (base.ERPAssetTypePlantRepository = new ERPAssetTypePlantRepository(base.ApiClientContext));
			using (iERPAssetTypePlantRepository)
			{
				foreach (ERPAssetTypePlantInformationDto item2 in await base.ERPAssetTypePlantRepository.GetAllAssetTypePlants(pageSize, pageNumber, filter, orderBy))
				{
					ERPAssetTypePlantDto item = new ERPAssetTypePlantDto
					{
						fayAccumDeprGlAccountID = item2.fayAccumDeprGlAccountID,
						fayAssetGlAccountID = item2.fayAssetGlAccountID,
						fayAssetTypeID = item2.fayAssetTypeID,
						fayAssetTypePlantID = item2.fayAssetTypePlantID,
						fayCreatedBy = item2.fayCreatedBy,
						fayCreatedDate = item2.fayCreatedDate,
						fayDepreciationGlAccountID = item2.fayDepreciationGlAccountID,
						fayUniqueID = item2.fayUniqueID,
						fayExpenseGlAccountID = item2.fayExpenseGlAccountID,
						fayLossGlAccountID = item2.fayLossGlAccountID,
						fayProfitGlAccountID = item2.fayProfitGlAccountID,
						fayRepairsGlAccountID = item2.fayRepairsGlAccountID,
						fayRevaluationGlAccountID = item2.fayRevaluationGlAccountID,
						fayRowVersion = item2.fayRowVersion,
						CustomFields = item2.CustomFields
					};
					allAssetTypePlantsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all AssetTypePlants]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAssetTypePlantDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAssetTypePlantsDto,
				RecordCount = allAssetTypePlantsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAssetTypePlantDto>> Process_GetAssetTypePlant(Guid assetTypePlantId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAssetTypePlantDto assetTypePlantDto = null;
		ERPResponseMessageDto<ERPAssetTypePlantDto> result;
		try
		{
			IERPAssetTypePlantRepository iERPAssetTypePlantRepository = (base.ERPAssetTypePlantRepository = new ERPAssetTypePlantRepository(base.ApiClientContext));
			using (iERPAssetTypePlantRepository)
			{
				ERPAssetTypePlantInformationDto eRPAssetTypePlantInformationDto = await base.ERPAssetTypePlantRepository.GetAssetTypePlant(assetTypePlantId);
				assetTypePlantDto = new ERPAssetTypePlantDto
				{
					fayAccumDeprGlAccountID = eRPAssetTypePlantInformationDto.fayAccumDeprGlAccountID,
					fayAssetGlAccountID = eRPAssetTypePlantInformationDto.fayAssetGlAccountID,
					fayAssetTypeID = eRPAssetTypePlantInformationDto.fayAssetTypeID,
					fayAssetTypePlantID = eRPAssetTypePlantInformationDto.fayAssetTypePlantID,
					fayCreatedBy = eRPAssetTypePlantInformationDto.fayCreatedBy,
					fayCreatedDate = eRPAssetTypePlantInformationDto.fayCreatedDate,
					fayDepreciationGlAccountID = eRPAssetTypePlantInformationDto.fayDepreciationGlAccountID,
					fayUniqueID = eRPAssetTypePlantInformationDto.fayUniqueID,
					fayExpenseGlAccountID = eRPAssetTypePlantInformationDto.fayExpenseGlAccountID,
					fayLossGlAccountID = eRPAssetTypePlantInformationDto.fayLossGlAccountID,
					fayProfitGlAccountID = eRPAssetTypePlantInformationDto.fayProfitGlAccountID,
					fayRepairsGlAccountID = eRPAssetTypePlantInformationDto.fayRepairsGlAccountID,
					fayRevaluationGlAccountID = eRPAssetTypePlantInformationDto.fayRevaluationGlAccountID,
					fayRowVersion = eRPAssetTypePlantInformationDto.fayRowVersion,
					CustomFields = eRPAssetTypePlantInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the AssetTypePlants []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetTypePlantDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = assetTypePlantDto
			};
		}
		return result;
	}
}
