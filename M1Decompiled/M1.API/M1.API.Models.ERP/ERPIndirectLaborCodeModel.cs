using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPIndirectLaborCodeModel : ERPBaseModel, IERPIndirectLaborCodeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllIndirectLaborCodes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPIndirectLaborCodeRepository iERPIndirectLaborCodeRepository = (base.ERPIndirectLaborCodeRepository = new ERPIndirectLaborCodeRepository(base.ApiClientContext));
		using (iERPIndirectLaborCodeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPIndirectLaborCodeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPIndirectLaborCodeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPIndirectLaborCodeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPIndirectLaborCodeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetIndirectLaborCode(Guid indirectLaborCodeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPIndirectLaborCodeRepository iERPIndirectLaborCodeRepository = (base.ERPIndirectLaborCodeRepository = new ERPIndirectLaborCodeRepository(base.ApiClientContext));
		using (iERPIndirectLaborCodeRepository)
		{
			if (!(await base.ERPIndirectLaborCodeRepository.DoesIndirectLaborCodeExist(indirectLaborCodeId)))
			{
				errorsList.Add($"IndirectLaborCode [{indirectLaborCodeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutIndirectLaborCode(ERPIndirectLaborCodeDto indirectLaborCode)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPIndirectLaborCodeRepository iERPIndirectLaborCodeRepository = (base.ERPIndirectLaborCodeRepository = new ERPIndirectLaborCodeRepository(base.ApiClientContext));
		using (iERPIndirectLaborCodeRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPIndirectLaborCodeDto>>> Process_GetAllIndirectLaborCodes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPIndirectLaborCodeDto> allIndirectLaborCodesDto = new List<ERPIndirectLaborCodeDto>();
		ERPResponseMessageDto<IList<ERPIndirectLaborCodeDto>> result;
		try
		{
			IERPIndirectLaborCodeRepository iERPIndirectLaborCodeRepository = (base.ERPIndirectLaborCodeRepository = new ERPIndirectLaborCodeRepository(base.ApiClientContext));
			using (iERPIndirectLaborCodeRepository)
			{
				foreach (ERPIndirectLaborCodeInformationDto item2 in await base.ERPIndirectLaborCodeRepository.GetAllIndirectLaborCodes(pageSize, pageNumber, filter, orderBy))
				{
					ERPIndirectLaborCodeDto item = new ERPIndirectLaborCodeDto
					{
						lmiCreatedBy = item2.lmiCreatedBy,
						lmiCreatedDate = item2.lmiCreatedDate,
						lmiDescription = item2.lmiDescription,
						lmiUniqueID = item2.lmiUniqueID,
						lmiInactiveDate = item2.lmiInactiveDate,
						lmiIndirectLaborID = item2.lmiIndirectLaborID,
						lmiIndirectLaborType = item2.lmiIndirectLaborType,
						lmiInactive = item2.lmiInactive,
						lmiRowVersion = item2.lmiRowVersion,
						CustomFields = item2.CustomFields
					};
					allIndirectLaborCodesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all IndirectLaborCodes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPIndirectLaborCodeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allIndirectLaborCodesDto,
				RecordCount = allIndirectLaborCodesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPIndirectLaborCodeDto>> Process_GetIndirectLaborCode(Guid indirectLaborCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPIndirectLaborCodeDto indirectLaborCodeDto = null;
		ERPResponseMessageDto<ERPIndirectLaborCodeDto> result;
		try
		{
			IERPIndirectLaborCodeRepository iERPIndirectLaborCodeRepository = (base.ERPIndirectLaborCodeRepository = new ERPIndirectLaborCodeRepository(base.ApiClientContext));
			using (iERPIndirectLaborCodeRepository)
			{
				ERPIndirectLaborCodeInformationDto eRPIndirectLaborCodeInformationDto = await base.ERPIndirectLaborCodeRepository.GetIndirectLaborCode(indirectLaborCodeId);
				indirectLaborCodeDto = new ERPIndirectLaborCodeDto
				{
					lmiCreatedBy = eRPIndirectLaborCodeInformationDto.lmiCreatedBy,
					lmiCreatedDate = eRPIndirectLaborCodeInformationDto.lmiCreatedDate,
					lmiDescription = eRPIndirectLaborCodeInformationDto.lmiDescription,
					lmiUniqueID = eRPIndirectLaborCodeInformationDto.lmiUniqueID,
					lmiInactiveDate = eRPIndirectLaborCodeInformationDto.lmiInactiveDate,
					lmiIndirectLaborID = eRPIndirectLaborCodeInformationDto.lmiIndirectLaborID,
					lmiIndirectLaborType = eRPIndirectLaborCodeInformationDto.lmiIndirectLaborType,
					lmiInactive = eRPIndirectLaborCodeInformationDto.lmiInactive,
					lmiRowVersion = eRPIndirectLaborCodeInformationDto.lmiRowVersion,
					CustomFields = eRPIndirectLaborCodeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the IndirectLaborCodes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPIndirectLaborCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = indirectLaborCodeDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPIndirectLaborCodeDto>> Process_PutIndirectLaborCode(ERPIndirectLaborCodeDto indirectLaborCode)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPIndirectLaborCodeDto createdObject = null;
		ERPResponseMessageDto<ERPIndirectLaborCodeDto> result;
		try
		{
			IERPIndirectLaborCodeRepository iERPIndirectLaborCodeRepository = (base.ERPIndirectLaborCodeRepository = new ERPIndirectLaborCodeRepository(base.ApiClientContext));
			using (iERPIndirectLaborCodeRepository)
			{
				APIValidationInfoDto postResult = await base.ERPIndirectLaborCodeRepository.SaveIndirectLaborCode(indirectLaborCode);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPIndirectLaborCodeInformationDto eRPIndirectLaborCodeInformationDto = await base.ERPIndirectLaborCodeRepository.GetIndirectLaborCode(indirectLaborCode.lmiUniqueID);
					createdObject = new ERPIndirectLaborCodeDto
					{
						lmiCreatedBy = eRPIndirectLaborCodeInformationDto.lmiCreatedBy,
						lmiCreatedDate = eRPIndirectLaborCodeInformationDto.lmiCreatedDate,
						lmiDescription = eRPIndirectLaborCodeInformationDto.lmiDescription,
						lmiUniqueID = eRPIndirectLaborCodeInformationDto.lmiUniqueID,
						lmiInactiveDate = eRPIndirectLaborCodeInformationDto.lmiInactiveDate,
						lmiIndirectLaborID = eRPIndirectLaborCodeInformationDto.lmiIndirectLaborID,
						lmiIndirectLaborType = eRPIndirectLaborCodeInformationDto.lmiIndirectLaborType,
						lmiInactive = eRPIndirectLaborCodeInformationDto.lmiInactive,
						lmiRowVersion = eRPIndirectLaborCodeInformationDto.lmiRowVersion,
						CustomFields = eRPIndirectLaborCodeInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing IndirectLaborCode [{indirectLaborCode.lmiUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPIndirectLaborCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteIndirectLaborCode(Guid indirectLaborCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPIndirectLaborCodeRepository iERPIndirectLaborCodeRepository = (base.ERPIndirectLaborCodeRepository = new ERPIndirectLaborCodeRepository(base.ApiClientContext));
		using (iERPIndirectLaborCodeRepository)
		{
			if (!(await base.ERPIndirectLaborCodeRepository.DoesIndirectLaborCodeExist(indirectLaborCodeId)))
			{
				base.ErrorsList.Add($"IndirectLaborCode [{indirectLaborCodeId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPIndirectLaborCodeInformationDto eRPIndirectLaborCodeInformationDto = await base.ERPIndirectLaborCodeRepository.GetIndirectLaborCode(indirectLaborCodeId);
				string text = await base.ERPIndirectLaborCodeRepository.WhereUsed("IndirectLaborCodes", new object[1] { eRPIndirectLaborCodeInformationDto.lmiIndirectLaborID }, new object[1] { "lmiIndirectLaborID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("IndirectLaborCode cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPIndirectLaborCodeDto>> Process_DeleteIndirectLaborCode(Guid indirectLaborCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPIndirectLaborCodeDto> result;
		try
		{
			IERPIndirectLaborCodeRepository iERPIndirectLaborCodeRepository = (base.ERPIndirectLaborCodeRepository = new ERPIndirectLaborCodeRepository(base.ApiClientContext));
			using (iERPIndirectLaborCodeRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPIndirectLaborCodeRepository.DeleteRowFromTable("IndirectLaborCodes", "lmi", indirectLaborCodeId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of IndirectLaborCode [{indirectLaborCodeId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPIndirectLaborCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPIndirectLaborCodeDto()
			};
		}
		return result;
	}
}
