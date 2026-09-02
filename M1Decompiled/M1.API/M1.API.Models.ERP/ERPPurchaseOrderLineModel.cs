using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPurchaseOrderLineModel : ERPBaseModel, IERPPurchaseOrderLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPurchaseOrderLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPurchaseOrderLineRepository iERPPurchaseOrderLineRepository = (base.ERPPurchaseOrderLineRepository = new ERPPurchaseOrderLineRepository(base.ApiClientContext));
		using (iERPPurchaseOrderLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPurchaseOrderLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPurchaseOrderLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPurchaseOrderLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPurchaseOrderLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPurchaseOrderLine(Guid purchaseOrderLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderLineRepository iERPPurchaseOrderLineRepository = (base.ERPPurchaseOrderLineRepository = new ERPPurchaseOrderLineRepository(base.ApiClientContext));
		using (iERPPurchaseOrderLineRepository)
		{
			if (!(await base.ERPPurchaseOrderLineRepository.DoesPurchaseOrderLineExist(purchaseOrderLineId)))
			{
				errorsList.Add($"PurchaseOrderLine [{purchaseOrderLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPurchaseOrderLine(ERPPurchaseOrderLineDto purchaseOrderLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderLineRepository iERPPurchaseOrderLineRepository = (base.ERPPurchaseOrderLineRepository = new ERPPurchaseOrderLineRepository(base.ApiClientContext));
		using (iERPPurchaseOrderLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlPurchaseOrderID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { purchaseOrderLine.pmlPurchaseOrderID })))
			{
				errorsList.Add("pmlPurchaseOrderID [" + purchaseOrderLine.pmlPurchaseOrderID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlJobID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { purchaseOrderLine.pmlJobID })))
			{
				errorsList.Add("pmlJobID [" + purchaseOrderLine.pmlJobID + "] not found.");
			}
			if (purchaseOrderLine.pmlJobAssemblyID > 0 && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { purchaseOrderLine.pmlJobID, purchaseOrderLine.pmlJobAssemblyID })))
			{
				errorsList.Add($"pmlJobAssemblyID [{purchaseOrderLine.pmlJobAssemblyID}] not found.");
			}
			if (purchaseOrderLine.pmlJobMaterialID > 0 && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { purchaseOrderLine.pmlJobID, purchaseOrderLine.pmlJobAssemblyID, purchaseOrderLine.pmlJobMaterialID })))
			{
				errorsList.Add($"pmlJobMaterialID [{purchaseOrderLine.pmlJobMaterialID}] not found.");
			}
			if (purchaseOrderLine.pmlJobOperationID > 0 && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { purchaseOrderLine.pmlJobID, purchaseOrderLine.pmlJobAssemblyID, purchaseOrderLine.pmlJobOperationID })))
			{
				errorsList.Add($"pmlJobOperationID [{purchaseOrderLine.pmlJobOperationID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlPartID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { purchaseOrderLine.pmlPartID })))
			{
				errorsList.Add("pmlPartID [" + purchaseOrderLine.pmlPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlPartRevisionID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { purchaseOrderLine.pmlPartID, purchaseOrderLine.pmlPartRevisionID })))
			{
				errorsList.Add("pmlPartRevisionID [" + purchaseOrderLine.pmlPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlPartWarehouseLocationID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { purchaseOrderLine.pmlPartID, purchaseOrderLine.pmlPartRevisionID, purchaseOrderLine.pmlPartWarehouseLocationID })))
			{
				errorsList.Add("pmlPartWarehouseLocationID [" + purchaseOrderLine.pmlPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlPartBinID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { purchaseOrderLine.pmlPartID, purchaseOrderLine.pmlPartRevisionID, purchaseOrderLine.pmlPartWarehouseLocationID, purchaseOrderLine.pmlPartBinID })))
			{
				errorsList.Add("pmlPartBinID [" + purchaseOrderLine.pmlPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlTaxCodeID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { purchaseOrderLine.pmlTaxCodeID })))
			{
				errorsList.Add("pmlTaxCodeID [" + purchaseOrderLine.pmlTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlNonTaxReasonID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { purchaseOrderLine.pmlNonTaxReasonID })))
			{
				errorsList.Add("pmlNonTaxReasonID [" + purchaseOrderLine.pmlNonTaxReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlSecondTaxCodeID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { purchaseOrderLine.pmlSecondTaxCodeID })))
			{
				errorsList.Add("pmlSecondTaxCodeID [" + purchaseOrderLine.pmlSecondTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlRfqID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("RFQs", new object[1] { "RQPRFQID" }, new object[1] { purchaseOrderLine.pmlRfqID })))
			{
				errorsList.Add("pmlRfqID [" + purchaseOrderLine.pmlRfqID + "] not found.");
			}
			if (purchaseOrderLine.pmlRfqLineID > 0 && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("RFQLines", new object[2] { "RQLRFQID", "RQLRFQLINEID" }, new object[2] { purchaseOrderLine.pmlRfqID, purchaseOrderLine.pmlRfqLineID })))
			{
				errorsList.Add($"pmlRfqLineID [{purchaseOrderLine.pmlRfqLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlRmaClaimID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("RMAClaims", new object[1] { "RAPRMACLAIMID" }, new object[1] { purchaseOrderLine.pmlRmaClaimID })))
			{
				errorsList.Add("pmlRmaClaimID [" + purchaseOrderLine.pmlRmaClaimID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlDmrClaimID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("DMRClaims", new object[1] { "DMPDMRCLAIMID" }, new object[1] { purchaseOrderLine.pmlDmrClaimID })))
			{
				errorsList.Add("pmlDmrClaimID [" + purchaseOrderLine.pmlDmrClaimID + "] not found.");
			}
			if (purchaseOrderLine.pmlRmaClaimLineID > 0 && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("RMAClaimLines", new object[2] { "RALRMACLAIMID", "RALRMACLAIMLINEID" }, new object[2] { purchaseOrderLine.pmlRmaClaimID, purchaseOrderLine.pmlRmaClaimLineID })))
			{
				errorsList.Add($"pmlRmaClaimLineID [{purchaseOrderLine.pmlRmaClaimLineID}] not found.");
			}
			if (purchaseOrderLine.pmlDmrClaimLineID > 0 && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("DMRClaimLines", new object[2] { "DMLDMRCLAIMID", "DMLDMRCLAIMLINEID" }, new object[2] { purchaseOrderLine.pmlDmrClaimID, purchaseOrderLine.pmlDmrClaimLineID })))
			{
				errorsList.Add($"pmlDmrClaimLineID [{purchaseOrderLine.pmlDmrClaimLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlProjectID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { purchaseOrderLine.pmlProjectID })))
			{
				errorsList.Add("pmlProjectID [" + purchaseOrderLine.pmlProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlProjectAreaID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { purchaseOrderLine.pmlProjectID, purchaseOrderLine.pmlProjectAreaID })))
			{
				errorsList.Add("pmlProjectAreaID [" + purchaseOrderLine.pmlProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlSalesOrderID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { purchaseOrderLine.pmlSalesOrderID })))
			{
				errorsList.Add("pmlSalesOrderID [" + purchaseOrderLine.pmlSalesOrderID + "] not found.");
			}
			if (purchaseOrderLine.pmlSalesOrderLineID > 0 && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("SalesOrderLines", new object[2] { "OMLSALESORDERID", "OMLSALESORDERLINEID" }, new object[2] { purchaseOrderLine.pmlSalesOrderID, purchaseOrderLine.pmlSalesOrderLineID })))
			{
				errorsList.Add($"pmlSalesOrderLineID [{purchaseOrderLine.pmlSalesOrderLineID}] not found.");
			}
			if (purchaseOrderLine.pmlSalesOrderDeliveryID > 0 && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("SalesOrderDeliveries", new object[3] { "OMDSALESORDERID", "OMDSALESORDERLINEID", "OMDSALESORDERDELIVERYID" }, new object[3] { purchaseOrderLine.pmlSalesOrderID, purchaseOrderLine.pmlSalesOrderLineID, purchaseOrderLine.pmlSalesOrderDeliveryID })))
			{
				errorsList.Add($"pmlSalesOrderDeliveryID [{purchaseOrderLine.pmlSalesOrderDeliveryID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlAssetTypeID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("AssetTypes", new object[1] { "FATASSETTYPEID" }, new object[1] { purchaseOrderLine.pmlAssetTypeID })))
			{
				errorsList.Add("pmlAssetTypeID [" + purchaseOrderLine.pmlAssetTypeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlAssetID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("Assets", new object[1] { "FAPASSETID" }, new object[1] { purchaseOrderLine.pmlAssetID })))
			{
				errorsList.Add("pmlAssetID [" + purchaseOrderLine.pmlAssetID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlSourcePurchaseOrderID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { purchaseOrderLine.pmlSourcePurchaseOrderID })))
			{
				errorsList.Add("pmlSourcePurchaseOrderID [" + purchaseOrderLine.pmlSourcePurchaseOrderID + "] not found.");
			}
			if (purchaseOrderLine.pmlSourcePurchaseOrderLineID > 0 && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("PurchaseOrderLines", new object[2] { "PMLPURCHASEORDERID", "PMLPURCHASEORDERLINEID" }, new object[2] { purchaseOrderLine.pmlSourcePurchaseOrderID, purchaseOrderLine.pmlSourcePurchaseOrderLineID })))
			{
				errorsList.Add($"pmlSourcePurchaseOrderLineID [{purchaseOrderLine.pmlSourcePurchaseOrderLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlLandedCostID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("LandedCosts", new object[1] { "RMCLANDEDCOSTID" }, new object[1] { purchaseOrderLine.pmlLandedCostID })))
			{
				errorsList.Add("pmlLandedCostID [" + purchaseOrderLine.pmlLandedCostID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlWorkCenterID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("WorkCenters", new object[1] { "XAWWORKCENTERID" }, new object[1] { purchaseOrderLine.pmlWorkCenterID })))
			{
				errorsList.Add("pmlWorkCenterID [" + purchaseOrderLine.pmlWorkCenterID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderLine.pmlProcessID) && !(await base.ERPPurchaseOrderLineRepository.DoesRecordExistInTableUsingKeys("Processes", new object[1] { "XACPROCESSID" }, new object[1] { purchaseOrderLine.pmlProcessID })))
			{
				errorsList.Add("pmlProcessID [" + purchaseOrderLine.pmlProcessID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPurchaseOrderLineDto>>> Process_GetAllPurchaseOrderLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPurchaseOrderLineDto> allPurchaseOrderLinesDto = new List<ERPPurchaseOrderLineDto>();
		ERPResponseMessageDto<IList<ERPPurchaseOrderLineDto>> result;
		try
		{
			IERPPurchaseOrderLineRepository iERPPurchaseOrderLineRepository = (base.ERPPurchaseOrderLineRepository = new ERPPurchaseOrderLineRepository(base.ApiClientContext));
			using (iERPPurchaseOrderLineRepository)
			{
				foreach (ERPPurchaseOrderLineInformationDto item2 in await base.ERPPurchaseOrderLineRepository.GetAllPurchaseOrderLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPPurchaseOrderLineDto item = new ERPPurchaseOrderLineDto
					{
						pmlAssetID = item2.pmlAssetID,
						pmlAssetTypeID = item2.pmlAssetTypeID,
						pmlConversionFactor = item2.pmlConversionFactor,
						pmlCreatedBy = item2.pmlCreatedBy,
						pmlCreatedDate = item2.pmlCreatedDate,
						pmlDmrClaimID = item2.pmlDmrClaimID,
						pmlDmrClaimLineID = item2.pmlDmrClaimLineID,
						pmlDocuments = item2.pmlDocuments,
						pmlDueDate = item2.pmlDueDate,
						pmlUniqueID = item2.pmlUniqueID,
						pmlExpenseSplitPercentTotal = item2.pmlExpenseSplitPercentTotal,
						pmlExtendedCostBase = item2.pmlExtendedCostBase,
						pmlExtendedCostForeign = item2.pmlExtendedCostForeign,
						pmlForm1099Box = item2.pmlForm1099Box,
						pmlInventoryQuantity = item2.pmlInventoryQuantity,
						pmlInventoryQuantityReceived = item2.pmlInventoryQuantityReceived,
						pmlInventoryUnitOfMeasure = item2.pmlInventoryUnitOfMeasure,
						pmlClosed = item2.pmlClosed,
						pmlCreateJobSeq = item2.pmlCreateJobSeq,
						pmlIntraCompanyPosted = item2.pmlIntraCompanyPosted,
						pmlInTransit = item2.pmlInTransit,
						pmlInTransitJournalsCreated = item2.pmlInTransitJournalsCreated,
						pmlInvoicedComplete = item2.pmlInvoicedComplete,
						pmlKitPart = item2.pmlKitPart,
						pmlPlanned = item2.pmlPlanned,
						pmlPriceOverride = item2.pmlPriceOverride,
						pmlReceivedComplete = item2.pmlReceivedComplete,
						pmlRequiresInspection = item2.pmlRequiresInspection,
						pmlSupplierRequirement = item2.pmlSupplierRequirement,
						pmlTaxable = item2.pmlTaxable,
						pmlItemType = item2.pmlItemType,
						pmlJobAssemblyID = item2.pmlJobAssemblyID,
						pmlJobID = item2.pmlJobID,
						pmlJobMaterialID = item2.pmlJobMaterialID,
						pmlJobOpenQuantity = item2.pmlJobOpenQuantity,
						pmlJobOperationID = item2.pmlJobOperationID,
						pmlJobType = item2.pmlJobType,
						pmlLandedCostID = item2.pmlLandedCostID,
						pmlLeadTime = item2.pmlLeadTime,
						pmlNonTaxReasonID = item2.pmlNonTaxReasonID,
						pmlOrgPartID = item2.pmlOrgPartID,
						pmlOrgPartShortDescription = item2.pmlOrgPartShortDescription,
						pmlPartBinID = item2.pmlPartBinID,
						pmlPartID = item2.pmlPartID,
						pmlPartLongDescriptionRtf = item2.pmlPartLongDescriptionRtf,
						pmlPartLongDescriptionText = item2.pmlPartLongDescriptionText,
						pmlPartRevisionID = item2.pmlPartRevisionID,
						pmlPartShortDescription = item2.pmlPartShortDescription,
						pmlPartWarehouseLocationID = item2.pmlPartWarehouseLocationID,
						pmlProcessID = item2.pmlProcessID,
						pmlProjectAreaID = item2.pmlProjectAreaID,
						pmlProjectID = item2.pmlProjectID,
						pmlPurchaseOrderID = item2.pmlPurchaseOrderID,
						pmlPurchaseQuantity = item2.pmlPurchaseQuantity,
						pmlPurchaseQuantityReceived = item2.pmlPurchaseQuantityReceived,
						pmlPurchaseType = item2.pmlPurchaseType,
						pmlPurchaseUnitCostBase = item2.pmlPurchaseUnitCostBase,
						pmlPurchaseUnitCostForeign = item2.pmlPurchaseUnitCostForeign,
						pmlPurchaseUnitOfMeasure = item2.pmlPurchaseUnitOfMeasure,
						pmlQuantityOnOrder = item2.pmlQuantityOnOrder,
						pmlRfqID = item2.pmlRfqID,
						pmlRfqLineID = item2.pmlRfqLineID,
						pmlRmaClaimID = item2.pmlRmaClaimID,
						pmlRmaClaimLineID = item2.pmlRmaClaimLineID,
						pmlRowVersion = item2.pmlRowVersion,
						pmlSalesOrderDeliveryID = item2.pmlSalesOrderDeliveryID,
						pmlSalesOrderID = item2.pmlSalesOrderID,
						pmlSalesOrderLineID = item2.pmlSalesOrderLineID,
						pmlSecondTaxAmountBase = item2.pmlSecondTaxAmountBase,
						pmlSecondTaxAmountForeign = item2.pmlSecondTaxAmountForeign,
						pmlSecondTaxCodeID = item2.pmlSecondTaxCodeID,
						pmlPurchaseOrderLineID = item2.pmlPurchaseOrderLineID,
						pmlSetupChargeBase = item2.pmlSetupChargeBase,
						pmlSetupChargeForeign = item2.pmlSetupChargeForeign,
						pmlSourcePurchaseOrderID = item2.pmlSourcePurchaseOrderID,
						pmlSourcePurchaseOrderLineID = item2.pmlSourcePurchaseOrderLineID,
						pmlSourceTableName = item2.pmlSourceTableName,
						pmlSourceTableUniqueID = item2.pmlSourceTableUniqueID,
						pmlTaxAmountBase = item2.pmlTaxAmountBase,
						pmlTaxAmountForeign = item2.pmlTaxAmountForeign,
						pmlTaxCodeID = item2.pmlTaxCodeID,
						pmlTotalComponentCosts = item2.pmlTotalComponentCosts,
						pmlTotalExtendedCostBase = item2.pmlTotalExtendedCostBase,
						pmlTotalExtendedCostForeign = item2.pmlTotalExtendedCostForeign,
						pmlTrackingNumber = item2.pmlTrackingNumber,
						pmlWorkCenterID = item2.pmlWorkCenterID,
						CustomFields = item2.CustomFields
					};
					allPurchaseOrderLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PurchaseOrderLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPurchaseOrderLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPurchaseOrderLinesDto,
				RecordCount = allPurchaseOrderLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderLineDto>> Process_GetPurchaseOrderLine(Guid purchaseOrderLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPurchaseOrderLineDto purchaseOrderLineDto = null;
		ERPResponseMessageDto<ERPPurchaseOrderLineDto> result;
		try
		{
			IERPPurchaseOrderLineRepository iERPPurchaseOrderLineRepository = (base.ERPPurchaseOrderLineRepository = new ERPPurchaseOrderLineRepository(base.ApiClientContext));
			using (iERPPurchaseOrderLineRepository)
			{
				ERPPurchaseOrderLineInformationDto eRPPurchaseOrderLineInformationDto = await base.ERPPurchaseOrderLineRepository.GetPurchaseOrderLine(purchaseOrderLineId);
				purchaseOrderLineDto = new ERPPurchaseOrderLineDto
				{
					pmlAssetID = eRPPurchaseOrderLineInformationDto.pmlAssetID,
					pmlAssetTypeID = eRPPurchaseOrderLineInformationDto.pmlAssetTypeID,
					pmlConversionFactor = eRPPurchaseOrderLineInformationDto.pmlConversionFactor,
					pmlCreatedBy = eRPPurchaseOrderLineInformationDto.pmlCreatedBy,
					pmlCreatedDate = eRPPurchaseOrderLineInformationDto.pmlCreatedDate,
					pmlDmrClaimID = eRPPurchaseOrderLineInformationDto.pmlDmrClaimID,
					pmlDmrClaimLineID = eRPPurchaseOrderLineInformationDto.pmlDmrClaimLineID,
					pmlDocuments = eRPPurchaseOrderLineInformationDto.pmlDocuments,
					pmlDueDate = eRPPurchaseOrderLineInformationDto.pmlDueDate,
					pmlUniqueID = eRPPurchaseOrderLineInformationDto.pmlUniqueID,
					pmlExpenseSplitPercentTotal = eRPPurchaseOrderLineInformationDto.pmlExpenseSplitPercentTotal,
					pmlExtendedCostBase = eRPPurchaseOrderLineInformationDto.pmlExtendedCostBase,
					pmlExtendedCostForeign = eRPPurchaseOrderLineInformationDto.pmlExtendedCostForeign,
					pmlForm1099Box = eRPPurchaseOrderLineInformationDto.pmlForm1099Box,
					pmlInventoryQuantity = eRPPurchaseOrderLineInformationDto.pmlInventoryQuantity,
					pmlInventoryQuantityReceived = eRPPurchaseOrderLineInformationDto.pmlInventoryQuantityReceived,
					pmlInventoryUnitOfMeasure = eRPPurchaseOrderLineInformationDto.pmlInventoryUnitOfMeasure,
					pmlClosed = eRPPurchaseOrderLineInformationDto.pmlClosed,
					pmlCreateJobSeq = eRPPurchaseOrderLineInformationDto.pmlCreateJobSeq,
					pmlIntraCompanyPosted = eRPPurchaseOrderLineInformationDto.pmlIntraCompanyPosted,
					pmlInTransit = eRPPurchaseOrderLineInformationDto.pmlInTransit,
					pmlInTransitJournalsCreated = eRPPurchaseOrderLineInformationDto.pmlInTransitJournalsCreated,
					pmlInvoicedComplete = eRPPurchaseOrderLineInformationDto.pmlInvoicedComplete,
					pmlKitPart = eRPPurchaseOrderLineInformationDto.pmlKitPart,
					pmlPlanned = eRPPurchaseOrderLineInformationDto.pmlPlanned,
					pmlPriceOverride = eRPPurchaseOrderLineInformationDto.pmlPriceOverride,
					pmlReceivedComplete = eRPPurchaseOrderLineInformationDto.pmlReceivedComplete,
					pmlRequiresInspection = eRPPurchaseOrderLineInformationDto.pmlRequiresInspection,
					pmlSupplierRequirement = eRPPurchaseOrderLineInformationDto.pmlSupplierRequirement,
					pmlTaxable = eRPPurchaseOrderLineInformationDto.pmlTaxable,
					pmlItemType = eRPPurchaseOrderLineInformationDto.pmlItemType,
					pmlJobAssemblyID = eRPPurchaseOrderLineInformationDto.pmlJobAssemblyID,
					pmlJobID = eRPPurchaseOrderLineInformationDto.pmlJobID,
					pmlJobMaterialID = eRPPurchaseOrderLineInformationDto.pmlJobMaterialID,
					pmlJobOpenQuantity = eRPPurchaseOrderLineInformationDto.pmlJobOpenQuantity,
					pmlJobOperationID = eRPPurchaseOrderLineInformationDto.pmlJobOperationID,
					pmlJobType = eRPPurchaseOrderLineInformationDto.pmlJobType,
					pmlLandedCostID = eRPPurchaseOrderLineInformationDto.pmlLandedCostID,
					pmlLeadTime = eRPPurchaseOrderLineInformationDto.pmlLeadTime,
					pmlNonTaxReasonID = eRPPurchaseOrderLineInformationDto.pmlNonTaxReasonID,
					pmlOrgPartID = eRPPurchaseOrderLineInformationDto.pmlOrgPartID,
					pmlOrgPartShortDescription = eRPPurchaseOrderLineInformationDto.pmlOrgPartShortDescription,
					pmlPartBinID = eRPPurchaseOrderLineInformationDto.pmlPartBinID,
					pmlPartID = eRPPurchaseOrderLineInformationDto.pmlPartID,
					pmlPartLongDescriptionRtf = eRPPurchaseOrderLineInformationDto.pmlPartLongDescriptionRtf,
					pmlPartLongDescriptionText = eRPPurchaseOrderLineInformationDto.pmlPartLongDescriptionText,
					pmlPartRevisionID = eRPPurchaseOrderLineInformationDto.pmlPartRevisionID,
					pmlPartShortDescription = eRPPurchaseOrderLineInformationDto.pmlPartShortDescription,
					pmlPartWarehouseLocationID = eRPPurchaseOrderLineInformationDto.pmlPartWarehouseLocationID,
					pmlProcessID = eRPPurchaseOrderLineInformationDto.pmlProcessID,
					pmlProjectAreaID = eRPPurchaseOrderLineInformationDto.pmlProjectAreaID,
					pmlProjectID = eRPPurchaseOrderLineInformationDto.pmlProjectID,
					pmlPurchaseOrderID = eRPPurchaseOrderLineInformationDto.pmlPurchaseOrderID,
					pmlPurchaseQuantity = eRPPurchaseOrderLineInformationDto.pmlPurchaseQuantity,
					pmlPurchaseQuantityReceived = eRPPurchaseOrderLineInformationDto.pmlPurchaseQuantityReceived,
					pmlPurchaseType = eRPPurchaseOrderLineInformationDto.pmlPurchaseType,
					pmlPurchaseUnitCostBase = eRPPurchaseOrderLineInformationDto.pmlPurchaseUnitCostBase,
					pmlPurchaseUnitCostForeign = eRPPurchaseOrderLineInformationDto.pmlPurchaseUnitCostForeign,
					pmlPurchaseUnitOfMeasure = eRPPurchaseOrderLineInformationDto.pmlPurchaseUnitOfMeasure,
					pmlQuantityOnOrder = eRPPurchaseOrderLineInformationDto.pmlQuantityOnOrder,
					pmlRfqID = eRPPurchaseOrderLineInformationDto.pmlRfqID,
					pmlRfqLineID = eRPPurchaseOrderLineInformationDto.pmlRfqLineID,
					pmlRmaClaimID = eRPPurchaseOrderLineInformationDto.pmlRmaClaimID,
					pmlRmaClaimLineID = eRPPurchaseOrderLineInformationDto.pmlRmaClaimLineID,
					pmlRowVersion = eRPPurchaseOrderLineInformationDto.pmlRowVersion,
					pmlSalesOrderDeliveryID = eRPPurchaseOrderLineInformationDto.pmlSalesOrderDeliveryID,
					pmlSalesOrderID = eRPPurchaseOrderLineInformationDto.pmlSalesOrderID,
					pmlSalesOrderLineID = eRPPurchaseOrderLineInformationDto.pmlSalesOrderLineID,
					pmlSecondTaxAmountBase = eRPPurchaseOrderLineInformationDto.pmlSecondTaxAmountBase,
					pmlSecondTaxAmountForeign = eRPPurchaseOrderLineInformationDto.pmlSecondTaxAmountForeign,
					pmlSecondTaxCodeID = eRPPurchaseOrderLineInformationDto.pmlSecondTaxCodeID,
					pmlPurchaseOrderLineID = eRPPurchaseOrderLineInformationDto.pmlPurchaseOrderLineID,
					pmlSetupChargeBase = eRPPurchaseOrderLineInformationDto.pmlSetupChargeBase,
					pmlSetupChargeForeign = eRPPurchaseOrderLineInformationDto.pmlSetupChargeForeign,
					pmlSourcePurchaseOrderID = eRPPurchaseOrderLineInformationDto.pmlSourcePurchaseOrderID,
					pmlSourcePurchaseOrderLineID = eRPPurchaseOrderLineInformationDto.pmlSourcePurchaseOrderLineID,
					pmlSourceTableName = eRPPurchaseOrderLineInformationDto.pmlSourceTableName,
					pmlSourceTableUniqueID = eRPPurchaseOrderLineInformationDto.pmlSourceTableUniqueID,
					pmlTaxAmountBase = eRPPurchaseOrderLineInformationDto.pmlTaxAmountBase,
					pmlTaxAmountForeign = eRPPurchaseOrderLineInformationDto.pmlTaxAmountForeign,
					pmlTaxCodeID = eRPPurchaseOrderLineInformationDto.pmlTaxCodeID,
					pmlTotalComponentCosts = eRPPurchaseOrderLineInformationDto.pmlTotalComponentCosts,
					pmlTotalExtendedCostBase = eRPPurchaseOrderLineInformationDto.pmlTotalExtendedCostBase,
					pmlTotalExtendedCostForeign = eRPPurchaseOrderLineInformationDto.pmlTotalExtendedCostForeign,
					pmlTrackingNumber = eRPPurchaseOrderLineInformationDto.pmlTrackingNumber,
					pmlWorkCenterID = eRPPurchaseOrderLineInformationDto.pmlWorkCenterID,
					CustomFields = eRPPurchaseOrderLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PurchaseOrderLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = purchaseOrderLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderLineDto>> Process_PutPurchaseOrderLine(ERPPurchaseOrderLineDto purchaseOrderLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPurchaseOrderLineDto createdObject = null;
		ERPResponseMessageDto<ERPPurchaseOrderLineDto> result;
		try
		{
			IERPPurchaseOrderLineRepository iERPPurchaseOrderLineRepository = (base.ERPPurchaseOrderLineRepository = new ERPPurchaseOrderLineRepository(base.ApiClientContext));
			using (iERPPurchaseOrderLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPurchaseOrderLineRepository.SavePurchaseOrderLine(purchaseOrderLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPurchaseOrderLineInformationDto eRPPurchaseOrderLineInformationDto = await base.ERPPurchaseOrderLineRepository.GetPurchaseOrderLine(purchaseOrderLine.pmlUniqueID);
					createdObject = new ERPPurchaseOrderLineDto
					{
						pmlAssetID = eRPPurchaseOrderLineInformationDto.pmlAssetID,
						pmlAssetTypeID = eRPPurchaseOrderLineInformationDto.pmlAssetTypeID,
						pmlConversionFactor = eRPPurchaseOrderLineInformationDto.pmlConversionFactor,
						pmlCreatedBy = eRPPurchaseOrderLineInformationDto.pmlCreatedBy,
						pmlCreatedDate = eRPPurchaseOrderLineInformationDto.pmlCreatedDate,
						pmlDmrClaimID = eRPPurchaseOrderLineInformationDto.pmlDmrClaimID,
						pmlDmrClaimLineID = eRPPurchaseOrderLineInformationDto.pmlDmrClaimLineID,
						pmlDocuments = eRPPurchaseOrderLineInformationDto.pmlDocuments,
						pmlDueDate = eRPPurchaseOrderLineInformationDto.pmlDueDate,
						pmlUniqueID = eRPPurchaseOrderLineInformationDto.pmlUniqueID,
						pmlExpenseSplitPercentTotal = eRPPurchaseOrderLineInformationDto.pmlExpenseSplitPercentTotal,
						pmlExtendedCostBase = eRPPurchaseOrderLineInformationDto.pmlExtendedCostBase,
						pmlExtendedCostForeign = eRPPurchaseOrderLineInformationDto.pmlExtendedCostForeign,
						pmlForm1099Box = eRPPurchaseOrderLineInformationDto.pmlForm1099Box,
						pmlInventoryQuantity = eRPPurchaseOrderLineInformationDto.pmlInventoryQuantity,
						pmlInventoryQuantityReceived = eRPPurchaseOrderLineInformationDto.pmlInventoryQuantityReceived,
						pmlInventoryUnitOfMeasure = eRPPurchaseOrderLineInformationDto.pmlInventoryUnitOfMeasure,
						pmlClosed = eRPPurchaseOrderLineInformationDto.pmlClosed,
						pmlCreateJobSeq = eRPPurchaseOrderLineInformationDto.pmlCreateJobSeq,
						pmlIntraCompanyPosted = eRPPurchaseOrderLineInformationDto.pmlIntraCompanyPosted,
						pmlInTransit = eRPPurchaseOrderLineInformationDto.pmlInTransit,
						pmlInTransitJournalsCreated = eRPPurchaseOrderLineInformationDto.pmlInTransitJournalsCreated,
						pmlInvoicedComplete = eRPPurchaseOrderLineInformationDto.pmlInvoicedComplete,
						pmlKitPart = eRPPurchaseOrderLineInformationDto.pmlKitPart,
						pmlPlanned = eRPPurchaseOrderLineInformationDto.pmlPlanned,
						pmlPriceOverride = eRPPurchaseOrderLineInformationDto.pmlPriceOverride,
						pmlReceivedComplete = eRPPurchaseOrderLineInformationDto.pmlReceivedComplete,
						pmlRequiresInspection = eRPPurchaseOrderLineInformationDto.pmlRequiresInspection,
						pmlSupplierRequirement = eRPPurchaseOrderLineInformationDto.pmlSupplierRequirement,
						pmlTaxable = eRPPurchaseOrderLineInformationDto.pmlTaxable,
						pmlItemType = eRPPurchaseOrderLineInformationDto.pmlItemType,
						pmlJobAssemblyID = eRPPurchaseOrderLineInformationDto.pmlJobAssemblyID,
						pmlJobID = eRPPurchaseOrderLineInformationDto.pmlJobID,
						pmlJobMaterialID = eRPPurchaseOrderLineInformationDto.pmlJobMaterialID,
						pmlJobOpenQuantity = eRPPurchaseOrderLineInformationDto.pmlJobOpenQuantity,
						pmlJobOperationID = eRPPurchaseOrderLineInformationDto.pmlJobOperationID,
						pmlJobType = eRPPurchaseOrderLineInformationDto.pmlJobType,
						pmlLandedCostID = eRPPurchaseOrderLineInformationDto.pmlLandedCostID,
						pmlLeadTime = eRPPurchaseOrderLineInformationDto.pmlLeadTime,
						pmlNonTaxReasonID = eRPPurchaseOrderLineInformationDto.pmlNonTaxReasonID,
						pmlOrgPartID = eRPPurchaseOrderLineInformationDto.pmlOrgPartID,
						pmlOrgPartShortDescription = eRPPurchaseOrderLineInformationDto.pmlOrgPartShortDescription,
						pmlPartBinID = eRPPurchaseOrderLineInformationDto.pmlPartBinID,
						pmlPartID = eRPPurchaseOrderLineInformationDto.pmlPartID,
						pmlPartLongDescriptionRtf = eRPPurchaseOrderLineInformationDto.pmlPartLongDescriptionRtf,
						pmlPartLongDescriptionText = eRPPurchaseOrderLineInformationDto.pmlPartLongDescriptionText,
						pmlPartRevisionID = eRPPurchaseOrderLineInformationDto.pmlPartRevisionID,
						pmlPartShortDescription = eRPPurchaseOrderLineInformationDto.pmlPartShortDescription,
						pmlPartWarehouseLocationID = eRPPurchaseOrderLineInformationDto.pmlPartWarehouseLocationID,
						pmlProcessID = eRPPurchaseOrderLineInformationDto.pmlProcessID,
						pmlProjectAreaID = eRPPurchaseOrderLineInformationDto.pmlProjectAreaID,
						pmlProjectID = eRPPurchaseOrderLineInformationDto.pmlProjectID,
						pmlPurchaseOrderID = eRPPurchaseOrderLineInformationDto.pmlPurchaseOrderID,
						pmlPurchaseQuantity = eRPPurchaseOrderLineInformationDto.pmlPurchaseQuantity,
						pmlPurchaseQuantityReceived = eRPPurchaseOrderLineInformationDto.pmlPurchaseQuantityReceived,
						pmlPurchaseType = eRPPurchaseOrderLineInformationDto.pmlPurchaseType,
						pmlPurchaseUnitCostBase = eRPPurchaseOrderLineInformationDto.pmlPurchaseUnitCostBase,
						pmlPurchaseUnitCostForeign = eRPPurchaseOrderLineInformationDto.pmlPurchaseUnitCostForeign,
						pmlPurchaseUnitOfMeasure = eRPPurchaseOrderLineInformationDto.pmlPurchaseUnitOfMeasure,
						pmlQuantityOnOrder = eRPPurchaseOrderLineInformationDto.pmlQuantityOnOrder,
						pmlRfqID = eRPPurchaseOrderLineInformationDto.pmlRfqID,
						pmlRfqLineID = eRPPurchaseOrderLineInformationDto.pmlRfqLineID,
						pmlRmaClaimID = eRPPurchaseOrderLineInformationDto.pmlRmaClaimID,
						pmlRmaClaimLineID = eRPPurchaseOrderLineInformationDto.pmlRmaClaimLineID,
						pmlRowVersion = eRPPurchaseOrderLineInformationDto.pmlRowVersion,
						pmlSalesOrderDeliveryID = eRPPurchaseOrderLineInformationDto.pmlSalesOrderDeliveryID,
						pmlSalesOrderID = eRPPurchaseOrderLineInformationDto.pmlSalesOrderID,
						pmlSalesOrderLineID = eRPPurchaseOrderLineInformationDto.pmlSalesOrderLineID,
						pmlSecondTaxAmountBase = eRPPurchaseOrderLineInformationDto.pmlSecondTaxAmountBase,
						pmlSecondTaxAmountForeign = eRPPurchaseOrderLineInformationDto.pmlSecondTaxAmountForeign,
						pmlSecondTaxCodeID = eRPPurchaseOrderLineInformationDto.pmlSecondTaxCodeID,
						pmlPurchaseOrderLineID = eRPPurchaseOrderLineInformationDto.pmlPurchaseOrderLineID,
						pmlSetupChargeBase = eRPPurchaseOrderLineInformationDto.pmlSetupChargeBase,
						pmlSetupChargeForeign = eRPPurchaseOrderLineInformationDto.pmlSetupChargeForeign,
						pmlSourcePurchaseOrderID = eRPPurchaseOrderLineInformationDto.pmlSourcePurchaseOrderID,
						pmlSourcePurchaseOrderLineID = eRPPurchaseOrderLineInformationDto.pmlSourcePurchaseOrderLineID,
						pmlSourceTableName = eRPPurchaseOrderLineInformationDto.pmlSourceTableName,
						pmlSourceTableUniqueID = eRPPurchaseOrderLineInformationDto.pmlSourceTableUniqueID,
						pmlTaxAmountBase = eRPPurchaseOrderLineInformationDto.pmlTaxAmountBase,
						pmlTaxAmountForeign = eRPPurchaseOrderLineInformationDto.pmlTaxAmountForeign,
						pmlTaxCodeID = eRPPurchaseOrderLineInformationDto.pmlTaxCodeID,
						pmlTotalComponentCosts = eRPPurchaseOrderLineInformationDto.pmlTotalComponentCosts,
						pmlTotalExtendedCostBase = eRPPurchaseOrderLineInformationDto.pmlTotalExtendedCostBase,
						pmlTotalExtendedCostForeign = eRPPurchaseOrderLineInformationDto.pmlTotalExtendedCostForeign,
						pmlTrackingNumber = eRPPurchaseOrderLineInformationDto.pmlTrackingNumber,
						pmlWorkCenterID = eRPPurchaseOrderLineInformationDto.pmlWorkCenterID,
						CustomFields = eRPPurchaseOrderLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PurchaseOrderLine [{purchaseOrderLine.pmlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePurchaseOrderLine(Guid purchaseOrderLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderLineRepository iERPPurchaseOrderLineRepository = (base.ERPPurchaseOrderLineRepository = new ERPPurchaseOrderLineRepository(base.ApiClientContext));
		using (iERPPurchaseOrderLineRepository)
		{
			if (!(await base.ERPPurchaseOrderLineRepository.DoesPurchaseOrderLineExist(purchaseOrderLineId)))
			{
				base.ErrorsList.Add($"PurchaseOrderLine [{purchaseOrderLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPurchaseOrderLineInformationDto eRPPurchaseOrderLineInformationDto = await base.ERPPurchaseOrderLineRepository.GetPurchaseOrderLine(purchaseOrderLineId);
				string text = await base.ERPPurchaseOrderLineRepository.WhereUsed("PurchaseOrderLines", new object[2] { eRPPurchaseOrderLineInformationDto.pmlPurchaseOrderID, eRPPurchaseOrderLineInformationDto.pmlPurchaseOrderLineID }, new object[2] { "pmlPurchaseOrderID", "pmlPurchaseOrderLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PurchaseOrderLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderLineDto>> Process_DeletePurchaseOrderLine(Guid purchaseOrderLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPurchaseOrderLineDto> result;
		try
		{
			IERPPurchaseOrderLineRepository iERPPurchaseOrderLineRepository = (base.ERPPurchaseOrderLineRepository = new ERPPurchaseOrderLineRepository(base.ApiClientContext));
			using (iERPPurchaseOrderLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPurchaseOrderLineRepository.DeleteRowFromTable("PurchaseOrderLines", "pml", purchaseOrderLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PurchaseOrderLine [{purchaseOrderLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPurchaseOrderLineDto()
			};
		}
		return result;
	}
}
