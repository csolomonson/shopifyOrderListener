using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPTaxCodeModel : ERPBaseModel, IERPTaxCodeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllTaxCodes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPTaxCodeRepository iERPTaxCodeRepository = (base.ERPTaxCodeRepository = new ERPTaxCodeRepository(base.ApiClientContext));
		using (iERPTaxCodeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPTaxCodeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPTaxCodeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPTaxCodeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPTaxCodeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetTaxCode(Guid taxCodeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPTaxCodeRepository iERPTaxCodeRepository = (base.ERPTaxCodeRepository = new ERPTaxCodeRepository(base.ApiClientContext));
		using (iERPTaxCodeRepository)
		{
			if (!(await base.ERPTaxCodeRepository.DoesTaxCodeExist(taxCodeId)))
			{
				errorsList.Add($"TaxCode [{taxCodeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutTaxCode(ERPTaxCodeDto taxCode)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPTaxCodeRepository iERPTaxCodeRepository = (base.ERPTaxCodeRepository = new ERPTaxCodeRepository(base.ApiClientContext));
		using (iERPTaxCodeRepository)
		{
			if (!string.IsNullOrWhiteSpace(taxCode.xaxAccrualGlAccountID) && !(await base.ERPTaxCodeRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { taxCode.xaxAccrualGlAccountID })))
			{
				errorsList.Add("xaxAccrualGlAccountID [" + taxCode.xaxAccrualGlAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPTaxCodeDto>>> Process_GetAllTaxCodes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPTaxCodeDto> allTaxCodesDto = new List<ERPTaxCodeDto>();
		ERPResponseMessageDto<IList<ERPTaxCodeDto>> result;
		try
		{
			IERPTaxCodeRepository iERPTaxCodeRepository = (base.ERPTaxCodeRepository = new ERPTaxCodeRepository(base.ApiClientContext));
			using (iERPTaxCodeRepository)
			{
				foreach (ERPTaxCodeInformationDto item2 in await base.ERPTaxCodeRepository.GetAllTaxCodes(pageSize, pageNumber, filter, orderBy))
				{
					ERPTaxCodeDto item = new ERPTaxCodeDto
					{
						xaxAccrualGlAccountID = item2.xaxAccrualGlAccountID,
						xaxTaxCodeID = item2.xaxTaxCodeID,
						xaxCreatedBy = item2.xaxCreatedBy,
						xaxCreatedDate = item2.xaxCreatedDate,
						xaxDescription = item2.xaxDescription,
						xaxUniqueID = item2.xaxUniqueID,
						xaxInactiveDate = item2.xaxInactiveDate,
						xaxInactive = item2.xaxInactive,
						xaxIncludePrimaryTax = item2.xaxIncludePrimaryTax,
						xaxRowVersion = item2.xaxRowVersion,
						xaxTaxOption = item2.xaxTaxOption,
						xaxTaxType = item2.xaxTaxType,
						CustomFields = item2.CustomFields
					};
					allTaxCodesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all TaxCodes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPTaxCodeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allTaxCodesDto,
				RecordCount = allTaxCodesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPTaxCodeDto>> Process_GetTaxCode(Guid taxCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPTaxCodeDto taxCodeDto = null;
		ERPResponseMessageDto<ERPTaxCodeDto> result;
		try
		{
			IERPTaxCodeRepository iERPTaxCodeRepository = (base.ERPTaxCodeRepository = new ERPTaxCodeRepository(base.ApiClientContext));
			using (iERPTaxCodeRepository)
			{
				ERPTaxCodeInformationDto eRPTaxCodeInformationDto = await base.ERPTaxCodeRepository.GetTaxCode(taxCodeId);
				taxCodeDto = new ERPTaxCodeDto
				{
					xaxAccrualGlAccountID = eRPTaxCodeInformationDto.xaxAccrualGlAccountID,
					xaxTaxCodeID = eRPTaxCodeInformationDto.xaxTaxCodeID,
					xaxCreatedBy = eRPTaxCodeInformationDto.xaxCreatedBy,
					xaxCreatedDate = eRPTaxCodeInformationDto.xaxCreatedDate,
					xaxDescription = eRPTaxCodeInformationDto.xaxDescription,
					xaxUniqueID = eRPTaxCodeInformationDto.xaxUniqueID,
					xaxInactiveDate = eRPTaxCodeInformationDto.xaxInactiveDate,
					xaxInactive = eRPTaxCodeInformationDto.xaxInactive,
					xaxIncludePrimaryTax = eRPTaxCodeInformationDto.xaxIncludePrimaryTax,
					xaxRowVersion = eRPTaxCodeInformationDto.xaxRowVersion,
					xaxTaxOption = eRPTaxCodeInformationDto.xaxTaxOption,
					xaxTaxType = eRPTaxCodeInformationDto.xaxTaxType,
					CustomFields = eRPTaxCodeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the TaxCodes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPTaxCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = taxCodeDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPTaxCodeDto>> Process_PutTaxCode(ERPTaxCodeDto taxCode)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPTaxCodeDto createdObject = null;
		ERPResponseMessageDto<ERPTaxCodeDto> result;
		try
		{
			IERPTaxCodeRepository iERPTaxCodeRepository = (base.ERPTaxCodeRepository = new ERPTaxCodeRepository(base.ApiClientContext));
			using (iERPTaxCodeRepository)
			{
				APIValidationInfoDto postResult = await base.ERPTaxCodeRepository.SaveTaxCode(taxCode);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPTaxCodeInformationDto eRPTaxCodeInformationDto = await base.ERPTaxCodeRepository.GetTaxCode(taxCode.xaxUniqueID);
					createdObject = new ERPTaxCodeDto
					{
						xaxAccrualGlAccountID = eRPTaxCodeInformationDto.xaxAccrualGlAccountID,
						xaxTaxCodeID = eRPTaxCodeInformationDto.xaxTaxCodeID,
						xaxCreatedBy = eRPTaxCodeInformationDto.xaxCreatedBy,
						xaxCreatedDate = eRPTaxCodeInformationDto.xaxCreatedDate,
						xaxDescription = eRPTaxCodeInformationDto.xaxDescription,
						xaxUniqueID = eRPTaxCodeInformationDto.xaxUniqueID,
						xaxInactiveDate = eRPTaxCodeInformationDto.xaxInactiveDate,
						xaxInactive = eRPTaxCodeInformationDto.xaxInactive,
						xaxIncludePrimaryTax = eRPTaxCodeInformationDto.xaxIncludePrimaryTax,
						xaxRowVersion = eRPTaxCodeInformationDto.xaxRowVersion,
						xaxTaxOption = eRPTaxCodeInformationDto.xaxTaxOption,
						xaxTaxType = eRPTaxCodeInformationDto.xaxTaxType,
						CustomFields = eRPTaxCodeInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing TaxCode [{taxCode.xaxUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPTaxCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteTaxCode(Guid taxCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPTaxCodeRepository iERPTaxCodeRepository = (base.ERPTaxCodeRepository = new ERPTaxCodeRepository(base.ApiClientContext));
		using (iERPTaxCodeRepository)
		{
			if (!(await base.ERPTaxCodeRepository.DoesTaxCodeExist(taxCodeId)))
			{
				base.ErrorsList.Add($"TaxCode [{taxCodeId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPTaxCodeInformationDto eRPTaxCodeInformationDto = await base.ERPTaxCodeRepository.GetTaxCode(taxCodeId);
				string text = await base.ERPTaxCodeRepository.WhereUsed("TaxCodes", new object[1] { eRPTaxCodeInformationDto.xaxTaxCodeID }, new object[1] { "xaxTaxCodeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("TaxCode cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPTaxCodeDto>> Process_DeleteTaxCode(Guid taxCodeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPTaxCodeDto> result;
		try
		{
			IERPTaxCodeRepository iERPTaxCodeRepository = (base.ERPTaxCodeRepository = new ERPTaxCodeRepository(base.ApiClientContext));
			using (iERPTaxCodeRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPTaxCodeRepository.DeleteRowFromTable("TaxCodes", "xax", taxCodeId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of TaxCode [{taxCodeId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPTaxCodeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPTaxCodeDto()
			};
		}
		return result;
	}
}
