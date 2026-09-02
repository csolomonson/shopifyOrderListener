using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPLandedCostCategoryModel : ERPBaseModel, IERPLandedCostCategoryModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllLandedCostCategories(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPLandedCostCategoryRepository iERPLandedCostCategoryRepository = (base.ERPLandedCostCategoryRepository = new ERPLandedCostCategoryRepository(base.ApiClientContext));
		using (iERPLandedCostCategoryRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPLandedCostCategoryRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPLandedCostCategoryRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPLandedCostCategoryRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPLandedCostCategoryRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetLandedCostCategory(Guid landedCostCategoryId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLandedCostCategoryRepository iERPLandedCostCategoryRepository = (base.ERPLandedCostCategoryRepository = new ERPLandedCostCategoryRepository(base.ApiClientContext));
		using (iERPLandedCostCategoryRepository)
		{
			if (!(await base.ERPLandedCostCategoryRepository.DoesLandedCostCategoryExist(landedCostCategoryId)))
			{
				errorsList.Add($"LandedCostCategory [{landedCostCategoryId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPLandedCostCategoryDto>>> Process_GetAllLandedCostCategories(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPLandedCostCategoryDto> allLandedCostCategoriesDto = new List<ERPLandedCostCategoryDto>();
		ERPResponseMessageDto<IList<ERPLandedCostCategoryDto>> result;
		try
		{
			IERPLandedCostCategoryRepository iERPLandedCostCategoryRepository = (base.ERPLandedCostCategoryRepository = new ERPLandedCostCategoryRepository(base.ApiClientContext));
			using (iERPLandedCostCategoryRepository)
			{
				foreach (ERPLandedCostCategoryInformationDto item2 in await base.ERPLandedCostCategoryRepository.GetAllLandedCostCategories(pageSize, pageNumber, filter, orderBy))
				{
					ERPLandedCostCategoryDto item = new ERPLandedCostCategoryDto
					{
						rmaCategoryType = item2.rmaCategoryType,
						rmaLandedCostCategoryID = item2.rmaLandedCostCategoryID,
						rmaCreatedBy = item2.rmaCreatedBy,
						rmaCreatedDate = item2.rmaCreatedDate,
						rmaDescription = item2.rmaDescription,
						rmaUniqueID = item2.rmaUniqueID,
						rmaExpenseSplitPercentTotal = item2.rmaExpenseSplitPercentTotal,
						rmaDefault = item2.rmaDefault,
						rmaLandedCostMethod = item2.rmaLandedCostMethod,
						rmaRowVersion = item2.rmaRowVersion,
						rmaSupplierLocationID = item2.rmaSupplierLocationID,
						rmaSupplierOrganizationID = item2.rmaSupplierOrganizationID,
						CustomFields = item2.CustomFields
					};
					allLandedCostCategoriesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all LandedCostCategories]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPLandedCostCategoryDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allLandedCostCategoriesDto,
				RecordCount = allLandedCostCategoriesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLandedCostCategoryDto>> Process_GetLandedCostCategory(Guid landedCostCategoryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPLandedCostCategoryDto landedCostCategoryDto = null;
		ERPResponseMessageDto<ERPLandedCostCategoryDto> result;
		try
		{
			IERPLandedCostCategoryRepository iERPLandedCostCategoryRepository = (base.ERPLandedCostCategoryRepository = new ERPLandedCostCategoryRepository(base.ApiClientContext));
			using (iERPLandedCostCategoryRepository)
			{
				ERPLandedCostCategoryInformationDto eRPLandedCostCategoryInformationDto = await base.ERPLandedCostCategoryRepository.GetLandedCostCategory(landedCostCategoryId);
				landedCostCategoryDto = new ERPLandedCostCategoryDto
				{
					rmaCategoryType = eRPLandedCostCategoryInformationDto.rmaCategoryType,
					rmaLandedCostCategoryID = eRPLandedCostCategoryInformationDto.rmaLandedCostCategoryID,
					rmaCreatedBy = eRPLandedCostCategoryInformationDto.rmaCreatedBy,
					rmaCreatedDate = eRPLandedCostCategoryInformationDto.rmaCreatedDate,
					rmaDescription = eRPLandedCostCategoryInformationDto.rmaDescription,
					rmaUniqueID = eRPLandedCostCategoryInformationDto.rmaUniqueID,
					rmaExpenseSplitPercentTotal = eRPLandedCostCategoryInformationDto.rmaExpenseSplitPercentTotal,
					rmaDefault = eRPLandedCostCategoryInformationDto.rmaDefault,
					rmaLandedCostMethod = eRPLandedCostCategoryInformationDto.rmaLandedCostMethod,
					rmaRowVersion = eRPLandedCostCategoryInformationDto.rmaRowVersion,
					rmaSupplierLocationID = eRPLandedCostCategoryInformationDto.rmaSupplierLocationID,
					rmaSupplierOrganizationID = eRPLandedCostCategoryInformationDto.rmaSupplierOrganizationID,
					CustomFields = eRPLandedCostCategoryInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the LandedCostCategories []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLandedCostCategoryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = landedCostCategoryDto
			};
		}
		return result;
	}
}
