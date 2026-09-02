using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPCallTypeModel : ERPBaseModel, IERPCallTypeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllCallTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCallTypeRepository iERPCallTypeRepository = (base.ERPCallTypeRepository = new ERPCallTypeRepository(base.ApiClientContext));
		using (iERPCallTypeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPCallTypeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPCallTypeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPCallTypeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPCallTypeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetCallType(Guid callTypeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCallTypeRepository iERPCallTypeRepository = (base.ERPCallTypeRepository = new ERPCallTypeRepository(base.ApiClientContext));
		using (iERPCallTypeRepository)
		{
			if (!(await base.ERPCallTypeRepository.DoesCallTypeExist(callTypeId)))
			{
				errorsList.Add($"CallType [{callTypeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPCallTypeDto>>> Process_GetAllCallTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPCallTypeDto> allCallTypesDto = new List<ERPCallTypeDto>();
		ERPResponseMessageDto<IList<ERPCallTypeDto>> result;
		try
		{
			IERPCallTypeRepository iERPCallTypeRepository = (base.ERPCallTypeRepository = new ERPCallTypeRepository(base.ApiClientContext));
			using (iERPCallTypeRepository)
			{
				foreach (ERPCallTypeInformationDto item2 in await base.ERPCallTypeRepository.GetAllCallTypes(pageSize, pageNumber, filter, orderBy))
				{
					ERPCallTypeDto item = new ERPCallTypeDto
					{
						kbtCallStatus = item2.kbtCallStatus,
						kbtCallTypeID = item2.kbtCallTypeID,
						kbtCreatedBy = item2.kbtCreatedBy,
						kbtCreatedDate = item2.kbtCreatedDate,
						kbtDescription = item2.kbtDescription,
						kbtUniqueID = item2.kbtUniqueID,
						kbtInactiveDate = item2.kbtInactiveDate,
						kbtInactive = item2.kbtInactive,
						kbtBillableCall = item2.kbtBillableCall,
						kbtFieldServiceCall = item2.kbtFieldServiceCall,
						kbtInboundCall = item2.kbtInboundCall,
						kbtInternalOnlyCall = item2.kbtInternalOnlyCall,
						kbtRowVersion = item2.kbtRowVersion,
						CustomFields = item2.CustomFields
					};
					allCallTypesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all CallTypes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPCallTypeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allCallTypesDto,
				RecordCount = allCallTypesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCallTypeDto>> Process_GetCallType(Guid callTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPCallTypeDto callTypeDto = null;
		ERPResponseMessageDto<ERPCallTypeDto> result;
		try
		{
			IERPCallTypeRepository iERPCallTypeRepository = (base.ERPCallTypeRepository = new ERPCallTypeRepository(base.ApiClientContext));
			using (iERPCallTypeRepository)
			{
				ERPCallTypeInformationDto eRPCallTypeInformationDto = await base.ERPCallTypeRepository.GetCallType(callTypeId);
				callTypeDto = new ERPCallTypeDto
				{
					kbtCallStatus = eRPCallTypeInformationDto.kbtCallStatus,
					kbtCallTypeID = eRPCallTypeInformationDto.kbtCallTypeID,
					kbtCreatedBy = eRPCallTypeInformationDto.kbtCreatedBy,
					kbtCreatedDate = eRPCallTypeInformationDto.kbtCreatedDate,
					kbtDescription = eRPCallTypeInformationDto.kbtDescription,
					kbtUniqueID = eRPCallTypeInformationDto.kbtUniqueID,
					kbtInactiveDate = eRPCallTypeInformationDto.kbtInactiveDate,
					kbtInactive = eRPCallTypeInformationDto.kbtInactive,
					kbtBillableCall = eRPCallTypeInformationDto.kbtBillableCall,
					kbtFieldServiceCall = eRPCallTypeInformationDto.kbtFieldServiceCall,
					kbtInboundCall = eRPCallTypeInformationDto.kbtInboundCall,
					kbtInternalOnlyCall = eRPCallTypeInformationDto.kbtInternalOnlyCall,
					kbtRowVersion = eRPCallTypeInformationDto.kbtRowVersion,
					CustomFields = eRPCallTypeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the CallTypes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCallTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = callTypeDto
			};
		}
		return result;
	}
}
