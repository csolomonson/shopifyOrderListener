using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPJobOperationModel : ERPBaseModel, IERPJobOperationModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllJobOperations(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPJobOperationRepository iERPJobOperationRepository = (base.ERPJobOperationRepository = new ERPJobOperationRepository(base.ApiClientContext));
		using (iERPJobOperationRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPJobOperationRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPJobOperationRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPJobOperationRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPJobOperationRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetJobOperation(Guid jobOperationId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobOperationRepository iERPJobOperationRepository = (base.ERPJobOperationRepository = new ERPJobOperationRepository(base.ApiClientContext));
		using (iERPJobOperationRepository)
		{
			if (!(await base.ERPJobOperationRepository.DoesJobOperationExist(jobOperationId)))
			{
				errorsList.Add($"JobOperation [{jobOperationId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutJobOperation(ERPJobOperationDto jobOperation)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobOperationRepository iERPJobOperationRepository = (base.ERPJobOperationRepository = new ERPJobOperationRepository(base.ApiClientContext));
		using (iERPJobOperationRepository)
		{
			if (!string.IsNullOrWhiteSpace(jobOperation.jmoJobID) && !(await base.ERPJobOperationRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { jobOperation.jmoJobID })))
			{
				errorsList.Add("jmoJobID [" + jobOperation.jmoJobID + "] not found.");
			}
			if (jobOperation.jmoJobAssemblyID > 0 && !(await base.ERPJobOperationRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { jobOperation.jmoJobID, jobOperation.jmoJobAssemblyID })))
			{
				errorsList.Add($"jmoJobAssemblyID [{jobOperation.jmoJobAssemblyID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobOperation.jmoPlantDepartmentID) && !(await base.ERPJobOperationRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { jobOperation.jmoPlantID, jobOperation.jmoPlantDepartmentID })))
			{
				errorsList.Add("jmoPlantDepartmentID [" + jobOperation.jmoPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobOperation.jmoPlantID) && !(await base.ERPJobOperationRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { jobOperation.jmoPlantID })))
			{
				errorsList.Add("jmoPlantID [" + jobOperation.jmoPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobOperation.jmoWorkCenterID) && !(await base.ERPJobOperationRepository.DoesRecordExistInTableUsingKeys("WorkCenters", new object[1] { "XAWWORKCENTERID" }, new object[1] { jobOperation.jmoWorkCenterID })))
			{
				errorsList.Add("jmoWorkCenterID [" + jobOperation.jmoWorkCenterID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobOperation.jmoProcessID) && !(await base.ERPJobOperationRepository.DoesRecordExistInTableUsingKeys("Processes", new object[1] { "XACPROCESSID" }, new object[1] { jobOperation.jmoProcessID })))
			{
				errorsList.Add("jmoProcessID [" + jobOperation.jmoProcessID + "] not found.");
			}
			if (jobOperation.jmoOverlapOperationID > 0 && !(await base.ERPJobOperationRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { jobOperation.jmoJobID, jobOperation.jmoJobAssemblyID, jobOperation.jmoOverlapOperationID })))
			{
				errorsList.Add($"jmoOverlapOperationID [{jobOperation.jmoOverlapOperationID}] not found.");
			}
			if (jobOperation.jmoWorkCenterMachineID > 0 && !(await base.ERPJobOperationRepository.DoesRecordExistInTableUsingKeys("WorkCenterMachines", new object[2] { "XAQWORKCENTERID", "XAQWORKCENTERMACHINEID" }, new object[2] { jobOperation.jmoWorkCenterID, jobOperation.jmoWorkCenterMachineID })))
			{
				errorsList.Add($"jmoWorkCenterMachineID [{jobOperation.jmoWorkCenterMachineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobOperation.jmoPartID) && !(await base.ERPJobOperationRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { jobOperation.jmoPartID })))
			{
				errorsList.Add("jmoPartID [" + jobOperation.jmoPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobOperation.jmoPartRevisionID) && !(await base.ERPJobOperationRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { jobOperation.jmoPartID, jobOperation.jmoPartRevisionID })))
			{
				errorsList.Add("jmoPartRevisionID [" + jobOperation.jmoPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobOperation.jmoPartWarehouseLocationID) && !(await base.ERPJobOperationRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { jobOperation.jmoPartID, jobOperation.jmoPartRevisionID, jobOperation.jmoPartWarehouseLocationID })))
			{
				errorsList.Add("jmoPartWarehouseLocationID [" + jobOperation.jmoPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobOperation.jmoPartBinID) && !(await base.ERPJobOperationRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { jobOperation.jmoPartID, jobOperation.jmoPartRevisionID, jobOperation.jmoPartWarehouseLocationID, jobOperation.jmoPartBinID })))
			{
				errorsList.Add("jmoPartBinID [" + jobOperation.jmoPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobOperation.jmoSupplierOrganizationID) && !(await base.ERPJobOperationRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { jobOperation.jmoSupplierOrganizationID })))
			{
				errorsList.Add("jmoSupplierOrganizationID [" + jobOperation.jmoSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobOperation.jmoPurchaseLocationID) && !(await base.ERPJobOperationRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { jobOperation.jmoSupplierOrganizationID, jobOperation.jmoPurchaseLocationID })))
			{
				errorsList.Add("jmoPurchaseLocationID [" + jobOperation.jmoPurchaseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobOperation.jmoPurchaseOrderID) && !(await base.ERPJobOperationRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { jobOperation.jmoPurchaseOrderID })))
			{
				errorsList.Add("jmoPurchaseOrderID [" + jobOperation.jmoPurchaseOrderID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobOperation.jmoRfqID) && !(await base.ERPJobOperationRepository.DoesRecordExistInTableUsingKeys("RFQs", new object[1] { "RQPRFQID" }, new object[1] { jobOperation.jmoRfqID })))
			{
				errorsList.Add("jmoRfqID [" + jobOperation.jmoRfqID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPJobOperationDto>>> Process_GetAllJobOperations(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPJobOperationDto> allJobOperationsDto = new List<ERPJobOperationDto>();
		ERPResponseMessageDto<IList<ERPJobOperationDto>> result;
		try
		{
			IERPJobOperationRepository iERPJobOperationRepository = (base.ERPJobOperationRepository = new ERPJobOperationRepository(base.ApiClientContext));
			using (iERPJobOperationRepository)
			{
				foreach (ERPJobOperationInformationDto item2 in await base.ERPJobOperationRepository.GetAllJobOperations(pageSize, pageNumber, filter, orderBy))
				{
					ERPJobOperationDto item = new ERPJobOperationDto
					{
						jmoActualProductionHours = item2.jmoActualProductionHours,
						jmoActualSetupHours = item2.jmoActualSetupHours,
						jmoCalculatedUnitCost = item2.jmoCalculatedUnitCost,
						jmoCompletedProductionHours = item2.jmoCompletedProductionHours,
						jmoCompletedSetupHours = item2.jmoCompletedSetupHours,
						jmoCreatedBy = item2.jmoCreatedBy,
						jmoCreatedDate = item2.jmoCreatedDate,
						jmoDocuments = item2.jmoDocuments,
						jmoDueDate = item2.jmoDueDate,
						jmoDueHour = item2.jmoDueHour,
						jmoUniqueID = item2.jmoUniqueID,
						jmoEstimatedProductionHours = item2.jmoEstimatedProductionHours,
						jmoEstimatedUnitCost = item2.jmoEstimatedUnitCost,
						jmoInspectionStatus = item2.jmoInspectionStatus,
						jmoInspectionType = item2.jmoInspectionType,
						jmoAddedOperation = item2.jmoAddedOperation,
						jmoClosed = item2.jmoClosed,
						jmoFirm = item2.jmoFirm,
						jmoInspectionComplete = item2.jmoInspectionComplete,
						jmoProductionComplete = item2.jmoProductionComplete,
						jmoPrototypeOperation = item2.jmoPrototypeOperation,
						jmoSetupComplete = item2.jmoSetupComplete,
						jmoJobAssemblyID = item2.jmoJobAssemblyID,
						jmoJobID = item2.jmoJobID,
						jmoMachinesToSchedule = item2.jmoMachinesToSchedule,
						jmoMachineType = item2.jmoMachineType,
						jmoMinimumCharge = item2.jmoMinimumCharge,
						jmoMoveTime = item2.jmoMoveTime,
						jmoOperationQuantity = item2.jmoOperationQuantity,
						jmoOperationType = item2.jmoOperationType,
						jmoOverheadRate = item2.jmoOverheadRate,
						jmoOverlap = item2.jmoOverlap,
						jmoOverlapDestinationLink = item2.jmoOverlapDestinationLink,
						jmoOverlapOffsetTime = item2.jmoOverlapOffsetTime,
						jmoOverlapOperationID = item2.jmoOverlapOperationID,
						jmoOverlapSourceLink = item2.jmoOverlapSourceLink,
						jmoPartBinID = item2.jmoPartBinID,
						jmoPartID = item2.jmoPartID,
						jmoPartRevisionID = item2.jmoPartRevisionID,
						jmoPartWarehouseLocationID = item2.jmoPartWarehouseLocationID,
						jmoPlantDepartmentID = item2.jmoPlantDepartmentID,
						jmoPlantID = item2.jmoPlantID,
						jmoProcessID = item2.jmoProcessID,
						jmoProcessLongDescriptionRtf = item2.jmoProcessLongDescriptionRtf,
						jmoProcessLongDescriptionText = item2.jmoProcessLongDescriptionText,
						jmoProcessShortDescription = item2.jmoProcessShortDescription,
						jmoProductionRate = item2.jmoProductionRate,
						jmoProductionStandard = item2.jmoProductionStandard,
						jmoPurchaseLocationID = item2.jmoPurchaseLocationID,
						jmoPurchaseOrderID = item2.jmoPurchaseOrderID,
						jmoQuantityBreak1 = item2.jmoQuantityBreak1,
						jmoQuantityBreak2 = item2.jmoQuantityBreak2,
						jmoQuantityBreak3 = item2.jmoQuantityBreak3,
						jmoQuantityBreak4 = item2.jmoQuantityBreak4,
						jmoQuantityBreak5 = item2.jmoQuantityBreak5,
						jmoQuantityBreak6 = item2.jmoQuantityBreak6,
						jmoQuantityBreak7 = item2.jmoQuantityBreak7,
						jmoQuantityBreak8 = item2.jmoQuantityBreak8,
						jmoQuantityBreak9 = item2.jmoQuantityBreak9,
						jmoQuantityComplete = item2.jmoQuantityComplete,
						jmoQuantityPerAssembly = item2.jmoQuantityPerAssembly,
						jmoQuantityToInspect = item2.jmoQuantityToInspect,
						jmoQuantityToReturn = item2.jmoQuantityToReturn,
						jmoQueueTime = item2.jmoQueueTime,
						jmoRfqID = item2.jmoRfqID,
						jmoRowVersion = item2.jmoRowVersion,
						jmoScrapQuantityReceived = item2.jmoScrapQuantityReceived,
						jmoJobOperationID = item2.jmoJobOperationID,
						jmoSetupCharge = item2.jmoSetupCharge,
						jmoSetupHours = item2.jmoSetupHours,
						jmoSetupPercentComplete = item2.jmoSetupPercentComplete,
						jmoSetupRate = item2.jmoSetupRate,
						jmoSfeMessageRTF = item2.jmoSfeMessageRTF,
						jmoSfeMessageText = item2.jmoSfeMessageText,
						jmoStandardFactor = item2.jmoStandardFactor,
						jmoStartDate = item2.jmoStartDate,
						jmoStartHour = item2.jmoStartHour,
						jmoSupplierOrganizationID = item2.jmoSupplierOrganizationID,
						jmoUnitCost1 = item2.jmoUnitCost1,
						jmoUnitCost2 = item2.jmoUnitCost2,
						jmoUnitCost3 = item2.jmoUnitCost3,
						jmoUnitCost4 = item2.jmoUnitCost4,
						jmoUnitCost5 = item2.jmoUnitCost5,
						jmoUnitCost6 = item2.jmoUnitCost6,
						jmoUnitCost7 = item2.jmoUnitCost7,
						jmoUnitCost8 = item2.jmoUnitCost8,
						jmoUnitCost9 = item2.jmoUnitCost9,
						jmoUnitOfMeasure = item2.jmoUnitOfMeasure,
						jmoWorkCenterID = item2.jmoWorkCenterID,
						jmoWorkCenterMachineID = item2.jmoWorkCenterMachineID,
						CustomFields = item2.CustomFields
					};
					allJobOperationsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all JobOperations]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPJobOperationDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allJobOperationsDto,
				RecordCount = allJobOperationsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobOperationDto>> Process_GetJobOperation(Guid jobOperationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPJobOperationDto jobOperationDto = null;
		ERPResponseMessageDto<ERPJobOperationDto> result;
		try
		{
			IERPJobOperationRepository iERPJobOperationRepository = (base.ERPJobOperationRepository = new ERPJobOperationRepository(base.ApiClientContext));
			using (iERPJobOperationRepository)
			{
				ERPJobOperationInformationDto eRPJobOperationInformationDto = await base.ERPJobOperationRepository.GetJobOperation(jobOperationId);
				jobOperationDto = new ERPJobOperationDto
				{
					jmoActualProductionHours = eRPJobOperationInformationDto.jmoActualProductionHours,
					jmoActualSetupHours = eRPJobOperationInformationDto.jmoActualSetupHours,
					jmoCalculatedUnitCost = eRPJobOperationInformationDto.jmoCalculatedUnitCost,
					jmoCompletedProductionHours = eRPJobOperationInformationDto.jmoCompletedProductionHours,
					jmoCompletedSetupHours = eRPJobOperationInformationDto.jmoCompletedSetupHours,
					jmoCreatedBy = eRPJobOperationInformationDto.jmoCreatedBy,
					jmoCreatedDate = eRPJobOperationInformationDto.jmoCreatedDate,
					jmoDocuments = eRPJobOperationInformationDto.jmoDocuments,
					jmoDueDate = eRPJobOperationInformationDto.jmoDueDate,
					jmoDueHour = eRPJobOperationInformationDto.jmoDueHour,
					jmoUniqueID = eRPJobOperationInformationDto.jmoUniqueID,
					jmoEstimatedProductionHours = eRPJobOperationInformationDto.jmoEstimatedProductionHours,
					jmoEstimatedUnitCost = eRPJobOperationInformationDto.jmoEstimatedUnitCost,
					jmoInspectionStatus = eRPJobOperationInformationDto.jmoInspectionStatus,
					jmoInspectionType = eRPJobOperationInformationDto.jmoInspectionType,
					jmoAddedOperation = eRPJobOperationInformationDto.jmoAddedOperation,
					jmoClosed = eRPJobOperationInformationDto.jmoClosed,
					jmoFirm = eRPJobOperationInformationDto.jmoFirm,
					jmoInspectionComplete = eRPJobOperationInformationDto.jmoInspectionComplete,
					jmoProductionComplete = eRPJobOperationInformationDto.jmoProductionComplete,
					jmoPrototypeOperation = eRPJobOperationInformationDto.jmoPrototypeOperation,
					jmoSetupComplete = eRPJobOperationInformationDto.jmoSetupComplete,
					jmoJobAssemblyID = eRPJobOperationInformationDto.jmoJobAssemblyID,
					jmoJobID = eRPJobOperationInformationDto.jmoJobID,
					jmoMachinesToSchedule = eRPJobOperationInformationDto.jmoMachinesToSchedule,
					jmoMachineType = eRPJobOperationInformationDto.jmoMachineType,
					jmoMinimumCharge = eRPJobOperationInformationDto.jmoMinimumCharge,
					jmoMoveTime = eRPJobOperationInformationDto.jmoMoveTime,
					jmoOperationQuantity = eRPJobOperationInformationDto.jmoOperationQuantity,
					jmoOperationType = eRPJobOperationInformationDto.jmoOperationType,
					jmoOverheadRate = eRPJobOperationInformationDto.jmoOverheadRate,
					jmoOverlap = eRPJobOperationInformationDto.jmoOverlap,
					jmoOverlapDestinationLink = eRPJobOperationInformationDto.jmoOverlapDestinationLink,
					jmoOverlapOffsetTime = eRPJobOperationInformationDto.jmoOverlapOffsetTime,
					jmoOverlapOperationID = eRPJobOperationInformationDto.jmoOverlapOperationID,
					jmoOverlapSourceLink = eRPJobOperationInformationDto.jmoOverlapSourceLink,
					jmoPartBinID = eRPJobOperationInformationDto.jmoPartBinID,
					jmoPartID = eRPJobOperationInformationDto.jmoPartID,
					jmoPartRevisionID = eRPJobOperationInformationDto.jmoPartRevisionID,
					jmoPartWarehouseLocationID = eRPJobOperationInformationDto.jmoPartWarehouseLocationID,
					jmoPlantDepartmentID = eRPJobOperationInformationDto.jmoPlantDepartmentID,
					jmoPlantID = eRPJobOperationInformationDto.jmoPlantID,
					jmoProcessID = eRPJobOperationInformationDto.jmoProcessID,
					jmoProcessLongDescriptionRtf = eRPJobOperationInformationDto.jmoProcessLongDescriptionRtf,
					jmoProcessLongDescriptionText = eRPJobOperationInformationDto.jmoProcessLongDescriptionText,
					jmoProcessShortDescription = eRPJobOperationInformationDto.jmoProcessShortDescription,
					jmoProductionRate = eRPJobOperationInformationDto.jmoProductionRate,
					jmoProductionStandard = eRPJobOperationInformationDto.jmoProductionStandard,
					jmoPurchaseLocationID = eRPJobOperationInformationDto.jmoPurchaseLocationID,
					jmoPurchaseOrderID = eRPJobOperationInformationDto.jmoPurchaseOrderID,
					jmoQuantityBreak1 = eRPJobOperationInformationDto.jmoQuantityBreak1,
					jmoQuantityBreak2 = eRPJobOperationInformationDto.jmoQuantityBreak2,
					jmoQuantityBreak3 = eRPJobOperationInformationDto.jmoQuantityBreak3,
					jmoQuantityBreak4 = eRPJobOperationInformationDto.jmoQuantityBreak4,
					jmoQuantityBreak5 = eRPJobOperationInformationDto.jmoQuantityBreak5,
					jmoQuantityBreak6 = eRPJobOperationInformationDto.jmoQuantityBreak6,
					jmoQuantityBreak7 = eRPJobOperationInformationDto.jmoQuantityBreak7,
					jmoQuantityBreak8 = eRPJobOperationInformationDto.jmoQuantityBreak8,
					jmoQuantityBreak9 = eRPJobOperationInformationDto.jmoQuantityBreak9,
					jmoQuantityComplete = eRPJobOperationInformationDto.jmoQuantityComplete,
					jmoQuantityPerAssembly = eRPJobOperationInformationDto.jmoQuantityPerAssembly,
					jmoQuantityToInspect = eRPJobOperationInformationDto.jmoQuantityToInspect,
					jmoQuantityToReturn = eRPJobOperationInformationDto.jmoQuantityToReturn,
					jmoQueueTime = eRPJobOperationInformationDto.jmoQueueTime,
					jmoRfqID = eRPJobOperationInformationDto.jmoRfqID,
					jmoRowVersion = eRPJobOperationInformationDto.jmoRowVersion,
					jmoScrapQuantityReceived = eRPJobOperationInformationDto.jmoScrapQuantityReceived,
					jmoJobOperationID = eRPJobOperationInformationDto.jmoJobOperationID,
					jmoSetupCharge = eRPJobOperationInformationDto.jmoSetupCharge,
					jmoSetupHours = eRPJobOperationInformationDto.jmoSetupHours,
					jmoSetupPercentComplete = eRPJobOperationInformationDto.jmoSetupPercentComplete,
					jmoSetupRate = eRPJobOperationInformationDto.jmoSetupRate,
					jmoSfeMessageRTF = eRPJobOperationInformationDto.jmoSfeMessageRTF,
					jmoSfeMessageText = eRPJobOperationInformationDto.jmoSfeMessageText,
					jmoStandardFactor = eRPJobOperationInformationDto.jmoStandardFactor,
					jmoStartDate = eRPJobOperationInformationDto.jmoStartDate,
					jmoStartHour = eRPJobOperationInformationDto.jmoStartHour,
					jmoSupplierOrganizationID = eRPJobOperationInformationDto.jmoSupplierOrganizationID,
					jmoUnitCost1 = eRPJobOperationInformationDto.jmoUnitCost1,
					jmoUnitCost2 = eRPJobOperationInformationDto.jmoUnitCost2,
					jmoUnitCost3 = eRPJobOperationInformationDto.jmoUnitCost3,
					jmoUnitCost4 = eRPJobOperationInformationDto.jmoUnitCost4,
					jmoUnitCost5 = eRPJobOperationInformationDto.jmoUnitCost5,
					jmoUnitCost6 = eRPJobOperationInformationDto.jmoUnitCost6,
					jmoUnitCost7 = eRPJobOperationInformationDto.jmoUnitCost7,
					jmoUnitCost8 = eRPJobOperationInformationDto.jmoUnitCost8,
					jmoUnitCost9 = eRPJobOperationInformationDto.jmoUnitCost9,
					jmoUnitOfMeasure = eRPJobOperationInformationDto.jmoUnitOfMeasure,
					jmoWorkCenterID = eRPJobOperationInformationDto.jmoWorkCenterID,
					jmoWorkCenterMachineID = eRPJobOperationInformationDto.jmoWorkCenterMachineID,
					CustomFields = eRPJobOperationInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the JobOperations []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobOperationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobOperationDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobOperationDto>> Process_PutJobOperation(ERPJobOperationDto jobOperation)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPJobOperationDto createdObject = null;
		ERPResponseMessageDto<ERPJobOperationDto> result;
		try
		{
			IERPJobOperationRepository iERPJobOperationRepository = (base.ERPJobOperationRepository = new ERPJobOperationRepository(base.ApiClientContext));
			using (iERPJobOperationRepository)
			{
				APIValidationInfoDto postResult = await base.ERPJobOperationRepository.SaveJobOperation(jobOperation);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPJobOperationInformationDto eRPJobOperationInformationDto = await base.ERPJobOperationRepository.GetJobOperation(jobOperation.jmoUniqueID);
					createdObject = new ERPJobOperationDto
					{
						jmoActualProductionHours = eRPJobOperationInformationDto.jmoActualProductionHours,
						jmoActualSetupHours = eRPJobOperationInformationDto.jmoActualSetupHours,
						jmoCalculatedUnitCost = eRPJobOperationInformationDto.jmoCalculatedUnitCost,
						jmoCompletedProductionHours = eRPJobOperationInformationDto.jmoCompletedProductionHours,
						jmoCompletedSetupHours = eRPJobOperationInformationDto.jmoCompletedSetupHours,
						jmoCreatedBy = eRPJobOperationInformationDto.jmoCreatedBy,
						jmoCreatedDate = eRPJobOperationInformationDto.jmoCreatedDate,
						jmoDocuments = eRPJobOperationInformationDto.jmoDocuments,
						jmoDueDate = eRPJobOperationInformationDto.jmoDueDate,
						jmoDueHour = eRPJobOperationInformationDto.jmoDueHour,
						jmoUniqueID = eRPJobOperationInformationDto.jmoUniqueID,
						jmoEstimatedProductionHours = eRPJobOperationInformationDto.jmoEstimatedProductionHours,
						jmoEstimatedUnitCost = eRPJobOperationInformationDto.jmoEstimatedUnitCost,
						jmoInspectionStatus = eRPJobOperationInformationDto.jmoInspectionStatus,
						jmoInspectionType = eRPJobOperationInformationDto.jmoInspectionType,
						jmoAddedOperation = eRPJobOperationInformationDto.jmoAddedOperation,
						jmoClosed = eRPJobOperationInformationDto.jmoClosed,
						jmoFirm = eRPJobOperationInformationDto.jmoFirm,
						jmoInspectionComplete = eRPJobOperationInformationDto.jmoInspectionComplete,
						jmoProductionComplete = eRPJobOperationInformationDto.jmoProductionComplete,
						jmoPrototypeOperation = eRPJobOperationInformationDto.jmoPrototypeOperation,
						jmoSetupComplete = eRPJobOperationInformationDto.jmoSetupComplete,
						jmoJobAssemblyID = eRPJobOperationInformationDto.jmoJobAssemblyID,
						jmoJobID = eRPJobOperationInformationDto.jmoJobID,
						jmoMachinesToSchedule = eRPJobOperationInformationDto.jmoMachinesToSchedule,
						jmoMachineType = eRPJobOperationInformationDto.jmoMachineType,
						jmoMinimumCharge = eRPJobOperationInformationDto.jmoMinimumCharge,
						jmoMoveTime = eRPJobOperationInformationDto.jmoMoveTime,
						jmoOperationQuantity = eRPJobOperationInformationDto.jmoOperationQuantity,
						jmoOperationType = eRPJobOperationInformationDto.jmoOperationType,
						jmoOverheadRate = eRPJobOperationInformationDto.jmoOverheadRate,
						jmoOverlap = eRPJobOperationInformationDto.jmoOverlap,
						jmoOverlapDestinationLink = eRPJobOperationInformationDto.jmoOverlapDestinationLink,
						jmoOverlapOffsetTime = eRPJobOperationInformationDto.jmoOverlapOffsetTime,
						jmoOverlapOperationID = eRPJobOperationInformationDto.jmoOverlapOperationID,
						jmoOverlapSourceLink = eRPJobOperationInformationDto.jmoOverlapSourceLink,
						jmoPartBinID = eRPJobOperationInformationDto.jmoPartBinID,
						jmoPartID = eRPJobOperationInformationDto.jmoPartID,
						jmoPartRevisionID = eRPJobOperationInformationDto.jmoPartRevisionID,
						jmoPartWarehouseLocationID = eRPJobOperationInformationDto.jmoPartWarehouseLocationID,
						jmoPlantDepartmentID = eRPJobOperationInformationDto.jmoPlantDepartmentID,
						jmoPlantID = eRPJobOperationInformationDto.jmoPlantID,
						jmoProcessID = eRPJobOperationInformationDto.jmoProcessID,
						jmoProcessLongDescriptionRtf = eRPJobOperationInformationDto.jmoProcessLongDescriptionRtf,
						jmoProcessLongDescriptionText = eRPJobOperationInformationDto.jmoProcessLongDescriptionText,
						jmoProcessShortDescription = eRPJobOperationInformationDto.jmoProcessShortDescription,
						jmoProductionRate = eRPJobOperationInformationDto.jmoProductionRate,
						jmoProductionStandard = eRPJobOperationInformationDto.jmoProductionStandard,
						jmoPurchaseLocationID = eRPJobOperationInformationDto.jmoPurchaseLocationID,
						jmoPurchaseOrderID = eRPJobOperationInformationDto.jmoPurchaseOrderID,
						jmoQuantityBreak1 = eRPJobOperationInformationDto.jmoQuantityBreak1,
						jmoQuantityBreak2 = eRPJobOperationInformationDto.jmoQuantityBreak2,
						jmoQuantityBreak3 = eRPJobOperationInformationDto.jmoQuantityBreak3,
						jmoQuantityBreak4 = eRPJobOperationInformationDto.jmoQuantityBreak4,
						jmoQuantityBreak5 = eRPJobOperationInformationDto.jmoQuantityBreak5,
						jmoQuantityBreak6 = eRPJobOperationInformationDto.jmoQuantityBreak6,
						jmoQuantityBreak7 = eRPJobOperationInformationDto.jmoQuantityBreak7,
						jmoQuantityBreak8 = eRPJobOperationInformationDto.jmoQuantityBreak8,
						jmoQuantityBreak9 = eRPJobOperationInformationDto.jmoQuantityBreak9,
						jmoQuantityComplete = eRPJobOperationInformationDto.jmoQuantityComplete,
						jmoQuantityPerAssembly = eRPJobOperationInformationDto.jmoQuantityPerAssembly,
						jmoQuantityToInspect = eRPJobOperationInformationDto.jmoQuantityToInspect,
						jmoQuantityToReturn = eRPJobOperationInformationDto.jmoQuantityToReturn,
						jmoQueueTime = eRPJobOperationInformationDto.jmoQueueTime,
						jmoRfqID = eRPJobOperationInformationDto.jmoRfqID,
						jmoRowVersion = eRPJobOperationInformationDto.jmoRowVersion,
						jmoScrapQuantityReceived = eRPJobOperationInformationDto.jmoScrapQuantityReceived,
						jmoJobOperationID = eRPJobOperationInformationDto.jmoJobOperationID,
						jmoSetupCharge = eRPJobOperationInformationDto.jmoSetupCharge,
						jmoSetupHours = eRPJobOperationInformationDto.jmoSetupHours,
						jmoSetupPercentComplete = eRPJobOperationInformationDto.jmoSetupPercentComplete,
						jmoSetupRate = eRPJobOperationInformationDto.jmoSetupRate,
						jmoSfeMessageRTF = eRPJobOperationInformationDto.jmoSfeMessageRTF,
						jmoSfeMessageText = eRPJobOperationInformationDto.jmoSfeMessageText,
						jmoStandardFactor = eRPJobOperationInformationDto.jmoStandardFactor,
						jmoStartDate = eRPJobOperationInformationDto.jmoStartDate,
						jmoStartHour = eRPJobOperationInformationDto.jmoStartHour,
						jmoSupplierOrganizationID = eRPJobOperationInformationDto.jmoSupplierOrganizationID,
						jmoUnitCost1 = eRPJobOperationInformationDto.jmoUnitCost1,
						jmoUnitCost2 = eRPJobOperationInformationDto.jmoUnitCost2,
						jmoUnitCost3 = eRPJobOperationInformationDto.jmoUnitCost3,
						jmoUnitCost4 = eRPJobOperationInformationDto.jmoUnitCost4,
						jmoUnitCost5 = eRPJobOperationInformationDto.jmoUnitCost5,
						jmoUnitCost6 = eRPJobOperationInformationDto.jmoUnitCost6,
						jmoUnitCost7 = eRPJobOperationInformationDto.jmoUnitCost7,
						jmoUnitCost8 = eRPJobOperationInformationDto.jmoUnitCost8,
						jmoUnitCost9 = eRPJobOperationInformationDto.jmoUnitCost9,
						jmoUnitOfMeasure = eRPJobOperationInformationDto.jmoUnitOfMeasure,
						jmoWorkCenterID = eRPJobOperationInformationDto.jmoWorkCenterID,
						jmoWorkCenterMachineID = eRPJobOperationInformationDto.jmoWorkCenterMachineID,
						CustomFields = eRPJobOperationInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing JobOperation [{jobOperation.jmoUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobOperationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteJobOperation(Guid jobOperationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobOperationRepository iERPJobOperationRepository = (base.ERPJobOperationRepository = new ERPJobOperationRepository(base.ApiClientContext));
		using (iERPJobOperationRepository)
		{
			if (!(await base.ERPJobOperationRepository.DoesJobOperationExist(jobOperationId)))
			{
				base.ErrorsList.Add($"JobOperation [{jobOperationId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPJobOperationInformationDto eRPJobOperationInformationDto = await base.ERPJobOperationRepository.GetJobOperation(jobOperationId);
				string text = await base.ERPJobOperationRepository.WhereUsed("JobOperations", new object[3] { eRPJobOperationInformationDto.jmoJobID, eRPJobOperationInformationDto.jmoJobAssemblyID, eRPJobOperationInformationDto.jmoJobOperationID }, new object[3] { "jmoJobID", "jmoJobAssemblyID", "jmoJobOperationID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("JobOperation cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPJobOperationDto>> Process_DeleteJobOperation(Guid jobOperationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPJobOperationDto> result;
		try
		{
			IERPJobOperationRepository iERPJobOperationRepository = (base.ERPJobOperationRepository = new ERPJobOperationRepository(base.ApiClientContext));
			using (iERPJobOperationRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPJobOperationRepository.DeleteRowFromTable("JobOperations", "jmo", jobOperationId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of JobOperation [{jobOperationId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobOperationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPJobOperationDto()
			};
		}
		return result;
	}
}
