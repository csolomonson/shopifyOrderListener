using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPMfgReceiptModel : ERPBaseModel, IERPMfgReceiptModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllMfgReceipts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPMfgReceiptRepository iERPMfgReceiptRepository = (base.ERPMfgReceiptRepository = new ERPMfgReceiptRepository(base.ApiClientContext));
		using (iERPMfgReceiptRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPMfgReceiptRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPMfgReceiptRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPMfgReceiptRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPMfgReceiptRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetMfgReceipt(Guid mfgReceiptId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMfgReceiptRepository iERPMfgReceiptRepository = (base.ERPMfgReceiptRepository = new ERPMfgReceiptRepository(base.ApiClientContext));
		using (iERPMfgReceiptRepository)
		{
			if (!(await base.ERPMfgReceiptRepository.DoesMfgReceiptExist(mfgReceiptId)))
			{
				errorsList.Add($"MfgReceipt [{mfgReceiptId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutMfgReceipt(ERPMfgReceiptDto mfgReceipt)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMfgReceiptRepository iERPMfgReceiptRepository = (base.ERPMfgReceiptRepository = new ERPMfgReceiptRepository(base.ApiClientContext));
		using (iERPMfgReceiptRepository)
		{
			if (!string.IsNullOrWhiteSpace(mfgReceipt.rmmPurchaseOrderID) && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { mfgReceipt.rmmPurchaseOrderID })))
			{
				errorsList.Add("rmmPurchaseOrderID [" + mfgReceipt.rmmPurchaseOrderID + "] not found.");
			}
			if (mfgReceipt.rmmPurchaseOrderLineID > 0 && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("PurchaseOrderLines", new object[2] { "PMLPURCHASEORDERID", "PMLPURCHASEORDERLINEID" }, new object[2] { mfgReceipt.rmmPurchaseOrderID, mfgReceipt.rmmPurchaseOrderLineID })))
			{
				errorsList.Add($"rmmPurchaseOrderLineID [{mfgReceipt.rmmPurchaseOrderLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceipt.rmmJobID) && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { mfgReceipt.rmmJobID })))
			{
				errorsList.Add("rmmJobID [" + mfgReceipt.rmmJobID + "] not found.");
			}
			if (mfgReceipt.rmmJobAssemblyID > 0 && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { mfgReceipt.rmmJobID, mfgReceipt.rmmJobAssemblyID })))
			{
				errorsList.Add($"rmmJobAssemblyID [{mfgReceipt.rmmJobAssemblyID}] not found.");
			}
			if (mfgReceipt.rmmJobMaterialID > 0 && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { mfgReceipt.rmmJobID, mfgReceipt.rmmJobAssemblyID, mfgReceipt.rmmJobMaterialID })))
			{
				errorsList.Add($"rmmJobMaterialID [{mfgReceipt.rmmJobMaterialID}] not found.");
			}
			if (mfgReceipt.rmmJobOperationID > 0 && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { mfgReceipt.rmmJobID, mfgReceipt.rmmJobAssemblyID, mfgReceipt.rmmJobOperationID })))
			{
				errorsList.Add($"rmmJobOperationID [{mfgReceipt.rmmJobOperationID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceipt.rmmPartID) && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { mfgReceipt.rmmPartID })))
			{
				errorsList.Add("rmmPartID [" + mfgReceipt.rmmPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceipt.rmmPartRevisionID) && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { mfgReceipt.rmmPartID, mfgReceipt.rmmPartRevisionID })))
			{
				errorsList.Add("rmmPartRevisionID [" + mfgReceipt.rmmPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceipt.rmmPartWarehouseLocationID) && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { mfgReceipt.rmmPartID, mfgReceipt.rmmPartRevisionID, mfgReceipt.rmmPartWarehouseLocationID })))
			{
				errorsList.Add("rmmPartWarehouseLocationID [" + mfgReceipt.rmmPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceipt.rmmPartBinID) && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { mfgReceipt.rmmPartID, mfgReceipt.rmmPartRevisionID, mfgReceipt.rmmPartWarehouseLocationID, mfgReceipt.rmmPartBinID })))
			{
				errorsList.Add("rmmPartBinID [" + mfgReceipt.rmmPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceipt.rmmSupplierOrganizationID) && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { mfgReceipt.rmmSupplierOrganizationID })))
			{
				errorsList.Add("rmmSupplierOrganizationID [" + mfgReceipt.rmmSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceipt.rmmPurchaseLocationID) && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { mfgReceipt.rmmSupplierOrganizationID, mfgReceipt.rmmPurchaseLocationID })))
			{
				errorsList.Add("rmmPurchaseLocationID [" + mfgReceipt.rmmPurchaseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceipt.rmmPlantID) && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { mfgReceipt.rmmPlantID })))
			{
				errorsList.Add("rmmPlantID [" + mfgReceipt.rmmPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceipt.rmmProjectID) && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { mfgReceipt.rmmProjectID })))
			{
				errorsList.Add("rmmProjectID [" + mfgReceipt.rmmProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceipt.rmmProjectAreaID) && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { mfgReceipt.rmmProjectID, mfgReceipt.rmmProjectAreaID })))
			{
				errorsList.Add("rmmProjectAreaID [" + mfgReceipt.rmmProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceipt.rmmPlantDepartmentID) && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { mfgReceipt.rmmPlantID, mfgReceipt.rmmPlantDepartmentID })))
			{
				errorsList.Add("rmmPlantDepartmentID [" + mfgReceipt.rmmPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceipt.rmmReverseMfgReceiptID) && !(await base.ERPMfgReceiptRepository.DoesRecordExistInTableUsingKeys("MfgReceipts", new object[1] { "rmmMfgReceiptID" }, new object[1] { mfgReceipt.rmmReverseMfgReceiptID })))
			{
				errorsList.Add("rmmReverseMfgReceiptID [" + mfgReceipt.rmmReverseMfgReceiptID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPMfgReceiptDto>>> Process_GetAllMfgReceipts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPMfgReceiptDto> allMfgReceiptsDto = new List<ERPMfgReceiptDto>();
		ERPResponseMessageDto<IList<ERPMfgReceiptDto>> result;
		try
		{
			IERPMfgReceiptRepository iERPMfgReceiptRepository = (base.ERPMfgReceiptRepository = new ERPMfgReceiptRepository(base.ApiClientContext));
			using (iERPMfgReceiptRepository)
			{
				foreach (ERPMfgReceiptInformationDto item2 in await base.ERPMfgReceiptRepository.GetAllMfgReceipts(pageSize, pageNumber, filter, orderBy))
				{
					ERPMfgReceiptDto item = new ERPMfgReceiptDto
					{
						rmmMfgReceiptID = item2.rmmMfgReceiptID,
						rmmCreatedBy = item2.rmmCreatedBy,
						rmmCreatedDate = item2.rmmCreatedDate,
						rmmUniqueID = item2.rmmUniqueID,
						rmmEstimatedQuantity = item2.rmmEstimatedQuantity,
						rmmExtendedCostBase = item2.rmmExtendedCostBase,
						rmmHeatLot = item2.rmmHeatLot,
						rmmImCostingMethod = item2.rmmImCostingMethod,
						rmmInventoryQuantity = item2.rmmInventoryQuantity,
						rmmInventoryQuantityReceived = item2.rmmInventoryQuantityReceived,
						rmmInventoryUnitOfMeasure = item2.rmmInventoryUnitOfMeasure,
						rmmCreateJobSeq = item2.rmmCreateJobSeq,
						rmmInInspection = item2.rmmInInspection,
						rmmInspectionComplete = item2.rmmInspectionComplete,
						rmmKitPart = item2.rmmKitPart,
						rmmNotUpdateJobQtyComplete = item2.rmmNotUpdateJobQtyComplete,
						rmmPoLineReceivedComplete = item2.rmmPoLineReceivedComplete,
						rmmPosted = item2.rmmPosted,
						rmmProductionComplete = item2.rmmProductionComplete,
						rmmReceivedComplete = item2.rmmReceivedComplete,
						rmmRequiresInspection = item2.rmmRequiresInspection,
						rmmReversalEntry = item2.rmmReversalEntry,
						rmmReversed = item2.rmmReversed,
						rmmJobAsmQuantityReceived = item2.rmmJobAsmQuantityReceived,
						rmmJobAssemblyID = item2.rmmJobAssemblyID,
						rmmJobID = item2.rmmJobID,
						rmmJobMaterialID = item2.rmmJobMaterialID,
						rmmJobMatQuantityReceived = item2.rmmJobMatQuantityReceived,
						rmmJobOpenQuantity = item2.rmmJobOpenQuantity,
						rmmJobOperationID = item2.rmmJobOperationID,
						rmmJobOprQuantityReceived = item2.rmmJobOprQuantityReceived,
						rmmJobScrapQuantity = item2.rmmJobScrapQuantity,
						rmmJobType = item2.rmmJobType,
						rmmLongDescriptionRtf = item2.rmmLongDescriptionRtf,
						rmmLongDescriptionText = item2.rmmLongDescriptionText,
						rmmMfgCostType = item2.rmmMfgCostType,
						rmmMiscInvQuantityReceived = item2.rmmMiscInvQuantityReceived,
						rmmPartBinID = item2.rmmPartBinID,
						rmmPartID = item2.rmmPartID,
						rmmPartRevisionID = item2.rmmPartRevisionID,
						rmmPartWarehouseLocationID = item2.rmmPartWarehouseLocationID,
						rmmPlantDepartmentID = item2.rmmPlantDepartmentID,
						rmmPlantID = item2.rmmPlantID,
						rmmPoOpenQuantity = item2.rmmPoOpenQuantity,
						rmmPostedDate = item2.rmmPostedDate,
						rmmProductionQuantity = item2.rmmProductionQuantity,
						rmmProjectAreaID = item2.rmmProjectAreaID,
						rmmProjectID = item2.rmmProjectID,
						rmmPurchaseLocationID = item2.rmmPurchaseLocationID,
						rmmPurchaseOrderID = item2.rmmPurchaseOrderID,
						rmmPurchaseOrderLineID = item2.rmmPurchaseOrderLineID,
						rmmPurchaseQuantity = item2.rmmPurchaseQuantity,
						rmmPurchaseQuantityReceived = item2.rmmPurchaseQuantityReceived,
						rmmPurchaseUnitCost = item2.rmmPurchaseUnitCost,
						rmmPurchaseUnitOfMeasure = item2.rmmPurchaseUnitOfMeasure,
						rmmQuantityCompleted = item2.rmmQuantityCompleted,
						rmmQuantityOnHand = item2.rmmQuantityOnHand,
						rmmQuantityReceivedToInventory = item2.rmmQuantityReceivedToInventory,
						rmmQuantityToInspect = item2.rmmQuantityToInspect,
						rmmReceiptDate = item2.rmmReceiptDate,
						rmmReceiptType = item2.rmmReceiptType,
						rmmReference = item2.rmmReference,
						rmmReverseMfgReceiptID = item2.rmmReverseMfgReceiptID,
						rmmRowVersion = item2.rmmRowVersion,
						rmmScrapQuantity = item2.rmmScrapQuantity,
						rmmSetupCharge = item2.rmmSetupCharge,
						rmmSupplierOrganizationID = item2.rmmSupplierOrganizationID,
						rmmTotalComponentCosts = item2.rmmTotalComponentCosts,
						rmmTotalUnitCost = item2.rmmTotalUnitCost,
						rmmUnitLaborCost = item2.rmmUnitLaborCost,
						rmmUnitMaterialCost = item2.rmmUnitMaterialCost,
						rmmUnitOverheadCost = item2.rmmUnitOverheadCost,
						rmmUnitSubcontractCost = item2.rmmUnitSubcontractCost,
						CustomFields = item2.CustomFields
					};
					allMfgReceiptsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all MfgReceipts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPMfgReceiptDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allMfgReceiptsDto,
				RecordCount = allMfgReceiptsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMfgReceiptDto>> Process_GetMfgReceipt(Guid mfgReceiptId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPMfgReceiptDto mfgReceiptDto = null;
		ERPResponseMessageDto<ERPMfgReceiptDto> result;
		try
		{
			IERPMfgReceiptRepository iERPMfgReceiptRepository = (base.ERPMfgReceiptRepository = new ERPMfgReceiptRepository(base.ApiClientContext));
			using (iERPMfgReceiptRepository)
			{
				ERPMfgReceiptInformationDto eRPMfgReceiptInformationDto = await base.ERPMfgReceiptRepository.GetMfgReceipt(mfgReceiptId);
				mfgReceiptDto = new ERPMfgReceiptDto
				{
					rmmMfgReceiptID = eRPMfgReceiptInformationDto.rmmMfgReceiptID,
					rmmCreatedBy = eRPMfgReceiptInformationDto.rmmCreatedBy,
					rmmCreatedDate = eRPMfgReceiptInformationDto.rmmCreatedDate,
					rmmUniqueID = eRPMfgReceiptInformationDto.rmmUniqueID,
					rmmEstimatedQuantity = eRPMfgReceiptInformationDto.rmmEstimatedQuantity,
					rmmExtendedCostBase = eRPMfgReceiptInformationDto.rmmExtendedCostBase,
					rmmHeatLot = eRPMfgReceiptInformationDto.rmmHeatLot,
					rmmImCostingMethod = eRPMfgReceiptInformationDto.rmmImCostingMethod,
					rmmInventoryQuantity = eRPMfgReceiptInformationDto.rmmInventoryQuantity,
					rmmInventoryQuantityReceived = eRPMfgReceiptInformationDto.rmmInventoryQuantityReceived,
					rmmInventoryUnitOfMeasure = eRPMfgReceiptInformationDto.rmmInventoryUnitOfMeasure,
					rmmCreateJobSeq = eRPMfgReceiptInformationDto.rmmCreateJobSeq,
					rmmInInspection = eRPMfgReceiptInformationDto.rmmInInspection,
					rmmInspectionComplete = eRPMfgReceiptInformationDto.rmmInspectionComplete,
					rmmKitPart = eRPMfgReceiptInformationDto.rmmKitPart,
					rmmNotUpdateJobQtyComplete = eRPMfgReceiptInformationDto.rmmNotUpdateJobQtyComplete,
					rmmPoLineReceivedComplete = eRPMfgReceiptInformationDto.rmmPoLineReceivedComplete,
					rmmPosted = eRPMfgReceiptInformationDto.rmmPosted,
					rmmProductionComplete = eRPMfgReceiptInformationDto.rmmProductionComplete,
					rmmReceivedComplete = eRPMfgReceiptInformationDto.rmmReceivedComplete,
					rmmRequiresInspection = eRPMfgReceiptInformationDto.rmmRequiresInspection,
					rmmReversalEntry = eRPMfgReceiptInformationDto.rmmReversalEntry,
					rmmReversed = eRPMfgReceiptInformationDto.rmmReversed,
					rmmJobAsmQuantityReceived = eRPMfgReceiptInformationDto.rmmJobAsmQuantityReceived,
					rmmJobAssemblyID = eRPMfgReceiptInformationDto.rmmJobAssemblyID,
					rmmJobID = eRPMfgReceiptInformationDto.rmmJobID,
					rmmJobMaterialID = eRPMfgReceiptInformationDto.rmmJobMaterialID,
					rmmJobMatQuantityReceived = eRPMfgReceiptInformationDto.rmmJobMatQuantityReceived,
					rmmJobOpenQuantity = eRPMfgReceiptInformationDto.rmmJobOpenQuantity,
					rmmJobOperationID = eRPMfgReceiptInformationDto.rmmJobOperationID,
					rmmJobOprQuantityReceived = eRPMfgReceiptInformationDto.rmmJobOprQuantityReceived,
					rmmJobScrapQuantity = eRPMfgReceiptInformationDto.rmmJobScrapQuantity,
					rmmJobType = eRPMfgReceiptInformationDto.rmmJobType,
					rmmLongDescriptionRtf = eRPMfgReceiptInformationDto.rmmLongDescriptionRtf,
					rmmLongDescriptionText = eRPMfgReceiptInformationDto.rmmLongDescriptionText,
					rmmMfgCostType = eRPMfgReceiptInformationDto.rmmMfgCostType,
					rmmMiscInvQuantityReceived = eRPMfgReceiptInformationDto.rmmMiscInvQuantityReceived,
					rmmPartBinID = eRPMfgReceiptInformationDto.rmmPartBinID,
					rmmPartID = eRPMfgReceiptInformationDto.rmmPartID,
					rmmPartRevisionID = eRPMfgReceiptInformationDto.rmmPartRevisionID,
					rmmPartWarehouseLocationID = eRPMfgReceiptInformationDto.rmmPartWarehouseLocationID,
					rmmPlantDepartmentID = eRPMfgReceiptInformationDto.rmmPlantDepartmentID,
					rmmPlantID = eRPMfgReceiptInformationDto.rmmPlantID,
					rmmPoOpenQuantity = eRPMfgReceiptInformationDto.rmmPoOpenQuantity,
					rmmPostedDate = eRPMfgReceiptInformationDto.rmmPostedDate,
					rmmProductionQuantity = eRPMfgReceiptInformationDto.rmmProductionQuantity,
					rmmProjectAreaID = eRPMfgReceiptInformationDto.rmmProjectAreaID,
					rmmProjectID = eRPMfgReceiptInformationDto.rmmProjectID,
					rmmPurchaseLocationID = eRPMfgReceiptInformationDto.rmmPurchaseLocationID,
					rmmPurchaseOrderID = eRPMfgReceiptInformationDto.rmmPurchaseOrderID,
					rmmPurchaseOrderLineID = eRPMfgReceiptInformationDto.rmmPurchaseOrderLineID,
					rmmPurchaseQuantity = eRPMfgReceiptInformationDto.rmmPurchaseQuantity,
					rmmPurchaseQuantityReceived = eRPMfgReceiptInformationDto.rmmPurchaseQuantityReceived,
					rmmPurchaseUnitCost = eRPMfgReceiptInformationDto.rmmPurchaseUnitCost,
					rmmPurchaseUnitOfMeasure = eRPMfgReceiptInformationDto.rmmPurchaseUnitOfMeasure,
					rmmQuantityCompleted = eRPMfgReceiptInformationDto.rmmQuantityCompleted,
					rmmQuantityOnHand = eRPMfgReceiptInformationDto.rmmQuantityOnHand,
					rmmQuantityReceivedToInventory = eRPMfgReceiptInformationDto.rmmQuantityReceivedToInventory,
					rmmQuantityToInspect = eRPMfgReceiptInformationDto.rmmQuantityToInspect,
					rmmReceiptDate = eRPMfgReceiptInformationDto.rmmReceiptDate,
					rmmReceiptType = eRPMfgReceiptInformationDto.rmmReceiptType,
					rmmReference = eRPMfgReceiptInformationDto.rmmReference,
					rmmReverseMfgReceiptID = eRPMfgReceiptInformationDto.rmmReverseMfgReceiptID,
					rmmRowVersion = eRPMfgReceiptInformationDto.rmmRowVersion,
					rmmScrapQuantity = eRPMfgReceiptInformationDto.rmmScrapQuantity,
					rmmSetupCharge = eRPMfgReceiptInformationDto.rmmSetupCharge,
					rmmSupplierOrganizationID = eRPMfgReceiptInformationDto.rmmSupplierOrganizationID,
					rmmTotalComponentCosts = eRPMfgReceiptInformationDto.rmmTotalComponentCosts,
					rmmTotalUnitCost = eRPMfgReceiptInformationDto.rmmTotalUnitCost,
					rmmUnitLaborCost = eRPMfgReceiptInformationDto.rmmUnitLaborCost,
					rmmUnitMaterialCost = eRPMfgReceiptInformationDto.rmmUnitMaterialCost,
					rmmUnitOverheadCost = eRPMfgReceiptInformationDto.rmmUnitOverheadCost,
					rmmUnitSubcontractCost = eRPMfgReceiptInformationDto.rmmUnitSubcontractCost,
					CustomFields = eRPMfgReceiptInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the MfgReceipts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMfgReceiptDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = mfgReceiptDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMfgReceiptDto>> Process_PutMfgReceipt(ERPMfgReceiptDto mfgReceipt)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPMfgReceiptDto createdObject = null;
		ERPResponseMessageDto<ERPMfgReceiptDto> result;
		try
		{
			IERPMfgReceiptRepository iERPMfgReceiptRepository = (base.ERPMfgReceiptRepository = new ERPMfgReceiptRepository(base.ApiClientContext));
			using (iERPMfgReceiptRepository)
			{
				APIValidationInfoDto postResult = await base.ERPMfgReceiptRepository.SaveMfgReceipt(mfgReceipt);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPMfgReceiptInformationDto eRPMfgReceiptInformationDto = await base.ERPMfgReceiptRepository.GetMfgReceipt(mfgReceipt.rmmUniqueID);
					createdObject = new ERPMfgReceiptDto
					{
						rmmMfgReceiptID = eRPMfgReceiptInformationDto.rmmMfgReceiptID,
						rmmCreatedBy = eRPMfgReceiptInformationDto.rmmCreatedBy,
						rmmCreatedDate = eRPMfgReceiptInformationDto.rmmCreatedDate,
						rmmUniqueID = eRPMfgReceiptInformationDto.rmmUniqueID,
						rmmEstimatedQuantity = eRPMfgReceiptInformationDto.rmmEstimatedQuantity,
						rmmExtendedCostBase = eRPMfgReceiptInformationDto.rmmExtendedCostBase,
						rmmHeatLot = eRPMfgReceiptInformationDto.rmmHeatLot,
						rmmImCostingMethod = eRPMfgReceiptInformationDto.rmmImCostingMethod,
						rmmInventoryQuantity = eRPMfgReceiptInformationDto.rmmInventoryQuantity,
						rmmInventoryQuantityReceived = eRPMfgReceiptInformationDto.rmmInventoryQuantityReceived,
						rmmInventoryUnitOfMeasure = eRPMfgReceiptInformationDto.rmmInventoryUnitOfMeasure,
						rmmCreateJobSeq = eRPMfgReceiptInformationDto.rmmCreateJobSeq,
						rmmInInspection = eRPMfgReceiptInformationDto.rmmInInspection,
						rmmInspectionComplete = eRPMfgReceiptInformationDto.rmmInspectionComplete,
						rmmKitPart = eRPMfgReceiptInformationDto.rmmKitPart,
						rmmNotUpdateJobQtyComplete = eRPMfgReceiptInformationDto.rmmNotUpdateJobQtyComplete,
						rmmPoLineReceivedComplete = eRPMfgReceiptInformationDto.rmmPoLineReceivedComplete,
						rmmPosted = eRPMfgReceiptInformationDto.rmmPosted,
						rmmProductionComplete = eRPMfgReceiptInformationDto.rmmProductionComplete,
						rmmReceivedComplete = eRPMfgReceiptInformationDto.rmmReceivedComplete,
						rmmRequiresInspection = eRPMfgReceiptInformationDto.rmmRequiresInspection,
						rmmReversalEntry = eRPMfgReceiptInformationDto.rmmReversalEntry,
						rmmReversed = eRPMfgReceiptInformationDto.rmmReversed,
						rmmJobAsmQuantityReceived = eRPMfgReceiptInformationDto.rmmJobAsmQuantityReceived,
						rmmJobAssemblyID = eRPMfgReceiptInformationDto.rmmJobAssemblyID,
						rmmJobID = eRPMfgReceiptInformationDto.rmmJobID,
						rmmJobMaterialID = eRPMfgReceiptInformationDto.rmmJobMaterialID,
						rmmJobMatQuantityReceived = eRPMfgReceiptInformationDto.rmmJobMatQuantityReceived,
						rmmJobOpenQuantity = eRPMfgReceiptInformationDto.rmmJobOpenQuantity,
						rmmJobOperationID = eRPMfgReceiptInformationDto.rmmJobOperationID,
						rmmJobOprQuantityReceived = eRPMfgReceiptInformationDto.rmmJobOprQuantityReceived,
						rmmJobScrapQuantity = eRPMfgReceiptInformationDto.rmmJobScrapQuantity,
						rmmJobType = eRPMfgReceiptInformationDto.rmmJobType,
						rmmLongDescriptionRtf = eRPMfgReceiptInformationDto.rmmLongDescriptionRtf,
						rmmLongDescriptionText = eRPMfgReceiptInformationDto.rmmLongDescriptionText,
						rmmMfgCostType = eRPMfgReceiptInformationDto.rmmMfgCostType,
						rmmMiscInvQuantityReceived = eRPMfgReceiptInformationDto.rmmMiscInvQuantityReceived,
						rmmPartBinID = eRPMfgReceiptInformationDto.rmmPartBinID,
						rmmPartID = eRPMfgReceiptInformationDto.rmmPartID,
						rmmPartRevisionID = eRPMfgReceiptInformationDto.rmmPartRevisionID,
						rmmPartWarehouseLocationID = eRPMfgReceiptInformationDto.rmmPartWarehouseLocationID,
						rmmPlantDepartmentID = eRPMfgReceiptInformationDto.rmmPlantDepartmentID,
						rmmPlantID = eRPMfgReceiptInformationDto.rmmPlantID,
						rmmPoOpenQuantity = eRPMfgReceiptInformationDto.rmmPoOpenQuantity,
						rmmPostedDate = eRPMfgReceiptInformationDto.rmmPostedDate,
						rmmProductionQuantity = eRPMfgReceiptInformationDto.rmmProductionQuantity,
						rmmProjectAreaID = eRPMfgReceiptInformationDto.rmmProjectAreaID,
						rmmProjectID = eRPMfgReceiptInformationDto.rmmProjectID,
						rmmPurchaseLocationID = eRPMfgReceiptInformationDto.rmmPurchaseLocationID,
						rmmPurchaseOrderID = eRPMfgReceiptInformationDto.rmmPurchaseOrderID,
						rmmPurchaseOrderLineID = eRPMfgReceiptInformationDto.rmmPurchaseOrderLineID,
						rmmPurchaseQuantity = eRPMfgReceiptInformationDto.rmmPurchaseQuantity,
						rmmPurchaseQuantityReceived = eRPMfgReceiptInformationDto.rmmPurchaseQuantityReceived,
						rmmPurchaseUnitCost = eRPMfgReceiptInformationDto.rmmPurchaseUnitCost,
						rmmPurchaseUnitOfMeasure = eRPMfgReceiptInformationDto.rmmPurchaseUnitOfMeasure,
						rmmQuantityCompleted = eRPMfgReceiptInformationDto.rmmQuantityCompleted,
						rmmQuantityOnHand = eRPMfgReceiptInformationDto.rmmQuantityOnHand,
						rmmQuantityReceivedToInventory = eRPMfgReceiptInformationDto.rmmQuantityReceivedToInventory,
						rmmQuantityToInspect = eRPMfgReceiptInformationDto.rmmQuantityToInspect,
						rmmReceiptDate = eRPMfgReceiptInformationDto.rmmReceiptDate,
						rmmReceiptType = eRPMfgReceiptInformationDto.rmmReceiptType,
						rmmReference = eRPMfgReceiptInformationDto.rmmReference,
						rmmReverseMfgReceiptID = eRPMfgReceiptInformationDto.rmmReverseMfgReceiptID,
						rmmRowVersion = eRPMfgReceiptInformationDto.rmmRowVersion,
						rmmScrapQuantity = eRPMfgReceiptInformationDto.rmmScrapQuantity,
						rmmSetupCharge = eRPMfgReceiptInformationDto.rmmSetupCharge,
						rmmSupplierOrganizationID = eRPMfgReceiptInformationDto.rmmSupplierOrganizationID,
						rmmTotalComponentCosts = eRPMfgReceiptInformationDto.rmmTotalComponentCosts,
						rmmTotalUnitCost = eRPMfgReceiptInformationDto.rmmTotalUnitCost,
						rmmUnitLaborCost = eRPMfgReceiptInformationDto.rmmUnitLaborCost,
						rmmUnitMaterialCost = eRPMfgReceiptInformationDto.rmmUnitMaterialCost,
						rmmUnitOverheadCost = eRPMfgReceiptInformationDto.rmmUnitOverheadCost,
						rmmUnitSubcontractCost = eRPMfgReceiptInformationDto.rmmUnitSubcontractCost,
						CustomFields = eRPMfgReceiptInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing MfgReceipt [{mfgReceipt.rmmUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMfgReceiptDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteMfgReceipt(Guid mfgReceiptId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMfgReceiptRepository iERPMfgReceiptRepository = (base.ERPMfgReceiptRepository = new ERPMfgReceiptRepository(base.ApiClientContext));
		using (iERPMfgReceiptRepository)
		{
			if (!(await base.ERPMfgReceiptRepository.DoesMfgReceiptExist(mfgReceiptId)))
			{
				base.ErrorsList.Add($"MfgReceipt [{mfgReceiptId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPMfgReceiptInformationDto eRPMfgReceiptInformationDto = await base.ERPMfgReceiptRepository.GetMfgReceipt(mfgReceiptId);
				string text = await base.ERPMfgReceiptRepository.WhereUsed("MfgReceipts", new object[1] { eRPMfgReceiptInformationDto.rmmMfgReceiptID }, new object[1] { "rmmMfgReceiptID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("MfgReceipt cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPMfgReceiptDto>> Process_DeleteMfgReceipt(Guid mfgReceiptId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPMfgReceiptDto> result;
		try
		{
			IERPMfgReceiptRepository iERPMfgReceiptRepository = (base.ERPMfgReceiptRepository = new ERPMfgReceiptRepository(base.ApiClientContext));
			using (iERPMfgReceiptRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPMfgReceiptRepository.DeleteRowFromTable("MfgReceipts", "rmm", mfgReceiptId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of MfgReceipt [{mfgReceiptId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMfgReceiptDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPMfgReceiptDto()
			};
		}
		return result;
	}
}
