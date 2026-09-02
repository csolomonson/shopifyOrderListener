using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartMaterialModel : ERPBaseModel, IERPPartMaterialModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartMaterials(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartMaterialRepository iERPPartMaterialRepository = (base.ERPPartMaterialRepository = new ERPPartMaterialRepository(base.ApiClientContext));
		using (iERPPartMaterialRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartMaterialRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartMaterialRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartMaterialRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartMaterialRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartMaterial(Guid partMaterialId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartMaterialRepository iERPPartMaterialRepository = (base.ERPPartMaterialRepository = new ERPPartMaterialRepository(base.ApiClientContext));
		using (iERPPartMaterialRepository)
		{
			if (!(await base.ERPPartMaterialRepository.DoesPartMaterialExist(partMaterialId)))
			{
				errorsList.Add($"PartMaterial [{partMaterialId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartMaterial(ERPPartMaterialDto partMaterial)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartMaterialRepository iERPPartMaterialRepository = (base.ERPPartMaterialRepository = new ERPPartMaterialRepository(base.ApiClientContext));
		using (iERPPartMaterialRepository)
		{
			if (!string.IsNullOrWhiteSpace(partMaterial.immMethodID) && !(await base.ERPPartMaterialRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partMaterial.immMethodID })))
			{
				errorsList.Add("immMethodID [" + partMaterial.immMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partMaterial.immMethodRevisionID) && !(await base.ERPPartMaterialRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partMaterial.immMethodID, partMaterial.immMethodRevisionID })))
			{
				errorsList.Add("immMethodRevisionID [" + partMaterial.immMethodRevisionID + "] not found.");
			}
			if (partMaterial.immMethodAssemblyID > 0 && !(await base.ERPPartMaterialRepository.DoesRecordExistInTableUsingKeys("PartAssemblies", new object[3] { "IMAMETHODID", "IMAMETHODREVISIONID", "IMAMETHODASSEMBLYID" }, new object[3] { partMaterial.immMethodID, partMaterial.immMethodRevisionID, partMaterial.immMethodAssemblyID })))
			{
				errorsList.Add($"immMethodAssemblyID [{partMaterial.immMethodAssemblyID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partMaterial.immPartID) && !(await base.ERPPartMaterialRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partMaterial.immPartID })))
			{
				errorsList.Add("immPartID [" + partMaterial.immPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partMaterial.immPartRevisionID) && !(await base.ERPPartMaterialRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partMaterial.immPartID, partMaterial.immPartRevisionID })))
			{
				errorsList.Add("immPartRevisionID [" + partMaterial.immPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partMaterial.immPartWarehouseLocationID) && !(await base.ERPPartMaterialRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { partMaterial.immPartID, partMaterial.immPartRevisionID, partMaterial.immPartWarehouseLocationID })))
			{
				errorsList.Add("immPartWarehouseLocationID [" + partMaterial.immPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partMaterial.immPartBinID) && !(await base.ERPPartMaterialRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { partMaterial.immPartID, partMaterial.immPartRevisionID, partMaterial.immPartWarehouseLocationID, partMaterial.immPartBinID })))
			{
				errorsList.Add("immPartBinID [" + partMaterial.immPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partMaterial.immSupplierOrganizationID) && !(await base.ERPPartMaterialRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { partMaterial.immSupplierOrganizationID })))
			{
				errorsList.Add("immSupplierOrganizationID [" + partMaterial.immSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partMaterial.immPurchaseLocationID) && !(await base.ERPPartMaterialRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { partMaterial.immSupplierOrganizationID, partMaterial.immPurchaseLocationID })))
			{
				errorsList.Add("immPurchaseLocationID [" + partMaterial.immPurchaseLocationID + "] not found.");
			}
			if (partMaterial.immRelatedPartOperationID > 0 && !(await base.ERPPartMaterialRepository.DoesRecordExistInTableUsingKeys("PartOperations", new object[4] { "IMOMETHODID", "IMOMETHODREVISIONID", "IMOMETHODASSEMBLYID", "IMOMETHODOPERATIONID" }, new object[4] { partMaterial.immMethodID, partMaterial.immMethodRevisionID, partMaterial.immMethodAssemblyID, partMaterial.immRelatedPartOperationID })))
			{
				errorsList.Add($"immRelatedPartOperationID [{partMaterial.immRelatedPartOperationID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartMaterialDto>>> Process_GetAllPartMaterials(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartMaterialDto> allPartMaterialsDto = new List<ERPPartMaterialDto>();
		ERPResponseMessageDto<IList<ERPPartMaterialDto>> result;
		try
		{
			IERPPartMaterialRepository iERPPartMaterialRepository = (base.ERPPartMaterialRepository = new ERPPartMaterialRepository(base.ApiClientContext));
			using (iERPPartMaterialRepository)
			{
				foreach (ERPPartMaterialInformationDto item2 in await base.ERPPartMaterialRepository.GetAllPartMaterials(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartMaterialDto item = new ERPPartMaterialDto
					{
						immCreatedBy = item2.immCreatedBy,
						immCreatedDate = item2.immCreatedDate,
						immDocuments = item2.immDocuments,
						immUniqueID = item2.immUniqueID,
						immEstimatedUnitCost = item2.immEstimatedUnitCost,
						immBackflush = item2.immBackflush,
						immManualPart = item2.immManualPart,
						immUseDefaultWarehouseAndBin = item2.immUseDefaultWarehouseAndBin,
						immLeadTime = item2.immLeadTime,
						immMethodAssemblyID = item2.immMethodAssemblyID,
						immMethodID = item2.immMethodID,
						immMethodMaterialID = item2.immMethodMaterialID,
						immMethodRevisionID = item2.immMethodRevisionID,
						immMinimumCharge = item2.immMinimumCharge,
						immPartBinID = item2.immPartBinID,
						immPartID = item2.immPartID,
						immPartLongDescriptionRtf = item2.immPartLongDescriptionRtf,
						immPartLongDescriptionText = item2.immPartLongDescriptionText,
						immPartRevisionID = item2.immPartRevisionID,
						immPartShortDescription = item2.immPartShortDescription,
						immPartWarehouseLocationID = item2.immPartWarehouseLocationID,
						immPurchaseLocationID = item2.immPurchaseLocationID,
						immQuantityPerAssembly = item2.immQuantityPerAssembly,
						immRelatedPartOperationID = item2.immRelatedPartOperationID,
						immRowVersion = item2.immRowVersion,
						immScrapPercent = item2.immScrapPercent,
						immScrapQuantity = item2.immScrapQuantity,
						immSupplierOrganizationID = item2.immSupplierOrganizationID,
						immUnitOfMeasure = item2.immUnitOfMeasure,
						CustomFields = item2.CustomFields
					};
					allPartMaterialsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartMaterials]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartMaterialDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartMaterialsDto,
				RecordCount = allPartMaterialsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartMaterialDto>> Process_GetPartMaterial(Guid partMaterialId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartMaterialDto partMaterialDto = null;
		ERPResponseMessageDto<ERPPartMaterialDto> result;
		try
		{
			IERPPartMaterialRepository iERPPartMaterialRepository = (base.ERPPartMaterialRepository = new ERPPartMaterialRepository(base.ApiClientContext));
			using (iERPPartMaterialRepository)
			{
				ERPPartMaterialInformationDto eRPPartMaterialInformationDto = await base.ERPPartMaterialRepository.GetPartMaterial(partMaterialId);
				partMaterialDto = new ERPPartMaterialDto
				{
					immCreatedBy = eRPPartMaterialInformationDto.immCreatedBy,
					immCreatedDate = eRPPartMaterialInformationDto.immCreatedDate,
					immDocuments = eRPPartMaterialInformationDto.immDocuments,
					immUniqueID = eRPPartMaterialInformationDto.immUniqueID,
					immEstimatedUnitCost = eRPPartMaterialInformationDto.immEstimatedUnitCost,
					immBackflush = eRPPartMaterialInformationDto.immBackflush,
					immManualPart = eRPPartMaterialInformationDto.immManualPart,
					immUseDefaultWarehouseAndBin = eRPPartMaterialInformationDto.immUseDefaultWarehouseAndBin,
					immLeadTime = eRPPartMaterialInformationDto.immLeadTime,
					immMethodAssemblyID = eRPPartMaterialInformationDto.immMethodAssemblyID,
					immMethodID = eRPPartMaterialInformationDto.immMethodID,
					immMethodMaterialID = eRPPartMaterialInformationDto.immMethodMaterialID,
					immMethodRevisionID = eRPPartMaterialInformationDto.immMethodRevisionID,
					immMinimumCharge = eRPPartMaterialInformationDto.immMinimumCharge,
					immPartBinID = eRPPartMaterialInformationDto.immPartBinID,
					immPartID = eRPPartMaterialInformationDto.immPartID,
					immPartLongDescriptionRtf = eRPPartMaterialInformationDto.immPartLongDescriptionRtf,
					immPartLongDescriptionText = eRPPartMaterialInformationDto.immPartLongDescriptionText,
					immPartRevisionID = eRPPartMaterialInformationDto.immPartRevisionID,
					immPartShortDescription = eRPPartMaterialInformationDto.immPartShortDescription,
					immPartWarehouseLocationID = eRPPartMaterialInformationDto.immPartWarehouseLocationID,
					immPurchaseLocationID = eRPPartMaterialInformationDto.immPurchaseLocationID,
					immQuantityPerAssembly = eRPPartMaterialInformationDto.immQuantityPerAssembly,
					immRelatedPartOperationID = eRPPartMaterialInformationDto.immRelatedPartOperationID,
					immRowVersion = eRPPartMaterialInformationDto.immRowVersion,
					immScrapPercent = eRPPartMaterialInformationDto.immScrapPercent,
					immScrapQuantity = eRPPartMaterialInformationDto.immScrapQuantity,
					immSupplierOrganizationID = eRPPartMaterialInformationDto.immSupplierOrganizationID,
					immUnitOfMeasure = eRPPartMaterialInformationDto.immUnitOfMeasure,
					CustomFields = eRPPartMaterialInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartMaterials []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartMaterialDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partMaterialDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartMaterialDto>> Process_PutPartMaterial(ERPPartMaterialDto partMaterial)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartMaterialDto createdObject = null;
		ERPResponseMessageDto<ERPPartMaterialDto> result;
		try
		{
			IERPPartMaterialRepository iERPPartMaterialRepository = (base.ERPPartMaterialRepository = new ERPPartMaterialRepository(base.ApiClientContext));
			using (iERPPartMaterialRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartMaterialRepository.SavePartMaterial(partMaterial);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartMaterialInformationDto eRPPartMaterialInformationDto = await base.ERPPartMaterialRepository.GetPartMaterial(partMaterial.immUniqueID);
					createdObject = new ERPPartMaterialDto
					{
						immCreatedBy = eRPPartMaterialInformationDto.immCreatedBy,
						immCreatedDate = eRPPartMaterialInformationDto.immCreatedDate,
						immDocuments = eRPPartMaterialInformationDto.immDocuments,
						immUniqueID = eRPPartMaterialInformationDto.immUniqueID,
						immEstimatedUnitCost = eRPPartMaterialInformationDto.immEstimatedUnitCost,
						immBackflush = eRPPartMaterialInformationDto.immBackflush,
						immManualPart = eRPPartMaterialInformationDto.immManualPart,
						immUseDefaultWarehouseAndBin = eRPPartMaterialInformationDto.immUseDefaultWarehouseAndBin,
						immLeadTime = eRPPartMaterialInformationDto.immLeadTime,
						immMethodAssemblyID = eRPPartMaterialInformationDto.immMethodAssemblyID,
						immMethodID = eRPPartMaterialInformationDto.immMethodID,
						immMethodMaterialID = eRPPartMaterialInformationDto.immMethodMaterialID,
						immMethodRevisionID = eRPPartMaterialInformationDto.immMethodRevisionID,
						immMinimumCharge = eRPPartMaterialInformationDto.immMinimumCharge,
						immPartBinID = eRPPartMaterialInformationDto.immPartBinID,
						immPartID = eRPPartMaterialInformationDto.immPartID,
						immPartLongDescriptionRtf = eRPPartMaterialInformationDto.immPartLongDescriptionRtf,
						immPartLongDescriptionText = eRPPartMaterialInformationDto.immPartLongDescriptionText,
						immPartRevisionID = eRPPartMaterialInformationDto.immPartRevisionID,
						immPartShortDescription = eRPPartMaterialInformationDto.immPartShortDescription,
						immPartWarehouseLocationID = eRPPartMaterialInformationDto.immPartWarehouseLocationID,
						immPurchaseLocationID = eRPPartMaterialInformationDto.immPurchaseLocationID,
						immQuantityPerAssembly = eRPPartMaterialInformationDto.immQuantityPerAssembly,
						immRelatedPartOperationID = eRPPartMaterialInformationDto.immRelatedPartOperationID,
						immRowVersion = eRPPartMaterialInformationDto.immRowVersion,
						immScrapPercent = eRPPartMaterialInformationDto.immScrapPercent,
						immScrapQuantity = eRPPartMaterialInformationDto.immScrapQuantity,
						immSupplierOrganizationID = eRPPartMaterialInformationDto.immSupplierOrganizationID,
						immUnitOfMeasure = eRPPartMaterialInformationDto.immUnitOfMeasure,
						CustomFields = eRPPartMaterialInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartMaterial [{partMaterial.immUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartMaterialDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartMaterial(Guid partMaterialId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartMaterialRepository iERPPartMaterialRepository = (base.ERPPartMaterialRepository = new ERPPartMaterialRepository(base.ApiClientContext));
		using (iERPPartMaterialRepository)
		{
			if (!(await base.ERPPartMaterialRepository.DoesPartMaterialExist(partMaterialId)))
			{
				base.ErrorsList.Add($"PartMaterial [{partMaterialId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartMaterialInformationDto eRPPartMaterialInformationDto = await base.ERPPartMaterialRepository.GetPartMaterial(partMaterialId);
				string text = await base.ERPPartMaterialRepository.WhereUsed("PartMaterials", new object[4] { eRPPartMaterialInformationDto.immMethodID, eRPPartMaterialInformationDto.immMethodRevisionID, eRPPartMaterialInformationDto.immMethodAssemblyID, eRPPartMaterialInformationDto.immMethodMaterialID }, new object[4] { "immMethodID", "immMethodRevisionID", "immMethodAssemblyID", "immMethodMaterialID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartMaterial cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartMaterialDto>> Process_DeletePartMaterial(Guid partMaterialId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartMaterialDto> result;
		try
		{
			IERPPartMaterialRepository iERPPartMaterialRepository = (base.ERPPartMaterialRepository = new ERPPartMaterialRepository(base.ApiClientContext));
			using (iERPPartMaterialRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartMaterialRepository.DeleteRowFromTable("PartMaterials", "imm", partMaterialId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartMaterial [{partMaterialId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartMaterialDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartMaterialDto()
			};
		}
		return result;
	}
}
