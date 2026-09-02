using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPServiceContractTypeModel : ERPBaseModel, IERPServiceContractTypeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllServiceContractTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPServiceContractTypeRepository iERPServiceContractTypeRepository = (base.ERPServiceContractTypeRepository = new ERPServiceContractTypeRepository(base.ApiClientContext));
		using (iERPServiceContractTypeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPServiceContractTypeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPServiceContractTypeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPServiceContractTypeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPServiceContractTypeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetServiceContractType(Guid serviceContractTypeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPServiceContractTypeRepository iERPServiceContractTypeRepository = (base.ERPServiceContractTypeRepository = new ERPServiceContractTypeRepository(base.ApiClientContext));
		using (iERPServiceContractTypeRepository)
		{
			if (!(await base.ERPServiceContractTypeRepository.DoesServiceContractTypeExist(serviceContractTypeId)))
			{
				errorsList.Add($"ServiceContractType [{serviceContractTypeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPServiceContractTypeDto>>> Process_GetAllServiceContractTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPServiceContractTypeDto> allServiceContractTypesDto = new List<ERPServiceContractTypeDto>();
		ERPResponseMessageDto<IList<ERPServiceContractTypeDto>> result;
		try
		{
			IERPServiceContractTypeRepository iERPServiceContractTypeRepository = (base.ERPServiceContractTypeRepository = new ERPServiceContractTypeRepository(base.ApiClientContext));
			using (iERPServiceContractTypeRepository)
			{
				foreach (ERPServiceContractTypeInformationDto item2 in await base.ERPServiceContractTypeRepository.GetAllServiceContractTypes(pageSize, pageNumber, filter, orderBy))
				{
					ERPServiceContractTypeDto item = new ERPServiceContractTypeDto
					{
						kbyServiceContractTypeID = item2.kbyServiceContractTypeID,
						kbyCreatedBy = item2.kbyCreatedBy,
						kbyCreatedDate = item2.kbyCreatedDate,
						kbyDescription = item2.kbyDescription,
						kbyUniqueID = item2.kbyUniqueID,
						kbyInactiveDate = item2.kbyInactiveDate,
						kbyInactive = item2.kbyInactive,
						kbyRowVersion = item2.kbyRowVersion,
						CustomFields = item2.CustomFields
					};
					allServiceContractTypesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ServiceContractTypes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPServiceContractTypeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allServiceContractTypesDto,
				RecordCount = allServiceContractTypesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPServiceContractTypeDto>> Process_GetServiceContractType(Guid serviceContractTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPServiceContractTypeDto serviceContractTypeDto = null;
		ERPResponseMessageDto<ERPServiceContractTypeDto> result;
		try
		{
			IERPServiceContractTypeRepository iERPServiceContractTypeRepository = (base.ERPServiceContractTypeRepository = new ERPServiceContractTypeRepository(base.ApiClientContext));
			using (iERPServiceContractTypeRepository)
			{
				ERPServiceContractTypeInformationDto eRPServiceContractTypeInformationDto = await base.ERPServiceContractTypeRepository.GetServiceContractType(serviceContractTypeId);
				serviceContractTypeDto = new ERPServiceContractTypeDto
				{
					kbyServiceContractTypeID = eRPServiceContractTypeInformationDto.kbyServiceContractTypeID,
					kbyCreatedBy = eRPServiceContractTypeInformationDto.kbyCreatedBy,
					kbyCreatedDate = eRPServiceContractTypeInformationDto.kbyCreatedDate,
					kbyDescription = eRPServiceContractTypeInformationDto.kbyDescription,
					kbyUniqueID = eRPServiceContractTypeInformationDto.kbyUniqueID,
					kbyInactiveDate = eRPServiceContractTypeInformationDto.kbyInactiveDate,
					kbyInactive = eRPServiceContractTypeInformationDto.kbyInactive,
					kbyRowVersion = eRPServiceContractTypeInformationDto.kbyRowVersion,
					CustomFields = eRPServiceContractTypeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ServiceContractTypes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPServiceContractTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = serviceContractTypeDto
			};
		}
		return result;
	}
}
