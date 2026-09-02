using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPReceiptLineModel : ERPBaseModel, IERPReceiptLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllReceiptLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPReceiptLineRepository iERPReceiptLineRepository = (base.ERPReceiptLineRepository = new ERPReceiptLineRepository(base.ApiClientContext));
		using (iERPReceiptLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPReceiptLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPReceiptLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPReceiptLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPReceiptLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetReceiptLine(Guid receiptLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPReceiptLineRepository iERPReceiptLineRepository = (base.ERPReceiptLineRepository = new ERPReceiptLineRepository(base.ApiClientContext));
		using (iERPReceiptLineRepository)
		{
			if (!(await base.ERPReceiptLineRepository.DoesReceiptLineExist(receiptLineId)))
			{
				errorsList.Add($"ReceiptLine [{receiptLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutReceiptLine(ERPReceiptLineDto receiptLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPReceiptLineRepository iERPReceiptLineRepository = (base.ERPReceiptLineRepository = new ERPReceiptLineRepository(base.ApiClientContext));
		using (iERPReceiptLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(receiptLine.rmlReceiptID) && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("Receipts", new object[1] { "RMPRECEIPTID" }, new object[1] { receiptLine.rmlReceiptID })))
			{
				errorsList.Add("rmlReceiptID [" + receiptLine.rmlReceiptID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptLine.rmlPurchaseOrderID) && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { receiptLine.rmlPurchaseOrderID })))
			{
				errorsList.Add("rmlPurchaseOrderID [" + receiptLine.rmlPurchaseOrderID + "] not found.");
			}
			if (receiptLine.rmlPurchaseOrderLineID > 0 && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("PurchaseOrderLines", new object[2] { "PMLPURCHASEORDERID", "PMLPURCHASEORDERLINEID" }, new object[2] { receiptLine.rmlPurchaseOrderID, receiptLine.rmlPurchaseOrderLineID })))
			{
				errorsList.Add($"rmlPurchaseOrderLineID [{receiptLine.rmlPurchaseOrderLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptLine.rmlRmaClaimID) && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("RMAClaims", new object[1] { "RAPRMACLAIMID" }, new object[1] { receiptLine.rmlRmaClaimID })))
			{
				errorsList.Add("rmlRmaClaimID [" + receiptLine.rmlRmaClaimID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptLine.rmlDmrClaimID) && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("DMRClaims", new object[1] { "DMPDMRCLAIMID" }, new object[1] { receiptLine.rmlDmrClaimID })))
			{
				errorsList.Add("rmlDmrClaimID [" + receiptLine.rmlDmrClaimID + "] not found.");
			}
			if (receiptLine.rmlDmrClaimLineID > 0 && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("DMRClaimLines", new object[2] { "DMLDMRCLAIMID", "DMLDMRCLAIMLINEID" }, new object[2] { receiptLine.rmlDmrClaimID, receiptLine.rmlDmrClaimLineID })))
			{
				errorsList.Add($"rmlDmrClaimLineID [{receiptLine.rmlDmrClaimLineID}] not found.");
			}
			if (receiptLine.rmlRmaClaimLineID > 0 && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("RMAClaimLines", new object[2] { "RALRMACLAIMID", "RALRMACLAIMLINEID" }, new object[2] { receiptLine.rmlRmaClaimID, receiptLine.rmlRmaClaimLineID })))
			{
				errorsList.Add($"rmlRmaClaimLineID [{receiptLine.rmlRmaClaimLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptLine.rmlJobID) && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { receiptLine.rmlJobID })))
			{
				errorsList.Add("rmlJobID [" + receiptLine.rmlJobID + "] not found.");
			}
			if (receiptLine.rmlJobAssemblyID > 0 && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { receiptLine.rmlJobID, receiptLine.rmlJobAssemblyID })))
			{
				errorsList.Add($"rmlJobAssemblyID [{receiptLine.rmlJobAssemblyID}] not found.");
			}
			if (receiptLine.rmlJobMaterialID > 0 && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { receiptLine.rmlJobID, receiptLine.rmlJobAssemblyID, receiptLine.rmlJobMaterialID })))
			{
				errorsList.Add($"rmlJobMaterialID [{receiptLine.rmlJobMaterialID}] not found.");
			}
			if (receiptLine.rmlJobOperationID > 0 && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { receiptLine.rmlJobID, receiptLine.rmlJobAssemblyID, receiptLine.rmlJobOperationID })))
			{
				errorsList.Add($"rmlJobOperationID [{receiptLine.rmlJobOperationID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptLine.rmlPartID) && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { receiptLine.rmlPartID })))
			{
				errorsList.Add("rmlPartID [" + receiptLine.rmlPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptLine.rmlPartRevisionID) && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { receiptLine.rmlPartID, receiptLine.rmlPartRevisionID })))
			{
				errorsList.Add("rmlPartRevisionID [" + receiptLine.rmlPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptLine.rmlPartWarehouseLocationID) && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { receiptLine.rmlPartID, receiptLine.rmlPartRevisionID, receiptLine.rmlPartWarehouseLocationID })))
			{
				errorsList.Add("rmlPartWarehouseLocationID [" + receiptLine.rmlPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptLine.rmlPartBinID) && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { receiptLine.rmlPartID, receiptLine.rmlPartRevisionID, receiptLine.rmlPartWarehouseLocationID, receiptLine.rmlPartBinID })))
			{
				errorsList.Add("rmlPartBinID [" + receiptLine.rmlPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptLine.rmlProjectID) && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { receiptLine.rmlProjectID })))
			{
				errorsList.Add("rmlProjectID [" + receiptLine.rmlProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptLine.rmlProjectAreaID) && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { receiptLine.rmlProjectID, receiptLine.rmlProjectAreaID })))
			{
				errorsList.Add("rmlProjectAreaID [" + receiptLine.rmlProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptLine.rmlSalesOrderID) && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { receiptLine.rmlSalesOrderID })))
			{
				errorsList.Add("rmlSalesOrderID [" + receiptLine.rmlSalesOrderID + "] not found.");
			}
			if (receiptLine.rmlSalesOrderLineID > 0 && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("SalesOrderLines", new object[2] { "OMLSALESORDERID", "OMLSALESORDERLINEID" }, new object[2] { receiptLine.rmlSalesOrderID, receiptLine.rmlSalesOrderLineID })))
			{
				errorsList.Add($"rmlSalesOrderLineID [{receiptLine.rmlSalesOrderLineID}] not found.");
			}
			if (receiptLine.rmlSalesOrderDeliveryID > 0 && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("SalesOrderDeliveries", new object[3] { "OMDSALESORDERID", "OMDSALESORDERLINEID", "OMDSALESORDERDELIVERYID" }, new object[3] { receiptLine.rmlSalesOrderID, receiptLine.rmlSalesOrderLineID, receiptLine.rmlSalesOrderDeliveryID })))
			{
				errorsList.Add($"rmlSalesOrderDeliveryID [{receiptLine.rmlSalesOrderDeliveryID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptLine.rmlReverseReceiptID) && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("Receipts", new object[1] { "RMPRECEIPTID" }, new object[1] { receiptLine.rmlReverseReceiptID })))
			{
				errorsList.Add("rmlReverseReceiptID [" + receiptLine.rmlReverseReceiptID + "] not found.");
			}
			if (receiptLine.rmlReverseReceiptLineID > 0 && !(await base.ERPReceiptLineRepository.DoesRecordExistInTableUsingKeys("ReceiptLines", new object[2] { "RMLRECEIPTID", "RMLRECEIPTLINEID" }, new object[2] { receiptLine.rmlReverseReceiptID, receiptLine.rmlReverseReceiptLineID })))
			{
				errorsList.Add($"rmlReverseReceiptLineID [{receiptLine.rmlReverseReceiptLineID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPReceiptLineDto>>> Process_GetAllReceiptLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPReceiptLineDto> allReceiptLinesDto = new List<ERPReceiptLineDto>();
		ERPResponseMessageDto<IList<ERPReceiptLineDto>> result;
		try
		{
			IERPReceiptLineRepository iERPReceiptLineRepository = (base.ERPReceiptLineRepository = new ERPReceiptLineRepository(base.ApiClientContext));
			using (iERPReceiptLineRepository)
			{
				foreach (ERPReceiptLineInformationDto item2 in await base.ERPReceiptLineRepository.GetAllReceiptLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPReceiptLineDto item = new ERPReceiptLineDto
					{
						rmlConversionFactor = item2.rmlConversionFactor,
						rmlCreatedBy = item2.rmlCreatedBy,
						rmlCreatedDate = item2.rmlCreatedDate,
						rmlDescription = item2.rmlDescription,
						rmlDmrClaimID = item2.rmlDmrClaimID,
						rmlDmrClaimLineID = item2.rmlDmrClaimLineID,
						rmlDutyUnitCost = item2.rmlDutyUnitCost,
						rmlUniqueID = item2.rmlUniqueID,
						rmlExtendedCostBase = item2.rmlExtendedCostBase,
						rmlExtendedCostForeign = item2.rmlExtendedCostForeign,
						rmlForm1099Box = item2.rmlForm1099Box,
						rmlFreightUnitCost = item2.rmlFreightUnitCost,
						rmlHeatLot = item2.rmlHeatLot,
						rmlInspectionNotesRTF = item2.rmlInspectionNotesRTF,
						rmlInspectionNotesText = item2.rmlInspectionNotesText,
						rmlInventoryQuantityReceived = item2.rmlInventoryQuantityReceived,
						rmlInventoryUnitCost = item2.rmlInventoryUnitCost,
						rmlInventoryUnitCostForeign = item2.rmlInventoryUnitCostForeign,
						rmlInventoryUnitOfMeasure = item2.rmlInventoryUnitOfMeasure,
						rmlClosed = item2.rmlClosed,
						rmlInInspection = item2.rmlInInspection,
						rmlInspectionComplete = item2.rmlInspectionComplete,
						rmlInvoicedComplete = item2.rmlInvoicedComplete,
						rmlJobReceivedComplete = item2.rmlJobReceivedComplete,
						rmlKitPart = item2.rmlKitPart,
						rmlPoReceivedComplete = item2.rmlPoReceivedComplete,
						rmlPostedToGl = item2.rmlPostedToGl,
						rmlRequiresInspection = item2.rmlRequiresInspection,
						rmlReversed = item2.rmlReversed,
						rmlTrackSerialNumbers = item2.rmlTrackSerialNumbers,
						rmlJobAssemblyID = item2.rmlJobAssemblyID,
						rmlJobEstimatedQuantity = item2.rmlJobEstimatedQuantity,
						rmlJobID = item2.rmlJobID,
						rmlJobMaterialID = item2.rmlJobMaterialID,
						rmlJobMatQuantityReceived = item2.rmlJobMatQuantityReceived,
						rmlJobOpenQuantity = item2.rmlJobOpenQuantity,
						rmlJobOperationID = item2.rmlJobOperationID,
						rmlJobOprQuantityReceived = item2.rmlJobOprQuantityReceived,
						rmlJobType = item2.rmlJobType,
						rmlMiscUnitCost = item2.rmlMiscUnitCost,
						rmlOrgPartID = item2.rmlOrgPartID,
						rmlOrgPartShortDescription = item2.rmlOrgPartShortDescription,
						rmlPartBinID = item2.rmlPartBinID,
						rmlPartID = item2.rmlPartID,
						rmlPartLongDescriptionRtf = item2.rmlPartLongDescriptionRtf,
						rmlPartLongDescriptionText = item2.rmlPartLongDescriptionText,
						rmlPartRevisionID = item2.rmlPartRevisionID,
						rmlPartWarehouseLocationID = item2.rmlPartWarehouseLocationID,
						rmlPoOpenQuantity = item2.rmlPoOpenQuantity,
						rmlPoPurchaseQuantity = item2.rmlPoPurchaseQuantity,
						rmlProjectAreaID = item2.rmlProjectAreaID,
						rmlProjectID = item2.rmlProjectID,
						rmlPurchaseOrderID = item2.rmlPurchaseOrderID,
						rmlPurchaseOrderLineID = item2.rmlPurchaseOrderLineID,
						rmlPurchaseQuantityReceived = item2.rmlPurchaseQuantityReceived,
						rmlPurchaseUnitCost = item2.rmlPurchaseUnitCost,
						rmlPurchaseUnitCostForeign = item2.rmlPurchaseUnitCostForeign,
						rmlPurchaseUnitOfMeasure = item2.rmlPurchaseUnitOfMeasure,
						rmlQuantityToInspect = item2.rmlQuantityToInspect,
						rmlReceiptID = item2.rmlReceiptID,
						rmlReference = item2.rmlReference,
						rmlReverseReceiptID = item2.rmlReverseReceiptID,
						rmlReverseReceiptLineID = item2.rmlReverseReceiptLineID,
						rmlRmaClaimID = item2.rmlRmaClaimID,
						rmlRmaClaimLineID = item2.rmlRmaClaimLineID,
						rmlRowVersion = item2.rmlRowVersion,
						rmlSalesOrderDeliveryID = item2.rmlSalesOrderDeliveryID,
						rmlSalesOrderID = item2.rmlSalesOrderID,
						rmlSalesOrderLineID = item2.rmlSalesOrderLineID,
						rmlReceiptLineID = item2.rmlReceiptLineID,
						rmlSetupCharge = item2.rmlSetupCharge,
						rmlSetupChargeForeign = item2.rmlSetupChargeForeign,
						rmlTotalComponentCosts = item2.rmlTotalComponentCosts,
						CustomFields = item2.CustomFields
					};
					allReceiptLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ReceiptLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPReceiptLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allReceiptLinesDto,
				RecordCount = allReceiptLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPReceiptLineDto>> Process_GetReceiptLine(Guid receiptLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPReceiptLineDto receiptLineDto = null;
		ERPResponseMessageDto<ERPReceiptLineDto> result;
		try
		{
			IERPReceiptLineRepository iERPReceiptLineRepository = (base.ERPReceiptLineRepository = new ERPReceiptLineRepository(base.ApiClientContext));
			using (iERPReceiptLineRepository)
			{
				ERPReceiptLineInformationDto eRPReceiptLineInformationDto = await base.ERPReceiptLineRepository.GetReceiptLine(receiptLineId);
				receiptLineDto = new ERPReceiptLineDto
				{
					rmlConversionFactor = eRPReceiptLineInformationDto.rmlConversionFactor,
					rmlCreatedBy = eRPReceiptLineInformationDto.rmlCreatedBy,
					rmlCreatedDate = eRPReceiptLineInformationDto.rmlCreatedDate,
					rmlDescription = eRPReceiptLineInformationDto.rmlDescription,
					rmlDmrClaimID = eRPReceiptLineInformationDto.rmlDmrClaimID,
					rmlDmrClaimLineID = eRPReceiptLineInformationDto.rmlDmrClaimLineID,
					rmlDutyUnitCost = eRPReceiptLineInformationDto.rmlDutyUnitCost,
					rmlUniqueID = eRPReceiptLineInformationDto.rmlUniqueID,
					rmlExtendedCostBase = eRPReceiptLineInformationDto.rmlExtendedCostBase,
					rmlExtendedCostForeign = eRPReceiptLineInformationDto.rmlExtendedCostForeign,
					rmlForm1099Box = eRPReceiptLineInformationDto.rmlForm1099Box,
					rmlFreightUnitCost = eRPReceiptLineInformationDto.rmlFreightUnitCost,
					rmlHeatLot = eRPReceiptLineInformationDto.rmlHeatLot,
					rmlInspectionNotesRTF = eRPReceiptLineInformationDto.rmlInspectionNotesRTF,
					rmlInspectionNotesText = eRPReceiptLineInformationDto.rmlInspectionNotesText,
					rmlInventoryQuantityReceived = eRPReceiptLineInformationDto.rmlInventoryQuantityReceived,
					rmlInventoryUnitCost = eRPReceiptLineInformationDto.rmlInventoryUnitCost,
					rmlInventoryUnitCostForeign = eRPReceiptLineInformationDto.rmlInventoryUnitCostForeign,
					rmlInventoryUnitOfMeasure = eRPReceiptLineInformationDto.rmlInventoryUnitOfMeasure,
					rmlClosed = eRPReceiptLineInformationDto.rmlClosed,
					rmlInInspection = eRPReceiptLineInformationDto.rmlInInspection,
					rmlInspectionComplete = eRPReceiptLineInformationDto.rmlInspectionComplete,
					rmlInvoicedComplete = eRPReceiptLineInformationDto.rmlInvoicedComplete,
					rmlJobReceivedComplete = eRPReceiptLineInformationDto.rmlJobReceivedComplete,
					rmlKitPart = eRPReceiptLineInformationDto.rmlKitPart,
					rmlPoReceivedComplete = eRPReceiptLineInformationDto.rmlPoReceivedComplete,
					rmlPostedToGl = eRPReceiptLineInformationDto.rmlPostedToGl,
					rmlRequiresInspection = eRPReceiptLineInformationDto.rmlRequiresInspection,
					rmlReversed = eRPReceiptLineInformationDto.rmlReversed,
					rmlTrackSerialNumbers = eRPReceiptLineInformationDto.rmlTrackSerialNumbers,
					rmlJobAssemblyID = eRPReceiptLineInformationDto.rmlJobAssemblyID,
					rmlJobEstimatedQuantity = eRPReceiptLineInformationDto.rmlJobEstimatedQuantity,
					rmlJobID = eRPReceiptLineInformationDto.rmlJobID,
					rmlJobMaterialID = eRPReceiptLineInformationDto.rmlJobMaterialID,
					rmlJobMatQuantityReceived = eRPReceiptLineInformationDto.rmlJobMatQuantityReceived,
					rmlJobOpenQuantity = eRPReceiptLineInformationDto.rmlJobOpenQuantity,
					rmlJobOperationID = eRPReceiptLineInformationDto.rmlJobOperationID,
					rmlJobOprQuantityReceived = eRPReceiptLineInformationDto.rmlJobOprQuantityReceived,
					rmlJobType = eRPReceiptLineInformationDto.rmlJobType,
					rmlMiscUnitCost = eRPReceiptLineInformationDto.rmlMiscUnitCost,
					rmlOrgPartID = eRPReceiptLineInformationDto.rmlOrgPartID,
					rmlOrgPartShortDescription = eRPReceiptLineInformationDto.rmlOrgPartShortDescription,
					rmlPartBinID = eRPReceiptLineInformationDto.rmlPartBinID,
					rmlPartID = eRPReceiptLineInformationDto.rmlPartID,
					rmlPartLongDescriptionRtf = eRPReceiptLineInformationDto.rmlPartLongDescriptionRtf,
					rmlPartLongDescriptionText = eRPReceiptLineInformationDto.rmlPartLongDescriptionText,
					rmlPartRevisionID = eRPReceiptLineInformationDto.rmlPartRevisionID,
					rmlPartWarehouseLocationID = eRPReceiptLineInformationDto.rmlPartWarehouseLocationID,
					rmlPoOpenQuantity = eRPReceiptLineInformationDto.rmlPoOpenQuantity,
					rmlPoPurchaseQuantity = eRPReceiptLineInformationDto.rmlPoPurchaseQuantity,
					rmlProjectAreaID = eRPReceiptLineInformationDto.rmlProjectAreaID,
					rmlProjectID = eRPReceiptLineInformationDto.rmlProjectID,
					rmlPurchaseOrderID = eRPReceiptLineInformationDto.rmlPurchaseOrderID,
					rmlPurchaseOrderLineID = eRPReceiptLineInformationDto.rmlPurchaseOrderLineID,
					rmlPurchaseQuantityReceived = eRPReceiptLineInformationDto.rmlPurchaseQuantityReceived,
					rmlPurchaseUnitCost = eRPReceiptLineInformationDto.rmlPurchaseUnitCost,
					rmlPurchaseUnitCostForeign = eRPReceiptLineInformationDto.rmlPurchaseUnitCostForeign,
					rmlPurchaseUnitOfMeasure = eRPReceiptLineInformationDto.rmlPurchaseUnitOfMeasure,
					rmlQuantityToInspect = eRPReceiptLineInformationDto.rmlQuantityToInspect,
					rmlReceiptID = eRPReceiptLineInformationDto.rmlReceiptID,
					rmlReference = eRPReceiptLineInformationDto.rmlReference,
					rmlReverseReceiptID = eRPReceiptLineInformationDto.rmlReverseReceiptID,
					rmlReverseReceiptLineID = eRPReceiptLineInformationDto.rmlReverseReceiptLineID,
					rmlRmaClaimID = eRPReceiptLineInformationDto.rmlRmaClaimID,
					rmlRmaClaimLineID = eRPReceiptLineInformationDto.rmlRmaClaimLineID,
					rmlRowVersion = eRPReceiptLineInformationDto.rmlRowVersion,
					rmlSalesOrderDeliveryID = eRPReceiptLineInformationDto.rmlSalesOrderDeliveryID,
					rmlSalesOrderID = eRPReceiptLineInformationDto.rmlSalesOrderID,
					rmlSalesOrderLineID = eRPReceiptLineInformationDto.rmlSalesOrderLineID,
					rmlReceiptLineID = eRPReceiptLineInformationDto.rmlReceiptLineID,
					rmlSetupCharge = eRPReceiptLineInformationDto.rmlSetupCharge,
					rmlSetupChargeForeign = eRPReceiptLineInformationDto.rmlSetupChargeForeign,
					rmlTotalComponentCosts = eRPReceiptLineInformationDto.rmlTotalComponentCosts,
					CustomFields = eRPReceiptLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ReceiptLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPReceiptLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = receiptLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPReceiptLineDto>> Process_PutReceiptLine(ERPReceiptLineDto receiptLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPReceiptLineDto createdObject = null;
		ERPResponseMessageDto<ERPReceiptLineDto> result;
		try
		{
			IERPReceiptLineRepository iERPReceiptLineRepository = (base.ERPReceiptLineRepository = new ERPReceiptLineRepository(base.ApiClientContext));
			using (iERPReceiptLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPReceiptLineRepository.SaveReceiptLine(receiptLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPReceiptLineInformationDto eRPReceiptLineInformationDto = await base.ERPReceiptLineRepository.GetReceiptLine(receiptLine.rmlUniqueID);
					createdObject = new ERPReceiptLineDto
					{
						rmlConversionFactor = eRPReceiptLineInformationDto.rmlConversionFactor,
						rmlCreatedBy = eRPReceiptLineInformationDto.rmlCreatedBy,
						rmlCreatedDate = eRPReceiptLineInformationDto.rmlCreatedDate,
						rmlDescription = eRPReceiptLineInformationDto.rmlDescription,
						rmlDmrClaimID = eRPReceiptLineInformationDto.rmlDmrClaimID,
						rmlDmrClaimLineID = eRPReceiptLineInformationDto.rmlDmrClaimLineID,
						rmlDutyUnitCost = eRPReceiptLineInformationDto.rmlDutyUnitCost,
						rmlUniqueID = eRPReceiptLineInformationDto.rmlUniqueID,
						rmlExtendedCostBase = eRPReceiptLineInformationDto.rmlExtendedCostBase,
						rmlExtendedCostForeign = eRPReceiptLineInformationDto.rmlExtendedCostForeign,
						rmlForm1099Box = eRPReceiptLineInformationDto.rmlForm1099Box,
						rmlFreightUnitCost = eRPReceiptLineInformationDto.rmlFreightUnitCost,
						rmlHeatLot = eRPReceiptLineInformationDto.rmlHeatLot,
						rmlInspectionNotesRTF = eRPReceiptLineInformationDto.rmlInspectionNotesRTF,
						rmlInspectionNotesText = eRPReceiptLineInformationDto.rmlInspectionNotesText,
						rmlInventoryQuantityReceived = eRPReceiptLineInformationDto.rmlInventoryQuantityReceived,
						rmlInventoryUnitCost = eRPReceiptLineInformationDto.rmlInventoryUnitCost,
						rmlInventoryUnitCostForeign = eRPReceiptLineInformationDto.rmlInventoryUnitCostForeign,
						rmlInventoryUnitOfMeasure = eRPReceiptLineInformationDto.rmlInventoryUnitOfMeasure,
						rmlClosed = eRPReceiptLineInformationDto.rmlClosed,
						rmlInInspection = eRPReceiptLineInformationDto.rmlInInspection,
						rmlInspectionComplete = eRPReceiptLineInformationDto.rmlInspectionComplete,
						rmlInvoicedComplete = eRPReceiptLineInformationDto.rmlInvoicedComplete,
						rmlJobReceivedComplete = eRPReceiptLineInformationDto.rmlJobReceivedComplete,
						rmlKitPart = eRPReceiptLineInformationDto.rmlKitPart,
						rmlPoReceivedComplete = eRPReceiptLineInformationDto.rmlPoReceivedComplete,
						rmlPostedToGl = eRPReceiptLineInformationDto.rmlPostedToGl,
						rmlRequiresInspection = eRPReceiptLineInformationDto.rmlRequiresInspection,
						rmlReversed = eRPReceiptLineInformationDto.rmlReversed,
						rmlTrackSerialNumbers = eRPReceiptLineInformationDto.rmlTrackSerialNumbers,
						rmlJobAssemblyID = eRPReceiptLineInformationDto.rmlJobAssemblyID,
						rmlJobEstimatedQuantity = eRPReceiptLineInformationDto.rmlJobEstimatedQuantity,
						rmlJobID = eRPReceiptLineInformationDto.rmlJobID,
						rmlJobMaterialID = eRPReceiptLineInformationDto.rmlJobMaterialID,
						rmlJobMatQuantityReceived = eRPReceiptLineInformationDto.rmlJobMatQuantityReceived,
						rmlJobOpenQuantity = eRPReceiptLineInformationDto.rmlJobOpenQuantity,
						rmlJobOperationID = eRPReceiptLineInformationDto.rmlJobOperationID,
						rmlJobOprQuantityReceived = eRPReceiptLineInformationDto.rmlJobOprQuantityReceived,
						rmlJobType = eRPReceiptLineInformationDto.rmlJobType,
						rmlMiscUnitCost = eRPReceiptLineInformationDto.rmlMiscUnitCost,
						rmlOrgPartID = eRPReceiptLineInformationDto.rmlOrgPartID,
						rmlOrgPartShortDescription = eRPReceiptLineInformationDto.rmlOrgPartShortDescription,
						rmlPartBinID = eRPReceiptLineInformationDto.rmlPartBinID,
						rmlPartID = eRPReceiptLineInformationDto.rmlPartID,
						rmlPartLongDescriptionRtf = eRPReceiptLineInformationDto.rmlPartLongDescriptionRtf,
						rmlPartLongDescriptionText = eRPReceiptLineInformationDto.rmlPartLongDescriptionText,
						rmlPartRevisionID = eRPReceiptLineInformationDto.rmlPartRevisionID,
						rmlPartWarehouseLocationID = eRPReceiptLineInformationDto.rmlPartWarehouseLocationID,
						rmlPoOpenQuantity = eRPReceiptLineInformationDto.rmlPoOpenQuantity,
						rmlPoPurchaseQuantity = eRPReceiptLineInformationDto.rmlPoPurchaseQuantity,
						rmlProjectAreaID = eRPReceiptLineInformationDto.rmlProjectAreaID,
						rmlProjectID = eRPReceiptLineInformationDto.rmlProjectID,
						rmlPurchaseOrderID = eRPReceiptLineInformationDto.rmlPurchaseOrderID,
						rmlPurchaseOrderLineID = eRPReceiptLineInformationDto.rmlPurchaseOrderLineID,
						rmlPurchaseQuantityReceived = eRPReceiptLineInformationDto.rmlPurchaseQuantityReceived,
						rmlPurchaseUnitCost = eRPReceiptLineInformationDto.rmlPurchaseUnitCost,
						rmlPurchaseUnitCostForeign = eRPReceiptLineInformationDto.rmlPurchaseUnitCostForeign,
						rmlPurchaseUnitOfMeasure = eRPReceiptLineInformationDto.rmlPurchaseUnitOfMeasure,
						rmlQuantityToInspect = eRPReceiptLineInformationDto.rmlQuantityToInspect,
						rmlReceiptID = eRPReceiptLineInformationDto.rmlReceiptID,
						rmlReference = eRPReceiptLineInformationDto.rmlReference,
						rmlReverseReceiptID = eRPReceiptLineInformationDto.rmlReverseReceiptID,
						rmlReverseReceiptLineID = eRPReceiptLineInformationDto.rmlReverseReceiptLineID,
						rmlRmaClaimID = eRPReceiptLineInformationDto.rmlRmaClaimID,
						rmlRmaClaimLineID = eRPReceiptLineInformationDto.rmlRmaClaimLineID,
						rmlRowVersion = eRPReceiptLineInformationDto.rmlRowVersion,
						rmlSalesOrderDeliveryID = eRPReceiptLineInformationDto.rmlSalesOrderDeliveryID,
						rmlSalesOrderID = eRPReceiptLineInformationDto.rmlSalesOrderID,
						rmlSalesOrderLineID = eRPReceiptLineInformationDto.rmlSalesOrderLineID,
						rmlReceiptLineID = eRPReceiptLineInformationDto.rmlReceiptLineID,
						rmlSetupCharge = eRPReceiptLineInformationDto.rmlSetupCharge,
						rmlSetupChargeForeign = eRPReceiptLineInformationDto.rmlSetupChargeForeign,
						rmlTotalComponentCosts = eRPReceiptLineInformationDto.rmlTotalComponentCosts,
						CustomFields = eRPReceiptLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ReceiptLine [{receiptLine.rmlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPReceiptLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteReceiptLine(Guid receiptLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPReceiptLineRepository iERPReceiptLineRepository = (base.ERPReceiptLineRepository = new ERPReceiptLineRepository(base.ApiClientContext));
		using (iERPReceiptLineRepository)
		{
			if (!(await base.ERPReceiptLineRepository.DoesReceiptLineExist(receiptLineId)))
			{
				base.ErrorsList.Add($"ReceiptLine [{receiptLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPReceiptLineInformationDto eRPReceiptLineInformationDto = await base.ERPReceiptLineRepository.GetReceiptLine(receiptLineId);
				string text = await base.ERPReceiptLineRepository.WhereUsed("ReceiptLines", new object[2] { eRPReceiptLineInformationDto.rmlReceiptID, eRPReceiptLineInformationDto.rmlReceiptLineID }, new object[2] { "rmlReceiptID", "rmlReceiptLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ReceiptLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPReceiptLineDto>> Process_DeleteReceiptLine(Guid receiptLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPReceiptLineDto> result;
		try
		{
			IERPReceiptLineRepository iERPReceiptLineRepository = (base.ERPReceiptLineRepository = new ERPReceiptLineRepository(base.ApiClientContext));
			using (iERPReceiptLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPReceiptLineRepository.DeleteRowFromTable("ReceiptLines", "rml", receiptLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ReceiptLine [{receiptLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPReceiptLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPReceiptLineDto()
			};
		}
		return result;
	}
}
