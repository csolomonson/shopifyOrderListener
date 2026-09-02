using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPCycleCodeModel : ERPBaseModel, IERPCycleCodeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllCycleCodes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCycleCodeRepository iERPCycleCodeRepository = (base.ERPCycleCodeRepository = new ERPCycleCodeRepository(base.ApiClientContext));
		using (iERPCycleCodeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPCycleCodeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPCycleCodeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPCycleCodeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPCycleCodeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetCycleCode(Guid cycleCodeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCycleCodeRepository iERPCycleCodeRepository = (base.ERPCycleCodeRepository = new ERPCycleCodeRepository(base.ApiClientContext));
		using (iERPCycleCodeRepository)
		{
			if (!(await base.ERPCycleCodeRepository.DoesCycleCodeExist(cycleCodeId)))
			{
				errorsList.Add($"CycleCode [{cycleCodeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutCycleCode(ERPCycleCodeDto cycleCode)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCycleCodeRepository iERPCycleCodeRepository = (base.ERPCycleCodeRepository = new ERPCycleCodeRepository(base.ApiClientContext));
		using (iERPCycleCodeRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPCycleCodeDto>>> Process_GetAllCycleCodes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPCycleCodeDto> allCycleCodesDto = new List<ERPCycleCodeDto>();
		ERPResponseMessageDto<IList<ERPCycleCodeDto>> result;
		try
		{
			IERPCycleCodeRepository iERPCycleCodeRepository = (base.ERPCycleCodeRepository = new ERPCycleCodeRepository(base.ApiClientContext));
			using (iERPCycleCodeRepository)
			{
				foreach (ERPCycleCodeInformationDto item2 in await base.ERPCycleCodeRepository.GetAllCycleCodes(pageSize, pageNumber, filter, orderBy))
				{
					ERPCycleCodeDto item = new ERPCycleCodeDto
					{
						imdCycleCodeID = item2.imdCycleCodeID,
						imdCreatedBy = item2.imdCreatedBy,
						imdCreatedDate = item2.imdCreatedDate,
						imdDescription = item2.imdDescription,
						imdUniqueID = item2.imdUniqueID,
						imdRowVersion = item2.imdRowVersion,
						CustomFields = item2.CustomFields
					};
					allCycleCodesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all CycleCodes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPCycleCodeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allCycleCodesDto,
				RecordCount = allCycleCodesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCycleCodeDto>> Process_GetCycleCode(Guid cycleCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPCycleCodeDto cycleCodeDto = null;
		ERPResponseMessageDto<ERPCycleCodeDto> result;
		try
		{
			IERPCycleCodeRepository iERPCycleCodeRepository = (base.ERPCycleCodeRepository = new ERPCycleCodeRepository(base.ApiClientContext));
			using (iERPCycleCodeRepository)
			{
				ERPCycleCodeInformationDto eRPCycleCodeInformationDto = await base.ERPCycleCodeRepository.GetCycleCode(cycleCodeId);
				cycleCodeDto = new ERPCycleCodeDto
				{
					imdCycleCodeID = eRPCycleCodeInformationDto.imdCycleCodeID,
					imdCreatedBy = eRPCycleCodeInformationDto.imdCreatedBy,
					imdCreatedDate = eRPCycleCodeInformationDto.imdCreatedDate,
					imdDescription = eRPCycleCodeInformationDto.imdDescription,
					imdUniqueID = eRPCycleCodeInformationDto.imdUniqueID,
					imdRowVersion = eRPCycleCodeInformationDto.imdRowVersion,
					CustomFields = eRPCycleCodeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the CycleCodes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCycleCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = cycleCodeDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCycleCodeDto>> Process_PutCycleCode(ERPCycleCodeDto cycleCode)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPCycleCodeDto createdObject = null;
		ERPResponseMessageDto<ERPCycleCodeDto> result;
		try
		{
			IERPCycleCodeRepository iERPCycleCodeRepository = (base.ERPCycleCodeRepository = new ERPCycleCodeRepository(base.ApiClientContext));
			using (iERPCycleCodeRepository)
			{
				APIValidationInfoDto postResult = await base.ERPCycleCodeRepository.SaveCycleCode(cycleCode);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPCycleCodeInformationDto eRPCycleCodeInformationDto = await base.ERPCycleCodeRepository.GetCycleCode(cycleCode.imdUniqueID);
					createdObject = new ERPCycleCodeDto
					{
						imdCycleCodeID = eRPCycleCodeInformationDto.imdCycleCodeID,
						imdCreatedBy = eRPCycleCodeInformationDto.imdCreatedBy,
						imdCreatedDate = eRPCycleCodeInformationDto.imdCreatedDate,
						imdDescription = eRPCycleCodeInformationDto.imdDescription,
						imdUniqueID = eRPCycleCodeInformationDto.imdUniqueID,
						imdRowVersion = eRPCycleCodeInformationDto.imdRowVersion,
						CustomFields = eRPCycleCodeInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing CycleCode [{cycleCode.imdUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCycleCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteCycleCode(Guid cycleCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCycleCodeRepository iERPCycleCodeRepository = (base.ERPCycleCodeRepository = new ERPCycleCodeRepository(base.ApiClientContext));
		using (iERPCycleCodeRepository)
		{
			if (!(await base.ERPCycleCodeRepository.DoesCycleCodeExist(cycleCodeId)))
			{
				base.ErrorsList.Add($"CycleCode [{cycleCodeId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPCycleCodeInformationDto eRPCycleCodeInformationDto = await base.ERPCycleCodeRepository.GetCycleCode(cycleCodeId);
				string text = await base.ERPCycleCodeRepository.WhereUsed("CycleCodes", new object[1] { eRPCycleCodeInformationDto.imdCycleCodeID }, new object[1] { "imdCycleCodeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("CycleCode cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPCycleCodeDto>> Process_DeleteCycleCode(Guid cycleCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPCycleCodeDto> result;
		try
		{
			IERPCycleCodeRepository iERPCycleCodeRepository = (base.ERPCycleCodeRepository = new ERPCycleCodeRepository(base.ApiClientContext));
			using (iERPCycleCodeRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPCycleCodeRepository.DeleteRowFromTable("CycleCodes", "imd", cycleCodeId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of CycleCode [{cycleCodeId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCycleCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPCycleCodeDto()
			};
		}
		return result;
	}
}
