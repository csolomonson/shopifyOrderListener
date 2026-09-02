using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPMaterialIssueComponentModel : ERPBaseModel, IERPMaterialIssueComponentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllMaterialIssueComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPMaterialIssueComponentRepository iERPMaterialIssueComponentRepository = (base.ERPMaterialIssueComponentRepository = new ERPMaterialIssueComponentRepository(base.ApiClientContext));
		using (iERPMaterialIssueComponentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPMaterialIssueComponentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPMaterialIssueComponentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPMaterialIssueComponentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPMaterialIssueComponentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetMaterialIssueComponent(Guid materialIssueComponentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMaterialIssueComponentRepository iERPMaterialIssueComponentRepository = (base.ERPMaterialIssueComponentRepository = new ERPMaterialIssueComponentRepository(base.ApiClientContext));
		using (iERPMaterialIssueComponentRepository)
		{
			if (!(await base.ERPMaterialIssueComponentRepository.DoesMaterialIssueComponentExist(materialIssueComponentId)))
			{
				errorsList.Add($"MaterialIssueComponent [{materialIssueComponentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutMaterialIssueComponent(ERPMaterialIssueComponentDto materialIssueComponent)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMaterialIssueComponentRepository iERPMaterialIssueComponentRepository = (base.ERPMaterialIssueComponentRepository = new ERPMaterialIssueComponentRepository(base.ApiClientContext));
		using (iERPMaterialIssueComponentRepository)
		{
			if (!string.IsNullOrWhiteSpace(materialIssueComponent.inkMaterialIssueID) && !(await base.ERPMaterialIssueComponentRepository.DoesRecordExistInTableUsingKeys("MaterialIssues", new object[1] { "iniMaterialIssueID" }, new object[1] { materialIssueComponent.inkMaterialIssueID })))
			{
				errorsList.Add("inkMaterialIssueID [" + materialIssueComponent.inkMaterialIssueID + "] not found.");
			}
			if (materialIssueComponent.inkMaterialIssueLineID > 0 && !(await base.ERPMaterialIssueComponentRepository.DoesRecordExistInTableUsingKeys("MaterialIssueLines", new object[2] { "injMaterialIssueID", "injMaterialIssueLineID" }, new object[2] { materialIssueComponent.inkMaterialIssueID, materialIssueComponent.inkMaterialIssueLineID })))
			{
				errorsList.Add($"inkMaterialIssueLineID [{materialIssueComponent.inkMaterialIssueLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(materialIssueComponent.inkPartID) && !(await base.ERPMaterialIssueComponentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { materialIssueComponent.inkPartID })))
			{
				errorsList.Add("inkPartID [" + materialIssueComponent.inkPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(materialIssueComponent.inkPartRevisionID) && !(await base.ERPMaterialIssueComponentRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { materialIssueComponent.inkPartID, materialIssueComponent.inkPartRevisionID })))
			{
				errorsList.Add("inkPartRevisionID [" + materialIssueComponent.inkPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(materialIssueComponent.inkPartWarehouseLocationID) && !(await base.ERPMaterialIssueComponentRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { materialIssueComponent.inkPartID, materialIssueComponent.inkPartRevisionID, materialIssueComponent.inkPartWarehouseLocationID })))
			{
				errorsList.Add("inkPartWarehouseLocationID [" + materialIssueComponent.inkPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(materialIssueComponent.inkPartBinID) && !(await base.ERPMaterialIssueComponentRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { materialIssueComponent.inkPartID, materialIssueComponent.inkPartRevisionID, materialIssueComponent.inkPartWarehouseLocationID, materialIssueComponent.inkPartBinID })))
			{
				errorsList.Add("inkPartBinID [" + materialIssueComponent.inkPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(materialIssueComponent.inkJobID) && !(await base.ERPMaterialIssueComponentRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { materialIssueComponent.inkJobID })))
			{
				errorsList.Add("inkJobID [" + materialIssueComponent.inkJobID + "] not found.");
			}
			if (materialIssueComponent.inkJobAssemblyID > 0 && !(await base.ERPMaterialIssueComponentRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { materialIssueComponent.inkJobID, materialIssueComponent.inkJobAssemblyID })))
			{
				errorsList.Add($"inkJobAssemblyID [{materialIssueComponent.inkJobAssemblyID}] not found.");
			}
			if (materialIssueComponent.inkJobMaterialID > 0 && !(await base.ERPMaterialIssueComponentRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { materialIssueComponent.inkJobID, materialIssueComponent.inkJobAssemblyID, materialIssueComponent.inkJobMaterialID })))
			{
				errorsList.Add($"inkJobMaterialID [{materialIssueComponent.inkJobMaterialID}] not found.");
			}
			if (materialIssueComponent.inkJobMaterialComponentID > 0 && !(await base.ERPMaterialIssueComponentRepository.DoesRecordExistInTableUsingKeys("JobMaterialComponents", new object[4] { "JMTJOBID", "JMTJOBASSEMBLYID", "JMTJOBMATERIALID", "JMTJOBMATERIALCOMPONENTID" }, new object[4] { materialIssueComponent.inkJobID, materialIssueComponent.inkJobAssemblyID, materialIssueComponent.inkJobMaterialID, materialIssueComponent.inkJobMaterialComponentID })))
			{
				errorsList.Add($"inkJobMaterialComponentID [{materialIssueComponent.inkJobMaterialComponentID}] not found.");
			}
			if (materialIssueComponent.inkReverseMaterialIssueCompID > 0 && !(await base.ERPMaterialIssueComponentRepository.DoesRecordExistInTableUsingKeys("MaterialIssueComponents", new object[3] { "inkMaterialIssueID", "inkMaterialIssueLineID", "inkMaterialIssueComponentID" }, new object[3] { materialIssueComponent.inkReverseMaterialIssueID, materialIssueComponent.inkReverseMaterialIssueLineID, materialIssueComponent.inkReverseMaterialIssueCompID })))
			{
				errorsList.Add($"inkReverseMaterialIssueCompID [{materialIssueComponent.inkReverseMaterialIssueCompID}] not found.");
			}
			if (materialIssueComponent.inkReverseMaterialIssueLineID > 0 && !(await base.ERPMaterialIssueComponentRepository.DoesRecordExistInTableUsingKeys("MaterialIssueLines", new object[2] { "injMaterialIssueID", "injMaterialIssueLineID" }, new object[2] { materialIssueComponent.inkReverseMaterialIssueID, materialIssueComponent.inkReverseMaterialIssueLineID })))
			{
				errorsList.Add($"inkReverseMaterialIssueLineID [{materialIssueComponent.inkReverseMaterialIssueLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(materialIssueComponent.inkReverseMaterialIssueID) && !(await base.ERPMaterialIssueComponentRepository.DoesRecordExistInTableUsingKeys("MaterialIssues", new object[1] { "iniMaterialIssueID" }, new object[1] { materialIssueComponent.inkReverseMaterialIssueID })))
			{
				errorsList.Add("inkReverseMaterialIssueID [" + materialIssueComponent.inkReverseMaterialIssueID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPMaterialIssueComponentDto>>> Process_GetAllMaterialIssueComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPMaterialIssueComponentDto> allMaterialIssueComponentsDto = new List<ERPMaterialIssueComponentDto>();
		ERPResponseMessageDto<IList<ERPMaterialIssueComponentDto>> result;
		try
		{
			IERPMaterialIssueComponentRepository iERPMaterialIssueComponentRepository = (base.ERPMaterialIssueComponentRepository = new ERPMaterialIssueComponentRepository(base.ApiClientContext));
			using (iERPMaterialIssueComponentRepository)
			{
				foreach (ERPMaterialIssueComponentInformationDto item2 in await base.ERPMaterialIssueComponentRepository.GetAllMaterialIssueComponents(pageSize, pageNumber, filter, orderBy))
				{
					ERPMaterialIssueComponentDto item = new ERPMaterialIssueComponentDto
					{
						inkAdditionalQuantity = item2.inkAdditionalQuantity,
						inkCreatedBy = item2.inkCreatedBy,
						inkCreatedDate = item2.inkCreatedDate,
						inkDescription = item2.inkDescription,
						inkUniqueID = item2.inkUniqueID,
						inkInvIssueQuantity = item2.inkInvIssueQuantity,
						inkInvParentQuantity = item2.inkInvParentQuantity,
						inkInvParentQuantityScrap = item2.inkInvParentQuantityScrap,
						inkInvScrapQuantity = item2.inkInvScrapQuantity,
						inkPosted = item2.inkPosted,
						inkReceivedComplete = item2.inkReceivedComplete,
						inkReversed = item2.inkReversed,
						inkJobAssemblyID = item2.inkJobAssemblyID,
						inkJobID = item2.inkJobID,
						inkJobMaterialComponentID = item2.inkJobMaterialComponentID,
						inkJobMaterialID = item2.inkJobMaterialID,
						inkJobMatIssueQuantity = item2.inkJobMatIssueQuantity,
						inkJobMatParentQuantity = item2.inkJobMatParentQuantity,
						inkJobMatParentQuantityScrap = item2.inkJobMatParentQuantityScrap,
						inkJobMatParentReturnQty = item2.inkJobMatParentReturnQty,
						inkJobMatParentReturnQtyScrap = item2.inkJobMatParentReturnQtyScrap,
						inkJobMatReturnIssueQuantity = item2.inkJobMatReturnIssueQuantity,
						inkJobMatReturnScrapQuantity = item2.inkJobMatReturnScrapQuantity,
						inkJobMatScrapQuantity = item2.inkJobMatScrapQuantity,
						inkMaterialIssueID = item2.inkMaterialIssueID,
						inkMaterialIssueLineID = item2.inkMaterialIssueLineID,
						inkPartBinID = item2.inkPartBinID,
						inkPartID = item2.inkPartID,
						inkPartRevisionID = item2.inkPartRevisionID,
						inkPartWarehouseLocationID = item2.inkPartWarehouseLocationID,
						inkQuantityPerParent = item2.inkQuantityPerParent,
						inkReverseMaterialIssueCompID = item2.inkReverseMaterialIssueCompID,
						inkReverseMaterialIssueID = item2.inkReverseMaterialIssueID,
						inkReverseMaterialIssueLineID = item2.inkReverseMaterialIssueLineID,
						inkRowVersion = item2.inkRowVersion,
						inkMaterialIssueComponentID = item2.inkMaterialIssueComponentID,
						inkUnitOfMeasure = item2.inkUnitOfMeasure,
						inkWeight = item2.inkWeight,
						CustomFields = item2.CustomFields
					};
					allMaterialIssueComponentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all MaterialIssueComponents]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPMaterialIssueComponentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allMaterialIssueComponentsDto,
				RecordCount = allMaterialIssueComponentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMaterialIssueComponentDto>> Process_GetMaterialIssueComponent(Guid materialIssueComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPMaterialIssueComponentDto materialIssueComponentDto = null;
		ERPResponseMessageDto<ERPMaterialIssueComponentDto> result;
		try
		{
			IERPMaterialIssueComponentRepository iERPMaterialIssueComponentRepository = (base.ERPMaterialIssueComponentRepository = new ERPMaterialIssueComponentRepository(base.ApiClientContext));
			using (iERPMaterialIssueComponentRepository)
			{
				ERPMaterialIssueComponentInformationDto eRPMaterialIssueComponentInformationDto = await base.ERPMaterialIssueComponentRepository.GetMaterialIssueComponent(materialIssueComponentId);
				materialIssueComponentDto = new ERPMaterialIssueComponentDto
				{
					inkAdditionalQuantity = eRPMaterialIssueComponentInformationDto.inkAdditionalQuantity,
					inkCreatedBy = eRPMaterialIssueComponentInformationDto.inkCreatedBy,
					inkCreatedDate = eRPMaterialIssueComponentInformationDto.inkCreatedDate,
					inkDescription = eRPMaterialIssueComponentInformationDto.inkDescription,
					inkUniqueID = eRPMaterialIssueComponentInformationDto.inkUniqueID,
					inkInvIssueQuantity = eRPMaterialIssueComponentInformationDto.inkInvIssueQuantity,
					inkInvParentQuantity = eRPMaterialIssueComponentInformationDto.inkInvParentQuantity,
					inkInvParentQuantityScrap = eRPMaterialIssueComponentInformationDto.inkInvParentQuantityScrap,
					inkInvScrapQuantity = eRPMaterialIssueComponentInformationDto.inkInvScrapQuantity,
					inkPosted = eRPMaterialIssueComponentInformationDto.inkPosted,
					inkReceivedComplete = eRPMaterialIssueComponentInformationDto.inkReceivedComplete,
					inkReversed = eRPMaterialIssueComponentInformationDto.inkReversed,
					inkJobAssemblyID = eRPMaterialIssueComponentInformationDto.inkJobAssemblyID,
					inkJobID = eRPMaterialIssueComponentInformationDto.inkJobID,
					inkJobMaterialComponentID = eRPMaterialIssueComponentInformationDto.inkJobMaterialComponentID,
					inkJobMaterialID = eRPMaterialIssueComponentInformationDto.inkJobMaterialID,
					inkJobMatIssueQuantity = eRPMaterialIssueComponentInformationDto.inkJobMatIssueQuantity,
					inkJobMatParentQuantity = eRPMaterialIssueComponentInformationDto.inkJobMatParentQuantity,
					inkJobMatParentQuantityScrap = eRPMaterialIssueComponentInformationDto.inkJobMatParentQuantityScrap,
					inkJobMatParentReturnQty = eRPMaterialIssueComponentInformationDto.inkJobMatParentReturnQty,
					inkJobMatParentReturnQtyScrap = eRPMaterialIssueComponentInformationDto.inkJobMatParentReturnQtyScrap,
					inkJobMatReturnIssueQuantity = eRPMaterialIssueComponentInformationDto.inkJobMatReturnIssueQuantity,
					inkJobMatReturnScrapQuantity = eRPMaterialIssueComponentInformationDto.inkJobMatReturnScrapQuantity,
					inkJobMatScrapQuantity = eRPMaterialIssueComponentInformationDto.inkJobMatScrapQuantity,
					inkMaterialIssueID = eRPMaterialIssueComponentInformationDto.inkMaterialIssueID,
					inkMaterialIssueLineID = eRPMaterialIssueComponentInformationDto.inkMaterialIssueLineID,
					inkPartBinID = eRPMaterialIssueComponentInformationDto.inkPartBinID,
					inkPartID = eRPMaterialIssueComponentInformationDto.inkPartID,
					inkPartRevisionID = eRPMaterialIssueComponentInformationDto.inkPartRevisionID,
					inkPartWarehouseLocationID = eRPMaterialIssueComponentInformationDto.inkPartWarehouseLocationID,
					inkQuantityPerParent = eRPMaterialIssueComponentInformationDto.inkQuantityPerParent,
					inkReverseMaterialIssueCompID = eRPMaterialIssueComponentInformationDto.inkReverseMaterialIssueCompID,
					inkReverseMaterialIssueID = eRPMaterialIssueComponentInformationDto.inkReverseMaterialIssueID,
					inkReverseMaterialIssueLineID = eRPMaterialIssueComponentInformationDto.inkReverseMaterialIssueLineID,
					inkRowVersion = eRPMaterialIssueComponentInformationDto.inkRowVersion,
					inkMaterialIssueComponentID = eRPMaterialIssueComponentInformationDto.inkMaterialIssueComponentID,
					inkUnitOfMeasure = eRPMaterialIssueComponentInformationDto.inkUnitOfMeasure,
					inkWeight = eRPMaterialIssueComponentInformationDto.inkWeight,
					CustomFields = eRPMaterialIssueComponentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the MaterialIssueComponents []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMaterialIssueComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = materialIssueComponentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMaterialIssueComponentDto>> Process_PutMaterialIssueComponent(ERPMaterialIssueComponentDto materialIssueComponent)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPMaterialIssueComponentDto createdObject = null;
		ERPResponseMessageDto<ERPMaterialIssueComponentDto> result;
		try
		{
			IERPMaterialIssueComponentRepository iERPMaterialIssueComponentRepository = (base.ERPMaterialIssueComponentRepository = new ERPMaterialIssueComponentRepository(base.ApiClientContext));
			using (iERPMaterialIssueComponentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPMaterialIssueComponentRepository.SaveMaterialIssueComponent(materialIssueComponent);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPMaterialIssueComponentInformationDto eRPMaterialIssueComponentInformationDto = await base.ERPMaterialIssueComponentRepository.GetMaterialIssueComponent(materialIssueComponent.inkUniqueID);
					createdObject = new ERPMaterialIssueComponentDto
					{
						inkAdditionalQuantity = eRPMaterialIssueComponentInformationDto.inkAdditionalQuantity,
						inkCreatedBy = eRPMaterialIssueComponentInformationDto.inkCreatedBy,
						inkCreatedDate = eRPMaterialIssueComponentInformationDto.inkCreatedDate,
						inkDescription = eRPMaterialIssueComponentInformationDto.inkDescription,
						inkUniqueID = eRPMaterialIssueComponentInformationDto.inkUniqueID,
						inkInvIssueQuantity = eRPMaterialIssueComponentInformationDto.inkInvIssueQuantity,
						inkInvParentQuantity = eRPMaterialIssueComponentInformationDto.inkInvParentQuantity,
						inkInvParentQuantityScrap = eRPMaterialIssueComponentInformationDto.inkInvParentQuantityScrap,
						inkInvScrapQuantity = eRPMaterialIssueComponentInformationDto.inkInvScrapQuantity,
						inkPosted = eRPMaterialIssueComponentInformationDto.inkPosted,
						inkReceivedComplete = eRPMaterialIssueComponentInformationDto.inkReceivedComplete,
						inkReversed = eRPMaterialIssueComponentInformationDto.inkReversed,
						inkJobAssemblyID = eRPMaterialIssueComponentInformationDto.inkJobAssemblyID,
						inkJobID = eRPMaterialIssueComponentInformationDto.inkJobID,
						inkJobMaterialComponentID = eRPMaterialIssueComponentInformationDto.inkJobMaterialComponentID,
						inkJobMaterialID = eRPMaterialIssueComponentInformationDto.inkJobMaterialID,
						inkJobMatIssueQuantity = eRPMaterialIssueComponentInformationDto.inkJobMatIssueQuantity,
						inkJobMatParentQuantity = eRPMaterialIssueComponentInformationDto.inkJobMatParentQuantity,
						inkJobMatParentQuantityScrap = eRPMaterialIssueComponentInformationDto.inkJobMatParentQuantityScrap,
						inkJobMatParentReturnQty = eRPMaterialIssueComponentInformationDto.inkJobMatParentReturnQty,
						inkJobMatParentReturnQtyScrap = eRPMaterialIssueComponentInformationDto.inkJobMatParentReturnQtyScrap,
						inkJobMatReturnIssueQuantity = eRPMaterialIssueComponentInformationDto.inkJobMatReturnIssueQuantity,
						inkJobMatReturnScrapQuantity = eRPMaterialIssueComponentInformationDto.inkJobMatReturnScrapQuantity,
						inkJobMatScrapQuantity = eRPMaterialIssueComponentInformationDto.inkJobMatScrapQuantity,
						inkMaterialIssueID = eRPMaterialIssueComponentInformationDto.inkMaterialIssueID,
						inkMaterialIssueLineID = eRPMaterialIssueComponentInformationDto.inkMaterialIssueLineID,
						inkPartBinID = eRPMaterialIssueComponentInformationDto.inkPartBinID,
						inkPartID = eRPMaterialIssueComponentInformationDto.inkPartID,
						inkPartRevisionID = eRPMaterialIssueComponentInformationDto.inkPartRevisionID,
						inkPartWarehouseLocationID = eRPMaterialIssueComponentInformationDto.inkPartWarehouseLocationID,
						inkQuantityPerParent = eRPMaterialIssueComponentInformationDto.inkQuantityPerParent,
						inkReverseMaterialIssueCompID = eRPMaterialIssueComponentInformationDto.inkReverseMaterialIssueCompID,
						inkReverseMaterialIssueID = eRPMaterialIssueComponentInformationDto.inkReverseMaterialIssueID,
						inkReverseMaterialIssueLineID = eRPMaterialIssueComponentInformationDto.inkReverseMaterialIssueLineID,
						inkRowVersion = eRPMaterialIssueComponentInformationDto.inkRowVersion,
						inkMaterialIssueComponentID = eRPMaterialIssueComponentInformationDto.inkMaterialIssueComponentID,
						inkUnitOfMeasure = eRPMaterialIssueComponentInformationDto.inkUnitOfMeasure,
						inkWeight = eRPMaterialIssueComponentInformationDto.inkWeight,
						CustomFields = eRPMaterialIssueComponentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing MaterialIssueComponent [{materialIssueComponent.inkUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMaterialIssueComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteMaterialIssueComponent(Guid materialIssueComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMaterialIssueComponentRepository iERPMaterialIssueComponentRepository = (base.ERPMaterialIssueComponentRepository = new ERPMaterialIssueComponentRepository(base.ApiClientContext));
		using (iERPMaterialIssueComponentRepository)
		{
			if (!(await base.ERPMaterialIssueComponentRepository.DoesMaterialIssueComponentExist(materialIssueComponentId)))
			{
				base.ErrorsList.Add($"MaterialIssueComponent [{materialIssueComponentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPMaterialIssueComponentInformationDto eRPMaterialIssueComponentInformationDto = await base.ERPMaterialIssueComponentRepository.GetMaterialIssueComponent(materialIssueComponentId);
				string text = await base.ERPMaterialIssueComponentRepository.WhereUsed("MaterialIssueComponents", new object[3] { eRPMaterialIssueComponentInformationDto.inkMaterialIssueID, eRPMaterialIssueComponentInformationDto.inkMaterialIssueLineID, eRPMaterialIssueComponentInformationDto.inkMaterialIssueComponentID }, new object[3] { "inkMaterialIssueID", "inkMaterialIssueLineID", "inkMaterialIssueComponentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("MaterialIssueComponent cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPMaterialIssueComponentDto>> Process_DeleteMaterialIssueComponent(Guid materialIssueComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPMaterialIssueComponentDto> result;
		try
		{
			IERPMaterialIssueComponentRepository iERPMaterialIssueComponentRepository = (base.ERPMaterialIssueComponentRepository = new ERPMaterialIssueComponentRepository(base.ApiClientContext));
			using (iERPMaterialIssueComponentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPMaterialIssueComponentRepository.DeleteRowFromTable("MaterialIssueComponents", "ink", materialIssueComponentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of MaterialIssueComponent [{materialIssueComponentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMaterialIssueComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPMaterialIssueComponentDto()
			};
		}
		return result;
	}
}
