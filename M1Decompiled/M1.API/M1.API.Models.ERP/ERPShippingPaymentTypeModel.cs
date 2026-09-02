using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPShippingPaymentTypeModel : ERPBaseModel, IERPShippingPaymentTypeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllShippingPaymentTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPShippingPaymentTypeRepository iERPShippingPaymentTypeRepository = (base.ERPShippingPaymentTypeRepository = new ERPShippingPaymentTypeRepository(base.ApiClientContext));
		using (iERPShippingPaymentTypeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPShippingPaymentTypeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPShippingPaymentTypeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPShippingPaymentTypeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPShippingPaymentTypeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetShippingPaymentType(Guid shippingPaymentTypeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShippingPaymentTypeRepository iERPShippingPaymentTypeRepository = (base.ERPShippingPaymentTypeRepository = new ERPShippingPaymentTypeRepository(base.ApiClientContext));
		using (iERPShippingPaymentTypeRepository)
		{
			if (!(await base.ERPShippingPaymentTypeRepository.DoesShippingPaymentTypeExist(shippingPaymentTypeId)))
			{
				errorsList.Add($"ShippingPaymentType [{shippingPaymentTypeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutShippingPaymentType(ERPShippingPaymentTypeDto shippingPaymentType)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPShippingPaymentTypeRepository iERPShippingPaymentTypeRepository = (base.ERPShippingPaymentTypeRepository = new ERPShippingPaymentTypeRepository(base.ApiClientContext));
		using (iERPShippingPaymentTypeRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPShippingPaymentTypeDto>>> Process_GetAllShippingPaymentTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPShippingPaymentTypeDto> allShippingPaymentTypesDto = new List<ERPShippingPaymentTypeDto>();
		ERPResponseMessageDto<IList<ERPShippingPaymentTypeDto>> result;
		try
		{
			IERPShippingPaymentTypeRepository iERPShippingPaymentTypeRepository = (base.ERPShippingPaymentTypeRepository = new ERPShippingPaymentTypeRepository(base.ApiClientContext));
			using (iERPShippingPaymentTypeRepository)
			{
				foreach (ERPShippingPaymentTypeInformationDto item2 in await base.ERPShippingPaymentTypeRepository.GetAllShippingPaymentTypes(pageSize, pageNumber, filter, orderBy))
				{
					ERPShippingPaymentTypeDto item = new ERPShippingPaymentTypeDto
					{
						xayShippingPaymentTypeID = item2.xayShippingPaymentTypeID,
						xayCreatedBy = item2.xayCreatedBy,
						xayCreatedDate = item2.xayCreatedDate,
						xayDescription = item2.xayDescription,
						xayUniqueID = item2.xayUniqueID,
						xayInactiveDate = item2.xayInactiveDate,
						xayInactive = item2.xayInactive,
						xayDoNotXferShipCostsToAr = item2.xayDoNotXferShipCostsToAr,
						xayRowVersion = item2.xayRowVersion,
						CustomFields = item2.CustomFields
					};
					allShippingPaymentTypesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ShippingPaymentTypes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPShippingPaymentTypeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allShippingPaymentTypesDto,
				RecordCount = allShippingPaymentTypesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShippingPaymentTypeDto>> Process_GetShippingPaymentType(Guid shippingPaymentTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPShippingPaymentTypeDto shippingPaymentTypeDto = null;
		ERPResponseMessageDto<ERPShippingPaymentTypeDto> result;
		try
		{
			IERPShippingPaymentTypeRepository iERPShippingPaymentTypeRepository = (base.ERPShippingPaymentTypeRepository = new ERPShippingPaymentTypeRepository(base.ApiClientContext));
			using (iERPShippingPaymentTypeRepository)
			{
				ERPShippingPaymentTypeInformationDto eRPShippingPaymentTypeInformationDto = await base.ERPShippingPaymentTypeRepository.GetShippingPaymentType(shippingPaymentTypeId);
				shippingPaymentTypeDto = new ERPShippingPaymentTypeDto
				{
					xayShippingPaymentTypeID = eRPShippingPaymentTypeInformationDto.xayShippingPaymentTypeID,
					xayCreatedBy = eRPShippingPaymentTypeInformationDto.xayCreatedBy,
					xayCreatedDate = eRPShippingPaymentTypeInformationDto.xayCreatedDate,
					xayDescription = eRPShippingPaymentTypeInformationDto.xayDescription,
					xayUniqueID = eRPShippingPaymentTypeInformationDto.xayUniqueID,
					xayInactiveDate = eRPShippingPaymentTypeInformationDto.xayInactiveDate,
					xayInactive = eRPShippingPaymentTypeInformationDto.xayInactive,
					xayDoNotXferShipCostsToAr = eRPShippingPaymentTypeInformationDto.xayDoNotXferShipCostsToAr,
					xayRowVersion = eRPShippingPaymentTypeInformationDto.xayRowVersion,
					CustomFields = eRPShippingPaymentTypeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ShippingPaymentTypes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShippingPaymentTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = shippingPaymentTypeDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShippingPaymentTypeDto>> Process_PutShippingPaymentType(ERPShippingPaymentTypeDto shippingPaymentType)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPShippingPaymentTypeDto createdObject = null;
		ERPResponseMessageDto<ERPShippingPaymentTypeDto> result;
		try
		{
			IERPShippingPaymentTypeRepository iERPShippingPaymentTypeRepository = (base.ERPShippingPaymentTypeRepository = new ERPShippingPaymentTypeRepository(base.ApiClientContext));
			using (iERPShippingPaymentTypeRepository)
			{
				APIValidationInfoDto postResult = await base.ERPShippingPaymentTypeRepository.SaveShippingPaymentType(shippingPaymentType);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPShippingPaymentTypeInformationDto eRPShippingPaymentTypeInformationDto = await base.ERPShippingPaymentTypeRepository.GetShippingPaymentType(shippingPaymentType.xayUniqueID);
					createdObject = new ERPShippingPaymentTypeDto
					{
						xayShippingPaymentTypeID = eRPShippingPaymentTypeInformationDto.xayShippingPaymentTypeID,
						xayCreatedBy = eRPShippingPaymentTypeInformationDto.xayCreatedBy,
						xayCreatedDate = eRPShippingPaymentTypeInformationDto.xayCreatedDate,
						xayDescription = eRPShippingPaymentTypeInformationDto.xayDescription,
						xayUniqueID = eRPShippingPaymentTypeInformationDto.xayUniqueID,
						xayInactiveDate = eRPShippingPaymentTypeInformationDto.xayInactiveDate,
						xayInactive = eRPShippingPaymentTypeInformationDto.xayInactive,
						xayDoNotXferShipCostsToAr = eRPShippingPaymentTypeInformationDto.xayDoNotXferShipCostsToAr,
						xayRowVersion = eRPShippingPaymentTypeInformationDto.xayRowVersion,
						CustomFields = eRPShippingPaymentTypeInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ShippingPaymentType [{shippingPaymentType.xayUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShippingPaymentTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteShippingPaymentType(Guid shippingPaymentTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShippingPaymentTypeRepository iERPShippingPaymentTypeRepository = (base.ERPShippingPaymentTypeRepository = new ERPShippingPaymentTypeRepository(base.ApiClientContext));
		using (iERPShippingPaymentTypeRepository)
		{
			if (!(await base.ERPShippingPaymentTypeRepository.DoesShippingPaymentTypeExist(shippingPaymentTypeId)))
			{
				base.ErrorsList.Add($"ShippingPaymentType [{shippingPaymentTypeId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPShippingPaymentTypeInformationDto eRPShippingPaymentTypeInformationDto = await base.ERPShippingPaymentTypeRepository.GetShippingPaymentType(shippingPaymentTypeId);
				string text = await base.ERPShippingPaymentTypeRepository.WhereUsed("ShippingPaymentTypes", new object[1] { eRPShippingPaymentTypeInformationDto.xayShippingPaymentTypeID }, new object[1] { "xayShippingPaymentTypeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ShippingPaymentType cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPShippingPaymentTypeDto>> Process_DeleteShippingPaymentType(Guid shippingPaymentTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPShippingPaymentTypeDto> result;
		try
		{
			IERPShippingPaymentTypeRepository iERPShippingPaymentTypeRepository = (base.ERPShippingPaymentTypeRepository = new ERPShippingPaymentTypeRepository(base.ApiClientContext));
			using (iERPShippingPaymentTypeRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPShippingPaymentTypeRepository.DeleteRowFromTable("ShippingPaymentTypes", "xay", shippingPaymentTypeId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ShippingPaymentType [{shippingPaymentTypeId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShippingPaymentTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPShippingPaymentTypeDto()
			};
		}
		return result;
	}
}
