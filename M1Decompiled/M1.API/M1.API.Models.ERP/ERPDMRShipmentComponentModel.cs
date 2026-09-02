using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPDMRShipmentComponentModel : ERPBaseModel, IERPDMRShipmentComponentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllDMRShipmentComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPDMRShipmentComponentRepository iERPDMRShipmentComponentRepository = (base.ERPDMRShipmentComponentRepository = new ERPDMRShipmentComponentRepository(base.ApiClientContext));
		using (iERPDMRShipmentComponentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPDMRShipmentComponentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPDMRShipmentComponentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPDMRShipmentComponentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPDMRShipmentComponentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetDMRShipmentComponent(Guid dMRShipmentComponentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRShipmentComponentRepository iERPDMRShipmentComponentRepository = (base.ERPDMRShipmentComponentRepository = new ERPDMRShipmentComponentRepository(base.ApiClientContext));
		using (iERPDMRShipmentComponentRepository)
		{
			if (!(await base.ERPDMRShipmentComponentRepository.DoesDMRShipmentComponentExist(dMRShipmentComponentId)))
			{
				errorsList.Add($"DMRShipmentComponent [{dMRShipmentComponentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutDMRShipmentComponent(ERPDMRShipmentComponentDto dMRShipmentComponent)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRShipmentComponentRepository iERPDMRShipmentComponentRepository = (base.ERPDMRShipmentComponentRepository = new ERPDMRShipmentComponentRepository(base.ApiClientContext));
		using (iERPDMRShipmentComponentRepository)
		{
			if (!string.IsNullOrWhiteSpace(dMRShipmentComponent.dsoDmrShipmentID) && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("DMRShipments", new object[1] { "DSPDMRSHIPMENTID" }, new object[1] { dMRShipmentComponent.dsoDmrShipmentID })))
			{
				errorsList.Add("dsoDmrShipmentID [" + dMRShipmentComponent.dsoDmrShipmentID + "] not found.");
			}
			if (dMRShipmentComponent.dsoDmrShipmentLineID > 0 && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("DMRShipmentLines", new object[2] { "DSLDMRSHIPMENTID", "DSLDMRSHIPMENTLINEID" }, new object[2] { dMRShipmentComponent.dsoDmrShipmentID, dMRShipmentComponent.dsoDmrShipmentLineID })))
			{
				errorsList.Add($"dsoDmrShipmentLineID [{dMRShipmentComponent.dsoDmrShipmentLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentComponent.dsoPartID) && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { dMRShipmentComponent.dsoPartID })))
			{
				errorsList.Add("dsoPartID [" + dMRShipmentComponent.dsoPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentComponent.dsoPartRevisionID) && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { dMRShipmentComponent.dsoPartID, dMRShipmentComponent.dsoPartRevisionID })))
			{
				errorsList.Add("dsoPartRevisionID [" + dMRShipmentComponent.dsoPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentComponent.dsoPartWarehouseLocationID) && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { dMRShipmentComponent.dsoPartID, dMRShipmentComponent.dsoPartRevisionID, dMRShipmentComponent.dsoPartWarehouseLocationID })))
			{
				errorsList.Add("dsoPartWarehouseLocationID [" + dMRShipmentComponent.dsoPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentComponent.dsoPartBinID) && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { dMRShipmentComponent.dsoPartID, dMRShipmentComponent.dsoPartRevisionID, dMRShipmentComponent.dsoPartWarehouseLocationID, dMRShipmentComponent.dsoPartBinID })))
			{
				errorsList.Add("dsoPartBinID [" + dMRShipmentComponent.dsoPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentComponent.dsoDmrClaimID) && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("DMRClaims", new object[1] { "DMPDMRCLAIMID" }, new object[1] { dMRShipmentComponent.dsoDmrClaimID })))
			{
				errorsList.Add("dsoDmrClaimID [" + dMRShipmentComponent.dsoDmrClaimID + "] not found.");
			}
			if (dMRShipmentComponent.dsoDmrClaimLineID > 0 && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("DMRClaimLines", new object[2] { "DMLDMRCLAIMID", "DMLDMRCLAIMLINEID" }, new object[2] { dMRShipmentComponent.dsoDmrClaimID, dMRShipmentComponent.dsoDmrClaimLineID })))
			{
				errorsList.Add($"dsoDmrClaimLineID [{dMRShipmentComponent.dsoDmrClaimLineID}] not found.");
			}
			if (dMRShipmentComponent.dsoDmrClaimComponentID > 0 && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("DMRClaimComponents", new object[3] { "dmoDMRClaimID", "dmoDMRClaimLineID", "dmoDMRClaimComponentID" }, new object[3] { dMRShipmentComponent.dsoDmrClaimID, dMRShipmentComponent.dsoDmrClaimLineID, dMRShipmentComponent.dsoDmrClaimComponentID })))
			{
				errorsList.Add($"dsoDmrClaimComponentID [{dMRShipmentComponent.dsoDmrClaimComponentID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentComponent.dsoJobID) && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { dMRShipmentComponent.dsoJobID })))
			{
				errorsList.Add("dsoJobID [" + dMRShipmentComponent.dsoJobID + "] not found.");
			}
			if (dMRShipmentComponent.dsoJobAssemblyID > 0 && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { dMRShipmentComponent.dsoJobID, dMRShipmentComponent.dsoJobAssemblyID })))
			{
				errorsList.Add($"dsoJobAssemblyID [{dMRShipmentComponent.dsoJobAssemblyID}] not found.");
			}
			if (dMRShipmentComponent.dsoJobMaterialID > 0 && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { dMRShipmentComponent.dsoJobID, dMRShipmentComponent.dsoJobAssemblyID, dMRShipmentComponent.dsoJobMaterialID })))
			{
				errorsList.Add($"dsoJobMaterialID [{dMRShipmentComponent.dsoJobMaterialID}] not found.");
			}
			if (dMRShipmentComponent.dsoJobMaterialComponentID > 0 && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("JobMaterialComponents", new object[4] { "JMTJOBID", "JMTJOBASSEMBLYID", "JMTJOBMATERIALID", "JMTJOBMATERIALCOMPONENTID" }, new object[4] { dMRShipmentComponent.dsoJobID, dMRShipmentComponent.dsoJobAssemblyID, dMRShipmentComponent.dsoJobMaterialID, dMRShipmentComponent.dsoJobMaterialComponentID })))
			{
				errorsList.Add($"dsoJobMaterialComponentID [{dMRShipmentComponent.dsoJobMaterialComponentID}] not found.");
			}
			if (dMRShipmentComponent.dsoInspectionLineID > 0 && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("InspectionLines", new object[2] { "QALINSPECTIONID", "QALINSPECTIONLINEID" }, new object[2] { dMRShipmentComponent.dsoInspectionID, dMRShipmentComponent.dsoInspectionLineID })))
			{
				errorsList.Add($"dsoInspectionLineID [{dMRShipmentComponent.dsoInspectionLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentComponent.dsoInspectionID) && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("Inspections", new object[1] { "QAPINSPECTIONID" }, new object[1] { dMRShipmentComponent.dsoInspectionID })))
			{
				errorsList.Add("dsoInspectionID [" + dMRShipmentComponent.dsoInspectionID + "] not found.");
			}
			if (dMRShipmentComponent.dsoInspectionComponentID > 0 && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("InspectionComponents", new object[3] { "qamInspectionID", "qamInspectionLineID", "qamInspectionComponentID" }, new object[3] { dMRShipmentComponent.dsoInspectionID, dMRShipmentComponent.dsoInspectionLineID, dMRShipmentComponent.dsoInspectionComponentID })))
			{
				errorsList.Add($"dsoInspectionComponentID [{dMRShipmentComponent.dsoInspectionComponentID}] not found.");
			}
			if (dMRShipmentComponent.dsoReverseDmrShipmentLineID > 0 && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("DMRShipmentLines", new object[2] { "DSLDMRSHIPMENTID", "DSLDMRSHIPMENTLINEID" }, new object[2] { dMRShipmentComponent.dsoReverseDmrShipmentID, dMRShipmentComponent.dsoReverseDmrShipmentLineID })))
			{
				errorsList.Add($"dsoReverseDmrShipmentLineID [{dMRShipmentComponent.dsoReverseDmrShipmentLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipmentComponent.dsoReverseDmrShipmentID) && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("DMRShipments", new object[1] { "DSPDMRSHIPMENTID" }, new object[1] { dMRShipmentComponent.dsoReverseDmrShipmentID })))
			{
				errorsList.Add("dsoReverseDmrShipmentID [" + dMRShipmentComponent.dsoReverseDmrShipmentID + "] not found.");
			}
			if (dMRShipmentComponent.dsoReverseDmrShipmentCompID > 0 && !(await base.ERPDMRShipmentComponentRepository.DoesRecordExistInTableUsingKeys("DMRShipmentComponents", new object[3] { "dsoDMRShipmentID", "dsoDMRShipmentLineID", "dsoDMRShipmentComponentID" }, new object[3] { dMRShipmentComponent.dsoReverseDmrShipmentID, dMRShipmentComponent.dsoReverseDmrShipmentLineID, dMRShipmentComponent.dsoReverseDmrShipmentCompID })))
			{
				errorsList.Add($"dsoReverseDmrShipmentCompID [{dMRShipmentComponent.dsoReverseDmrShipmentCompID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPDMRShipmentComponentDto>>> Process_GetAllDMRShipmentComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPDMRShipmentComponentDto> allDMRShipmentComponentsDto = new List<ERPDMRShipmentComponentDto>();
		ERPResponseMessageDto<IList<ERPDMRShipmentComponentDto>> result;
		try
		{
			IERPDMRShipmentComponentRepository iERPDMRShipmentComponentRepository = (base.ERPDMRShipmentComponentRepository = new ERPDMRShipmentComponentRepository(base.ApiClientContext));
			using (iERPDMRShipmentComponentRepository)
			{
				foreach (ERPDMRShipmentComponentInformationDto item2 in await base.ERPDMRShipmentComponentRepository.GetAllDMRShipmentComponents(pageSize, pageNumber, filter, orderBy))
				{
					ERPDMRShipmentComponentDto item = new ERPDMRShipmentComponentDto
					{
						dsoAdditionalQuantity = item2.dsoAdditionalQuantity,
						dsoCreatedBy = item2.dsoCreatedBy,
						dsoCreatedDate = item2.dsoCreatedDate,
						dsoDescription = item2.dsoDescription,
						dsoDmrClaimComponentID = item2.dsoDmrClaimComponentID,
						dsoDmrClaimID = item2.dsoDmrClaimID,
						dsoDmrClaimLineID = item2.dsoDmrClaimLineID,
						dsoDmrShipmentID = item2.dsoDmrShipmentID,
						dsoDmrShipmentLineID = item2.dsoDmrShipmentLineID,
						dsoUniqueID = item2.dsoUniqueID,
						dsoInspectionComponentID = item2.dsoInspectionComponentID,
						dsoInspectionID = item2.dsoInspectionID,
						dsoInspectionLineID = item2.dsoInspectionLineID,
						dsoInvParentQuantity = item2.dsoInvParentQuantity,
						dsoInvQuantityShipped = item2.dsoInvQuantityShipped,
						dsoClosed = item2.dsoClosed,
						dsoPosted = item2.dsoPosted,
						dsoReversed = item2.dsoReversed,
						dsoShippedComplete = item2.dsoShippedComplete,
						dsoJobAssemblyID = item2.dsoJobAssemblyID,
						dsoJobID = item2.dsoJobID,
						dsoJobMaterialComponentID = item2.dsoJobMaterialComponentID,
						dsoJobMaterialID = item2.dsoJobMaterialID,
						dsoJobMatParentQuantity = item2.dsoJobMatParentQuantity,
						dsoJobMatQuantityShipped = item2.dsoJobMatQuantityShipped,
						dsoPartBinID = item2.dsoPartBinID,
						dsoPartID = item2.dsoPartID,
						dsoPartRevisionID = item2.dsoPartRevisionID,
						dsoPartWarehouseLocationID = item2.dsoPartWarehouseLocationID,
						dsoQuantityPerParent = item2.dsoQuantityPerParent,
						dsoReturnParentQuantity = item2.dsoReturnParentQuantity,
						dsoReturnQuantityShipped = item2.dsoReturnQuantityShipped,
						dsoReverseDmrShipmentCompID = item2.dsoReverseDmrShipmentCompID,
						dsoReverseDmrShipmentID = item2.dsoReverseDmrShipmentID,
						dsoReverseDmrShipmentLineID = item2.dsoReverseDmrShipmentLineID,
						dsoRowVersion = item2.dsoRowVersion,
						dsoDmrShipmentComponentID = item2.dsoDmrShipmentComponentID,
						dsoUnitOfMeasure = item2.dsoUnitOfMeasure,
						dsoWeight = item2.dsoWeight,
						CustomFields = item2.CustomFields
					};
					allDMRShipmentComponentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all DMRShipmentComponents]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPDMRShipmentComponentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allDMRShipmentComponentsDto,
				RecordCount = allDMRShipmentComponentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPDMRShipmentComponentDto>> Process_GetDMRShipmentComponent(Guid dMRShipmentComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPDMRShipmentComponentDto dMRShipmentComponentDto = null;
		ERPResponseMessageDto<ERPDMRShipmentComponentDto> result;
		try
		{
			IERPDMRShipmentComponentRepository iERPDMRShipmentComponentRepository = (base.ERPDMRShipmentComponentRepository = new ERPDMRShipmentComponentRepository(base.ApiClientContext));
			using (iERPDMRShipmentComponentRepository)
			{
				ERPDMRShipmentComponentInformationDto eRPDMRShipmentComponentInformationDto = await base.ERPDMRShipmentComponentRepository.GetDMRShipmentComponent(dMRShipmentComponentId);
				dMRShipmentComponentDto = new ERPDMRShipmentComponentDto
				{
					dsoAdditionalQuantity = eRPDMRShipmentComponentInformationDto.dsoAdditionalQuantity,
					dsoCreatedBy = eRPDMRShipmentComponentInformationDto.dsoCreatedBy,
					dsoCreatedDate = eRPDMRShipmentComponentInformationDto.dsoCreatedDate,
					dsoDescription = eRPDMRShipmentComponentInformationDto.dsoDescription,
					dsoDmrClaimComponentID = eRPDMRShipmentComponentInformationDto.dsoDmrClaimComponentID,
					dsoDmrClaimID = eRPDMRShipmentComponentInformationDto.dsoDmrClaimID,
					dsoDmrClaimLineID = eRPDMRShipmentComponentInformationDto.dsoDmrClaimLineID,
					dsoDmrShipmentID = eRPDMRShipmentComponentInformationDto.dsoDmrShipmentID,
					dsoDmrShipmentLineID = eRPDMRShipmentComponentInformationDto.dsoDmrShipmentLineID,
					dsoUniqueID = eRPDMRShipmentComponentInformationDto.dsoUniqueID,
					dsoInspectionComponentID = eRPDMRShipmentComponentInformationDto.dsoInspectionComponentID,
					dsoInspectionID = eRPDMRShipmentComponentInformationDto.dsoInspectionID,
					dsoInspectionLineID = eRPDMRShipmentComponentInformationDto.dsoInspectionLineID,
					dsoInvParentQuantity = eRPDMRShipmentComponentInformationDto.dsoInvParentQuantity,
					dsoInvQuantityShipped = eRPDMRShipmentComponentInformationDto.dsoInvQuantityShipped,
					dsoClosed = eRPDMRShipmentComponentInformationDto.dsoClosed,
					dsoPosted = eRPDMRShipmentComponentInformationDto.dsoPosted,
					dsoReversed = eRPDMRShipmentComponentInformationDto.dsoReversed,
					dsoShippedComplete = eRPDMRShipmentComponentInformationDto.dsoShippedComplete,
					dsoJobAssemblyID = eRPDMRShipmentComponentInformationDto.dsoJobAssemblyID,
					dsoJobID = eRPDMRShipmentComponentInformationDto.dsoJobID,
					dsoJobMaterialComponentID = eRPDMRShipmentComponentInformationDto.dsoJobMaterialComponentID,
					dsoJobMaterialID = eRPDMRShipmentComponentInformationDto.dsoJobMaterialID,
					dsoJobMatParentQuantity = eRPDMRShipmentComponentInformationDto.dsoJobMatParentQuantity,
					dsoJobMatQuantityShipped = eRPDMRShipmentComponentInformationDto.dsoJobMatQuantityShipped,
					dsoPartBinID = eRPDMRShipmentComponentInformationDto.dsoPartBinID,
					dsoPartID = eRPDMRShipmentComponentInformationDto.dsoPartID,
					dsoPartRevisionID = eRPDMRShipmentComponentInformationDto.dsoPartRevisionID,
					dsoPartWarehouseLocationID = eRPDMRShipmentComponentInformationDto.dsoPartWarehouseLocationID,
					dsoQuantityPerParent = eRPDMRShipmentComponentInformationDto.dsoQuantityPerParent,
					dsoReturnParentQuantity = eRPDMRShipmentComponentInformationDto.dsoReturnParentQuantity,
					dsoReturnQuantityShipped = eRPDMRShipmentComponentInformationDto.dsoReturnQuantityShipped,
					dsoReverseDmrShipmentCompID = eRPDMRShipmentComponentInformationDto.dsoReverseDmrShipmentCompID,
					dsoReverseDmrShipmentID = eRPDMRShipmentComponentInformationDto.dsoReverseDmrShipmentID,
					dsoReverseDmrShipmentLineID = eRPDMRShipmentComponentInformationDto.dsoReverseDmrShipmentLineID,
					dsoRowVersion = eRPDMRShipmentComponentInformationDto.dsoRowVersion,
					dsoDmrShipmentComponentID = eRPDMRShipmentComponentInformationDto.dsoDmrShipmentComponentID,
					dsoUnitOfMeasure = eRPDMRShipmentComponentInformationDto.dsoUnitOfMeasure,
					dsoWeight = eRPDMRShipmentComponentInformationDto.dsoWeight,
					CustomFields = eRPDMRShipmentComponentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the DMRShipmentComponents []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRShipmentComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = dMRShipmentComponentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPDMRShipmentComponentDto>> Process_PutDMRShipmentComponent(ERPDMRShipmentComponentDto dMRShipmentComponent)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPDMRShipmentComponentDto createdObject = null;
		ERPResponseMessageDto<ERPDMRShipmentComponentDto> result;
		try
		{
			IERPDMRShipmentComponentRepository iERPDMRShipmentComponentRepository = (base.ERPDMRShipmentComponentRepository = new ERPDMRShipmentComponentRepository(base.ApiClientContext));
			using (iERPDMRShipmentComponentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPDMRShipmentComponentRepository.SaveDMRShipmentComponent(dMRShipmentComponent);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPDMRShipmentComponentInformationDto eRPDMRShipmentComponentInformationDto = await base.ERPDMRShipmentComponentRepository.GetDMRShipmentComponent(dMRShipmentComponent.dsoUniqueID);
					createdObject = new ERPDMRShipmentComponentDto
					{
						dsoAdditionalQuantity = eRPDMRShipmentComponentInformationDto.dsoAdditionalQuantity,
						dsoCreatedBy = eRPDMRShipmentComponentInformationDto.dsoCreatedBy,
						dsoCreatedDate = eRPDMRShipmentComponentInformationDto.dsoCreatedDate,
						dsoDescription = eRPDMRShipmentComponentInformationDto.dsoDescription,
						dsoDmrClaimComponentID = eRPDMRShipmentComponentInformationDto.dsoDmrClaimComponentID,
						dsoDmrClaimID = eRPDMRShipmentComponentInformationDto.dsoDmrClaimID,
						dsoDmrClaimLineID = eRPDMRShipmentComponentInformationDto.dsoDmrClaimLineID,
						dsoDmrShipmentID = eRPDMRShipmentComponentInformationDto.dsoDmrShipmentID,
						dsoDmrShipmentLineID = eRPDMRShipmentComponentInformationDto.dsoDmrShipmentLineID,
						dsoUniqueID = eRPDMRShipmentComponentInformationDto.dsoUniqueID,
						dsoInspectionComponentID = eRPDMRShipmentComponentInformationDto.dsoInspectionComponentID,
						dsoInspectionID = eRPDMRShipmentComponentInformationDto.dsoInspectionID,
						dsoInspectionLineID = eRPDMRShipmentComponentInformationDto.dsoInspectionLineID,
						dsoInvParentQuantity = eRPDMRShipmentComponentInformationDto.dsoInvParentQuantity,
						dsoInvQuantityShipped = eRPDMRShipmentComponentInformationDto.dsoInvQuantityShipped,
						dsoClosed = eRPDMRShipmentComponentInformationDto.dsoClosed,
						dsoPosted = eRPDMRShipmentComponentInformationDto.dsoPosted,
						dsoReversed = eRPDMRShipmentComponentInformationDto.dsoReversed,
						dsoShippedComplete = eRPDMRShipmentComponentInformationDto.dsoShippedComplete,
						dsoJobAssemblyID = eRPDMRShipmentComponentInformationDto.dsoJobAssemblyID,
						dsoJobID = eRPDMRShipmentComponentInformationDto.dsoJobID,
						dsoJobMaterialComponentID = eRPDMRShipmentComponentInformationDto.dsoJobMaterialComponentID,
						dsoJobMaterialID = eRPDMRShipmentComponentInformationDto.dsoJobMaterialID,
						dsoJobMatParentQuantity = eRPDMRShipmentComponentInformationDto.dsoJobMatParentQuantity,
						dsoJobMatQuantityShipped = eRPDMRShipmentComponentInformationDto.dsoJobMatQuantityShipped,
						dsoPartBinID = eRPDMRShipmentComponentInformationDto.dsoPartBinID,
						dsoPartID = eRPDMRShipmentComponentInformationDto.dsoPartID,
						dsoPartRevisionID = eRPDMRShipmentComponentInformationDto.dsoPartRevisionID,
						dsoPartWarehouseLocationID = eRPDMRShipmentComponentInformationDto.dsoPartWarehouseLocationID,
						dsoQuantityPerParent = eRPDMRShipmentComponentInformationDto.dsoQuantityPerParent,
						dsoReturnParentQuantity = eRPDMRShipmentComponentInformationDto.dsoReturnParentQuantity,
						dsoReturnQuantityShipped = eRPDMRShipmentComponentInformationDto.dsoReturnQuantityShipped,
						dsoReverseDmrShipmentCompID = eRPDMRShipmentComponentInformationDto.dsoReverseDmrShipmentCompID,
						dsoReverseDmrShipmentID = eRPDMRShipmentComponentInformationDto.dsoReverseDmrShipmentID,
						dsoReverseDmrShipmentLineID = eRPDMRShipmentComponentInformationDto.dsoReverseDmrShipmentLineID,
						dsoRowVersion = eRPDMRShipmentComponentInformationDto.dsoRowVersion,
						dsoDmrShipmentComponentID = eRPDMRShipmentComponentInformationDto.dsoDmrShipmentComponentID,
						dsoUnitOfMeasure = eRPDMRShipmentComponentInformationDto.dsoUnitOfMeasure,
						dsoWeight = eRPDMRShipmentComponentInformationDto.dsoWeight,
						CustomFields = eRPDMRShipmentComponentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing DMRShipmentComponent [{dMRShipmentComponent.dsoUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRShipmentComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteDMRShipmentComponent(Guid dMRShipmentComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRShipmentComponentRepository iERPDMRShipmentComponentRepository = (base.ERPDMRShipmentComponentRepository = new ERPDMRShipmentComponentRepository(base.ApiClientContext));
		using (iERPDMRShipmentComponentRepository)
		{
			if (!(await base.ERPDMRShipmentComponentRepository.DoesDMRShipmentComponentExist(dMRShipmentComponentId)))
			{
				base.ErrorsList.Add($"DMRShipmentComponent [{dMRShipmentComponentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPDMRShipmentComponentInformationDto eRPDMRShipmentComponentInformationDto = await base.ERPDMRShipmentComponentRepository.GetDMRShipmentComponent(dMRShipmentComponentId);
				string text = await base.ERPDMRShipmentComponentRepository.WhereUsed("DMRShipmentComponents", new object[3] { eRPDMRShipmentComponentInformationDto.dsoDmrShipmentID, eRPDMRShipmentComponentInformationDto.dsoDmrShipmentLineID, eRPDMRShipmentComponentInformationDto.dsoDmrShipmentComponentID }, new object[3] { "dsoDmrShipmentID", "dsoDmrShipmentLineID", "dsoDmrShipmentComponentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("DMRShipmentComponent cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPDMRShipmentComponentDto>> Process_DeleteDMRShipmentComponent(Guid dMRShipmentComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPDMRShipmentComponentDto> result;
		try
		{
			IERPDMRShipmentComponentRepository iERPDMRShipmentComponentRepository = (base.ERPDMRShipmentComponentRepository = new ERPDMRShipmentComponentRepository(base.ApiClientContext));
			using (iERPDMRShipmentComponentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPDMRShipmentComponentRepository.DeleteRowFromTable("DMRShipmentComponents", "dso", dMRShipmentComponentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of DMRShipmentComponent [{dMRShipmentComponentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRShipmentComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPDMRShipmentComponentDto()
			};
		}
		return result;
	}
}
