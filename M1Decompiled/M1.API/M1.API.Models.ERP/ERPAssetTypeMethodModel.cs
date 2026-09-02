using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAssetTypeMethodModel : ERPBaseModel, IERPAssetTypeMethodModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAssetTypeMethods(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAssetTypeMethodRepository iERPAssetTypeMethodRepository = (base.ERPAssetTypeMethodRepository = new ERPAssetTypeMethodRepository(base.ApiClientContext));
		using (iERPAssetTypeMethodRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAssetTypeMethodRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAssetTypeMethodRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAssetTypeMethodRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAssetTypeMethodRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAssetTypeMethod(Guid assetTypeMethodId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetTypeMethodRepository iERPAssetTypeMethodRepository = (base.ERPAssetTypeMethodRepository = new ERPAssetTypeMethodRepository(base.ApiClientContext));
		using (iERPAssetTypeMethodRepository)
		{
			if (!(await base.ERPAssetTypeMethodRepository.DoesAssetTypeMethodExist(assetTypeMethodId)))
			{
				errorsList.Add($"AssetTypeMethod [{assetTypeMethodId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAssetTypeMethodDto>>> Process_GetAllAssetTypeMethods(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAssetTypeMethodDto> allAssetTypeMethodsDto = new List<ERPAssetTypeMethodDto>();
		ERPResponseMessageDto<IList<ERPAssetTypeMethodDto>> result;
		try
		{
			IERPAssetTypeMethodRepository iERPAssetTypeMethodRepository = (base.ERPAssetTypeMethodRepository = new ERPAssetTypeMethodRepository(base.ApiClientContext));
			using (iERPAssetTypeMethodRepository)
			{
				foreach (ERPAssetTypeMethodInformationDto item2 in await base.ERPAssetTypeMethodRepository.GetAllAssetTypeMethods(pageSize, pageNumber, filter, orderBy))
				{
					ERPAssetTypeMethodDto item = new ERPAssetTypeMethodDto
					{
						famAssetTypeID = item2.famAssetTypeID,
						famBookDepreciationMethod = item2.famBookDepreciationMethod,
						famBookMultiplier = item2.famBookMultiplier,
						famCalculationMethod = item2.famCalculationMethod,
						famCreatedBy = item2.famCreatedBy,
						famCreatedDate = item2.famCreatedDate,
						famUniqueID = item2.famUniqueID,
						famCurrentMethod = item2.famCurrentMethod,
						famMonthCalculationType = item2.famMonthCalculationType,
						famRowVersion = item2.famRowVersion,
						famAssetTypeMethodID = item2.famAssetTypeMethodID,
						famStartDate = item2.famStartDate,
						famTaxDepreciationMethod = item2.famTaxDepreciationMethod,
						famTaxMultiplier = item2.famTaxMultiplier,
						CustomFields = item2.CustomFields
					};
					allAssetTypeMethodsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all AssetTypeMethods]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAssetTypeMethodDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAssetTypeMethodsDto,
				RecordCount = allAssetTypeMethodsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAssetTypeMethodDto>> Process_GetAssetTypeMethod(Guid assetTypeMethodId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAssetTypeMethodDto assetTypeMethodDto = null;
		ERPResponseMessageDto<ERPAssetTypeMethodDto> result;
		try
		{
			IERPAssetTypeMethodRepository iERPAssetTypeMethodRepository = (base.ERPAssetTypeMethodRepository = new ERPAssetTypeMethodRepository(base.ApiClientContext));
			using (iERPAssetTypeMethodRepository)
			{
				ERPAssetTypeMethodInformationDto eRPAssetTypeMethodInformationDto = await base.ERPAssetTypeMethodRepository.GetAssetTypeMethod(assetTypeMethodId);
				assetTypeMethodDto = new ERPAssetTypeMethodDto
				{
					famAssetTypeID = eRPAssetTypeMethodInformationDto.famAssetTypeID,
					famBookDepreciationMethod = eRPAssetTypeMethodInformationDto.famBookDepreciationMethod,
					famBookMultiplier = eRPAssetTypeMethodInformationDto.famBookMultiplier,
					famCalculationMethod = eRPAssetTypeMethodInformationDto.famCalculationMethod,
					famCreatedBy = eRPAssetTypeMethodInformationDto.famCreatedBy,
					famCreatedDate = eRPAssetTypeMethodInformationDto.famCreatedDate,
					famUniqueID = eRPAssetTypeMethodInformationDto.famUniqueID,
					famCurrentMethod = eRPAssetTypeMethodInformationDto.famCurrentMethod,
					famMonthCalculationType = eRPAssetTypeMethodInformationDto.famMonthCalculationType,
					famRowVersion = eRPAssetTypeMethodInformationDto.famRowVersion,
					famAssetTypeMethodID = eRPAssetTypeMethodInformationDto.famAssetTypeMethodID,
					famStartDate = eRPAssetTypeMethodInformationDto.famStartDate,
					famTaxDepreciationMethod = eRPAssetTypeMethodInformationDto.famTaxDepreciationMethod,
					famTaxMultiplier = eRPAssetTypeMethodInformationDto.famTaxMultiplier,
					CustomFields = eRPAssetTypeMethodInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the AssetTypeMethods []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetTypeMethodDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = assetTypeMethodDto
			};
		}
		return result;
	}
}
