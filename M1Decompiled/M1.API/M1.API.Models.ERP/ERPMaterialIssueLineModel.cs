using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPMaterialIssueLineModel : ERPBaseModel, IERPMaterialIssueLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllMaterialIssueLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPMaterialIssueLineRepository iERPMaterialIssueLineRepository = (base.ERPMaterialIssueLineRepository = new ERPMaterialIssueLineRepository(base.ApiClientContext));
		using (iERPMaterialIssueLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPMaterialIssueLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPMaterialIssueLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPMaterialIssueLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPMaterialIssueLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetMaterialIssueLine(Guid materialIssueLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMaterialIssueLineRepository iERPMaterialIssueLineRepository = (base.ERPMaterialIssueLineRepository = new ERPMaterialIssueLineRepository(base.ApiClientContext));
		using (iERPMaterialIssueLineRepository)
		{
			if (!(await base.ERPMaterialIssueLineRepository.DoesMaterialIssueLineExist(materialIssueLineId)))
			{
				errorsList.Add($"MaterialIssueLine [{materialIssueLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutMaterialIssueLine(ERPMaterialIssueLineDto materialIssueLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMaterialIssueLineRepository iERPMaterialIssueLineRepository = (base.ERPMaterialIssueLineRepository = new ERPMaterialIssueLineRepository(base.ApiClientContext));
		using (iERPMaterialIssueLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(materialIssueLine.injMaterialIssueID) && !(await base.ERPMaterialIssueLineRepository.DoesRecordExistInTableUsingKeys("MaterialIssues", new object[1] { "iniMaterialIssueID" }, new object[1] { materialIssueLine.injMaterialIssueID })))
			{
				errorsList.Add("injMaterialIssueID [" + materialIssueLine.injMaterialIssueID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(materialIssueLine.injJobID) && !(await base.ERPMaterialIssueLineRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { materialIssueLine.injJobID })))
			{
				errorsList.Add("injJobID [" + materialIssueLine.injJobID + "] not found.");
			}
			if (materialIssueLine.injJobAssemblyID > 0 && !(await base.ERPMaterialIssueLineRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { materialIssueLine.injJobID, materialIssueLine.injJobAssemblyID })))
			{
				errorsList.Add($"injJobAssemblyID [{materialIssueLine.injJobAssemblyID}] not found.");
			}
			if (materialIssueLine.injJobMaterialID > 0 && !(await base.ERPMaterialIssueLineRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { materialIssueLine.injJobID, materialIssueLine.injJobAssemblyID, materialIssueLine.injJobMaterialID })))
			{
				errorsList.Add($"injJobMaterialID [{materialIssueLine.injJobMaterialID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(materialIssueLine.injPartID) && !(await base.ERPMaterialIssueLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { materialIssueLine.injPartID })))
			{
				errorsList.Add("injPartID [" + materialIssueLine.injPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(materialIssueLine.injPartRevisionID) && !(await base.ERPMaterialIssueLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { materialIssueLine.injPartID, materialIssueLine.injPartRevisionID })))
			{
				errorsList.Add("injPartRevisionID [" + materialIssueLine.injPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(materialIssueLine.injPartWarehouseLocationID) && !(await base.ERPMaterialIssueLineRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { materialIssueLine.injPartID, materialIssueLine.injPartRevisionID, materialIssueLine.injPartWarehouseLocationID })))
			{
				errorsList.Add("injPartWarehouseLocationID [" + materialIssueLine.injPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(materialIssueLine.injPartBinID) && !(await base.ERPMaterialIssueLineRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { materialIssueLine.injPartID, materialIssueLine.injPartRevisionID, materialIssueLine.injPartWarehouseLocationID, materialIssueLine.injPartBinID })))
			{
				errorsList.Add("injPartBinID [" + materialIssueLine.injPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(materialIssueLine.injMiscIssueReasonID) && !(await base.ERPMaterialIssueLineRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { materialIssueLine.injMiscIssueReasonID })))
			{
				errorsList.Add("injMiscIssueReasonID [" + materialIssueLine.injMiscIssueReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(materialIssueLine.injProjectID) && !(await base.ERPMaterialIssueLineRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { materialIssueLine.injProjectID })))
			{
				errorsList.Add("injProjectID [" + materialIssueLine.injProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(materialIssueLine.injProjectAreaID) && !(await base.ERPMaterialIssueLineRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { materialIssueLine.injProjectID, materialIssueLine.injProjectAreaID })))
			{
				errorsList.Add("injProjectAreaID [" + materialIssueLine.injProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(materialIssueLine.injPlantID) && !(await base.ERPMaterialIssueLineRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { materialIssueLine.injPlantID })))
			{
				errorsList.Add("injPlantID [" + materialIssueLine.injPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(materialIssueLine.injReverseMaterialIssueID) && !(await base.ERPMaterialIssueLineRepository.DoesRecordExistInTableUsingKeys("MaterialIssues", new object[1] { "iniMaterialIssueID" }, new object[1] { materialIssueLine.injReverseMaterialIssueID })))
			{
				errorsList.Add("injReverseMaterialIssueID [" + materialIssueLine.injReverseMaterialIssueID + "] not found.");
			}
			if (materialIssueLine.injReverseMaterialIssueLineID > 0 && !(await base.ERPMaterialIssueLineRepository.DoesRecordExistInTableUsingKeys("MaterialIssueLines", new object[2] { "injMaterialIssueID", "injMaterialIssueLineID" }, new object[2] { materialIssueLine.injReverseMaterialIssueID, materialIssueLine.injReverseMaterialIssueLineID })))
			{
				errorsList.Add($"injReverseMaterialIssueLineID [{materialIssueLine.injReverseMaterialIssueLineID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPMaterialIssueLineDto>>> Process_GetAllMaterialIssueLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPMaterialIssueLineDto> allMaterialIssueLinesDto = new List<ERPMaterialIssueLineDto>();
		ERPResponseMessageDto<IList<ERPMaterialIssueLineDto>> result;
		try
		{
			IERPMaterialIssueLineRepository iERPMaterialIssueLineRepository = (base.ERPMaterialIssueLineRepository = new ERPMaterialIssueLineRepository(base.ApiClientContext));
			using (iERPMaterialIssueLineRepository)
			{
				foreach (ERPMaterialIssueLineInformationDto item2 in await base.ERPMaterialIssueLineRepository.GetAllMaterialIssueLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPMaterialIssueLineDto item = new ERPMaterialIssueLineDto
					{
						injCreatedBy = item2.injCreatedBy,
						injCreatedDate = item2.injCreatedDate,
						injUniqueID = item2.injUniqueID,
						injEstimatedQuantity = item2.injEstimatedQuantity,
						injHeatLot = item2.injHeatLot,
						injInvIssueQuantity = item2.injInvIssueQuantity,
						injInvScrapQuantity = item2.injInvScrapQuantity,
						injCreateJobSeq = item2.injCreateJobSeq,
						injIssueComplete = item2.injIssueComplete,
						injKitPart = item2.injKitPart,
						injPosted = item2.injPosted,
						injReversed = item2.injReversed,
						injIssueType = item2.injIssueType,
						injJobAsmIssueQuantity = item2.injJobAsmIssueQuantity,
						injJobAsmScrapQuantity = item2.injJobAsmScrapQuantity,
						injJobAssemblyID = item2.injJobAssemblyID,
						injJobID = item2.injJobID,
						injJobMaterialID = item2.injJobMaterialID,
						injJobMatIssueQuantity = item2.injJobMatIssueQuantity,
						injJobMatReturnIssueQuantity = item2.injJobMatReturnIssueQuantity,
						injJobMatReturnScrapQuantity = item2.injJobMatReturnScrapQuantity,
						injJobMatScrapQuantity = item2.injJobMatScrapQuantity,
						injJobOpenQuantity = item2.injJobOpenQuantity,
						injJobType = item2.injJobType,
						injLongDescriptionRtf = item2.injLongDescriptionRtf,
						injLongDescriptionText = item2.injLongDescriptionText,
						injMaterialIssueID = item2.injMaterialIssueID,
						injMiscIssueReasonID = item2.injMiscIssueReasonID,
						injPartBinID = item2.injPartBinID,
						injPartID = item2.injPartID,
						injPartRevisionID = item2.injPartRevisionID,
						injPartWarehouseLocationID = item2.injPartWarehouseLocationID,
						injPlantID = item2.injPlantID,
						injProjectAreaID = item2.injProjectAreaID,
						injProjectID = item2.injProjectID,
						injQuantityAllocated = item2.injQuantityAllocated,
						injQuantityOnHand = item2.injQuantityOnHand,
						injReference = item2.injReference,
						injReverseMaterialIssueID = item2.injReverseMaterialIssueID,
						injReverseMaterialIssueLineID = item2.injReverseMaterialIssueLineID,
						injRowVersion = item2.injRowVersion,
						injMaterialIssueLineID = item2.injMaterialIssueLineID,
						CustomFields = item2.CustomFields
					};
					allMaterialIssueLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all MaterialIssueLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPMaterialIssueLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allMaterialIssueLinesDto,
				RecordCount = allMaterialIssueLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMaterialIssueLineDto>> Process_GetMaterialIssueLine(Guid materialIssueLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPMaterialIssueLineDto materialIssueLineDto = null;
		ERPResponseMessageDto<ERPMaterialIssueLineDto> result;
		try
		{
			IERPMaterialIssueLineRepository iERPMaterialIssueLineRepository = (base.ERPMaterialIssueLineRepository = new ERPMaterialIssueLineRepository(base.ApiClientContext));
			using (iERPMaterialIssueLineRepository)
			{
				ERPMaterialIssueLineInformationDto eRPMaterialIssueLineInformationDto = await base.ERPMaterialIssueLineRepository.GetMaterialIssueLine(materialIssueLineId);
				materialIssueLineDto = new ERPMaterialIssueLineDto
				{
					injCreatedBy = eRPMaterialIssueLineInformationDto.injCreatedBy,
					injCreatedDate = eRPMaterialIssueLineInformationDto.injCreatedDate,
					injUniqueID = eRPMaterialIssueLineInformationDto.injUniqueID,
					injEstimatedQuantity = eRPMaterialIssueLineInformationDto.injEstimatedQuantity,
					injHeatLot = eRPMaterialIssueLineInformationDto.injHeatLot,
					injInvIssueQuantity = eRPMaterialIssueLineInformationDto.injInvIssueQuantity,
					injInvScrapQuantity = eRPMaterialIssueLineInformationDto.injInvScrapQuantity,
					injCreateJobSeq = eRPMaterialIssueLineInformationDto.injCreateJobSeq,
					injIssueComplete = eRPMaterialIssueLineInformationDto.injIssueComplete,
					injKitPart = eRPMaterialIssueLineInformationDto.injKitPart,
					injPosted = eRPMaterialIssueLineInformationDto.injPosted,
					injReversed = eRPMaterialIssueLineInformationDto.injReversed,
					injIssueType = eRPMaterialIssueLineInformationDto.injIssueType,
					injJobAsmIssueQuantity = eRPMaterialIssueLineInformationDto.injJobAsmIssueQuantity,
					injJobAsmScrapQuantity = eRPMaterialIssueLineInformationDto.injJobAsmScrapQuantity,
					injJobAssemblyID = eRPMaterialIssueLineInformationDto.injJobAssemblyID,
					injJobID = eRPMaterialIssueLineInformationDto.injJobID,
					injJobMaterialID = eRPMaterialIssueLineInformationDto.injJobMaterialID,
					injJobMatIssueQuantity = eRPMaterialIssueLineInformationDto.injJobMatIssueQuantity,
					injJobMatReturnIssueQuantity = eRPMaterialIssueLineInformationDto.injJobMatReturnIssueQuantity,
					injJobMatReturnScrapQuantity = eRPMaterialIssueLineInformationDto.injJobMatReturnScrapQuantity,
					injJobMatScrapQuantity = eRPMaterialIssueLineInformationDto.injJobMatScrapQuantity,
					injJobOpenQuantity = eRPMaterialIssueLineInformationDto.injJobOpenQuantity,
					injJobType = eRPMaterialIssueLineInformationDto.injJobType,
					injLongDescriptionRtf = eRPMaterialIssueLineInformationDto.injLongDescriptionRtf,
					injLongDescriptionText = eRPMaterialIssueLineInformationDto.injLongDescriptionText,
					injMaterialIssueID = eRPMaterialIssueLineInformationDto.injMaterialIssueID,
					injMiscIssueReasonID = eRPMaterialIssueLineInformationDto.injMiscIssueReasonID,
					injPartBinID = eRPMaterialIssueLineInformationDto.injPartBinID,
					injPartID = eRPMaterialIssueLineInformationDto.injPartID,
					injPartRevisionID = eRPMaterialIssueLineInformationDto.injPartRevisionID,
					injPartWarehouseLocationID = eRPMaterialIssueLineInformationDto.injPartWarehouseLocationID,
					injPlantID = eRPMaterialIssueLineInformationDto.injPlantID,
					injProjectAreaID = eRPMaterialIssueLineInformationDto.injProjectAreaID,
					injProjectID = eRPMaterialIssueLineInformationDto.injProjectID,
					injQuantityAllocated = eRPMaterialIssueLineInformationDto.injQuantityAllocated,
					injQuantityOnHand = eRPMaterialIssueLineInformationDto.injQuantityOnHand,
					injReference = eRPMaterialIssueLineInformationDto.injReference,
					injReverseMaterialIssueID = eRPMaterialIssueLineInformationDto.injReverseMaterialIssueID,
					injReverseMaterialIssueLineID = eRPMaterialIssueLineInformationDto.injReverseMaterialIssueLineID,
					injRowVersion = eRPMaterialIssueLineInformationDto.injRowVersion,
					injMaterialIssueLineID = eRPMaterialIssueLineInformationDto.injMaterialIssueLineID,
					CustomFields = eRPMaterialIssueLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the MaterialIssueLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMaterialIssueLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = materialIssueLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMaterialIssueLineDto>> Process_PutMaterialIssueLine(ERPMaterialIssueLineDto materialIssueLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPMaterialIssueLineDto createdObject = null;
		ERPResponseMessageDto<ERPMaterialIssueLineDto> result;
		try
		{
			IERPMaterialIssueLineRepository iERPMaterialIssueLineRepository = (base.ERPMaterialIssueLineRepository = new ERPMaterialIssueLineRepository(base.ApiClientContext));
			using (iERPMaterialIssueLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPMaterialIssueLineRepository.SaveMaterialIssueLine(materialIssueLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPMaterialIssueLineInformationDto eRPMaterialIssueLineInformationDto = await base.ERPMaterialIssueLineRepository.GetMaterialIssueLine(materialIssueLine.injUniqueID);
					createdObject = new ERPMaterialIssueLineDto
					{
						injCreatedBy = eRPMaterialIssueLineInformationDto.injCreatedBy,
						injCreatedDate = eRPMaterialIssueLineInformationDto.injCreatedDate,
						injUniqueID = eRPMaterialIssueLineInformationDto.injUniqueID,
						injEstimatedQuantity = eRPMaterialIssueLineInformationDto.injEstimatedQuantity,
						injHeatLot = eRPMaterialIssueLineInformationDto.injHeatLot,
						injInvIssueQuantity = eRPMaterialIssueLineInformationDto.injInvIssueQuantity,
						injInvScrapQuantity = eRPMaterialIssueLineInformationDto.injInvScrapQuantity,
						injCreateJobSeq = eRPMaterialIssueLineInformationDto.injCreateJobSeq,
						injIssueComplete = eRPMaterialIssueLineInformationDto.injIssueComplete,
						injKitPart = eRPMaterialIssueLineInformationDto.injKitPart,
						injPosted = eRPMaterialIssueLineInformationDto.injPosted,
						injReversed = eRPMaterialIssueLineInformationDto.injReversed,
						injIssueType = eRPMaterialIssueLineInformationDto.injIssueType,
						injJobAsmIssueQuantity = eRPMaterialIssueLineInformationDto.injJobAsmIssueQuantity,
						injJobAsmScrapQuantity = eRPMaterialIssueLineInformationDto.injJobAsmScrapQuantity,
						injJobAssemblyID = eRPMaterialIssueLineInformationDto.injJobAssemblyID,
						injJobID = eRPMaterialIssueLineInformationDto.injJobID,
						injJobMaterialID = eRPMaterialIssueLineInformationDto.injJobMaterialID,
						injJobMatIssueQuantity = eRPMaterialIssueLineInformationDto.injJobMatIssueQuantity,
						injJobMatReturnIssueQuantity = eRPMaterialIssueLineInformationDto.injJobMatReturnIssueQuantity,
						injJobMatReturnScrapQuantity = eRPMaterialIssueLineInformationDto.injJobMatReturnScrapQuantity,
						injJobMatScrapQuantity = eRPMaterialIssueLineInformationDto.injJobMatScrapQuantity,
						injJobOpenQuantity = eRPMaterialIssueLineInformationDto.injJobOpenQuantity,
						injJobType = eRPMaterialIssueLineInformationDto.injJobType,
						injLongDescriptionRtf = eRPMaterialIssueLineInformationDto.injLongDescriptionRtf,
						injLongDescriptionText = eRPMaterialIssueLineInformationDto.injLongDescriptionText,
						injMaterialIssueID = eRPMaterialIssueLineInformationDto.injMaterialIssueID,
						injMiscIssueReasonID = eRPMaterialIssueLineInformationDto.injMiscIssueReasonID,
						injPartBinID = eRPMaterialIssueLineInformationDto.injPartBinID,
						injPartID = eRPMaterialIssueLineInformationDto.injPartID,
						injPartRevisionID = eRPMaterialIssueLineInformationDto.injPartRevisionID,
						injPartWarehouseLocationID = eRPMaterialIssueLineInformationDto.injPartWarehouseLocationID,
						injPlantID = eRPMaterialIssueLineInformationDto.injPlantID,
						injProjectAreaID = eRPMaterialIssueLineInformationDto.injProjectAreaID,
						injProjectID = eRPMaterialIssueLineInformationDto.injProjectID,
						injQuantityAllocated = eRPMaterialIssueLineInformationDto.injQuantityAllocated,
						injQuantityOnHand = eRPMaterialIssueLineInformationDto.injQuantityOnHand,
						injReference = eRPMaterialIssueLineInformationDto.injReference,
						injReverseMaterialIssueID = eRPMaterialIssueLineInformationDto.injReverseMaterialIssueID,
						injReverseMaterialIssueLineID = eRPMaterialIssueLineInformationDto.injReverseMaterialIssueLineID,
						injRowVersion = eRPMaterialIssueLineInformationDto.injRowVersion,
						injMaterialIssueLineID = eRPMaterialIssueLineInformationDto.injMaterialIssueLineID,
						CustomFields = eRPMaterialIssueLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing MaterialIssueLine [{materialIssueLine.injUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMaterialIssueLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteMaterialIssueLine(Guid materialIssueLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMaterialIssueLineRepository iERPMaterialIssueLineRepository = (base.ERPMaterialIssueLineRepository = new ERPMaterialIssueLineRepository(base.ApiClientContext));
		using (iERPMaterialIssueLineRepository)
		{
			if (!(await base.ERPMaterialIssueLineRepository.DoesMaterialIssueLineExist(materialIssueLineId)))
			{
				base.ErrorsList.Add($"MaterialIssueLine [{materialIssueLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPMaterialIssueLineInformationDto eRPMaterialIssueLineInformationDto = await base.ERPMaterialIssueLineRepository.GetMaterialIssueLine(materialIssueLineId);
				string text = await base.ERPMaterialIssueLineRepository.WhereUsed("MaterialIssueLines", new object[2] { eRPMaterialIssueLineInformationDto.injMaterialIssueID, eRPMaterialIssueLineInformationDto.injMaterialIssueLineID }, new object[2] { "injMaterialIssueID", "injMaterialIssueLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("MaterialIssueLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPMaterialIssueLineDto>> Process_DeleteMaterialIssueLine(Guid materialIssueLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPMaterialIssueLineDto> result;
		try
		{
			IERPMaterialIssueLineRepository iERPMaterialIssueLineRepository = (base.ERPMaterialIssueLineRepository = new ERPMaterialIssueLineRepository(base.ApiClientContext));
			using (iERPMaterialIssueLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPMaterialIssueLineRepository.DeleteRowFromTable("MaterialIssueLines", "inj", materialIssueLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of MaterialIssueLine [{materialIssueLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMaterialIssueLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPMaterialIssueLineDto()
			};
		}
		return result;
	}
}
