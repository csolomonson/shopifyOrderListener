using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAssetScheduleModel : ERPBaseModel, IERPAssetScheduleModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAssetSchedules(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAssetScheduleRepository iERPAssetScheduleRepository = (base.ERPAssetScheduleRepository = new ERPAssetScheduleRepository(base.ApiClientContext));
		using (iERPAssetScheduleRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAssetScheduleRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAssetScheduleRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAssetScheduleRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAssetScheduleRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAssetSchedule(Guid assetScheduleId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetScheduleRepository iERPAssetScheduleRepository = (base.ERPAssetScheduleRepository = new ERPAssetScheduleRepository(base.ApiClientContext));
		using (iERPAssetScheduleRepository)
		{
			if (!(await base.ERPAssetScheduleRepository.DoesAssetScheduleExist(assetScheduleId)))
			{
				errorsList.Add($"AssetSchedule [{assetScheduleId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutAssetSchedule(ERPAssetScheduleDto assetSchedule)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetScheduleRepository iERPAssetScheduleRepository = (base.ERPAssetScheduleRepository = new ERPAssetScheduleRepository(base.ApiClientContext));
		using (iERPAssetScheduleRepository)
		{
			if (!string.IsNullOrWhiteSpace(assetSchedule.fasAssetID) && !(await base.ERPAssetScheduleRepository.DoesRecordExistInTableUsingKeys("Assets", new object[1] { "FAPASSETID" }, new object[1] { assetSchedule.fasAssetID })))
			{
				errorsList.Add("fasAssetID [" + assetSchedule.fasAssetID + "] not found.");
			}
			if (assetSchedule.fasGlFiscalYearID > 0 && !(await base.ERPAssetScheduleRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { assetSchedule.fasGlFiscalYearID })))
			{
				errorsList.Add($"fasGlFiscalYearID [{assetSchedule.fasGlFiscalYearID}] not found.");
			}
			if (assetSchedule.fasGlFiscalYearPeriodID > 0 && !(await base.ERPAssetScheduleRepository.DoesRecordExistInTableUsingKeys("GLFiscalYearPeriods", new object[2] { "GLFGLFISCALYEARID", "GLFGLFISCALYEARPERIODID" }, new object[2] { assetSchedule.fasGlFiscalYearID, assetSchedule.fasGlFiscalYearPeriodID })))
			{
				errorsList.Add($"fasGlFiscalYearPeriodID [{assetSchedule.fasGlFiscalYearPeriodID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAssetScheduleDto>>> Process_GetAllAssetSchedules(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAssetScheduleDto> allAssetSchedulesDto = new List<ERPAssetScheduleDto>();
		ERPResponseMessageDto<IList<ERPAssetScheduleDto>> result;
		try
		{
			IERPAssetScheduleRepository iERPAssetScheduleRepository = (base.ERPAssetScheduleRepository = new ERPAssetScheduleRepository(base.ApiClientContext));
			using (iERPAssetScheduleRepository)
			{
				foreach (ERPAssetScheduleInformationDto item2 in await base.ERPAssetScheduleRepository.GetAllAssetSchedules(pageSize, pageNumber, filter, orderBy))
				{
					ERPAssetScheduleDto item = new ERPAssetScheduleDto
					{
						fasActualProductionUnits = item2.fasActualProductionUnits,
						fasAdditionalAssetAmount = item2.fasAdditionalAssetAmount,
						fasAssetID = item2.fasAssetID,
						fasClosingAccumBalance = item2.fasClosingAccumBalance,
						fasClosingAssetValue = item2.fasClosingAssetValue,
						fasCreatedBy = item2.fasCreatedBy,
						fasCreatedDate = item2.fasCreatedDate,
						fasDepreciationAmount = item2.fasDepreciationAmount,
						fasUniqueID = item2.fasUniqueID,
						fasEstimatedProductionUnits = item2.fasEstimatedProductionUnits,
						fasGlFiscalYearID = item2.fasGlFiscalYearID,
						fasGlFiscalYearPeriodID = item2.fasGlFiscalYearPeriodID,
						fasPostedToGl = item2.fasPostedToGl,
						fasNetAssetValue = item2.fasNetAssetValue,
						fasOpeningAccumBalance = item2.fasOpeningAccumBalance,
						fasOpeningAssetValue = item2.fasOpeningAssetValue,
						fasRowVersion = item2.fasRowVersion,
						fasAssetScheduleID = item2.fasAssetScheduleID,
						fasSubtractAssetAmount = item2.fasSubtractAssetAmount,
						fasType = item2.fasType,
						fasWritebackAmount = item2.fasWritebackAmount,
						CustomFields = item2.CustomFields
					};
					allAssetSchedulesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all AssetSchedules]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAssetScheduleDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAssetSchedulesDto,
				RecordCount = allAssetSchedulesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAssetScheduleDto>> Process_GetAssetSchedule(Guid assetScheduleId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAssetScheduleDto assetScheduleDto = null;
		ERPResponseMessageDto<ERPAssetScheduleDto> result;
		try
		{
			IERPAssetScheduleRepository iERPAssetScheduleRepository = (base.ERPAssetScheduleRepository = new ERPAssetScheduleRepository(base.ApiClientContext));
			using (iERPAssetScheduleRepository)
			{
				ERPAssetScheduleInformationDto eRPAssetScheduleInformationDto = await base.ERPAssetScheduleRepository.GetAssetSchedule(assetScheduleId);
				assetScheduleDto = new ERPAssetScheduleDto
				{
					fasActualProductionUnits = eRPAssetScheduleInformationDto.fasActualProductionUnits,
					fasAdditionalAssetAmount = eRPAssetScheduleInformationDto.fasAdditionalAssetAmount,
					fasAssetID = eRPAssetScheduleInformationDto.fasAssetID,
					fasClosingAccumBalance = eRPAssetScheduleInformationDto.fasClosingAccumBalance,
					fasClosingAssetValue = eRPAssetScheduleInformationDto.fasClosingAssetValue,
					fasCreatedBy = eRPAssetScheduleInformationDto.fasCreatedBy,
					fasCreatedDate = eRPAssetScheduleInformationDto.fasCreatedDate,
					fasDepreciationAmount = eRPAssetScheduleInformationDto.fasDepreciationAmount,
					fasUniqueID = eRPAssetScheduleInformationDto.fasUniqueID,
					fasEstimatedProductionUnits = eRPAssetScheduleInformationDto.fasEstimatedProductionUnits,
					fasGlFiscalYearID = eRPAssetScheduleInformationDto.fasGlFiscalYearID,
					fasGlFiscalYearPeriodID = eRPAssetScheduleInformationDto.fasGlFiscalYearPeriodID,
					fasPostedToGl = eRPAssetScheduleInformationDto.fasPostedToGl,
					fasNetAssetValue = eRPAssetScheduleInformationDto.fasNetAssetValue,
					fasOpeningAccumBalance = eRPAssetScheduleInformationDto.fasOpeningAccumBalance,
					fasOpeningAssetValue = eRPAssetScheduleInformationDto.fasOpeningAssetValue,
					fasRowVersion = eRPAssetScheduleInformationDto.fasRowVersion,
					fasAssetScheduleID = eRPAssetScheduleInformationDto.fasAssetScheduleID,
					fasSubtractAssetAmount = eRPAssetScheduleInformationDto.fasSubtractAssetAmount,
					fasType = eRPAssetScheduleInformationDto.fasType,
					fasWritebackAmount = eRPAssetScheduleInformationDto.fasWritebackAmount,
					CustomFields = eRPAssetScheduleInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the AssetSchedules []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetScheduleDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = assetScheduleDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAssetScheduleDto>> Process_PutAssetSchedule(ERPAssetScheduleDto assetSchedule)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPAssetScheduleDto createdObject = null;
		ERPResponseMessageDto<ERPAssetScheduleDto> result;
		try
		{
			IERPAssetScheduleRepository iERPAssetScheduleRepository = (base.ERPAssetScheduleRepository = new ERPAssetScheduleRepository(base.ApiClientContext));
			using (iERPAssetScheduleRepository)
			{
				APIValidationInfoDto postResult = await base.ERPAssetScheduleRepository.SaveAssetSchedule(assetSchedule);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPAssetScheduleInformationDto eRPAssetScheduleInformationDto = await base.ERPAssetScheduleRepository.GetAssetSchedule(assetSchedule.fasUniqueID);
					createdObject = new ERPAssetScheduleDto
					{
						fasActualProductionUnits = eRPAssetScheduleInformationDto.fasActualProductionUnits,
						fasAdditionalAssetAmount = eRPAssetScheduleInformationDto.fasAdditionalAssetAmount,
						fasAssetID = eRPAssetScheduleInformationDto.fasAssetID,
						fasClosingAccumBalance = eRPAssetScheduleInformationDto.fasClosingAccumBalance,
						fasClosingAssetValue = eRPAssetScheduleInformationDto.fasClosingAssetValue,
						fasCreatedBy = eRPAssetScheduleInformationDto.fasCreatedBy,
						fasCreatedDate = eRPAssetScheduleInformationDto.fasCreatedDate,
						fasDepreciationAmount = eRPAssetScheduleInformationDto.fasDepreciationAmount,
						fasUniqueID = eRPAssetScheduleInformationDto.fasUniqueID,
						fasEstimatedProductionUnits = eRPAssetScheduleInformationDto.fasEstimatedProductionUnits,
						fasGlFiscalYearID = eRPAssetScheduleInformationDto.fasGlFiscalYearID,
						fasGlFiscalYearPeriodID = eRPAssetScheduleInformationDto.fasGlFiscalYearPeriodID,
						fasPostedToGl = eRPAssetScheduleInformationDto.fasPostedToGl,
						fasNetAssetValue = eRPAssetScheduleInformationDto.fasNetAssetValue,
						fasOpeningAccumBalance = eRPAssetScheduleInformationDto.fasOpeningAccumBalance,
						fasOpeningAssetValue = eRPAssetScheduleInformationDto.fasOpeningAssetValue,
						fasRowVersion = eRPAssetScheduleInformationDto.fasRowVersion,
						fasAssetScheduleID = eRPAssetScheduleInformationDto.fasAssetScheduleID,
						fasSubtractAssetAmount = eRPAssetScheduleInformationDto.fasSubtractAssetAmount,
						fasType = eRPAssetScheduleInformationDto.fasType,
						fasWritebackAmount = eRPAssetScheduleInformationDto.fasWritebackAmount,
						CustomFields = eRPAssetScheduleInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing AssetSchedule [{assetSchedule.fasUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetScheduleDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteAssetSchedule(Guid assetScheduleId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetScheduleRepository iERPAssetScheduleRepository = (base.ERPAssetScheduleRepository = new ERPAssetScheduleRepository(base.ApiClientContext));
		using (iERPAssetScheduleRepository)
		{
			if (!(await base.ERPAssetScheduleRepository.DoesAssetScheduleExist(assetScheduleId)))
			{
				base.ErrorsList.Add($"AssetSchedule [{assetScheduleId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPAssetScheduleInformationDto eRPAssetScheduleInformationDto = await base.ERPAssetScheduleRepository.GetAssetSchedule(assetScheduleId);
				string text = await base.ERPAssetScheduleRepository.WhereUsed("AssetSchedules", new object[2] { eRPAssetScheduleInformationDto.fasAssetID, eRPAssetScheduleInformationDto.fasAssetScheduleID }, new object[2] { "fasAssetID", "fasAssetScheduleID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("AssetSchedule cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPAssetScheduleDto>> Process_DeleteAssetSchedule(Guid assetScheduleId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPAssetScheduleDto> result;
		try
		{
			IERPAssetScheduleRepository iERPAssetScheduleRepository = (base.ERPAssetScheduleRepository = new ERPAssetScheduleRepository(base.ApiClientContext));
			using (iERPAssetScheduleRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPAssetScheduleRepository.DeleteRowFromTable("AssetSchedules", "fas", assetScheduleId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of AssetSchedule [{assetScheduleId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetScheduleDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPAssetScheduleDto()
			};
		}
		return result;
	}
}
