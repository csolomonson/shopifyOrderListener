using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPQuoteOperationModel : ERPBaseModel, IERPQuoteOperationModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllQuoteOperations(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPQuoteOperationRepository iERPQuoteOperationRepository = (base.ERPQuoteOperationRepository = new ERPQuoteOperationRepository(base.ApiClientContext));
		using (iERPQuoteOperationRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPQuoteOperationRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPQuoteOperationRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPQuoteOperationRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPQuoteOperationRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetQuoteOperation(Guid quoteOperationId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteOperationRepository iERPQuoteOperationRepository = (base.ERPQuoteOperationRepository = new ERPQuoteOperationRepository(base.ApiClientContext));
		using (iERPQuoteOperationRepository)
		{
			if (!(await base.ERPQuoteOperationRepository.DoesQuoteOperationExist(quoteOperationId)))
			{
				errorsList.Add($"QuoteOperation [{quoteOperationId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutQuoteOperation(ERPQuoteOperationDto quoteOperation)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteOperationRepository iERPQuoteOperationRepository = (base.ERPQuoteOperationRepository = new ERPQuoteOperationRepository(base.ApiClientContext));
		using (iERPQuoteOperationRepository)
		{
			if (!string.IsNullOrWhiteSpace(quoteOperation.qmoQuoteID) && !(await base.ERPQuoteOperationRepository.DoesRecordExistInTableUsingKeys("Quotes", new object[1] { "QMPQUOTEID" }, new object[1] { quoteOperation.qmoQuoteID })))
			{
				errorsList.Add("qmoQuoteID [" + quoteOperation.qmoQuoteID + "] not found.");
			}
			if (quoteOperation.qmoQuoteLineID > 0 && !(await base.ERPQuoteOperationRepository.DoesRecordExistInTableUsingKeys("QuoteLines", new object[2] { "QMLQUOTEID", "QMLQUOTELINEID" }, new object[2] { quoteOperation.qmoQuoteID, quoteOperation.qmoQuoteLineID })))
			{
				errorsList.Add($"qmoQuoteLineID [{quoteOperation.qmoQuoteLineID}] not found.");
			}
			if (quoteOperation.qmoQuoteAssemblyID > 0 && !(await base.ERPQuoteOperationRepository.DoesRecordExistInTableUsingKeys("QuoteAssemblies", new object[3] { "QMAQUOTEID", "QMAQUOTELINEID", "QMAQUOTEASSEMBLYID" }, new object[3] { quoteOperation.qmoQuoteID, quoteOperation.qmoQuoteLineID, quoteOperation.qmoQuoteAssemblyID })))
			{
				errorsList.Add($"qmoQuoteAssemblyID [{quoteOperation.qmoQuoteAssemblyID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteOperation.qmoPlantDepartmentID) && !(await base.ERPQuoteOperationRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { quoteOperation.qmoPlantID, quoteOperation.qmoPlantDepartmentID })))
			{
				errorsList.Add("qmoPlantDepartmentID [" + quoteOperation.qmoPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteOperation.qmoPlantID) && !(await base.ERPQuoteOperationRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { quoteOperation.qmoPlantID })))
			{
				errorsList.Add("qmoPlantID [" + quoteOperation.qmoPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteOperation.qmoWorkCenterID) && !(await base.ERPQuoteOperationRepository.DoesRecordExistInTableUsingKeys("WorkCenters", new object[1] { "XAWWORKCENTERID" }, new object[1] { quoteOperation.qmoWorkCenterID })))
			{
				errorsList.Add("qmoWorkCenterID [" + quoteOperation.qmoWorkCenterID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteOperation.qmoProcessID) && !(await base.ERPQuoteOperationRepository.DoesRecordExistInTableUsingKeys("Processes", new object[1] { "XACPROCESSID" }, new object[1] { quoteOperation.qmoProcessID })))
			{
				errorsList.Add("qmoProcessID [" + quoteOperation.qmoProcessID + "] not found.");
			}
			if (quoteOperation.qmoWorkCenterMachineID > 0 && !(await base.ERPQuoteOperationRepository.DoesRecordExistInTableUsingKeys("WorkCenterMachines", new object[2] { "XAQWORKCENTERID", "XAQWORKCENTERMACHINEID" }, new object[2] { quoteOperation.qmoWorkCenterID, quoteOperation.qmoWorkCenterMachineID })))
			{
				errorsList.Add($"qmoWorkCenterMachineID [{quoteOperation.qmoWorkCenterMachineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteOperation.qmoPartID) && !(await base.ERPQuoteOperationRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { quoteOperation.qmoPartID })))
			{
				errorsList.Add("qmoPartID [" + quoteOperation.qmoPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteOperation.qmoPartRevisionID) && !(await base.ERPQuoteOperationRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { quoteOperation.qmoPartID, quoteOperation.qmoPartRevisionID })))
			{
				errorsList.Add("qmoPartRevisionID [" + quoteOperation.qmoPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteOperation.qmoSupplierOrganizationID) && !(await base.ERPQuoteOperationRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { quoteOperation.qmoSupplierOrganizationID })))
			{
				errorsList.Add("qmoSupplierOrganizationID [" + quoteOperation.qmoSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteOperation.qmoPurchaseLocationID) && !(await base.ERPQuoteOperationRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { quoteOperation.qmoSupplierOrganizationID, quoteOperation.qmoPurchaseLocationID })))
			{
				errorsList.Add("qmoPurchaseLocationID [" + quoteOperation.qmoPurchaseLocationID + "] not found.");
			}
			if (quoteOperation.qmoOverlapOperationID > 0 && !(await base.ERPQuoteOperationRepository.DoesRecordExistInTableUsingKeys("QuoteOperations", new object[4] { "QMOQUOTEID", "QMOQUOTELINEID", "QMOQUOTEASSEMBLYID", "QMOQUOTEOPERATIONID" }, new object[4] { quoteOperation.qmoQuoteID, quoteOperation.qmoQuoteLineID, quoteOperation.qmoQuoteAssemblyID, quoteOperation.qmoOverlapOperationID })))
			{
				errorsList.Add($"qmoOverlapOperationID [{quoteOperation.qmoOverlapOperationID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPQuoteOperationDto>>> Process_GetAllQuoteOperations(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPQuoteOperationDto> allQuoteOperationsDto = new List<ERPQuoteOperationDto>();
		ERPResponseMessageDto<IList<ERPQuoteOperationDto>> result;
		try
		{
			IERPQuoteOperationRepository iERPQuoteOperationRepository = (base.ERPQuoteOperationRepository = new ERPQuoteOperationRepository(base.ApiClientContext));
			using (iERPQuoteOperationRepository)
			{
				foreach (ERPQuoteOperationInformationDto item2 in await base.ERPQuoteOperationRepository.GetAllQuoteOperations(pageSize, pageNumber, filter, orderBy))
				{
					ERPQuoteOperationDto item = new ERPQuoteOperationDto
					{
						qmoAdditionalSetupHours = item2.qmoAdditionalSetupHours,
						qmoAdditionalSetupQuantity = item2.qmoAdditionalSetupQuantity,
						qmoCreatedBy = item2.qmoCreatedBy,
						qmoCreatedDate = item2.qmoCreatedDate,
						qmoDocuments = item2.qmoDocuments,
						qmoUniqueID = item2.qmoUniqueID,
						qmoEstimatedUnitCost = item2.qmoEstimatedUnitCost,
						qmoInspectionType = item2.qmoInspectionType,
						qmoClosed = item2.qmoClosed,
						qmoMachinesToSchedule = item2.qmoMachinesToSchedule,
						qmoMachineType = item2.qmoMachineType,
						qmoMinimumCharge = item2.qmoMinimumCharge,
						qmoMoveTime = item2.qmoMoveTime,
						qmoOperationType = item2.qmoOperationType,
						qmoOverheadRate = item2.qmoOverheadRate,
						qmoOverlap = item2.qmoOverlap,
						qmoOverlapDestinationLink = item2.qmoOverlapDestinationLink,
						qmoOverlapOffsetTime = item2.qmoOverlapOffsetTime,
						qmoOverlapOperationID = item2.qmoOverlapOperationID,
						qmoOverlapSourceLink = item2.qmoOverlapSourceLink,
						qmoPartID = item2.qmoPartID,
						qmoPartRevisionID = item2.qmoPartRevisionID,
						qmoPlantDepartmentID = item2.qmoPlantDepartmentID,
						qmoPlantID = item2.qmoPlantID,
						qmoProcessID = item2.qmoProcessID,
						qmoProcessLongDescriptionRtf = item2.qmoProcessLongDescriptionRtf,
						qmoProcessLongDescriptionText = item2.qmoProcessLongDescriptionText,
						qmoProcessShortDescription = item2.qmoProcessShortDescription,
						qmoProductionRate = item2.qmoProductionRate,
						qmoProductionStandard = item2.qmoProductionStandard,
						qmoPurchaseLocationID = item2.qmoPurchaseLocationID,
						qmoQuantityBreak1 = item2.qmoQuantityBreak1,
						qmoQuantityBreak2 = item2.qmoQuantityBreak2,
						qmoQuantityBreak3 = item2.qmoQuantityBreak3,
						qmoQuantityBreak4 = item2.qmoQuantityBreak4,
						qmoQuantityBreak5 = item2.qmoQuantityBreak5,
						qmoQuantityBreak6 = item2.qmoQuantityBreak6,
						qmoQuantityBreak7 = item2.qmoQuantityBreak7,
						qmoQuantityBreak8 = item2.qmoQuantityBreak8,
						qmoQuantityBreak9 = item2.qmoQuantityBreak9,
						qmoQuantityPerAssembly = item2.qmoQuantityPerAssembly,
						qmoQueueTime = item2.qmoQueueTime,
						qmoQuoteAssemblyID = item2.qmoQuoteAssemblyID,
						qmoQuoteID = item2.qmoQuoteID,
						qmoQuoteLineID = item2.qmoQuoteLineID,
						qmoQuotingRate = item2.qmoQuotingRate,
						qmoRowVersion = item2.qmoRowVersion,
						qmoQuoteOperationID = item2.qmoQuoteOperationID,
						qmoSetupCharge = item2.qmoSetupCharge,
						qmoSetupHours = item2.qmoSetupHours,
						qmoSetupRate = item2.qmoSetupRate,
						qmoSfeMessageRTF = item2.qmoSfeMessageRTF,
						qmoSfeMessageText = item2.qmoSfeMessageText,
						qmoStandardFactor = item2.qmoStandardFactor,
						qmoSupplierOrganizationID = item2.qmoSupplierOrganizationID,
						qmoUnitCost1 = item2.qmoUnitCost1,
						qmoUnitCost2 = item2.qmoUnitCost2,
						qmoUnitCost3 = item2.qmoUnitCost3,
						qmoUnitCost4 = item2.qmoUnitCost4,
						qmoUnitCost5 = item2.qmoUnitCost5,
						qmoUnitCost6 = item2.qmoUnitCost6,
						qmoUnitCost7 = item2.qmoUnitCost7,
						qmoUnitCost8 = item2.qmoUnitCost8,
						qmoUnitCost9 = item2.qmoUnitCost9,
						qmoUnitOfMeasure = item2.qmoUnitOfMeasure,
						qmoWorkCenterID = item2.qmoWorkCenterID,
						qmoWorkCenterMachineID = item2.qmoWorkCenterMachineID,
						CustomFields = item2.CustomFields
					};
					allQuoteOperationsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all QuoteOperations]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPQuoteOperationDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allQuoteOperationsDto,
				RecordCount = allQuoteOperationsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuoteOperationDto>> Process_GetQuoteOperation(Guid quoteOperationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPQuoteOperationDto quoteOperationDto = null;
		ERPResponseMessageDto<ERPQuoteOperationDto> result;
		try
		{
			IERPQuoteOperationRepository iERPQuoteOperationRepository = (base.ERPQuoteOperationRepository = new ERPQuoteOperationRepository(base.ApiClientContext));
			using (iERPQuoteOperationRepository)
			{
				ERPQuoteOperationInformationDto eRPQuoteOperationInformationDto = await base.ERPQuoteOperationRepository.GetQuoteOperation(quoteOperationId);
				quoteOperationDto = new ERPQuoteOperationDto
				{
					qmoAdditionalSetupHours = eRPQuoteOperationInformationDto.qmoAdditionalSetupHours,
					qmoAdditionalSetupQuantity = eRPQuoteOperationInformationDto.qmoAdditionalSetupQuantity,
					qmoCreatedBy = eRPQuoteOperationInformationDto.qmoCreatedBy,
					qmoCreatedDate = eRPQuoteOperationInformationDto.qmoCreatedDate,
					qmoDocuments = eRPQuoteOperationInformationDto.qmoDocuments,
					qmoUniqueID = eRPQuoteOperationInformationDto.qmoUniqueID,
					qmoEstimatedUnitCost = eRPQuoteOperationInformationDto.qmoEstimatedUnitCost,
					qmoInspectionType = eRPQuoteOperationInformationDto.qmoInspectionType,
					qmoClosed = eRPQuoteOperationInformationDto.qmoClosed,
					qmoMachinesToSchedule = eRPQuoteOperationInformationDto.qmoMachinesToSchedule,
					qmoMachineType = eRPQuoteOperationInformationDto.qmoMachineType,
					qmoMinimumCharge = eRPQuoteOperationInformationDto.qmoMinimumCharge,
					qmoMoveTime = eRPQuoteOperationInformationDto.qmoMoveTime,
					qmoOperationType = eRPQuoteOperationInformationDto.qmoOperationType,
					qmoOverheadRate = eRPQuoteOperationInformationDto.qmoOverheadRate,
					qmoOverlap = eRPQuoteOperationInformationDto.qmoOverlap,
					qmoOverlapDestinationLink = eRPQuoteOperationInformationDto.qmoOverlapDestinationLink,
					qmoOverlapOffsetTime = eRPQuoteOperationInformationDto.qmoOverlapOffsetTime,
					qmoOverlapOperationID = eRPQuoteOperationInformationDto.qmoOverlapOperationID,
					qmoOverlapSourceLink = eRPQuoteOperationInformationDto.qmoOverlapSourceLink,
					qmoPartID = eRPQuoteOperationInformationDto.qmoPartID,
					qmoPartRevisionID = eRPQuoteOperationInformationDto.qmoPartRevisionID,
					qmoPlantDepartmentID = eRPQuoteOperationInformationDto.qmoPlantDepartmentID,
					qmoPlantID = eRPQuoteOperationInformationDto.qmoPlantID,
					qmoProcessID = eRPQuoteOperationInformationDto.qmoProcessID,
					qmoProcessLongDescriptionRtf = eRPQuoteOperationInformationDto.qmoProcessLongDescriptionRtf,
					qmoProcessLongDescriptionText = eRPQuoteOperationInformationDto.qmoProcessLongDescriptionText,
					qmoProcessShortDescription = eRPQuoteOperationInformationDto.qmoProcessShortDescription,
					qmoProductionRate = eRPQuoteOperationInformationDto.qmoProductionRate,
					qmoProductionStandard = eRPQuoteOperationInformationDto.qmoProductionStandard,
					qmoPurchaseLocationID = eRPQuoteOperationInformationDto.qmoPurchaseLocationID,
					qmoQuantityBreak1 = eRPQuoteOperationInformationDto.qmoQuantityBreak1,
					qmoQuantityBreak2 = eRPQuoteOperationInformationDto.qmoQuantityBreak2,
					qmoQuantityBreak3 = eRPQuoteOperationInformationDto.qmoQuantityBreak3,
					qmoQuantityBreak4 = eRPQuoteOperationInformationDto.qmoQuantityBreak4,
					qmoQuantityBreak5 = eRPQuoteOperationInformationDto.qmoQuantityBreak5,
					qmoQuantityBreak6 = eRPQuoteOperationInformationDto.qmoQuantityBreak6,
					qmoQuantityBreak7 = eRPQuoteOperationInformationDto.qmoQuantityBreak7,
					qmoQuantityBreak8 = eRPQuoteOperationInformationDto.qmoQuantityBreak8,
					qmoQuantityBreak9 = eRPQuoteOperationInformationDto.qmoQuantityBreak9,
					qmoQuantityPerAssembly = eRPQuoteOperationInformationDto.qmoQuantityPerAssembly,
					qmoQueueTime = eRPQuoteOperationInformationDto.qmoQueueTime,
					qmoQuoteAssemblyID = eRPQuoteOperationInformationDto.qmoQuoteAssemblyID,
					qmoQuoteID = eRPQuoteOperationInformationDto.qmoQuoteID,
					qmoQuoteLineID = eRPQuoteOperationInformationDto.qmoQuoteLineID,
					qmoQuotingRate = eRPQuoteOperationInformationDto.qmoQuotingRate,
					qmoRowVersion = eRPQuoteOperationInformationDto.qmoRowVersion,
					qmoQuoteOperationID = eRPQuoteOperationInformationDto.qmoQuoteOperationID,
					qmoSetupCharge = eRPQuoteOperationInformationDto.qmoSetupCharge,
					qmoSetupHours = eRPQuoteOperationInformationDto.qmoSetupHours,
					qmoSetupRate = eRPQuoteOperationInformationDto.qmoSetupRate,
					qmoSfeMessageRTF = eRPQuoteOperationInformationDto.qmoSfeMessageRTF,
					qmoSfeMessageText = eRPQuoteOperationInformationDto.qmoSfeMessageText,
					qmoStandardFactor = eRPQuoteOperationInformationDto.qmoStandardFactor,
					qmoSupplierOrganizationID = eRPQuoteOperationInformationDto.qmoSupplierOrganizationID,
					qmoUnitCost1 = eRPQuoteOperationInformationDto.qmoUnitCost1,
					qmoUnitCost2 = eRPQuoteOperationInformationDto.qmoUnitCost2,
					qmoUnitCost3 = eRPQuoteOperationInformationDto.qmoUnitCost3,
					qmoUnitCost4 = eRPQuoteOperationInformationDto.qmoUnitCost4,
					qmoUnitCost5 = eRPQuoteOperationInformationDto.qmoUnitCost5,
					qmoUnitCost6 = eRPQuoteOperationInformationDto.qmoUnitCost6,
					qmoUnitCost7 = eRPQuoteOperationInformationDto.qmoUnitCost7,
					qmoUnitCost8 = eRPQuoteOperationInformationDto.qmoUnitCost8,
					qmoUnitCost9 = eRPQuoteOperationInformationDto.qmoUnitCost9,
					qmoUnitOfMeasure = eRPQuoteOperationInformationDto.qmoUnitOfMeasure,
					qmoWorkCenterID = eRPQuoteOperationInformationDto.qmoWorkCenterID,
					qmoWorkCenterMachineID = eRPQuoteOperationInformationDto.qmoWorkCenterMachineID,
					CustomFields = eRPQuoteOperationInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the QuoteOperations []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteOperationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteOperationDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuoteOperationDto>> Process_PutQuoteOperation(ERPQuoteOperationDto quoteOperation)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPQuoteOperationDto createdObject = null;
		ERPResponseMessageDto<ERPQuoteOperationDto> result;
		try
		{
			IERPQuoteOperationRepository iERPQuoteOperationRepository = (base.ERPQuoteOperationRepository = new ERPQuoteOperationRepository(base.ApiClientContext));
			using (iERPQuoteOperationRepository)
			{
				APIValidationInfoDto postResult = await base.ERPQuoteOperationRepository.SaveQuoteOperation(quoteOperation);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPQuoteOperationInformationDto eRPQuoteOperationInformationDto = await base.ERPQuoteOperationRepository.GetQuoteOperation(quoteOperation.qmoUniqueID);
					createdObject = new ERPQuoteOperationDto
					{
						qmoAdditionalSetupHours = eRPQuoteOperationInformationDto.qmoAdditionalSetupHours,
						qmoAdditionalSetupQuantity = eRPQuoteOperationInformationDto.qmoAdditionalSetupQuantity,
						qmoCreatedBy = eRPQuoteOperationInformationDto.qmoCreatedBy,
						qmoCreatedDate = eRPQuoteOperationInformationDto.qmoCreatedDate,
						qmoDocuments = eRPQuoteOperationInformationDto.qmoDocuments,
						qmoUniqueID = eRPQuoteOperationInformationDto.qmoUniqueID,
						qmoEstimatedUnitCost = eRPQuoteOperationInformationDto.qmoEstimatedUnitCost,
						qmoInspectionType = eRPQuoteOperationInformationDto.qmoInspectionType,
						qmoClosed = eRPQuoteOperationInformationDto.qmoClosed,
						qmoMachinesToSchedule = eRPQuoteOperationInformationDto.qmoMachinesToSchedule,
						qmoMachineType = eRPQuoteOperationInformationDto.qmoMachineType,
						qmoMinimumCharge = eRPQuoteOperationInformationDto.qmoMinimumCharge,
						qmoMoveTime = eRPQuoteOperationInformationDto.qmoMoveTime,
						qmoOperationType = eRPQuoteOperationInformationDto.qmoOperationType,
						qmoOverheadRate = eRPQuoteOperationInformationDto.qmoOverheadRate,
						qmoOverlap = eRPQuoteOperationInformationDto.qmoOverlap,
						qmoOverlapDestinationLink = eRPQuoteOperationInformationDto.qmoOverlapDestinationLink,
						qmoOverlapOffsetTime = eRPQuoteOperationInformationDto.qmoOverlapOffsetTime,
						qmoOverlapOperationID = eRPQuoteOperationInformationDto.qmoOverlapOperationID,
						qmoOverlapSourceLink = eRPQuoteOperationInformationDto.qmoOverlapSourceLink,
						qmoPartID = eRPQuoteOperationInformationDto.qmoPartID,
						qmoPartRevisionID = eRPQuoteOperationInformationDto.qmoPartRevisionID,
						qmoPlantDepartmentID = eRPQuoteOperationInformationDto.qmoPlantDepartmentID,
						qmoPlantID = eRPQuoteOperationInformationDto.qmoPlantID,
						qmoProcessID = eRPQuoteOperationInformationDto.qmoProcessID,
						qmoProcessLongDescriptionRtf = eRPQuoteOperationInformationDto.qmoProcessLongDescriptionRtf,
						qmoProcessLongDescriptionText = eRPQuoteOperationInformationDto.qmoProcessLongDescriptionText,
						qmoProcessShortDescription = eRPQuoteOperationInformationDto.qmoProcessShortDescription,
						qmoProductionRate = eRPQuoteOperationInformationDto.qmoProductionRate,
						qmoProductionStandard = eRPQuoteOperationInformationDto.qmoProductionStandard,
						qmoPurchaseLocationID = eRPQuoteOperationInformationDto.qmoPurchaseLocationID,
						qmoQuantityBreak1 = eRPQuoteOperationInformationDto.qmoQuantityBreak1,
						qmoQuantityBreak2 = eRPQuoteOperationInformationDto.qmoQuantityBreak2,
						qmoQuantityBreak3 = eRPQuoteOperationInformationDto.qmoQuantityBreak3,
						qmoQuantityBreak4 = eRPQuoteOperationInformationDto.qmoQuantityBreak4,
						qmoQuantityBreak5 = eRPQuoteOperationInformationDto.qmoQuantityBreak5,
						qmoQuantityBreak6 = eRPQuoteOperationInformationDto.qmoQuantityBreak6,
						qmoQuantityBreak7 = eRPQuoteOperationInformationDto.qmoQuantityBreak7,
						qmoQuantityBreak8 = eRPQuoteOperationInformationDto.qmoQuantityBreak8,
						qmoQuantityBreak9 = eRPQuoteOperationInformationDto.qmoQuantityBreak9,
						qmoQuantityPerAssembly = eRPQuoteOperationInformationDto.qmoQuantityPerAssembly,
						qmoQueueTime = eRPQuoteOperationInformationDto.qmoQueueTime,
						qmoQuoteAssemblyID = eRPQuoteOperationInformationDto.qmoQuoteAssemblyID,
						qmoQuoteID = eRPQuoteOperationInformationDto.qmoQuoteID,
						qmoQuoteLineID = eRPQuoteOperationInformationDto.qmoQuoteLineID,
						qmoQuotingRate = eRPQuoteOperationInformationDto.qmoQuotingRate,
						qmoRowVersion = eRPQuoteOperationInformationDto.qmoRowVersion,
						qmoQuoteOperationID = eRPQuoteOperationInformationDto.qmoQuoteOperationID,
						qmoSetupCharge = eRPQuoteOperationInformationDto.qmoSetupCharge,
						qmoSetupHours = eRPQuoteOperationInformationDto.qmoSetupHours,
						qmoSetupRate = eRPQuoteOperationInformationDto.qmoSetupRate,
						qmoSfeMessageRTF = eRPQuoteOperationInformationDto.qmoSfeMessageRTF,
						qmoSfeMessageText = eRPQuoteOperationInformationDto.qmoSfeMessageText,
						qmoStandardFactor = eRPQuoteOperationInformationDto.qmoStandardFactor,
						qmoSupplierOrganizationID = eRPQuoteOperationInformationDto.qmoSupplierOrganizationID,
						qmoUnitCost1 = eRPQuoteOperationInformationDto.qmoUnitCost1,
						qmoUnitCost2 = eRPQuoteOperationInformationDto.qmoUnitCost2,
						qmoUnitCost3 = eRPQuoteOperationInformationDto.qmoUnitCost3,
						qmoUnitCost4 = eRPQuoteOperationInformationDto.qmoUnitCost4,
						qmoUnitCost5 = eRPQuoteOperationInformationDto.qmoUnitCost5,
						qmoUnitCost6 = eRPQuoteOperationInformationDto.qmoUnitCost6,
						qmoUnitCost7 = eRPQuoteOperationInformationDto.qmoUnitCost7,
						qmoUnitCost8 = eRPQuoteOperationInformationDto.qmoUnitCost8,
						qmoUnitCost9 = eRPQuoteOperationInformationDto.qmoUnitCost9,
						qmoUnitOfMeasure = eRPQuoteOperationInformationDto.qmoUnitOfMeasure,
						qmoWorkCenterID = eRPQuoteOperationInformationDto.qmoWorkCenterID,
						qmoWorkCenterMachineID = eRPQuoteOperationInformationDto.qmoWorkCenterMachineID,
						CustomFields = eRPQuoteOperationInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing QuoteOperation [{quoteOperation.qmoUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteOperationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteQuoteOperation(Guid quoteOperationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteOperationRepository iERPQuoteOperationRepository = (base.ERPQuoteOperationRepository = new ERPQuoteOperationRepository(base.ApiClientContext));
		using (iERPQuoteOperationRepository)
		{
			if (!(await base.ERPQuoteOperationRepository.DoesQuoteOperationExist(quoteOperationId)))
			{
				base.ErrorsList.Add($"QuoteOperation [{quoteOperationId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPQuoteOperationInformationDto eRPQuoteOperationInformationDto = await base.ERPQuoteOperationRepository.GetQuoteOperation(quoteOperationId);
				string text = await base.ERPQuoteOperationRepository.WhereUsed("QuoteOperations", new object[4] { eRPQuoteOperationInformationDto.qmoQuoteID, eRPQuoteOperationInformationDto.qmoQuoteLineID, eRPQuoteOperationInformationDto.qmoQuoteAssemblyID, eRPQuoteOperationInformationDto.qmoQuoteOperationID }, new object[4] { "qmoQuoteID", "qmoQuoteLineID", "qmoQuoteAssemblyID", "qmoQuoteOperationID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("QuoteOperation cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPQuoteOperationDto>> Process_DeleteQuoteOperation(Guid quoteOperationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPQuoteOperationDto> result;
		try
		{
			IERPQuoteOperationRepository iERPQuoteOperationRepository = (base.ERPQuoteOperationRepository = new ERPQuoteOperationRepository(base.ApiClientContext));
			using (iERPQuoteOperationRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPQuoteOperationRepository.DeleteRowFromTable("QuoteOperations", "qmo", quoteOperationId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of QuoteOperation [{quoteOperationId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteOperationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPQuoteOperationDto()
			};
		}
		return result;
	}
}
