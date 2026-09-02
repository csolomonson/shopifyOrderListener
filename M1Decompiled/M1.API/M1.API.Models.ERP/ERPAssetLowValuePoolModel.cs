using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAssetLowValuePoolModel : ERPBaseModel, IERPAssetLowValuePoolModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAssetLowValuePool(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAssetLowValuePoolRepository iERPAssetLowValuePoolRepository = (base.ERPAssetLowValuePoolRepository = new ERPAssetLowValuePoolRepository(base.ApiClientContext));
		using (iERPAssetLowValuePoolRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAssetLowValuePoolRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAssetLowValuePoolRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAssetLowValuePoolRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAssetLowValuePoolRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAssetLowValuePool(Guid assetLowValuePoolId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetLowValuePoolRepository iERPAssetLowValuePoolRepository = (base.ERPAssetLowValuePoolRepository = new ERPAssetLowValuePoolRepository(base.ApiClientContext));
		using (iERPAssetLowValuePoolRepository)
		{
			if (!(await base.ERPAssetLowValuePoolRepository.DoesAssetLowValuePoolExist(assetLowValuePoolId)))
			{
				errorsList.Add($"AssetLowValuePool [{assetLowValuePoolId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutAssetLowValuePool(ERPAssetLowValuePoolDto assetLowValuePool)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAssetLowValuePoolRepository iERPAssetLowValuePoolRepository = (base.ERPAssetLowValuePoolRepository = new ERPAssetLowValuePoolRepository(base.ApiClientContext));
		using (iERPAssetLowValuePoolRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAssetLowValuePoolDto>>> Process_GetAllAssetLowValuePool(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAssetLowValuePoolDto> allAssetLowValuePoolDto = new List<ERPAssetLowValuePoolDto>();
		ERPResponseMessageDto<IList<ERPAssetLowValuePoolDto>> result;
		try
		{
			IERPAssetLowValuePoolRepository iERPAssetLowValuePoolRepository = (base.ERPAssetLowValuePoolRepository = new ERPAssetLowValuePoolRepository(base.ApiClientContext));
			using (iERPAssetLowValuePoolRepository)
			{
				foreach (ERPAssetLowValuePoolInformationDto item2 in await base.ERPAssetLowValuePoolRepository.GetAllAssetLowValuePool(pageSize, pageNumber, filter, orderBy))
				{
					ERPAssetLowValuePoolDto item = new ERPAssetLowValuePoolDto
					{
						favClosedDate = item2.favClosedDate,
						favCreatedBy = item2.favCreatedBy,
						favCreatedDate = item2.favCreatedDate,
						favEndingBalance = item2.favEndingBalance,
						favUniqueID = item2.favUniqueID,
						favHighRate = item2.favHighRate,
						favHighRateDepreciation = item2.favHighRateDepreciation,
						favImprovement = item2.favImprovement,
						favClosed = item2.favClosed,
						favLowCostAddition = item2.favLowCostAddition,
						favLowRate = item2.favLowRate,
						favLowRateDepreciation = item2.favLowRateDepreciation,
						favLowValueAddition = item2.favLowValueAddition,
						favOpeningBalance = item2.favOpeningBalance,
						favPoolYearID = item2.favPoolYearID,
						favRowVersion = item2.favRowVersion,
						favTermination = item2.favTermination,
						CustomFields = item2.CustomFields
					};
					allAssetLowValuePoolDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all AssetLowValuePool]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAssetLowValuePoolDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAssetLowValuePoolDto,
				RecordCount = allAssetLowValuePoolDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAssetLowValuePoolDto>> Process_GetAssetLowValuePool(Guid assetLowValuePoolId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAssetLowValuePoolDto assetLowValuePoolDto = null;
		ERPResponseMessageDto<ERPAssetLowValuePoolDto> result;
		try
		{
			IERPAssetLowValuePoolRepository iERPAssetLowValuePoolRepository = (base.ERPAssetLowValuePoolRepository = new ERPAssetLowValuePoolRepository(base.ApiClientContext));
			using (iERPAssetLowValuePoolRepository)
			{
				ERPAssetLowValuePoolInformationDto eRPAssetLowValuePoolInformationDto = await base.ERPAssetLowValuePoolRepository.GetAssetLowValuePool(assetLowValuePoolId);
				assetLowValuePoolDto = new ERPAssetLowValuePoolDto
				{
					favClosedDate = eRPAssetLowValuePoolInformationDto.favClosedDate,
					favCreatedBy = eRPAssetLowValuePoolInformationDto.favCreatedBy,
					favCreatedDate = eRPAssetLowValuePoolInformationDto.favCreatedDate,
					favEndingBalance = eRPAssetLowValuePoolInformationDto.favEndingBalance,
					favUniqueID = eRPAssetLowValuePoolInformationDto.favUniqueID,
					favHighRate = eRPAssetLowValuePoolInformationDto.favHighRate,
					favHighRateDepreciation = eRPAssetLowValuePoolInformationDto.favHighRateDepreciation,
					favImprovement = eRPAssetLowValuePoolInformationDto.favImprovement,
					favClosed = eRPAssetLowValuePoolInformationDto.favClosed,
					favLowCostAddition = eRPAssetLowValuePoolInformationDto.favLowCostAddition,
					favLowRate = eRPAssetLowValuePoolInformationDto.favLowRate,
					favLowRateDepreciation = eRPAssetLowValuePoolInformationDto.favLowRateDepreciation,
					favLowValueAddition = eRPAssetLowValuePoolInformationDto.favLowValueAddition,
					favOpeningBalance = eRPAssetLowValuePoolInformationDto.favOpeningBalance,
					favPoolYearID = eRPAssetLowValuePoolInformationDto.favPoolYearID,
					favRowVersion = eRPAssetLowValuePoolInformationDto.favRowVersion,
					favTermination = eRPAssetLowValuePoolInformationDto.favTermination,
					CustomFields = eRPAssetLowValuePoolInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the AssetLowValuePool []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetLowValuePoolDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = assetLowValuePoolDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAssetLowValuePoolDto>> Process_PutAssetLowValuePool(ERPAssetLowValuePoolDto assetLowValuePool)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPAssetLowValuePoolDto createdObject = null;
		ERPResponseMessageDto<ERPAssetLowValuePoolDto> result;
		try
		{
			IERPAssetLowValuePoolRepository iERPAssetLowValuePoolRepository = (base.ERPAssetLowValuePoolRepository = new ERPAssetLowValuePoolRepository(base.ApiClientContext));
			using (iERPAssetLowValuePoolRepository)
			{
				APIValidationInfoDto postResult = await base.ERPAssetLowValuePoolRepository.SaveAssetLowValuePool(assetLowValuePool);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPAssetLowValuePoolInformationDto eRPAssetLowValuePoolInformationDto = await base.ERPAssetLowValuePoolRepository.GetAssetLowValuePool(assetLowValuePool.favUniqueID);
					createdObject = new ERPAssetLowValuePoolDto
					{
						favClosedDate = eRPAssetLowValuePoolInformationDto.favClosedDate,
						favCreatedBy = eRPAssetLowValuePoolInformationDto.favCreatedBy,
						favCreatedDate = eRPAssetLowValuePoolInformationDto.favCreatedDate,
						favEndingBalance = eRPAssetLowValuePoolInformationDto.favEndingBalance,
						favUniqueID = eRPAssetLowValuePoolInformationDto.favUniqueID,
						favHighRate = eRPAssetLowValuePoolInformationDto.favHighRate,
						favHighRateDepreciation = eRPAssetLowValuePoolInformationDto.favHighRateDepreciation,
						favImprovement = eRPAssetLowValuePoolInformationDto.favImprovement,
						favClosed = eRPAssetLowValuePoolInformationDto.favClosed,
						favLowCostAddition = eRPAssetLowValuePoolInformationDto.favLowCostAddition,
						favLowRate = eRPAssetLowValuePoolInformationDto.favLowRate,
						favLowRateDepreciation = eRPAssetLowValuePoolInformationDto.favLowRateDepreciation,
						favLowValueAddition = eRPAssetLowValuePoolInformationDto.favLowValueAddition,
						favOpeningBalance = eRPAssetLowValuePoolInformationDto.favOpeningBalance,
						favPoolYearID = eRPAssetLowValuePoolInformationDto.favPoolYearID,
						favRowVersion = eRPAssetLowValuePoolInformationDto.favRowVersion,
						favTermination = eRPAssetLowValuePoolInformationDto.favTermination,
						CustomFields = eRPAssetLowValuePoolInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing AssetLowValuePool [{assetLowValuePool.favUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetLowValuePoolDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteAssetLowValuePool(Guid assetLowValuePoolId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetLowValuePoolRepository iERPAssetLowValuePoolRepository = (base.ERPAssetLowValuePoolRepository = new ERPAssetLowValuePoolRepository(base.ApiClientContext));
		using (iERPAssetLowValuePoolRepository)
		{
			if (!(await base.ERPAssetLowValuePoolRepository.DoesAssetLowValuePoolExist(assetLowValuePoolId)))
			{
				base.ErrorsList.Add($"AssetLowValuePool [{assetLowValuePoolId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPAssetLowValuePoolInformationDto eRPAssetLowValuePoolInformationDto = await base.ERPAssetLowValuePoolRepository.GetAssetLowValuePool(assetLowValuePoolId);
				string text = await base.ERPAssetLowValuePoolRepository.WhereUsed("AssetLowValuePool", new object[1] { eRPAssetLowValuePoolInformationDto.favPoolYearID }, new object[1] { "favPoolYearID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("AssetLowValuePool cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPAssetLowValuePoolDto>> Process_DeleteAssetLowValuePool(Guid assetLowValuePoolId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPAssetLowValuePoolDto> result;
		try
		{
			IERPAssetLowValuePoolRepository iERPAssetLowValuePoolRepository = (base.ERPAssetLowValuePoolRepository = new ERPAssetLowValuePoolRepository(base.ApiClientContext));
			using (iERPAssetLowValuePoolRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPAssetLowValuePoolRepository.DeleteRowFromTable("AssetLowValuePool", "fav", assetLowValuePoolId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of AssetLowValuePool [{assetLowValuePoolId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetLowValuePoolDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPAssetLowValuePoolDto()
			};
		}
		return result;
	}
}
