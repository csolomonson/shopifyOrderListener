using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPProductCategoryModel : ERPBaseModel, IERPProductCategoryModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllProductCategories(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPProductCategoryRepository iERPProductCategoryRepository = (base.ERPProductCategoryRepository = new ERPProductCategoryRepository(base.ApiClientContext));
		using (iERPProductCategoryRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPProductCategoryRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPProductCategoryRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPProductCategoryRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPProductCategoryRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetProductCategory(Guid productCategoryId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProductCategoryRepository iERPProductCategoryRepository = (base.ERPProductCategoryRepository = new ERPProductCategoryRepository(base.ApiClientContext));
		using (iERPProductCategoryRepository)
		{
			if (!(await base.ERPProductCategoryRepository.DoesProductCategoryExist(productCategoryId)))
			{
				errorsList.Add($"ProductCategory [{productCategoryId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPProductCategoryDto>>> Process_GetAllProductCategories(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPProductCategoryDto> allProductCategoriesDto = new List<ERPProductCategoryDto>();
		ERPResponseMessageDto<IList<ERPProductCategoryDto>> result;
		try
		{
			IERPProductCategoryRepository iERPProductCategoryRepository = (base.ERPProductCategoryRepository = new ERPProductCategoryRepository(base.ApiClientContext));
			using (iERPProductCategoryRepository)
			{
				foreach (ERPProductCategoryInformationDto item2 in await base.ERPProductCategoryRepository.GetAllProductCategories(pageSize, pageNumber, filter, orderBy))
				{
					ERPProductCategoryDto item = new ERPProductCategoryDto
					{
						incProductCategoryID = item2.incProductCategoryID,
						incCreatedBy = item2.incCreatedBy,
						incCreatedDate = item2.incCreatedDate,
						incDescription = item2.incDescription,
						incUniqueID = item2.incUniqueID,
						incImageFilePath = item2.incImageFilePath,
						incInactiveDate = item2.incInactiveDate,
						incInactive = item2.incInactive,
						INCRowVersion = item2.INCRowVersion,
						incStructureCode = item2.incStructureCode,
						incStructureID = item2.incStructureID,
						CustomFields = item2.CustomFields
					};
					allProductCategoriesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ProductCategories]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPProductCategoryDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allProductCategoriesDto,
				RecordCount = allProductCategoriesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProductCategoryDto>> Process_GetProductCategory(Guid productCategoryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPProductCategoryDto productCategoryDto = null;
		ERPResponseMessageDto<ERPProductCategoryDto> result;
		try
		{
			IERPProductCategoryRepository iERPProductCategoryRepository = (base.ERPProductCategoryRepository = new ERPProductCategoryRepository(base.ApiClientContext));
			using (iERPProductCategoryRepository)
			{
				ERPProductCategoryInformationDto eRPProductCategoryInformationDto = await base.ERPProductCategoryRepository.GetProductCategory(productCategoryId);
				productCategoryDto = new ERPProductCategoryDto
				{
					incProductCategoryID = eRPProductCategoryInformationDto.incProductCategoryID,
					incCreatedBy = eRPProductCategoryInformationDto.incCreatedBy,
					incCreatedDate = eRPProductCategoryInformationDto.incCreatedDate,
					incDescription = eRPProductCategoryInformationDto.incDescription,
					incUniqueID = eRPProductCategoryInformationDto.incUniqueID,
					incImageFilePath = eRPProductCategoryInformationDto.incImageFilePath,
					incInactiveDate = eRPProductCategoryInformationDto.incInactiveDate,
					incInactive = eRPProductCategoryInformationDto.incInactive,
					INCRowVersion = eRPProductCategoryInformationDto.INCRowVersion,
					incStructureCode = eRPProductCategoryInformationDto.incStructureCode,
					incStructureID = eRPProductCategoryInformationDto.incStructureID,
					CustomFields = eRPProductCategoryInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ProductCategories []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProductCategoryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = productCategoryDto
			};
		}
		return result;
	}
}
