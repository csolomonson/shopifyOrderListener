using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartClassPlantModel : ERPBaseModel, IERPPartClassPlantModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartClassPlants(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartClassPlantRepository iERPPartClassPlantRepository = (base.ERPPartClassPlantRepository = new ERPPartClassPlantRepository(base.ApiClientContext));
		using (iERPPartClassPlantRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartClassPlantRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartClassPlantRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartClassPlantRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartClassPlantRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartClassPlant(Guid partClassPlantId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartClassPlantRepository iERPPartClassPlantRepository = (base.ERPPartClassPlantRepository = new ERPPartClassPlantRepository(base.ApiClientContext));
		using (iERPPartClassPlantRepository)
		{
			if (!(await base.ERPPartClassPlantRepository.DoesPartClassPlantExist(partClassPlantId)))
			{
				errorsList.Add($"PartClassPlant [{partClassPlantId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartClassPlantDto>>> Process_GetAllPartClassPlants(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartClassPlantDto> allPartClassPlantsDto = new List<ERPPartClassPlantDto>();
		ERPResponseMessageDto<IList<ERPPartClassPlantDto>> result;
		try
		{
			IERPPartClassPlantRepository iERPPartClassPlantRepository = (base.ERPPartClassPlantRepository = new ERPPartClassPlantRepository(base.ApiClientContext));
			using (iERPPartClassPlantRepository)
			{
				foreach (ERPPartClassPlantInformationDto item2 in await base.ERPPartClassPlantRepository.GetAllPartClassPlants(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartClassPlantDto item = new ERPPartClassPlantDto
					{
						imfPartClassPlantID = item2.imfPartClassPlantID,
						imfCreatedBy = item2.imfCreatedBy,
						imfCreatedDate = item2.imfCreatedDate,
						imfUniqueID = item2.imfUniqueID,
						imfInventoryGlAccountID = item2.imfInventoryGlAccountID,
						imfInvInInspectionGlAccountID = item2.imfInvInInspectionGlAccountID,
						imfInvInTransferGlAccountID = item2.imfInvInTransferGlAccountID,
						imfInvToReturnGlAccountID = item2.imfInvToReturnGlAccountID,
						imfPartClassID = item2.imfPartClassID,
						imfRowVersion = item2.imfRowVersion,
						CustomFields = item2.CustomFields
					};
					allPartClassPlantsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartClassPlants]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartClassPlantDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartClassPlantsDto,
				RecordCount = allPartClassPlantsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartClassPlantDto>> Process_GetPartClassPlant(Guid partClassPlantId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartClassPlantDto partClassPlantDto = null;
		ERPResponseMessageDto<ERPPartClassPlantDto> result;
		try
		{
			IERPPartClassPlantRepository iERPPartClassPlantRepository = (base.ERPPartClassPlantRepository = new ERPPartClassPlantRepository(base.ApiClientContext));
			using (iERPPartClassPlantRepository)
			{
				ERPPartClassPlantInformationDto eRPPartClassPlantInformationDto = await base.ERPPartClassPlantRepository.GetPartClassPlant(partClassPlantId);
				partClassPlantDto = new ERPPartClassPlantDto
				{
					imfPartClassPlantID = eRPPartClassPlantInformationDto.imfPartClassPlantID,
					imfCreatedBy = eRPPartClassPlantInformationDto.imfCreatedBy,
					imfCreatedDate = eRPPartClassPlantInformationDto.imfCreatedDate,
					imfUniqueID = eRPPartClassPlantInformationDto.imfUniqueID,
					imfInventoryGlAccountID = eRPPartClassPlantInformationDto.imfInventoryGlAccountID,
					imfInvInInspectionGlAccountID = eRPPartClassPlantInformationDto.imfInvInInspectionGlAccountID,
					imfInvInTransferGlAccountID = eRPPartClassPlantInformationDto.imfInvInTransferGlAccountID,
					imfInvToReturnGlAccountID = eRPPartClassPlantInformationDto.imfInvToReturnGlAccountID,
					imfPartClassID = eRPPartClassPlantInformationDto.imfPartClassID,
					imfRowVersion = eRPPartClassPlantInformationDto.imfRowVersion,
					CustomFields = eRPPartClassPlantInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartClassPlants []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartClassPlantDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partClassPlantDto
			};
		}
		return result;
	}
}
