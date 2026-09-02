using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPInspectionLineModel : ERPBaseModel, IERPInspectionLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllInspectionLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPInspectionLineRepository iERPInspectionLineRepository = (base.ERPInspectionLineRepository = new ERPInspectionLineRepository(base.ApiClientContext));
		using (iERPInspectionLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPInspectionLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPInspectionLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPInspectionLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPInspectionLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetInspectionLine(Guid inspectionLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInspectionLineRepository iERPInspectionLineRepository = (base.ERPInspectionLineRepository = new ERPInspectionLineRepository(base.ApiClientContext));
		using (iERPInspectionLineRepository)
		{
			if (!(await base.ERPInspectionLineRepository.DoesInspectionLineExist(inspectionLineId)))
			{
				errorsList.Add($"InspectionLine [{inspectionLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutInspectionLine(ERPInspectionLineDto inspectionLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInspectionLineRepository iERPInspectionLineRepository = (base.ERPInspectionLineRepository = new ERPInspectionLineRepository(base.ApiClientContext));
		using (iERPInspectionLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(inspectionLine.qalInspectionID) && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("Inspections", new object[1] { "QAPINSPECTIONID" }, new object[1] { inspectionLine.qalInspectionID })))
			{
				errorsList.Add("qalInspectionID [" + inspectionLine.qalInspectionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionLine.qalPartID) && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { inspectionLine.qalPartID })))
			{
				errorsList.Add("qalPartID [" + inspectionLine.qalPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionLine.qalPartRevisionID) && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { inspectionLine.qalPartID, inspectionLine.qalPartRevisionID })))
			{
				errorsList.Add("qalPartRevisionID [" + inspectionLine.qalPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionLine.qalPartWarehouseLocationID) && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { inspectionLine.qalPartID, inspectionLine.qalPartRevisionID, inspectionLine.qalPartWarehouseLocationID })))
			{
				errorsList.Add("qalPartWarehouseLocationID [" + inspectionLine.qalPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionLine.qalPartBinID) && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { inspectionLine.qalPartID, inspectionLine.qalPartRevisionID, inspectionLine.qalPartWarehouseLocationID, inspectionLine.qalPartBinID })))
			{
				errorsList.Add("qalPartBinID [" + inspectionLine.qalPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionLine.qalSupplierOrganizationID) && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { inspectionLine.qalSupplierOrganizationID })))
			{
				errorsList.Add("qalSupplierOrganizationID [" + inspectionLine.qalSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionLine.qalPurchaseLocationID) && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { inspectionLine.qalSupplierOrganizationID, inspectionLine.qalPurchaseLocationID })))
			{
				errorsList.Add("qalPurchaseLocationID [" + inspectionLine.qalPurchaseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionLine.qalScrapReasonID) && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { inspectionLine.qalScrapReasonID })))
			{
				errorsList.Add("qalScrapReasonID [" + inspectionLine.qalScrapReasonID + "] not found.");
			}
			if (inspectionLine.qalPartTransactionID > 0 && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("PartTransactions", new object[1] { "IMTPARTTRANSACTIONID" }, new object[1] { inspectionLine.qalPartTransactionID })))
			{
				errorsList.Add($"qalPartTransactionID [{inspectionLine.qalPartTransactionID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionLine.qalNextApprovalEmployeeID) && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { inspectionLine.qalNextApprovalEmployeeID })))
			{
				errorsList.Add("qalNextApprovalEmployeeID [" + inspectionLine.qalNextApprovalEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionLine.qalProjectID) && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { inspectionLine.qalProjectID })))
			{
				errorsList.Add("qalProjectID [" + inspectionLine.qalProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionLine.qalProjectAreaID) && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { inspectionLine.qalProjectID, inspectionLine.qalProjectAreaID })))
			{
				errorsList.Add("qalProjectAreaID [" + inspectionLine.qalProjectAreaID + "] not found.");
			}
			if (inspectionLine.qalJobMaterialID > 0 && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { inspectionLine.qalJobID, inspectionLine.qalJobAssemblyID, inspectionLine.qalJobMaterialID })))
			{
				errorsList.Add($"qalJobMaterialID [{inspectionLine.qalJobMaterialID}] not found.");
			}
			if (inspectionLine.qalJobOperationID > 0 && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { inspectionLine.qalJobID, inspectionLine.qalJobAssemblyID, inspectionLine.qalJobOperationID })))
			{
				errorsList.Add($"qalJobOperationID [{inspectionLine.qalJobOperationID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionLine.qalJobID) && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { inspectionLine.qalJobID })))
			{
				errorsList.Add("qalJobID [" + inspectionLine.qalJobID + "] not found.");
			}
			if (inspectionLine.qalJobAssemblyID > 0 && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { inspectionLine.qalJobID, inspectionLine.qalJobAssemblyID })))
			{
				errorsList.Add($"qalJobAssemblyID [{inspectionLine.qalJobAssemblyID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionLine.qalInspectorEmployeeID) && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { inspectionLine.qalInspectorEmployeeID })))
			{
				errorsList.Add("qalInspectorEmployeeID [" + inspectionLine.qalInspectorEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionLine.qalReverseInspectionID) && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("Inspections", new object[1] { "QAPINSPECTIONID" }, new object[1] { inspectionLine.qalReverseInspectionID })))
			{
				errorsList.Add("qalReverseInspectionID [" + inspectionLine.qalReverseInspectionID + "] not found.");
			}
			if (inspectionLine.qalReverseInspectionLineID > 0 && !(await base.ERPInspectionLineRepository.DoesRecordExistInTableUsingKeys("InspectionLines", new object[2] { "QALINSPECTIONID", "QALINSPECTIONLINEID" }, new object[2] { inspectionLine.qalReverseInspectionID, inspectionLine.qalReverseInspectionLineID })))
			{
				errorsList.Add($"qalReverseInspectionLineID [{inspectionLine.qalReverseInspectionLineID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPInspectionLineDto>>> Process_GetAllInspectionLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPInspectionLineDto> allInspectionLinesDto = new List<ERPInspectionLineDto>();
		ERPResponseMessageDto<IList<ERPInspectionLineDto>> result;
		try
		{
			IERPInspectionLineRepository iERPInspectionLineRepository = (base.ERPInspectionLineRepository = new ERPInspectionLineRepository(base.ApiClientContext));
			using (iERPInspectionLineRepository)
			{
				foreach (ERPInspectionLineInformationDto item2 in await base.ERPInspectionLineRepository.GetAllInspectionLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPInspectionLineDto item = new ERPInspectionLineDto
					{
						qalActionType = item2.qalActionType,
						qalApprovalDecisionDate = item2.qalApprovalDecisionDate,
						qalApprovalRequestDate = item2.qalApprovalRequestDate,
						qalApprovalStatus = item2.qalApprovalStatus,
						qalClosedDate = item2.qalClosedDate,
						qalCreatedBy = item2.qalCreatedBy,
						qalCreatedDate = item2.qalCreatedDate,
						qalUniqueID = item2.qalUniqueID,
						qalInspectionDate = item2.qalInspectionDate,
						qalInspectionID = item2.qalInspectionID,
						qalInspectionNotesRTF = item2.qalInspectionNotesRTF,
						qalInspectionNotesText = item2.qalInspectionNotesText,
						qalInspectionType = item2.qalInspectionType,
						qalInspectorEmployeeID = item2.qalInspectorEmployeeID,
						qalInvQuantityAccepted = item2.qalInvQuantityAccepted,
						qalInvQuantityToReturn = item2.qalInvQuantityToReturn,
						qalInvQuantityToScrap = item2.qalInvQuantityToScrap,
						qalFirstOffInspection = item2.qalFirstOffInspection,
						qalInspectionComplete = item2.qalInspectionComplete,
						qalKitPart = item2.qalKitPart,
						qalManualInspectionFinalized = item2.qalManualInspectionFinalized,
						qalPosted = item2.qalPosted,
						qalReturnToSupplier = item2.qalReturnToSupplier,
						qalReversed = item2.qalReversed,
						qalTransferredToDmr = item2.qalTransferredToDmr,
						qalJobAssemblyID = item2.qalJobAssemblyID,
						qalJobID = item2.qalJobID,
						qalJobMaterialID = item2.qalJobMaterialID,
						qalJobMatQuantityAccepted = item2.qalJobMatQuantityAccepted,
						qalJobMatQuantityRejected = item2.qalJobMatQuantityRejected,
						qalJobMatQuantityToReturn = item2.qalJobMatQuantityToReturn,
						qalJobMatQuantityToScrap = item2.qalJobMatQuantityToScrap,
						qalJobOperationID = item2.qalJobOperationID,
						qalJobOprQuantityAccepted = item2.qalJobOprQuantityAccepted,
						qalJobOprQuantityRejected = item2.qalJobOprQuantityRejected,
						qalJobOprQuantityToReturn = item2.qalJobOprQuantityToReturn,
						qalJobOprQuantityToScrap = item2.qalJobOprQuantityToScrap,
						qalJobType = item2.qalJobType,
						qalMfgReceiptQuantityAccepted = item2.qalMfgReceiptQuantityAccepted,
						qalMfgReceiptQuantityToReturn = item2.qalMfgReceiptQuantityToReturn,
						qalMfgReceiptQuantityToScrap = item2.qalMfgReceiptQuantityToScrap,
						qalNextApprovalEmployeeID = item2.qalNextApprovalEmployeeID,
						qalPartBinID = item2.qalPartBinID,
						qalPartID = item2.qalPartID,
						qalPartLongDescriptionRtf = item2.qalPartLongDescriptionRtf,
						qalPartLongDescriptionText = item2.qalPartLongDescriptionText,
						qalPartRevisionID = item2.qalPartRevisionID,
						qalPartShortDescription = item2.qalPartShortDescription,
						qalPartTransactionID = item2.qalPartTransactionID,
						qalPartWarehouseLocationID = item2.qalPartWarehouseLocationID,
						qalProjectAreaID = item2.qalProjectAreaID,
						qalProjectID = item2.qalProjectID,
						qalPurchaseLocationID = item2.qalPurchaseLocationID,
						qalQuantityRejected = item2.qalQuantityRejected,
						qalQuantityToInspect = item2.qalQuantityToInspect,
						qalReverseInspectionID = item2.qalReverseInspectionID,
						qalReverseInspectionLineID = item2.qalReverseInspectionLineID,
						qalScrapReasonID = item2.qalScrapReasonID,
						qalInspectionLineID = item2.qalInspectionLineID,
						qalSourceTableName = item2.qalSourceTableName,
						qalSourceTableUniqueID = item2.qalSourceTableUniqueID,
						qalStatus = item2.qalStatus,
						qalSupplierOrganizationID = item2.qalSupplierOrganizationID,
						qalUnitCost = item2.qalUnitCost,
						qalUnitOfMeasure = item2.qalUnitOfMeasure,
						CustomFields = item2.CustomFields
					};
					allInspectionLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all InspectionLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPInspectionLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allInspectionLinesDto,
				RecordCount = allInspectionLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPInspectionLineDto>> Process_GetInspectionLine(Guid inspectionLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPInspectionLineDto inspectionLineDto = null;
		ERPResponseMessageDto<ERPInspectionLineDto> result;
		try
		{
			IERPInspectionLineRepository iERPInspectionLineRepository = (base.ERPInspectionLineRepository = new ERPInspectionLineRepository(base.ApiClientContext));
			using (iERPInspectionLineRepository)
			{
				ERPInspectionLineInformationDto eRPInspectionLineInformationDto = await base.ERPInspectionLineRepository.GetInspectionLine(inspectionLineId);
				inspectionLineDto = new ERPInspectionLineDto
				{
					qalActionType = eRPInspectionLineInformationDto.qalActionType,
					qalApprovalDecisionDate = eRPInspectionLineInformationDto.qalApprovalDecisionDate,
					qalApprovalRequestDate = eRPInspectionLineInformationDto.qalApprovalRequestDate,
					qalApprovalStatus = eRPInspectionLineInformationDto.qalApprovalStatus,
					qalClosedDate = eRPInspectionLineInformationDto.qalClosedDate,
					qalCreatedBy = eRPInspectionLineInformationDto.qalCreatedBy,
					qalCreatedDate = eRPInspectionLineInformationDto.qalCreatedDate,
					qalUniqueID = eRPInspectionLineInformationDto.qalUniqueID,
					qalInspectionDate = eRPInspectionLineInformationDto.qalInspectionDate,
					qalInspectionID = eRPInspectionLineInformationDto.qalInspectionID,
					qalInspectionNotesRTF = eRPInspectionLineInformationDto.qalInspectionNotesRTF,
					qalInspectionNotesText = eRPInspectionLineInformationDto.qalInspectionNotesText,
					qalInspectionType = eRPInspectionLineInformationDto.qalInspectionType,
					qalInspectorEmployeeID = eRPInspectionLineInformationDto.qalInspectorEmployeeID,
					qalInvQuantityAccepted = eRPInspectionLineInformationDto.qalInvQuantityAccepted,
					qalInvQuantityToReturn = eRPInspectionLineInformationDto.qalInvQuantityToReturn,
					qalInvQuantityToScrap = eRPInspectionLineInformationDto.qalInvQuantityToScrap,
					qalFirstOffInspection = eRPInspectionLineInformationDto.qalFirstOffInspection,
					qalInspectionComplete = eRPInspectionLineInformationDto.qalInspectionComplete,
					qalKitPart = eRPInspectionLineInformationDto.qalKitPart,
					qalManualInspectionFinalized = eRPInspectionLineInformationDto.qalManualInspectionFinalized,
					qalPosted = eRPInspectionLineInformationDto.qalPosted,
					qalReturnToSupplier = eRPInspectionLineInformationDto.qalReturnToSupplier,
					qalReversed = eRPInspectionLineInformationDto.qalReversed,
					qalTransferredToDmr = eRPInspectionLineInformationDto.qalTransferredToDmr,
					qalJobAssemblyID = eRPInspectionLineInformationDto.qalJobAssemblyID,
					qalJobID = eRPInspectionLineInformationDto.qalJobID,
					qalJobMaterialID = eRPInspectionLineInformationDto.qalJobMaterialID,
					qalJobMatQuantityAccepted = eRPInspectionLineInformationDto.qalJobMatQuantityAccepted,
					qalJobMatQuantityRejected = eRPInspectionLineInformationDto.qalJobMatQuantityRejected,
					qalJobMatQuantityToReturn = eRPInspectionLineInformationDto.qalJobMatQuantityToReturn,
					qalJobMatQuantityToScrap = eRPInspectionLineInformationDto.qalJobMatQuantityToScrap,
					qalJobOperationID = eRPInspectionLineInformationDto.qalJobOperationID,
					qalJobOprQuantityAccepted = eRPInspectionLineInformationDto.qalJobOprQuantityAccepted,
					qalJobOprQuantityRejected = eRPInspectionLineInformationDto.qalJobOprQuantityRejected,
					qalJobOprQuantityToReturn = eRPInspectionLineInformationDto.qalJobOprQuantityToReturn,
					qalJobOprQuantityToScrap = eRPInspectionLineInformationDto.qalJobOprQuantityToScrap,
					qalJobType = eRPInspectionLineInformationDto.qalJobType,
					qalMfgReceiptQuantityAccepted = eRPInspectionLineInformationDto.qalMfgReceiptQuantityAccepted,
					qalMfgReceiptQuantityToReturn = eRPInspectionLineInformationDto.qalMfgReceiptQuantityToReturn,
					qalMfgReceiptQuantityToScrap = eRPInspectionLineInformationDto.qalMfgReceiptQuantityToScrap,
					qalNextApprovalEmployeeID = eRPInspectionLineInformationDto.qalNextApprovalEmployeeID,
					qalPartBinID = eRPInspectionLineInformationDto.qalPartBinID,
					qalPartID = eRPInspectionLineInformationDto.qalPartID,
					qalPartLongDescriptionRtf = eRPInspectionLineInformationDto.qalPartLongDescriptionRtf,
					qalPartLongDescriptionText = eRPInspectionLineInformationDto.qalPartLongDescriptionText,
					qalPartRevisionID = eRPInspectionLineInformationDto.qalPartRevisionID,
					qalPartShortDescription = eRPInspectionLineInformationDto.qalPartShortDescription,
					qalPartTransactionID = eRPInspectionLineInformationDto.qalPartTransactionID,
					qalPartWarehouseLocationID = eRPInspectionLineInformationDto.qalPartWarehouseLocationID,
					qalProjectAreaID = eRPInspectionLineInformationDto.qalProjectAreaID,
					qalProjectID = eRPInspectionLineInformationDto.qalProjectID,
					qalPurchaseLocationID = eRPInspectionLineInformationDto.qalPurchaseLocationID,
					qalQuantityRejected = eRPInspectionLineInformationDto.qalQuantityRejected,
					qalQuantityToInspect = eRPInspectionLineInformationDto.qalQuantityToInspect,
					qalReverseInspectionID = eRPInspectionLineInformationDto.qalReverseInspectionID,
					qalReverseInspectionLineID = eRPInspectionLineInformationDto.qalReverseInspectionLineID,
					qalScrapReasonID = eRPInspectionLineInformationDto.qalScrapReasonID,
					qalInspectionLineID = eRPInspectionLineInformationDto.qalInspectionLineID,
					qalSourceTableName = eRPInspectionLineInformationDto.qalSourceTableName,
					qalSourceTableUniqueID = eRPInspectionLineInformationDto.qalSourceTableUniqueID,
					qalStatus = eRPInspectionLineInformationDto.qalStatus,
					qalSupplierOrganizationID = eRPInspectionLineInformationDto.qalSupplierOrganizationID,
					qalUnitCost = eRPInspectionLineInformationDto.qalUnitCost,
					qalUnitOfMeasure = eRPInspectionLineInformationDto.qalUnitOfMeasure,
					CustomFields = eRPInspectionLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the InspectionLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInspectionLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = inspectionLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPInspectionLineDto>> Process_PutInspectionLine(ERPInspectionLineDto inspectionLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPInspectionLineDto createdObject = null;
		ERPResponseMessageDto<ERPInspectionLineDto> result;
		try
		{
			IERPInspectionLineRepository iERPInspectionLineRepository = (base.ERPInspectionLineRepository = new ERPInspectionLineRepository(base.ApiClientContext));
			using (iERPInspectionLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPInspectionLineRepository.SaveInspectionLine(inspectionLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPInspectionLineInformationDto eRPInspectionLineInformationDto = await base.ERPInspectionLineRepository.GetInspectionLine(inspectionLine.qalUniqueID);
					createdObject = new ERPInspectionLineDto
					{
						qalActionType = eRPInspectionLineInformationDto.qalActionType,
						qalApprovalDecisionDate = eRPInspectionLineInformationDto.qalApprovalDecisionDate,
						qalApprovalRequestDate = eRPInspectionLineInformationDto.qalApprovalRequestDate,
						qalApprovalStatus = eRPInspectionLineInformationDto.qalApprovalStatus,
						qalClosedDate = eRPInspectionLineInformationDto.qalClosedDate,
						qalCreatedBy = eRPInspectionLineInformationDto.qalCreatedBy,
						qalCreatedDate = eRPInspectionLineInformationDto.qalCreatedDate,
						qalUniqueID = eRPInspectionLineInformationDto.qalUniqueID,
						qalInspectionDate = eRPInspectionLineInformationDto.qalInspectionDate,
						qalInspectionID = eRPInspectionLineInformationDto.qalInspectionID,
						qalInspectionNotesRTF = eRPInspectionLineInformationDto.qalInspectionNotesRTF,
						qalInspectionNotesText = eRPInspectionLineInformationDto.qalInspectionNotesText,
						qalInspectionType = eRPInspectionLineInformationDto.qalInspectionType,
						qalInspectorEmployeeID = eRPInspectionLineInformationDto.qalInspectorEmployeeID,
						qalInvQuantityAccepted = eRPInspectionLineInformationDto.qalInvQuantityAccepted,
						qalInvQuantityToReturn = eRPInspectionLineInformationDto.qalInvQuantityToReturn,
						qalInvQuantityToScrap = eRPInspectionLineInformationDto.qalInvQuantityToScrap,
						qalFirstOffInspection = eRPInspectionLineInformationDto.qalFirstOffInspection,
						qalInspectionComplete = eRPInspectionLineInformationDto.qalInspectionComplete,
						qalKitPart = eRPInspectionLineInformationDto.qalKitPart,
						qalManualInspectionFinalized = eRPInspectionLineInformationDto.qalManualInspectionFinalized,
						qalPosted = eRPInspectionLineInformationDto.qalPosted,
						qalReturnToSupplier = eRPInspectionLineInformationDto.qalReturnToSupplier,
						qalReversed = eRPInspectionLineInformationDto.qalReversed,
						qalTransferredToDmr = eRPInspectionLineInformationDto.qalTransferredToDmr,
						qalJobAssemblyID = eRPInspectionLineInformationDto.qalJobAssemblyID,
						qalJobID = eRPInspectionLineInformationDto.qalJobID,
						qalJobMaterialID = eRPInspectionLineInformationDto.qalJobMaterialID,
						qalJobMatQuantityAccepted = eRPInspectionLineInformationDto.qalJobMatQuantityAccepted,
						qalJobMatQuantityRejected = eRPInspectionLineInformationDto.qalJobMatQuantityRejected,
						qalJobMatQuantityToReturn = eRPInspectionLineInformationDto.qalJobMatQuantityToReturn,
						qalJobMatQuantityToScrap = eRPInspectionLineInformationDto.qalJobMatQuantityToScrap,
						qalJobOperationID = eRPInspectionLineInformationDto.qalJobOperationID,
						qalJobOprQuantityAccepted = eRPInspectionLineInformationDto.qalJobOprQuantityAccepted,
						qalJobOprQuantityRejected = eRPInspectionLineInformationDto.qalJobOprQuantityRejected,
						qalJobOprQuantityToReturn = eRPInspectionLineInformationDto.qalJobOprQuantityToReturn,
						qalJobOprQuantityToScrap = eRPInspectionLineInformationDto.qalJobOprQuantityToScrap,
						qalJobType = eRPInspectionLineInformationDto.qalJobType,
						qalMfgReceiptQuantityAccepted = eRPInspectionLineInformationDto.qalMfgReceiptQuantityAccepted,
						qalMfgReceiptQuantityToReturn = eRPInspectionLineInformationDto.qalMfgReceiptQuantityToReturn,
						qalMfgReceiptQuantityToScrap = eRPInspectionLineInformationDto.qalMfgReceiptQuantityToScrap,
						qalNextApprovalEmployeeID = eRPInspectionLineInformationDto.qalNextApprovalEmployeeID,
						qalPartBinID = eRPInspectionLineInformationDto.qalPartBinID,
						qalPartID = eRPInspectionLineInformationDto.qalPartID,
						qalPartLongDescriptionRtf = eRPInspectionLineInformationDto.qalPartLongDescriptionRtf,
						qalPartLongDescriptionText = eRPInspectionLineInformationDto.qalPartLongDescriptionText,
						qalPartRevisionID = eRPInspectionLineInformationDto.qalPartRevisionID,
						qalPartShortDescription = eRPInspectionLineInformationDto.qalPartShortDescription,
						qalPartTransactionID = eRPInspectionLineInformationDto.qalPartTransactionID,
						qalPartWarehouseLocationID = eRPInspectionLineInformationDto.qalPartWarehouseLocationID,
						qalProjectAreaID = eRPInspectionLineInformationDto.qalProjectAreaID,
						qalProjectID = eRPInspectionLineInformationDto.qalProjectID,
						qalPurchaseLocationID = eRPInspectionLineInformationDto.qalPurchaseLocationID,
						qalQuantityRejected = eRPInspectionLineInformationDto.qalQuantityRejected,
						qalQuantityToInspect = eRPInspectionLineInformationDto.qalQuantityToInspect,
						qalReverseInspectionID = eRPInspectionLineInformationDto.qalReverseInspectionID,
						qalReverseInspectionLineID = eRPInspectionLineInformationDto.qalReverseInspectionLineID,
						qalScrapReasonID = eRPInspectionLineInformationDto.qalScrapReasonID,
						qalInspectionLineID = eRPInspectionLineInformationDto.qalInspectionLineID,
						qalSourceTableName = eRPInspectionLineInformationDto.qalSourceTableName,
						qalSourceTableUniqueID = eRPInspectionLineInformationDto.qalSourceTableUniqueID,
						qalStatus = eRPInspectionLineInformationDto.qalStatus,
						qalSupplierOrganizationID = eRPInspectionLineInformationDto.qalSupplierOrganizationID,
						qalUnitCost = eRPInspectionLineInformationDto.qalUnitCost,
						qalUnitOfMeasure = eRPInspectionLineInformationDto.qalUnitOfMeasure,
						CustomFields = eRPInspectionLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing InspectionLine [{inspectionLine.qalUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInspectionLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteInspectionLine(Guid inspectionLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInspectionLineRepository iERPInspectionLineRepository = (base.ERPInspectionLineRepository = new ERPInspectionLineRepository(base.ApiClientContext));
		using (iERPInspectionLineRepository)
		{
			if (!(await base.ERPInspectionLineRepository.DoesInspectionLineExist(inspectionLineId)))
			{
				base.ErrorsList.Add($"InspectionLine [{inspectionLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPInspectionLineInformationDto eRPInspectionLineInformationDto = await base.ERPInspectionLineRepository.GetInspectionLine(inspectionLineId);
				string text = await base.ERPInspectionLineRepository.WhereUsed("InspectionLines", new object[2] { eRPInspectionLineInformationDto.qalInspectionID, eRPInspectionLineInformationDto.qalInspectionLineID }, new object[2] { "qalInspectionID", "qalInspectionLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("InspectionLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPInspectionLineDto>> Process_DeleteInspectionLine(Guid inspectionLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPInspectionLineDto> result;
		try
		{
			IERPInspectionLineRepository iERPInspectionLineRepository = (base.ERPInspectionLineRepository = new ERPInspectionLineRepository(base.ApiClientContext));
			using (iERPInspectionLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPInspectionLineRepository.DeleteRowFromTable("InspectionLines", "qal", inspectionLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of InspectionLine [{inspectionLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInspectionLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPInspectionLineDto()
			};
		}
		return result;
	}
}
