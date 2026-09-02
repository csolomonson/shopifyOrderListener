using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPJobMaterialModel : ERPBaseModel, IERPJobMaterialModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllJobMaterials(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPJobMaterialRepository iERPJobMaterialRepository = (base.ERPJobMaterialRepository = new ERPJobMaterialRepository(base.ApiClientContext));
		using (iERPJobMaterialRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPJobMaterialRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPJobMaterialRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPJobMaterialRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPJobMaterialRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetJobMaterial(Guid jobMaterialId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobMaterialRepository iERPJobMaterialRepository = (base.ERPJobMaterialRepository = new ERPJobMaterialRepository(base.ApiClientContext));
		using (iERPJobMaterialRepository)
		{
			if (!(await base.ERPJobMaterialRepository.DoesJobMaterialExist(jobMaterialId)))
			{
				errorsList.Add($"JobMaterial [{jobMaterialId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutJobMaterial(ERPJobMaterialDto jobMaterial)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobMaterialRepository iERPJobMaterialRepository = (base.ERPJobMaterialRepository = new ERPJobMaterialRepository(base.ApiClientContext));
		using (iERPJobMaterialRepository)
		{
			if (!string.IsNullOrWhiteSpace(jobMaterial.jmmJobID) && !(await base.ERPJobMaterialRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { jobMaterial.jmmJobID })))
			{
				errorsList.Add("jmmJobID [" + jobMaterial.jmmJobID + "] not found.");
			}
			if (jobMaterial.jmmJobAssemblyID > 0 && !(await base.ERPJobMaterialRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { jobMaterial.jmmJobID, jobMaterial.jmmJobAssemblyID })))
			{
				errorsList.Add($"jmmJobAssemblyID [{jobMaterial.jmmJobAssemblyID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobMaterial.jmmPartID) && !(await base.ERPJobMaterialRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { jobMaterial.jmmPartID })))
			{
				errorsList.Add("jmmPartID [" + jobMaterial.jmmPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobMaterial.jmmPartRevisionID) && !(await base.ERPJobMaterialRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { jobMaterial.jmmPartID, jobMaterial.jmmPartRevisionID })))
			{
				errorsList.Add("jmmPartRevisionID [" + jobMaterial.jmmPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobMaterial.jmmPartWarehouseLocationID) && !(await base.ERPJobMaterialRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { jobMaterial.jmmPartID, jobMaterial.jmmPartRevisionID, jobMaterial.jmmPartWarehouseLocationID })))
			{
				errorsList.Add("jmmPartWarehouseLocationID [" + jobMaterial.jmmPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobMaterial.jmmPartBinID) && !(await base.ERPJobMaterialRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { jobMaterial.jmmPartID, jobMaterial.jmmPartRevisionID, jobMaterial.jmmPartWarehouseLocationID, jobMaterial.jmmPartBinID })))
			{
				errorsList.Add("jmmPartBinID [" + jobMaterial.jmmPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobMaterial.jmmSupplierOrganizationID) && !(await base.ERPJobMaterialRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { jobMaterial.jmmSupplierOrganizationID })))
			{
				errorsList.Add("jmmSupplierOrganizationID [" + jobMaterial.jmmSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobMaterial.jmmPurchaseLocationID) && !(await base.ERPJobMaterialRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { jobMaterial.jmmSupplierOrganizationID, jobMaterial.jmmPurchaseLocationID })))
			{
				errorsList.Add("jmmPurchaseLocationID [" + jobMaterial.jmmPurchaseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobMaterial.jmmPurchaseOrderID) && !(await base.ERPJobMaterialRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { jobMaterial.jmmPurchaseOrderID })))
			{
				errorsList.Add("jmmPurchaseOrderID [" + jobMaterial.jmmPurchaseOrderID + "] not found.");
			}
			if (jobMaterial.jmmRelatedJobOperationID > 0 && !(await base.ERPJobMaterialRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { jobMaterial.jmmJobID, jobMaterial.jmmJobAssemblyID, jobMaterial.jmmRelatedJobOperationID })))
			{
				errorsList.Add($"jmmRelatedJobOperationID [{jobMaterial.jmmRelatedJobOperationID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobMaterial.jmmRfqID) && !(await base.ERPJobMaterialRepository.DoesRecordExistInTableUsingKeys("RFQs", new object[1] { "RQPRFQID" }, new object[1] { jobMaterial.jmmRfqID })))
			{
				errorsList.Add("jmmRfqID [" + jobMaterial.jmmRfqID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPJobMaterialDto>>> Process_GetAllJobMaterials(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPJobMaterialDto> allJobMaterialsDto = new List<ERPJobMaterialDto>();
		ERPResponseMessageDto<IList<ERPJobMaterialDto>> result;
		try
		{
			IERPJobMaterialRepository iERPJobMaterialRepository = (base.ERPJobMaterialRepository = new ERPJobMaterialRepository(base.ApiClientContext));
			using (iERPJobMaterialRepository)
			{
				foreach (ERPJobMaterialInformationDto item2 in await base.ERPJobMaterialRepository.GetAllJobMaterials(pageSize, pageNumber, filter, orderBy))
				{
					ERPJobMaterialDto item = new ERPJobMaterialDto
					{
						jmmCalculatedUnitCost = item2.jmmCalculatedUnitCost,
						jmmCreatedBy = item2.jmmCreatedBy,
						jmmCreatedDate = item2.jmmCreatedDate,
						jmmDocuments = item2.jmmDocuments,
						jmmDueInDate = item2.jmmDueInDate,
						jmmUniqueID = item2.jmmUniqueID,
						jmmEstimatedQuantity = item2.jmmEstimatedQuantity,
						jmmEstimatedUnitCost = item2.jmmEstimatedUnitCost,
						jmmBackflush = item2.jmmBackflush,
						jmmClosed = item2.jmmClosed,
						jmmCostOverride = item2.jmmCostOverride,
						jmmFirm = item2.jmmFirm,
						jmmKitPart = item2.jmmKitPart,
						jmmPullAllFromStock = item2.jmmPullAllFromStock,
						jmmReceivedComplete = item2.jmmReceivedComplete,
						jmmJobAssemblyID = item2.jmmJobAssemblyID,
						jmmJobID = item2.jmmJobID,
						jmmLeadTime = item2.jmmLeadTime,
						jmmLeadTime1 = item2.jmmLeadTime1,
						jmmLeadTime2 = item2.jmmLeadTime2,
						jmmLeadTime3 = item2.jmmLeadTime3,
						jmmLeadTime4 = item2.jmmLeadTime4,
						jmmLeadTime5 = item2.jmmLeadTime5,
						jmmLeadTime6 = item2.jmmLeadTime6,
						jmmLeadTime7 = item2.jmmLeadTime7,
						jmmLeadTime8 = item2.jmmLeadTime8,
						jmmLeadTime9 = item2.jmmLeadTime9,
						jmmMinimumCharge = item2.jmmMinimumCharge,
						jmmOrderByDate = item2.jmmOrderByDate,
						jmmPartBinID = item2.jmmPartBinID,
						jmmPartID = item2.jmmPartID,
						jmmPartLongDescriptionRtf = item2.jmmPartLongDescriptionRtf,
						jmmPartLongDescriptionText = item2.jmmPartLongDescriptionText,
						jmmPartRevisionID = item2.jmmPartRevisionID,
						jmmPartShortDescription = item2.jmmPartShortDescription,
						jmmPartWarehouseLocationID = item2.jmmPartWarehouseLocationID,
						jmmPullFromStockQuantity = item2.jmmPullFromStockQuantity,
						jmmPurchaseLocationID = item2.jmmPurchaseLocationID,
						jmmPurchaseOrderID = item2.jmmPurchaseOrderID,
						jmmPurchaseToJobQuantity = item2.jmmPurchaseToJobQuantity,
						jmmQuantityAllocated = item2.jmmQuantityAllocated,
						jmmQuantityBreak1 = item2.jmmQuantityBreak1,
						jmmQuantityBreak2 = item2.jmmQuantityBreak2,
						jmmQuantityBreak3 = item2.jmmQuantityBreak3,
						jmmQuantityBreak4 = item2.jmmQuantityBreak4,
						jmmQuantityBreak5 = item2.jmmQuantityBreak5,
						jmmQuantityBreak6 = item2.jmmQuantityBreak6,
						jmmQuantityBreak7 = item2.jmmQuantityBreak7,
						jmmQuantityBreak8 = item2.jmmQuantityBreak8,
						jmmQuantityBreak9 = item2.jmmQuantityBreak9,
						jmmQuantityPerAssembly = item2.jmmQuantityPerAssembly,
						jmmQuantityReceived = item2.jmmQuantityReceived,
						jmmQuantityToInspect = item2.jmmQuantityToInspect,
						jmmQuantityToReturn = item2.jmmQuantityToReturn,
						jmmRelatedJobOperationID = item2.jmmRelatedJobOperationID,
						jmmRequiredDate = item2.jmmRequiredDate,
						jmmRfqID = item2.jmmRfqID,
						jmmRowVersion = item2.jmmRowVersion,
						jmmScrapPercent = item2.jmmScrapPercent,
						jmmScrapQuantity = item2.jmmScrapQuantity,
						jmmScrapQuantityReceived = item2.jmmScrapQuantityReceived,
						jmmJobMaterialID = item2.jmmJobMaterialID,
						jmmSupplierOrganizationID = item2.jmmSupplierOrganizationID,
						jmmUnitCost1 = item2.jmmUnitCost1,
						jmmUnitCost2 = item2.jmmUnitCost2,
						jmmUnitCost3 = item2.jmmUnitCost3,
						jmmUnitCost4 = item2.jmmUnitCost4,
						jmmUnitCost5 = item2.jmmUnitCost5,
						jmmUnitCost6 = item2.jmmUnitCost6,
						jmmUnitCost7 = item2.jmmUnitCost7,
						jmmUnitCost8 = item2.jmmUnitCost8,
						jmmUnitCost9 = item2.jmmUnitCost9,
						jmmUnitOfMeasure = item2.jmmUnitOfMeasure,
						CustomFields = item2.CustomFields
					};
					allJobMaterialsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all JobMaterials]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPJobMaterialDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allJobMaterialsDto,
				RecordCount = allJobMaterialsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobMaterialDto>> Process_GetJobMaterial(Guid jobMaterialId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPJobMaterialDto jobMaterialDto = null;
		ERPResponseMessageDto<ERPJobMaterialDto> result;
		try
		{
			IERPJobMaterialRepository iERPJobMaterialRepository = (base.ERPJobMaterialRepository = new ERPJobMaterialRepository(base.ApiClientContext));
			using (iERPJobMaterialRepository)
			{
				ERPJobMaterialInformationDto eRPJobMaterialInformationDto = await base.ERPJobMaterialRepository.GetJobMaterial(jobMaterialId);
				jobMaterialDto = new ERPJobMaterialDto
				{
					jmmCalculatedUnitCost = eRPJobMaterialInformationDto.jmmCalculatedUnitCost,
					jmmCreatedBy = eRPJobMaterialInformationDto.jmmCreatedBy,
					jmmCreatedDate = eRPJobMaterialInformationDto.jmmCreatedDate,
					jmmDocuments = eRPJobMaterialInformationDto.jmmDocuments,
					jmmDueInDate = eRPJobMaterialInformationDto.jmmDueInDate,
					jmmUniqueID = eRPJobMaterialInformationDto.jmmUniqueID,
					jmmEstimatedQuantity = eRPJobMaterialInformationDto.jmmEstimatedQuantity,
					jmmEstimatedUnitCost = eRPJobMaterialInformationDto.jmmEstimatedUnitCost,
					jmmBackflush = eRPJobMaterialInformationDto.jmmBackflush,
					jmmClosed = eRPJobMaterialInformationDto.jmmClosed,
					jmmCostOverride = eRPJobMaterialInformationDto.jmmCostOverride,
					jmmFirm = eRPJobMaterialInformationDto.jmmFirm,
					jmmKitPart = eRPJobMaterialInformationDto.jmmKitPart,
					jmmPullAllFromStock = eRPJobMaterialInformationDto.jmmPullAllFromStock,
					jmmReceivedComplete = eRPJobMaterialInformationDto.jmmReceivedComplete,
					jmmJobAssemblyID = eRPJobMaterialInformationDto.jmmJobAssemblyID,
					jmmJobID = eRPJobMaterialInformationDto.jmmJobID,
					jmmLeadTime = eRPJobMaterialInformationDto.jmmLeadTime,
					jmmLeadTime1 = eRPJobMaterialInformationDto.jmmLeadTime1,
					jmmLeadTime2 = eRPJobMaterialInformationDto.jmmLeadTime2,
					jmmLeadTime3 = eRPJobMaterialInformationDto.jmmLeadTime3,
					jmmLeadTime4 = eRPJobMaterialInformationDto.jmmLeadTime4,
					jmmLeadTime5 = eRPJobMaterialInformationDto.jmmLeadTime5,
					jmmLeadTime6 = eRPJobMaterialInformationDto.jmmLeadTime6,
					jmmLeadTime7 = eRPJobMaterialInformationDto.jmmLeadTime7,
					jmmLeadTime8 = eRPJobMaterialInformationDto.jmmLeadTime8,
					jmmLeadTime9 = eRPJobMaterialInformationDto.jmmLeadTime9,
					jmmMinimumCharge = eRPJobMaterialInformationDto.jmmMinimumCharge,
					jmmOrderByDate = eRPJobMaterialInformationDto.jmmOrderByDate,
					jmmPartBinID = eRPJobMaterialInformationDto.jmmPartBinID,
					jmmPartID = eRPJobMaterialInformationDto.jmmPartID,
					jmmPartLongDescriptionRtf = eRPJobMaterialInformationDto.jmmPartLongDescriptionRtf,
					jmmPartLongDescriptionText = eRPJobMaterialInformationDto.jmmPartLongDescriptionText,
					jmmPartRevisionID = eRPJobMaterialInformationDto.jmmPartRevisionID,
					jmmPartShortDescription = eRPJobMaterialInformationDto.jmmPartShortDescription,
					jmmPartWarehouseLocationID = eRPJobMaterialInformationDto.jmmPartWarehouseLocationID,
					jmmPullFromStockQuantity = eRPJobMaterialInformationDto.jmmPullFromStockQuantity,
					jmmPurchaseLocationID = eRPJobMaterialInformationDto.jmmPurchaseLocationID,
					jmmPurchaseOrderID = eRPJobMaterialInformationDto.jmmPurchaseOrderID,
					jmmPurchaseToJobQuantity = eRPJobMaterialInformationDto.jmmPurchaseToJobQuantity,
					jmmQuantityAllocated = eRPJobMaterialInformationDto.jmmQuantityAllocated,
					jmmQuantityBreak1 = eRPJobMaterialInformationDto.jmmQuantityBreak1,
					jmmQuantityBreak2 = eRPJobMaterialInformationDto.jmmQuantityBreak2,
					jmmQuantityBreak3 = eRPJobMaterialInformationDto.jmmQuantityBreak3,
					jmmQuantityBreak4 = eRPJobMaterialInformationDto.jmmQuantityBreak4,
					jmmQuantityBreak5 = eRPJobMaterialInformationDto.jmmQuantityBreak5,
					jmmQuantityBreak6 = eRPJobMaterialInformationDto.jmmQuantityBreak6,
					jmmQuantityBreak7 = eRPJobMaterialInformationDto.jmmQuantityBreak7,
					jmmQuantityBreak8 = eRPJobMaterialInformationDto.jmmQuantityBreak8,
					jmmQuantityBreak9 = eRPJobMaterialInformationDto.jmmQuantityBreak9,
					jmmQuantityPerAssembly = eRPJobMaterialInformationDto.jmmQuantityPerAssembly,
					jmmQuantityReceived = eRPJobMaterialInformationDto.jmmQuantityReceived,
					jmmQuantityToInspect = eRPJobMaterialInformationDto.jmmQuantityToInspect,
					jmmQuantityToReturn = eRPJobMaterialInformationDto.jmmQuantityToReturn,
					jmmRelatedJobOperationID = eRPJobMaterialInformationDto.jmmRelatedJobOperationID,
					jmmRequiredDate = eRPJobMaterialInformationDto.jmmRequiredDate,
					jmmRfqID = eRPJobMaterialInformationDto.jmmRfqID,
					jmmRowVersion = eRPJobMaterialInformationDto.jmmRowVersion,
					jmmScrapPercent = eRPJobMaterialInformationDto.jmmScrapPercent,
					jmmScrapQuantity = eRPJobMaterialInformationDto.jmmScrapQuantity,
					jmmScrapQuantityReceived = eRPJobMaterialInformationDto.jmmScrapQuantityReceived,
					jmmJobMaterialID = eRPJobMaterialInformationDto.jmmJobMaterialID,
					jmmSupplierOrganizationID = eRPJobMaterialInformationDto.jmmSupplierOrganizationID,
					jmmUnitCost1 = eRPJobMaterialInformationDto.jmmUnitCost1,
					jmmUnitCost2 = eRPJobMaterialInformationDto.jmmUnitCost2,
					jmmUnitCost3 = eRPJobMaterialInformationDto.jmmUnitCost3,
					jmmUnitCost4 = eRPJobMaterialInformationDto.jmmUnitCost4,
					jmmUnitCost5 = eRPJobMaterialInformationDto.jmmUnitCost5,
					jmmUnitCost6 = eRPJobMaterialInformationDto.jmmUnitCost6,
					jmmUnitCost7 = eRPJobMaterialInformationDto.jmmUnitCost7,
					jmmUnitCost8 = eRPJobMaterialInformationDto.jmmUnitCost8,
					jmmUnitCost9 = eRPJobMaterialInformationDto.jmmUnitCost9,
					jmmUnitOfMeasure = eRPJobMaterialInformationDto.jmmUnitOfMeasure,
					CustomFields = eRPJobMaterialInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the JobMaterials []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobMaterialDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobMaterialDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobMaterialDto>> Process_PutJobMaterial(ERPJobMaterialDto jobMaterial)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPJobMaterialDto createdObject = null;
		ERPResponseMessageDto<ERPJobMaterialDto> result;
		try
		{
			IERPJobMaterialRepository iERPJobMaterialRepository = (base.ERPJobMaterialRepository = new ERPJobMaterialRepository(base.ApiClientContext));
			using (iERPJobMaterialRepository)
			{
				APIValidationInfoDto postResult = await base.ERPJobMaterialRepository.SaveJobMaterial(jobMaterial);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPJobMaterialInformationDto eRPJobMaterialInformationDto = await base.ERPJobMaterialRepository.GetJobMaterial(jobMaterial.jmmUniqueID);
					createdObject = new ERPJobMaterialDto
					{
						jmmCalculatedUnitCost = eRPJobMaterialInformationDto.jmmCalculatedUnitCost,
						jmmCreatedBy = eRPJobMaterialInformationDto.jmmCreatedBy,
						jmmCreatedDate = eRPJobMaterialInformationDto.jmmCreatedDate,
						jmmDocuments = eRPJobMaterialInformationDto.jmmDocuments,
						jmmDueInDate = eRPJobMaterialInformationDto.jmmDueInDate,
						jmmUniqueID = eRPJobMaterialInformationDto.jmmUniqueID,
						jmmEstimatedQuantity = eRPJobMaterialInformationDto.jmmEstimatedQuantity,
						jmmEstimatedUnitCost = eRPJobMaterialInformationDto.jmmEstimatedUnitCost,
						jmmBackflush = eRPJobMaterialInformationDto.jmmBackflush,
						jmmClosed = eRPJobMaterialInformationDto.jmmClosed,
						jmmCostOverride = eRPJobMaterialInformationDto.jmmCostOverride,
						jmmFirm = eRPJobMaterialInformationDto.jmmFirm,
						jmmKitPart = eRPJobMaterialInformationDto.jmmKitPart,
						jmmPullAllFromStock = eRPJobMaterialInformationDto.jmmPullAllFromStock,
						jmmReceivedComplete = eRPJobMaterialInformationDto.jmmReceivedComplete,
						jmmJobAssemblyID = eRPJobMaterialInformationDto.jmmJobAssemblyID,
						jmmJobID = eRPJobMaterialInformationDto.jmmJobID,
						jmmLeadTime = eRPJobMaterialInformationDto.jmmLeadTime,
						jmmLeadTime1 = eRPJobMaterialInformationDto.jmmLeadTime1,
						jmmLeadTime2 = eRPJobMaterialInformationDto.jmmLeadTime2,
						jmmLeadTime3 = eRPJobMaterialInformationDto.jmmLeadTime3,
						jmmLeadTime4 = eRPJobMaterialInformationDto.jmmLeadTime4,
						jmmLeadTime5 = eRPJobMaterialInformationDto.jmmLeadTime5,
						jmmLeadTime6 = eRPJobMaterialInformationDto.jmmLeadTime6,
						jmmLeadTime7 = eRPJobMaterialInformationDto.jmmLeadTime7,
						jmmLeadTime8 = eRPJobMaterialInformationDto.jmmLeadTime8,
						jmmLeadTime9 = eRPJobMaterialInformationDto.jmmLeadTime9,
						jmmMinimumCharge = eRPJobMaterialInformationDto.jmmMinimumCharge,
						jmmOrderByDate = eRPJobMaterialInformationDto.jmmOrderByDate,
						jmmPartBinID = eRPJobMaterialInformationDto.jmmPartBinID,
						jmmPartID = eRPJobMaterialInformationDto.jmmPartID,
						jmmPartLongDescriptionRtf = eRPJobMaterialInformationDto.jmmPartLongDescriptionRtf,
						jmmPartLongDescriptionText = eRPJobMaterialInformationDto.jmmPartLongDescriptionText,
						jmmPartRevisionID = eRPJobMaterialInformationDto.jmmPartRevisionID,
						jmmPartShortDescription = eRPJobMaterialInformationDto.jmmPartShortDescription,
						jmmPartWarehouseLocationID = eRPJobMaterialInformationDto.jmmPartWarehouseLocationID,
						jmmPullFromStockQuantity = eRPJobMaterialInformationDto.jmmPullFromStockQuantity,
						jmmPurchaseLocationID = eRPJobMaterialInformationDto.jmmPurchaseLocationID,
						jmmPurchaseOrderID = eRPJobMaterialInformationDto.jmmPurchaseOrderID,
						jmmPurchaseToJobQuantity = eRPJobMaterialInformationDto.jmmPurchaseToJobQuantity,
						jmmQuantityAllocated = eRPJobMaterialInformationDto.jmmQuantityAllocated,
						jmmQuantityBreak1 = eRPJobMaterialInformationDto.jmmQuantityBreak1,
						jmmQuantityBreak2 = eRPJobMaterialInformationDto.jmmQuantityBreak2,
						jmmQuantityBreak3 = eRPJobMaterialInformationDto.jmmQuantityBreak3,
						jmmQuantityBreak4 = eRPJobMaterialInformationDto.jmmQuantityBreak4,
						jmmQuantityBreak5 = eRPJobMaterialInformationDto.jmmQuantityBreak5,
						jmmQuantityBreak6 = eRPJobMaterialInformationDto.jmmQuantityBreak6,
						jmmQuantityBreak7 = eRPJobMaterialInformationDto.jmmQuantityBreak7,
						jmmQuantityBreak8 = eRPJobMaterialInformationDto.jmmQuantityBreak8,
						jmmQuantityBreak9 = eRPJobMaterialInformationDto.jmmQuantityBreak9,
						jmmQuantityPerAssembly = eRPJobMaterialInformationDto.jmmQuantityPerAssembly,
						jmmQuantityReceived = eRPJobMaterialInformationDto.jmmQuantityReceived,
						jmmQuantityToInspect = eRPJobMaterialInformationDto.jmmQuantityToInspect,
						jmmQuantityToReturn = eRPJobMaterialInformationDto.jmmQuantityToReturn,
						jmmRelatedJobOperationID = eRPJobMaterialInformationDto.jmmRelatedJobOperationID,
						jmmRequiredDate = eRPJobMaterialInformationDto.jmmRequiredDate,
						jmmRfqID = eRPJobMaterialInformationDto.jmmRfqID,
						jmmRowVersion = eRPJobMaterialInformationDto.jmmRowVersion,
						jmmScrapPercent = eRPJobMaterialInformationDto.jmmScrapPercent,
						jmmScrapQuantity = eRPJobMaterialInformationDto.jmmScrapQuantity,
						jmmScrapQuantityReceived = eRPJobMaterialInformationDto.jmmScrapQuantityReceived,
						jmmJobMaterialID = eRPJobMaterialInformationDto.jmmJobMaterialID,
						jmmSupplierOrganizationID = eRPJobMaterialInformationDto.jmmSupplierOrganizationID,
						jmmUnitCost1 = eRPJobMaterialInformationDto.jmmUnitCost1,
						jmmUnitCost2 = eRPJobMaterialInformationDto.jmmUnitCost2,
						jmmUnitCost3 = eRPJobMaterialInformationDto.jmmUnitCost3,
						jmmUnitCost4 = eRPJobMaterialInformationDto.jmmUnitCost4,
						jmmUnitCost5 = eRPJobMaterialInformationDto.jmmUnitCost5,
						jmmUnitCost6 = eRPJobMaterialInformationDto.jmmUnitCost6,
						jmmUnitCost7 = eRPJobMaterialInformationDto.jmmUnitCost7,
						jmmUnitCost8 = eRPJobMaterialInformationDto.jmmUnitCost8,
						jmmUnitCost9 = eRPJobMaterialInformationDto.jmmUnitCost9,
						jmmUnitOfMeasure = eRPJobMaterialInformationDto.jmmUnitOfMeasure,
						CustomFields = eRPJobMaterialInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing JobMaterial [{jobMaterial.jmmUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobMaterialDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteJobMaterial(Guid jobMaterialId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobMaterialRepository iERPJobMaterialRepository = (base.ERPJobMaterialRepository = new ERPJobMaterialRepository(base.ApiClientContext));
		using (iERPJobMaterialRepository)
		{
			if (!(await base.ERPJobMaterialRepository.DoesJobMaterialExist(jobMaterialId)))
			{
				base.ErrorsList.Add($"JobMaterial [{jobMaterialId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPJobMaterialInformationDto eRPJobMaterialInformationDto = await base.ERPJobMaterialRepository.GetJobMaterial(jobMaterialId);
				string text = await base.ERPJobMaterialRepository.WhereUsed("JobMaterials", new object[3] { eRPJobMaterialInformationDto.jmmJobID, eRPJobMaterialInformationDto.jmmJobAssemblyID, eRPJobMaterialInformationDto.jmmJobMaterialID }, new object[3] { "jmmJobID", "jmmJobAssemblyID", "jmmJobMaterialID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("JobMaterial cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPJobMaterialDto>> Process_DeleteJobMaterial(Guid jobMaterialId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPJobMaterialDto> result;
		try
		{
			IERPJobMaterialRepository iERPJobMaterialRepository = (base.ERPJobMaterialRepository = new ERPJobMaterialRepository(base.ApiClientContext));
			using (iERPJobMaterialRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPJobMaterialRepository.DeleteRowFromTable("JobMaterials", "jmm", jobMaterialId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of JobMaterial [{jobMaterialId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobMaterialDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPJobMaterialDto()
			};
		}
		return result;
	}
}
