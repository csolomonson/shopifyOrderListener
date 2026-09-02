using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPDMRClaimLineModel : ERPBaseModel, IERPDMRClaimLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllDMRClaimLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPDMRClaimLineRepository iERPDMRClaimLineRepository = (base.ERPDMRClaimLineRepository = new ERPDMRClaimLineRepository(base.ApiClientContext));
		using (iERPDMRClaimLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPDMRClaimLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPDMRClaimLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPDMRClaimLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPDMRClaimLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetDMRClaimLine(Guid dMRClaimLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRClaimLineRepository iERPDMRClaimLineRepository = (base.ERPDMRClaimLineRepository = new ERPDMRClaimLineRepository(base.ApiClientContext));
		using (iERPDMRClaimLineRepository)
		{
			if (!(await base.ERPDMRClaimLineRepository.DoesDMRClaimLineExist(dMRClaimLineId)))
			{
				errorsList.Add($"DMRClaimLine [{dMRClaimLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutDMRClaimLine(ERPDMRClaimLineDto dMRClaimLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRClaimLineRepository iERPDMRClaimLineRepository = (base.ERPDMRClaimLineRepository = new ERPDMRClaimLineRepository(base.ApiClientContext));
		using (iERPDMRClaimLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(dMRClaimLine.dmlDmrClaimID) && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("DMRClaims", new object[1] { "DMPDMRCLAIMID" }, new object[1] { dMRClaimLine.dmlDmrClaimID })))
			{
				errorsList.Add("dmlDmrClaimID [" + dMRClaimLine.dmlDmrClaimID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimLine.dmlPartID) && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { dMRClaimLine.dmlPartID })))
			{
				errorsList.Add("dmlPartID [" + dMRClaimLine.dmlPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimLine.dmlPartRevisionID) && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { dMRClaimLine.dmlPartID, dMRClaimLine.dmlPartRevisionID })))
			{
				errorsList.Add("dmlPartRevisionID [" + dMRClaimLine.dmlPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimLine.dmlPartWarehouseLocationID) && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { dMRClaimLine.dmlPartID, dMRClaimLine.dmlPartRevisionID, dMRClaimLine.dmlPartWarehouseLocationID })))
			{
				errorsList.Add("dmlPartWarehouseLocationID [" + dMRClaimLine.dmlPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimLine.dmlPartBinID) && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { dMRClaimLine.dmlPartID, dMRClaimLine.dmlPartRevisionID, dMRClaimLine.dmlPartWarehouseLocationID, dMRClaimLine.dmlPartBinID })))
			{
				errorsList.Add("dmlPartBinID [" + dMRClaimLine.dmlPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimLine.dmlReturnReasonID) && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { dMRClaimLine.dmlReturnReasonID })))
			{
				errorsList.Add("dmlReturnReasonID [" + dMRClaimLine.dmlReturnReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimLine.dmlShippingMethodID) && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { dMRClaimLine.dmlShippingMethodID })))
			{
				errorsList.Add("dmlShippingMethodID [" + dMRClaimLine.dmlShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimLine.dmlInspectionID) && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("Inspections", new object[1] { "QAPINSPECTIONID" }, new object[1] { dMRClaimLine.dmlInspectionID })))
			{
				errorsList.Add("dmlInspectionID [" + dMRClaimLine.dmlInspectionID + "] not found.");
			}
			if (dMRClaimLine.dmlInspectionLineID > 0 && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("InspectionLines", new object[2] { "QALINSPECTIONID", "QALINSPECTIONLINEID" }, new object[2] { dMRClaimLine.dmlInspectionID, dMRClaimLine.dmlInspectionLineID })))
			{
				errorsList.Add($"dmlInspectionLineID [{dMRClaimLine.dmlInspectionLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimLine.dmlPurchaseOrderID) && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { dMRClaimLine.dmlPurchaseOrderID })))
			{
				errorsList.Add("dmlPurchaseOrderID [" + dMRClaimLine.dmlPurchaseOrderID + "] not found.");
			}
			if (dMRClaimLine.dmlPurchaseOrderLineID > 0 && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("PurchaseOrderLines", new object[2] { "PMLPURCHASEORDERID", "PMLPURCHASEORDERLINEID" }, new object[2] { dMRClaimLine.dmlPurchaseOrderID, dMRClaimLine.dmlPurchaseOrderLineID })))
			{
				errorsList.Add($"dmlPurchaseOrderLineID [{dMRClaimLine.dmlPurchaseOrderLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimLine.dmlReceiptID) && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("Receipts", new object[1] { "RMPRECEIPTID" }, new object[1] { dMRClaimLine.dmlReceiptID })))
			{
				errorsList.Add("dmlReceiptID [" + dMRClaimLine.dmlReceiptID + "] not found.");
			}
			if (dMRClaimLine.dmlReceiptLineID > 0 && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("ReceiptLines", new object[2] { "RMLRECEIPTID", "RMLRECEIPTLINEID" }, new object[2] { dMRClaimLine.dmlReceiptID, dMRClaimLine.dmlReceiptLineID })))
			{
				errorsList.Add($"dmlReceiptLineID [{dMRClaimLine.dmlReceiptLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimLine.dmlProjectID) && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { dMRClaimLine.dmlProjectID })))
			{
				errorsList.Add("dmlProjectID [" + dMRClaimLine.dmlProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimLine.dmlProjectAreaID) && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { dMRClaimLine.dmlProjectID, dMRClaimLine.dmlProjectAreaID })))
			{
				errorsList.Add("dmlProjectAreaID [" + dMRClaimLine.dmlProjectAreaID + "] not found.");
			}
			if (dMRClaimLine.dmlJobAssemblyID > 0 && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { dMRClaimLine.dmlJobID, dMRClaimLine.dmlJobAssemblyID })))
			{
				errorsList.Add($"dmlJobAssemblyID [{dMRClaimLine.dmlJobAssemblyID}] not found.");
			}
			if (dMRClaimLine.dmlJobMaterialID > 0 && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { dMRClaimLine.dmlJobID, dMRClaimLine.dmlJobAssemblyID, dMRClaimLine.dmlJobMaterialID })))
			{
				errorsList.Add($"dmlJobMaterialID [{dMRClaimLine.dmlJobMaterialID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimLine.dmlJobID) && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { dMRClaimLine.dmlJobID })))
			{
				errorsList.Add("dmlJobID [" + dMRClaimLine.dmlJobID + "] not found.");
			}
			if (dMRClaimLine.dmlJobOperationID > 0 && !(await base.ERPDMRClaimLineRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { dMRClaimLine.dmlJobID, dMRClaimLine.dmlJobAssemblyID, dMRClaimLine.dmlJobOperationID })))
			{
				errorsList.Add($"dmlJobOperationID [{dMRClaimLine.dmlJobOperationID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPDMRClaimLineDto>>> Process_GetAllDMRClaimLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPDMRClaimLineDto> allDMRClaimLinesDto = new List<ERPDMRClaimLineDto>();
		ERPResponseMessageDto<IList<ERPDMRClaimLineDto>> result;
		try
		{
			IERPDMRClaimLineRepository iERPDMRClaimLineRepository = (base.ERPDMRClaimLineRepository = new ERPDMRClaimLineRepository(base.ApiClientContext));
			using (iERPDMRClaimLineRepository)
			{
				foreach (ERPDMRClaimLineInformationDto item2 in await base.ERPDMRClaimLineRepository.GetAllDMRClaimLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPDMRClaimLineDto item = new ERPDMRClaimLineDto
					{
						dmlConversionFactor = item2.dmlConversionFactor,
						dmlCreatedBy = item2.dmlCreatedBy,
						dmlCreatedDate = item2.dmlCreatedDate,
						dmlDmrClaimID = item2.dmlDmrClaimID,
						dmlDmrShipmentID = item2.dmlDmrShipmentID,
						dmlDmrShipmentLineID = item2.dmlDmrShipmentLineID,
						dmlUniqueID = item2.dmlUniqueID,
						dmlExtendedCost = item2.dmlExtendedCost,
						dmlExtendedCostForeign = item2.dmlExtendedCostForeign,
						dmlInspectionID = item2.dmlInspectionID,
						dmlInspectionLineID = item2.dmlInspectionLineID,
						dmlInventoryQuantity = item2.dmlInventoryQuantity,
						dmlInventoryQuantityShipped = item2.dmlInventoryQuantityShipped,
						dmlInventoryUnitOfMeasure = item2.dmlInventoryUnitOfMeasure,
						dmlInvoicedComplete = item2.dmlInvoicedComplete,
						dmlKitPart = item2.dmlKitPart,
						dmlScrap = item2.dmlScrap,
						dmlShippedComplete = item2.dmlShippedComplete,
						dmlTransferredToDmrShipment = item2.dmlTransferredToDmrShipment,
						dmlTransferredToPurchaseOrder = item2.dmlTransferredToPurchaseOrder,
						dmlJobAssemblyID = item2.dmlJobAssemblyID,
						dmlJobID = item2.dmlJobID,
						dmlJobMaterialID = item2.dmlJobMaterialID,
						dmlJobOperationID = item2.dmlJobOperationID,
						dmlOrgPartID = item2.dmlOrgPartID,
						dmlOrgPartShortDescription = item2.dmlOrgPartShortDescription,
						dmlPartBinID = item2.dmlPartBinID,
						dmlPartID = item2.dmlPartID,
						dmlPartLongDescriptionRtf = item2.dmlPartLongDescriptionRtf,
						dmlPartLongDescriptionText = item2.dmlPartLongDescriptionText,
						dmlPartRevisionID = item2.dmlPartRevisionID,
						dmlPartShortDescription = item2.dmlPartShortDescription,
						dmlPartWarehouseLocationID = item2.dmlPartWarehouseLocationID,
						dmlProjectAreaID = item2.dmlProjectAreaID,
						dmlProjectID = item2.dmlProjectID,
						dmlPurchaseOrderID = item2.dmlPurchaseOrderID,
						dmlPurchaseOrderLineID = item2.dmlPurchaseOrderLineID,
						dmlQuantity = item2.dmlQuantity,
						dmlQuantityShipped = item2.dmlQuantityShipped,
						dmlReceiptID = item2.dmlReceiptID,
						dmlReceiptLineID = item2.dmlReceiptLineID,
						dmlReceivedDate = item2.dmlReceivedDate,
						dmlRequiredDate = item2.dmlRequiredDate,
						dmlReturnedDate = item2.dmlReturnedDate,
						dmlReturnReasonID = item2.dmlReturnReasonID,
						dmlRowVersion = item2.dmlRowVersion,
						dmlDmrClaimLineID = item2.dmlDmrClaimLineID,
						dmlShippedDate = item2.dmlShippedDate,
						dmlShippingMethodID = item2.dmlShippingMethodID,
						dmlSupplierAuthorizationNumber = item2.dmlSupplierAuthorizationNumber,
						dmlTrackingNumber = item2.dmlTrackingNumber,
						dmlUnitCost = item2.dmlUnitCost,
						dmlUnitCostForeign = item2.dmlUnitCostForeign,
						dmlUnitOfMeasure = item2.dmlUnitOfMeasure,
						CustomFields = item2.CustomFields
					};
					allDMRClaimLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all DMRClaimLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPDMRClaimLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allDMRClaimLinesDto,
				RecordCount = allDMRClaimLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPDMRClaimLineDto>> Process_GetDMRClaimLine(Guid dMRClaimLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPDMRClaimLineDto dMRClaimLineDto = null;
		ERPResponseMessageDto<ERPDMRClaimLineDto> result;
		try
		{
			IERPDMRClaimLineRepository iERPDMRClaimLineRepository = (base.ERPDMRClaimLineRepository = new ERPDMRClaimLineRepository(base.ApiClientContext));
			using (iERPDMRClaimLineRepository)
			{
				ERPDMRClaimLineInformationDto eRPDMRClaimLineInformationDto = await base.ERPDMRClaimLineRepository.GetDMRClaimLine(dMRClaimLineId);
				dMRClaimLineDto = new ERPDMRClaimLineDto
				{
					dmlConversionFactor = eRPDMRClaimLineInformationDto.dmlConversionFactor,
					dmlCreatedBy = eRPDMRClaimLineInformationDto.dmlCreatedBy,
					dmlCreatedDate = eRPDMRClaimLineInformationDto.dmlCreatedDate,
					dmlDmrClaimID = eRPDMRClaimLineInformationDto.dmlDmrClaimID,
					dmlDmrShipmentID = eRPDMRClaimLineInformationDto.dmlDmrShipmentID,
					dmlDmrShipmentLineID = eRPDMRClaimLineInformationDto.dmlDmrShipmentLineID,
					dmlUniqueID = eRPDMRClaimLineInformationDto.dmlUniqueID,
					dmlExtendedCost = eRPDMRClaimLineInformationDto.dmlExtendedCost,
					dmlExtendedCostForeign = eRPDMRClaimLineInformationDto.dmlExtendedCostForeign,
					dmlInspectionID = eRPDMRClaimLineInformationDto.dmlInspectionID,
					dmlInspectionLineID = eRPDMRClaimLineInformationDto.dmlInspectionLineID,
					dmlInventoryQuantity = eRPDMRClaimLineInformationDto.dmlInventoryQuantity,
					dmlInventoryQuantityShipped = eRPDMRClaimLineInformationDto.dmlInventoryQuantityShipped,
					dmlInventoryUnitOfMeasure = eRPDMRClaimLineInformationDto.dmlInventoryUnitOfMeasure,
					dmlInvoicedComplete = eRPDMRClaimLineInformationDto.dmlInvoicedComplete,
					dmlKitPart = eRPDMRClaimLineInformationDto.dmlKitPart,
					dmlScrap = eRPDMRClaimLineInformationDto.dmlScrap,
					dmlShippedComplete = eRPDMRClaimLineInformationDto.dmlShippedComplete,
					dmlTransferredToDmrShipment = eRPDMRClaimLineInformationDto.dmlTransferredToDmrShipment,
					dmlTransferredToPurchaseOrder = eRPDMRClaimLineInformationDto.dmlTransferredToPurchaseOrder,
					dmlJobAssemblyID = eRPDMRClaimLineInformationDto.dmlJobAssemblyID,
					dmlJobID = eRPDMRClaimLineInformationDto.dmlJobID,
					dmlJobMaterialID = eRPDMRClaimLineInformationDto.dmlJobMaterialID,
					dmlJobOperationID = eRPDMRClaimLineInformationDto.dmlJobOperationID,
					dmlOrgPartID = eRPDMRClaimLineInformationDto.dmlOrgPartID,
					dmlOrgPartShortDescription = eRPDMRClaimLineInformationDto.dmlOrgPartShortDescription,
					dmlPartBinID = eRPDMRClaimLineInformationDto.dmlPartBinID,
					dmlPartID = eRPDMRClaimLineInformationDto.dmlPartID,
					dmlPartLongDescriptionRtf = eRPDMRClaimLineInformationDto.dmlPartLongDescriptionRtf,
					dmlPartLongDescriptionText = eRPDMRClaimLineInformationDto.dmlPartLongDescriptionText,
					dmlPartRevisionID = eRPDMRClaimLineInformationDto.dmlPartRevisionID,
					dmlPartShortDescription = eRPDMRClaimLineInformationDto.dmlPartShortDescription,
					dmlPartWarehouseLocationID = eRPDMRClaimLineInformationDto.dmlPartWarehouseLocationID,
					dmlProjectAreaID = eRPDMRClaimLineInformationDto.dmlProjectAreaID,
					dmlProjectID = eRPDMRClaimLineInformationDto.dmlProjectID,
					dmlPurchaseOrderID = eRPDMRClaimLineInformationDto.dmlPurchaseOrderID,
					dmlPurchaseOrderLineID = eRPDMRClaimLineInformationDto.dmlPurchaseOrderLineID,
					dmlQuantity = eRPDMRClaimLineInformationDto.dmlQuantity,
					dmlQuantityShipped = eRPDMRClaimLineInformationDto.dmlQuantityShipped,
					dmlReceiptID = eRPDMRClaimLineInformationDto.dmlReceiptID,
					dmlReceiptLineID = eRPDMRClaimLineInformationDto.dmlReceiptLineID,
					dmlReceivedDate = eRPDMRClaimLineInformationDto.dmlReceivedDate,
					dmlRequiredDate = eRPDMRClaimLineInformationDto.dmlRequiredDate,
					dmlReturnedDate = eRPDMRClaimLineInformationDto.dmlReturnedDate,
					dmlReturnReasonID = eRPDMRClaimLineInformationDto.dmlReturnReasonID,
					dmlRowVersion = eRPDMRClaimLineInformationDto.dmlRowVersion,
					dmlDmrClaimLineID = eRPDMRClaimLineInformationDto.dmlDmrClaimLineID,
					dmlShippedDate = eRPDMRClaimLineInformationDto.dmlShippedDate,
					dmlShippingMethodID = eRPDMRClaimLineInformationDto.dmlShippingMethodID,
					dmlSupplierAuthorizationNumber = eRPDMRClaimLineInformationDto.dmlSupplierAuthorizationNumber,
					dmlTrackingNumber = eRPDMRClaimLineInformationDto.dmlTrackingNumber,
					dmlUnitCost = eRPDMRClaimLineInformationDto.dmlUnitCost,
					dmlUnitCostForeign = eRPDMRClaimLineInformationDto.dmlUnitCostForeign,
					dmlUnitOfMeasure = eRPDMRClaimLineInformationDto.dmlUnitOfMeasure,
					CustomFields = eRPDMRClaimLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the DMRClaimLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRClaimLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = dMRClaimLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPDMRClaimLineDto>> Process_PutDMRClaimLine(ERPDMRClaimLineDto dMRClaimLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPDMRClaimLineDto createdObject = null;
		ERPResponseMessageDto<ERPDMRClaimLineDto> result;
		try
		{
			IERPDMRClaimLineRepository iERPDMRClaimLineRepository = (base.ERPDMRClaimLineRepository = new ERPDMRClaimLineRepository(base.ApiClientContext));
			using (iERPDMRClaimLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPDMRClaimLineRepository.SaveDMRClaimLine(dMRClaimLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPDMRClaimLineInformationDto eRPDMRClaimLineInformationDto = await base.ERPDMRClaimLineRepository.GetDMRClaimLine(dMRClaimLine.dmlUniqueID);
					createdObject = new ERPDMRClaimLineDto
					{
						dmlConversionFactor = eRPDMRClaimLineInformationDto.dmlConversionFactor,
						dmlCreatedBy = eRPDMRClaimLineInformationDto.dmlCreatedBy,
						dmlCreatedDate = eRPDMRClaimLineInformationDto.dmlCreatedDate,
						dmlDmrClaimID = eRPDMRClaimLineInformationDto.dmlDmrClaimID,
						dmlDmrShipmentID = eRPDMRClaimLineInformationDto.dmlDmrShipmentID,
						dmlDmrShipmentLineID = eRPDMRClaimLineInformationDto.dmlDmrShipmentLineID,
						dmlUniqueID = eRPDMRClaimLineInformationDto.dmlUniqueID,
						dmlExtendedCost = eRPDMRClaimLineInformationDto.dmlExtendedCost,
						dmlExtendedCostForeign = eRPDMRClaimLineInformationDto.dmlExtendedCostForeign,
						dmlInspectionID = eRPDMRClaimLineInformationDto.dmlInspectionID,
						dmlInspectionLineID = eRPDMRClaimLineInformationDto.dmlInspectionLineID,
						dmlInventoryQuantity = eRPDMRClaimLineInformationDto.dmlInventoryQuantity,
						dmlInventoryQuantityShipped = eRPDMRClaimLineInformationDto.dmlInventoryQuantityShipped,
						dmlInventoryUnitOfMeasure = eRPDMRClaimLineInformationDto.dmlInventoryUnitOfMeasure,
						dmlInvoicedComplete = eRPDMRClaimLineInformationDto.dmlInvoicedComplete,
						dmlKitPart = eRPDMRClaimLineInformationDto.dmlKitPart,
						dmlScrap = eRPDMRClaimLineInformationDto.dmlScrap,
						dmlShippedComplete = eRPDMRClaimLineInformationDto.dmlShippedComplete,
						dmlTransferredToDmrShipment = eRPDMRClaimLineInformationDto.dmlTransferredToDmrShipment,
						dmlTransferredToPurchaseOrder = eRPDMRClaimLineInformationDto.dmlTransferredToPurchaseOrder,
						dmlJobAssemblyID = eRPDMRClaimLineInformationDto.dmlJobAssemblyID,
						dmlJobID = eRPDMRClaimLineInformationDto.dmlJobID,
						dmlJobMaterialID = eRPDMRClaimLineInformationDto.dmlJobMaterialID,
						dmlJobOperationID = eRPDMRClaimLineInformationDto.dmlJobOperationID,
						dmlOrgPartID = eRPDMRClaimLineInformationDto.dmlOrgPartID,
						dmlOrgPartShortDescription = eRPDMRClaimLineInformationDto.dmlOrgPartShortDescription,
						dmlPartBinID = eRPDMRClaimLineInformationDto.dmlPartBinID,
						dmlPartID = eRPDMRClaimLineInformationDto.dmlPartID,
						dmlPartLongDescriptionRtf = eRPDMRClaimLineInformationDto.dmlPartLongDescriptionRtf,
						dmlPartLongDescriptionText = eRPDMRClaimLineInformationDto.dmlPartLongDescriptionText,
						dmlPartRevisionID = eRPDMRClaimLineInformationDto.dmlPartRevisionID,
						dmlPartShortDescription = eRPDMRClaimLineInformationDto.dmlPartShortDescription,
						dmlPartWarehouseLocationID = eRPDMRClaimLineInformationDto.dmlPartWarehouseLocationID,
						dmlProjectAreaID = eRPDMRClaimLineInformationDto.dmlProjectAreaID,
						dmlProjectID = eRPDMRClaimLineInformationDto.dmlProjectID,
						dmlPurchaseOrderID = eRPDMRClaimLineInformationDto.dmlPurchaseOrderID,
						dmlPurchaseOrderLineID = eRPDMRClaimLineInformationDto.dmlPurchaseOrderLineID,
						dmlQuantity = eRPDMRClaimLineInformationDto.dmlQuantity,
						dmlQuantityShipped = eRPDMRClaimLineInformationDto.dmlQuantityShipped,
						dmlReceiptID = eRPDMRClaimLineInformationDto.dmlReceiptID,
						dmlReceiptLineID = eRPDMRClaimLineInformationDto.dmlReceiptLineID,
						dmlReceivedDate = eRPDMRClaimLineInformationDto.dmlReceivedDate,
						dmlRequiredDate = eRPDMRClaimLineInformationDto.dmlRequiredDate,
						dmlReturnedDate = eRPDMRClaimLineInformationDto.dmlReturnedDate,
						dmlReturnReasonID = eRPDMRClaimLineInformationDto.dmlReturnReasonID,
						dmlRowVersion = eRPDMRClaimLineInformationDto.dmlRowVersion,
						dmlDmrClaimLineID = eRPDMRClaimLineInformationDto.dmlDmrClaimLineID,
						dmlShippedDate = eRPDMRClaimLineInformationDto.dmlShippedDate,
						dmlShippingMethodID = eRPDMRClaimLineInformationDto.dmlShippingMethodID,
						dmlSupplierAuthorizationNumber = eRPDMRClaimLineInformationDto.dmlSupplierAuthorizationNumber,
						dmlTrackingNumber = eRPDMRClaimLineInformationDto.dmlTrackingNumber,
						dmlUnitCost = eRPDMRClaimLineInformationDto.dmlUnitCost,
						dmlUnitCostForeign = eRPDMRClaimLineInformationDto.dmlUnitCostForeign,
						dmlUnitOfMeasure = eRPDMRClaimLineInformationDto.dmlUnitOfMeasure,
						CustomFields = eRPDMRClaimLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing DMRClaimLine [{dMRClaimLine.dmlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRClaimLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteDMRClaimLine(Guid dMRClaimLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRClaimLineRepository iERPDMRClaimLineRepository = (base.ERPDMRClaimLineRepository = new ERPDMRClaimLineRepository(base.ApiClientContext));
		using (iERPDMRClaimLineRepository)
		{
			if (!(await base.ERPDMRClaimLineRepository.DoesDMRClaimLineExist(dMRClaimLineId)))
			{
				base.ErrorsList.Add($"DMRClaimLine [{dMRClaimLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPDMRClaimLineInformationDto eRPDMRClaimLineInformationDto = await base.ERPDMRClaimLineRepository.GetDMRClaimLine(dMRClaimLineId);
				string text = await base.ERPDMRClaimLineRepository.WhereUsed("DMRClaimLines", new object[2] { eRPDMRClaimLineInformationDto.dmlDmrClaimID, eRPDMRClaimLineInformationDto.dmlDmrClaimLineID }, new object[2] { "dmlDmrClaimID", "dmlDmrClaimLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("DMRClaimLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPDMRClaimLineDto>> Process_DeleteDMRClaimLine(Guid dMRClaimLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPDMRClaimLineDto> result;
		try
		{
			IERPDMRClaimLineRepository iERPDMRClaimLineRepository = (base.ERPDMRClaimLineRepository = new ERPDMRClaimLineRepository(base.ApiClientContext));
			using (iERPDMRClaimLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPDMRClaimLineRepository.DeleteRowFromTable("DMRClaimLines", "dml", dMRClaimLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of DMRClaimLine [{dMRClaimLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRClaimLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPDMRClaimLineDto()
			};
		}
		return result;
	}
}
