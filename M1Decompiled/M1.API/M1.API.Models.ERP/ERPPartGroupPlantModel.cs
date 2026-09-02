using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartGroupPlantModel : ERPBaseModel, IERPPartGroupPlantModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartGroupPlants(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartGroupPlantRepository iERPPartGroupPlantRepository = (base.ERPPartGroupPlantRepository = new ERPPartGroupPlantRepository(base.ApiClientContext));
		using (iERPPartGroupPlantRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartGroupPlantRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartGroupPlantRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartGroupPlantRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartGroupPlantRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartGroupPlant(Guid partGroupPlantId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartGroupPlantRepository iERPPartGroupPlantRepository = (base.ERPPartGroupPlantRepository = new ERPPartGroupPlantRepository(base.ApiClientContext));
		using (iERPPartGroupPlantRepository)
		{
			if (!(await base.ERPPartGroupPlantRepository.DoesPartGroupPlantExist(partGroupPlantId)))
			{
				errorsList.Add($"PartGroupPlant [{partGroupPlantId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartGroupPlantDto>>> Process_GetAllPartGroupPlants(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartGroupPlantDto> allPartGroupPlantsDto = new List<ERPPartGroupPlantDto>();
		ERPResponseMessageDto<IList<ERPPartGroupPlantDto>> result;
		try
		{
			IERPPartGroupPlantRepository iERPPartGroupPlantRepository = (base.ERPPartGroupPlantRepository = new ERPPartGroupPlantRepository(base.ApiClientContext));
			using (iERPPartGroupPlantRepository)
			{
				foreach (ERPPartGroupPlantInformationDto item2 in await base.ERPPartGroupPlantRepository.GetAllPartGroupPlants(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartGroupPlantDto item = new ERPPartGroupPlantDto
					{
						imvArDepositGlAccountID = item2.imvArDepositGlAccountID,
						imvPartGroupPlantID = item2.imvPartGroupPlantID,
						imvCogsLaborGlAccountID = item2.imvCogsLaborGlAccountID,
						imvCogsMaterialGlAccountID = item2.imvCogsMaterialGlAccountID,
						imvCogsOverheadGlAccountID = item2.imvCogsOverheadGlAccountID,
						imvCogsSubcontractGlAccountID = item2.imvCogsSubcontractGlAccountID,
						imvCreatedBy = item2.imvCreatedBy,
						imvCreatedDate = item2.imvCreatedDate,
						imvDiscountGlAccountID = item2.imvDiscountGlAccountID,
						imvUniqueID = item2.imvUniqueID,
						imvPartGroupID = item2.imvPartGroupID,
						imvRowVersion = item2.imvRowVersion,
						imvSalesGlAccountID = item2.imvSalesGlAccountID,
						CustomFields = item2.CustomFields
					};
					allPartGroupPlantsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartGroupPlants]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartGroupPlantDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartGroupPlantsDto,
				RecordCount = allPartGroupPlantsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartGroupPlantDto>> Process_GetPartGroupPlant(Guid partGroupPlantId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartGroupPlantDto partGroupPlantDto = null;
		ERPResponseMessageDto<ERPPartGroupPlantDto> result;
		try
		{
			IERPPartGroupPlantRepository iERPPartGroupPlantRepository = (base.ERPPartGroupPlantRepository = new ERPPartGroupPlantRepository(base.ApiClientContext));
			using (iERPPartGroupPlantRepository)
			{
				ERPPartGroupPlantInformationDto eRPPartGroupPlantInformationDto = await base.ERPPartGroupPlantRepository.GetPartGroupPlant(partGroupPlantId);
				partGroupPlantDto = new ERPPartGroupPlantDto
				{
					imvArDepositGlAccountID = eRPPartGroupPlantInformationDto.imvArDepositGlAccountID,
					imvPartGroupPlantID = eRPPartGroupPlantInformationDto.imvPartGroupPlantID,
					imvCogsLaborGlAccountID = eRPPartGroupPlantInformationDto.imvCogsLaborGlAccountID,
					imvCogsMaterialGlAccountID = eRPPartGroupPlantInformationDto.imvCogsMaterialGlAccountID,
					imvCogsOverheadGlAccountID = eRPPartGroupPlantInformationDto.imvCogsOverheadGlAccountID,
					imvCogsSubcontractGlAccountID = eRPPartGroupPlantInformationDto.imvCogsSubcontractGlAccountID,
					imvCreatedBy = eRPPartGroupPlantInformationDto.imvCreatedBy,
					imvCreatedDate = eRPPartGroupPlantInformationDto.imvCreatedDate,
					imvDiscountGlAccountID = eRPPartGroupPlantInformationDto.imvDiscountGlAccountID,
					imvUniqueID = eRPPartGroupPlantInformationDto.imvUniqueID,
					imvPartGroupID = eRPPartGroupPlantInformationDto.imvPartGroupID,
					imvRowVersion = eRPPartGroupPlantInformationDto.imvRowVersion,
					imvSalesGlAccountID = eRPPartGroupPlantInformationDto.imvSalesGlAccountID,
					CustomFields = eRPPartGroupPlantInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartGroupPlants []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartGroupPlantDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partGroupPlantDto
			};
		}
		return result;
	}
}
