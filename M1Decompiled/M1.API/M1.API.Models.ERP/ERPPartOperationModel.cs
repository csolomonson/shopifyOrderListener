using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartOperationModel : ERPBaseModel, IERPPartOperationModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartOperations(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartOperationRepository iERPPartOperationRepository = (base.ERPPartOperationRepository = new ERPPartOperationRepository(base.ApiClientContext));
		using (iERPPartOperationRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartOperationRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartOperationRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartOperationRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartOperationRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartOperation(Guid partOperationId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartOperationRepository iERPPartOperationRepository = (base.ERPPartOperationRepository = new ERPPartOperationRepository(base.ApiClientContext));
		using (iERPPartOperationRepository)
		{
			if (!(await base.ERPPartOperationRepository.DoesPartOperationExist(partOperationId)))
			{
				errorsList.Add($"PartOperation [{partOperationId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartOperation(ERPPartOperationDto partOperation)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartOperationRepository iERPPartOperationRepository = (base.ERPPartOperationRepository = new ERPPartOperationRepository(base.ApiClientContext));
		using (iERPPartOperationRepository)
		{
			if (!string.IsNullOrWhiteSpace(partOperation.imoMethodID) && !(await base.ERPPartOperationRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partOperation.imoMethodID })))
			{
				errorsList.Add("imoMethodID [" + partOperation.imoMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partOperation.imoMethodRevisionID) && !(await base.ERPPartOperationRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partOperation.imoMethodID, partOperation.imoMethodRevisionID })))
			{
				errorsList.Add("imoMethodRevisionID [" + partOperation.imoMethodRevisionID + "] not found.");
			}
			if (partOperation.imoMethodAssemblyID > 0 && !(await base.ERPPartOperationRepository.DoesRecordExistInTableUsingKeys("PartAssemblies", new object[3] { "IMAMETHODID", "IMAMETHODREVISIONID", "IMAMETHODASSEMBLYID" }, new object[3] { partOperation.imoMethodID, partOperation.imoMethodRevisionID, partOperation.imoMethodAssemblyID })))
			{
				errorsList.Add($"imoMethodAssemblyID [{partOperation.imoMethodAssemblyID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partOperation.imoPlantDepartmentID) && !(await base.ERPPartOperationRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { partOperation.imoPlantID, partOperation.imoPlantDepartmentID })))
			{
				errorsList.Add("imoPlantDepartmentID [" + partOperation.imoPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partOperation.imoPlantID) && !(await base.ERPPartOperationRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { partOperation.imoPlantID })))
			{
				errorsList.Add("imoPlantID [" + partOperation.imoPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partOperation.imoWorkCenterID) && !(await base.ERPPartOperationRepository.DoesRecordExistInTableUsingKeys("WorkCenters", new object[1] { "XAWWORKCENTERID" }, new object[1] { partOperation.imoWorkCenterID })))
			{
				errorsList.Add("imoWorkCenterID [" + partOperation.imoWorkCenterID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partOperation.imoProcessID) && !(await base.ERPPartOperationRepository.DoesRecordExistInTableUsingKeys("Processes", new object[1] { "XACPROCESSID" }, new object[1] { partOperation.imoProcessID })))
			{
				errorsList.Add("imoProcessID [" + partOperation.imoProcessID + "] not found.");
			}
			if (partOperation.imoWorkCenterMachineID > 0 && !(await base.ERPPartOperationRepository.DoesRecordExistInTableUsingKeys("WorkCenterMachines", new object[2] { "XAQWORKCENTERID", "XAQWORKCENTERMACHINEID" }, new object[2] { partOperation.imoWorkCenterID, partOperation.imoWorkCenterMachineID })))
			{
				errorsList.Add($"imoWorkCenterMachineID [{partOperation.imoWorkCenterMachineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partOperation.imoPartID) && !(await base.ERPPartOperationRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partOperation.imoPartID })))
			{
				errorsList.Add("imoPartID [" + partOperation.imoPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partOperation.imoPartRevisionID) && !(await base.ERPPartOperationRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partOperation.imoPartID, partOperation.imoPartRevisionID })))
			{
				errorsList.Add("imoPartRevisionID [" + partOperation.imoPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partOperation.imoSupplierOrganizationID) && !(await base.ERPPartOperationRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { partOperation.imoSupplierOrganizationID })))
			{
				errorsList.Add("imoSupplierOrganizationID [" + partOperation.imoSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partOperation.imoPurchaseLocationID) && !(await base.ERPPartOperationRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { partOperation.imoSupplierOrganizationID, partOperation.imoPurchaseLocationID })))
			{
				errorsList.Add("imoPurchaseLocationID [" + partOperation.imoPurchaseLocationID + "] not found.");
			}
			if (partOperation.imoOverlapOperationID > 0 && !(await base.ERPPartOperationRepository.DoesRecordExistInTableUsingKeys("PartOperations", new object[4] { "IMOMETHODID", "IMOMETHODREVISIONID", "IMOMETHODASSEMBLYID", "IMOMETHODOPERATIONID" }, new object[4] { partOperation.imoMethodID, partOperation.imoMethodRevisionID, partOperation.imoMethodAssemblyID, partOperation.imoOverlapOperationID })))
			{
				errorsList.Add($"imoOverlapOperationID [{partOperation.imoOverlapOperationID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartOperationDto>>> Process_GetAllPartOperations(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartOperationDto> allPartOperationsDto = new List<ERPPartOperationDto>();
		ERPResponseMessageDto<IList<ERPPartOperationDto>> result;
		try
		{
			IERPPartOperationRepository iERPPartOperationRepository = (base.ERPPartOperationRepository = new ERPPartOperationRepository(base.ApiClientContext));
			using (iERPPartOperationRepository)
			{
				foreach (ERPPartOperationInformationDto item2 in await base.ERPPartOperationRepository.GetAllPartOperations(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartOperationDto item = new ERPPartOperationDto
					{
						imoCreatedBy = item2.imoCreatedBy,
						imoCreatedDate = item2.imoCreatedDate,
						imoDocuments = item2.imoDocuments,
						imoUniqueID = item2.imoUniqueID,
						imoEstimatedUnitCost = item2.imoEstimatedUnitCost,
						imoInspectionType = item2.imoInspectionType,
						imoMachinesToSchedule = item2.imoMachinesToSchedule,
						imoMachineType = item2.imoMachineType,
						imoMethodAssemblyID = item2.imoMethodAssemblyID,
						imoMethodID = item2.imoMethodID,
						imoMethodOperationID = item2.imoMethodOperationID,
						imoMethodRevisionID = item2.imoMethodRevisionID,
						imoMinimumCharge = item2.imoMinimumCharge,
						imoMoveTime = item2.imoMoveTime,
						imoOperationType = item2.imoOperationType,
						imoOverlap = item2.imoOverlap,
						imoOverlapDestinationLink = item2.imoOverlapDestinationLink,
						imoOverlapOffsetTime = item2.imoOverlapOffsetTime,
						imoOverlapOperationID = item2.imoOverlapOperationID,
						imoOverlapSourceLink = item2.imoOverlapSourceLink,
						imoPartID = item2.imoPartID,
						imoPartRevisionID = item2.imoPartRevisionID,
						imoPlantDepartmentID = item2.imoPlantDepartmentID,
						imoPlantID = item2.imoPlantID,
						imoProcessID = item2.imoProcessID,
						imoProcessLongDescriptionRtf = item2.imoProcessLongDescriptionRtf,
						imoProcessLongDescriptionText = item2.imoProcessLongDescriptionText,
						imoProcessShortDescription = item2.imoProcessShortDescription,
						imoProductionStandard = item2.imoProductionStandard,
						imoPurchaseLocationID = item2.imoPurchaseLocationID,
						imoQuantityBreak1 = item2.imoQuantityBreak1,
						imoQuantityBreak2 = item2.imoQuantityBreak2,
						imoQuantityBreak3 = item2.imoQuantityBreak3,
						imoQuantityBreak4 = item2.imoQuantityBreak4,
						imoQuantityBreak5 = item2.imoQuantityBreak5,
						imoQuantityBreak6 = item2.imoQuantityBreak6,
						imoQuantityBreak7 = item2.imoQuantityBreak7,
						imoQuantityBreak8 = item2.imoQuantityBreak8,
						imoQuantityBreak9 = item2.imoQuantityBreak9,
						imoQuantityPerAssembly = item2.imoQuantityPerAssembly,
						imoQueueTime = item2.imoQueueTime,
						imoRowVersion = item2.imoRowVersion,
						imoSetupCharge = item2.imoSetupCharge,
						imoSetupHours = item2.imoSetupHours,
						imoSfeMessageRTF = item2.imoSfeMessageRTF,
						imoSfeMessageText = item2.imoSfeMessageText,
						imoStandardFactor = item2.imoStandardFactor,
						imoSupplierOrganizationID = item2.imoSupplierOrganizationID,
						imoUnitCost1 = item2.imoUnitCost1,
						imoUnitCost2 = item2.imoUnitCost2,
						imoUnitCost3 = item2.imoUnitCost3,
						imoUnitCost4 = item2.imoUnitCost4,
						imoUnitCost5 = item2.imoUnitCost5,
						imoUnitCost6 = item2.imoUnitCost6,
						imoUnitCost7 = item2.imoUnitCost7,
						imoUnitCost8 = item2.imoUnitCost8,
						imoUnitCost9 = item2.imoUnitCost9,
						imoUnitOfMeasure = item2.imoUnitOfMeasure,
						imoWorkCenterID = item2.imoWorkCenterID,
						imoWorkCenterMachineID = item2.imoWorkCenterMachineID,
						CustomFields = item2.CustomFields
					};
					allPartOperationsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartOperations]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartOperationDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartOperationsDto,
				RecordCount = allPartOperationsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartOperationDto>> Process_GetPartOperation(Guid partOperationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartOperationDto partOperationDto = null;
		ERPResponseMessageDto<ERPPartOperationDto> result;
		try
		{
			IERPPartOperationRepository iERPPartOperationRepository = (base.ERPPartOperationRepository = new ERPPartOperationRepository(base.ApiClientContext));
			using (iERPPartOperationRepository)
			{
				ERPPartOperationInformationDto eRPPartOperationInformationDto = await base.ERPPartOperationRepository.GetPartOperation(partOperationId);
				partOperationDto = new ERPPartOperationDto
				{
					imoCreatedBy = eRPPartOperationInformationDto.imoCreatedBy,
					imoCreatedDate = eRPPartOperationInformationDto.imoCreatedDate,
					imoDocuments = eRPPartOperationInformationDto.imoDocuments,
					imoUniqueID = eRPPartOperationInformationDto.imoUniqueID,
					imoEstimatedUnitCost = eRPPartOperationInformationDto.imoEstimatedUnitCost,
					imoInspectionType = eRPPartOperationInformationDto.imoInspectionType,
					imoMachinesToSchedule = eRPPartOperationInformationDto.imoMachinesToSchedule,
					imoMachineType = eRPPartOperationInformationDto.imoMachineType,
					imoMethodAssemblyID = eRPPartOperationInformationDto.imoMethodAssemblyID,
					imoMethodID = eRPPartOperationInformationDto.imoMethodID,
					imoMethodOperationID = eRPPartOperationInformationDto.imoMethodOperationID,
					imoMethodRevisionID = eRPPartOperationInformationDto.imoMethodRevisionID,
					imoMinimumCharge = eRPPartOperationInformationDto.imoMinimumCharge,
					imoMoveTime = eRPPartOperationInformationDto.imoMoveTime,
					imoOperationType = eRPPartOperationInformationDto.imoOperationType,
					imoOverlap = eRPPartOperationInformationDto.imoOverlap,
					imoOverlapDestinationLink = eRPPartOperationInformationDto.imoOverlapDestinationLink,
					imoOverlapOffsetTime = eRPPartOperationInformationDto.imoOverlapOffsetTime,
					imoOverlapOperationID = eRPPartOperationInformationDto.imoOverlapOperationID,
					imoOverlapSourceLink = eRPPartOperationInformationDto.imoOverlapSourceLink,
					imoPartID = eRPPartOperationInformationDto.imoPartID,
					imoPartRevisionID = eRPPartOperationInformationDto.imoPartRevisionID,
					imoPlantDepartmentID = eRPPartOperationInformationDto.imoPlantDepartmentID,
					imoPlantID = eRPPartOperationInformationDto.imoPlantID,
					imoProcessID = eRPPartOperationInformationDto.imoProcessID,
					imoProcessLongDescriptionRtf = eRPPartOperationInformationDto.imoProcessLongDescriptionRtf,
					imoProcessLongDescriptionText = eRPPartOperationInformationDto.imoProcessLongDescriptionText,
					imoProcessShortDescription = eRPPartOperationInformationDto.imoProcessShortDescription,
					imoProductionStandard = eRPPartOperationInformationDto.imoProductionStandard,
					imoPurchaseLocationID = eRPPartOperationInformationDto.imoPurchaseLocationID,
					imoQuantityBreak1 = eRPPartOperationInformationDto.imoQuantityBreak1,
					imoQuantityBreak2 = eRPPartOperationInformationDto.imoQuantityBreak2,
					imoQuantityBreak3 = eRPPartOperationInformationDto.imoQuantityBreak3,
					imoQuantityBreak4 = eRPPartOperationInformationDto.imoQuantityBreak4,
					imoQuantityBreak5 = eRPPartOperationInformationDto.imoQuantityBreak5,
					imoQuantityBreak6 = eRPPartOperationInformationDto.imoQuantityBreak6,
					imoQuantityBreak7 = eRPPartOperationInformationDto.imoQuantityBreak7,
					imoQuantityBreak8 = eRPPartOperationInformationDto.imoQuantityBreak8,
					imoQuantityBreak9 = eRPPartOperationInformationDto.imoQuantityBreak9,
					imoQuantityPerAssembly = eRPPartOperationInformationDto.imoQuantityPerAssembly,
					imoQueueTime = eRPPartOperationInformationDto.imoQueueTime,
					imoRowVersion = eRPPartOperationInformationDto.imoRowVersion,
					imoSetupCharge = eRPPartOperationInformationDto.imoSetupCharge,
					imoSetupHours = eRPPartOperationInformationDto.imoSetupHours,
					imoSfeMessageRTF = eRPPartOperationInformationDto.imoSfeMessageRTF,
					imoSfeMessageText = eRPPartOperationInformationDto.imoSfeMessageText,
					imoStandardFactor = eRPPartOperationInformationDto.imoStandardFactor,
					imoSupplierOrganizationID = eRPPartOperationInformationDto.imoSupplierOrganizationID,
					imoUnitCost1 = eRPPartOperationInformationDto.imoUnitCost1,
					imoUnitCost2 = eRPPartOperationInformationDto.imoUnitCost2,
					imoUnitCost3 = eRPPartOperationInformationDto.imoUnitCost3,
					imoUnitCost4 = eRPPartOperationInformationDto.imoUnitCost4,
					imoUnitCost5 = eRPPartOperationInformationDto.imoUnitCost5,
					imoUnitCost6 = eRPPartOperationInformationDto.imoUnitCost6,
					imoUnitCost7 = eRPPartOperationInformationDto.imoUnitCost7,
					imoUnitCost8 = eRPPartOperationInformationDto.imoUnitCost8,
					imoUnitCost9 = eRPPartOperationInformationDto.imoUnitCost9,
					imoUnitOfMeasure = eRPPartOperationInformationDto.imoUnitOfMeasure,
					imoWorkCenterID = eRPPartOperationInformationDto.imoWorkCenterID,
					imoWorkCenterMachineID = eRPPartOperationInformationDto.imoWorkCenterMachineID,
					CustomFields = eRPPartOperationInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartOperations []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartOperationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partOperationDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartOperationDto>> Process_PutPartOperation(ERPPartOperationDto partOperation)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartOperationDto createdObject = null;
		ERPResponseMessageDto<ERPPartOperationDto> result;
		try
		{
			IERPPartOperationRepository iERPPartOperationRepository = (base.ERPPartOperationRepository = new ERPPartOperationRepository(base.ApiClientContext));
			using (iERPPartOperationRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartOperationRepository.SavePartOperation(partOperation);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartOperationInformationDto eRPPartOperationInformationDto = await base.ERPPartOperationRepository.GetPartOperation(partOperation.imoUniqueID);
					createdObject = new ERPPartOperationDto
					{
						imoCreatedBy = eRPPartOperationInformationDto.imoCreatedBy,
						imoCreatedDate = eRPPartOperationInformationDto.imoCreatedDate,
						imoDocuments = eRPPartOperationInformationDto.imoDocuments,
						imoUniqueID = eRPPartOperationInformationDto.imoUniqueID,
						imoEstimatedUnitCost = eRPPartOperationInformationDto.imoEstimatedUnitCost,
						imoInspectionType = eRPPartOperationInformationDto.imoInspectionType,
						imoMachinesToSchedule = eRPPartOperationInformationDto.imoMachinesToSchedule,
						imoMachineType = eRPPartOperationInformationDto.imoMachineType,
						imoMethodAssemblyID = eRPPartOperationInformationDto.imoMethodAssemblyID,
						imoMethodID = eRPPartOperationInformationDto.imoMethodID,
						imoMethodOperationID = eRPPartOperationInformationDto.imoMethodOperationID,
						imoMethodRevisionID = eRPPartOperationInformationDto.imoMethodRevisionID,
						imoMinimumCharge = eRPPartOperationInformationDto.imoMinimumCharge,
						imoMoveTime = eRPPartOperationInformationDto.imoMoveTime,
						imoOperationType = eRPPartOperationInformationDto.imoOperationType,
						imoOverlap = eRPPartOperationInformationDto.imoOverlap,
						imoOverlapDestinationLink = eRPPartOperationInformationDto.imoOverlapDestinationLink,
						imoOverlapOffsetTime = eRPPartOperationInformationDto.imoOverlapOffsetTime,
						imoOverlapOperationID = eRPPartOperationInformationDto.imoOverlapOperationID,
						imoOverlapSourceLink = eRPPartOperationInformationDto.imoOverlapSourceLink,
						imoPartID = eRPPartOperationInformationDto.imoPartID,
						imoPartRevisionID = eRPPartOperationInformationDto.imoPartRevisionID,
						imoPlantDepartmentID = eRPPartOperationInformationDto.imoPlantDepartmentID,
						imoPlantID = eRPPartOperationInformationDto.imoPlantID,
						imoProcessID = eRPPartOperationInformationDto.imoProcessID,
						imoProcessLongDescriptionRtf = eRPPartOperationInformationDto.imoProcessLongDescriptionRtf,
						imoProcessLongDescriptionText = eRPPartOperationInformationDto.imoProcessLongDescriptionText,
						imoProcessShortDescription = eRPPartOperationInformationDto.imoProcessShortDescription,
						imoProductionStandard = eRPPartOperationInformationDto.imoProductionStandard,
						imoPurchaseLocationID = eRPPartOperationInformationDto.imoPurchaseLocationID,
						imoQuantityBreak1 = eRPPartOperationInformationDto.imoQuantityBreak1,
						imoQuantityBreak2 = eRPPartOperationInformationDto.imoQuantityBreak2,
						imoQuantityBreak3 = eRPPartOperationInformationDto.imoQuantityBreak3,
						imoQuantityBreak4 = eRPPartOperationInformationDto.imoQuantityBreak4,
						imoQuantityBreak5 = eRPPartOperationInformationDto.imoQuantityBreak5,
						imoQuantityBreak6 = eRPPartOperationInformationDto.imoQuantityBreak6,
						imoQuantityBreak7 = eRPPartOperationInformationDto.imoQuantityBreak7,
						imoQuantityBreak8 = eRPPartOperationInformationDto.imoQuantityBreak8,
						imoQuantityBreak9 = eRPPartOperationInformationDto.imoQuantityBreak9,
						imoQuantityPerAssembly = eRPPartOperationInformationDto.imoQuantityPerAssembly,
						imoQueueTime = eRPPartOperationInformationDto.imoQueueTime,
						imoRowVersion = eRPPartOperationInformationDto.imoRowVersion,
						imoSetupCharge = eRPPartOperationInformationDto.imoSetupCharge,
						imoSetupHours = eRPPartOperationInformationDto.imoSetupHours,
						imoSfeMessageRTF = eRPPartOperationInformationDto.imoSfeMessageRTF,
						imoSfeMessageText = eRPPartOperationInformationDto.imoSfeMessageText,
						imoStandardFactor = eRPPartOperationInformationDto.imoStandardFactor,
						imoSupplierOrganizationID = eRPPartOperationInformationDto.imoSupplierOrganizationID,
						imoUnitCost1 = eRPPartOperationInformationDto.imoUnitCost1,
						imoUnitCost2 = eRPPartOperationInformationDto.imoUnitCost2,
						imoUnitCost3 = eRPPartOperationInformationDto.imoUnitCost3,
						imoUnitCost4 = eRPPartOperationInformationDto.imoUnitCost4,
						imoUnitCost5 = eRPPartOperationInformationDto.imoUnitCost5,
						imoUnitCost6 = eRPPartOperationInformationDto.imoUnitCost6,
						imoUnitCost7 = eRPPartOperationInformationDto.imoUnitCost7,
						imoUnitCost8 = eRPPartOperationInformationDto.imoUnitCost8,
						imoUnitCost9 = eRPPartOperationInformationDto.imoUnitCost9,
						imoUnitOfMeasure = eRPPartOperationInformationDto.imoUnitOfMeasure,
						imoWorkCenterID = eRPPartOperationInformationDto.imoWorkCenterID,
						imoWorkCenterMachineID = eRPPartOperationInformationDto.imoWorkCenterMachineID,
						CustomFields = eRPPartOperationInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartOperation [{partOperation.imoUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartOperationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartOperation(Guid partOperationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartOperationRepository iERPPartOperationRepository = (base.ERPPartOperationRepository = new ERPPartOperationRepository(base.ApiClientContext));
		using (iERPPartOperationRepository)
		{
			if (!(await base.ERPPartOperationRepository.DoesPartOperationExist(partOperationId)))
			{
				base.ErrorsList.Add($"PartOperation [{partOperationId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartOperationInformationDto eRPPartOperationInformationDto = await base.ERPPartOperationRepository.GetPartOperation(partOperationId);
				string text = await base.ERPPartOperationRepository.WhereUsed("PartOperations", new object[4] { eRPPartOperationInformationDto.imoMethodID, eRPPartOperationInformationDto.imoMethodRevisionID, eRPPartOperationInformationDto.imoMethodAssemblyID, eRPPartOperationInformationDto.imoMethodOperationID }, new object[4] { "imoMethodID", "imoMethodRevisionID", "imoMethodAssemblyID", "imoMethodOperationID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartOperation cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartOperationDto>> Process_DeletePartOperation(Guid partOperationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartOperationDto> result;
		try
		{
			IERPPartOperationRepository iERPPartOperationRepository = (base.ERPPartOperationRepository = new ERPPartOperationRepository(base.ApiClientContext));
			using (iERPPartOperationRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartOperationRepository.DeleteRowFromTable("PartOperations", "imo", partOperationId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartOperation [{partOperationId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartOperationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartOperationDto()
			};
		}
		return result;
	}
}
