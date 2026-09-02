using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAssetAdjustmentModel : ERPBaseModel, IERPAssetAdjustmentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAssetAdjustments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAssetAdjustmentRepository iERPAssetAdjustmentRepository = (base.ERPAssetAdjustmentRepository = new ERPAssetAdjustmentRepository(base.ApiClientContext));
		using (iERPAssetAdjustmentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAssetAdjustmentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAssetAdjustmentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAssetAdjustmentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAssetAdjustmentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAssetAdjustment(Guid assetAdjustmentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetAdjustmentRepository iERPAssetAdjustmentRepository = (base.ERPAssetAdjustmentRepository = new ERPAssetAdjustmentRepository(base.ApiClientContext));
		using (iERPAssetAdjustmentRepository)
		{
			if (!(await base.ERPAssetAdjustmentRepository.DoesAssetAdjustmentExist(assetAdjustmentId)))
			{
				errorsList.Add($"AssetAdjustment [{assetAdjustmentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutAssetAdjustment(ERPAssetAdjustmentDto assetAdjustment)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetAdjustmentRepository iERPAssetAdjustmentRepository = (base.ERPAssetAdjustmentRepository = new ERPAssetAdjustmentRepository(base.ApiClientContext));
		using (iERPAssetAdjustmentRepository)
		{
			if (!string.IsNullOrWhiteSpace(assetAdjustment.faaAssetID) && !(await base.ERPAssetAdjustmentRepository.DoesRecordExistInTableUsingKeys("Assets", new object[1] { "FAPASSETID" }, new object[1] { assetAdjustment.faaAssetID })))
			{
				errorsList.Add("faaAssetID [" + assetAdjustment.faaAssetID + "] not found.");
			}
			if (assetAdjustment.faaGlFiscalYearID > 0 && !(await base.ERPAssetAdjustmentRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { assetAdjustment.faaGlFiscalYearID })))
			{
				errorsList.Add($"faaGlFiscalYearID [{assetAdjustment.faaGlFiscalYearID}] not found.");
			}
			if (assetAdjustment.faaGlFiscalYearPeriodID > 0 && !(await base.ERPAssetAdjustmentRepository.DoesRecordExistInTableUsingKeys("GLFiscalYearPeriods", new object[2] { "GLFGLFISCALYEARID", "GLFGLFISCALYEARPERIODID" }, new object[2] { assetAdjustment.faaGlFiscalYearID, assetAdjustment.faaGlFiscalYearPeriodID })))
			{
				errorsList.Add($"faaGlFiscalYearPeriodID [{assetAdjustment.faaGlFiscalYearPeriodID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(assetAdjustment.faaAuthorizedByEmployeeID) && !(await base.ERPAssetAdjustmentRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { assetAdjustment.faaAuthorizedByEmployeeID })))
			{
				errorsList.Add("faaAuthorizedByEmployeeID [" + assetAdjustment.faaAuthorizedByEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(assetAdjustment.faaSourcePlantID) && !(await base.ERPAssetAdjustmentRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { assetAdjustment.faaSourcePlantID })))
			{
				errorsList.Add("faaSourcePlantID [" + assetAdjustment.faaSourcePlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(assetAdjustment.faaDestinationPlantID) && !(await base.ERPAssetAdjustmentRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { assetAdjustment.faaDestinationPlantID })))
			{
				errorsList.Add("faaDestinationPlantID [" + assetAdjustment.faaDestinationPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(assetAdjustment.faaCustomerOrganizationID) && !(await base.ERPAssetAdjustmentRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { assetAdjustment.faaCustomerOrganizationID })))
			{
				errorsList.Add("faaCustomerOrganizationID [" + assetAdjustment.faaCustomerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(assetAdjustment.faaArInvoiceLocationID) && !(await base.ERPAssetAdjustmentRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { assetAdjustment.faaCustomerOrganizationID, assetAdjustment.faaArInvoiceLocationID })))
			{
				errorsList.Add("faaArInvoiceLocationID [" + assetAdjustment.faaArInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(assetAdjustment.faaArInvoiceContactID) && !(await base.ERPAssetAdjustmentRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { assetAdjustment.faaCustomerOrganizationID, assetAdjustment.faaArInvoiceLocationID, assetAdjustment.faaArInvoiceContactID })))
			{
				errorsList.Add("faaArInvoiceContactID [" + assetAdjustment.faaArInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(assetAdjustment.faaCurrencyRateID) && !(await base.ERPAssetAdjustmentRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { assetAdjustment.faaCurrencyRateID })))
			{
				errorsList.Add("faaCurrencyRateID [" + assetAdjustment.faaCurrencyRateID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAssetAdjustmentDto>>> Process_GetAllAssetAdjustments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAssetAdjustmentDto> allAssetAdjustmentsDto = new List<ERPAssetAdjustmentDto>();
		ERPResponseMessageDto<IList<ERPAssetAdjustmentDto>> result;
		try
		{
			IERPAssetAdjustmentRepository iERPAssetAdjustmentRepository = (base.ERPAssetAdjustmentRepository = new ERPAssetAdjustmentRepository(base.ApiClientContext));
			using (iERPAssetAdjustmentRepository)
			{
				foreach (ERPAssetAdjustmentInformationDto item2 in await base.ERPAssetAdjustmentRepository.GetAllAssetAdjustments(pageSize, pageNumber, filter, orderBy))
				{
					ERPAssetAdjustmentDto item = new ERPAssetAdjustmentDto
					{
						faaAccumulatedDepreciation = item2.faaAccumulatedDepreciation,
						faaAdjustmentDate = item2.faaAdjustmentDate,
						faaAdjustmentType = item2.faaAdjustmentType,
						faaArInvoiceContactID = item2.faaArInvoiceContactID,
						faaArInvoiceLocationID = item2.faaArInvoiceLocationID,
						faaAssetID = item2.faaAssetID,
						faaAuthorizedByEmployeeID = item2.faaAuthorizedByEmployeeID,
						faaClosingPercent = item2.faaClosingPercent,
						faaClosingPeriodDepreciation = item2.faaClosingPeriodDepreciation,
						faaCreatedBy = item2.faaCreatedBy,
						faaCreatedDate = item2.faaCreatedDate,
						faaCurrencyRateID = item2.faaCurrencyRateID,
						faaCustomerOrganizationID = item2.faaCustomerOrganizationID,
						faaDepreciationThisYear = item2.faaDepreciationThisYear,
						faaDestinationPlantID = item2.faaDestinationPlantID,
						faaUniqueID = item2.faaUniqueID,
						faaExchangeRate = item2.faaExchangeRate,
						faaGlFiscalYearID = item2.faaGlFiscalYearID,
						faaGlFiscalYearPeriodID = item2.faaGlFiscalYearPeriodID,
						faaCustomRate = item2.faaCustomRate,
						faaPostedToGl = item2.faaPostedToGl,
						faaLongDescriptionRtf = item2.faaLongDescriptionRtf,
						faaLongDescriptionText = item2.faaLongDescriptionText,
						faaNetAssetValue = item2.faaNetAssetValue,
						faaOpeningAssetValue = item2.faaOpeningAssetValue,
						faaPostedDate = item2.faaPostedDate,
						faaProfitOrLoss = item2.faaProfitOrLoss,
						faaQuantity = item2.faaQuantity,
						faaRowVersion = item2.faaRowVersion,
						faaAssetAdjustmentID = item2.faaAssetAdjustmentID,
						faaSourcePlantID = item2.faaSourcePlantID,
						faaValue = item2.faaValue,
						faaValueForeign = item2.faaValueForeign,
						CustomFields = item2.CustomFields
					};
					allAssetAdjustmentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all AssetAdjustments]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAssetAdjustmentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAssetAdjustmentsDto,
				RecordCount = allAssetAdjustmentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAssetAdjustmentDto>> Process_GetAssetAdjustment(Guid assetAdjustmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAssetAdjustmentDto assetAdjustmentDto = null;
		ERPResponseMessageDto<ERPAssetAdjustmentDto> result;
		try
		{
			IERPAssetAdjustmentRepository iERPAssetAdjustmentRepository = (base.ERPAssetAdjustmentRepository = new ERPAssetAdjustmentRepository(base.ApiClientContext));
			using (iERPAssetAdjustmentRepository)
			{
				ERPAssetAdjustmentInformationDto eRPAssetAdjustmentInformationDto = await base.ERPAssetAdjustmentRepository.GetAssetAdjustment(assetAdjustmentId);
				assetAdjustmentDto = new ERPAssetAdjustmentDto
				{
					faaAccumulatedDepreciation = eRPAssetAdjustmentInformationDto.faaAccumulatedDepreciation,
					faaAdjustmentDate = eRPAssetAdjustmentInformationDto.faaAdjustmentDate,
					faaAdjustmentType = eRPAssetAdjustmentInformationDto.faaAdjustmentType,
					faaArInvoiceContactID = eRPAssetAdjustmentInformationDto.faaArInvoiceContactID,
					faaArInvoiceLocationID = eRPAssetAdjustmentInformationDto.faaArInvoiceLocationID,
					faaAssetID = eRPAssetAdjustmentInformationDto.faaAssetID,
					faaAuthorizedByEmployeeID = eRPAssetAdjustmentInformationDto.faaAuthorizedByEmployeeID,
					faaClosingPercent = eRPAssetAdjustmentInformationDto.faaClosingPercent,
					faaClosingPeriodDepreciation = eRPAssetAdjustmentInformationDto.faaClosingPeriodDepreciation,
					faaCreatedBy = eRPAssetAdjustmentInformationDto.faaCreatedBy,
					faaCreatedDate = eRPAssetAdjustmentInformationDto.faaCreatedDate,
					faaCurrencyRateID = eRPAssetAdjustmentInformationDto.faaCurrencyRateID,
					faaCustomerOrganizationID = eRPAssetAdjustmentInformationDto.faaCustomerOrganizationID,
					faaDepreciationThisYear = eRPAssetAdjustmentInformationDto.faaDepreciationThisYear,
					faaDestinationPlantID = eRPAssetAdjustmentInformationDto.faaDestinationPlantID,
					faaUniqueID = eRPAssetAdjustmentInformationDto.faaUniqueID,
					faaExchangeRate = eRPAssetAdjustmentInformationDto.faaExchangeRate,
					faaGlFiscalYearID = eRPAssetAdjustmentInformationDto.faaGlFiscalYearID,
					faaGlFiscalYearPeriodID = eRPAssetAdjustmentInformationDto.faaGlFiscalYearPeriodID,
					faaCustomRate = eRPAssetAdjustmentInformationDto.faaCustomRate,
					faaPostedToGl = eRPAssetAdjustmentInformationDto.faaPostedToGl,
					faaLongDescriptionRtf = eRPAssetAdjustmentInformationDto.faaLongDescriptionRtf,
					faaLongDescriptionText = eRPAssetAdjustmentInformationDto.faaLongDescriptionText,
					faaNetAssetValue = eRPAssetAdjustmentInformationDto.faaNetAssetValue,
					faaOpeningAssetValue = eRPAssetAdjustmentInformationDto.faaOpeningAssetValue,
					faaPostedDate = eRPAssetAdjustmentInformationDto.faaPostedDate,
					faaProfitOrLoss = eRPAssetAdjustmentInformationDto.faaProfitOrLoss,
					faaQuantity = eRPAssetAdjustmentInformationDto.faaQuantity,
					faaRowVersion = eRPAssetAdjustmentInformationDto.faaRowVersion,
					faaAssetAdjustmentID = eRPAssetAdjustmentInformationDto.faaAssetAdjustmentID,
					faaSourcePlantID = eRPAssetAdjustmentInformationDto.faaSourcePlantID,
					faaValue = eRPAssetAdjustmentInformationDto.faaValue,
					faaValueForeign = eRPAssetAdjustmentInformationDto.faaValueForeign,
					CustomFields = eRPAssetAdjustmentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the AssetAdjustments []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetAdjustmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = assetAdjustmentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAssetAdjustmentDto>> Process_PutAssetAdjustment(ERPAssetAdjustmentDto assetAdjustment)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPAssetAdjustmentDto createdObject = null;
		ERPResponseMessageDto<ERPAssetAdjustmentDto> result;
		try
		{
			IERPAssetAdjustmentRepository iERPAssetAdjustmentRepository = (base.ERPAssetAdjustmentRepository = new ERPAssetAdjustmentRepository(base.ApiClientContext));
			using (iERPAssetAdjustmentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPAssetAdjustmentRepository.SaveAssetAdjustment(assetAdjustment);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPAssetAdjustmentInformationDto eRPAssetAdjustmentInformationDto = await base.ERPAssetAdjustmentRepository.GetAssetAdjustment(assetAdjustment.faaUniqueID);
					createdObject = new ERPAssetAdjustmentDto
					{
						faaAccumulatedDepreciation = eRPAssetAdjustmentInformationDto.faaAccumulatedDepreciation,
						faaAdjustmentDate = eRPAssetAdjustmentInformationDto.faaAdjustmentDate,
						faaAdjustmentType = eRPAssetAdjustmentInformationDto.faaAdjustmentType,
						faaArInvoiceContactID = eRPAssetAdjustmentInformationDto.faaArInvoiceContactID,
						faaArInvoiceLocationID = eRPAssetAdjustmentInformationDto.faaArInvoiceLocationID,
						faaAssetID = eRPAssetAdjustmentInformationDto.faaAssetID,
						faaAuthorizedByEmployeeID = eRPAssetAdjustmentInformationDto.faaAuthorizedByEmployeeID,
						faaClosingPercent = eRPAssetAdjustmentInformationDto.faaClosingPercent,
						faaClosingPeriodDepreciation = eRPAssetAdjustmentInformationDto.faaClosingPeriodDepreciation,
						faaCreatedBy = eRPAssetAdjustmentInformationDto.faaCreatedBy,
						faaCreatedDate = eRPAssetAdjustmentInformationDto.faaCreatedDate,
						faaCurrencyRateID = eRPAssetAdjustmentInformationDto.faaCurrencyRateID,
						faaCustomerOrganizationID = eRPAssetAdjustmentInformationDto.faaCustomerOrganizationID,
						faaDepreciationThisYear = eRPAssetAdjustmentInformationDto.faaDepreciationThisYear,
						faaDestinationPlantID = eRPAssetAdjustmentInformationDto.faaDestinationPlantID,
						faaUniqueID = eRPAssetAdjustmentInformationDto.faaUniqueID,
						faaExchangeRate = eRPAssetAdjustmentInformationDto.faaExchangeRate,
						faaGlFiscalYearID = eRPAssetAdjustmentInformationDto.faaGlFiscalYearID,
						faaGlFiscalYearPeriodID = eRPAssetAdjustmentInformationDto.faaGlFiscalYearPeriodID,
						faaCustomRate = eRPAssetAdjustmentInformationDto.faaCustomRate,
						faaPostedToGl = eRPAssetAdjustmentInformationDto.faaPostedToGl,
						faaLongDescriptionRtf = eRPAssetAdjustmentInformationDto.faaLongDescriptionRtf,
						faaLongDescriptionText = eRPAssetAdjustmentInformationDto.faaLongDescriptionText,
						faaNetAssetValue = eRPAssetAdjustmentInformationDto.faaNetAssetValue,
						faaOpeningAssetValue = eRPAssetAdjustmentInformationDto.faaOpeningAssetValue,
						faaPostedDate = eRPAssetAdjustmentInformationDto.faaPostedDate,
						faaProfitOrLoss = eRPAssetAdjustmentInformationDto.faaProfitOrLoss,
						faaQuantity = eRPAssetAdjustmentInformationDto.faaQuantity,
						faaRowVersion = eRPAssetAdjustmentInformationDto.faaRowVersion,
						faaAssetAdjustmentID = eRPAssetAdjustmentInformationDto.faaAssetAdjustmentID,
						faaSourcePlantID = eRPAssetAdjustmentInformationDto.faaSourcePlantID,
						faaValue = eRPAssetAdjustmentInformationDto.faaValue,
						faaValueForeign = eRPAssetAdjustmentInformationDto.faaValueForeign,
						CustomFields = eRPAssetAdjustmentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing AssetAdjustment [{assetAdjustment.faaUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetAdjustmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteAssetAdjustment(Guid assetAdjustmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAssetAdjustmentRepository iERPAssetAdjustmentRepository = (base.ERPAssetAdjustmentRepository = new ERPAssetAdjustmentRepository(base.ApiClientContext));
		using (iERPAssetAdjustmentRepository)
		{
			if (!(await base.ERPAssetAdjustmentRepository.DoesAssetAdjustmentExist(assetAdjustmentId)))
			{
				base.ErrorsList.Add($"AssetAdjustment [{assetAdjustmentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPAssetAdjustmentInformationDto eRPAssetAdjustmentInformationDto = await base.ERPAssetAdjustmentRepository.GetAssetAdjustment(assetAdjustmentId);
				string text = await base.ERPAssetAdjustmentRepository.WhereUsed("AssetAdjustments", new object[1] { eRPAssetAdjustmentInformationDto.faaAssetAdjustmentID }, new object[1] { "faaAssetAdjustmentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("AssetAdjustment cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPAssetAdjustmentDto>> Process_DeleteAssetAdjustment(Guid assetAdjustmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPAssetAdjustmentDto> result;
		try
		{
			IERPAssetAdjustmentRepository iERPAssetAdjustmentRepository = (base.ERPAssetAdjustmentRepository = new ERPAssetAdjustmentRepository(base.ApiClientContext));
			using (iERPAssetAdjustmentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPAssetAdjustmentRepository.DeleteRowFromTable("AssetAdjustments", "faa", assetAdjustmentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of AssetAdjustment [{assetAdjustmentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAssetAdjustmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPAssetAdjustmentDto()
			};
		}
		return result;
	}
}
