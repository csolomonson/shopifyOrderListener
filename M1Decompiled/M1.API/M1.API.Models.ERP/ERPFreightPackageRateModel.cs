using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPFreightPackageRateModel : ERPBaseModel, IERPFreightPackageRateModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllFreightPackageRates(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPFreightPackageRateRepository iERPFreightPackageRateRepository = (base.ERPFreightPackageRateRepository = new ERPFreightPackageRateRepository(base.ApiClientContext));
		using (iERPFreightPackageRateRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPFreightPackageRateRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPFreightPackageRateRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPFreightPackageRateRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPFreightPackageRateRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetFreightPackageRate(Guid freightPackageRateId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFreightPackageRateRepository iERPFreightPackageRateRepository = (base.ERPFreightPackageRateRepository = new ERPFreightPackageRateRepository(base.ApiClientContext));
		using (iERPFreightPackageRateRepository)
		{
			if (!(await base.ERPFreightPackageRateRepository.DoesFreightPackageRateExist(freightPackageRateId)))
			{
				errorsList.Add($"FreightPackageRate [{freightPackageRateId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutFreightPackageRate(ERPFreightPackageRateDto freightPackageRate)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFreightPackageRateRepository iERPFreightPackageRateRepository = (base.ERPFreightPackageRateRepository = new ERPFreightPackageRateRepository(base.ApiClientContext));
		using (iERPFreightPackageRateRepository)
		{
			if (freightPackageRate.fprFreightPackageID > 0 && !(await base.ERPFreightPackageRateRepository.DoesRecordExistInTableUsingKeys("FreightPackages", new object[2] { "FSLFREIGHTSHIPMENTID", "FSLFREIGHTPACKAGEID" }, new object[2] { freightPackageRate.fprFreightShipmentID, freightPackageRate.fprFreightPackageID })))
			{
				errorsList.Add($"fprFreightPackageID [{freightPackageRate.fprFreightPackageID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPFreightPackageRateDto>>> Process_GetAllFreightPackageRates(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPFreightPackageRateDto> allFreightPackageRatesDto = new List<ERPFreightPackageRateDto>();
		ERPResponseMessageDto<IList<ERPFreightPackageRateDto>> result;
		try
		{
			IERPFreightPackageRateRepository iERPFreightPackageRateRepository = (base.ERPFreightPackageRateRepository = new ERPFreightPackageRateRepository(base.ApiClientContext));
			using (iERPFreightPackageRateRepository)
			{
				foreach (ERPFreightPackageRateInformationDto item2 in await base.ERPFreightPackageRateRepository.GetAllFreightPackageRates(pageSize, pageNumber, filter, orderBy))
				{
					ERPFreightPackageRateDto item = new ERPFreightPackageRateDto
					{
						fprCreatedBy = item2.fprCreatedBy,
						fprCreatedDate = item2.fprCreatedDate,
						fprUniqueID = item2.fprUniqueID,
						fprFdxBaseCharge = item2.fprFdxBaseCharge,
						fprFdxCurrency = item2.fprFdxCurrency,
						fprFdxDeliveryDate = item2.fprFdxDeliveryDate,
						fprFdxDeliveryDay = item2.fprFdxDeliveryDay,
						fprFdxDestinationStationID = item2.fprFdxDestinationStationID,
						fprFdxPackageBaseCharge = item2.fprFdxPackageBaseCharge,
						fprFdxPackageBillingWeight = item2.fprFdxPackageBillingWeight,
						fprFdxPackageDimWeight = item2.fprFdxPackageDimWeight,
						fprFdxPackageFreightDiscount = item2.fprFdxPackageFreightDiscount,
						fprFdxPackageNetCharge = item2.fprFdxPackageNetCharge,
						fprFdxPackageNetFreight = item2.fprFdxPackageNetFreight,
						fprFdxPackageSurcharges = item2.fprFdxPackageSurcharges,
						fprFdxPackaging = item2.fprFdxPackaging,
						fprFdxService = item2.fprFdxService,
						fprFdxTimeInTransit = item2.fprFdxTimeInTransit,
						fprFdxTotalBillingWeight = item2.fprFdxTotalBillingWeight,
						fprFdxTotalCustomerCharge = item2.fprFdxTotalCustomerCharge,
						fprFdxTotalDimWeight = item2.fprFdxTotalDimWeight,
						fprFdxTotalFreightDiscount = item2.fprFdxTotalFreightDiscount,
						fprFdxTotalNetCharge = item2.fprFdxTotalNetCharge,
						fprFdxTotalNetFreightCharge = item2.fprFdxTotalNetFreightCharge,
						fprFdxTotalSurcharges = item2.fprFdxTotalSurcharges,
						fprFdxUnits = item2.fprFdxUnits,
						fprFdxVariableHandlingCharge = item2.fprFdxVariableHandlingCharge,
						fprFreightPackageID = item2.fprFreightPackageID,
						fprFreightShipmentID = item2.fprFreightShipmentID,
						fprRCTI = item2.fprRCTI,
						fprRowVersion = item2.fprRowVersion,
						fprFreightPackageRateID = item2.fprFreightPackageRateID,
						CustomFields = item2.CustomFields
					};
					allFreightPackageRatesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all FreightPackageRates]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPFreightPackageRateDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allFreightPackageRatesDto,
				RecordCount = allFreightPackageRatesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPFreightPackageRateDto>> Process_GetFreightPackageRate(Guid freightPackageRateId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPFreightPackageRateDto freightPackageRateDto = null;
		ERPResponseMessageDto<ERPFreightPackageRateDto> result;
		try
		{
			IERPFreightPackageRateRepository iERPFreightPackageRateRepository = (base.ERPFreightPackageRateRepository = new ERPFreightPackageRateRepository(base.ApiClientContext));
			using (iERPFreightPackageRateRepository)
			{
				ERPFreightPackageRateInformationDto eRPFreightPackageRateInformationDto = await base.ERPFreightPackageRateRepository.GetFreightPackageRate(freightPackageRateId);
				freightPackageRateDto = new ERPFreightPackageRateDto
				{
					fprCreatedBy = eRPFreightPackageRateInformationDto.fprCreatedBy,
					fprCreatedDate = eRPFreightPackageRateInformationDto.fprCreatedDate,
					fprUniqueID = eRPFreightPackageRateInformationDto.fprUniqueID,
					fprFdxBaseCharge = eRPFreightPackageRateInformationDto.fprFdxBaseCharge,
					fprFdxCurrency = eRPFreightPackageRateInformationDto.fprFdxCurrency,
					fprFdxDeliveryDate = eRPFreightPackageRateInformationDto.fprFdxDeliveryDate,
					fprFdxDeliveryDay = eRPFreightPackageRateInformationDto.fprFdxDeliveryDay,
					fprFdxDestinationStationID = eRPFreightPackageRateInformationDto.fprFdxDestinationStationID,
					fprFdxPackageBaseCharge = eRPFreightPackageRateInformationDto.fprFdxPackageBaseCharge,
					fprFdxPackageBillingWeight = eRPFreightPackageRateInformationDto.fprFdxPackageBillingWeight,
					fprFdxPackageDimWeight = eRPFreightPackageRateInformationDto.fprFdxPackageDimWeight,
					fprFdxPackageFreightDiscount = eRPFreightPackageRateInformationDto.fprFdxPackageFreightDiscount,
					fprFdxPackageNetCharge = eRPFreightPackageRateInformationDto.fprFdxPackageNetCharge,
					fprFdxPackageNetFreight = eRPFreightPackageRateInformationDto.fprFdxPackageNetFreight,
					fprFdxPackageSurcharges = eRPFreightPackageRateInformationDto.fprFdxPackageSurcharges,
					fprFdxPackaging = eRPFreightPackageRateInformationDto.fprFdxPackaging,
					fprFdxService = eRPFreightPackageRateInformationDto.fprFdxService,
					fprFdxTimeInTransit = eRPFreightPackageRateInformationDto.fprFdxTimeInTransit,
					fprFdxTotalBillingWeight = eRPFreightPackageRateInformationDto.fprFdxTotalBillingWeight,
					fprFdxTotalCustomerCharge = eRPFreightPackageRateInformationDto.fprFdxTotalCustomerCharge,
					fprFdxTotalDimWeight = eRPFreightPackageRateInformationDto.fprFdxTotalDimWeight,
					fprFdxTotalFreightDiscount = eRPFreightPackageRateInformationDto.fprFdxTotalFreightDiscount,
					fprFdxTotalNetCharge = eRPFreightPackageRateInformationDto.fprFdxTotalNetCharge,
					fprFdxTotalNetFreightCharge = eRPFreightPackageRateInformationDto.fprFdxTotalNetFreightCharge,
					fprFdxTotalSurcharges = eRPFreightPackageRateInformationDto.fprFdxTotalSurcharges,
					fprFdxUnits = eRPFreightPackageRateInformationDto.fprFdxUnits,
					fprFdxVariableHandlingCharge = eRPFreightPackageRateInformationDto.fprFdxVariableHandlingCharge,
					fprFreightPackageID = eRPFreightPackageRateInformationDto.fprFreightPackageID,
					fprFreightShipmentID = eRPFreightPackageRateInformationDto.fprFreightShipmentID,
					fprRCTI = eRPFreightPackageRateInformationDto.fprRCTI,
					fprRowVersion = eRPFreightPackageRateInformationDto.fprRowVersion,
					fprFreightPackageRateID = eRPFreightPackageRateInformationDto.fprFreightPackageRateID,
					CustomFields = eRPFreightPackageRateInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the FreightPackageRates []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFreightPackageRateDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = freightPackageRateDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPFreightPackageRateDto>> Process_PutFreightPackageRate(ERPFreightPackageRateDto freightPackageRate)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPFreightPackageRateDto createdObject = null;
		ERPResponseMessageDto<ERPFreightPackageRateDto> result;
		try
		{
			IERPFreightPackageRateRepository iERPFreightPackageRateRepository = (base.ERPFreightPackageRateRepository = new ERPFreightPackageRateRepository(base.ApiClientContext));
			using (iERPFreightPackageRateRepository)
			{
				APIValidationInfoDto postResult = await base.ERPFreightPackageRateRepository.SaveFreightPackageRate(freightPackageRate);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPFreightPackageRateInformationDto eRPFreightPackageRateInformationDto = await base.ERPFreightPackageRateRepository.GetFreightPackageRate(freightPackageRate.fprUniqueID);
					createdObject = new ERPFreightPackageRateDto
					{
						fprCreatedBy = eRPFreightPackageRateInformationDto.fprCreatedBy,
						fprCreatedDate = eRPFreightPackageRateInformationDto.fprCreatedDate,
						fprUniqueID = eRPFreightPackageRateInformationDto.fprUniqueID,
						fprFdxBaseCharge = eRPFreightPackageRateInformationDto.fprFdxBaseCharge,
						fprFdxCurrency = eRPFreightPackageRateInformationDto.fprFdxCurrency,
						fprFdxDeliveryDate = eRPFreightPackageRateInformationDto.fprFdxDeliveryDate,
						fprFdxDeliveryDay = eRPFreightPackageRateInformationDto.fprFdxDeliveryDay,
						fprFdxDestinationStationID = eRPFreightPackageRateInformationDto.fprFdxDestinationStationID,
						fprFdxPackageBaseCharge = eRPFreightPackageRateInformationDto.fprFdxPackageBaseCharge,
						fprFdxPackageBillingWeight = eRPFreightPackageRateInformationDto.fprFdxPackageBillingWeight,
						fprFdxPackageDimWeight = eRPFreightPackageRateInformationDto.fprFdxPackageDimWeight,
						fprFdxPackageFreightDiscount = eRPFreightPackageRateInformationDto.fprFdxPackageFreightDiscount,
						fprFdxPackageNetCharge = eRPFreightPackageRateInformationDto.fprFdxPackageNetCharge,
						fprFdxPackageNetFreight = eRPFreightPackageRateInformationDto.fprFdxPackageNetFreight,
						fprFdxPackageSurcharges = eRPFreightPackageRateInformationDto.fprFdxPackageSurcharges,
						fprFdxPackaging = eRPFreightPackageRateInformationDto.fprFdxPackaging,
						fprFdxService = eRPFreightPackageRateInformationDto.fprFdxService,
						fprFdxTimeInTransit = eRPFreightPackageRateInformationDto.fprFdxTimeInTransit,
						fprFdxTotalBillingWeight = eRPFreightPackageRateInformationDto.fprFdxTotalBillingWeight,
						fprFdxTotalCustomerCharge = eRPFreightPackageRateInformationDto.fprFdxTotalCustomerCharge,
						fprFdxTotalDimWeight = eRPFreightPackageRateInformationDto.fprFdxTotalDimWeight,
						fprFdxTotalFreightDiscount = eRPFreightPackageRateInformationDto.fprFdxTotalFreightDiscount,
						fprFdxTotalNetCharge = eRPFreightPackageRateInformationDto.fprFdxTotalNetCharge,
						fprFdxTotalNetFreightCharge = eRPFreightPackageRateInformationDto.fprFdxTotalNetFreightCharge,
						fprFdxTotalSurcharges = eRPFreightPackageRateInformationDto.fprFdxTotalSurcharges,
						fprFdxUnits = eRPFreightPackageRateInformationDto.fprFdxUnits,
						fprFdxVariableHandlingCharge = eRPFreightPackageRateInformationDto.fprFdxVariableHandlingCharge,
						fprFreightPackageID = eRPFreightPackageRateInformationDto.fprFreightPackageID,
						fprFreightShipmentID = eRPFreightPackageRateInformationDto.fprFreightShipmentID,
						fprRCTI = eRPFreightPackageRateInformationDto.fprRCTI,
						fprRowVersion = eRPFreightPackageRateInformationDto.fprRowVersion,
						fprFreightPackageRateID = eRPFreightPackageRateInformationDto.fprFreightPackageRateID,
						CustomFields = eRPFreightPackageRateInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing FreightPackageRate [{freightPackageRate.fprUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFreightPackageRateDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteFreightPackageRate(Guid freightPackageRateId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFreightPackageRateRepository iERPFreightPackageRateRepository = (base.ERPFreightPackageRateRepository = new ERPFreightPackageRateRepository(base.ApiClientContext));
		using (iERPFreightPackageRateRepository)
		{
			if (!(await base.ERPFreightPackageRateRepository.DoesFreightPackageRateExist(freightPackageRateId)))
			{
				base.ErrorsList.Add($"FreightPackageRate [{freightPackageRateId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPFreightPackageRateInformationDto eRPFreightPackageRateInformationDto = await base.ERPFreightPackageRateRepository.GetFreightPackageRate(freightPackageRateId);
				string text = await base.ERPFreightPackageRateRepository.WhereUsed("FreightPackageRates", new object[3] { eRPFreightPackageRateInformationDto.fprFreightShipmentID, eRPFreightPackageRateInformationDto.fprFreightPackageID, eRPFreightPackageRateInformationDto.fprFreightPackageRateID }, new object[3] { "fprFreightShipmentID", "fprFreightPackageID", "fprFreightPackageRateID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("FreightPackageRate cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPFreightPackageRateDto>> Process_DeleteFreightPackageRate(Guid freightPackageRateId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPFreightPackageRateDto> result;
		try
		{
			IERPFreightPackageRateRepository iERPFreightPackageRateRepository = (base.ERPFreightPackageRateRepository = new ERPFreightPackageRateRepository(base.ApiClientContext));
			using (iERPFreightPackageRateRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPFreightPackageRateRepository.DeleteRowFromTable("FreightPackageRates", "fpr", freightPackageRateId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of FreightPackageRate [{freightPackageRateId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFreightPackageRateDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPFreightPackageRateDto()
			};
		}
		return result;
	}
}
