using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPReasonPlantModel : ERPBaseModel, IERPReasonPlantModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllReasonPlants(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPReasonPlantRepository iERPReasonPlantRepository = (base.ERPReasonPlantRepository = new ERPReasonPlantRepository(base.ApiClientContext));
		using (iERPReasonPlantRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPReasonPlantRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPReasonPlantRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPReasonPlantRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPReasonPlantRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetReasonPlant(Guid reasonPlantId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPReasonPlantRepository iERPReasonPlantRepository = (base.ERPReasonPlantRepository = new ERPReasonPlantRepository(base.ApiClientContext));
		using (iERPReasonPlantRepository)
		{
			if (!(await base.ERPReasonPlantRepository.DoesReasonPlantExist(reasonPlantId)))
			{
				errorsList.Add($"ReasonPlant [{reasonPlantId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPReasonPlantDto>>> Process_GetAllReasonPlants(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPReasonPlantDto> allReasonPlantsDto = new List<ERPReasonPlantDto>();
		ERPResponseMessageDto<IList<ERPReasonPlantDto>> result;
		try
		{
			IERPReasonPlantRepository iERPReasonPlantRepository = (base.ERPReasonPlantRepository = new ERPReasonPlantRepository(base.ApiClientContext));
			using (iERPReasonPlantRepository)
			{
				foreach (ERPReasonPlantInformationDto item2 in await base.ERPReasonPlantRepository.GetAllReasonPlants(pageSize, pageNumber, filter, orderBy))
				{
					ERPReasonPlantDto item = new ERPReasonPlantDto
					{
						xajReasonPlantID = item2.xajReasonPlantID,
						xajCreatedBy = item2.xajCreatedBy,
						xajCreatedDate = item2.xajCreatedDate,
						xajUniqueID = item2.xajUniqueID,
						xajReasonGlAccountID = item2.xajReasonGlAccountID,
						xajReasonID = item2.xajReasonID,
						xajRowVersion = item2.xajRowVersion,
						xajScrapGlAccountID = item2.xajScrapGlAccountID,
						CustomFields = item2.CustomFields
					};
					allReasonPlantsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ReasonPlants]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPReasonPlantDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allReasonPlantsDto,
				RecordCount = allReasonPlantsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPReasonPlantDto>> Process_GetReasonPlant(Guid reasonPlantId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPReasonPlantDto reasonPlantDto = null;
		ERPResponseMessageDto<ERPReasonPlantDto> result;
		try
		{
			IERPReasonPlantRepository iERPReasonPlantRepository = (base.ERPReasonPlantRepository = new ERPReasonPlantRepository(base.ApiClientContext));
			using (iERPReasonPlantRepository)
			{
				ERPReasonPlantInformationDto eRPReasonPlantInformationDto = await base.ERPReasonPlantRepository.GetReasonPlant(reasonPlantId);
				reasonPlantDto = new ERPReasonPlantDto
				{
					xajReasonPlantID = eRPReasonPlantInformationDto.xajReasonPlantID,
					xajCreatedBy = eRPReasonPlantInformationDto.xajCreatedBy,
					xajCreatedDate = eRPReasonPlantInformationDto.xajCreatedDate,
					xajUniqueID = eRPReasonPlantInformationDto.xajUniqueID,
					xajReasonGlAccountID = eRPReasonPlantInformationDto.xajReasonGlAccountID,
					xajReasonID = eRPReasonPlantInformationDto.xajReasonID,
					xajRowVersion = eRPReasonPlantInformationDto.xajRowVersion,
					xajScrapGlAccountID = eRPReasonPlantInformationDto.xajScrapGlAccountID,
					CustomFields = eRPReasonPlantInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ReasonPlants []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPReasonPlantDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = reasonPlantDto
			};
		}
		return result;
	}
}
