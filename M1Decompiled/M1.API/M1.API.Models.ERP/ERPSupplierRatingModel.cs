using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSupplierRatingModel : ERPBaseModel, IERPSupplierRatingModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSupplierRatings(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSupplierRatingRepository iERPSupplierRatingRepository = (base.ERPSupplierRatingRepository = new ERPSupplierRatingRepository(base.ApiClientContext));
		using (iERPSupplierRatingRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSupplierRatingRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSupplierRatingRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSupplierRatingRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSupplierRatingRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSupplierRating(Guid supplierRatingId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSupplierRatingRepository iERPSupplierRatingRepository = (base.ERPSupplierRatingRepository = new ERPSupplierRatingRepository(base.ApiClientContext));
		using (iERPSupplierRatingRepository)
		{
			if (!(await base.ERPSupplierRatingRepository.DoesSupplierRatingExist(supplierRatingId)))
			{
				errorsList.Add($"SupplierRating [{supplierRatingId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutSupplierRating(ERPSupplierRatingDto supplierRating)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSupplierRatingRepository iERPSupplierRatingRepository = (base.ERPSupplierRatingRepository = new ERPSupplierRatingRepository(base.ApiClientContext));
		using (iERPSupplierRatingRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSupplierRatingDto>>> Process_GetAllSupplierRatings(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSupplierRatingDto> allSupplierRatingsDto = new List<ERPSupplierRatingDto>();
		ERPResponseMessageDto<IList<ERPSupplierRatingDto>> result;
		try
		{
			IERPSupplierRatingRepository iERPSupplierRatingRepository = (base.ERPSupplierRatingRepository = new ERPSupplierRatingRepository(base.ApiClientContext));
			using (iERPSupplierRatingRepository)
			{
				foreach (ERPSupplierRatingInformationDto item2 in await base.ERPSupplierRatingRepository.GetAllSupplierRatings(pageSize, pageNumber, filter, orderBy))
				{
					ERPSupplierRatingDto item = new ERPSupplierRatingDto
					{
						cmsSupplierRatingID = item2.cmsSupplierRatingID,
						cmsCreatedBy = item2.cmsCreatedBy,
						cmsCreatedDate = item2.cmsCreatedDate,
						cmsDescription = item2.cmsDescription,
						cmsUniqueID = item2.cmsUniqueID,
						cmsRowVersion = item2.cmsRowVersion,
						CustomFields = item2.CustomFields
					};
					allSupplierRatingsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SupplierRatings]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSupplierRatingDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSupplierRatingsDto,
				RecordCount = allSupplierRatingsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSupplierRatingDto>> Process_GetSupplierRating(Guid supplierRatingId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSupplierRatingDto supplierRatingDto = null;
		ERPResponseMessageDto<ERPSupplierRatingDto> result;
		try
		{
			IERPSupplierRatingRepository iERPSupplierRatingRepository = (base.ERPSupplierRatingRepository = new ERPSupplierRatingRepository(base.ApiClientContext));
			using (iERPSupplierRatingRepository)
			{
				ERPSupplierRatingInformationDto eRPSupplierRatingInformationDto = await base.ERPSupplierRatingRepository.GetSupplierRating(supplierRatingId);
				supplierRatingDto = new ERPSupplierRatingDto
				{
					cmsSupplierRatingID = eRPSupplierRatingInformationDto.cmsSupplierRatingID,
					cmsCreatedBy = eRPSupplierRatingInformationDto.cmsCreatedBy,
					cmsCreatedDate = eRPSupplierRatingInformationDto.cmsCreatedDate,
					cmsDescription = eRPSupplierRatingInformationDto.cmsDescription,
					cmsUniqueID = eRPSupplierRatingInformationDto.cmsUniqueID,
					cmsRowVersion = eRPSupplierRatingInformationDto.cmsRowVersion,
					CustomFields = eRPSupplierRatingInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SupplierRatings []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSupplierRatingDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = supplierRatingDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSupplierRatingDto>> Process_PutSupplierRating(ERPSupplierRatingDto supplierRating)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPSupplierRatingDto createdObject = null;
		ERPResponseMessageDto<ERPSupplierRatingDto> result;
		try
		{
			IERPSupplierRatingRepository iERPSupplierRatingRepository = (base.ERPSupplierRatingRepository = new ERPSupplierRatingRepository(base.ApiClientContext));
			using (iERPSupplierRatingRepository)
			{
				APIValidationInfoDto postResult = await base.ERPSupplierRatingRepository.SaveSupplierRating(supplierRating);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPSupplierRatingInformationDto eRPSupplierRatingInformationDto = await base.ERPSupplierRatingRepository.GetSupplierRating(supplierRating.cmsUniqueID);
					createdObject = new ERPSupplierRatingDto
					{
						cmsSupplierRatingID = eRPSupplierRatingInformationDto.cmsSupplierRatingID,
						cmsCreatedBy = eRPSupplierRatingInformationDto.cmsCreatedBy,
						cmsCreatedDate = eRPSupplierRatingInformationDto.cmsCreatedDate,
						cmsDescription = eRPSupplierRatingInformationDto.cmsDescription,
						cmsUniqueID = eRPSupplierRatingInformationDto.cmsUniqueID,
						cmsRowVersion = eRPSupplierRatingInformationDto.cmsRowVersion,
						CustomFields = eRPSupplierRatingInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing SupplierRating [{supplierRating.cmsUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSupplierRatingDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteSupplierRating(Guid supplierRatingId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSupplierRatingRepository iERPSupplierRatingRepository = (base.ERPSupplierRatingRepository = new ERPSupplierRatingRepository(base.ApiClientContext));
		using (iERPSupplierRatingRepository)
		{
			if (!(await base.ERPSupplierRatingRepository.DoesSupplierRatingExist(supplierRatingId)))
			{
				base.ErrorsList.Add($"SupplierRating [{supplierRatingId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPSupplierRatingInformationDto eRPSupplierRatingInformationDto = await base.ERPSupplierRatingRepository.GetSupplierRating(supplierRatingId);
				string text = await base.ERPSupplierRatingRepository.WhereUsed("SupplierRatings", new object[1] { eRPSupplierRatingInformationDto.cmsSupplierRatingID }, new object[1] { "cmsSupplierRatingID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("SupplierRating cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPSupplierRatingDto>> Process_DeleteSupplierRating(Guid supplierRatingId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPSupplierRatingDto> result;
		try
		{
			IERPSupplierRatingRepository iERPSupplierRatingRepository = (base.ERPSupplierRatingRepository = new ERPSupplierRatingRepository(base.ApiClientContext));
			using (iERPSupplierRatingRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPSupplierRatingRepository.DeleteRowFromTable("SupplierRatings", "cms", supplierRatingId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of SupplierRating [{supplierRatingId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSupplierRatingDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPSupplierRatingDto()
			};
		}
		return result;
	}
}
