using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPTaxCodePlantModel : ERPBaseModel, IERPTaxCodePlantModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllTaxCodePlants(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPTaxCodePlantRepository iERPTaxCodePlantRepository = (base.ERPTaxCodePlantRepository = new ERPTaxCodePlantRepository(base.ApiClientContext));
		using (iERPTaxCodePlantRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPTaxCodePlantRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPTaxCodePlantRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPTaxCodePlantRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPTaxCodePlantRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetTaxCodePlant(Guid taxCodePlantId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPTaxCodePlantRepository iERPTaxCodePlantRepository = (base.ERPTaxCodePlantRepository = new ERPTaxCodePlantRepository(base.ApiClientContext));
		using (iERPTaxCodePlantRepository)
		{
			if (!(await base.ERPTaxCodePlantRepository.DoesTaxCodePlantExist(taxCodePlantId)))
			{
				errorsList.Add($"TaxCodePlant [{taxCodePlantId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutTaxCodePlant(ERPTaxCodePlantDto taxCodePlant)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPTaxCodePlantRepository iERPTaxCodePlantRepository = (base.ERPTaxCodePlantRepository = new ERPTaxCodePlantRepository(base.ApiClientContext));
		using (iERPTaxCodePlantRepository)
		{
			if (!string.IsNullOrWhiteSpace(taxCodePlant.xtpTaxCodeID) && !(await base.ERPTaxCodePlantRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { taxCodePlant.xtpTaxCodeID })))
			{
				errorsList.Add("xtpTaxCodeID [" + taxCodePlant.xtpTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(taxCodePlant.xtpPlantID) && !(await base.ERPTaxCodePlantRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { taxCodePlant.xtpPlantID })))
			{
				errorsList.Add("xtpPlantID [" + taxCodePlant.xtpPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(taxCodePlant.xtpAccrualGlAccountID) && !(await base.ERPTaxCodePlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { taxCodePlant.xtpAccrualGlAccountID })))
			{
				errorsList.Add("xtpAccrualGlAccountID [" + taxCodePlant.xtpAccrualGlAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPTaxCodePlantDto>>> Process_GetAllTaxCodePlants(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPTaxCodePlantDto> allTaxCodePlantsDto = new List<ERPTaxCodePlantDto>();
		ERPResponseMessageDto<IList<ERPTaxCodePlantDto>> result;
		try
		{
			IERPTaxCodePlantRepository iERPTaxCodePlantRepository = (base.ERPTaxCodePlantRepository = new ERPTaxCodePlantRepository(base.ApiClientContext));
			using (iERPTaxCodePlantRepository)
			{
				foreach (ERPTaxCodePlantInformationDto item2 in await base.ERPTaxCodePlantRepository.GetAllTaxCodePlants(pageSize, pageNumber, filter, orderBy))
				{
					ERPTaxCodePlantDto item = new ERPTaxCodePlantDto
					{
						xtpAccrualGlAccountID = item2.xtpAccrualGlAccountID,
						xtpCreatedBy = item2.xtpCreatedBy,
						xtpCreatedDate = item2.xtpCreatedDate,
						xtpUniqueID = item2.xtpUniqueID,
						xtpPlantID = item2.xtpPlantID,
						xtpRowVersion = item2.xtpRowVersion,
						xtpTaxCodeID = item2.xtpTaxCodeID,
						CustomFields = item2.CustomFields
					};
					allTaxCodePlantsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all TaxCodePlants]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPTaxCodePlantDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allTaxCodePlantsDto,
				RecordCount = allTaxCodePlantsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPTaxCodePlantDto>> Process_GetTaxCodePlant(Guid taxCodePlantId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPTaxCodePlantDto taxCodePlantDto = null;
		ERPResponseMessageDto<ERPTaxCodePlantDto> result;
		try
		{
			IERPTaxCodePlantRepository iERPTaxCodePlantRepository = (base.ERPTaxCodePlantRepository = new ERPTaxCodePlantRepository(base.ApiClientContext));
			using (iERPTaxCodePlantRepository)
			{
				ERPTaxCodePlantInformationDto eRPTaxCodePlantInformationDto = await base.ERPTaxCodePlantRepository.GetTaxCodePlant(taxCodePlantId);
				taxCodePlantDto = new ERPTaxCodePlantDto
				{
					xtpAccrualGlAccountID = eRPTaxCodePlantInformationDto.xtpAccrualGlAccountID,
					xtpCreatedBy = eRPTaxCodePlantInformationDto.xtpCreatedBy,
					xtpCreatedDate = eRPTaxCodePlantInformationDto.xtpCreatedDate,
					xtpUniqueID = eRPTaxCodePlantInformationDto.xtpUniqueID,
					xtpPlantID = eRPTaxCodePlantInformationDto.xtpPlantID,
					xtpRowVersion = eRPTaxCodePlantInformationDto.xtpRowVersion,
					xtpTaxCodeID = eRPTaxCodePlantInformationDto.xtpTaxCodeID,
					CustomFields = eRPTaxCodePlantInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the TaxCodePlants []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPTaxCodePlantDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = taxCodePlantDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPTaxCodePlantDto>> Process_PutTaxCodePlant(ERPTaxCodePlantDto taxCodePlant)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPTaxCodePlantDto createdObject = null;
		ERPResponseMessageDto<ERPTaxCodePlantDto> result;
		try
		{
			IERPTaxCodePlantRepository iERPTaxCodePlantRepository = (base.ERPTaxCodePlantRepository = new ERPTaxCodePlantRepository(base.ApiClientContext));
			using (iERPTaxCodePlantRepository)
			{
				APIValidationInfoDto postResult = await base.ERPTaxCodePlantRepository.SaveTaxCodePlant(taxCodePlant);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPTaxCodePlantInformationDto eRPTaxCodePlantInformationDto = await base.ERPTaxCodePlantRepository.GetTaxCodePlant(taxCodePlant.xtpUniqueID);
					createdObject = new ERPTaxCodePlantDto
					{
						xtpAccrualGlAccountID = eRPTaxCodePlantInformationDto.xtpAccrualGlAccountID,
						xtpCreatedBy = eRPTaxCodePlantInformationDto.xtpCreatedBy,
						xtpCreatedDate = eRPTaxCodePlantInformationDto.xtpCreatedDate,
						xtpUniqueID = eRPTaxCodePlantInformationDto.xtpUniqueID,
						xtpPlantID = eRPTaxCodePlantInformationDto.xtpPlantID,
						xtpRowVersion = eRPTaxCodePlantInformationDto.xtpRowVersion,
						xtpTaxCodeID = eRPTaxCodePlantInformationDto.xtpTaxCodeID,
						CustomFields = eRPTaxCodePlantInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing TaxCodePlant [{taxCodePlant.xtpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPTaxCodePlantDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteTaxCodePlant(Guid taxCodePlantId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPTaxCodePlantRepository iERPTaxCodePlantRepository = (base.ERPTaxCodePlantRepository = new ERPTaxCodePlantRepository(base.ApiClientContext));
		using (iERPTaxCodePlantRepository)
		{
			if (!(await base.ERPTaxCodePlantRepository.DoesTaxCodePlantExist(taxCodePlantId)))
			{
				base.ErrorsList.Add($"TaxCodePlant [{taxCodePlantId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPTaxCodePlantInformationDto eRPTaxCodePlantInformationDto = await base.ERPTaxCodePlantRepository.GetTaxCodePlant(taxCodePlantId);
				string text = await base.ERPTaxCodePlantRepository.WhereUsed("TaxCodePlants", new object[2] { eRPTaxCodePlantInformationDto.xtpTaxCodeID, eRPTaxCodePlantInformationDto.xtpPlantID }, new object[2] { "xtpTaxCodeID", "xtpPlantID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("TaxCodePlant cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPTaxCodePlantDto>> Process_DeleteTaxCodePlant(Guid taxCodePlantId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPTaxCodePlantDto> result;
		try
		{
			IERPTaxCodePlantRepository iERPTaxCodePlantRepository = (base.ERPTaxCodePlantRepository = new ERPTaxCodePlantRepository(base.ApiClientContext));
			using (iERPTaxCodePlantRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPTaxCodePlantRepository.DeleteRowFromTable("TaxCodePlants", "xtp", taxCodePlantId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of TaxCodePlant [{taxCodePlantId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPTaxCodePlantDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPTaxCodePlantDto()
			};
		}
		return result;
	}
}
