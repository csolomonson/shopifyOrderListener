using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartForecastModel : ERPBaseModel, IERPPartForecastModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartForecasts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartForecastRepository iERPPartForecastRepository = (base.ERPPartForecastRepository = new ERPPartForecastRepository(base.ApiClientContext));
		using (iERPPartForecastRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartForecastRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartForecastRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartForecastRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartForecastRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartForecast(Guid partForecastId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartForecastRepository iERPPartForecastRepository = (base.ERPPartForecastRepository = new ERPPartForecastRepository(base.ApiClientContext));
		using (iERPPartForecastRepository)
		{
			if (!(await base.ERPPartForecastRepository.DoesPartForecastExist(partForecastId)))
			{
				errorsList.Add($"PartForecast [{partForecastId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartForecast(ERPPartForecastDto partForecast)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartForecastRepository iERPPartForecastRepository = (base.ERPPartForecastRepository = new ERPPartForecastRepository(base.ApiClientContext));
		using (iERPPartForecastRepository)
		{
			if (!string.IsNullOrWhiteSpace(partForecast.inpPartID) && !(await base.ERPPartForecastRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partForecast.inpPartID })))
			{
				errorsList.Add("inpPartID [" + partForecast.inpPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partForecast.inpPartRevisionID) && !(await base.ERPPartForecastRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partForecast.inpPartID, partForecast.inpPartRevisionID })))
			{
				errorsList.Add("inpPartRevisionID [" + partForecast.inpPartRevisionID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartForecastDto>>> Process_GetAllPartForecasts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartForecastDto> allPartForecastsDto = new List<ERPPartForecastDto>();
		ERPResponseMessageDto<IList<ERPPartForecastDto>> result;
		try
		{
			IERPPartForecastRepository iERPPartForecastRepository = (base.ERPPartForecastRepository = new ERPPartForecastRepository(base.ApiClientContext));
			using (iERPPartForecastRepository)
			{
				foreach (ERPPartForecastInformationDto item2 in await base.ERPPartForecastRepository.GetAllPartForecasts(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartForecastDto item = new ERPPartForecastDto
					{
						inpAnnualQuantity = item2.inpAnnualQuantity,
						inpCreatedBy = item2.inpCreatedBy,
						inpCreatedDate = item2.inpCreatedDate,
						inpEndDate = item2.inpEndDate,
						inpUniqueID = item2.inpUniqueID,
						inpForecastMethod = item2.inpForecastMethod,
						inpForecastNumberOfYears = item2.inpForecastNumberOfYears,
						inpIntervalType = item2.inpIntervalType,
						inpPartForecastYearID = item2.inpPartForecastYearID,
						inpPartID = item2.inpPartID,
						inpPartRevisionID = item2.inpPartRevisionID,
						inpRowVersion = item2.inpRowVersion,
						inpStartDate = item2.inpStartDate,
						CustomFields = item2.CustomFields
					};
					allPartForecastsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartForecasts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartForecastDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartForecastsDto,
				RecordCount = allPartForecastsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartForecastDto>> Process_GetPartForecast(Guid partForecastId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartForecastDto partForecastDto = null;
		ERPResponseMessageDto<ERPPartForecastDto> result;
		try
		{
			IERPPartForecastRepository iERPPartForecastRepository = (base.ERPPartForecastRepository = new ERPPartForecastRepository(base.ApiClientContext));
			using (iERPPartForecastRepository)
			{
				ERPPartForecastInformationDto eRPPartForecastInformationDto = await base.ERPPartForecastRepository.GetPartForecast(partForecastId);
				partForecastDto = new ERPPartForecastDto
				{
					inpAnnualQuantity = eRPPartForecastInformationDto.inpAnnualQuantity,
					inpCreatedBy = eRPPartForecastInformationDto.inpCreatedBy,
					inpCreatedDate = eRPPartForecastInformationDto.inpCreatedDate,
					inpEndDate = eRPPartForecastInformationDto.inpEndDate,
					inpUniqueID = eRPPartForecastInformationDto.inpUniqueID,
					inpForecastMethod = eRPPartForecastInformationDto.inpForecastMethod,
					inpForecastNumberOfYears = eRPPartForecastInformationDto.inpForecastNumberOfYears,
					inpIntervalType = eRPPartForecastInformationDto.inpIntervalType,
					inpPartForecastYearID = eRPPartForecastInformationDto.inpPartForecastYearID,
					inpPartID = eRPPartForecastInformationDto.inpPartID,
					inpPartRevisionID = eRPPartForecastInformationDto.inpPartRevisionID,
					inpRowVersion = eRPPartForecastInformationDto.inpRowVersion,
					inpStartDate = eRPPartForecastInformationDto.inpStartDate,
					CustomFields = eRPPartForecastInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartForecasts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartForecastDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partForecastDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartForecastDto>> Process_PutPartForecast(ERPPartForecastDto partForecast)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartForecastDto createdObject = null;
		ERPResponseMessageDto<ERPPartForecastDto> result;
		try
		{
			IERPPartForecastRepository iERPPartForecastRepository = (base.ERPPartForecastRepository = new ERPPartForecastRepository(base.ApiClientContext));
			using (iERPPartForecastRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartForecastRepository.SavePartForecast(partForecast);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartForecastInformationDto eRPPartForecastInformationDto = await base.ERPPartForecastRepository.GetPartForecast(partForecast.inpUniqueID);
					createdObject = new ERPPartForecastDto
					{
						inpAnnualQuantity = eRPPartForecastInformationDto.inpAnnualQuantity,
						inpCreatedBy = eRPPartForecastInformationDto.inpCreatedBy,
						inpCreatedDate = eRPPartForecastInformationDto.inpCreatedDate,
						inpEndDate = eRPPartForecastInformationDto.inpEndDate,
						inpUniqueID = eRPPartForecastInformationDto.inpUniqueID,
						inpForecastMethod = eRPPartForecastInformationDto.inpForecastMethod,
						inpForecastNumberOfYears = eRPPartForecastInformationDto.inpForecastNumberOfYears,
						inpIntervalType = eRPPartForecastInformationDto.inpIntervalType,
						inpPartForecastYearID = eRPPartForecastInformationDto.inpPartForecastYearID,
						inpPartID = eRPPartForecastInformationDto.inpPartID,
						inpPartRevisionID = eRPPartForecastInformationDto.inpPartRevisionID,
						inpRowVersion = eRPPartForecastInformationDto.inpRowVersion,
						inpStartDate = eRPPartForecastInformationDto.inpStartDate,
						CustomFields = eRPPartForecastInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartForecast [{partForecast.inpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartForecastDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartForecast(Guid partForecastId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartForecastRepository iERPPartForecastRepository = (base.ERPPartForecastRepository = new ERPPartForecastRepository(base.ApiClientContext));
		using (iERPPartForecastRepository)
		{
			if (!(await base.ERPPartForecastRepository.DoesPartForecastExist(partForecastId)))
			{
				base.ErrorsList.Add($"PartForecast [{partForecastId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartForecastInformationDto eRPPartForecastInformationDto = await base.ERPPartForecastRepository.GetPartForecast(partForecastId);
				string text = await base.ERPPartForecastRepository.WhereUsed("PartForecasts", new object[3] { eRPPartForecastInformationDto.inpPartID, eRPPartForecastInformationDto.inpPartRevisionID, eRPPartForecastInformationDto.inpPartForecastYearID }, new object[3] { "inpPartID", "inpPartRevisionID", "inpPartForecastYearID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartForecast cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartForecastDto>> Process_DeletePartForecast(Guid partForecastId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartForecastDto> result;
		try
		{
			IERPPartForecastRepository iERPPartForecastRepository = (base.ERPPartForecastRepository = new ERPPartForecastRepository(base.ApiClientContext));
			using (iERPPartForecastRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartForecastRepository.DeleteRowFromTable("PartForecasts", "inp", partForecastId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartForecast [{partForecastId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartForecastDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartForecastDto()
			};
		}
		return result;
	}
}
