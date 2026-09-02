using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPIndustryTypeModel : ERPBaseModel, IERPIndustryTypeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllIndustryTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPIndustryTypeRepository iERPIndustryTypeRepository = (base.ERPIndustryTypeRepository = new ERPIndustryTypeRepository(base.ApiClientContext));
		using (iERPIndustryTypeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPIndustryTypeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPIndustryTypeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPIndustryTypeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPIndustryTypeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetIndustryType(Guid industryTypeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPIndustryTypeRepository iERPIndustryTypeRepository = (base.ERPIndustryTypeRepository = new ERPIndustryTypeRepository(base.ApiClientContext));
		using (iERPIndustryTypeRepository)
		{
			if (!(await base.ERPIndustryTypeRepository.DoesIndustryTypeExist(industryTypeId)))
			{
				errorsList.Add($"IndustryType [{industryTypeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutIndustryType(ERPIndustryTypeDto industryType)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPIndustryTypeRepository iERPIndustryTypeRepository = (base.ERPIndustryTypeRepository = new ERPIndustryTypeRepository(base.ApiClientContext));
		using (iERPIndustryTypeRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPIndustryTypeDto>>> Process_GetAllIndustryTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPIndustryTypeDto> allIndustryTypesDto = new List<ERPIndustryTypeDto>();
		ERPResponseMessageDto<IList<ERPIndustryTypeDto>> result;
		try
		{
			IERPIndustryTypeRepository iERPIndustryTypeRepository = (base.ERPIndustryTypeRepository = new ERPIndustryTypeRepository(base.ApiClientContext));
			using (iERPIndustryTypeRepository)
			{
				foreach (ERPIndustryTypeInformationDto item2 in await base.ERPIndustryTypeRepository.GetAllIndustryTypes(pageSize, pageNumber, filter, orderBy))
				{
					ERPIndustryTypeDto item = new ERPIndustryTypeDto
					{
						cmiIndustryTypeID = item2.cmiIndustryTypeID,
						cmiCreatedBy = item2.cmiCreatedBy,
						cmiCreatedDate = item2.cmiCreatedDate,
						cmiUniqueID = item2.cmiUniqueID,
						cmiLongDescriptionRtf = item2.cmiLongDescriptionRtf,
						cmiLongDescriptionText = item2.cmiLongDescriptionText,
						cmiRowVersion = item2.cmiRowVersion,
						cmiShortDescription = item2.cmiShortDescription,
						CustomFields = item2.CustomFields
					};
					allIndustryTypesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all IndustryTypes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPIndustryTypeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allIndustryTypesDto,
				RecordCount = allIndustryTypesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPIndustryTypeDto>> Process_GetIndustryType(Guid industryTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPIndustryTypeDto industryTypeDto = null;
		ERPResponseMessageDto<ERPIndustryTypeDto> result;
		try
		{
			IERPIndustryTypeRepository iERPIndustryTypeRepository = (base.ERPIndustryTypeRepository = new ERPIndustryTypeRepository(base.ApiClientContext));
			using (iERPIndustryTypeRepository)
			{
				ERPIndustryTypeInformationDto eRPIndustryTypeInformationDto = await base.ERPIndustryTypeRepository.GetIndustryType(industryTypeId);
				industryTypeDto = new ERPIndustryTypeDto
				{
					cmiIndustryTypeID = eRPIndustryTypeInformationDto.cmiIndustryTypeID,
					cmiCreatedBy = eRPIndustryTypeInformationDto.cmiCreatedBy,
					cmiCreatedDate = eRPIndustryTypeInformationDto.cmiCreatedDate,
					cmiUniqueID = eRPIndustryTypeInformationDto.cmiUniqueID,
					cmiLongDescriptionRtf = eRPIndustryTypeInformationDto.cmiLongDescriptionRtf,
					cmiLongDescriptionText = eRPIndustryTypeInformationDto.cmiLongDescriptionText,
					cmiRowVersion = eRPIndustryTypeInformationDto.cmiRowVersion,
					cmiShortDescription = eRPIndustryTypeInformationDto.cmiShortDescription,
					CustomFields = eRPIndustryTypeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the IndustryTypes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPIndustryTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = industryTypeDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPIndustryTypeDto>> Process_PutIndustryType(ERPIndustryTypeDto industryType)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPIndustryTypeDto createdObject = null;
		ERPResponseMessageDto<ERPIndustryTypeDto> result;
		try
		{
			IERPIndustryTypeRepository iERPIndustryTypeRepository = (base.ERPIndustryTypeRepository = new ERPIndustryTypeRepository(base.ApiClientContext));
			using (iERPIndustryTypeRepository)
			{
				APIValidationInfoDto postResult = await base.ERPIndustryTypeRepository.SaveIndustryType(industryType);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPIndustryTypeInformationDto eRPIndustryTypeInformationDto = await base.ERPIndustryTypeRepository.GetIndustryType(industryType.cmiUniqueID);
					createdObject = new ERPIndustryTypeDto
					{
						cmiIndustryTypeID = eRPIndustryTypeInformationDto.cmiIndustryTypeID,
						cmiCreatedBy = eRPIndustryTypeInformationDto.cmiCreatedBy,
						cmiCreatedDate = eRPIndustryTypeInformationDto.cmiCreatedDate,
						cmiUniqueID = eRPIndustryTypeInformationDto.cmiUniqueID,
						cmiLongDescriptionRtf = eRPIndustryTypeInformationDto.cmiLongDescriptionRtf,
						cmiLongDescriptionText = eRPIndustryTypeInformationDto.cmiLongDescriptionText,
						cmiRowVersion = eRPIndustryTypeInformationDto.cmiRowVersion,
						cmiShortDescription = eRPIndustryTypeInformationDto.cmiShortDescription,
						CustomFields = eRPIndustryTypeInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing IndustryType [{industryType.cmiUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPIndustryTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteIndustryType(Guid industryTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPIndustryTypeRepository iERPIndustryTypeRepository = (base.ERPIndustryTypeRepository = new ERPIndustryTypeRepository(base.ApiClientContext));
		using (iERPIndustryTypeRepository)
		{
			if (!(await base.ERPIndustryTypeRepository.DoesIndustryTypeExist(industryTypeId)))
			{
				base.ErrorsList.Add($"IndustryType [{industryTypeId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPIndustryTypeInformationDto eRPIndustryTypeInformationDto = await base.ERPIndustryTypeRepository.GetIndustryType(industryTypeId);
				string text = await base.ERPIndustryTypeRepository.WhereUsed("IndustryTypes", new object[1] { eRPIndustryTypeInformationDto.cmiIndustryTypeID }, new object[1] { "cmiIndustryTypeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("IndustryType cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPIndustryTypeDto>> Process_DeleteIndustryType(Guid industryTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPIndustryTypeDto> result;
		try
		{
			IERPIndustryTypeRepository iERPIndustryTypeRepository = (base.ERPIndustryTypeRepository = new ERPIndustryTypeRepository(base.ApiClientContext));
			using (iERPIndustryTypeRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPIndustryTypeRepository.DeleteRowFromTable("IndustryTypes", "cmi", industryTypeId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of IndustryType [{industryTypeId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPIndustryTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPIndustryTypeDto()
			};
		}
		return result;
	}
}
