using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPReasonModel : ERPBaseModel, IERPReasonModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllReasons(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPReasonRepository iERPReasonRepository = (base.ERPReasonRepository = new ERPReasonRepository(base.ApiClientContext));
		using (iERPReasonRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPReasonRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPReasonRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPReasonRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPReasonRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetReason(Guid reasonId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPReasonRepository iERPReasonRepository = (base.ERPReasonRepository = new ERPReasonRepository(base.ApiClientContext));
		using (iERPReasonRepository)
		{
			if (!(await base.ERPReasonRepository.DoesReasonExist(reasonId)))
			{
				errorsList.Add($"Reason [{reasonId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPReasonDto>>> Process_GetAllReasons(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPReasonDto> allReasonsDto = new List<ERPReasonDto>();
		ERPResponseMessageDto<IList<ERPReasonDto>> result;
		try
		{
			IERPReasonRepository iERPReasonRepository = (base.ERPReasonRepository = new ERPReasonRepository(base.ApiClientContext));
			using (iERPReasonRepository)
			{
				foreach (ERPReasonInformationDto item2 in await base.ERPReasonRepository.GetAllReasons(pageSize, pageNumber, filter, orderBy))
				{
					ERPReasonDto item = new ERPReasonDto
					{
						xarReasonID = item2.xarReasonID,
						xarCreatedBy = item2.xarCreatedBy,
						xarCreatedDate = item2.xarCreatedDate,
						xarDescription = item2.xarDescription,
						xarUniqueID = item2.xarUniqueID,
						xarReasonGlAccountID = item2.xarReasonGlAccountID,
						xarReasonType = item2.xarReasonType,
						xarRowVersion = item2.xarRowVersion,
						xarScrapGlAccountID = item2.xarScrapGlAccountID,
						CustomFields = item2.CustomFields
					};
					allReasonsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Reasons]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPReasonDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allReasonsDto,
				RecordCount = allReasonsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPReasonDto>> Process_GetReason(Guid reasonId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPReasonDto reasonDto = null;
		ERPResponseMessageDto<ERPReasonDto> result;
		try
		{
			IERPReasonRepository iERPReasonRepository = (base.ERPReasonRepository = new ERPReasonRepository(base.ApiClientContext));
			using (iERPReasonRepository)
			{
				ERPReasonInformationDto eRPReasonInformationDto = await base.ERPReasonRepository.GetReason(reasonId);
				reasonDto = new ERPReasonDto
				{
					xarReasonID = eRPReasonInformationDto.xarReasonID,
					xarCreatedBy = eRPReasonInformationDto.xarCreatedBy,
					xarCreatedDate = eRPReasonInformationDto.xarCreatedDate,
					xarDescription = eRPReasonInformationDto.xarDescription,
					xarUniqueID = eRPReasonInformationDto.xarUniqueID,
					xarReasonGlAccountID = eRPReasonInformationDto.xarReasonGlAccountID,
					xarReasonType = eRPReasonInformationDto.xarReasonType,
					xarRowVersion = eRPReasonInformationDto.xarRowVersion,
					xarScrapGlAccountID = eRPReasonInformationDto.xarScrapGlAccountID,
					CustomFields = eRPReasonInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Reasons []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPReasonDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = reasonDto
			};
		}
		return result;
	}
}
