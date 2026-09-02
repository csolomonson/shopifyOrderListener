using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPCountyCodeModel : ERPBaseModel, IERPCountyCodeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllCountyCodes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCountyCodeRepository iERPCountyCodeRepository = (base.ERPCountyCodeRepository = new ERPCountyCodeRepository(base.ApiClientContext));
		using (iERPCountyCodeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPCountyCodeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPCountyCodeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPCountyCodeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPCountyCodeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetCountyCode(Guid countyCodeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCountyCodeRepository iERPCountyCodeRepository = (base.ERPCountyCodeRepository = new ERPCountyCodeRepository(base.ApiClientContext));
		using (iERPCountyCodeRepository)
		{
			if (!(await base.ERPCountyCodeRepository.DoesCountyCodeExist(countyCodeId)))
			{
				errorsList.Add($"CountyCode [{countyCodeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutCountyCode(ERPCountyCodeDto countyCode)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCountyCodeRepository iERPCountyCodeRepository = (base.ERPCountyCodeRepository = new ERPCountyCodeRepository(base.ApiClientContext));
		using (iERPCountyCodeRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPCountyCodeDto>>> Process_GetAllCountyCodes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPCountyCodeDto> allCountyCodesDto = new List<ERPCountyCodeDto>();
		ERPResponseMessageDto<IList<ERPCountyCodeDto>> result;
		try
		{
			IERPCountyCodeRepository iERPCountyCodeRepository = (base.ERPCountyCodeRepository = new ERPCountyCodeRepository(base.ApiClientContext));
			using (iERPCountyCodeRepository)
			{
				foreach (ERPCountyCodeInformationDto item2 in await base.ERPCountyCodeRepository.GetAllCountyCodes(pageSize, pageNumber, filter, orderBy))
				{
					ERPCountyCodeDto item = new ERPCountyCodeDto
					{
						xccCountyCodeID = item2.xccCountyCodeID,
						xccCounty = item2.xccCounty,
						xccCountyCode = item2.xccCountyCode,
						xccCreatedBy = item2.xccCreatedBy,
						xccCreatedDate = item2.xccCreatedDate,
						xccUniqueID = item2.xccUniqueID,
						xccRowVersion = item2.XCCRowVersion,
						xccStateCode = item2.xccStateCode,
						CustomFields = item2.CustomFields
					};
					allCountyCodesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all CountyCodes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPCountyCodeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allCountyCodesDto,
				RecordCount = allCountyCodesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCountyCodeDto>> Process_GetCountyCode(Guid countyCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPCountyCodeDto countyCodeDto = null;
		ERPResponseMessageDto<ERPCountyCodeDto> result;
		try
		{
			IERPCountyCodeRepository iERPCountyCodeRepository = (base.ERPCountyCodeRepository = new ERPCountyCodeRepository(base.ApiClientContext));
			using (iERPCountyCodeRepository)
			{
				ERPCountyCodeInformationDto eRPCountyCodeInformationDto = await base.ERPCountyCodeRepository.GetCountyCode(countyCodeId);
				countyCodeDto = new ERPCountyCodeDto
				{
					xccCountyCodeID = eRPCountyCodeInformationDto.xccCountyCodeID,
					xccCounty = eRPCountyCodeInformationDto.xccCounty,
					xccCountyCode = eRPCountyCodeInformationDto.xccCountyCode,
					xccCreatedBy = eRPCountyCodeInformationDto.xccCreatedBy,
					xccCreatedDate = eRPCountyCodeInformationDto.xccCreatedDate,
					xccUniqueID = eRPCountyCodeInformationDto.xccUniqueID,
					xccRowVersion = eRPCountyCodeInformationDto.XCCRowVersion,
					xccStateCode = eRPCountyCodeInformationDto.xccStateCode,
					CustomFields = eRPCountyCodeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the CountyCodes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCountyCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = countyCodeDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCountyCodeDto>> Process_PutCountyCode(ERPCountyCodeDto countyCode)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPCountyCodeDto createdObject = null;
		ERPResponseMessageDto<ERPCountyCodeDto> result;
		try
		{
			IERPCountyCodeRepository iERPCountyCodeRepository = (base.ERPCountyCodeRepository = new ERPCountyCodeRepository(base.ApiClientContext));
			using (iERPCountyCodeRepository)
			{
				APIValidationInfoDto postResult = await base.ERPCountyCodeRepository.SaveCountyCode(countyCode);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPCountyCodeInformationDto eRPCountyCodeInformationDto = await base.ERPCountyCodeRepository.GetCountyCode(countyCode.xccUniqueID);
					createdObject = new ERPCountyCodeDto
					{
						xccCountyCodeID = eRPCountyCodeInformationDto.xccCountyCodeID,
						xccCounty = eRPCountyCodeInformationDto.xccCounty,
						xccCountyCode = eRPCountyCodeInformationDto.xccCountyCode,
						xccCreatedBy = eRPCountyCodeInformationDto.xccCreatedBy,
						xccCreatedDate = eRPCountyCodeInformationDto.xccCreatedDate,
						xccUniqueID = eRPCountyCodeInformationDto.xccUniqueID,
						xccRowVersion = eRPCountyCodeInformationDto.XCCRowVersion,
						xccStateCode = eRPCountyCodeInformationDto.xccStateCode,
						CustomFields = eRPCountyCodeInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing CountyCode [{countyCode.xccUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCountyCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteCountyCode(Guid countyCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCountyCodeRepository iERPCountyCodeRepository = (base.ERPCountyCodeRepository = new ERPCountyCodeRepository(base.ApiClientContext));
		using (iERPCountyCodeRepository)
		{
			if (!(await base.ERPCountyCodeRepository.DoesCountyCodeExist(countyCodeId)))
			{
				base.ErrorsList.Add($"CountyCode [{countyCodeId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPCountyCodeInformationDto eRPCountyCodeInformationDto = await base.ERPCountyCodeRepository.GetCountyCode(countyCodeId);
				string text = await base.ERPCountyCodeRepository.WhereUsed("CountyCodes", new object[1] { eRPCountyCodeInformationDto.xccCountyCodeID }, new object[1] { "xccCountyCodeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("CountyCode cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPCountyCodeDto>> Process_DeleteCountyCode(Guid countyCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPCountyCodeDto> result;
		try
		{
			IERPCountyCodeRepository iERPCountyCodeRepository = (base.ERPCountyCodeRepository = new ERPCountyCodeRepository(base.ApiClientContext));
			using (iERPCountyCodeRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPCountyCodeRepository.DeleteRowFromTable("CountyCodes", "xcc", countyCodeId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of CountyCode [{countyCodeId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCountyCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPCountyCodeDto()
			};
		}
		return result;
	}
}
