using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPNonConformanceCodeModel : ERPBaseModel, IERPNonConformanceCodeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllNonConformanceCodes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPNonConformanceCodeRepository iERPNonConformanceCodeRepository = (base.ERPNonConformanceCodeRepository = new ERPNonConformanceCodeRepository(base.ApiClientContext));
		using (iERPNonConformanceCodeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPNonConformanceCodeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPNonConformanceCodeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPNonConformanceCodeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPNonConformanceCodeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetNonConformanceCode(Guid nonConformanceCodeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPNonConformanceCodeRepository iERPNonConformanceCodeRepository = (base.ERPNonConformanceCodeRepository = new ERPNonConformanceCodeRepository(base.ApiClientContext));
		using (iERPNonConformanceCodeRepository)
		{
			if (!(await base.ERPNonConformanceCodeRepository.DoesNonConformanceCodeExist(nonConformanceCodeId)))
			{
				errorsList.Add($"NonConformanceCode [{nonConformanceCodeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutNonConformanceCode(ERPNonConformanceCodeDto nonConformanceCode)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPNonConformanceCodeRepository iERPNonConformanceCodeRepository = (base.ERPNonConformanceCodeRepository = new ERPNonConformanceCodeRepository(base.ApiClientContext));
		using (iERPNonConformanceCodeRepository)
		{
			if (!string.IsNullOrWhiteSpace(nonConformanceCode.qacNonConformanceCategoryID) && !(await base.ERPNonConformanceCodeRepository.DoesRecordExistInTableUsingKeys("NonConformanceCategories", new object[1] { "QAGNONCONFORMANCECATEGORYID" }, new object[1] { nonConformanceCode.qacNonConformanceCategoryID })))
			{
				errorsList.Add("qacNonConformanceCategoryID [" + nonConformanceCode.qacNonConformanceCategoryID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPNonConformanceCodeDto>>> Process_GetAllNonConformanceCodes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPNonConformanceCodeDto> allNonConformanceCodesDto = new List<ERPNonConformanceCodeDto>();
		ERPResponseMessageDto<IList<ERPNonConformanceCodeDto>> result;
		try
		{
			IERPNonConformanceCodeRepository iERPNonConformanceCodeRepository = (base.ERPNonConformanceCodeRepository = new ERPNonConformanceCodeRepository(base.ApiClientContext));
			using (iERPNonConformanceCodeRepository)
			{
				foreach (ERPNonConformanceCodeInformationDto item2 in await base.ERPNonConformanceCodeRepository.GetAllNonConformanceCodes(pageSize, pageNumber, filter, orderBy))
				{
					ERPNonConformanceCodeDto item = new ERPNonConformanceCodeDto
					{
						qacNonConformanceCodeID = item2.qacNonConformanceCodeID,
						qacCreatedBy = item2.qacCreatedBy,
						qacCreatedDate = item2.qacCreatedDate,
						qacDescription = item2.qacDescription,
						qacUniqueID = item2.qacUniqueID,
						qacNonConformanceCategoryID = item2.qacNonConformanceCategoryID,
						qacRowVersion = item2.qacRowVersion,
						CustomFields = item2.CustomFields
					};
					allNonConformanceCodesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all NonConformanceCodes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPNonConformanceCodeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allNonConformanceCodesDto,
				RecordCount = allNonConformanceCodesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPNonConformanceCodeDto>> Process_GetNonConformanceCode(Guid nonConformanceCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPNonConformanceCodeDto nonConformanceCodeDto = null;
		ERPResponseMessageDto<ERPNonConformanceCodeDto> result;
		try
		{
			IERPNonConformanceCodeRepository iERPNonConformanceCodeRepository = (base.ERPNonConformanceCodeRepository = new ERPNonConformanceCodeRepository(base.ApiClientContext));
			using (iERPNonConformanceCodeRepository)
			{
				ERPNonConformanceCodeInformationDto eRPNonConformanceCodeInformationDto = await base.ERPNonConformanceCodeRepository.GetNonConformanceCode(nonConformanceCodeId);
				nonConformanceCodeDto = new ERPNonConformanceCodeDto
				{
					qacNonConformanceCodeID = eRPNonConformanceCodeInformationDto.qacNonConformanceCodeID,
					qacCreatedBy = eRPNonConformanceCodeInformationDto.qacCreatedBy,
					qacCreatedDate = eRPNonConformanceCodeInformationDto.qacCreatedDate,
					qacDescription = eRPNonConformanceCodeInformationDto.qacDescription,
					qacUniqueID = eRPNonConformanceCodeInformationDto.qacUniqueID,
					qacNonConformanceCategoryID = eRPNonConformanceCodeInformationDto.qacNonConformanceCategoryID,
					qacRowVersion = eRPNonConformanceCodeInformationDto.qacRowVersion,
					CustomFields = eRPNonConformanceCodeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the NonConformanceCodes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPNonConformanceCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = nonConformanceCodeDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPNonConformanceCodeDto>> Process_PutNonConformanceCode(ERPNonConformanceCodeDto nonConformanceCode)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPNonConformanceCodeDto createdObject = null;
		ERPResponseMessageDto<ERPNonConformanceCodeDto> result;
		try
		{
			IERPNonConformanceCodeRepository iERPNonConformanceCodeRepository = (base.ERPNonConformanceCodeRepository = new ERPNonConformanceCodeRepository(base.ApiClientContext));
			using (iERPNonConformanceCodeRepository)
			{
				APIValidationInfoDto postResult = await base.ERPNonConformanceCodeRepository.SaveNonConformanceCode(nonConformanceCode);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPNonConformanceCodeInformationDto eRPNonConformanceCodeInformationDto = await base.ERPNonConformanceCodeRepository.GetNonConformanceCode(nonConformanceCode.qacUniqueID);
					createdObject = new ERPNonConformanceCodeDto
					{
						qacNonConformanceCodeID = eRPNonConformanceCodeInformationDto.qacNonConformanceCodeID,
						qacCreatedBy = eRPNonConformanceCodeInformationDto.qacCreatedBy,
						qacCreatedDate = eRPNonConformanceCodeInformationDto.qacCreatedDate,
						qacDescription = eRPNonConformanceCodeInformationDto.qacDescription,
						qacUniqueID = eRPNonConformanceCodeInformationDto.qacUniqueID,
						qacNonConformanceCategoryID = eRPNonConformanceCodeInformationDto.qacNonConformanceCategoryID,
						qacRowVersion = eRPNonConformanceCodeInformationDto.qacRowVersion,
						CustomFields = eRPNonConformanceCodeInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing NonConformanceCode [{nonConformanceCode.qacUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPNonConformanceCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteNonConformanceCode(Guid nonConformanceCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPNonConformanceCodeRepository iERPNonConformanceCodeRepository = (base.ERPNonConformanceCodeRepository = new ERPNonConformanceCodeRepository(base.ApiClientContext));
		using (iERPNonConformanceCodeRepository)
		{
			if (!(await base.ERPNonConformanceCodeRepository.DoesNonConformanceCodeExist(nonConformanceCodeId)))
			{
				base.ErrorsList.Add($"NonConformanceCode [{nonConformanceCodeId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPNonConformanceCodeInformationDto eRPNonConformanceCodeInformationDto = await base.ERPNonConformanceCodeRepository.GetNonConformanceCode(nonConformanceCodeId);
				string text = await base.ERPNonConformanceCodeRepository.WhereUsed("NonConformanceCodes", new object[1] { eRPNonConformanceCodeInformationDto.qacNonConformanceCodeID }, new object[1] { "qacNonConformanceCodeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("NonConformanceCode cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPNonConformanceCodeDto>> Process_DeleteNonConformanceCode(Guid nonConformanceCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPNonConformanceCodeDto> result;
		try
		{
			IERPNonConformanceCodeRepository iERPNonConformanceCodeRepository = (base.ERPNonConformanceCodeRepository = new ERPNonConformanceCodeRepository(base.ApiClientContext));
			using (iERPNonConformanceCodeRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPNonConformanceCodeRepository.DeleteRowFromTable("NonConformanceCodes", "qac", nonConformanceCodeId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of NonConformanceCode [{nonConformanceCodeId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPNonConformanceCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPNonConformanceCodeDto()
			};
		}
		return result;
	}
}
