using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPReceiptModel : ERPBaseModel, IERPReceiptModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllReceipts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPReceiptRepository iERPReceiptRepository = (base.ERPReceiptRepository = new ERPReceiptRepository(base.ApiClientContext));
		using (iERPReceiptRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPReceiptRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPReceiptRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPReceiptRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPReceiptRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetReceipt(Guid receiptId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPReceiptRepository iERPReceiptRepository = (base.ERPReceiptRepository = new ERPReceiptRepository(base.ApiClientContext));
		using (iERPReceiptRepository)
		{
			if (!(await base.ERPReceiptRepository.DoesReceiptExist(receiptId)))
			{
				errorsList.Add($"Receipt [{receiptId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutReceipt(ERPReceiptDto receipt)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPReceiptRepository iERPReceiptRepository = (base.ERPReceiptRepository = new ERPReceiptRepository(base.ApiClientContext));
		using (iERPReceiptRepository)
		{
			if (!string.IsNullOrWhiteSpace(receipt.rmpPlantDepartmentID) && !(await base.ERPReceiptRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { receipt.rmpPlantID, receipt.rmpPlantDepartmentID })))
			{
				errorsList.Add("rmpPlantDepartmentID [" + receipt.rmpPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receipt.rmpPlantID) && !(await base.ERPReceiptRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { receipt.rmpPlantID })))
			{
				errorsList.Add("rmpPlantID [" + receipt.rmpPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receipt.rmpSupplierOrganizationID) && !(await base.ERPReceiptRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { receipt.rmpSupplierOrganizationID })))
			{
				errorsList.Add("rmpSupplierOrganizationID [" + receipt.rmpSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receipt.rmpApInvoiceLocationID) && !(await base.ERPReceiptRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { receipt.rmpSupplierOrganizationID, receipt.rmpApInvoiceLocationID })))
			{
				errorsList.Add("rmpApInvoiceLocationID [" + receipt.rmpApInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receipt.rmpApInvoiceContactID) && !(await base.ERPReceiptRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { receipt.rmpSupplierOrganizationID, receipt.rmpApInvoiceLocationID, receipt.rmpApInvoiceContactID })))
			{
				errorsList.Add("rmpApInvoiceContactID [" + receipt.rmpApInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receipt.rmpPurchaseLocationID) && !(await base.ERPReceiptRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { receipt.rmpSupplierOrganizationID, receipt.rmpPurchaseLocationID })))
			{
				errorsList.Add("rmpPurchaseLocationID [" + receipt.rmpPurchaseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receipt.rmpPurchaseContactID) && !(await base.ERPReceiptRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { receipt.rmpSupplierOrganizationID, receipt.rmpPurchaseLocationID, receipt.rmpPurchaseContactID })))
			{
				errorsList.Add("rmpPurchaseContactID [" + receipt.rmpPurchaseContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receipt.rmpShippingMethodID) && !(await base.ERPReceiptRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { receipt.rmpShippingMethodID })))
			{
				errorsList.Add("rmpShippingMethodID [" + receipt.rmpShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receipt.rmpProjectID) && !(await base.ERPReceiptRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { receipt.rmpProjectID })))
			{
				errorsList.Add("rmpProjectID [" + receipt.rmpProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receipt.rmpCurrencyRateID) && !(await base.ERPReceiptRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { receipt.rmpCurrencyRateID })))
			{
				errorsList.Add("rmpCurrencyRateID [" + receipt.rmpCurrencyRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receipt.rmpLandedCostID) && !(await base.ERPReceiptRepository.DoesRecordExistInTableUsingKeys("LandedCosts", new object[1] { "RMCLANDEDCOSTID" }, new object[1] { receipt.rmpLandedCostID })))
			{
				errorsList.Add("rmpLandedCostID [" + receipt.rmpLandedCostID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPReceiptDto>>> Process_GetAllReceipts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPReceiptDto> allReceiptsDto = new List<ERPReceiptDto>();
		ERPResponseMessageDto<IList<ERPReceiptDto>> result;
		try
		{
			IERPReceiptRepository iERPReceiptRepository = (base.ERPReceiptRepository = new ERPReceiptRepository(base.ApiClientContext));
			using (iERPReceiptRepository)
			{
				foreach (ERPReceiptInformationDto item2 in await base.ERPReceiptRepository.GetAllReceipts(pageSize, pageNumber, filter, orderBy))
				{
					ERPReceiptDto item = new ERPReceiptDto
					{
						rmpApInvoiceContactID = item2.rmpApInvoiceContactID,
						rmpApInvoiceLocationID = item2.rmpApInvoiceLocationID,
						rmpClosedDate = item2.rmpClosedDate,
						rmpReceiptID = item2.rmpReceiptID,
						rmpCreatedBy = item2.rmpCreatedBy,
						rmpCreatedDate = item2.rmpCreatedDate,
						rmpCurrencyRateID = item2.rmpCurrencyRateID,
						rmpDeliveryDocket = item2.rmpDeliveryDocket,
						rmpUniqueID = item2.rmpUniqueID,
						rmpExchangeRate = item2.rmpExchangeRate,
						rmpFreightCharge = item2.rmpFreightCharge,
						rmpFreightChargeForeign = item2.rmpFreightChargeForeign,
						rmpClosed = item2.rmpClosed,
						rmpCustomRate = item2.rmpCustomRate,
						rmpNestlinkProcessed = item2.rmpNestlinkProcessed,
						rmpPostedToGl = item2.rmpPostedToGl,
						rmpReversalEntry = item2.rmpReversalEntry,
						rmpReversed = item2.rmpReversed,
						rmpLandedCostID = item2.rmpLandedCostID,
						rmpPlantDepartmentID = item2.rmpPlantDepartmentID,
						rmpPlantID = item2.rmpPlantID,
						rmpPostedDate = item2.rmpPostedDate,
						rmpProjectID = item2.rmpProjectID,
						rmpPurchaseContactID = item2.rmpPurchaseContactID,
						rmpPurchaseLocationID = item2.rmpPurchaseLocationID,
						rmpReceiptDate = item2.rmpReceiptDate,
						rmpReceiptSubtotal = item2.rmpReceiptSubtotal,
						rmpReceiptSubtotalForeign = item2.rmpReceiptSubtotalForeign,
						rmpReceiptTotal = item2.rmpReceiptTotal,
						rmpReceiptTotalForeign = item2.rmpReceiptTotalForeign,
						rmpRowVersion = item2.rmpRowVersion,
						rmpShippingMethodID = item2.rmpShippingMethodID,
						rmpSupplierOrganizationID = item2.rmpSupplierOrganizationID,
						CustomFields = item2.CustomFields
					};
					allReceiptsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Receipts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPReceiptDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allReceiptsDto,
				RecordCount = allReceiptsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPReceiptDto>> Process_GetReceipt(Guid receiptId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPReceiptDto receiptDto = null;
		ERPResponseMessageDto<ERPReceiptDto> result;
		try
		{
			IERPReceiptRepository iERPReceiptRepository = (base.ERPReceiptRepository = new ERPReceiptRepository(base.ApiClientContext));
			using (iERPReceiptRepository)
			{
				ERPReceiptInformationDto eRPReceiptInformationDto = await base.ERPReceiptRepository.GetReceipt(receiptId);
				receiptDto = new ERPReceiptDto
				{
					rmpApInvoiceContactID = eRPReceiptInformationDto.rmpApInvoiceContactID,
					rmpApInvoiceLocationID = eRPReceiptInformationDto.rmpApInvoiceLocationID,
					rmpClosedDate = eRPReceiptInformationDto.rmpClosedDate,
					rmpReceiptID = eRPReceiptInformationDto.rmpReceiptID,
					rmpCreatedBy = eRPReceiptInformationDto.rmpCreatedBy,
					rmpCreatedDate = eRPReceiptInformationDto.rmpCreatedDate,
					rmpCurrencyRateID = eRPReceiptInformationDto.rmpCurrencyRateID,
					rmpDeliveryDocket = eRPReceiptInformationDto.rmpDeliveryDocket,
					rmpUniqueID = eRPReceiptInformationDto.rmpUniqueID,
					rmpExchangeRate = eRPReceiptInformationDto.rmpExchangeRate,
					rmpFreightCharge = eRPReceiptInformationDto.rmpFreightCharge,
					rmpFreightChargeForeign = eRPReceiptInformationDto.rmpFreightChargeForeign,
					rmpClosed = eRPReceiptInformationDto.rmpClosed,
					rmpCustomRate = eRPReceiptInformationDto.rmpCustomRate,
					rmpNestlinkProcessed = eRPReceiptInformationDto.rmpNestlinkProcessed,
					rmpPostedToGl = eRPReceiptInformationDto.rmpPostedToGl,
					rmpReversalEntry = eRPReceiptInformationDto.rmpReversalEntry,
					rmpReversed = eRPReceiptInformationDto.rmpReversed,
					rmpLandedCostID = eRPReceiptInformationDto.rmpLandedCostID,
					rmpPlantDepartmentID = eRPReceiptInformationDto.rmpPlantDepartmentID,
					rmpPlantID = eRPReceiptInformationDto.rmpPlantID,
					rmpPostedDate = eRPReceiptInformationDto.rmpPostedDate,
					rmpProjectID = eRPReceiptInformationDto.rmpProjectID,
					rmpPurchaseContactID = eRPReceiptInformationDto.rmpPurchaseContactID,
					rmpPurchaseLocationID = eRPReceiptInformationDto.rmpPurchaseLocationID,
					rmpReceiptDate = eRPReceiptInformationDto.rmpReceiptDate,
					rmpReceiptSubtotal = eRPReceiptInformationDto.rmpReceiptSubtotal,
					rmpReceiptSubtotalForeign = eRPReceiptInformationDto.rmpReceiptSubtotalForeign,
					rmpReceiptTotal = eRPReceiptInformationDto.rmpReceiptTotal,
					rmpReceiptTotalForeign = eRPReceiptInformationDto.rmpReceiptTotalForeign,
					rmpRowVersion = eRPReceiptInformationDto.rmpRowVersion,
					rmpShippingMethodID = eRPReceiptInformationDto.rmpShippingMethodID,
					rmpSupplierOrganizationID = eRPReceiptInformationDto.rmpSupplierOrganizationID,
					CustomFields = eRPReceiptInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Receipts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPReceiptDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = receiptDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPReceiptDto>> Process_PutReceipt(ERPReceiptDto receipt)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPReceiptDto createdObject = null;
		ERPResponseMessageDto<ERPReceiptDto> result;
		try
		{
			IERPReceiptRepository iERPReceiptRepository = (base.ERPReceiptRepository = new ERPReceiptRepository(base.ApiClientContext));
			using (iERPReceiptRepository)
			{
				APIValidationInfoDto postResult = await base.ERPReceiptRepository.SaveReceipt(receipt);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPReceiptInformationDto eRPReceiptInformationDto = await base.ERPReceiptRepository.GetReceipt(receipt.rmpUniqueID);
					createdObject = new ERPReceiptDto
					{
						rmpApInvoiceContactID = eRPReceiptInformationDto.rmpApInvoiceContactID,
						rmpApInvoiceLocationID = eRPReceiptInformationDto.rmpApInvoiceLocationID,
						rmpClosedDate = eRPReceiptInformationDto.rmpClosedDate,
						rmpReceiptID = eRPReceiptInformationDto.rmpReceiptID,
						rmpCreatedBy = eRPReceiptInformationDto.rmpCreatedBy,
						rmpCreatedDate = eRPReceiptInformationDto.rmpCreatedDate,
						rmpCurrencyRateID = eRPReceiptInformationDto.rmpCurrencyRateID,
						rmpDeliveryDocket = eRPReceiptInformationDto.rmpDeliveryDocket,
						rmpUniqueID = eRPReceiptInformationDto.rmpUniqueID,
						rmpExchangeRate = eRPReceiptInformationDto.rmpExchangeRate,
						rmpFreightCharge = eRPReceiptInformationDto.rmpFreightCharge,
						rmpFreightChargeForeign = eRPReceiptInformationDto.rmpFreightChargeForeign,
						rmpClosed = eRPReceiptInformationDto.rmpClosed,
						rmpCustomRate = eRPReceiptInformationDto.rmpCustomRate,
						rmpNestlinkProcessed = eRPReceiptInformationDto.rmpNestlinkProcessed,
						rmpPostedToGl = eRPReceiptInformationDto.rmpPostedToGl,
						rmpReversalEntry = eRPReceiptInformationDto.rmpReversalEntry,
						rmpReversed = eRPReceiptInformationDto.rmpReversed,
						rmpLandedCostID = eRPReceiptInformationDto.rmpLandedCostID,
						rmpPlantDepartmentID = eRPReceiptInformationDto.rmpPlantDepartmentID,
						rmpPlantID = eRPReceiptInformationDto.rmpPlantID,
						rmpPostedDate = eRPReceiptInformationDto.rmpPostedDate,
						rmpProjectID = eRPReceiptInformationDto.rmpProjectID,
						rmpPurchaseContactID = eRPReceiptInformationDto.rmpPurchaseContactID,
						rmpPurchaseLocationID = eRPReceiptInformationDto.rmpPurchaseLocationID,
						rmpReceiptDate = eRPReceiptInformationDto.rmpReceiptDate,
						rmpReceiptSubtotal = eRPReceiptInformationDto.rmpReceiptSubtotal,
						rmpReceiptSubtotalForeign = eRPReceiptInformationDto.rmpReceiptSubtotalForeign,
						rmpReceiptTotal = eRPReceiptInformationDto.rmpReceiptTotal,
						rmpReceiptTotalForeign = eRPReceiptInformationDto.rmpReceiptTotalForeign,
						rmpRowVersion = eRPReceiptInformationDto.rmpRowVersion,
						rmpShippingMethodID = eRPReceiptInformationDto.rmpShippingMethodID,
						rmpSupplierOrganizationID = eRPReceiptInformationDto.rmpSupplierOrganizationID,
						CustomFields = eRPReceiptInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Receipt [{receipt.rmpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPReceiptDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteReceipt(Guid receiptId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPReceiptRepository iERPReceiptRepository = (base.ERPReceiptRepository = new ERPReceiptRepository(base.ApiClientContext));
		using (iERPReceiptRepository)
		{
			if (!(await base.ERPReceiptRepository.DoesReceiptExist(receiptId)))
			{
				base.ErrorsList.Add($"Receipt [{receiptId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPReceiptInformationDto eRPReceiptInformationDto = await base.ERPReceiptRepository.GetReceipt(receiptId);
				string text = await base.ERPReceiptRepository.WhereUsed("Receipts", new object[1] { eRPReceiptInformationDto.rmpReceiptID }, new object[1] { "rmpReceiptID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Receipt cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPReceiptDto>> Process_DeleteReceipt(Guid receiptId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPReceiptDto> result;
		try
		{
			IERPReceiptRepository iERPReceiptRepository = (base.ERPReceiptRepository = new ERPReceiptRepository(base.ApiClientContext));
			using (iERPReceiptRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPReceiptRepository.DeleteRowFromTable("Receipts", "rmp", receiptId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Receipt [{receiptId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPReceiptDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPReceiptDto()
			};
		}
		return result;
	}
}
