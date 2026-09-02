using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPARInvoiceLineModel : ERPBaseModel, IERPARInvoiceLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllARInvoiceLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPARInvoiceLineRepository iERPARInvoiceLineRepository = (base.ERPARInvoiceLineRepository = new ERPARInvoiceLineRepository(base.ApiClientContext));
		using (iERPARInvoiceLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPARInvoiceLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPARInvoiceLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPARInvoiceLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPARInvoiceLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetARInvoiceLine(Guid aRInvoiceLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARInvoiceLineRepository iERPARInvoiceLineRepository = (base.ERPARInvoiceLineRepository = new ERPARInvoiceLineRepository(base.ApiClientContext));
		using (iERPARInvoiceLineRepository)
		{
			if (!(await base.ERPARInvoiceLineRepository.DoesARInvoiceLineExist(aRInvoiceLineId)))
			{
				errorsList.Add($"ARInvoiceLine [{aRInvoiceLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutARInvoiceLine(ERPARInvoiceLineDto aRInvoiceLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARInvoiceLineRepository iERPARInvoiceLineRepository = (base.ERPARInvoiceLineRepository = new ERPARInvoiceLineRepository(base.ApiClientContext));
		using (iERPARInvoiceLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlArInvoiceID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("ARInvoices", new object[1] { "ARPARINVOICEID" }, new object[1] { aRInvoiceLine.arlArInvoiceID })))
			{
				errorsList.Add("arlArInvoiceID [" + aRInvoiceLine.arlArInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlPartID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { aRInvoiceLine.arlPartID })))
			{
				errorsList.Add("arlPartID [" + aRInvoiceLine.arlPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlPartRevisionID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { aRInvoiceLine.arlPartID, aRInvoiceLine.arlPartRevisionID })))
			{
				errorsList.Add("arlPartRevisionID [" + aRInvoiceLine.arlPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlPartGroupID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("PartGroups", new object[1] { "IMUPARTGROUPID" }, new object[1] { aRInvoiceLine.arlPartGroupID })))
			{
				errorsList.Add("arlPartGroupID [" + aRInvoiceLine.arlPartGroupID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlTaxCodeID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aRInvoiceLine.arlTaxCodeID })))
			{
				errorsList.Add("arlTaxCodeID [" + aRInvoiceLine.arlTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlNonTaxReasonID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { aRInvoiceLine.arlNonTaxReasonID })))
			{
				errorsList.Add("arlNonTaxReasonID [" + aRInvoiceLine.arlNonTaxReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlSecondTaxCodeID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aRInvoiceLine.arlSecondTaxCodeID })))
			{
				errorsList.Add("arlSecondTaxCodeID [" + aRInvoiceLine.arlSecondTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlSalesOrderID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { aRInvoiceLine.arlSalesOrderID })))
			{
				errorsList.Add("arlSalesOrderID [" + aRInvoiceLine.arlSalesOrderID + "] not found.");
			}
			if (aRInvoiceLine.arlSalesOrderLineID > 0 && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("SalesOrderLines", new object[2] { "OMLSALESORDERID", "OMLSALESORDERLINEID" }, new object[2] { aRInvoiceLine.arlSalesOrderID, aRInvoiceLine.arlSalesOrderLineID })))
			{
				errorsList.Add($"arlSalesOrderLineID [{aRInvoiceLine.arlSalesOrderLineID}] not found.");
			}
			if (aRInvoiceLine.arlSalesOrderDeliveryID > 0 && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("SalesOrderDeliveries", new object[3] { "OMDSALESORDERID", "OMDSALESORDERLINEID", "OMDSALESORDERDELIVERYID" }, new object[3] { aRInvoiceLine.arlSalesOrderID, aRInvoiceLine.arlSalesOrderLineID, aRInvoiceLine.arlSalesOrderDeliveryID })))
			{
				errorsList.Add($"arlSalesOrderDeliveryID [{aRInvoiceLine.arlSalesOrderDeliveryID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlShipmentID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("Shipments", new object[1] { "SMPSHIPMENTID" }, new object[1] { aRInvoiceLine.arlShipmentID })))
			{
				errorsList.Add("arlShipmentID [" + aRInvoiceLine.arlShipmentID + "] not found.");
			}
			if (aRInvoiceLine.arlShipmentLineID > 0 && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("ShipmentLines", new object[2] { "SMLSHIPMENTID", "SMLSHIPMENTLINEID" }, new object[2] { aRInvoiceLine.arlShipmentID, aRInvoiceLine.arlShipmentLineID })))
			{
				errorsList.Add($"arlShipmentLineID [{aRInvoiceLine.arlShipmentLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlAssetID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("Assets", new object[1] { "FAPASSETID" }, new object[1] { aRInvoiceLine.arlAssetID })))
			{
				errorsList.Add("arlAssetID [" + aRInvoiceLine.arlAssetID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlRmaClaimID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("RMAClaims", new object[1] { "RAPRMACLAIMID" }, new object[1] { aRInvoiceLine.arlRmaClaimID })))
			{
				errorsList.Add("arlRmaClaimID [" + aRInvoiceLine.arlRmaClaimID + "] not found.");
			}
			if (aRInvoiceLine.arlRmaClaimLineID > 0 && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("RMAClaimLines", new object[2] { "RALRMACLAIMID", "RALRMACLAIMLINEID" }, new object[2] { aRInvoiceLine.arlRmaClaimID, aRInvoiceLine.arlRmaClaimLineID })))
			{
				errorsList.Add($"arlRmaClaimLineID [{aRInvoiceLine.arlRmaClaimLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlRmaReceiptID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("RMAReceipts", new object[1] { "RRPRMARECEIPTID" }, new object[1] { aRInvoiceLine.arlRmaReceiptID })))
			{
				errorsList.Add("arlRmaReceiptID [" + aRInvoiceLine.arlRmaReceiptID + "] not found.");
			}
			if (aRInvoiceLine.arlRmaReceiptLineID > 0 && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("RMAReceiptLines", new object[2] { "RRLRMARECEIPTID", "RRLRMARECEIPTLINEID" }, new object[2] { aRInvoiceLine.arlRmaReceiptID, aRInvoiceLine.arlRmaReceiptLineID })))
			{
				errorsList.Add($"arlRmaReceiptLineID [{aRInvoiceLine.arlRmaReceiptLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlProjectID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { aRInvoiceLine.arlProjectID })))
			{
				errorsList.Add("arlProjectID [" + aRInvoiceLine.arlProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlProjectAreaID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { aRInvoiceLine.arlProjectID, aRInvoiceLine.arlProjectAreaID })))
			{
				errorsList.Add("arlProjectAreaID [" + aRInvoiceLine.arlProjectAreaID + "] not found.");
			}
			if (aRInvoiceLine.arlArRecurringInvoiceID > 0 && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("ARRecurringInvoices", new object[1] { "ARRARRECURRINGINVOICEID" }, new object[1] { aRInvoiceLine.arlArRecurringInvoiceID })))
			{
				errorsList.Add($"arlArRecurringInvoiceID [{aRInvoiceLine.arlArRecurringInvoiceID}] not found.");
			}
			if (aRInvoiceLine.arlArRecurringInvoiceLineID > 0 && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("ARRecurringInvoiceLines", new object[2] { "ARQARRECURRINGINVOICEID", "ARQARRECURRINGINVOICELINEID" }, new object[2] { aRInvoiceLine.arlArRecurringInvoiceID, aRInvoiceLine.arlArRecurringInvoiceLineID })))
			{
				errorsList.Add($"arlArRecurringInvoiceLineID [{aRInvoiceLine.arlArRecurringInvoiceLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlDepositInvoiceID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("ARInvoices", new object[1] { "ARPARINVOICEID" }, new object[1] { aRInvoiceLine.arlDepositInvoiceID })))
			{
				errorsList.Add("arlDepositInvoiceID [" + aRInvoiceLine.arlDepositInvoiceID + "] not found.");
			}
			if (aRInvoiceLine.arlDepositInvoiceLineID > 0 && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("ARInvoiceLines", new object[2] { "ARLARINVOICEID", "ARLARINVOICELINEID" }, new object[2] { aRInvoiceLine.arlDepositInvoiceID, aRInvoiceLine.arlDepositInvoiceLineID })))
			{
				errorsList.Add($"arlDepositInvoiceLineID [{aRInvoiceLine.arlDepositInvoiceLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlCallID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("Calls", new object[1] { "KBPCALLID" }, new object[1] { aRInvoiceLine.arlCallID })))
			{
				errorsList.Add("arlCallID [" + aRInvoiceLine.arlCallID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlJobID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { aRInvoiceLine.arlJobID })))
			{
				errorsList.Add("arlJobID [" + aRInvoiceLine.arlJobID + "] not found.");
			}
			if (aRInvoiceLine.arlJobAssemblyID > 0 && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { aRInvoiceLine.arlJobID, aRInvoiceLine.arlJobAssemblyID })))
			{
				errorsList.Add($"arlJobAssemblyID [{aRInvoiceLine.arlJobAssemblyID}] not found.");
			}
			if (aRInvoiceLine.arlJobMaterialID > 0 && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { aRInvoiceLine.arlJobID, aRInvoiceLine.arlJobAssemblyID, aRInvoiceLine.arlJobMaterialID })))
			{
				errorsList.Add($"arlJobMaterialID [{aRInvoiceLine.arlJobMaterialID}] not found.");
			}
			if (aRInvoiceLine.arlAssetAdjustmentID > 0 && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("AssetAdjustments", new object[1] { "FAAASSETADJUSTMENTID" }, new object[1] { aRInvoiceLine.arlAssetAdjustmentID })))
			{
				errorsList.Add($"arlAssetAdjustmentID [{aRInvoiceLine.arlAssetAdjustmentID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceLine.arlFinanceSourceInvoiceID) && !(await base.ERPARInvoiceLineRepository.DoesRecordExistInTableUsingKeys("ARInvoices", new object[1] { "ARPARINVOICEID" }, new object[1] { aRInvoiceLine.arlFinanceSourceInvoiceID })))
			{
				errorsList.Add("arlFinanceSourceInvoiceID [" + aRInvoiceLine.arlFinanceSourceInvoiceID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPARInvoiceLineDto>>> Process_GetAllARInvoiceLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPARInvoiceLineDto> allARInvoiceLinesDto = new List<ERPARInvoiceLineDto>();
		ERPResponseMessageDto<IList<ERPARInvoiceLineDto>> result;
		try
		{
			IERPARInvoiceLineRepository iERPARInvoiceLineRepository = (base.ERPARInvoiceLineRepository = new ERPARInvoiceLineRepository(base.ApiClientContext));
			using (iERPARInvoiceLineRepository)
			{
				foreach (ERPARInvoiceLineInformationDto item2 in await base.ERPARInvoiceLineRepository.GetAllARInvoiceLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPARInvoiceLineDto item = new ERPARInvoiceLineDto
					{
						arlActualTotalCostOfGoodsSold = item2.arlActualTotalCostOfGoodsSold,
						arlActualTotalLaborCost = item2.arlActualTotalLaborCost,
						arlActualTotalMaterialCost = item2.arlActualTotalMaterialCost,
						arlActualTotalOverheadCost = item2.arlActualTotalOverheadCost,
						arlActualTotalSubcontractCost = item2.arlActualTotalSubcontractCost,
						arlActualUnitCostOfGoodsSold = item2.arlActualUnitCostOfGoodsSold,
						arlActualUnitLaborCost = item2.arlActualUnitLaborCost,
						arlActualUnitMaterialCost = item2.arlActualUnitMaterialCost,
						arlActualUnitOverheadCost = item2.arlActualUnitOverheadCost,
						arlActualUnitSubcontractCost = item2.arlActualUnitSubcontractCost,
						arlAmtForResellerCommission = item2.arlAmtForResellerCommission,
						arlAmtForSalesCommission = item2.arlAmtForSalesCommission,
						arlArInvoiceID = item2.arlArInvoiceID,
						arlArRecurringInvoiceID = item2.arlArRecurringInvoiceID,
						arlArRecurringInvoiceLineID = item2.arlArRecurringInvoiceLineID,
						arlAssetAdjustmentID = item2.arlAssetAdjustmentID,
						arlAssetID = item2.arlAssetID,
						arlCallID = item2.arlCallID,
						arlCogsCalculatedDate = item2.arlCogsCalculatedDate,
						arlCommissionAmount = item2.arlCommissionAmount,
						arlCommissionRate = item2.arlCommissionRate,
						arlCreatedBy = item2.arlCreatedBy,
						arlCreatedDate = item2.arlCreatedDate,
						arlCustomerPo = item2.arlCustomerPo,
						arlDepositAmountBase = item2.arlDepositAmountBase,
						arlDepositAmountForeign = item2.arlDepositAmountForeign,
						arlDepositBalanceBase = item2.arlDepositBalanceBase,
						arlDepositBalanceForeign = item2.arlDepositBalanceForeign,
						arlDepositInvoiceID = item2.arlDepositInvoiceID,
						arlDepositInvoiceLineID = item2.arlDepositInvoiceLineID,
						arlDepositTransferredBase = item2.arlDepositTransferredBase,
						arlDepositTransferredForeign = item2.arlDepositTransferredForeign,
						arlDiscountPercent = item2.arlDiscountPercent,
						arlUniqueID = item2.arlUniqueID,
						arlEstTotalCostOfGoodsSold = item2.arlEstTotalCostOfGoodsSold,
						arlEstTotalLaborCost = item2.arlEstTotalLaborCost,
						arlEstTotalMaterialCost = item2.arlEstTotalMaterialCost,
						arlEstTotalOverheadCost = item2.arlEstTotalOverheadCost,
						arlEstTotalSubcontractCost = item2.arlEstTotalSubcontractCost,
						arlEstUnitCostOfGoodsSold = item2.arlEstUnitCostOfGoodsSold,
						arlEstUnitLaborCost = item2.arlEstUnitLaborCost,
						arlEstUnitMaterialCost = item2.arlEstUnitMaterialCost,
						arlEstUnitOverheadCost = item2.arlEstUnitOverheadCost,
						arlEstUnitSubcontractCost = item2.arlEstUnitSubcontractCost,
						arlExtendedDiscountBase = item2.arlExtendedDiscountBase,
						arlExtendedDiscountForeign = item2.arlExtendedDiscountForeign,
						arlExtendedPriceBase = item2.arlExtendedPriceBase,
						arlExtendedPriceForeign = item2.arlExtendedPriceForeign,
						arlFinanceSourceInvoiceID = item2.arlFinanceSourceInvoiceID,
						arlFreightAmountBase = item2.arlFreightAmountBase,
						arlFreightAmountForeign = item2.arlFreightAmountForeign,
						arlFullExtendedPriceBase = item2.arlFullExtendedPriceBase,
						arlFullExtendedPriceForeign = item2.arlFullExtendedPriceForeign,
						arlFullUnitPriceBase = item2.arlFullUnitPriceBase,
						arlFullUnitPriceForeign = item2.arlFullUnitPriceForeign,
						arlInvoiceQuantity = item2.arlInvoiceQuantity,
						arlAvalaraIgnoreLine = item2.arlAvalaraIgnoreLine,
						arlCogsPostedToGl = item2.arlCogsPostedToGl,
						arlDeliveryInvoicedComplete = item2.arlDeliveryInvoicedComplete,
						arlDepositLine = item2.arlDepositLine,
						arlIncludeTaxInRetention = item2.arlIncludeTaxInRetention,
						arlIntraCompanyPosted = item2.arlIntraCompanyPosted,
						arlPayCommission = item2.arlPayCommission,
						arlPostedToGl = item2.arlPostedToGl,
						arlRetention = item2.arlRetention,
						arlJobAssemblyID = item2.arlJobAssemblyID,
						arlJobID = item2.arlJobID,
						arlJobMaterialID = item2.arlJobMaterialID,
						arlLineType = item2.arlLineType,
						arlNonTaxReasonID = item2.arlNonTaxReasonID,
						arlOrderQuantity = item2.arlOrderQuantity,
						arlOrgPartID = item2.arlOrgPartID,
						arlOrgPartShortDescription = item2.arlOrgPartShortDescription,
						arlPartGroupID = item2.arlPartGroupID,
						arlPartID = item2.arlPartID,
						arlPartLongDescriptionRtf = item2.arlPartLongDescriptionRtf,
						arlPartLongDescriptionText = item2.arlPartLongDescriptionText,
						arlPartRevisionID = item2.arlPartRevisionID,
						arlPartShortDescription = item2.arlPartShortDescription,
						arlProjectAreaID = item2.arlProjectAreaID,
						arlProjectID = item2.arlProjectID,
						arlRetentionAmountBase = item2.arlRetentionAmountBase,
						arlRetentionAmountForeign = item2.arlRetentionAmountForeign,
						arlRetentionDueDate = item2.arlRetentionDueDate,
						arlRetentionPercent = item2.arlRetentionPercent,
						arlRmaClaimID = item2.arlRmaClaimID,
						arlRmaClaimLineID = item2.arlRmaClaimLineID,
						arlRmaReceiptID = item2.arlRmaReceiptID,
						arlRmaReceiptLineID = item2.arlRmaReceiptLineID,
						arlRowVersion = item2.arlRowVersion,
						arlSalesOrderDeliveryID = item2.arlSalesOrderDeliveryID,
						arlSalesOrderID = item2.arlSalesOrderID,
						arlSalesOrderLineID = item2.arlSalesOrderLineID,
						arlSecondTaxAmountBase = item2.arlSecondTaxAmountBase,
						arlSecondTaxAmountForeign = item2.arlSecondTaxAmountForeign,
						arlSecondTaxCodeID = item2.arlSecondTaxCodeID,
						arlArInvoiceLineID = item2.arlArInvoiceLineID,
						arlShipmentID = item2.arlShipmentID,
						arlShipmentLineID = item2.arlShipmentLineID,
						arlTaxAmountBase = item2.arlTaxAmountBase,
						arlTaxAmountForeign = item2.arlTaxAmountForeign,
						arlTaxCodeID = item2.arlTaxCodeID,
						arlTaxDate = item2.arlTaxDate,
						arlUnitDiscountBase = item2.arlUnitDiscountBase,
						arlUnitDiscountForeign = item2.arlUnitDiscountForeign,
						arlUnitOfMeasure = item2.arlUnitOfMeasure,
						arlUnitPriceBase = item2.arlUnitPriceBase,
						arlUnitPriceForeign = item2.arlUnitPriceForeign,
						CustomFields = item2.CustomFields
					};
					allARInvoiceLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ARInvoiceLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPARInvoiceLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allARInvoiceLinesDto,
				RecordCount = allARInvoiceLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPARInvoiceLineDto>> Process_GetARInvoiceLine(Guid aRInvoiceLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPARInvoiceLineDto aRInvoiceLineDto = null;
		ERPResponseMessageDto<ERPARInvoiceLineDto> result;
		try
		{
			IERPARInvoiceLineRepository iERPARInvoiceLineRepository = (base.ERPARInvoiceLineRepository = new ERPARInvoiceLineRepository(base.ApiClientContext));
			using (iERPARInvoiceLineRepository)
			{
				ERPARInvoiceLineInformationDto eRPARInvoiceLineInformationDto = await base.ERPARInvoiceLineRepository.GetARInvoiceLine(aRInvoiceLineId);
				aRInvoiceLineDto = new ERPARInvoiceLineDto
				{
					arlActualTotalCostOfGoodsSold = eRPARInvoiceLineInformationDto.arlActualTotalCostOfGoodsSold,
					arlActualTotalLaborCost = eRPARInvoiceLineInformationDto.arlActualTotalLaborCost,
					arlActualTotalMaterialCost = eRPARInvoiceLineInformationDto.arlActualTotalMaterialCost,
					arlActualTotalOverheadCost = eRPARInvoiceLineInformationDto.arlActualTotalOverheadCost,
					arlActualTotalSubcontractCost = eRPARInvoiceLineInformationDto.arlActualTotalSubcontractCost,
					arlActualUnitCostOfGoodsSold = eRPARInvoiceLineInformationDto.arlActualUnitCostOfGoodsSold,
					arlActualUnitLaborCost = eRPARInvoiceLineInformationDto.arlActualUnitLaborCost,
					arlActualUnitMaterialCost = eRPARInvoiceLineInformationDto.arlActualUnitMaterialCost,
					arlActualUnitOverheadCost = eRPARInvoiceLineInformationDto.arlActualUnitOverheadCost,
					arlActualUnitSubcontractCost = eRPARInvoiceLineInformationDto.arlActualUnitSubcontractCost,
					arlAmtForResellerCommission = eRPARInvoiceLineInformationDto.arlAmtForResellerCommission,
					arlAmtForSalesCommission = eRPARInvoiceLineInformationDto.arlAmtForSalesCommission,
					arlArInvoiceID = eRPARInvoiceLineInformationDto.arlArInvoiceID,
					arlArRecurringInvoiceID = eRPARInvoiceLineInformationDto.arlArRecurringInvoiceID,
					arlArRecurringInvoiceLineID = eRPARInvoiceLineInformationDto.arlArRecurringInvoiceLineID,
					arlAssetAdjustmentID = eRPARInvoiceLineInformationDto.arlAssetAdjustmentID,
					arlAssetID = eRPARInvoiceLineInformationDto.arlAssetID,
					arlCallID = eRPARInvoiceLineInformationDto.arlCallID,
					arlCogsCalculatedDate = eRPARInvoiceLineInformationDto.arlCogsCalculatedDate,
					arlCommissionAmount = eRPARInvoiceLineInformationDto.arlCommissionAmount,
					arlCommissionRate = eRPARInvoiceLineInformationDto.arlCommissionRate,
					arlCreatedBy = eRPARInvoiceLineInformationDto.arlCreatedBy,
					arlCreatedDate = eRPARInvoiceLineInformationDto.arlCreatedDate,
					arlCustomerPo = eRPARInvoiceLineInformationDto.arlCustomerPo,
					arlDepositAmountBase = eRPARInvoiceLineInformationDto.arlDepositAmountBase,
					arlDepositAmountForeign = eRPARInvoiceLineInformationDto.arlDepositAmountForeign,
					arlDepositBalanceBase = eRPARInvoiceLineInformationDto.arlDepositBalanceBase,
					arlDepositBalanceForeign = eRPARInvoiceLineInformationDto.arlDepositBalanceForeign,
					arlDepositInvoiceID = eRPARInvoiceLineInformationDto.arlDepositInvoiceID,
					arlDepositInvoiceLineID = eRPARInvoiceLineInformationDto.arlDepositInvoiceLineID,
					arlDepositTransferredBase = eRPARInvoiceLineInformationDto.arlDepositTransferredBase,
					arlDepositTransferredForeign = eRPARInvoiceLineInformationDto.arlDepositTransferredForeign,
					arlDiscountPercent = eRPARInvoiceLineInformationDto.arlDiscountPercent,
					arlUniqueID = eRPARInvoiceLineInformationDto.arlUniqueID,
					arlEstTotalCostOfGoodsSold = eRPARInvoiceLineInformationDto.arlEstTotalCostOfGoodsSold,
					arlEstTotalLaborCost = eRPARInvoiceLineInformationDto.arlEstTotalLaborCost,
					arlEstTotalMaterialCost = eRPARInvoiceLineInformationDto.arlEstTotalMaterialCost,
					arlEstTotalOverheadCost = eRPARInvoiceLineInformationDto.arlEstTotalOverheadCost,
					arlEstTotalSubcontractCost = eRPARInvoiceLineInformationDto.arlEstTotalSubcontractCost,
					arlEstUnitCostOfGoodsSold = eRPARInvoiceLineInformationDto.arlEstUnitCostOfGoodsSold,
					arlEstUnitLaborCost = eRPARInvoiceLineInformationDto.arlEstUnitLaborCost,
					arlEstUnitMaterialCost = eRPARInvoiceLineInformationDto.arlEstUnitMaterialCost,
					arlEstUnitOverheadCost = eRPARInvoiceLineInformationDto.arlEstUnitOverheadCost,
					arlEstUnitSubcontractCost = eRPARInvoiceLineInformationDto.arlEstUnitSubcontractCost,
					arlExtendedDiscountBase = eRPARInvoiceLineInformationDto.arlExtendedDiscountBase,
					arlExtendedDiscountForeign = eRPARInvoiceLineInformationDto.arlExtendedDiscountForeign,
					arlExtendedPriceBase = eRPARInvoiceLineInformationDto.arlExtendedPriceBase,
					arlExtendedPriceForeign = eRPARInvoiceLineInformationDto.arlExtendedPriceForeign,
					arlFinanceSourceInvoiceID = eRPARInvoiceLineInformationDto.arlFinanceSourceInvoiceID,
					arlFreightAmountBase = eRPARInvoiceLineInformationDto.arlFreightAmountBase,
					arlFreightAmountForeign = eRPARInvoiceLineInformationDto.arlFreightAmountForeign,
					arlFullExtendedPriceBase = eRPARInvoiceLineInformationDto.arlFullExtendedPriceBase,
					arlFullExtendedPriceForeign = eRPARInvoiceLineInformationDto.arlFullExtendedPriceForeign,
					arlFullUnitPriceBase = eRPARInvoiceLineInformationDto.arlFullUnitPriceBase,
					arlFullUnitPriceForeign = eRPARInvoiceLineInformationDto.arlFullUnitPriceForeign,
					arlInvoiceQuantity = eRPARInvoiceLineInformationDto.arlInvoiceQuantity,
					arlAvalaraIgnoreLine = eRPARInvoiceLineInformationDto.arlAvalaraIgnoreLine,
					arlCogsPostedToGl = eRPARInvoiceLineInformationDto.arlCogsPostedToGl,
					arlDeliveryInvoicedComplete = eRPARInvoiceLineInformationDto.arlDeliveryInvoicedComplete,
					arlDepositLine = eRPARInvoiceLineInformationDto.arlDepositLine,
					arlIncludeTaxInRetention = eRPARInvoiceLineInformationDto.arlIncludeTaxInRetention,
					arlIntraCompanyPosted = eRPARInvoiceLineInformationDto.arlIntraCompanyPosted,
					arlPayCommission = eRPARInvoiceLineInformationDto.arlPayCommission,
					arlPostedToGl = eRPARInvoiceLineInformationDto.arlPostedToGl,
					arlRetention = eRPARInvoiceLineInformationDto.arlRetention,
					arlJobAssemblyID = eRPARInvoiceLineInformationDto.arlJobAssemblyID,
					arlJobID = eRPARInvoiceLineInformationDto.arlJobID,
					arlJobMaterialID = eRPARInvoiceLineInformationDto.arlJobMaterialID,
					arlLineType = eRPARInvoiceLineInformationDto.arlLineType,
					arlNonTaxReasonID = eRPARInvoiceLineInformationDto.arlNonTaxReasonID,
					arlOrderQuantity = eRPARInvoiceLineInformationDto.arlOrderQuantity,
					arlOrgPartID = eRPARInvoiceLineInformationDto.arlOrgPartID,
					arlOrgPartShortDescription = eRPARInvoiceLineInformationDto.arlOrgPartShortDescription,
					arlPartGroupID = eRPARInvoiceLineInformationDto.arlPartGroupID,
					arlPartID = eRPARInvoiceLineInformationDto.arlPartID,
					arlPartLongDescriptionRtf = eRPARInvoiceLineInformationDto.arlPartLongDescriptionRtf,
					arlPartLongDescriptionText = eRPARInvoiceLineInformationDto.arlPartLongDescriptionText,
					arlPartRevisionID = eRPARInvoiceLineInformationDto.arlPartRevisionID,
					arlPartShortDescription = eRPARInvoiceLineInformationDto.arlPartShortDescription,
					arlProjectAreaID = eRPARInvoiceLineInformationDto.arlProjectAreaID,
					arlProjectID = eRPARInvoiceLineInformationDto.arlProjectID,
					arlRetentionAmountBase = eRPARInvoiceLineInformationDto.arlRetentionAmountBase,
					arlRetentionAmountForeign = eRPARInvoiceLineInformationDto.arlRetentionAmountForeign,
					arlRetentionDueDate = eRPARInvoiceLineInformationDto.arlRetentionDueDate,
					arlRetentionPercent = eRPARInvoiceLineInformationDto.arlRetentionPercent,
					arlRmaClaimID = eRPARInvoiceLineInformationDto.arlRmaClaimID,
					arlRmaClaimLineID = eRPARInvoiceLineInformationDto.arlRmaClaimLineID,
					arlRmaReceiptID = eRPARInvoiceLineInformationDto.arlRmaReceiptID,
					arlRmaReceiptLineID = eRPARInvoiceLineInformationDto.arlRmaReceiptLineID,
					arlRowVersion = eRPARInvoiceLineInformationDto.arlRowVersion,
					arlSalesOrderDeliveryID = eRPARInvoiceLineInformationDto.arlSalesOrderDeliveryID,
					arlSalesOrderID = eRPARInvoiceLineInformationDto.arlSalesOrderID,
					arlSalesOrderLineID = eRPARInvoiceLineInformationDto.arlSalesOrderLineID,
					arlSecondTaxAmountBase = eRPARInvoiceLineInformationDto.arlSecondTaxAmountBase,
					arlSecondTaxAmountForeign = eRPARInvoiceLineInformationDto.arlSecondTaxAmountForeign,
					arlSecondTaxCodeID = eRPARInvoiceLineInformationDto.arlSecondTaxCodeID,
					arlArInvoiceLineID = eRPARInvoiceLineInformationDto.arlArInvoiceLineID,
					arlShipmentID = eRPARInvoiceLineInformationDto.arlShipmentID,
					arlShipmentLineID = eRPARInvoiceLineInformationDto.arlShipmentLineID,
					arlTaxAmountBase = eRPARInvoiceLineInformationDto.arlTaxAmountBase,
					arlTaxAmountForeign = eRPARInvoiceLineInformationDto.arlTaxAmountForeign,
					arlTaxCodeID = eRPARInvoiceLineInformationDto.arlTaxCodeID,
					arlTaxDate = eRPARInvoiceLineInformationDto.arlTaxDate,
					arlUnitDiscountBase = eRPARInvoiceLineInformationDto.arlUnitDiscountBase,
					arlUnitDiscountForeign = eRPARInvoiceLineInformationDto.arlUnitDiscountForeign,
					arlUnitOfMeasure = eRPARInvoiceLineInformationDto.arlUnitOfMeasure,
					arlUnitPriceBase = eRPARInvoiceLineInformationDto.arlUnitPriceBase,
					arlUnitPriceForeign = eRPARInvoiceLineInformationDto.arlUnitPriceForeign,
					CustomFields = eRPARInvoiceLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ARInvoiceLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARInvoiceLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = aRInvoiceLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPARInvoiceLineDto>> Process_PutARInvoiceLine(ERPARInvoiceLineDto aRInvoiceLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPARInvoiceLineDto createdObject = null;
		ERPResponseMessageDto<ERPARInvoiceLineDto> result;
		try
		{
			IERPARInvoiceLineRepository iERPARInvoiceLineRepository = (base.ERPARInvoiceLineRepository = new ERPARInvoiceLineRepository(base.ApiClientContext));
			using (iERPARInvoiceLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPARInvoiceLineRepository.SaveARInvoiceLine(aRInvoiceLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPARInvoiceLineInformationDto eRPARInvoiceLineInformationDto = await base.ERPARInvoiceLineRepository.GetARInvoiceLine(aRInvoiceLine.arlUniqueID);
					createdObject = new ERPARInvoiceLineDto
					{
						arlActualTotalCostOfGoodsSold = eRPARInvoiceLineInformationDto.arlActualTotalCostOfGoodsSold,
						arlActualTotalLaborCost = eRPARInvoiceLineInformationDto.arlActualTotalLaborCost,
						arlActualTotalMaterialCost = eRPARInvoiceLineInformationDto.arlActualTotalMaterialCost,
						arlActualTotalOverheadCost = eRPARInvoiceLineInformationDto.arlActualTotalOverheadCost,
						arlActualTotalSubcontractCost = eRPARInvoiceLineInformationDto.arlActualTotalSubcontractCost,
						arlActualUnitCostOfGoodsSold = eRPARInvoiceLineInformationDto.arlActualUnitCostOfGoodsSold,
						arlActualUnitLaborCost = eRPARInvoiceLineInformationDto.arlActualUnitLaborCost,
						arlActualUnitMaterialCost = eRPARInvoiceLineInformationDto.arlActualUnitMaterialCost,
						arlActualUnitOverheadCost = eRPARInvoiceLineInformationDto.arlActualUnitOverheadCost,
						arlActualUnitSubcontractCost = eRPARInvoiceLineInformationDto.arlActualUnitSubcontractCost,
						arlAmtForResellerCommission = eRPARInvoiceLineInformationDto.arlAmtForResellerCommission,
						arlAmtForSalesCommission = eRPARInvoiceLineInformationDto.arlAmtForSalesCommission,
						arlArInvoiceID = eRPARInvoiceLineInformationDto.arlArInvoiceID,
						arlArRecurringInvoiceID = eRPARInvoiceLineInformationDto.arlArRecurringInvoiceID,
						arlArRecurringInvoiceLineID = eRPARInvoiceLineInformationDto.arlArRecurringInvoiceLineID,
						arlAssetAdjustmentID = eRPARInvoiceLineInformationDto.arlAssetAdjustmentID,
						arlAssetID = eRPARInvoiceLineInformationDto.arlAssetID,
						arlCallID = eRPARInvoiceLineInformationDto.arlCallID,
						arlCogsCalculatedDate = eRPARInvoiceLineInformationDto.arlCogsCalculatedDate,
						arlCommissionAmount = eRPARInvoiceLineInformationDto.arlCommissionAmount,
						arlCommissionRate = eRPARInvoiceLineInformationDto.arlCommissionRate,
						arlCreatedBy = eRPARInvoiceLineInformationDto.arlCreatedBy,
						arlCreatedDate = eRPARInvoiceLineInformationDto.arlCreatedDate,
						arlCustomerPo = eRPARInvoiceLineInformationDto.arlCustomerPo,
						arlDepositAmountBase = eRPARInvoiceLineInformationDto.arlDepositAmountBase,
						arlDepositAmountForeign = eRPARInvoiceLineInformationDto.arlDepositAmountForeign,
						arlDepositBalanceBase = eRPARInvoiceLineInformationDto.arlDepositBalanceBase,
						arlDepositBalanceForeign = eRPARInvoiceLineInformationDto.arlDepositBalanceForeign,
						arlDepositInvoiceID = eRPARInvoiceLineInformationDto.arlDepositInvoiceID,
						arlDepositInvoiceLineID = eRPARInvoiceLineInformationDto.arlDepositInvoiceLineID,
						arlDepositTransferredBase = eRPARInvoiceLineInformationDto.arlDepositTransferredBase,
						arlDepositTransferredForeign = eRPARInvoiceLineInformationDto.arlDepositTransferredForeign,
						arlDiscountPercent = eRPARInvoiceLineInformationDto.arlDiscountPercent,
						arlUniqueID = eRPARInvoiceLineInformationDto.arlUniqueID,
						arlEstTotalCostOfGoodsSold = eRPARInvoiceLineInformationDto.arlEstTotalCostOfGoodsSold,
						arlEstTotalLaborCost = eRPARInvoiceLineInformationDto.arlEstTotalLaborCost,
						arlEstTotalMaterialCost = eRPARInvoiceLineInformationDto.arlEstTotalMaterialCost,
						arlEstTotalOverheadCost = eRPARInvoiceLineInformationDto.arlEstTotalOverheadCost,
						arlEstTotalSubcontractCost = eRPARInvoiceLineInformationDto.arlEstTotalSubcontractCost,
						arlEstUnitCostOfGoodsSold = eRPARInvoiceLineInformationDto.arlEstUnitCostOfGoodsSold,
						arlEstUnitLaborCost = eRPARInvoiceLineInformationDto.arlEstUnitLaborCost,
						arlEstUnitMaterialCost = eRPARInvoiceLineInformationDto.arlEstUnitMaterialCost,
						arlEstUnitOverheadCost = eRPARInvoiceLineInformationDto.arlEstUnitOverheadCost,
						arlEstUnitSubcontractCost = eRPARInvoiceLineInformationDto.arlEstUnitSubcontractCost,
						arlExtendedDiscountBase = eRPARInvoiceLineInformationDto.arlExtendedDiscountBase,
						arlExtendedDiscountForeign = eRPARInvoiceLineInformationDto.arlExtendedDiscountForeign,
						arlExtendedPriceBase = eRPARInvoiceLineInformationDto.arlExtendedPriceBase,
						arlExtendedPriceForeign = eRPARInvoiceLineInformationDto.arlExtendedPriceForeign,
						arlFinanceSourceInvoiceID = eRPARInvoiceLineInformationDto.arlFinanceSourceInvoiceID,
						arlFreightAmountBase = eRPARInvoiceLineInformationDto.arlFreightAmountBase,
						arlFreightAmountForeign = eRPARInvoiceLineInformationDto.arlFreightAmountForeign,
						arlFullExtendedPriceBase = eRPARInvoiceLineInformationDto.arlFullExtendedPriceBase,
						arlFullExtendedPriceForeign = eRPARInvoiceLineInformationDto.arlFullExtendedPriceForeign,
						arlFullUnitPriceBase = eRPARInvoiceLineInformationDto.arlFullUnitPriceBase,
						arlFullUnitPriceForeign = eRPARInvoiceLineInformationDto.arlFullUnitPriceForeign,
						arlInvoiceQuantity = eRPARInvoiceLineInformationDto.arlInvoiceQuantity,
						arlAvalaraIgnoreLine = eRPARInvoiceLineInformationDto.arlAvalaraIgnoreLine,
						arlCogsPostedToGl = eRPARInvoiceLineInformationDto.arlCogsPostedToGl,
						arlDeliveryInvoicedComplete = eRPARInvoiceLineInformationDto.arlDeliveryInvoicedComplete,
						arlDepositLine = eRPARInvoiceLineInformationDto.arlDepositLine,
						arlIncludeTaxInRetention = eRPARInvoiceLineInformationDto.arlIncludeTaxInRetention,
						arlIntraCompanyPosted = eRPARInvoiceLineInformationDto.arlIntraCompanyPosted,
						arlPayCommission = eRPARInvoiceLineInformationDto.arlPayCommission,
						arlPostedToGl = eRPARInvoiceLineInformationDto.arlPostedToGl,
						arlRetention = eRPARInvoiceLineInformationDto.arlRetention,
						arlJobAssemblyID = eRPARInvoiceLineInformationDto.arlJobAssemblyID,
						arlJobID = eRPARInvoiceLineInformationDto.arlJobID,
						arlJobMaterialID = eRPARInvoiceLineInformationDto.arlJobMaterialID,
						arlLineType = eRPARInvoiceLineInformationDto.arlLineType,
						arlNonTaxReasonID = eRPARInvoiceLineInformationDto.arlNonTaxReasonID,
						arlOrderQuantity = eRPARInvoiceLineInformationDto.arlOrderQuantity,
						arlOrgPartID = eRPARInvoiceLineInformationDto.arlOrgPartID,
						arlOrgPartShortDescription = eRPARInvoiceLineInformationDto.arlOrgPartShortDescription,
						arlPartGroupID = eRPARInvoiceLineInformationDto.arlPartGroupID,
						arlPartID = eRPARInvoiceLineInformationDto.arlPartID,
						arlPartLongDescriptionRtf = eRPARInvoiceLineInformationDto.arlPartLongDescriptionRtf,
						arlPartLongDescriptionText = eRPARInvoiceLineInformationDto.arlPartLongDescriptionText,
						arlPartRevisionID = eRPARInvoiceLineInformationDto.arlPartRevisionID,
						arlPartShortDescription = eRPARInvoiceLineInformationDto.arlPartShortDescription,
						arlProjectAreaID = eRPARInvoiceLineInformationDto.arlProjectAreaID,
						arlProjectID = eRPARInvoiceLineInformationDto.arlProjectID,
						arlRetentionAmountBase = eRPARInvoiceLineInformationDto.arlRetentionAmountBase,
						arlRetentionAmountForeign = eRPARInvoiceLineInformationDto.arlRetentionAmountForeign,
						arlRetentionDueDate = eRPARInvoiceLineInformationDto.arlRetentionDueDate,
						arlRetentionPercent = eRPARInvoiceLineInformationDto.arlRetentionPercent,
						arlRmaClaimID = eRPARInvoiceLineInformationDto.arlRmaClaimID,
						arlRmaClaimLineID = eRPARInvoiceLineInformationDto.arlRmaClaimLineID,
						arlRmaReceiptID = eRPARInvoiceLineInformationDto.arlRmaReceiptID,
						arlRmaReceiptLineID = eRPARInvoiceLineInformationDto.arlRmaReceiptLineID,
						arlRowVersion = eRPARInvoiceLineInformationDto.arlRowVersion,
						arlSalesOrderDeliveryID = eRPARInvoiceLineInformationDto.arlSalesOrderDeliveryID,
						arlSalesOrderID = eRPARInvoiceLineInformationDto.arlSalesOrderID,
						arlSalesOrderLineID = eRPARInvoiceLineInformationDto.arlSalesOrderLineID,
						arlSecondTaxAmountBase = eRPARInvoiceLineInformationDto.arlSecondTaxAmountBase,
						arlSecondTaxAmountForeign = eRPARInvoiceLineInformationDto.arlSecondTaxAmountForeign,
						arlSecondTaxCodeID = eRPARInvoiceLineInformationDto.arlSecondTaxCodeID,
						arlArInvoiceLineID = eRPARInvoiceLineInformationDto.arlArInvoiceLineID,
						arlShipmentID = eRPARInvoiceLineInformationDto.arlShipmentID,
						arlShipmentLineID = eRPARInvoiceLineInformationDto.arlShipmentLineID,
						arlTaxAmountBase = eRPARInvoiceLineInformationDto.arlTaxAmountBase,
						arlTaxAmountForeign = eRPARInvoiceLineInformationDto.arlTaxAmountForeign,
						arlTaxCodeID = eRPARInvoiceLineInformationDto.arlTaxCodeID,
						arlTaxDate = eRPARInvoiceLineInformationDto.arlTaxDate,
						arlUnitDiscountBase = eRPARInvoiceLineInformationDto.arlUnitDiscountBase,
						arlUnitDiscountForeign = eRPARInvoiceLineInformationDto.arlUnitDiscountForeign,
						arlUnitOfMeasure = eRPARInvoiceLineInformationDto.arlUnitOfMeasure,
						arlUnitPriceBase = eRPARInvoiceLineInformationDto.arlUnitPriceBase,
						arlUnitPriceForeign = eRPARInvoiceLineInformationDto.arlUnitPriceForeign,
						CustomFields = eRPARInvoiceLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ARInvoiceLine [{aRInvoiceLine.arlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARInvoiceLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteARInvoiceLine(Guid aRInvoiceLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARInvoiceLineRepository iERPARInvoiceLineRepository = (base.ERPARInvoiceLineRepository = new ERPARInvoiceLineRepository(base.ApiClientContext));
		using (iERPARInvoiceLineRepository)
		{
			if (!(await base.ERPARInvoiceLineRepository.DoesARInvoiceLineExist(aRInvoiceLineId)))
			{
				base.ErrorsList.Add($"ARInvoiceLine [{aRInvoiceLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPARInvoiceLineInformationDto eRPARInvoiceLineInformationDto = await base.ERPARInvoiceLineRepository.GetARInvoiceLine(aRInvoiceLineId);
				string text = await base.ERPARInvoiceLineRepository.WhereUsed("ARInvoiceLines", new object[2] { eRPARInvoiceLineInformationDto.arlArInvoiceID, eRPARInvoiceLineInformationDto.arlArInvoiceLineID }, new object[2] { "arlArInvoiceID", "arlArInvoiceLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ARInvoiceLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPARInvoiceLineDto>> Process_DeleteARInvoiceLine(Guid aRInvoiceLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPARInvoiceLineDto> result;
		try
		{
			IERPARInvoiceLineRepository iERPARInvoiceLineRepository = (base.ERPARInvoiceLineRepository = new ERPARInvoiceLineRepository(base.ApiClientContext));
			using (iERPARInvoiceLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPARInvoiceLineRepository.DeleteRowFromTable("ARInvoiceLines", "arl", aRInvoiceLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ARInvoiceLine [{aRInvoiceLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARInvoiceLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPARInvoiceLineDto()
			};
		}
		return result;
	}
}
