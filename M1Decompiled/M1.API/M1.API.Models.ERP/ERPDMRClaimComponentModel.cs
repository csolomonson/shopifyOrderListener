using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPDMRClaimComponentModel : ERPBaseModel, IERPDMRClaimComponentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllDMRClaimComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPDMRClaimComponentRepository iERPDMRClaimComponentRepository = (base.ERPDMRClaimComponentRepository = new ERPDMRClaimComponentRepository(base.ApiClientContext));
		using (iERPDMRClaimComponentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPDMRClaimComponentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPDMRClaimComponentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPDMRClaimComponentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPDMRClaimComponentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetDMRClaimComponent(Guid dMRClaimComponentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRClaimComponentRepository iERPDMRClaimComponentRepository = (base.ERPDMRClaimComponentRepository = new ERPDMRClaimComponentRepository(base.ApiClientContext));
		using (iERPDMRClaimComponentRepository)
		{
			if (!(await base.ERPDMRClaimComponentRepository.DoesDMRClaimComponentExist(dMRClaimComponentId)))
			{
				errorsList.Add($"DMRClaimComponent [{dMRClaimComponentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutDMRClaimComponent(ERPDMRClaimComponentDto dMRClaimComponent)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRClaimComponentRepository iERPDMRClaimComponentRepository = (base.ERPDMRClaimComponentRepository = new ERPDMRClaimComponentRepository(base.ApiClientContext));
		using (iERPDMRClaimComponentRepository)
		{
			if (!string.IsNullOrWhiteSpace(dMRClaimComponent.dmoDmrClaimID) && !(await base.ERPDMRClaimComponentRepository.DoesRecordExistInTableUsingKeys("DMRClaims", new object[1] { "DMPDMRCLAIMID" }, new object[1] { dMRClaimComponent.dmoDmrClaimID })))
			{
				errorsList.Add("dmoDmrClaimID [" + dMRClaimComponent.dmoDmrClaimID + "] not found.");
			}
			if (dMRClaimComponent.dmoDmrClaimLineID > 0 && !(await base.ERPDMRClaimComponentRepository.DoesRecordExistInTableUsingKeys("DMRClaimLines", new object[2] { "DMLDMRCLAIMID", "DMLDMRCLAIMLINEID" }, new object[2] { dMRClaimComponent.dmoDmrClaimID, dMRClaimComponent.dmoDmrClaimLineID })))
			{
				errorsList.Add($"dmoDmrClaimLineID [{dMRClaimComponent.dmoDmrClaimLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimComponent.dmoPartID) && !(await base.ERPDMRClaimComponentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { dMRClaimComponent.dmoPartID })))
			{
				errorsList.Add("dmoPartID [" + dMRClaimComponent.dmoPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimComponent.dmoPartRevisionID) && !(await base.ERPDMRClaimComponentRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { dMRClaimComponent.dmoPartID, dMRClaimComponent.dmoPartRevisionID })))
			{
				errorsList.Add("dmoPartRevisionID [" + dMRClaimComponent.dmoPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimComponent.dmoPartWarehouseLocationID) && !(await base.ERPDMRClaimComponentRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { dMRClaimComponent.dmoPartID, dMRClaimComponent.dmoPartRevisionID, dMRClaimComponent.dmoPartWarehouseLocationID })))
			{
				errorsList.Add("dmoPartWarehouseLocationID [" + dMRClaimComponent.dmoPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimComponent.dmoPartBinID) && !(await base.ERPDMRClaimComponentRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { dMRClaimComponent.dmoPartID, dMRClaimComponent.dmoPartRevisionID, dMRClaimComponent.dmoPartWarehouseLocationID, dMRClaimComponent.dmoPartBinID })))
			{
				errorsList.Add("dmoPartBinID [" + dMRClaimComponent.dmoPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimComponent.dmoInspectionID) && !(await base.ERPDMRClaimComponentRepository.DoesRecordExistInTableUsingKeys("Inspections", new object[1] { "QAPINSPECTIONID" }, new object[1] { dMRClaimComponent.dmoInspectionID })))
			{
				errorsList.Add("dmoInspectionID [" + dMRClaimComponent.dmoInspectionID + "] not found.");
			}
			if (dMRClaimComponent.dmoInspectionLineID > 0 && !(await base.ERPDMRClaimComponentRepository.DoesRecordExistInTableUsingKeys("InspectionLines", new object[2] { "QALINSPECTIONID", "QALINSPECTIONLINEID" }, new object[2] { dMRClaimComponent.dmoInspectionID, dMRClaimComponent.dmoInspectionLineID })))
			{
				errorsList.Add($"dmoInspectionLineID [{dMRClaimComponent.dmoInspectionLineID}] not found.");
			}
			if (dMRClaimComponent.dmoInspectionComponentID > 0 && !(await base.ERPDMRClaimComponentRepository.DoesRecordExistInTableUsingKeys("InspectionComponents", new object[3] { "qamInspectionID", "qamInspectionLineID", "qamInspectionComponentID" }, new object[3] { dMRClaimComponent.dmoInspectionID, dMRClaimComponent.dmoInspectionLineID, dMRClaimComponent.dmoInspectionComponentID })))
			{
				errorsList.Add($"dmoInspectionComponentID [{dMRClaimComponent.dmoInspectionComponentID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaimComponent.dmoJobID) && !(await base.ERPDMRClaimComponentRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { dMRClaimComponent.dmoJobID })))
			{
				errorsList.Add("dmoJobID [" + dMRClaimComponent.dmoJobID + "] not found.");
			}
			if (dMRClaimComponent.dmoJobAssemblyID > 0 && !(await base.ERPDMRClaimComponentRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { dMRClaimComponent.dmoJobID, dMRClaimComponent.dmoJobAssemblyID })))
			{
				errorsList.Add($"dmoJobAssemblyID [{dMRClaimComponent.dmoJobAssemblyID}] not found.");
			}
			if (dMRClaimComponent.dmoJobMaterialID > 0 && !(await base.ERPDMRClaimComponentRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { dMRClaimComponent.dmoJobID, dMRClaimComponent.dmoJobAssemblyID, dMRClaimComponent.dmoJobMaterialID })))
			{
				errorsList.Add($"dmoJobMaterialID [{dMRClaimComponent.dmoJobMaterialID}] not found.");
			}
			if (dMRClaimComponent.dmoJobMaterialComponentID > 0 && !(await base.ERPDMRClaimComponentRepository.DoesRecordExistInTableUsingKeys("JobMaterialComponents", new object[4] { "JMTJOBID", "JMTJOBASSEMBLYID", "JMTJOBMATERIALID", "JMTJOBMATERIALCOMPONENTID" }, new object[4] { dMRClaimComponent.dmoJobID, dMRClaimComponent.dmoJobAssemblyID, dMRClaimComponent.dmoJobMaterialID, dMRClaimComponent.dmoJobMaterialComponentID })))
			{
				errorsList.Add($"dmoJobMaterialComponentID [{dMRClaimComponent.dmoJobMaterialComponentID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPDMRClaimComponentDto>>> Process_GetAllDMRClaimComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPDMRClaimComponentDto> allDMRClaimComponentsDto = new List<ERPDMRClaimComponentDto>();
		ERPResponseMessageDto<IList<ERPDMRClaimComponentDto>> result;
		try
		{
			IERPDMRClaimComponentRepository iERPDMRClaimComponentRepository = (base.ERPDMRClaimComponentRepository = new ERPDMRClaimComponentRepository(base.ApiClientContext));
			using (iERPDMRClaimComponentRepository)
			{
				foreach (ERPDMRClaimComponentInformationDto item2 in await base.ERPDMRClaimComponentRepository.GetAllDMRClaimComponents(pageSize, pageNumber, filter, orderBy))
				{
					ERPDMRClaimComponentDto item = new ERPDMRClaimComponentDto
					{
						dmoAdditionalQuantity = item2.dmoAdditionalQuantity,
						dmoCreatedBy = item2.dmoCreatedBy,
						dmoCreatedDate = item2.dmoCreatedDate,
						dmoDescription = item2.dmoDescription,
						dmoDmrClaimID = item2.dmoDmrClaimID,
						dmoDmrClaimLineID = item2.dmoDmrClaimLineID,
						dmoUniqueID = item2.dmoUniqueID,
						dmoInspectionComponentID = item2.dmoInspectionComponentID,
						dmoInspectionID = item2.dmoInspectionID,
						dmoInspectionLineID = item2.dmoInspectionLineID,
						dmoShippedComplete = item2.dmoShippedComplete,
						dmoJobAssemblyID = item2.dmoJobAssemblyID,
						dmoJobID = item2.dmoJobID,
						dmoJobMaterialComponentID = item2.dmoJobMaterialComponentID,
						dmoJobMaterialID = item2.dmoJobMaterialID,
						dmoParentQuantity = item2.dmoParentQuantity,
						dmoPartBinID = item2.dmoPartBinID,
						dmoPartID = item2.dmoPartID,
						dmoPartRevisionID = item2.dmoPartRevisionID,
						dmoPartWarehouseLocationID = item2.dmoPartWarehouseLocationID,
						dmoQuantity = item2.dmoQuantity,
						dmoQuantityPerParent = item2.dmoQuantityPerParent,
						dmoQuantityShipped = item2.dmoQuantityShipped,
						dmoRowVersion = item2.dmoRowVersion,
						dmoDmrClaimComponentID = item2.dmoDmrClaimComponentID,
						dmoUnitOfMeasure = item2.dmoUnitOfMeasure,
						dmoWeight = item2.dmoWeight,
						CustomFields = item2.CustomFields
					};
					allDMRClaimComponentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all DMRClaimComponents]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPDMRClaimComponentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allDMRClaimComponentsDto,
				RecordCount = allDMRClaimComponentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPDMRClaimComponentDto>> Process_GetDMRClaimComponent(Guid dMRClaimComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPDMRClaimComponentDto dMRClaimComponentDto = null;
		ERPResponseMessageDto<ERPDMRClaimComponentDto> result;
		try
		{
			IERPDMRClaimComponentRepository iERPDMRClaimComponentRepository = (base.ERPDMRClaimComponentRepository = new ERPDMRClaimComponentRepository(base.ApiClientContext));
			using (iERPDMRClaimComponentRepository)
			{
				ERPDMRClaimComponentInformationDto eRPDMRClaimComponentInformationDto = await base.ERPDMRClaimComponentRepository.GetDMRClaimComponent(dMRClaimComponentId);
				dMRClaimComponentDto = new ERPDMRClaimComponentDto
				{
					dmoAdditionalQuantity = eRPDMRClaimComponentInformationDto.dmoAdditionalQuantity,
					dmoCreatedBy = eRPDMRClaimComponentInformationDto.dmoCreatedBy,
					dmoCreatedDate = eRPDMRClaimComponentInformationDto.dmoCreatedDate,
					dmoDescription = eRPDMRClaimComponentInformationDto.dmoDescription,
					dmoDmrClaimID = eRPDMRClaimComponentInformationDto.dmoDmrClaimID,
					dmoDmrClaimLineID = eRPDMRClaimComponentInformationDto.dmoDmrClaimLineID,
					dmoUniqueID = eRPDMRClaimComponentInformationDto.dmoUniqueID,
					dmoInspectionComponentID = eRPDMRClaimComponentInformationDto.dmoInspectionComponentID,
					dmoInspectionID = eRPDMRClaimComponentInformationDto.dmoInspectionID,
					dmoInspectionLineID = eRPDMRClaimComponentInformationDto.dmoInspectionLineID,
					dmoShippedComplete = eRPDMRClaimComponentInformationDto.dmoShippedComplete,
					dmoJobAssemblyID = eRPDMRClaimComponentInformationDto.dmoJobAssemblyID,
					dmoJobID = eRPDMRClaimComponentInformationDto.dmoJobID,
					dmoJobMaterialComponentID = eRPDMRClaimComponentInformationDto.dmoJobMaterialComponentID,
					dmoJobMaterialID = eRPDMRClaimComponentInformationDto.dmoJobMaterialID,
					dmoParentQuantity = eRPDMRClaimComponentInformationDto.dmoParentQuantity,
					dmoPartBinID = eRPDMRClaimComponentInformationDto.dmoPartBinID,
					dmoPartID = eRPDMRClaimComponentInformationDto.dmoPartID,
					dmoPartRevisionID = eRPDMRClaimComponentInformationDto.dmoPartRevisionID,
					dmoPartWarehouseLocationID = eRPDMRClaimComponentInformationDto.dmoPartWarehouseLocationID,
					dmoQuantity = eRPDMRClaimComponentInformationDto.dmoQuantity,
					dmoQuantityPerParent = eRPDMRClaimComponentInformationDto.dmoQuantityPerParent,
					dmoQuantityShipped = eRPDMRClaimComponentInformationDto.dmoQuantityShipped,
					dmoRowVersion = eRPDMRClaimComponentInformationDto.dmoRowVersion,
					dmoDmrClaimComponentID = eRPDMRClaimComponentInformationDto.dmoDmrClaimComponentID,
					dmoUnitOfMeasure = eRPDMRClaimComponentInformationDto.dmoUnitOfMeasure,
					dmoWeight = eRPDMRClaimComponentInformationDto.dmoWeight,
					CustomFields = eRPDMRClaimComponentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the DMRClaimComponents []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRClaimComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = dMRClaimComponentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPDMRClaimComponentDto>> Process_PutDMRClaimComponent(ERPDMRClaimComponentDto dMRClaimComponent)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPDMRClaimComponentDto createdObject = null;
		ERPResponseMessageDto<ERPDMRClaimComponentDto> result;
		try
		{
			IERPDMRClaimComponentRepository iERPDMRClaimComponentRepository = (base.ERPDMRClaimComponentRepository = new ERPDMRClaimComponentRepository(base.ApiClientContext));
			using (iERPDMRClaimComponentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPDMRClaimComponentRepository.SaveDMRClaimComponent(dMRClaimComponent);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPDMRClaimComponentInformationDto eRPDMRClaimComponentInformationDto = await base.ERPDMRClaimComponentRepository.GetDMRClaimComponent(dMRClaimComponent.dmoUniqueID);
					createdObject = new ERPDMRClaimComponentDto
					{
						dmoAdditionalQuantity = eRPDMRClaimComponentInformationDto.dmoAdditionalQuantity,
						dmoCreatedBy = eRPDMRClaimComponentInformationDto.dmoCreatedBy,
						dmoCreatedDate = eRPDMRClaimComponentInformationDto.dmoCreatedDate,
						dmoDescription = eRPDMRClaimComponentInformationDto.dmoDescription,
						dmoDmrClaimID = eRPDMRClaimComponentInformationDto.dmoDmrClaimID,
						dmoDmrClaimLineID = eRPDMRClaimComponentInformationDto.dmoDmrClaimLineID,
						dmoUniqueID = eRPDMRClaimComponentInformationDto.dmoUniqueID,
						dmoInspectionComponentID = eRPDMRClaimComponentInformationDto.dmoInspectionComponentID,
						dmoInspectionID = eRPDMRClaimComponentInformationDto.dmoInspectionID,
						dmoInspectionLineID = eRPDMRClaimComponentInformationDto.dmoInspectionLineID,
						dmoShippedComplete = eRPDMRClaimComponentInformationDto.dmoShippedComplete,
						dmoJobAssemblyID = eRPDMRClaimComponentInformationDto.dmoJobAssemblyID,
						dmoJobID = eRPDMRClaimComponentInformationDto.dmoJobID,
						dmoJobMaterialComponentID = eRPDMRClaimComponentInformationDto.dmoJobMaterialComponentID,
						dmoJobMaterialID = eRPDMRClaimComponentInformationDto.dmoJobMaterialID,
						dmoParentQuantity = eRPDMRClaimComponentInformationDto.dmoParentQuantity,
						dmoPartBinID = eRPDMRClaimComponentInformationDto.dmoPartBinID,
						dmoPartID = eRPDMRClaimComponentInformationDto.dmoPartID,
						dmoPartRevisionID = eRPDMRClaimComponentInformationDto.dmoPartRevisionID,
						dmoPartWarehouseLocationID = eRPDMRClaimComponentInformationDto.dmoPartWarehouseLocationID,
						dmoQuantity = eRPDMRClaimComponentInformationDto.dmoQuantity,
						dmoQuantityPerParent = eRPDMRClaimComponentInformationDto.dmoQuantityPerParent,
						dmoQuantityShipped = eRPDMRClaimComponentInformationDto.dmoQuantityShipped,
						dmoRowVersion = eRPDMRClaimComponentInformationDto.dmoRowVersion,
						dmoDmrClaimComponentID = eRPDMRClaimComponentInformationDto.dmoDmrClaimComponentID,
						dmoUnitOfMeasure = eRPDMRClaimComponentInformationDto.dmoUnitOfMeasure,
						dmoWeight = eRPDMRClaimComponentInformationDto.dmoWeight,
						CustomFields = eRPDMRClaimComponentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing DMRClaimComponent [{dMRClaimComponent.dmoUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRClaimComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteDMRClaimComponent(Guid dMRClaimComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRClaimComponentRepository iERPDMRClaimComponentRepository = (base.ERPDMRClaimComponentRepository = new ERPDMRClaimComponentRepository(base.ApiClientContext));
		using (iERPDMRClaimComponentRepository)
		{
			if (!(await base.ERPDMRClaimComponentRepository.DoesDMRClaimComponentExist(dMRClaimComponentId)))
			{
				base.ErrorsList.Add($"DMRClaimComponent [{dMRClaimComponentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPDMRClaimComponentInformationDto eRPDMRClaimComponentInformationDto = await base.ERPDMRClaimComponentRepository.GetDMRClaimComponent(dMRClaimComponentId);
				string text = await base.ERPDMRClaimComponentRepository.WhereUsed("DMRClaimComponents", new object[3] { eRPDMRClaimComponentInformationDto.dmoDmrClaimID, eRPDMRClaimComponentInformationDto.dmoDmrClaimLineID, eRPDMRClaimComponentInformationDto.dmoDmrClaimComponentID }, new object[3] { "dmoDmrClaimID", "dmoDmrClaimLineID", "dmoDmrClaimComponentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("DMRClaimComponent cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPDMRClaimComponentDto>> Process_DeleteDMRClaimComponent(Guid dMRClaimComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPDMRClaimComponentDto> result;
		try
		{
			IERPDMRClaimComponentRepository iERPDMRClaimComponentRepository = (base.ERPDMRClaimComponentRepository = new ERPDMRClaimComponentRepository(base.ApiClientContext));
			using (iERPDMRClaimComponentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPDMRClaimComponentRepository.DeleteRowFromTable("DMRClaimComponents", "dmo", dMRClaimComponentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of DMRClaimComponent [{dMRClaimComponentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRClaimComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPDMRClaimComponentDto()
			};
		}
		return result;
	}
}
