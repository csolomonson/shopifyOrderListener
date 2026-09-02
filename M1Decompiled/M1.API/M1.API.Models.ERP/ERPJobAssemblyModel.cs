using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPJobAssemblyModel : ERPBaseModel, IERPJobAssemblyModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllJobAssemblies(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPJobAssemblyRepository iERPJobAssemblyRepository = (base.ERPJobAssemblyRepository = new ERPJobAssemblyRepository(base.ApiClientContext));
		using (iERPJobAssemblyRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPJobAssemblyRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPJobAssemblyRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPJobAssemblyRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPJobAssemblyRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetJobAssembly(Guid jobAssemblyId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobAssemblyRepository iERPJobAssemblyRepository = (base.ERPJobAssemblyRepository = new ERPJobAssemblyRepository(base.ApiClientContext));
		using (iERPJobAssemblyRepository)
		{
			if (!(await base.ERPJobAssemblyRepository.DoesJobAssemblyExist(jobAssemblyId)))
			{
				errorsList.Add($"JobAssembly [{jobAssemblyId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutJobAssembly(ERPJobAssemblyDto jobAssembly)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobAssemblyRepository iERPJobAssemblyRepository = (base.ERPJobAssemblyRepository = new ERPJobAssemblyRepository(base.ApiClientContext));
		using (iERPJobAssemblyRepository)
		{
			if (!string.IsNullOrWhiteSpace(jobAssembly.jmaJobID) && !(await base.ERPJobAssemblyRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { jobAssembly.jmaJobID })))
			{
				errorsList.Add("jmaJobID [" + jobAssembly.jmaJobID + "] not found.");
			}
			if (jobAssembly.jmaParentAssemblyID > 0 && !(await base.ERPJobAssemblyRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { jobAssembly.jmaJobID, jobAssembly.jmaParentAssemblyID })))
			{
				errorsList.Add($"jmaParentAssemblyID [{jobAssembly.jmaParentAssemblyID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobAssembly.jmaSourceMethodID) && !(await base.ERPJobAssemblyRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { jobAssembly.jmaSourceMethodID })))
			{
				errorsList.Add("jmaSourceMethodID [" + jobAssembly.jmaSourceMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobAssembly.jmaSourceRevisionID) && !(await base.ERPJobAssemblyRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { jobAssembly.jmaSourceMethodID, jobAssembly.jmaSourceRevisionID })))
			{
				errorsList.Add("jmaSourceRevisionID [" + jobAssembly.jmaSourceRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobAssembly.jmaPartID) && !(await base.ERPJobAssemblyRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { jobAssembly.jmaPartID })))
			{
				errorsList.Add("jmaPartID [" + jobAssembly.jmaPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobAssembly.jmaPartRevisionID) && !(await base.ERPJobAssemblyRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { jobAssembly.jmaPartID, jobAssembly.jmaPartRevisionID })))
			{
				errorsList.Add("jmaPartRevisionID [" + jobAssembly.jmaPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobAssembly.jmaPartWareHouseLocationID) && !(await base.ERPJobAssemblyRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { jobAssembly.jmaPartID, jobAssembly.jmaPartRevisionID, jobAssembly.jmaPartWareHouseLocationID })))
			{
				errorsList.Add("jmaPartWareHouseLocationID [" + jobAssembly.jmaPartWareHouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobAssembly.jmaPartBinID) && !(await base.ERPJobAssemblyRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { jobAssembly.jmaPartID, jobAssembly.jmaPartRevisionID, jobAssembly.jmaPartWareHouseLocationID, jobAssembly.jmaPartBinID })))
			{
				errorsList.Add("jmaPartBinID [" + jobAssembly.jmaPartBinID + "] not found.");
			}
			if (jobAssembly.jmaOverlapSourceOperationID > 0 && !(await base.ERPJobAssemblyRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { jobAssembly.jmaJobID, jobAssembly.jmaJobAssemblyID, jobAssembly.jmaOverlapSourceOperationID })))
			{
				errorsList.Add($"jmaOverlapSourceOperationID [{jobAssembly.jmaOverlapSourceOperationID}] not found.");
			}
			if (jobAssembly.jmaOverlapOperationID > 0 && !(await base.ERPJobAssemblyRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { jobAssembly.jmaJobID, jobAssembly.jmaParentAssemblyID, jobAssembly.jmaOverlapOperationID })))
			{
				errorsList.Add($"jmaOverlapOperationID [{jobAssembly.jmaOverlapOperationID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPJobAssemblyDto>>> Process_GetAllJobAssemblies(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPJobAssemblyDto> allJobAssembliesDto = new List<ERPJobAssemblyDto>();
		ERPResponseMessageDto<IList<ERPJobAssemblyDto>> result;
		try
		{
			IERPJobAssemblyRepository iERPJobAssemblyRepository = (base.ERPJobAssemblyRepository = new ERPJobAssemblyRepository(base.ApiClientContext));
			using (iERPJobAssemblyRepository)
			{
				foreach (ERPJobAssemblyInformationDto item2 in await base.ERPJobAssemblyRepository.GetAllJobAssemblies(pageSize, pageNumber, filter, orderBy))
				{
					ERPJobAssemblyDto item = new ERPJobAssemblyDto
					{
						jmaAssemblyOverlap = item2.jmaAssemblyOverlap,
						jmaCompletedDate = item2.jmaCompletedDate,
						jmaCreatedBy = item2.jmaCreatedBy,
						jmaCreatedDate = item2.jmaCreatedDate,
						jmaDocuments = item2.jmaDocuments,
						jmaUniqueID = item2.jmaUniqueID,
						jmaEstimatedUnitCost = item2.jmaEstimatedUnitCost,
						jmaInventoryQuantity = item2.jmaInventoryQuantity,
						jmaClosed = item2.jmaClosed,
						jmaIssuedComplete = item2.jmaIssuedComplete,
						jmaProductionComplete = item2.jmaProductionComplete,
						jmaPullAllFromStock = item2.jmaPullAllFromStock,
						jmaReceivedComplete = item2.jmaReceivedComplete,
						jmaJobID = item2.jmaJobID,
						jmaLevel = item2.jmaLevel,
						jmaOrderQuantity = item2.jmaOrderQuantity,
						jmaOverlapDestinationLink = item2.jmaOverlapDestinationLink,
						jmaOverlapOffsetTime = item2.jmaOverlapOffsetTime,
						jmaOverlapOperationID = item2.jmaOverlapOperationID,
						jmaOverlapSourceLink = item2.jmaOverlapSourceLink,
						jmaOverlapSourceOperationID = item2.jmaOverlapSourceOperationID,
						jmaOverlapType = item2.jmaOverlapType,
						jmaParentAssemblyID = item2.jmaParentAssemblyID,
						jmaPartBinID = item2.jmaPartBinID,
						jmaPartID = item2.jmaPartID,
						jmaPartLongDescriptionRtf = item2.jmaPartLongDescriptionRtf,
						jmaPartLongDescriptionText = item2.jmaPartLongDescriptionText,
						jmaPartRevisionID = item2.jmaPartRevisionID,
						jmaPartShortDescription = item2.jmaPartShortDescription,
						jmaPartWareHouseLocationID = item2.jmaPartWareHouseLocationID,
						jmaProductionNotesRTF = item2.jmaProductionNotesRTF,
						jmaProductionNotesText = item2.jmaProductionNotesText,
						jmaProductionQuantity = item2.jmaProductionQuantity,
						jmaQuantityCompleted = item2.jmaQuantityCompleted,
						jmaQuantityIssued = item2.jmaQuantityIssued,
						jmaQuantityPerParent = item2.jmaQuantityPerParent,
						jmaQuantityReceivedToInventory = item2.jmaQuantityReceivedToInventory,
						jmaQuantityToInspect = item2.jmaQuantityToInspect,
						jmaQuantityToMake = item2.jmaQuantityToMake,
						jmaQuantityToPull = item2.jmaQuantityToPull,
						jmaQuantityToReturn = item2.jmaQuantityToReturn,
						jmaReworkDate = item2.jmaReworkDate,
						jmaReworkQuantity = item2.jmaReworkQuantity,
						jmaRowVersion = item2.jmaRowVersion,
						jmaScheduledDueDate = item2.jmaScheduledDueDate,
						jmaScheduledDueHour = item2.jmaScheduledDueHour,
						jmaScheduledStartDate = item2.jmaScheduledStartDate,
						jmaScheduledStartHour = item2.jmaScheduledStartHour,
						jmaScrapQuantity = item2.jmaScrapQuantity,
						jmaScrapQuantityCompleted = item2.jmaScrapQuantityCompleted,
						jmaJobAssemblyID = item2.jmaJobAssemblyID,
						jmaSourceMethodID = item2.jmaSourceMethodID,
						jmaSourceRevisionID = item2.jmaSourceRevisionID,
						jmaUnitOfMeasure = item2.jmaUnitOfMeasure,
						CustomFields = item2.CustomFields
					};
					allJobAssembliesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all JobAssemblies]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPJobAssemblyDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allJobAssembliesDto,
				RecordCount = allJobAssembliesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobAssemblyDto>> Process_GetJobAssembly(Guid jobAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPJobAssemblyDto jobAssemblyDto = null;
		ERPResponseMessageDto<ERPJobAssemblyDto> result;
		try
		{
			IERPJobAssemblyRepository iERPJobAssemblyRepository = (base.ERPJobAssemblyRepository = new ERPJobAssemblyRepository(base.ApiClientContext));
			using (iERPJobAssemblyRepository)
			{
				ERPJobAssemblyInformationDto eRPJobAssemblyInformationDto = await base.ERPJobAssemblyRepository.GetJobAssembly(jobAssemblyId);
				jobAssemblyDto = new ERPJobAssemblyDto
				{
					jmaAssemblyOverlap = eRPJobAssemblyInformationDto.jmaAssemblyOverlap,
					jmaCompletedDate = eRPJobAssemblyInformationDto.jmaCompletedDate,
					jmaCreatedBy = eRPJobAssemblyInformationDto.jmaCreatedBy,
					jmaCreatedDate = eRPJobAssemblyInformationDto.jmaCreatedDate,
					jmaDocuments = eRPJobAssemblyInformationDto.jmaDocuments,
					jmaUniqueID = eRPJobAssemblyInformationDto.jmaUniqueID,
					jmaEstimatedUnitCost = eRPJobAssemblyInformationDto.jmaEstimatedUnitCost,
					jmaInventoryQuantity = eRPJobAssemblyInformationDto.jmaInventoryQuantity,
					jmaClosed = eRPJobAssemblyInformationDto.jmaClosed,
					jmaIssuedComplete = eRPJobAssemblyInformationDto.jmaIssuedComplete,
					jmaProductionComplete = eRPJobAssemblyInformationDto.jmaProductionComplete,
					jmaPullAllFromStock = eRPJobAssemblyInformationDto.jmaPullAllFromStock,
					jmaReceivedComplete = eRPJobAssemblyInformationDto.jmaReceivedComplete,
					jmaJobID = eRPJobAssemblyInformationDto.jmaJobID,
					jmaLevel = eRPJobAssemblyInformationDto.jmaLevel,
					jmaOrderQuantity = eRPJobAssemblyInformationDto.jmaOrderQuantity,
					jmaOverlapDestinationLink = eRPJobAssemblyInformationDto.jmaOverlapDestinationLink,
					jmaOverlapOffsetTime = eRPJobAssemblyInformationDto.jmaOverlapOffsetTime,
					jmaOverlapOperationID = eRPJobAssemblyInformationDto.jmaOverlapOperationID,
					jmaOverlapSourceLink = eRPJobAssemblyInformationDto.jmaOverlapSourceLink,
					jmaOverlapSourceOperationID = eRPJobAssemblyInformationDto.jmaOverlapSourceOperationID,
					jmaOverlapType = eRPJobAssemblyInformationDto.jmaOverlapType,
					jmaParentAssemblyID = eRPJobAssemblyInformationDto.jmaParentAssemblyID,
					jmaPartBinID = eRPJobAssemblyInformationDto.jmaPartBinID,
					jmaPartID = eRPJobAssemblyInformationDto.jmaPartID,
					jmaPartLongDescriptionRtf = eRPJobAssemblyInformationDto.jmaPartLongDescriptionRtf,
					jmaPartLongDescriptionText = eRPJobAssemblyInformationDto.jmaPartLongDescriptionText,
					jmaPartRevisionID = eRPJobAssemblyInformationDto.jmaPartRevisionID,
					jmaPartShortDescription = eRPJobAssemblyInformationDto.jmaPartShortDescription,
					jmaPartWareHouseLocationID = eRPJobAssemblyInformationDto.jmaPartWareHouseLocationID,
					jmaProductionNotesRTF = eRPJobAssemblyInformationDto.jmaProductionNotesRTF,
					jmaProductionNotesText = eRPJobAssemblyInformationDto.jmaProductionNotesText,
					jmaProductionQuantity = eRPJobAssemblyInformationDto.jmaProductionQuantity,
					jmaQuantityCompleted = eRPJobAssemblyInformationDto.jmaQuantityCompleted,
					jmaQuantityIssued = eRPJobAssemblyInformationDto.jmaQuantityIssued,
					jmaQuantityPerParent = eRPJobAssemblyInformationDto.jmaQuantityPerParent,
					jmaQuantityReceivedToInventory = eRPJobAssemblyInformationDto.jmaQuantityReceivedToInventory,
					jmaQuantityToInspect = eRPJobAssemblyInformationDto.jmaQuantityToInspect,
					jmaQuantityToMake = eRPJobAssemblyInformationDto.jmaQuantityToMake,
					jmaQuantityToPull = eRPJobAssemblyInformationDto.jmaQuantityToPull,
					jmaQuantityToReturn = eRPJobAssemblyInformationDto.jmaQuantityToReturn,
					jmaReworkDate = eRPJobAssemblyInformationDto.jmaReworkDate,
					jmaReworkQuantity = eRPJobAssemblyInformationDto.jmaReworkQuantity,
					jmaRowVersion = eRPJobAssemblyInformationDto.jmaRowVersion,
					jmaScheduledDueDate = eRPJobAssemblyInformationDto.jmaScheduledDueDate,
					jmaScheduledDueHour = eRPJobAssemblyInformationDto.jmaScheduledDueHour,
					jmaScheduledStartDate = eRPJobAssemblyInformationDto.jmaScheduledStartDate,
					jmaScheduledStartHour = eRPJobAssemblyInformationDto.jmaScheduledStartHour,
					jmaScrapQuantity = eRPJobAssemblyInformationDto.jmaScrapQuantity,
					jmaScrapQuantityCompleted = eRPJobAssemblyInformationDto.jmaScrapQuantityCompleted,
					jmaJobAssemblyID = eRPJobAssemblyInformationDto.jmaJobAssemblyID,
					jmaSourceMethodID = eRPJobAssemblyInformationDto.jmaSourceMethodID,
					jmaSourceRevisionID = eRPJobAssemblyInformationDto.jmaSourceRevisionID,
					jmaUnitOfMeasure = eRPJobAssemblyInformationDto.jmaUnitOfMeasure,
					CustomFields = eRPJobAssemblyInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the JobAssemblies []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobAssemblyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobAssemblyDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobAssemblyDto>> Process_PutJobAssembly(ERPJobAssemblyDto jobAssembly)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPJobAssemblyDto createdObject = null;
		ERPResponseMessageDto<ERPJobAssemblyDto> result;
		try
		{
			IERPJobAssemblyRepository iERPJobAssemblyRepository = (base.ERPJobAssemblyRepository = new ERPJobAssemblyRepository(base.ApiClientContext));
			using (iERPJobAssemblyRepository)
			{
				APIValidationInfoDto postResult = await base.ERPJobAssemblyRepository.SaveJobAssembly(jobAssembly);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPJobAssemblyInformationDto eRPJobAssemblyInformationDto = await base.ERPJobAssemblyRepository.GetJobAssembly(jobAssembly.jmaUniqueID);
					createdObject = new ERPJobAssemblyDto
					{
						jmaAssemblyOverlap = eRPJobAssemblyInformationDto.jmaAssemblyOverlap,
						jmaCompletedDate = eRPJobAssemblyInformationDto.jmaCompletedDate,
						jmaCreatedBy = eRPJobAssemblyInformationDto.jmaCreatedBy,
						jmaCreatedDate = eRPJobAssemblyInformationDto.jmaCreatedDate,
						jmaDocuments = eRPJobAssemblyInformationDto.jmaDocuments,
						jmaUniqueID = eRPJobAssemblyInformationDto.jmaUniqueID,
						jmaEstimatedUnitCost = eRPJobAssemblyInformationDto.jmaEstimatedUnitCost,
						jmaInventoryQuantity = eRPJobAssemblyInformationDto.jmaInventoryQuantity,
						jmaClosed = eRPJobAssemblyInformationDto.jmaClosed,
						jmaIssuedComplete = eRPJobAssemblyInformationDto.jmaIssuedComplete,
						jmaProductionComplete = eRPJobAssemblyInformationDto.jmaProductionComplete,
						jmaPullAllFromStock = eRPJobAssemblyInformationDto.jmaPullAllFromStock,
						jmaReceivedComplete = eRPJobAssemblyInformationDto.jmaReceivedComplete,
						jmaJobID = eRPJobAssemblyInformationDto.jmaJobID,
						jmaLevel = eRPJobAssemblyInformationDto.jmaLevel,
						jmaOrderQuantity = eRPJobAssemblyInformationDto.jmaOrderQuantity,
						jmaOverlapDestinationLink = eRPJobAssemblyInformationDto.jmaOverlapDestinationLink,
						jmaOverlapOffsetTime = eRPJobAssemblyInformationDto.jmaOverlapOffsetTime,
						jmaOverlapOperationID = eRPJobAssemblyInformationDto.jmaOverlapOperationID,
						jmaOverlapSourceLink = eRPJobAssemblyInformationDto.jmaOverlapSourceLink,
						jmaOverlapSourceOperationID = eRPJobAssemblyInformationDto.jmaOverlapSourceOperationID,
						jmaOverlapType = eRPJobAssemblyInformationDto.jmaOverlapType,
						jmaParentAssemblyID = eRPJobAssemblyInformationDto.jmaParentAssemblyID,
						jmaPartBinID = eRPJobAssemblyInformationDto.jmaPartBinID,
						jmaPartID = eRPJobAssemblyInformationDto.jmaPartID,
						jmaPartLongDescriptionRtf = eRPJobAssemblyInformationDto.jmaPartLongDescriptionRtf,
						jmaPartLongDescriptionText = eRPJobAssemblyInformationDto.jmaPartLongDescriptionText,
						jmaPartRevisionID = eRPJobAssemblyInformationDto.jmaPartRevisionID,
						jmaPartShortDescription = eRPJobAssemblyInformationDto.jmaPartShortDescription,
						jmaPartWareHouseLocationID = eRPJobAssemblyInformationDto.jmaPartWareHouseLocationID,
						jmaProductionNotesRTF = eRPJobAssemblyInformationDto.jmaProductionNotesRTF,
						jmaProductionNotesText = eRPJobAssemblyInformationDto.jmaProductionNotesText,
						jmaProductionQuantity = eRPJobAssemblyInformationDto.jmaProductionQuantity,
						jmaQuantityCompleted = eRPJobAssemblyInformationDto.jmaQuantityCompleted,
						jmaQuantityIssued = eRPJobAssemblyInformationDto.jmaQuantityIssued,
						jmaQuantityPerParent = eRPJobAssemblyInformationDto.jmaQuantityPerParent,
						jmaQuantityReceivedToInventory = eRPJobAssemblyInformationDto.jmaQuantityReceivedToInventory,
						jmaQuantityToInspect = eRPJobAssemblyInformationDto.jmaQuantityToInspect,
						jmaQuantityToMake = eRPJobAssemblyInformationDto.jmaQuantityToMake,
						jmaQuantityToPull = eRPJobAssemblyInformationDto.jmaQuantityToPull,
						jmaQuantityToReturn = eRPJobAssemblyInformationDto.jmaQuantityToReturn,
						jmaReworkDate = eRPJobAssemblyInformationDto.jmaReworkDate,
						jmaReworkQuantity = eRPJobAssemblyInformationDto.jmaReworkQuantity,
						jmaRowVersion = eRPJobAssemblyInformationDto.jmaRowVersion,
						jmaScheduledDueDate = eRPJobAssemblyInformationDto.jmaScheduledDueDate,
						jmaScheduledDueHour = eRPJobAssemblyInformationDto.jmaScheduledDueHour,
						jmaScheduledStartDate = eRPJobAssemblyInformationDto.jmaScheduledStartDate,
						jmaScheduledStartHour = eRPJobAssemblyInformationDto.jmaScheduledStartHour,
						jmaScrapQuantity = eRPJobAssemblyInformationDto.jmaScrapQuantity,
						jmaScrapQuantityCompleted = eRPJobAssemblyInformationDto.jmaScrapQuantityCompleted,
						jmaJobAssemblyID = eRPJobAssemblyInformationDto.jmaJobAssemblyID,
						jmaSourceMethodID = eRPJobAssemblyInformationDto.jmaSourceMethodID,
						jmaSourceRevisionID = eRPJobAssemblyInformationDto.jmaSourceRevisionID,
						jmaUnitOfMeasure = eRPJobAssemblyInformationDto.jmaUnitOfMeasure,
						CustomFields = eRPJobAssemblyInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing JobAssembly [{jobAssembly.jmaUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobAssemblyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteJobAssembly(Guid jobAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobAssemblyRepository iERPJobAssemblyRepository = (base.ERPJobAssemblyRepository = new ERPJobAssemblyRepository(base.ApiClientContext));
		using (iERPJobAssemblyRepository)
		{
			if (!(await base.ERPJobAssemblyRepository.DoesJobAssemblyExist(jobAssemblyId)))
			{
				base.ErrorsList.Add($"JobAssembly [{jobAssemblyId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPJobAssemblyInformationDto eRPJobAssemblyInformationDto = await base.ERPJobAssemblyRepository.GetJobAssembly(jobAssemblyId);
				string text = await base.ERPJobAssemblyRepository.WhereUsed("JobAssemblies", new object[2] { eRPJobAssemblyInformationDto.jmaJobID, eRPJobAssemblyInformationDto.jmaJobAssemblyID }, new object[2] { "jmaJobID", "jmaJobAssemblyID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("JobAssembly cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPJobAssemblyDto>> Process_DeleteJobAssembly(Guid jobAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPJobAssemblyDto> result;
		try
		{
			IERPJobAssemblyRepository iERPJobAssemblyRepository = (base.ERPJobAssemblyRepository = new ERPJobAssemblyRepository(base.ApiClientContext));
			using (iERPJobAssemblyRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPJobAssemblyRepository.DeleteRowFromTable("JobAssemblies", "jma", jobAssemblyId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of JobAssembly [{jobAssemblyId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobAssemblyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPJobAssemblyDto()
			};
		}
		return result;
	}
}
