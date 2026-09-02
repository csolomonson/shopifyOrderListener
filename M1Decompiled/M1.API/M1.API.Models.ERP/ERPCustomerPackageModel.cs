using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPCustomerPackageModel : ERPBaseModel, IERPCustomerPackageModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllCustomerPackages(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCustomerPackageRepository iERPCustomerPackageRepository = (base.ERPCustomerPackageRepository = new ERPCustomerPackageRepository(base.ApiClientContext));
		using (iERPCustomerPackageRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPCustomerPackageRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPCustomerPackageRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPCustomerPackageRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPCustomerPackageRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetCustomerPackage(Guid customerPackageId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCustomerPackageRepository iERPCustomerPackageRepository = (base.ERPCustomerPackageRepository = new ERPCustomerPackageRepository(base.ApiClientContext));
		using (iERPCustomerPackageRepository)
		{
			if (!(await base.ERPCustomerPackageRepository.DoesCustomerPackageExist(customerPackageId)))
			{
				errorsList.Add($"CustomerPackage [{customerPackageId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutCustomerPackage(ERPCustomerPackageDto customerPackage)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCustomerPackageRepository iERPCustomerPackageRepository = (base.ERPCustomerPackageRepository = new ERPCustomerPackageRepository(base.ApiClientContext));
		using (iERPCustomerPackageRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPCustomerPackageDto>>> Process_GetAllCustomerPackages(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPCustomerPackageDto> allCustomerPackagesDto = new List<ERPCustomerPackageDto>();
		ERPResponseMessageDto<IList<ERPCustomerPackageDto>> result;
		try
		{
			IERPCustomerPackageRepository iERPCustomerPackageRepository = (base.ERPCustomerPackageRepository = new ERPCustomerPackageRepository(base.ApiClientContext));
			using (iERPCustomerPackageRepository)
			{
				foreach (ERPCustomerPackageInformationDto item2 in await base.ERPCustomerPackageRepository.GetAllCustomerPackages(pageSize, pageNumber, filter, orderBy))
				{
					ERPCustomerPackageDto item = new ERPCustomerPackageDto
					{
						cpaCustomerPackageID = item2.cpaCustomerPackageID,
						cpaCreatedBy = item2.cpaCreatedBy,
						cpaCreatedDate = item2.cpaCreatedDate,
						cpaUniqueID = item2.cpaUniqueID,
						cpaInactiveDate = item2.cpaInactiveDate,
						cpaInactive = item2.cpaInactive,
						cpaPackageDescription = item2.cpaPackageDescription,
						cpaPackageDimensionsUom = item2.cpaPackageDimensionsUom,
						cpaPackageHeight = item2.cpaPackageHeight,
						cpaPackageLength = item2.cpaPackageLength,
						cpaPackageWidth = item2.cpaPackageWidth,
						cpaRowVersion = item2.cpaRowVersion,
						CustomFields = item2.CustomFields
					};
					allCustomerPackagesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all CustomerPackages]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPCustomerPackageDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allCustomerPackagesDto,
				RecordCount = allCustomerPackagesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCustomerPackageDto>> Process_GetCustomerPackage(Guid customerPackageId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPCustomerPackageDto customerPackageDto = null;
		ERPResponseMessageDto<ERPCustomerPackageDto> result;
		try
		{
			IERPCustomerPackageRepository iERPCustomerPackageRepository = (base.ERPCustomerPackageRepository = new ERPCustomerPackageRepository(base.ApiClientContext));
			using (iERPCustomerPackageRepository)
			{
				ERPCustomerPackageInformationDto eRPCustomerPackageInformationDto = await base.ERPCustomerPackageRepository.GetCustomerPackage(customerPackageId);
				customerPackageDto = new ERPCustomerPackageDto
				{
					cpaCustomerPackageID = eRPCustomerPackageInformationDto.cpaCustomerPackageID,
					cpaCreatedBy = eRPCustomerPackageInformationDto.cpaCreatedBy,
					cpaCreatedDate = eRPCustomerPackageInformationDto.cpaCreatedDate,
					cpaUniqueID = eRPCustomerPackageInformationDto.cpaUniqueID,
					cpaInactiveDate = eRPCustomerPackageInformationDto.cpaInactiveDate,
					cpaInactive = eRPCustomerPackageInformationDto.cpaInactive,
					cpaPackageDescription = eRPCustomerPackageInformationDto.cpaPackageDescription,
					cpaPackageDimensionsUom = eRPCustomerPackageInformationDto.cpaPackageDimensionsUom,
					cpaPackageHeight = eRPCustomerPackageInformationDto.cpaPackageHeight,
					cpaPackageLength = eRPCustomerPackageInformationDto.cpaPackageLength,
					cpaPackageWidth = eRPCustomerPackageInformationDto.cpaPackageWidth,
					cpaRowVersion = eRPCustomerPackageInformationDto.cpaRowVersion,
					CustomFields = eRPCustomerPackageInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the CustomerPackages []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCustomerPackageDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = customerPackageDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCustomerPackageDto>> Process_PutCustomerPackage(ERPCustomerPackageDto customerPackage)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPCustomerPackageDto createdObject = null;
		ERPResponseMessageDto<ERPCustomerPackageDto> result;
		try
		{
			IERPCustomerPackageRepository iERPCustomerPackageRepository = (base.ERPCustomerPackageRepository = new ERPCustomerPackageRepository(base.ApiClientContext));
			using (iERPCustomerPackageRepository)
			{
				APIValidationInfoDto postResult = await base.ERPCustomerPackageRepository.SaveCustomerPackage(customerPackage);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPCustomerPackageInformationDto eRPCustomerPackageInformationDto = await base.ERPCustomerPackageRepository.GetCustomerPackage(customerPackage.cpaUniqueID);
					createdObject = new ERPCustomerPackageDto
					{
						cpaCustomerPackageID = eRPCustomerPackageInformationDto.cpaCustomerPackageID,
						cpaCreatedBy = eRPCustomerPackageInformationDto.cpaCreatedBy,
						cpaCreatedDate = eRPCustomerPackageInformationDto.cpaCreatedDate,
						cpaUniqueID = eRPCustomerPackageInformationDto.cpaUniqueID,
						cpaInactiveDate = eRPCustomerPackageInformationDto.cpaInactiveDate,
						cpaInactive = eRPCustomerPackageInformationDto.cpaInactive,
						cpaPackageDescription = eRPCustomerPackageInformationDto.cpaPackageDescription,
						cpaPackageDimensionsUom = eRPCustomerPackageInformationDto.cpaPackageDimensionsUom,
						cpaPackageHeight = eRPCustomerPackageInformationDto.cpaPackageHeight,
						cpaPackageLength = eRPCustomerPackageInformationDto.cpaPackageLength,
						cpaPackageWidth = eRPCustomerPackageInformationDto.cpaPackageWidth,
						cpaRowVersion = eRPCustomerPackageInformationDto.cpaRowVersion,
						CustomFields = eRPCustomerPackageInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing CustomerPackage [{customerPackage.cpaUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCustomerPackageDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteCustomerPackage(Guid customerPackageId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCustomerPackageRepository iERPCustomerPackageRepository = (base.ERPCustomerPackageRepository = new ERPCustomerPackageRepository(base.ApiClientContext));
		using (iERPCustomerPackageRepository)
		{
			if (!(await base.ERPCustomerPackageRepository.DoesCustomerPackageExist(customerPackageId)))
			{
				base.ErrorsList.Add($"CustomerPackage [{customerPackageId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPCustomerPackageInformationDto eRPCustomerPackageInformationDto = await base.ERPCustomerPackageRepository.GetCustomerPackage(customerPackageId);
				string text = await base.ERPCustomerPackageRepository.WhereUsed("CustomerPackages", new object[1] { eRPCustomerPackageInformationDto.cpaCustomerPackageID }, new object[1] { "cpaCustomerPackageID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("CustomerPackage cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPCustomerPackageDto>> Process_DeleteCustomerPackage(Guid customerPackageId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPCustomerPackageDto> result;
		try
		{
			IERPCustomerPackageRepository iERPCustomerPackageRepository = (base.ERPCustomerPackageRepository = new ERPCustomerPackageRepository(base.ApiClientContext));
			using (iERPCustomerPackageRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPCustomerPackageRepository.DeleteRowFromTable("CustomerPackages", "cpa", customerPackageId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of CustomerPackage [{customerPackageId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCustomerPackageDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPCustomerPackageDto()
			};
		}
		return result;
	}
}
