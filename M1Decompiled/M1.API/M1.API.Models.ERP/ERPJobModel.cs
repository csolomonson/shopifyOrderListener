using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPJobModel : ERPBaseModel, IERPJobModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllJobs(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPJobRepository iERPJobRepository = (base.ERPJobRepository = new ERPJobRepository(base.ApiClientContext));
		using (iERPJobRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPJobRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPJobRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPJobRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPJobRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetJob(Guid jobId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobRepository iERPJobRepository = (base.ERPJobRepository = new ERPJobRepository(base.ApiClientContext));
		using (iERPJobRepository)
		{
			if (!(await base.ERPJobRepository.DoesJobExist(jobId)))
			{
				errorsList.Add($"Job [{jobId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutJob(ERPJobDto job)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobRepository iERPJobRepository = (base.ERPJobRepository = new ERPJobRepository(base.ApiClientContext));
		using (iERPJobRepository)
		{
			if (!string.IsNullOrWhiteSpace(job.jmpPlantDepartmentID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { job.jmpPlantID, job.jmpPlantDepartmentID })))
			{
				errorsList.Add("jmpPlantDepartmentID [" + job.jmpPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpPlantID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { job.jmpPlantID })))
			{
				errorsList.Add("jmpPlantID [" + job.jmpPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpCustomerOrganizationID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { job.jmpCustomerOrganizationID })))
			{
				errorsList.Add("jmpCustomerOrganizationID [" + job.jmpCustomerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpPartID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { job.jmpPartID })))
			{
				errorsList.Add("jmpPartID [" + job.jmpPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpPartRevisionID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { job.jmpPartID, job.jmpPartRevisionID })))
			{
				errorsList.Add("jmpPartRevisionID [" + job.jmpPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpPartWareHouseLocationID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { job.jmpPartID, job.jmpPartRevisionID, job.jmpPartWareHouseLocationID })))
			{
				errorsList.Add("jmpPartWareHouseLocationID [" + job.jmpPartWareHouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpPartBinID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { job.jmpPartID, job.jmpPartRevisionID, job.jmpPartWareHouseLocationID, job.jmpPartBinID })))
			{
				errorsList.Add("jmpPartBinID [" + job.jmpPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpCallID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("Calls", new object[1] { "KBPCALLID" }, new object[1] { job.jmpCallID })))
			{
				errorsList.Add("jmpCallID [" + job.jmpCallID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpPlannerEmployeeID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { job.jmpPlannerEmployeeID })))
			{
				errorsList.Add("jmpPlannerEmployeeID [" + job.jmpPlannerEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpQuoteID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("Quotes", new object[1] { "QMPQUOTEID" }, new object[1] { job.jmpQuoteID })))
			{
				errorsList.Add("jmpQuoteID [" + job.jmpQuoteID + "] not found.");
			}
			if (job.jmpQuoteLineID > 0 && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("QuoteLines", new object[2] { "QMLQUOTEID", "QMLQUOTELINEID" }, new object[2] { job.jmpQuoteID, job.jmpQuoteLineID })))
			{
				errorsList.Add($"jmpQuoteLineID [{job.jmpQuoteLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpSourceMethodID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { job.jmpSourceMethodID })))
			{
				errorsList.Add("jmpSourceMethodID [" + job.jmpSourceMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpSourceRevisionID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { job.jmpSourceMethodID, job.jmpSourceRevisionID })))
			{
				errorsList.Add("jmpSourceRevisionID [" + job.jmpSourceRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpRmaClaimID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("RMAClaims", new object[1] { "RAPRMACLAIMID" }, new object[1] { job.jmpRmaClaimID })))
			{
				errorsList.Add("jmpRmaClaimID [" + job.jmpRmaClaimID + "] not found.");
			}
			if (job.jmpRmaClaimLineID > 0 && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("RMAClaimLines", new object[2] { "RALRMACLAIMID", "RALRMACLAIMLINEID" }, new object[2] { job.jmpRmaClaimID, job.jmpRmaClaimLineID })))
			{
				errorsList.Add($"jmpRmaClaimLineID [{job.jmpRmaClaimLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpProjectID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { job.jmpProjectID })))
			{
				errorsList.Add("jmpProjectID [" + job.jmpProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpProjectAreaID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { job.jmpProjectID, job.jmpProjectAreaID })))
			{
				errorsList.Add("jmpProjectAreaID [" + job.jmpProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpShipOrganizationID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { job.jmpShipOrganizationID })))
			{
				errorsList.Add("jmpShipOrganizationID [" + job.jmpShipOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpNonConformanceID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("NonConformances", new object[1] { "QARNONCONFORMANCEID" }, new object[1] { job.jmpNonConformanceID })))
			{
				errorsList.Add("jmpNonConformanceID [" + job.jmpNonConformanceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(job.jmpShipLocationID) && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { job.jmpShipOrganizationID, job.jmpShipLocationID })))
			{
				errorsList.Add("jmpShipLocationID [" + job.jmpShipLocationID + "] not found.");
			}
			if (job.jmpJobPriorityID > 0 && !(await base.ERPJobRepository.DoesRecordExistInTableUsingKeys("JobPriorities", new object[1] { "JMJJOBPRIORITYID" }, new object[1] { job.jmpJobPriorityID })))
			{
				errorsList.Add($"jmpJobPriorityID [{job.jmpJobPriorityID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPJobDto>>> Process_GetAllJobs(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPJobDto> allJobsDto = new List<ERPJobDto>();
		ERPResponseMessageDto<IList<ERPJobDto>> result;
		try
		{
			IERPJobRepository iERPJobRepository = (base.ERPJobRepository = new ERPJobRepository(base.ApiClientContext));
			using (iERPJobRepository)
			{
				foreach (ERPJobInformationDto item2 in await base.ERPJobRepository.GetAllJobs(pageSize, pageNumber, filter, orderBy))
				{
					ERPJobDto item = new ERPJobDto
					{
						jmpCallID = item2.jmpCallID,
						jmpClosedDate = item2.jmpClosedDate,
						jmpJobID = item2.jmpJobID,
						jmpCompletedDate = item2.jmpCompletedDate,
						jmpCreatedBy = item2.jmpCreatedBy,
						jmpCreatedDate = item2.jmpCreatedDate,
						jmpCustomerOrganizationID = item2.jmpCustomerOrganizationID,
						jmpDocuments = item2.jmpDocuments,
						jmpUniqueID = item2.jmpUniqueID,
						jmpInventoryQuantity = item2.jmpInventoryQuantity,
						jmpClosed = item2.jmpClosed,
						jmpFirm = item2.jmpFirm,
						jmpNestlinkProcessed = item2.jmpNestlinkProcessed,
						jmpOnHold = item2.jmpOnHold,
						jmpPlanningComplete = item2.jmpPlanningComplete,
						jmpProductionComplete = item2.jmpProductionComplete,
						jmpReadyToPrint = item2.jmpReadyToPrint,
						jmpReleasedToFloor = item2.jmpReleasedToFloor,
						jmpScheduleComplete = item2.jmpScheduleComplete,
						jmpScheduleLocked = item2.jmpScheduleLocked,
						jmpTimeAndMaterial = item2.jmpTimeAndMaterial,
						jmpJobDate = item2.jmpJobDate,
						jmpJobPriorityID = item2.jmpJobPriorityID,
						jmpNonConformanceID = item2.jmpNonConformanceID,
						jmpOrderQuantity = item2.jmpOrderQuantity,
						jmpPartBinID = item2.jmpPartBinID,
						jmpPartForecastPeriodID = item2.jmpPartForecastPeriodID,
						jmpPartForecastYearID = item2.jmpPartForecastYearID,
						jmpPartID = item2.jmpPartID,
						jmpPartLongDescriptionRtf = item2.jmpPartLongDescriptionRtf,
						jmpPartLongDescriptionText = item2.jmpPartLongDescriptionText,
						jmpPartRevisionID = item2.jmpPartRevisionID,
						jmpPartShortDescription = item2.jmpPartShortDescription,
						jmpPartWareHouseLocationID = item2.jmpPartWareHouseLocationID,
						jmpPlannerEmployeeID = item2.jmpPlannerEmployeeID,
						jmpPlantDepartmentID = item2.jmpPlantDepartmentID,
						jmpPlantID = item2.jmpPlantID,
						jmpProductionDueDate = item2.jmpProductionDueDate,
						jmpProductionNotesRTF = item2.jmpProductionNotesRTF,
						jmpProductionNotesText = item2.jmpProductionNotesText,
						jmpProductionQuantity = item2.jmpProductionQuantity,
						jmpProjectAreaID = item2.jmpProjectAreaID,
						jmpProjectID = item2.jmpProjectID,
						jmpQuantityCompleted = item2.jmpQuantityCompleted,
						jmpQuantityReceivedToInventory = item2.jmpQuantityReceivedToInventory,
						jmpQuantityShipped = item2.jmpQuantityShipped,
						jmpQuoteID = item2.jmpQuoteID,
						jmpQuoteLineID = item2.jmpQuoteLineID,
						jmpReworkDate = item2.jmpReworkDate,
						jmpReworkQuantity = item2.jmpReworkQuantity,
						jmpRmaClaimID = item2.jmpRmaClaimID,
						jmpRmaClaimLineID = item2.jmpRmaClaimLineID,
						jmpRowVersion = item2.jmpRowVersion,
						jmpScheduledDueDate = item2.jmpScheduledDueDate,
						jmpScheduledDueHour = item2.jmpScheduledDueHour,
						jmpScheduledStartDate = item2.jmpScheduledStartDate,
						jmpScheduledStartHour = item2.jmpScheduledStartHour,
						jmpScrapQuantity = item2.jmpScrapQuantity,
						jmpScrapQuantityCompleted = item2.jmpScrapQuantityCompleted,
						jmpShipLocationID = item2.jmpShipLocationID,
						jmpShipOrganizationID = item2.jmpShipOrganizationID,
						jmpSourceMethodID = item2.jmpSourceMethodID,
						jmpSourceRevisionID = item2.jmpSourceRevisionID,
						jmpUnitOfMeasure = item2.jmpUnitOfMeasure,
						CustomFields = item2.CustomFields
					};
					allJobsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Jobs]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPJobDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allJobsDto,
				RecordCount = allJobsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobDto>> Process_GetJob(Guid jobId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPJobDto jobDto = null;
		ERPResponseMessageDto<ERPJobDto> result;
		try
		{
			IERPJobRepository iERPJobRepository = (base.ERPJobRepository = new ERPJobRepository(base.ApiClientContext));
			using (iERPJobRepository)
			{
				ERPJobInformationDto eRPJobInformationDto = await base.ERPJobRepository.GetJob(jobId);
				jobDto = new ERPJobDto
				{
					jmpCallID = eRPJobInformationDto.jmpCallID,
					jmpClosedDate = eRPJobInformationDto.jmpClosedDate,
					jmpJobID = eRPJobInformationDto.jmpJobID,
					jmpCompletedDate = eRPJobInformationDto.jmpCompletedDate,
					jmpCreatedBy = eRPJobInformationDto.jmpCreatedBy,
					jmpCreatedDate = eRPJobInformationDto.jmpCreatedDate,
					jmpCustomerOrganizationID = eRPJobInformationDto.jmpCustomerOrganizationID,
					jmpDocuments = eRPJobInformationDto.jmpDocuments,
					jmpUniqueID = eRPJobInformationDto.jmpUniqueID,
					jmpInventoryQuantity = eRPJobInformationDto.jmpInventoryQuantity,
					jmpClosed = eRPJobInformationDto.jmpClosed,
					jmpFirm = eRPJobInformationDto.jmpFirm,
					jmpNestlinkProcessed = eRPJobInformationDto.jmpNestlinkProcessed,
					jmpOnHold = eRPJobInformationDto.jmpOnHold,
					jmpPlanningComplete = eRPJobInformationDto.jmpPlanningComplete,
					jmpProductionComplete = eRPJobInformationDto.jmpProductionComplete,
					jmpReadyToPrint = eRPJobInformationDto.jmpReadyToPrint,
					jmpReleasedToFloor = eRPJobInformationDto.jmpReleasedToFloor,
					jmpScheduleComplete = eRPJobInformationDto.jmpScheduleComplete,
					jmpScheduleLocked = eRPJobInformationDto.jmpScheduleLocked,
					jmpTimeAndMaterial = eRPJobInformationDto.jmpTimeAndMaterial,
					jmpJobDate = eRPJobInformationDto.jmpJobDate,
					jmpJobPriorityID = eRPJobInformationDto.jmpJobPriorityID,
					jmpNonConformanceID = eRPJobInformationDto.jmpNonConformanceID,
					jmpOrderQuantity = eRPJobInformationDto.jmpOrderQuantity,
					jmpPartBinID = eRPJobInformationDto.jmpPartBinID,
					jmpPartForecastPeriodID = eRPJobInformationDto.jmpPartForecastPeriodID,
					jmpPartForecastYearID = eRPJobInformationDto.jmpPartForecastYearID,
					jmpPartID = eRPJobInformationDto.jmpPartID,
					jmpPartLongDescriptionRtf = eRPJobInformationDto.jmpPartLongDescriptionRtf,
					jmpPartLongDescriptionText = eRPJobInformationDto.jmpPartLongDescriptionText,
					jmpPartRevisionID = eRPJobInformationDto.jmpPartRevisionID,
					jmpPartShortDescription = eRPJobInformationDto.jmpPartShortDescription,
					jmpPartWareHouseLocationID = eRPJobInformationDto.jmpPartWareHouseLocationID,
					jmpPlannerEmployeeID = eRPJobInformationDto.jmpPlannerEmployeeID,
					jmpPlantDepartmentID = eRPJobInformationDto.jmpPlantDepartmentID,
					jmpPlantID = eRPJobInformationDto.jmpPlantID,
					jmpProductionDueDate = eRPJobInformationDto.jmpProductionDueDate,
					jmpProductionNotesRTF = eRPJobInformationDto.jmpProductionNotesRTF,
					jmpProductionNotesText = eRPJobInformationDto.jmpProductionNotesText,
					jmpProductionQuantity = eRPJobInformationDto.jmpProductionQuantity,
					jmpProjectAreaID = eRPJobInformationDto.jmpProjectAreaID,
					jmpProjectID = eRPJobInformationDto.jmpProjectID,
					jmpQuantityCompleted = eRPJobInformationDto.jmpQuantityCompleted,
					jmpQuantityReceivedToInventory = eRPJobInformationDto.jmpQuantityReceivedToInventory,
					jmpQuantityShipped = eRPJobInformationDto.jmpQuantityShipped,
					jmpQuoteID = eRPJobInformationDto.jmpQuoteID,
					jmpQuoteLineID = eRPJobInformationDto.jmpQuoteLineID,
					jmpReworkDate = eRPJobInformationDto.jmpReworkDate,
					jmpReworkQuantity = eRPJobInformationDto.jmpReworkQuantity,
					jmpRmaClaimID = eRPJobInformationDto.jmpRmaClaimID,
					jmpRmaClaimLineID = eRPJobInformationDto.jmpRmaClaimLineID,
					jmpRowVersion = eRPJobInformationDto.jmpRowVersion,
					jmpScheduledDueDate = eRPJobInformationDto.jmpScheduledDueDate,
					jmpScheduledDueHour = eRPJobInformationDto.jmpScheduledDueHour,
					jmpScheduledStartDate = eRPJobInformationDto.jmpScheduledStartDate,
					jmpScheduledStartHour = eRPJobInformationDto.jmpScheduledStartHour,
					jmpScrapQuantity = eRPJobInformationDto.jmpScrapQuantity,
					jmpScrapQuantityCompleted = eRPJobInformationDto.jmpScrapQuantityCompleted,
					jmpShipLocationID = eRPJobInformationDto.jmpShipLocationID,
					jmpShipOrganizationID = eRPJobInformationDto.jmpShipOrganizationID,
					jmpSourceMethodID = eRPJobInformationDto.jmpSourceMethodID,
					jmpSourceRevisionID = eRPJobInformationDto.jmpSourceRevisionID,
					jmpUnitOfMeasure = eRPJobInformationDto.jmpUnitOfMeasure,
					CustomFields = eRPJobInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Jobs []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobDto>> Process_PutJob(ERPJobDto job)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPJobDto createdObject = null;
		ERPResponseMessageDto<ERPJobDto> result;
		try
		{
			IERPJobRepository iERPJobRepository = (base.ERPJobRepository = new ERPJobRepository(base.ApiClientContext));
			using (iERPJobRepository)
			{
				APIValidationInfoDto postResult = await base.ERPJobRepository.SaveJob(job);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPJobInformationDto eRPJobInformationDto = await base.ERPJobRepository.GetJob(job.jmpUniqueID);
					createdObject = new ERPJobDto
					{
						jmpCallID = eRPJobInformationDto.jmpCallID,
						jmpClosedDate = eRPJobInformationDto.jmpClosedDate,
						jmpJobID = eRPJobInformationDto.jmpJobID,
						jmpCompletedDate = eRPJobInformationDto.jmpCompletedDate,
						jmpCreatedBy = eRPJobInformationDto.jmpCreatedBy,
						jmpCreatedDate = eRPJobInformationDto.jmpCreatedDate,
						jmpCustomerOrganizationID = eRPJobInformationDto.jmpCustomerOrganizationID,
						jmpDocuments = eRPJobInformationDto.jmpDocuments,
						jmpUniqueID = eRPJobInformationDto.jmpUniqueID,
						jmpInventoryQuantity = eRPJobInformationDto.jmpInventoryQuantity,
						jmpClosed = eRPJobInformationDto.jmpClosed,
						jmpFirm = eRPJobInformationDto.jmpFirm,
						jmpNestlinkProcessed = eRPJobInformationDto.jmpNestlinkProcessed,
						jmpOnHold = eRPJobInformationDto.jmpOnHold,
						jmpPlanningComplete = eRPJobInformationDto.jmpPlanningComplete,
						jmpProductionComplete = eRPJobInformationDto.jmpProductionComplete,
						jmpReadyToPrint = eRPJobInformationDto.jmpReadyToPrint,
						jmpReleasedToFloor = eRPJobInformationDto.jmpReleasedToFloor,
						jmpScheduleComplete = eRPJobInformationDto.jmpScheduleComplete,
						jmpScheduleLocked = eRPJobInformationDto.jmpScheduleLocked,
						jmpTimeAndMaterial = eRPJobInformationDto.jmpTimeAndMaterial,
						jmpJobDate = eRPJobInformationDto.jmpJobDate,
						jmpJobPriorityID = eRPJobInformationDto.jmpJobPriorityID,
						jmpNonConformanceID = eRPJobInformationDto.jmpNonConformanceID,
						jmpOrderQuantity = eRPJobInformationDto.jmpOrderQuantity,
						jmpPartBinID = eRPJobInformationDto.jmpPartBinID,
						jmpPartForecastPeriodID = eRPJobInformationDto.jmpPartForecastPeriodID,
						jmpPartForecastYearID = eRPJobInformationDto.jmpPartForecastYearID,
						jmpPartID = eRPJobInformationDto.jmpPartID,
						jmpPartLongDescriptionRtf = eRPJobInformationDto.jmpPartLongDescriptionRtf,
						jmpPartLongDescriptionText = eRPJobInformationDto.jmpPartLongDescriptionText,
						jmpPartRevisionID = eRPJobInformationDto.jmpPartRevisionID,
						jmpPartShortDescription = eRPJobInformationDto.jmpPartShortDescription,
						jmpPartWareHouseLocationID = eRPJobInformationDto.jmpPartWareHouseLocationID,
						jmpPlannerEmployeeID = eRPJobInformationDto.jmpPlannerEmployeeID,
						jmpPlantDepartmentID = eRPJobInformationDto.jmpPlantDepartmentID,
						jmpPlantID = eRPJobInformationDto.jmpPlantID,
						jmpProductionDueDate = eRPJobInformationDto.jmpProductionDueDate,
						jmpProductionNotesRTF = eRPJobInformationDto.jmpProductionNotesRTF,
						jmpProductionNotesText = eRPJobInformationDto.jmpProductionNotesText,
						jmpProductionQuantity = eRPJobInformationDto.jmpProductionQuantity,
						jmpProjectAreaID = eRPJobInformationDto.jmpProjectAreaID,
						jmpProjectID = eRPJobInformationDto.jmpProjectID,
						jmpQuantityCompleted = eRPJobInformationDto.jmpQuantityCompleted,
						jmpQuantityReceivedToInventory = eRPJobInformationDto.jmpQuantityReceivedToInventory,
						jmpQuantityShipped = eRPJobInformationDto.jmpQuantityShipped,
						jmpQuoteID = eRPJobInformationDto.jmpQuoteID,
						jmpQuoteLineID = eRPJobInformationDto.jmpQuoteLineID,
						jmpReworkDate = eRPJobInformationDto.jmpReworkDate,
						jmpReworkQuantity = eRPJobInformationDto.jmpReworkQuantity,
						jmpRmaClaimID = eRPJobInformationDto.jmpRmaClaimID,
						jmpRmaClaimLineID = eRPJobInformationDto.jmpRmaClaimLineID,
						jmpRowVersion = eRPJobInformationDto.jmpRowVersion,
						jmpScheduledDueDate = eRPJobInformationDto.jmpScheduledDueDate,
						jmpScheduledDueHour = eRPJobInformationDto.jmpScheduledDueHour,
						jmpScheduledStartDate = eRPJobInformationDto.jmpScheduledStartDate,
						jmpScheduledStartHour = eRPJobInformationDto.jmpScheduledStartHour,
						jmpScrapQuantity = eRPJobInformationDto.jmpScrapQuantity,
						jmpScrapQuantityCompleted = eRPJobInformationDto.jmpScrapQuantityCompleted,
						jmpShipLocationID = eRPJobInformationDto.jmpShipLocationID,
						jmpShipOrganizationID = eRPJobInformationDto.jmpShipOrganizationID,
						jmpSourceMethodID = eRPJobInformationDto.jmpSourceMethodID,
						jmpSourceRevisionID = eRPJobInformationDto.jmpSourceRevisionID,
						jmpUnitOfMeasure = eRPJobInformationDto.jmpUnitOfMeasure,
						CustomFields = eRPJobInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Job [{job.jmpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteJob(Guid jobId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobRepository iERPJobRepository = (base.ERPJobRepository = new ERPJobRepository(base.ApiClientContext));
		using (iERPJobRepository)
		{
			if (!(await base.ERPJobRepository.DoesJobExist(jobId)))
			{
				base.ErrorsList.Add($"Job [{jobId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPJobInformationDto eRPJobInformationDto = await base.ERPJobRepository.GetJob(jobId);
				string text = await base.ERPJobRepository.WhereUsed("Jobs", new object[1] { eRPJobInformationDto.jmpJobID }, new object[1] { "jmpJobID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Job cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPJobDto>> Process_DeleteJob(Guid jobId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPJobDto> result;
		try
		{
			IERPJobRepository iERPJobRepository = (base.ERPJobRepository = new ERPJobRepository(base.ApiClientContext));
			using (iERPJobRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPJobRepository.DeleteRowFromTable("Jobs", "jmp", jobId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Job [{jobId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPJobDto()
			};
		}
		return result;
	}
}
