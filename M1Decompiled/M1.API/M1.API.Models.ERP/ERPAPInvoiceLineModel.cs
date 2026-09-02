using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAPInvoiceLineModel : ERPBaseModel, IERPAPInvoiceLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAPInvoiceLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAPInvoiceLineRepository iERPAPInvoiceLineRepository = (base.ERPAPInvoiceLineRepository = new ERPAPInvoiceLineRepository(base.ApiClientContext));
		using (iERPAPInvoiceLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAPInvoiceLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAPInvoiceLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAPInvoiceLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAPInvoiceLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAPInvoiceLine(Guid aPInvoiceLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPInvoiceLineRepository iERPAPInvoiceLineRepository = (base.ERPAPInvoiceLineRepository = new ERPAPInvoiceLineRepository(base.ApiClientContext));
		using (iERPAPInvoiceLineRepository)
		{
			if (!(await base.ERPAPInvoiceLineRepository.DoesAPInvoiceLineExist(aPInvoiceLineId)))
			{
				errorsList.Add($"APInvoiceLine [{aPInvoiceLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutAPInvoiceLine(ERPAPInvoiceLineDto aPInvoiceLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPInvoiceLineRepository iERPAPInvoiceLineRepository = (base.ERPAPInvoiceLineRepository = new ERPAPInvoiceLineRepository(base.ApiClientContext));
		using (iERPAPInvoiceLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplApInvoiceID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("APInvoices", new object[1] { "APPAPINVOICEID" }, new object[1] { aPInvoiceLine.aplApInvoiceID })))
			{
				errorsList.Add("aplApInvoiceID [" + aPInvoiceLine.aplApInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplPurchaseOrderID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { aPInvoiceLine.aplPurchaseOrderID })))
			{
				errorsList.Add("aplPurchaseOrderID [" + aPInvoiceLine.aplPurchaseOrderID + "] not found.");
			}
			if (aPInvoiceLine.aplPurchaseOrderLineID > 0 && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("PurchaseOrderLines", new object[2] { "PMLPURCHASEORDERID", "PMLPURCHASEORDERLINEID" }, new object[2] { aPInvoiceLine.aplPurchaseOrderID, aPInvoiceLine.aplPurchaseOrderLineID })))
			{
				errorsList.Add($"aplPurchaseOrderLineID [{aPInvoiceLine.aplPurchaseOrderLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplReceiptID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("Receipts", new object[1] { "RMPRECEIPTID" }, new object[1] { aPInvoiceLine.aplReceiptID })))
			{
				errorsList.Add("aplReceiptID [" + aPInvoiceLine.aplReceiptID + "] not found.");
			}
			if (aPInvoiceLine.aplReceiptLineID > 0 && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("ReceiptLines", new object[2] { "RMLRECEIPTID", "RMLRECEIPTLINEID" }, new object[2] { aPInvoiceLine.aplReceiptID, aPInvoiceLine.aplReceiptLineID })))
			{
				errorsList.Add($"aplReceiptLineID [{aPInvoiceLine.aplReceiptLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplRmaClaimID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("RMAClaims", new object[1] { "RAPRMACLAIMID" }, new object[1] { aPInvoiceLine.aplRmaClaimID })))
			{
				errorsList.Add("aplRmaClaimID [" + aPInvoiceLine.aplRmaClaimID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplDmrClaimID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("DMRClaims", new object[1] { "DMPDMRCLAIMID" }, new object[1] { aPInvoiceLine.aplDmrClaimID })))
			{
				errorsList.Add("aplDmrClaimID [" + aPInvoiceLine.aplDmrClaimID + "] not found.");
			}
			if (aPInvoiceLine.aplDmrClaimLineID > 0 && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("DMRClaimLines", new object[2] { "DMLDMRCLAIMID", "DMLDMRCLAIMLINEID" }, new object[2] { aPInvoiceLine.aplDmrClaimID, aPInvoiceLine.aplDmrClaimLineID })))
			{
				errorsList.Add($"aplDmrClaimLineID [{aPInvoiceLine.aplDmrClaimLineID}] not found.");
			}
			if (aPInvoiceLine.aplRmaClaimLineID > 0 && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("RMAClaimLines", new object[2] { "RALRMACLAIMID", "RALRMACLAIMLINEID" }, new object[2] { aPInvoiceLine.aplRmaClaimID, aPInvoiceLine.aplRmaClaimLineID })))
			{
				errorsList.Add($"aplRmaClaimLineID [{aPInvoiceLine.aplRmaClaimLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplJobID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { aPInvoiceLine.aplJobID })))
			{
				errorsList.Add("aplJobID [" + aPInvoiceLine.aplJobID + "] not found.");
			}
			if (aPInvoiceLine.aplJobAssemblyID > 0 && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { aPInvoiceLine.aplJobID, aPInvoiceLine.aplJobAssemblyID })))
			{
				errorsList.Add($"aplJobAssemblyID [{aPInvoiceLine.aplJobAssemblyID}] not found.");
			}
			if (aPInvoiceLine.aplJobMaterialID > 0 && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { aPInvoiceLine.aplJobID, aPInvoiceLine.aplJobAssemblyID, aPInvoiceLine.aplJobMaterialID })))
			{
				errorsList.Add($"aplJobMaterialID [{aPInvoiceLine.aplJobMaterialID}] not found.");
			}
			if (aPInvoiceLine.aplJobOperationID > 0 && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { aPInvoiceLine.aplJobID, aPInvoiceLine.aplJobAssemblyID, aPInvoiceLine.aplJobOperationID })))
			{
				errorsList.Add($"aplJobOperationID [{aPInvoiceLine.aplJobOperationID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplPartID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { aPInvoiceLine.aplPartID })))
			{
				errorsList.Add("aplPartID [" + aPInvoiceLine.aplPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplPartRevisionID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { aPInvoiceLine.aplPartID, aPInvoiceLine.aplPartRevisionID })))
			{
				errorsList.Add("aplPartRevisionID [" + aPInvoiceLine.aplPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplTaxCodeID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aPInvoiceLine.aplTaxCodeID })))
			{
				errorsList.Add("aplTaxCodeID [" + aPInvoiceLine.aplTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplNonTaxReasonID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { aPInvoiceLine.aplNonTaxReasonID })))
			{
				errorsList.Add("aplNonTaxReasonID [" + aPInvoiceLine.aplNonTaxReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplSecondTaxCodeID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aPInvoiceLine.aplSecondTaxCodeID })))
			{
				errorsList.Add("aplSecondTaxCodeID [" + aPInvoiceLine.aplSecondTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplDmrShipmentID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("DMRShipments", new object[1] { "DSPDMRSHIPMENTID" }, new object[1] { aPInvoiceLine.aplDmrShipmentID })))
			{
				errorsList.Add("aplDmrShipmentID [" + aPInvoiceLine.aplDmrShipmentID + "] not found.");
			}
			if (aPInvoiceLine.aplDmrShipmentLineID > 0 && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("DMRShipmentLines", new object[2] { "DSLDMRSHIPMENTID", "DSLDMRSHIPMENTLINEID" }, new object[2] { aPInvoiceLine.aplDmrShipmentID, aPInvoiceLine.aplDmrShipmentLineID })))
			{
				errorsList.Add($"aplDmrShipmentLineID [{aPInvoiceLine.aplDmrShipmentLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplAssetTypeID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("AssetTypes", new object[1] { "FATASSETTYPEID" }, new object[1] { aPInvoiceLine.aplAssetTypeID })))
			{
				errorsList.Add("aplAssetTypeID [" + aPInvoiceLine.aplAssetTypeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplAssetID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("Assets", new object[1] { "FAPASSETID" }, new object[1] { aPInvoiceLine.aplAssetID })))
			{
				errorsList.Add("aplAssetID [" + aPInvoiceLine.aplAssetID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplLandedCostID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("LandedCosts", new object[1] { "RMCLANDEDCOSTID" }, new object[1] { aPInvoiceLine.aplLandedCostID })))
			{
				errorsList.Add("aplLandedCostID [" + aPInvoiceLine.aplLandedCostID + "] not found.");
			}
			if (aPInvoiceLine.aplLandedCostChargeID > 0 && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("LandedCostCharges", new object[2] { "RMHLANDEDCOSTID", "RMHLANDEDCOSTCHARGEID" }, new object[2] { aPInvoiceLine.aplLandedCostID, aPInvoiceLine.aplLandedCostChargeID })))
			{
				errorsList.Add($"aplLandedCostChargeID [{aPInvoiceLine.aplLandedCostChargeID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplProjectID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { aPInvoiceLine.aplProjectID })))
			{
				errorsList.Add("aplProjectID [" + aPInvoiceLine.aplProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoiceLine.aplProjectAreaID) && !(await base.ERPAPInvoiceLineRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { aPInvoiceLine.aplProjectID, aPInvoiceLine.aplProjectAreaID })))
			{
				errorsList.Add("aplProjectAreaID [" + aPInvoiceLine.aplProjectAreaID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAPInvoiceLineDto>>> Process_GetAllAPInvoiceLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAPInvoiceLineDto> allAPInvoiceLinesDto = new List<ERPAPInvoiceLineDto>();
		ERPResponseMessageDto<IList<ERPAPInvoiceLineDto>> result;
		try
		{
			IERPAPInvoiceLineRepository iERPAPInvoiceLineRepository = (base.ERPAPInvoiceLineRepository = new ERPAPInvoiceLineRepository(base.ApiClientContext));
			using (iERPAPInvoiceLineRepository)
			{
				foreach (ERPAPInvoiceLineInformationDto item2 in await base.ERPAPInvoiceLineRepository.GetAllAPInvoiceLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPAPInvoiceLineDto item = new ERPAPInvoiceLineDto
					{
						aplApInvoiceID = item2.aplApInvoiceID,
						aplAssetID = item2.aplAssetID,
						aplAssetTypeID = item2.aplAssetTypeID,
						aplConversionFactor = item2.aplConversionFactor,
						aplCreatedBy = item2.aplCreatedBy,
						aplCreatedDate = item2.aplCreatedDate,
						aplDmrClaimID = item2.aplDmrClaimID,
						aplDmrClaimLineID = item2.aplDmrClaimLineID,
						aplDmrShipmentID = item2.aplDmrShipmentID,
						aplDmrShipmentLineID = item2.aplDmrShipmentLineID,
						aplUniqueID = item2.aplUniqueID,
						aplExtendedCostBase = item2.aplExtendedCostBase,
						aplExtendedCostForeign = item2.aplExtendedCostForeign,
						aplForm1099Box = item2.aplForm1099Box,
						aplInvoicedComplete = item2.aplInvoicedComplete,
						aplPostedToGl = item2.aplPostedToGl,
						aplRetention = item2.aplRetention,
						aplItemType = item2.aplItemType,
						aplJobAssemblyID = item2.aplJobAssemblyID,
						aplJobID = item2.aplJobID,
						aplJobMaterialID = item2.aplJobMaterialID,
						aplJobOperationID = item2.aplJobOperationID,
						aplJobType = item2.aplJobType,
						aplLandedCostChargeID = item2.aplLandedCostChargeID,
						aplLandedCostID = item2.aplLandedCostID,
						aplNonTaxReasonID = item2.aplNonTaxReasonID,
						aplOrgPartID = item2.aplOrgPartID,
						aplOrgPartShortDescription = item2.aplOrgPartShortDescription,
						aplPartDescription = item2.aplPartDescription,
						aplPartID = item2.aplPartID,
						aplPartLongDescriptionRtf = item2.aplPartLongDescriptionRtf,
						aplPartLongDescriptionText = item2.aplPartLongDescriptionText,
						aplPartRevisionID = item2.aplPartRevisionID,
						aplProjectAreaID = item2.aplProjectAreaID,
						aplProjectID = item2.aplProjectID,
						aplPurchaseOrderID = item2.aplPurchaseOrderID,
						aplPurchaseOrderLineID = item2.aplPurchaseOrderLineID,
						aplPurchaseQuantity = item2.aplPurchaseQuantity,
						aplPurchaseUnitCostBase = item2.aplPurchaseUnitCostBase,
						aplPurchaseUnitCostForeign = item2.aplPurchaseUnitCostForeign,
						aplPurchaseUnitOfMeasure = item2.aplPurchaseUnitOfMeasure,
						aplReceiptID = item2.aplReceiptID,
						aplReceiptLineID = item2.aplReceiptLineID,
						aplReceivedQuantity = item2.aplReceivedQuantity,
						aplReceivedUnitOfMeasure = item2.aplReceivedUnitOfMeasure,
						aplRetentionAmountBase = item2.aplRetentionAmountBase,
						aplRetentionAmountForeign = item2.aplRetentionAmountForeign,
						aplRetentionPercent = item2.aplRetentionPercent,
						aplRetentionReleaseDate = item2.aplRetentionReleaseDate,
						aplRmaClaimID = item2.aplRmaClaimID,
						aplRmaClaimLineID = item2.aplRmaClaimLineID,
						aplRowVersion = item2.aplRowVersion,
						aplSecondTaxAmountBase = item2.aplSecondTaxAmountBase,
						aplSecondTaxAmountForeign = item2.aplSecondTaxAmountForeign,
						aplSecondTaxCodeID = item2.aplSecondTaxCodeID,
						aplApInvoiceLineID = item2.aplApInvoiceLineID,
						aplSetupChargeBase = item2.aplSetupChargeBase,
						aplSetupChargeForeign = item2.aplSetupChargeForeign,
						aplTaxAmountBase = item2.aplTaxAmountBase,
						aplTaxAmountForeign = item2.aplTaxAmountForeign,
						aplTaxCodeID = item2.aplTaxCodeID,
						aplTotalExtendedCostBase = item2.aplTotalExtendedCostBase,
						aplTotalExtendedCostForeign = item2.aplTotalExtendedCostForeign,
						CustomFields = item2.CustomFields
					};
					allAPInvoiceLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all APInvoiceLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAPInvoiceLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAPInvoiceLinesDto,
				RecordCount = allAPInvoiceLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAPInvoiceLineDto>> Process_GetAPInvoiceLine(Guid aPInvoiceLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAPInvoiceLineDto aPInvoiceLineDto = null;
		ERPResponseMessageDto<ERPAPInvoiceLineDto> result;
		try
		{
			IERPAPInvoiceLineRepository iERPAPInvoiceLineRepository = (base.ERPAPInvoiceLineRepository = new ERPAPInvoiceLineRepository(base.ApiClientContext));
			using (iERPAPInvoiceLineRepository)
			{
				ERPAPInvoiceLineInformationDto eRPAPInvoiceLineInformationDto = await base.ERPAPInvoiceLineRepository.GetAPInvoiceLine(aPInvoiceLineId);
				aPInvoiceLineDto = new ERPAPInvoiceLineDto
				{
					aplApInvoiceID = eRPAPInvoiceLineInformationDto.aplApInvoiceID,
					aplAssetID = eRPAPInvoiceLineInformationDto.aplAssetID,
					aplAssetTypeID = eRPAPInvoiceLineInformationDto.aplAssetTypeID,
					aplConversionFactor = eRPAPInvoiceLineInformationDto.aplConversionFactor,
					aplCreatedBy = eRPAPInvoiceLineInformationDto.aplCreatedBy,
					aplCreatedDate = eRPAPInvoiceLineInformationDto.aplCreatedDate,
					aplDmrClaimID = eRPAPInvoiceLineInformationDto.aplDmrClaimID,
					aplDmrClaimLineID = eRPAPInvoiceLineInformationDto.aplDmrClaimLineID,
					aplDmrShipmentID = eRPAPInvoiceLineInformationDto.aplDmrShipmentID,
					aplDmrShipmentLineID = eRPAPInvoiceLineInformationDto.aplDmrShipmentLineID,
					aplUniqueID = eRPAPInvoiceLineInformationDto.aplUniqueID,
					aplExtendedCostBase = eRPAPInvoiceLineInformationDto.aplExtendedCostBase,
					aplExtendedCostForeign = eRPAPInvoiceLineInformationDto.aplExtendedCostForeign,
					aplForm1099Box = eRPAPInvoiceLineInformationDto.aplForm1099Box,
					aplInvoicedComplete = eRPAPInvoiceLineInformationDto.aplInvoicedComplete,
					aplPostedToGl = eRPAPInvoiceLineInformationDto.aplPostedToGl,
					aplRetention = eRPAPInvoiceLineInformationDto.aplRetention,
					aplItemType = eRPAPInvoiceLineInformationDto.aplItemType,
					aplJobAssemblyID = eRPAPInvoiceLineInformationDto.aplJobAssemblyID,
					aplJobID = eRPAPInvoiceLineInformationDto.aplJobID,
					aplJobMaterialID = eRPAPInvoiceLineInformationDto.aplJobMaterialID,
					aplJobOperationID = eRPAPInvoiceLineInformationDto.aplJobOperationID,
					aplJobType = eRPAPInvoiceLineInformationDto.aplJobType,
					aplLandedCostChargeID = eRPAPInvoiceLineInformationDto.aplLandedCostChargeID,
					aplLandedCostID = eRPAPInvoiceLineInformationDto.aplLandedCostID,
					aplNonTaxReasonID = eRPAPInvoiceLineInformationDto.aplNonTaxReasonID,
					aplOrgPartID = eRPAPInvoiceLineInformationDto.aplOrgPartID,
					aplOrgPartShortDescription = eRPAPInvoiceLineInformationDto.aplOrgPartShortDescription,
					aplPartDescription = eRPAPInvoiceLineInformationDto.aplPartDescription,
					aplPartID = eRPAPInvoiceLineInformationDto.aplPartID,
					aplPartLongDescriptionRtf = eRPAPInvoiceLineInformationDto.aplPartLongDescriptionRtf,
					aplPartLongDescriptionText = eRPAPInvoiceLineInformationDto.aplPartLongDescriptionText,
					aplPartRevisionID = eRPAPInvoiceLineInformationDto.aplPartRevisionID,
					aplProjectAreaID = eRPAPInvoiceLineInformationDto.aplProjectAreaID,
					aplProjectID = eRPAPInvoiceLineInformationDto.aplProjectID,
					aplPurchaseOrderID = eRPAPInvoiceLineInformationDto.aplPurchaseOrderID,
					aplPurchaseOrderLineID = eRPAPInvoiceLineInformationDto.aplPurchaseOrderLineID,
					aplPurchaseQuantity = eRPAPInvoiceLineInformationDto.aplPurchaseQuantity,
					aplPurchaseUnitCostBase = eRPAPInvoiceLineInformationDto.aplPurchaseUnitCostBase,
					aplPurchaseUnitCostForeign = eRPAPInvoiceLineInformationDto.aplPurchaseUnitCostForeign,
					aplPurchaseUnitOfMeasure = eRPAPInvoiceLineInformationDto.aplPurchaseUnitOfMeasure,
					aplReceiptID = eRPAPInvoiceLineInformationDto.aplReceiptID,
					aplReceiptLineID = eRPAPInvoiceLineInformationDto.aplReceiptLineID,
					aplReceivedQuantity = eRPAPInvoiceLineInformationDto.aplReceivedQuantity,
					aplReceivedUnitOfMeasure = eRPAPInvoiceLineInformationDto.aplReceivedUnitOfMeasure,
					aplRetentionAmountBase = eRPAPInvoiceLineInformationDto.aplRetentionAmountBase,
					aplRetentionAmountForeign = eRPAPInvoiceLineInformationDto.aplRetentionAmountForeign,
					aplRetentionPercent = eRPAPInvoiceLineInformationDto.aplRetentionPercent,
					aplRetentionReleaseDate = eRPAPInvoiceLineInformationDto.aplRetentionReleaseDate,
					aplRmaClaimID = eRPAPInvoiceLineInformationDto.aplRmaClaimID,
					aplRmaClaimLineID = eRPAPInvoiceLineInformationDto.aplRmaClaimLineID,
					aplRowVersion = eRPAPInvoiceLineInformationDto.aplRowVersion,
					aplSecondTaxAmountBase = eRPAPInvoiceLineInformationDto.aplSecondTaxAmountBase,
					aplSecondTaxAmountForeign = eRPAPInvoiceLineInformationDto.aplSecondTaxAmountForeign,
					aplSecondTaxCodeID = eRPAPInvoiceLineInformationDto.aplSecondTaxCodeID,
					aplApInvoiceLineID = eRPAPInvoiceLineInformationDto.aplApInvoiceLineID,
					aplSetupChargeBase = eRPAPInvoiceLineInformationDto.aplSetupChargeBase,
					aplSetupChargeForeign = eRPAPInvoiceLineInformationDto.aplSetupChargeForeign,
					aplTaxAmountBase = eRPAPInvoiceLineInformationDto.aplTaxAmountBase,
					aplTaxAmountForeign = eRPAPInvoiceLineInformationDto.aplTaxAmountForeign,
					aplTaxCodeID = eRPAPInvoiceLineInformationDto.aplTaxCodeID,
					aplTotalExtendedCostBase = eRPAPInvoiceLineInformationDto.aplTotalExtendedCostBase,
					aplTotalExtendedCostForeign = eRPAPInvoiceLineInformationDto.aplTotalExtendedCostForeign,
					CustomFields = eRPAPInvoiceLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the APInvoiceLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPInvoiceLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = aPInvoiceLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAPInvoiceLineDto>> Process_PutAPInvoiceLine(ERPAPInvoiceLineDto aPInvoiceLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPAPInvoiceLineDto createdObject = null;
		ERPResponseMessageDto<ERPAPInvoiceLineDto> result;
		try
		{
			IERPAPInvoiceLineRepository iERPAPInvoiceLineRepository = (base.ERPAPInvoiceLineRepository = new ERPAPInvoiceLineRepository(base.ApiClientContext));
			using (iERPAPInvoiceLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPAPInvoiceLineRepository.SaveAPInvoiceLine(aPInvoiceLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPAPInvoiceLineInformationDto eRPAPInvoiceLineInformationDto = await base.ERPAPInvoiceLineRepository.GetAPInvoiceLine(aPInvoiceLine.aplUniqueID);
					createdObject = new ERPAPInvoiceLineDto
					{
						aplApInvoiceID = eRPAPInvoiceLineInformationDto.aplApInvoiceID,
						aplAssetID = eRPAPInvoiceLineInformationDto.aplAssetID,
						aplAssetTypeID = eRPAPInvoiceLineInformationDto.aplAssetTypeID,
						aplConversionFactor = eRPAPInvoiceLineInformationDto.aplConversionFactor,
						aplCreatedBy = eRPAPInvoiceLineInformationDto.aplCreatedBy,
						aplCreatedDate = eRPAPInvoiceLineInformationDto.aplCreatedDate,
						aplDmrClaimID = eRPAPInvoiceLineInformationDto.aplDmrClaimID,
						aplDmrClaimLineID = eRPAPInvoiceLineInformationDto.aplDmrClaimLineID,
						aplDmrShipmentID = eRPAPInvoiceLineInformationDto.aplDmrShipmentID,
						aplDmrShipmentLineID = eRPAPInvoiceLineInformationDto.aplDmrShipmentLineID,
						aplUniqueID = eRPAPInvoiceLineInformationDto.aplUniqueID,
						aplExtendedCostBase = eRPAPInvoiceLineInformationDto.aplExtendedCostBase,
						aplExtendedCostForeign = eRPAPInvoiceLineInformationDto.aplExtendedCostForeign,
						aplForm1099Box = eRPAPInvoiceLineInformationDto.aplForm1099Box,
						aplInvoicedComplete = eRPAPInvoiceLineInformationDto.aplInvoicedComplete,
						aplPostedToGl = eRPAPInvoiceLineInformationDto.aplPostedToGl,
						aplRetention = eRPAPInvoiceLineInformationDto.aplRetention,
						aplItemType = eRPAPInvoiceLineInformationDto.aplItemType,
						aplJobAssemblyID = eRPAPInvoiceLineInformationDto.aplJobAssemblyID,
						aplJobID = eRPAPInvoiceLineInformationDto.aplJobID,
						aplJobMaterialID = eRPAPInvoiceLineInformationDto.aplJobMaterialID,
						aplJobOperationID = eRPAPInvoiceLineInformationDto.aplJobOperationID,
						aplJobType = eRPAPInvoiceLineInformationDto.aplJobType,
						aplLandedCostChargeID = eRPAPInvoiceLineInformationDto.aplLandedCostChargeID,
						aplLandedCostID = eRPAPInvoiceLineInformationDto.aplLandedCostID,
						aplNonTaxReasonID = eRPAPInvoiceLineInformationDto.aplNonTaxReasonID,
						aplOrgPartID = eRPAPInvoiceLineInformationDto.aplOrgPartID,
						aplOrgPartShortDescription = eRPAPInvoiceLineInformationDto.aplOrgPartShortDescription,
						aplPartDescription = eRPAPInvoiceLineInformationDto.aplPartDescription,
						aplPartID = eRPAPInvoiceLineInformationDto.aplPartID,
						aplPartLongDescriptionRtf = eRPAPInvoiceLineInformationDto.aplPartLongDescriptionRtf,
						aplPartLongDescriptionText = eRPAPInvoiceLineInformationDto.aplPartLongDescriptionText,
						aplPartRevisionID = eRPAPInvoiceLineInformationDto.aplPartRevisionID,
						aplProjectAreaID = eRPAPInvoiceLineInformationDto.aplProjectAreaID,
						aplProjectID = eRPAPInvoiceLineInformationDto.aplProjectID,
						aplPurchaseOrderID = eRPAPInvoiceLineInformationDto.aplPurchaseOrderID,
						aplPurchaseOrderLineID = eRPAPInvoiceLineInformationDto.aplPurchaseOrderLineID,
						aplPurchaseQuantity = eRPAPInvoiceLineInformationDto.aplPurchaseQuantity,
						aplPurchaseUnitCostBase = eRPAPInvoiceLineInformationDto.aplPurchaseUnitCostBase,
						aplPurchaseUnitCostForeign = eRPAPInvoiceLineInformationDto.aplPurchaseUnitCostForeign,
						aplPurchaseUnitOfMeasure = eRPAPInvoiceLineInformationDto.aplPurchaseUnitOfMeasure,
						aplReceiptID = eRPAPInvoiceLineInformationDto.aplReceiptID,
						aplReceiptLineID = eRPAPInvoiceLineInformationDto.aplReceiptLineID,
						aplReceivedQuantity = eRPAPInvoiceLineInformationDto.aplReceivedQuantity,
						aplReceivedUnitOfMeasure = eRPAPInvoiceLineInformationDto.aplReceivedUnitOfMeasure,
						aplRetentionAmountBase = eRPAPInvoiceLineInformationDto.aplRetentionAmountBase,
						aplRetentionAmountForeign = eRPAPInvoiceLineInformationDto.aplRetentionAmountForeign,
						aplRetentionPercent = eRPAPInvoiceLineInformationDto.aplRetentionPercent,
						aplRetentionReleaseDate = eRPAPInvoiceLineInformationDto.aplRetentionReleaseDate,
						aplRmaClaimID = eRPAPInvoiceLineInformationDto.aplRmaClaimID,
						aplRmaClaimLineID = eRPAPInvoiceLineInformationDto.aplRmaClaimLineID,
						aplRowVersion = eRPAPInvoiceLineInformationDto.aplRowVersion,
						aplSecondTaxAmountBase = eRPAPInvoiceLineInformationDto.aplSecondTaxAmountBase,
						aplSecondTaxAmountForeign = eRPAPInvoiceLineInformationDto.aplSecondTaxAmountForeign,
						aplSecondTaxCodeID = eRPAPInvoiceLineInformationDto.aplSecondTaxCodeID,
						aplApInvoiceLineID = eRPAPInvoiceLineInformationDto.aplApInvoiceLineID,
						aplSetupChargeBase = eRPAPInvoiceLineInformationDto.aplSetupChargeBase,
						aplSetupChargeForeign = eRPAPInvoiceLineInformationDto.aplSetupChargeForeign,
						aplTaxAmountBase = eRPAPInvoiceLineInformationDto.aplTaxAmountBase,
						aplTaxAmountForeign = eRPAPInvoiceLineInformationDto.aplTaxAmountForeign,
						aplTaxCodeID = eRPAPInvoiceLineInformationDto.aplTaxCodeID,
						aplTotalExtendedCostBase = eRPAPInvoiceLineInformationDto.aplTotalExtendedCostBase,
						aplTotalExtendedCostForeign = eRPAPInvoiceLineInformationDto.aplTotalExtendedCostForeign,
						CustomFields = eRPAPInvoiceLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing APInvoiceLine [{aPInvoiceLine.aplUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPInvoiceLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteAPInvoiceLine(Guid aPInvoiceLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPInvoiceLineRepository iERPAPInvoiceLineRepository = (base.ERPAPInvoiceLineRepository = new ERPAPInvoiceLineRepository(base.ApiClientContext));
		using (iERPAPInvoiceLineRepository)
		{
			if (!(await base.ERPAPInvoiceLineRepository.DoesAPInvoiceLineExist(aPInvoiceLineId)))
			{
				base.ErrorsList.Add($"APInvoiceLine [{aPInvoiceLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPAPInvoiceLineInformationDto eRPAPInvoiceLineInformationDto = await base.ERPAPInvoiceLineRepository.GetAPInvoiceLine(aPInvoiceLineId);
				string text = await base.ERPAPInvoiceLineRepository.WhereUsed("APInvoiceLines", new object[2] { eRPAPInvoiceLineInformationDto.aplApInvoiceID, eRPAPInvoiceLineInformationDto.aplApInvoiceLineID }, new object[2] { "aplApInvoiceID", "aplApInvoiceLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("APInvoiceLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPAPInvoiceLineDto>> Process_DeleteAPInvoiceLine(Guid aPInvoiceLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPAPInvoiceLineDto> result;
		try
		{
			IERPAPInvoiceLineRepository iERPAPInvoiceLineRepository = (base.ERPAPInvoiceLineRepository = new ERPAPInvoiceLineRepository(base.ApiClientContext));
			using (iERPAPInvoiceLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPAPInvoiceLineRepository.DeleteRowFromTable("APInvoiceLines", "apl", aPInvoiceLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of APInvoiceLine [{aPInvoiceLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPInvoiceLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPAPInvoiceLineDto()
			};
		}
		return result;
	}
}
