using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPLandedCostChargeModel : ERPBaseModel, IERPLandedCostChargeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllLandedCostCharges(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPLandedCostChargeRepository iERPLandedCostChargeRepository = (base.ERPLandedCostChargeRepository = new ERPLandedCostChargeRepository(base.ApiClientContext));
		using (iERPLandedCostChargeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPLandedCostChargeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPLandedCostChargeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPLandedCostChargeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPLandedCostChargeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetLandedCostCharge(Guid landedCostChargeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLandedCostChargeRepository iERPLandedCostChargeRepository = (base.ERPLandedCostChargeRepository = new ERPLandedCostChargeRepository(base.ApiClientContext));
		using (iERPLandedCostChargeRepository)
		{
			if (!(await base.ERPLandedCostChargeRepository.DoesLandedCostChargeExist(landedCostChargeId)))
			{
				errorsList.Add($"LandedCostCharge [{landedCostChargeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutLandedCostCharge(ERPLandedCostChargeDto landedCostCharge)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLandedCostChargeRepository iERPLandedCostChargeRepository = (base.ERPLandedCostChargeRepository = new ERPLandedCostChargeRepository(base.ApiClientContext));
		using (iERPLandedCostChargeRepository)
		{
			if (!string.IsNullOrWhiteSpace(landedCostCharge.rmhLandedCostID) && !(await base.ERPLandedCostChargeRepository.DoesRecordExistInTableUsingKeys("LandedCosts", new object[1] { "RMCLANDEDCOSTID" }, new object[1] { landedCostCharge.rmhLandedCostID })))
			{
				errorsList.Add("rmhLandedCostID [" + landedCostCharge.rmhLandedCostID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(landedCostCharge.rmhLandedCostCategoryID) && !(await base.ERPLandedCostChargeRepository.DoesRecordExistInTableUsingKeys("LandedCostCategories", new object[1] { "RMALANDEDCOSTCATEGORYID" }, new object[1] { landedCostCharge.rmhLandedCostCategoryID })))
			{
				errorsList.Add("rmhLandedCostCategoryID [" + landedCostCharge.rmhLandedCostCategoryID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(landedCostCharge.rmhApInvoiceID) && !(await base.ERPLandedCostChargeRepository.DoesRecordExistInTableUsingKeys("APInvoices", new object[1] { "APPAPINVOICEID" }, new object[1] { landedCostCharge.rmhApInvoiceID })))
			{
				errorsList.Add("rmhApInvoiceID [" + landedCostCharge.rmhApInvoiceID + "] not found.");
			}
			if (landedCostCharge.rmhApInvoiceLineID > 0 && !(await base.ERPLandedCostChargeRepository.DoesRecordExistInTableUsingKeys("APInvoiceLines", new object[2] { "APLAPINVOICEID", "APLAPINVOICELINEID" }, new object[2] { landedCostCharge.rmhApInvoiceID, landedCostCharge.rmhApInvoiceLineID })))
			{
				errorsList.Add($"rmhApInvoiceLineID [{landedCostCharge.rmhApInvoiceLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(landedCostCharge.rmhCurrencyRateID) && !(await base.ERPLandedCostChargeRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { landedCostCharge.rmhCurrencyRateID })))
			{
				errorsList.Add("rmhCurrencyRateID [" + landedCostCharge.rmhCurrencyRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(landedCostCharge.rmhSupplierOrganizationID) && !(await base.ERPLandedCostChargeRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { landedCostCharge.rmhSupplierOrganizationID })))
			{
				errorsList.Add("rmhSupplierOrganizationID [" + landedCostCharge.rmhSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(landedCostCharge.rmhSupplierLocationID) && !(await base.ERPLandedCostChargeRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { landedCostCharge.rmhSupplierOrganizationID, landedCostCharge.rmhSupplierLocationID })))
			{
				errorsList.Add("rmhSupplierLocationID [" + landedCostCharge.rmhSupplierLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(landedCostCharge.rmhSupplierContactID) && !(await base.ERPLandedCostChargeRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { landedCostCharge.rmhSupplierOrganizationID, landedCostCharge.rmhSupplierLocationID, landedCostCharge.rmhSupplierContactID })))
			{
				errorsList.Add("rmhSupplierContactID [" + landedCostCharge.rmhSupplierContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(landedCostCharge.rmhReverseLandedCostID) && !(await base.ERPLandedCostChargeRepository.DoesRecordExistInTableUsingKeys("LandedCosts", new object[1] { "RMCLANDEDCOSTID" }, new object[1] { landedCostCharge.rmhReverseLandedCostID })))
			{
				errorsList.Add("rmhReverseLandedCostID [" + landedCostCharge.rmhReverseLandedCostID + "] not found.");
			}
			if (landedCostCharge.rmhReverseLandedCostChargeID > 0 && !(await base.ERPLandedCostChargeRepository.DoesRecordExistInTableUsingKeys("LandedCostCharges", new object[2] { "RMHLANDEDCOSTID", "RMHLANDEDCOSTCHARGEID" }, new object[2] { landedCostCharge.rmhReverseLandedCostID, landedCostCharge.rmhReverseLandedCostChargeID })))
			{
				errorsList.Add($"rmhReverseLandedCostChargeID [{landedCostCharge.rmhReverseLandedCostChargeID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPLandedCostChargeDto>>> Process_GetAllLandedCostCharges(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPLandedCostChargeDto> allLandedCostChargesDto = new List<ERPLandedCostChargeDto>();
		ERPResponseMessageDto<IList<ERPLandedCostChargeDto>> result;
		try
		{
			IERPLandedCostChargeRepository iERPLandedCostChargeRepository = (base.ERPLandedCostChargeRepository = new ERPLandedCostChargeRepository(base.ApiClientContext));
			using (iERPLandedCostChargeRepository)
			{
				foreach (ERPLandedCostChargeInformationDto item2 in await base.ERPLandedCostChargeRepository.GetAllLandedCostCharges(pageSize, pageNumber, filter, orderBy))
				{
					ERPLandedCostChargeDto item = new ERPLandedCostChargeDto
					{
						rmhApInvoiceID = item2.rmhApInvoiceID,
						rmhApInvoiceLineID = item2.rmhApInvoiceLineID,
						rmhCreatedBy = item2.rmhCreatedBy,
						rmhCreatedDate = item2.rmhCreatedDate,
						rmhCurrencyRateID = item2.rmhCurrencyRateID,
						rmhDescription = item2.rmhDescription,
						rmhUniqueID = item2.rmhUniqueID,
						rmhEstExchangeRate = item2.rmhEstExchangeRate,
						rmhEstTotalCost = item2.rmhEstTotalCost,
						rmhEstTotalCostForeign = item2.rmhEstTotalCostForeign,
						rmhExchangeRate = item2.rmhExchangeRate,
						rmhCustomRate = item2.rmhCustomRate,
						rmhInTransitJournalsCreated = item2.rmhInTransitJournalsCreated,
						rmhInvoicedComplete = item2.rmhInvoicedComplete,
						rmhReversed = item2.rmhReversed,
						rmhLandedCostCategoryID = item2.rmhLandedCostCategoryID,
						rmhLandedCostID = item2.rmhLandedCostID,
						rmhLandedCostMethod = item2.rmhLandedCostMethod,
						rmhReverseLandedCostChargeID = item2.rmhReverseLandedCostChargeID,
						rmhReverseLandedCostID = item2.rmhReverseLandedCostID,
						rmhRowVersion = item2.rmhRowVersion,
						rmhLandedCostChargeID = item2.rmhLandedCostChargeID,
						rmhSupplierContactID = item2.rmhSupplierContactID,
						rmhSupplierLocationID = item2.rmhSupplierLocationID,
						rmhSupplierOrganizationID = item2.rmhSupplierOrganizationID,
						rmhTotalCost = item2.rmhTotalCost,
						rmhTotalCostForeign = item2.rmhTotalCostForeign,
						CustomFields = item2.CustomFields
					};
					allLandedCostChargesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all LandedCostCharges]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPLandedCostChargeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allLandedCostChargesDto,
				RecordCount = allLandedCostChargesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLandedCostChargeDto>> Process_GetLandedCostCharge(Guid landedCostChargeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPLandedCostChargeDto landedCostChargeDto = null;
		ERPResponseMessageDto<ERPLandedCostChargeDto> result;
		try
		{
			IERPLandedCostChargeRepository iERPLandedCostChargeRepository = (base.ERPLandedCostChargeRepository = new ERPLandedCostChargeRepository(base.ApiClientContext));
			using (iERPLandedCostChargeRepository)
			{
				ERPLandedCostChargeInformationDto eRPLandedCostChargeInformationDto = await base.ERPLandedCostChargeRepository.GetLandedCostCharge(landedCostChargeId);
				landedCostChargeDto = new ERPLandedCostChargeDto
				{
					rmhApInvoiceID = eRPLandedCostChargeInformationDto.rmhApInvoiceID,
					rmhApInvoiceLineID = eRPLandedCostChargeInformationDto.rmhApInvoiceLineID,
					rmhCreatedBy = eRPLandedCostChargeInformationDto.rmhCreatedBy,
					rmhCreatedDate = eRPLandedCostChargeInformationDto.rmhCreatedDate,
					rmhCurrencyRateID = eRPLandedCostChargeInformationDto.rmhCurrencyRateID,
					rmhDescription = eRPLandedCostChargeInformationDto.rmhDescription,
					rmhUniqueID = eRPLandedCostChargeInformationDto.rmhUniqueID,
					rmhEstExchangeRate = eRPLandedCostChargeInformationDto.rmhEstExchangeRate,
					rmhEstTotalCost = eRPLandedCostChargeInformationDto.rmhEstTotalCost,
					rmhEstTotalCostForeign = eRPLandedCostChargeInformationDto.rmhEstTotalCostForeign,
					rmhExchangeRate = eRPLandedCostChargeInformationDto.rmhExchangeRate,
					rmhCustomRate = eRPLandedCostChargeInformationDto.rmhCustomRate,
					rmhInTransitJournalsCreated = eRPLandedCostChargeInformationDto.rmhInTransitJournalsCreated,
					rmhInvoicedComplete = eRPLandedCostChargeInformationDto.rmhInvoicedComplete,
					rmhReversed = eRPLandedCostChargeInformationDto.rmhReversed,
					rmhLandedCostCategoryID = eRPLandedCostChargeInformationDto.rmhLandedCostCategoryID,
					rmhLandedCostID = eRPLandedCostChargeInformationDto.rmhLandedCostID,
					rmhLandedCostMethod = eRPLandedCostChargeInformationDto.rmhLandedCostMethod,
					rmhReverseLandedCostChargeID = eRPLandedCostChargeInformationDto.rmhReverseLandedCostChargeID,
					rmhReverseLandedCostID = eRPLandedCostChargeInformationDto.rmhReverseLandedCostID,
					rmhRowVersion = eRPLandedCostChargeInformationDto.rmhRowVersion,
					rmhLandedCostChargeID = eRPLandedCostChargeInformationDto.rmhLandedCostChargeID,
					rmhSupplierContactID = eRPLandedCostChargeInformationDto.rmhSupplierContactID,
					rmhSupplierLocationID = eRPLandedCostChargeInformationDto.rmhSupplierLocationID,
					rmhSupplierOrganizationID = eRPLandedCostChargeInformationDto.rmhSupplierOrganizationID,
					rmhTotalCost = eRPLandedCostChargeInformationDto.rmhTotalCost,
					rmhTotalCostForeign = eRPLandedCostChargeInformationDto.rmhTotalCostForeign,
					CustomFields = eRPLandedCostChargeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the LandedCostCharges []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLandedCostChargeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = landedCostChargeDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLandedCostChargeDto>> Process_PutLandedCostCharge(ERPLandedCostChargeDto landedCostCharge)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPLandedCostChargeDto createdObject = null;
		ERPResponseMessageDto<ERPLandedCostChargeDto> result;
		try
		{
			IERPLandedCostChargeRepository iERPLandedCostChargeRepository = (base.ERPLandedCostChargeRepository = new ERPLandedCostChargeRepository(base.ApiClientContext));
			using (iERPLandedCostChargeRepository)
			{
				APIValidationInfoDto postResult = await base.ERPLandedCostChargeRepository.SaveLandedCostCharge(landedCostCharge);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPLandedCostChargeInformationDto eRPLandedCostChargeInformationDto = await base.ERPLandedCostChargeRepository.GetLandedCostCharge(landedCostCharge.rmhUniqueID);
					createdObject = new ERPLandedCostChargeDto
					{
						rmhApInvoiceID = eRPLandedCostChargeInformationDto.rmhApInvoiceID,
						rmhApInvoiceLineID = eRPLandedCostChargeInformationDto.rmhApInvoiceLineID,
						rmhCreatedBy = eRPLandedCostChargeInformationDto.rmhCreatedBy,
						rmhCreatedDate = eRPLandedCostChargeInformationDto.rmhCreatedDate,
						rmhCurrencyRateID = eRPLandedCostChargeInformationDto.rmhCurrencyRateID,
						rmhDescription = eRPLandedCostChargeInformationDto.rmhDescription,
						rmhUniqueID = eRPLandedCostChargeInformationDto.rmhUniqueID,
						rmhEstExchangeRate = eRPLandedCostChargeInformationDto.rmhEstExchangeRate,
						rmhEstTotalCost = eRPLandedCostChargeInformationDto.rmhEstTotalCost,
						rmhEstTotalCostForeign = eRPLandedCostChargeInformationDto.rmhEstTotalCostForeign,
						rmhExchangeRate = eRPLandedCostChargeInformationDto.rmhExchangeRate,
						rmhCustomRate = eRPLandedCostChargeInformationDto.rmhCustomRate,
						rmhInTransitJournalsCreated = eRPLandedCostChargeInformationDto.rmhInTransitJournalsCreated,
						rmhInvoicedComplete = eRPLandedCostChargeInformationDto.rmhInvoicedComplete,
						rmhReversed = eRPLandedCostChargeInformationDto.rmhReversed,
						rmhLandedCostCategoryID = eRPLandedCostChargeInformationDto.rmhLandedCostCategoryID,
						rmhLandedCostID = eRPLandedCostChargeInformationDto.rmhLandedCostID,
						rmhLandedCostMethod = eRPLandedCostChargeInformationDto.rmhLandedCostMethod,
						rmhReverseLandedCostChargeID = eRPLandedCostChargeInformationDto.rmhReverseLandedCostChargeID,
						rmhReverseLandedCostID = eRPLandedCostChargeInformationDto.rmhReverseLandedCostID,
						rmhRowVersion = eRPLandedCostChargeInformationDto.rmhRowVersion,
						rmhLandedCostChargeID = eRPLandedCostChargeInformationDto.rmhLandedCostChargeID,
						rmhSupplierContactID = eRPLandedCostChargeInformationDto.rmhSupplierContactID,
						rmhSupplierLocationID = eRPLandedCostChargeInformationDto.rmhSupplierLocationID,
						rmhSupplierOrganizationID = eRPLandedCostChargeInformationDto.rmhSupplierOrganizationID,
						rmhTotalCost = eRPLandedCostChargeInformationDto.rmhTotalCost,
						rmhTotalCostForeign = eRPLandedCostChargeInformationDto.rmhTotalCostForeign,
						CustomFields = eRPLandedCostChargeInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing LandedCostCharge [{landedCostCharge.rmhUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLandedCostChargeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteLandedCostCharge(Guid landedCostChargeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLandedCostChargeRepository iERPLandedCostChargeRepository = (base.ERPLandedCostChargeRepository = new ERPLandedCostChargeRepository(base.ApiClientContext));
		using (iERPLandedCostChargeRepository)
		{
			if (!(await base.ERPLandedCostChargeRepository.DoesLandedCostChargeExist(landedCostChargeId)))
			{
				base.ErrorsList.Add($"LandedCostCharge [{landedCostChargeId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPLandedCostChargeInformationDto eRPLandedCostChargeInformationDto = await base.ERPLandedCostChargeRepository.GetLandedCostCharge(landedCostChargeId);
				string text = await base.ERPLandedCostChargeRepository.WhereUsed("LandedCostCharges", new object[2] { eRPLandedCostChargeInformationDto.rmhLandedCostID, eRPLandedCostChargeInformationDto.rmhLandedCostChargeID }, new object[2] { "rmhLandedCostID", "rmhLandedCostChargeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("LandedCostCharge cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPLandedCostChargeDto>> Process_DeleteLandedCostCharge(Guid landedCostChargeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPLandedCostChargeDto> result;
		try
		{
			IERPLandedCostChargeRepository iERPLandedCostChargeRepository = (base.ERPLandedCostChargeRepository = new ERPLandedCostChargeRepository(base.ApiClientContext));
			using (iERPLandedCostChargeRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPLandedCostChargeRepository.DeleteRowFromTable("LandedCostCharges", "rmh", landedCostChargeId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of LandedCostCharge [{landedCostChargeId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLandedCostChargeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPLandedCostChargeDto()
			};
		}
		return result;
	}
}
