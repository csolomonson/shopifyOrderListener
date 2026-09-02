using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPNonConformanceModel : ERPBaseModel, IERPNonConformanceModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllNonConformances(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPNonConformanceRepository iERPNonConformanceRepository = (base.ERPNonConformanceRepository = new ERPNonConformanceRepository(base.ApiClientContext));
		using (iERPNonConformanceRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPNonConformanceRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPNonConformanceRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPNonConformanceRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPNonConformanceRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetNonConformance(Guid nonConformanceId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPNonConformanceRepository iERPNonConformanceRepository = (base.ERPNonConformanceRepository = new ERPNonConformanceRepository(base.ApiClientContext));
		using (iERPNonConformanceRepository)
		{
			if (!(await base.ERPNonConformanceRepository.DoesNonConformanceExist(nonConformanceId)))
			{
				errorsList.Add($"NonConformance [{nonConformanceId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutNonConformance(ERPNonConformanceDto nonConformance)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPNonConformanceRepository iERPNonConformanceRepository = (base.ERPNonConformanceRepository = new ERPNonConformanceRepository(base.ApiClientContext));
		using (iERPNonConformanceRepository)
		{
			if (!string.IsNullOrWhiteSpace(nonConformance.qarInspectionID) && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("Inspections", new object[1] { "QAPINSPECTIONID" }, new object[1] { nonConformance.qarInspectionID })))
			{
				errorsList.Add("qarInspectionID [" + nonConformance.qarInspectionID + "] not found.");
			}
			if (nonConformance.qarInspectionLineID > 0 && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("InspectionLines", new object[2] { "QALINSPECTIONID", "QALINSPECTIONLINEID" }, new object[2] { nonConformance.qarInspectionID, nonConformance.qarInspectionLineID })))
			{
				errorsList.Add($"qarInspectionLineID [{nonConformance.qarInspectionLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(nonConformance.qarNonConformanceCategoryID) && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("NonConformanceCategories", new object[1] { "QAGNONCONFORMANCECATEGORYID" }, new object[1] { nonConformance.qarNonConformanceCategoryID })))
			{
				errorsList.Add("qarNonConformanceCategoryID [" + nonConformance.qarNonConformanceCategoryID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(nonConformance.qarNonConformanceCodeID) && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("NonConformanceCodes", new object[1] { "QACNONCONFORMANCECODEID" }, new object[1] { nonConformance.qarNonConformanceCodeID })))
			{
				errorsList.Add("qarNonConformanceCodeID [" + nonConformance.qarNonConformanceCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(nonConformance.qarNonConformanceCauseID) && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("NonConformanceCauses", new object[1] { "QAUNONCONFORMANCECAUSEID" }, new object[1] { nonConformance.qarNonConformanceCauseID })))
			{
				errorsList.Add("qarNonConformanceCauseID [" + nonConformance.qarNonConformanceCauseID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(nonConformance.qarCorrectiveActionCategoryID) && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("CorrectiveActionCategories", new object[1] { "QATCORRECTIVEACTIONCATEGORYID" }, new object[1] { nonConformance.qarCorrectiveActionCategoryID })))
			{
				errorsList.Add("qarCorrectiveActionCategoryID [" + nonConformance.qarCorrectiveActionCategoryID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(nonConformance.qarCorrectiveActionCodeID) && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("CorrectiveActionCodes", new object[1] { "QAOCORRECTIVEACTIONCODEID" }, new object[1] { nonConformance.qarCorrectiveActionCodeID })))
			{
				errorsList.Add("qarCorrectiveActionCodeID [" + nonConformance.qarCorrectiveActionCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(nonConformance.qarRepairedByOrganizationID) && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { nonConformance.qarRepairedByOrganizationID })))
			{
				errorsList.Add("qarRepairedByOrganizationID [" + nonConformance.qarRepairedByOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(nonConformance.qarPartID) && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { nonConformance.qarPartID })))
			{
				errorsList.Add("qarPartID [" + nonConformance.qarPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(nonConformance.qarPartRevisionID) && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { nonConformance.qarPartID, nonConformance.qarPartRevisionID })))
			{
				errorsList.Add("qarPartRevisionID [" + nonConformance.qarPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(nonConformance.qarPartWareHouseLocationID) && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { nonConformance.qarPartID, nonConformance.qarPartRevisionID, nonConformance.qarPartWareHouseLocationID })))
			{
				errorsList.Add("qarPartWareHouseLocationID [" + nonConformance.qarPartWareHouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(nonConformance.qarPartBinID) && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { nonConformance.qarPartID, nonConformance.qarPartRevisionID, nonConformance.qarPartWareHouseLocationID, nonConformance.qarPartBinID })))
			{
				errorsList.Add("qarPartBinID [" + nonConformance.qarPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(nonConformance.qarJobID) && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { nonConformance.qarJobID })))
			{
				errorsList.Add("qarJobID [" + nonConformance.qarJobID + "] not found.");
			}
			if (nonConformance.qarJobAssemblyID > 0 && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { nonConformance.qarJobID, nonConformance.qarJobAssemblyID })))
			{
				errorsList.Add($"qarJobAssemblyID [{nonConformance.qarJobAssemblyID}] not found.");
			}
			if (nonConformance.qarJobMaterialID > 0 && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { nonConformance.qarJobID, nonConformance.qarJobAssemblyID, nonConformance.qarJobMaterialID })))
			{
				errorsList.Add($"qarJobMaterialID [{nonConformance.qarJobMaterialID}] not found.");
			}
			if (nonConformance.qarJobOperationID > 0 && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { nonConformance.qarJobID, nonConformance.qarJobAssemblyID, nonConformance.qarJobOperationID })))
			{
				errorsList.Add($"qarJobOperationID [{nonConformance.qarJobOperationID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(nonConformance.qarReportedByEmployeeID) && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { nonConformance.qarReportedByEmployeeID })))
			{
				errorsList.Add("qarReportedByEmployeeID [" + nonConformance.qarReportedByEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(nonConformance.qarRmaClaimID) && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("RMAClaims", new object[1] { "RAPRMACLAIMID" }, new object[1] { nonConformance.qarRmaClaimID })))
			{
				errorsList.Add("qarRmaClaimID [" + nonConformance.qarRmaClaimID + "] not found.");
			}
			if (nonConformance.qarRmaClaimLineID > 0 && !(await base.ERPNonConformanceRepository.DoesRecordExistInTableUsingKeys("RMAClaimLines", new object[2] { "RALRMACLAIMID", "RALRMACLAIMLINEID" }, new object[2] { nonConformance.qarRmaClaimID, nonConformance.qarRmaClaimLineID })))
			{
				errorsList.Add($"qarRmaClaimLineID [{nonConformance.qarRmaClaimLineID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPNonConformanceDto>>> Process_GetAllNonConformances(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPNonConformanceDto> allNonConformancesDto = new List<ERPNonConformanceDto>();
		ERPResponseMessageDto<IList<ERPNonConformanceDto>> result;
		try
		{
			IERPNonConformanceRepository iERPNonConformanceRepository = (base.ERPNonConformanceRepository = new ERPNonConformanceRepository(base.ApiClientContext));
			using (iERPNonConformanceRepository)
			{
				foreach (ERPNonConformanceInformationDto item2 in await base.ERPNonConformanceRepository.GetAllNonConformances(pageSize, pageNumber, filter, orderBy))
				{
					ERPNonConformanceDto item = new ERPNonConformanceDto
					{
						qarActualHours = item2.qarActualHours,
						qarNonConformanceID = item2.qarNonConformanceID,
						qarCorrectiveActionCategoryID = item2.qarCorrectiveActionCategoryID,
						qarCorrectiveActionCodeID = item2.qarCorrectiveActionCodeID,
						qarCorrectiveActionDate = item2.qarCorrectiveActionDate,
						qarCorrectiveActionRTF = item2.qarCorrectiveActionRTF,
						qarCorrectiveActionText = item2.qarCorrectiveActionText,
						qarCorrectiveActionType = item2.qarCorrectiveActionType,
						qarCreatedBy = item2.qarCreatedBy,
						qarCreatedDate = item2.qarCreatedDate,
						qarUniqueID = item2.qarUniqueID,
						qarHoursAllowed = item2.qarHoursAllowed,
						qarHoursRequested = item2.qarHoursRequested,
						qarInspectionID = item2.qarInspectionID,
						qarInspectionLineID = item2.qarInspectionLineID,
						qarCorrectiveActionComplete = item2.qarCorrectiveActionComplete,
						qarJobAssemblyID = item2.qarJobAssemblyID,
						qarJobID = item2.qarJobID,
						qarJobMaterialID = item2.qarJobMaterialID,
						qarJobOperationID = item2.qarJobOperationID,
						qarNonConformanceCategoryID = item2.qarNonConformanceCategoryID,
						qarNonConformanceCauseID = item2.qarNonConformanceCauseID,
						qarNonConformanceCodeID = item2.qarNonConformanceCodeID,
						qarNonConformanceRTF = item2.qarNonConformanceRTF,
						qarNonConformanceText = item2.qarNonConformanceText,
						qarPartBinID = item2.qarPartBinID,
						qarPartID = item2.qarPartID,
						qarPartRevisionID = item2.qarPartRevisionID,
						qarPartShortDescription = item2.qarPartShortDescription,
						qarPartWareHouseLocationID = item2.qarPartWareHouseLocationID,
						qarQuantity = item2.qarQuantity,
						qarRepairedByOrganizationID = item2.qarRepairedByOrganizationID,
						qarReportedByEmployeeID = item2.qarReportedByEmployeeID,
						qarRmaClaimID = item2.qarRmaClaimID,
						qarRmaClaimLineID = item2.qarRmaClaimLineID,
						qarRowVersion = item2.qarRowVersion,
						qarSubcontractAmount = item2.qarSubcontractAmount,
						qarSubcontractAmountForeign = item2.qarSubcontractAmountForeign,
						qarUnitOfMeasure = item2.qarUnitOfMeasure,
						CustomFields = item2.CustomFields
					};
					allNonConformancesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all NonConformances]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPNonConformanceDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allNonConformancesDto,
				RecordCount = allNonConformancesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPNonConformanceDto>> Process_GetNonConformance(Guid nonConformanceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPNonConformanceDto nonConformanceDto = null;
		ERPResponseMessageDto<ERPNonConformanceDto> result;
		try
		{
			IERPNonConformanceRepository iERPNonConformanceRepository = (base.ERPNonConformanceRepository = new ERPNonConformanceRepository(base.ApiClientContext));
			using (iERPNonConformanceRepository)
			{
				ERPNonConformanceInformationDto eRPNonConformanceInformationDto = await base.ERPNonConformanceRepository.GetNonConformance(nonConformanceId);
				nonConformanceDto = new ERPNonConformanceDto
				{
					qarActualHours = eRPNonConformanceInformationDto.qarActualHours,
					qarNonConformanceID = eRPNonConformanceInformationDto.qarNonConformanceID,
					qarCorrectiveActionCategoryID = eRPNonConformanceInformationDto.qarCorrectiveActionCategoryID,
					qarCorrectiveActionCodeID = eRPNonConformanceInformationDto.qarCorrectiveActionCodeID,
					qarCorrectiveActionDate = eRPNonConformanceInformationDto.qarCorrectiveActionDate,
					qarCorrectiveActionRTF = eRPNonConformanceInformationDto.qarCorrectiveActionRTF,
					qarCorrectiveActionText = eRPNonConformanceInformationDto.qarCorrectiveActionText,
					qarCorrectiveActionType = eRPNonConformanceInformationDto.qarCorrectiveActionType,
					qarCreatedBy = eRPNonConformanceInformationDto.qarCreatedBy,
					qarCreatedDate = eRPNonConformanceInformationDto.qarCreatedDate,
					qarUniqueID = eRPNonConformanceInformationDto.qarUniqueID,
					qarHoursAllowed = eRPNonConformanceInformationDto.qarHoursAllowed,
					qarHoursRequested = eRPNonConformanceInformationDto.qarHoursRequested,
					qarInspectionID = eRPNonConformanceInformationDto.qarInspectionID,
					qarInspectionLineID = eRPNonConformanceInformationDto.qarInspectionLineID,
					qarCorrectiveActionComplete = eRPNonConformanceInformationDto.qarCorrectiveActionComplete,
					qarJobAssemblyID = eRPNonConformanceInformationDto.qarJobAssemblyID,
					qarJobID = eRPNonConformanceInformationDto.qarJobID,
					qarJobMaterialID = eRPNonConformanceInformationDto.qarJobMaterialID,
					qarJobOperationID = eRPNonConformanceInformationDto.qarJobOperationID,
					qarNonConformanceCategoryID = eRPNonConformanceInformationDto.qarNonConformanceCategoryID,
					qarNonConformanceCauseID = eRPNonConformanceInformationDto.qarNonConformanceCauseID,
					qarNonConformanceCodeID = eRPNonConformanceInformationDto.qarNonConformanceCodeID,
					qarNonConformanceRTF = eRPNonConformanceInformationDto.qarNonConformanceRTF,
					qarNonConformanceText = eRPNonConformanceInformationDto.qarNonConformanceText,
					qarPartBinID = eRPNonConformanceInformationDto.qarPartBinID,
					qarPartID = eRPNonConformanceInformationDto.qarPartID,
					qarPartRevisionID = eRPNonConformanceInformationDto.qarPartRevisionID,
					qarPartShortDescription = eRPNonConformanceInformationDto.qarPartShortDescription,
					qarPartWareHouseLocationID = eRPNonConformanceInformationDto.qarPartWareHouseLocationID,
					qarQuantity = eRPNonConformanceInformationDto.qarQuantity,
					qarRepairedByOrganizationID = eRPNonConformanceInformationDto.qarRepairedByOrganizationID,
					qarReportedByEmployeeID = eRPNonConformanceInformationDto.qarReportedByEmployeeID,
					qarRmaClaimID = eRPNonConformanceInformationDto.qarRmaClaimID,
					qarRmaClaimLineID = eRPNonConformanceInformationDto.qarRmaClaimLineID,
					qarRowVersion = eRPNonConformanceInformationDto.qarRowVersion,
					qarSubcontractAmount = eRPNonConformanceInformationDto.qarSubcontractAmount,
					qarSubcontractAmountForeign = eRPNonConformanceInformationDto.qarSubcontractAmountForeign,
					qarUnitOfMeasure = eRPNonConformanceInformationDto.qarUnitOfMeasure,
					CustomFields = eRPNonConformanceInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the NonConformances []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPNonConformanceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = nonConformanceDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPNonConformanceDto>> Process_PutNonConformance(ERPNonConformanceDto nonConformance)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPNonConformanceDto createdObject = null;
		ERPResponseMessageDto<ERPNonConformanceDto> result;
		try
		{
			IERPNonConformanceRepository iERPNonConformanceRepository = (base.ERPNonConformanceRepository = new ERPNonConformanceRepository(base.ApiClientContext));
			using (iERPNonConformanceRepository)
			{
				APIValidationInfoDto postResult = await base.ERPNonConformanceRepository.SaveNonConformance(nonConformance);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPNonConformanceInformationDto eRPNonConformanceInformationDto = await base.ERPNonConformanceRepository.GetNonConformance(nonConformance.qarUniqueID);
					createdObject = new ERPNonConformanceDto
					{
						qarActualHours = eRPNonConformanceInformationDto.qarActualHours,
						qarNonConformanceID = eRPNonConformanceInformationDto.qarNonConformanceID,
						qarCorrectiveActionCategoryID = eRPNonConformanceInformationDto.qarCorrectiveActionCategoryID,
						qarCorrectiveActionCodeID = eRPNonConformanceInformationDto.qarCorrectiveActionCodeID,
						qarCorrectiveActionDate = eRPNonConformanceInformationDto.qarCorrectiveActionDate,
						qarCorrectiveActionRTF = eRPNonConformanceInformationDto.qarCorrectiveActionRTF,
						qarCorrectiveActionText = eRPNonConformanceInformationDto.qarCorrectiveActionText,
						qarCorrectiveActionType = eRPNonConformanceInformationDto.qarCorrectiveActionType,
						qarCreatedBy = eRPNonConformanceInformationDto.qarCreatedBy,
						qarCreatedDate = eRPNonConformanceInformationDto.qarCreatedDate,
						qarUniqueID = eRPNonConformanceInformationDto.qarUniqueID,
						qarHoursAllowed = eRPNonConformanceInformationDto.qarHoursAllowed,
						qarHoursRequested = eRPNonConformanceInformationDto.qarHoursRequested,
						qarInspectionID = eRPNonConformanceInformationDto.qarInspectionID,
						qarInspectionLineID = eRPNonConformanceInformationDto.qarInspectionLineID,
						qarCorrectiveActionComplete = eRPNonConformanceInformationDto.qarCorrectiveActionComplete,
						qarJobAssemblyID = eRPNonConformanceInformationDto.qarJobAssemblyID,
						qarJobID = eRPNonConformanceInformationDto.qarJobID,
						qarJobMaterialID = eRPNonConformanceInformationDto.qarJobMaterialID,
						qarJobOperationID = eRPNonConformanceInformationDto.qarJobOperationID,
						qarNonConformanceCategoryID = eRPNonConformanceInformationDto.qarNonConformanceCategoryID,
						qarNonConformanceCauseID = eRPNonConformanceInformationDto.qarNonConformanceCauseID,
						qarNonConformanceCodeID = eRPNonConformanceInformationDto.qarNonConformanceCodeID,
						qarNonConformanceRTF = eRPNonConformanceInformationDto.qarNonConformanceRTF,
						qarNonConformanceText = eRPNonConformanceInformationDto.qarNonConformanceText,
						qarPartBinID = eRPNonConformanceInformationDto.qarPartBinID,
						qarPartID = eRPNonConformanceInformationDto.qarPartID,
						qarPartRevisionID = eRPNonConformanceInformationDto.qarPartRevisionID,
						qarPartShortDescription = eRPNonConformanceInformationDto.qarPartShortDescription,
						qarPartWareHouseLocationID = eRPNonConformanceInformationDto.qarPartWareHouseLocationID,
						qarQuantity = eRPNonConformanceInformationDto.qarQuantity,
						qarRepairedByOrganizationID = eRPNonConformanceInformationDto.qarRepairedByOrganizationID,
						qarReportedByEmployeeID = eRPNonConformanceInformationDto.qarReportedByEmployeeID,
						qarRmaClaimID = eRPNonConformanceInformationDto.qarRmaClaimID,
						qarRmaClaimLineID = eRPNonConformanceInformationDto.qarRmaClaimLineID,
						qarRowVersion = eRPNonConformanceInformationDto.qarRowVersion,
						qarSubcontractAmount = eRPNonConformanceInformationDto.qarSubcontractAmount,
						qarSubcontractAmountForeign = eRPNonConformanceInformationDto.qarSubcontractAmountForeign,
						qarUnitOfMeasure = eRPNonConformanceInformationDto.qarUnitOfMeasure,
						CustomFields = eRPNonConformanceInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing NonConformance [{nonConformance.qarUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPNonConformanceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteNonConformance(Guid nonConformanceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPNonConformanceRepository iERPNonConformanceRepository = (base.ERPNonConformanceRepository = new ERPNonConformanceRepository(base.ApiClientContext));
		using (iERPNonConformanceRepository)
		{
			if (!(await base.ERPNonConformanceRepository.DoesNonConformanceExist(nonConformanceId)))
			{
				base.ErrorsList.Add($"NonConformance [{nonConformanceId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPNonConformanceInformationDto eRPNonConformanceInformationDto = await base.ERPNonConformanceRepository.GetNonConformance(nonConformanceId);
				string text = await base.ERPNonConformanceRepository.WhereUsed("NonConformances", new object[1] { eRPNonConformanceInformationDto.qarNonConformanceID }, new object[1] { "qarNonConformanceID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("NonConformance cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPNonConformanceDto>> Process_DeleteNonConformance(Guid nonConformanceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPNonConformanceDto> result;
		try
		{
			IERPNonConformanceRepository iERPNonConformanceRepository = (base.ERPNonConformanceRepository = new ERPNonConformanceRepository(base.ApiClientContext));
			using (iERPNonConformanceRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPNonConformanceRepository.DeleteRowFromTable("NonConformances", "qar", nonConformanceId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of NonConformance [{nonConformanceId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPNonConformanceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPNonConformanceDto()
			};
		}
		return result;
	}
}
