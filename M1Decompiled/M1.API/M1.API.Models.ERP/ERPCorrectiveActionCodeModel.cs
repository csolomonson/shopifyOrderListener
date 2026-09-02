using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPCorrectiveActionCodeModel : ERPBaseModel, IERPCorrectiveActionCodeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllCorrectiveActionCodes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCorrectiveActionCodeRepository iERPCorrectiveActionCodeRepository = (base.ERPCorrectiveActionCodeRepository = new ERPCorrectiveActionCodeRepository(base.ApiClientContext));
		using (iERPCorrectiveActionCodeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPCorrectiveActionCodeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPCorrectiveActionCodeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPCorrectiveActionCodeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPCorrectiveActionCodeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetCorrectiveActionCode(Guid correctiveActionCodeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCorrectiveActionCodeRepository iERPCorrectiveActionCodeRepository = (base.ERPCorrectiveActionCodeRepository = new ERPCorrectiveActionCodeRepository(base.ApiClientContext));
		using (iERPCorrectiveActionCodeRepository)
		{
			if (!(await base.ERPCorrectiveActionCodeRepository.DoesCorrectiveActionCodeExist(correctiveActionCodeId)))
			{
				errorsList.Add($"CorrectiveActionCode [{correctiveActionCodeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutCorrectiveActionCode(ERPCorrectiveActionCodeDto correctiveActionCode)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCorrectiveActionCodeRepository iERPCorrectiveActionCodeRepository = (base.ERPCorrectiveActionCodeRepository = new ERPCorrectiveActionCodeRepository(base.ApiClientContext));
		using (iERPCorrectiveActionCodeRepository)
		{
			if (!string.IsNullOrWhiteSpace(correctiveActionCode.qaoCorrectiveActionCategoryID) && !(await base.ERPCorrectiveActionCodeRepository.DoesRecordExistInTableUsingKeys("CorrectiveActionCategories", new object[1] { "QATCORRECTIVEACTIONCATEGORYID" }, new object[1] { correctiveActionCode.qaoCorrectiveActionCategoryID })))
			{
				errorsList.Add("qaoCorrectiveActionCategoryID [" + correctiveActionCode.qaoCorrectiveActionCategoryID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPCorrectiveActionCodeDto>>> Process_GetAllCorrectiveActionCodes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPCorrectiveActionCodeDto> allCorrectiveActionCodesDto = new List<ERPCorrectiveActionCodeDto>();
		ERPResponseMessageDto<IList<ERPCorrectiveActionCodeDto>> result;
		try
		{
			IERPCorrectiveActionCodeRepository iERPCorrectiveActionCodeRepository = (base.ERPCorrectiveActionCodeRepository = new ERPCorrectiveActionCodeRepository(base.ApiClientContext));
			using (iERPCorrectiveActionCodeRepository)
			{
				foreach (ERPCorrectiveActionCodeInformationDto item2 in await base.ERPCorrectiveActionCodeRepository.GetAllCorrectiveActionCodes(pageSize, pageNumber, filter, orderBy))
				{
					ERPCorrectiveActionCodeDto item = new ERPCorrectiveActionCodeDto
					{
						qaoCorrectiveActionCodeID = item2.qaoCorrectiveActionCodeID,
						qaoCorrectiveActionCategoryID = item2.qaoCorrectiveActionCategoryID,
						qaoCreatedBy = item2.qaoCreatedBy,
						qaoCreatedDate = item2.qaoCreatedDate,
						qaoDescription = item2.qaoDescription,
						qaoUniqueID = item2.qaoUniqueID,
						qaoHoursAllowed = item2.qaoHoursAllowed,
						qaoRowVersion = item2.qaoRowVersion,
						CustomFields = item2.CustomFields
					};
					allCorrectiveActionCodesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all CorrectiveActionCodes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPCorrectiveActionCodeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allCorrectiveActionCodesDto,
				RecordCount = allCorrectiveActionCodesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCorrectiveActionCodeDto>> Process_GetCorrectiveActionCode(Guid correctiveActionCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPCorrectiveActionCodeDto correctiveActionCodeDto = null;
		ERPResponseMessageDto<ERPCorrectiveActionCodeDto> result;
		try
		{
			IERPCorrectiveActionCodeRepository iERPCorrectiveActionCodeRepository = (base.ERPCorrectiveActionCodeRepository = new ERPCorrectiveActionCodeRepository(base.ApiClientContext));
			using (iERPCorrectiveActionCodeRepository)
			{
				ERPCorrectiveActionCodeInformationDto eRPCorrectiveActionCodeInformationDto = await base.ERPCorrectiveActionCodeRepository.GetCorrectiveActionCode(correctiveActionCodeId);
				correctiveActionCodeDto = new ERPCorrectiveActionCodeDto
				{
					qaoCorrectiveActionCodeID = eRPCorrectiveActionCodeInformationDto.qaoCorrectiveActionCodeID,
					qaoCorrectiveActionCategoryID = eRPCorrectiveActionCodeInformationDto.qaoCorrectiveActionCategoryID,
					qaoCreatedBy = eRPCorrectiveActionCodeInformationDto.qaoCreatedBy,
					qaoCreatedDate = eRPCorrectiveActionCodeInformationDto.qaoCreatedDate,
					qaoDescription = eRPCorrectiveActionCodeInformationDto.qaoDescription,
					qaoUniqueID = eRPCorrectiveActionCodeInformationDto.qaoUniqueID,
					qaoHoursAllowed = eRPCorrectiveActionCodeInformationDto.qaoHoursAllowed,
					qaoRowVersion = eRPCorrectiveActionCodeInformationDto.qaoRowVersion,
					CustomFields = eRPCorrectiveActionCodeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the CorrectiveActionCodes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCorrectiveActionCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = correctiveActionCodeDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCorrectiveActionCodeDto>> Process_PutCorrectiveActionCode(ERPCorrectiveActionCodeDto correctiveActionCode)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPCorrectiveActionCodeDto createdObject = null;
		ERPResponseMessageDto<ERPCorrectiveActionCodeDto> result;
		try
		{
			IERPCorrectiveActionCodeRepository iERPCorrectiveActionCodeRepository = (base.ERPCorrectiveActionCodeRepository = new ERPCorrectiveActionCodeRepository(base.ApiClientContext));
			using (iERPCorrectiveActionCodeRepository)
			{
				APIValidationInfoDto postResult = await base.ERPCorrectiveActionCodeRepository.SaveCorrectiveActionCode(correctiveActionCode);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPCorrectiveActionCodeInformationDto eRPCorrectiveActionCodeInformationDto = await base.ERPCorrectiveActionCodeRepository.GetCorrectiveActionCode(correctiveActionCode.qaoUniqueID);
					createdObject = new ERPCorrectiveActionCodeDto
					{
						qaoCorrectiveActionCodeID = eRPCorrectiveActionCodeInformationDto.qaoCorrectiveActionCodeID,
						qaoCorrectiveActionCategoryID = eRPCorrectiveActionCodeInformationDto.qaoCorrectiveActionCategoryID,
						qaoCreatedBy = eRPCorrectiveActionCodeInformationDto.qaoCreatedBy,
						qaoCreatedDate = eRPCorrectiveActionCodeInformationDto.qaoCreatedDate,
						qaoDescription = eRPCorrectiveActionCodeInformationDto.qaoDescription,
						qaoUniqueID = eRPCorrectiveActionCodeInformationDto.qaoUniqueID,
						qaoHoursAllowed = eRPCorrectiveActionCodeInformationDto.qaoHoursAllowed,
						qaoRowVersion = eRPCorrectiveActionCodeInformationDto.qaoRowVersion,
						CustomFields = eRPCorrectiveActionCodeInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing CorrectiveActionCode [{correctiveActionCode.qaoUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCorrectiveActionCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteCorrectiveActionCode(Guid correctiveActionCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCorrectiveActionCodeRepository iERPCorrectiveActionCodeRepository = (base.ERPCorrectiveActionCodeRepository = new ERPCorrectiveActionCodeRepository(base.ApiClientContext));
		using (iERPCorrectiveActionCodeRepository)
		{
			if (!(await base.ERPCorrectiveActionCodeRepository.DoesCorrectiveActionCodeExist(correctiveActionCodeId)))
			{
				base.ErrorsList.Add($"CorrectiveActionCode [{correctiveActionCodeId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPCorrectiveActionCodeInformationDto eRPCorrectiveActionCodeInformationDto = await base.ERPCorrectiveActionCodeRepository.GetCorrectiveActionCode(correctiveActionCodeId);
				string text = await base.ERPCorrectiveActionCodeRepository.WhereUsed("CorrectiveActionCodes", new object[1] { eRPCorrectiveActionCodeInformationDto.qaoCorrectiveActionCodeID }, new object[1] { "qaoCorrectiveActionCodeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("CorrectiveActionCode cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPCorrectiveActionCodeDto>> Process_DeleteCorrectiveActionCode(Guid correctiveActionCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPCorrectiveActionCodeDto> result;
		try
		{
			IERPCorrectiveActionCodeRepository iERPCorrectiveActionCodeRepository = (base.ERPCorrectiveActionCodeRepository = new ERPCorrectiveActionCodeRepository(base.ApiClientContext));
			using (iERPCorrectiveActionCodeRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPCorrectiveActionCodeRepository.DeleteRowFromTable("CorrectiveActionCodes", "qao", correctiveActionCodeId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of CorrectiveActionCode [{correctiveActionCodeId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCorrectiveActionCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPCorrectiveActionCodeDto()
			};
		}
		return result;
	}
}
