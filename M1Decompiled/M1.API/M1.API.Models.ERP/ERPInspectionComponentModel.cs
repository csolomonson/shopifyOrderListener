using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPInspectionComponentModel : ERPBaseModel, IERPInspectionComponentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllInspectionComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPInspectionComponentRepository iERPInspectionComponentRepository = (base.ERPInspectionComponentRepository = new ERPInspectionComponentRepository(base.ApiClientContext));
		using (iERPInspectionComponentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPInspectionComponentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPInspectionComponentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPInspectionComponentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPInspectionComponentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetInspectionComponent(Guid inspectionComponentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInspectionComponentRepository iERPInspectionComponentRepository = (base.ERPInspectionComponentRepository = new ERPInspectionComponentRepository(base.ApiClientContext));
		using (iERPInspectionComponentRepository)
		{
			if (!(await base.ERPInspectionComponentRepository.DoesInspectionComponentExist(inspectionComponentId)))
			{
				errorsList.Add($"InspectionComponent [{inspectionComponentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutInspectionComponent(ERPInspectionComponentDto inspectionComponent)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInspectionComponentRepository iERPInspectionComponentRepository = (base.ERPInspectionComponentRepository = new ERPInspectionComponentRepository(base.ApiClientContext));
		using (iERPInspectionComponentRepository)
		{
			if (!string.IsNullOrWhiteSpace(inspectionComponent.qamInspectionID) && !(await base.ERPInspectionComponentRepository.DoesRecordExistInTableUsingKeys("Inspections", new object[1] { "QAPINSPECTIONID" }, new object[1] { inspectionComponent.qamInspectionID })))
			{
				errorsList.Add("qamInspectionID [" + inspectionComponent.qamInspectionID + "] not found.");
			}
			if (inspectionComponent.qamInspectionLineID > 0 && !(await base.ERPInspectionComponentRepository.DoesRecordExistInTableUsingKeys("InspectionLines", new object[2] { "QALINSPECTIONID", "QALINSPECTIONLINEID" }, new object[2] { inspectionComponent.qamInspectionID, inspectionComponent.qamInspectionLineID })))
			{
				errorsList.Add($"qamInspectionLineID [{inspectionComponent.qamInspectionLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionComponent.qamPartID) && !(await base.ERPInspectionComponentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { inspectionComponent.qamPartID })))
			{
				errorsList.Add("qamPartID [" + inspectionComponent.qamPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionComponent.qamPartRevisionID) && !(await base.ERPInspectionComponentRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { inspectionComponent.qamPartID, inspectionComponent.qamPartRevisionID })))
			{
				errorsList.Add("qamPartRevisionID [" + inspectionComponent.qamPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionComponent.qamPartWarehouseLocationID) && !(await base.ERPInspectionComponentRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { inspectionComponent.qamPartID, inspectionComponent.qamPartRevisionID, inspectionComponent.qamPartWarehouseLocationID })))
			{
				errorsList.Add("qamPartWarehouseLocationID [" + inspectionComponent.qamPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionComponent.qamPartBinID) && !(await base.ERPInspectionComponentRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { inspectionComponent.qamPartID, inspectionComponent.qamPartRevisionID, inspectionComponent.qamPartWarehouseLocationID, inspectionComponent.qamPartBinID })))
			{
				errorsList.Add("qamPartBinID [" + inspectionComponent.qamPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspectionComponent.qamJobID) && !(await base.ERPInspectionComponentRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { inspectionComponent.qamJobID })))
			{
				errorsList.Add("qamJobID [" + inspectionComponent.qamJobID + "] not found.");
			}
			if (inspectionComponent.qamJobAssemblyID > 0 && !(await base.ERPInspectionComponentRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { inspectionComponent.qamJobID, inspectionComponent.qamJobAssemblyID })))
			{
				errorsList.Add($"qamJobAssemblyID [{inspectionComponent.qamJobAssemblyID}] not found.");
			}
			if (inspectionComponent.qamJobMaterialID > 0 && !(await base.ERPInspectionComponentRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { inspectionComponent.qamJobID, inspectionComponent.qamJobAssemblyID, inspectionComponent.qamJobMaterialID })))
			{
				errorsList.Add($"qamJobMaterialID [{inspectionComponent.qamJobMaterialID}] not found.");
			}
			if (inspectionComponent.qamJobMaterialComponentID > 0 && !(await base.ERPInspectionComponentRepository.DoesRecordExistInTableUsingKeys("JobMaterialComponents", new object[4] { "JMTJOBID", "JMTJOBASSEMBLYID", "JMTJOBMATERIALID", "JMTJOBMATERIALCOMPONENTID" }, new object[4] { inspectionComponent.qamJobID, inspectionComponent.qamJobAssemblyID, inspectionComponent.qamJobMaterialID, inspectionComponent.qamJobMaterialComponentID })))
			{
				errorsList.Add($"qamJobMaterialComponentID [{inspectionComponent.qamJobMaterialComponentID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPInspectionComponentDto>>> Process_GetAllInspectionComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPInspectionComponentDto> allInspectionComponentsDto = new List<ERPInspectionComponentDto>();
		ERPResponseMessageDto<IList<ERPInspectionComponentDto>> result;
		try
		{
			IERPInspectionComponentRepository iERPInspectionComponentRepository = (base.ERPInspectionComponentRepository = new ERPInspectionComponentRepository(base.ApiClientContext));
			using (iERPInspectionComponentRepository)
			{
				foreach (ERPInspectionComponentInformationDto item2 in await base.ERPInspectionComponentRepository.GetAllInspectionComponents(pageSize, pageNumber, filter, orderBy))
				{
					ERPInspectionComponentDto item = new ERPInspectionComponentDto
					{
						qamAdditionalQuantity = item2.qamAdditionalQuantity,
						qamComponentQtyToInspect = item2.qamComponentQtyToInspect,
						qamCreatedBy = item2.qamCreatedBy,
						qamCreatedDate = item2.qamCreatedDate,
						qamDescription = item2.qamDescription,
						qamUniqueID = item2.qamUniqueID,
						qamInspectionID = item2.qamInspectionID,
						qamInspectionLineID = item2.qamInspectionLineID,
						qamInspectionType = item2.qamInspectionType,
						qamInvParentQtyAccepted = item2.qamInvParentQtyAccepted,
						qamInvParentQtyToReturn = item2.qamInvParentQtyToReturn,
						qamInvParentQtyToScrap = item2.qamInvParentQtyToScrap,
						qamInvQuantityAccepted = item2.qamInvQuantityAccepted,
						qamInvQuantityToReturn = item2.qamInvQuantityToReturn,
						qamInvQuantityToScrap = item2.qamInvQuantityToScrap,
						qamInspectionComplete = item2.qamInspectionComplete,
						qamManualInspectionFinalized = item2.qamManualInspectionFinalized,
						qamPosted = item2.qamPosted,
						qamJobAssemblyID = item2.qamJobAssemblyID,
						qamJobID = item2.qamJobID,
						qamJobMaterialComponentID = item2.qamJobMaterialComponentID,
						qamJobMaterialID = item2.qamJobMaterialID,
						qamJobMatParentQtyAccepted = item2.qamJobMatParentQtyAccepted,
						qamJobMatParentQtyToReturn = item2.qamJobMatParentQtyToReturn,
						qamJobMatParentQtyToScrap = item2.qamJobMatParentQtyToScrap,
						qamJobMatQuantityAccepted = item2.qamJobMatQuantityAccepted,
						qamJobMatQuantityToReturn = item2.qamJobMatQuantityToReturn,
						qamJobMatQuantityToScrap = item2.qamJobMatQuantityToScrap,
						qamParentQtyToInspect = item2.qamParentQtyToInspect,
						qamPartBinID = item2.qamPartBinID,
						qamPartID = item2.qamPartID,
						qamPartRevisionID = item2.qamPartRevisionID,
						qamPartWarehouseLocationID = item2.qamPartWarehouseLocationID,
						qamQuantityPerParent = item2.qamQuantityPerParent,
						qamInspectionComponentID = item2.qamInspectionComponentID,
						qamSourceTableName = item2.qamSourceTableName,
						qamSourceTableUniqueID = item2.qamSourceTableUniqueID,
						qamUnitOfMeasure = item2.qamUnitOfMeasure,
						qamWeight = item2.qamWeight,
						CustomFields = item2.CustomFields
					};
					allInspectionComponentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all InspectionComponents]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPInspectionComponentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allInspectionComponentsDto,
				RecordCount = allInspectionComponentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPInspectionComponentDto>> Process_GetInspectionComponent(Guid inspectionComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPInspectionComponentDto inspectionComponentDto = null;
		ERPResponseMessageDto<ERPInspectionComponentDto> result;
		try
		{
			IERPInspectionComponentRepository iERPInspectionComponentRepository = (base.ERPInspectionComponentRepository = new ERPInspectionComponentRepository(base.ApiClientContext));
			using (iERPInspectionComponentRepository)
			{
				ERPInspectionComponentInformationDto eRPInspectionComponentInformationDto = await base.ERPInspectionComponentRepository.GetInspectionComponent(inspectionComponentId);
				inspectionComponentDto = new ERPInspectionComponentDto
				{
					qamAdditionalQuantity = eRPInspectionComponentInformationDto.qamAdditionalQuantity,
					qamComponentQtyToInspect = eRPInspectionComponentInformationDto.qamComponentQtyToInspect,
					qamCreatedBy = eRPInspectionComponentInformationDto.qamCreatedBy,
					qamCreatedDate = eRPInspectionComponentInformationDto.qamCreatedDate,
					qamDescription = eRPInspectionComponentInformationDto.qamDescription,
					qamUniqueID = eRPInspectionComponentInformationDto.qamUniqueID,
					qamInspectionID = eRPInspectionComponentInformationDto.qamInspectionID,
					qamInspectionLineID = eRPInspectionComponentInformationDto.qamInspectionLineID,
					qamInspectionType = eRPInspectionComponentInformationDto.qamInspectionType,
					qamInvParentQtyAccepted = eRPInspectionComponentInformationDto.qamInvParentQtyAccepted,
					qamInvParentQtyToReturn = eRPInspectionComponentInformationDto.qamInvParentQtyToReturn,
					qamInvParentQtyToScrap = eRPInspectionComponentInformationDto.qamInvParentQtyToScrap,
					qamInvQuantityAccepted = eRPInspectionComponentInformationDto.qamInvQuantityAccepted,
					qamInvQuantityToReturn = eRPInspectionComponentInformationDto.qamInvQuantityToReturn,
					qamInvQuantityToScrap = eRPInspectionComponentInformationDto.qamInvQuantityToScrap,
					qamInspectionComplete = eRPInspectionComponentInformationDto.qamInspectionComplete,
					qamManualInspectionFinalized = eRPInspectionComponentInformationDto.qamManualInspectionFinalized,
					qamPosted = eRPInspectionComponentInformationDto.qamPosted,
					qamJobAssemblyID = eRPInspectionComponentInformationDto.qamJobAssemblyID,
					qamJobID = eRPInspectionComponentInformationDto.qamJobID,
					qamJobMaterialComponentID = eRPInspectionComponentInformationDto.qamJobMaterialComponentID,
					qamJobMaterialID = eRPInspectionComponentInformationDto.qamJobMaterialID,
					qamJobMatParentQtyAccepted = eRPInspectionComponentInformationDto.qamJobMatParentQtyAccepted,
					qamJobMatParentQtyToReturn = eRPInspectionComponentInformationDto.qamJobMatParentQtyToReturn,
					qamJobMatParentQtyToScrap = eRPInspectionComponentInformationDto.qamJobMatParentQtyToScrap,
					qamJobMatQuantityAccepted = eRPInspectionComponentInformationDto.qamJobMatQuantityAccepted,
					qamJobMatQuantityToReturn = eRPInspectionComponentInformationDto.qamJobMatQuantityToReturn,
					qamJobMatQuantityToScrap = eRPInspectionComponentInformationDto.qamJobMatQuantityToScrap,
					qamParentQtyToInspect = eRPInspectionComponentInformationDto.qamParentQtyToInspect,
					qamPartBinID = eRPInspectionComponentInformationDto.qamPartBinID,
					qamPartID = eRPInspectionComponentInformationDto.qamPartID,
					qamPartRevisionID = eRPInspectionComponentInformationDto.qamPartRevisionID,
					qamPartWarehouseLocationID = eRPInspectionComponentInformationDto.qamPartWarehouseLocationID,
					qamQuantityPerParent = eRPInspectionComponentInformationDto.qamQuantityPerParent,
					qamInspectionComponentID = eRPInspectionComponentInformationDto.qamInspectionComponentID,
					qamSourceTableName = eRPInspectionComponentInformationDto.qamSourceTableName,
					qamSourceTableUniqueID = eRPInspectionComponentInformationDto.qamSourceTableUniqueID,
					qamUnitOfMeasure = eRPInspectionComponentInformationDto.qamUnitOfMeasure,
					qamWeight = eRPInspectionComponentInformationDto.qamWeight,
					CustomFields = eRPInspectionComponentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the InspectionComponents []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInspectionComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = inspectionComponentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPInspectionComponentDto>> Process_PutInspectionComponent(ERPInspectionComponentDto inspectionComponent)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPInspectionComponentDto createdObject = null;
		ERPResponseMessageDto<ERPInspectionComponentDto> result;
		try
		{
			IERPInspectionComponentRepository iERPInspectionComponentRepository = (base.ERPInspectionComponentRepository = new ERPInspectionComponentRepository(base.ApiClientContext));
			using (iERPInspectionComponentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPInspectionComponentRepository.SaveInspectionComponent(inspectionComponent);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPInspectionComponentInformationDto eRPInspectionComponentInformationDto = await base.ERPInspectionComponentRepository.GetInspectionComponent(inspectionComponent.qamUniqueID);
					createdObject = new ERPInspectionComponentDto
					{
						qamAdditionalQuantity = eRPInspectionComponentInformationDto.qamAdditionalQuantity,
						qamComponentQtyToInspect = eRPInspectionComponentInformationDto.qamComponentQtyToInspect,
						qamCreatedBy = eRPInspectionComponentInformationDto.qamCreatedBy,
						qamCreatedDate = eRPInspectionComponentInformationDto.qamCreatedDate,
						qamDescription = eRPInspectionComponentInformationDto.qamDescription,
						qamUniqueID = eRPInspectionComponentInformationDto.qamUniqueID,
						qamInspectionID = eRPInspectionComponentInformationDto.qamInspectionID,
						qamInspectionLineID = eRPInspectionComponentInformationDto.qamInspectionLineID,
						qamInspectionType = eRPInspectionComponentInformationDto.qamInspectionType,
						qamInvParentQtyAccepted = eRPInspectionComponentInformationDto.qamInvParentQtyAccepted,
						qamInvParentQtyToReturn = eRPInspectionComponentInformationDto.qamInvParentQtyToReturn,
						qamInvParentQtyToScrap = eRPInspectionComponentInformationDto.qamInvParentQtyToScrap,
						qamInvQuantityAccepted = eRPInspectionComponentInformationDto.qamInvQuantityAccepted,
						qamInvQuantityToReturn = eRPInspectionComponentInformationDto.qamInvQuantityToReturn,
						qamInvQuantityToScrap = eRPInspectionComponentInformationDto.qamInvQuantityToScrap,
						qamInspectionComplete = eRPInspectionComponentInformationDto.qamInspectionComplete,
						qamManualInspectionFinalized = eRPInspectionComponentInformationDto.qamManualInspectionFinalized,
						qamPosted = eRPInspectionComponentInformationDto.qamPosted,
						qamJobAssemblyID = eRPInspectionComponentInformationDto.qamJobAssemblyID,
						qamJobID = eRPInspectionComponentInformationDto.qamJobID,
						qamJobMaterialComponentID = eRPInspectionComponentInformationDto.qamJobMaterialComponentID,
						qamJobMaterialID = eRPInspectionComponentInformationDto.qamJobMaterialID,
						qamJobMatParentQtyAccepted = eRPInspectionComponentInformationDto.qamJobMatParentQtyAccepted,
						qamJobMatParentQtyToReturn = eRPInspectionComponentInformationDto.qamJobMatParentQtyToReturn,
						qamJobMatParentQtyToScrap = eRPInspectionComponentInformationDto.qamJobMatParentQtyToScrap,
						qamJobMatQuantityAccepted = eRPInspectionComponentInformationDto.qamJobMatQuantityAccepted,
						qamJobMatQuantityToReturn = eRPInspectionComponentInformationDto.qamJobMatQuantityToReturn,
						qamJobMatQuantityToScrap = eRPInspectionComponentInformationDto.qamJobMatQuantityToScrap,
						qamParentQtyToInspect = eRPInspectionComponentInformationDto.qamParentQtyToInspect,
						qamPartBinID = eRPInspectionComponentInformationDto.qamPartBinID,
						qamPartID = eRPInspectionComponentInformationDto.qamPartID,
						qamPartRevisionID = eRPInspectionComponentInformationDto.qamPartRevisionID,
						qamPartWarehouseLocationID = eRPInspectionComponentInformationDto.qamPartWarehouseLocationID,
						qamQuantityPerParent = eRPInspectionComponentInformationDto.qamQuantityPerParent,
						qamInspectionComponentID = eRPInspectionComponentInformationDto.qamInspectionComponentID,
						qamSourceTableName = eRPInspectionComponentInformationDto.qamSourceTableName,
						qamSourceTableUniqueID = eRPInspectionComponentInformationDto.qamSourceTableUniqueID,
						qamUnitOfMeasure = eRPInspectionComponentInformationDto.qamUnitOfMeasure,
						qamWeight = eRPInspectionComponentInformationDto.qamWeight,
						CustomFields = eRPInspectionComponentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing InspectionComponent [{inspectionComponent.qamUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInspectionComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteInspectionComponent(Guid inspectionComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInspectionComponentRepository iERPInspectionComponentRepository = (base.ERPInspectionComponentRepository = new ERPInspectionComponentRepository(base.ApiClientContext));
		using (iERPInspectionComponentRepository)
		{
			if (!(await base.ERPInspectionComponentRepository.DoesInspectionComponentExist(inspectionComponentId)))
			{
				base.ErrorsList.Add($"InspectionComponent [{inspectionComponentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPInspectionComponentInformationDto eRPInspectionComponentInformationDto = await base.ERPInspectionComponentRepository.GetInspectionComponent(inspectionComponentId);
				string text = await base.ERPInspectionComponentRepository.WhereUsed("InspectionComponents", new object[3] { eRPInspectionComponentInformationDto.qamInspectionID, eRPInspectionComponentInformationDto.qamInspectionLineID, eRPInspectionComponentInformationDto.qamInspectionComponentID }, new object[3] { "qamInspectionID", "qamInspectionLineID", "qamInspectionComponentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("InspectionComponent cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPInspectionComponentDto>> Process_DeleteInspectionComponent(Guid inspectionComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPInspectionComponentDto> result;
		try
		{
			IERPInspectionComponentRepository iERPInspectionComponentRepository = (base.ERPInspectionComponentRepository = new ERPInspectionComponentRepository(base.ApiClientContext));
			using (iERPInspectionComponentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPInspectionComponentRepository.DeleteRowFromTable("InspectionComponents", "qam", inspectionComponentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of InspectionComponent [{inspectionComponentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInspectionComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPInspectionComponentDto()
			};
		}
		return result;
	}
}
