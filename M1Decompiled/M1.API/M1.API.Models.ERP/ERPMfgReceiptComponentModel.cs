using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPMfgReceiptComponentModel : ERPBaseModel, IERPMfgReceiptComponentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllMfgReceiptComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPMfgReceiptComponentRepository iERPMfgReceiptComponentRepository = (base.ERPMfgReceiptComponentRepository = new ERPMfgReceiptComponentRepository(base.ApiClientContext));
		using (iERPMfgReceiptComponentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPMfgReceiptComponentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPMfgReceiptComponentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPMfgReceiptComponentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPMfgReceiptComponentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetMfgReceiptComponent(Guid mfgReceiptComponentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMfgReceiptComponentRepository iERPMfgReceiptComponentRepository = (base.ERPMfgReceiptComponentRepository = new ERPMfgReceiptComponentRepository(base.ApiClientContext));
		using (iERPMfgReceiptComponentRepository)
		{
			if (!(await base.ERPMfgReceiptComponentRepository.DoesMfgReceiptComponentExist(mfgReceiptComponentId)))
			{
				errorsList.Add($"MfgReceiptComponent [{mfgReceiptComponentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutMfgReceiptComponent(ERPMfgReceiptComponentDto mfgReceiptComponent)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMfgReceiptComponentRepository iERPMfgReceiptComponentRepository = (base.ERPMfgReceiptComponentRepository = new ERPMfgReceiptComponentRepository(base.ApiClientContext));
		using (iERPMfgReceiptComponentRepository)
		{
			if (!string.IsNullOrWhiteSpace(mfgReceiptComponent.rmnMfgReceiptID) && !(await base.ERPMfgReceiptComponentRepository.DoesRecordExistInTableUsingKeys("MfgReceipts", new object[1] { "rmmMfgReceiptID" }, new object[1] { mfgReceiptComponent.rmnMfgReceiptID })))
			{
				errorsList.Add("rmnMfgReceiptID [" + mfgReceiptComponent.rmnMfgReceiptID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceiptComponent.rmnPartID) && !(await base.ERPMfgReceiptComponentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { mfgReceiptComponent.rmnPartID })))
			{
				errorsList.Add("rmnPartID [" + mfgReceiptComponent.rmnPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceiptComponent.rmnPartRevisionID) && !(await base.ERPMfgReceiptComponentRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { mfgReceiptComponent.rmnPartID, mfgReceiptComponent.rmnPartRevisionID })))
			{
				errorsList.Add("rmnPartRevisionID [" + mfgReceiptComponent.rmnPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceiptComponent.rmnPartWarehouseLocationID) && !(await base.ERPMfgReceiptComponentRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { mfgReceiptComponent.rmnPartID, mfgReceiptComponent.rmnPartRevisionID, mfgReceiptComponent.rmnPartWarehouseLocationID })))
			{
				errorsList.Add("rmnPartWarehouseLocationID [" + mfgReceiptComponent.rmnPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceiptComponent.rmnPartBinID) && !(await base.ERPMfgReceiptComponentRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { mfgReceiptComponent.rmnPartID, mfgReceiptComponent.rmnPartRevisionID, mfgReceiptComponent.rmnPartWarehouseLocationID, mfgReceiptComponent.rmnPartBinID })))
			{
				errorsList.Add("rmnPartBinID [" + mfgReceiptComponent.rmnPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceiptComponent.rmnJobID) && !(await base.ERPMfgReceiptComponentRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { mfgReceiptComponent.rmnJobID })))
			{
				errorsList.Add("rmnJobID [" + mfgReceiptComponent.rmnJobID + "] not found.");
			}
			if (mfgReceiptComponent.rmnJobAssemblyID > 0 && !(await base.ERPMfgReceiptComponentRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { mfgReceiptComponent.rmnJobID, mfgReceiptComponent.rmnJobAssemblyID })))
			{
				errorsList.Add($"rmnJobAssemblyID [{mfgReceiptComponent.rmnJobAssemblyID}] not found.");
			}
			if (mfgReceiptComponent.rmnJobMaterialID > 0 && !(await base.ERPMfgReceiptComponentRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { mfgReceiptComponent.rmnJobID, mfgReceiptComponent.rmnJobAssemblyID, mfgReceiptComponent.rmnJobMaterialID })))
			{
				errorsList.Add($"rmnJobMaterialID [{mfgReceiptComponent.rmnJobMaterialID}] not found.");
			}
			if (mfgReceiptComponent.rmnJobMaterialComponentID > 0 && !(await base.ERPMfgReceiptComponentRepository.DoesRecordExistInTableUsingKeys("JobMaterialComponents", new object[4] { "JMTJOBID", "JMTJOBASSEMBLYID", "JMTJOBMATERIALID", "JMTJOBMATERIALCOMPONENTID" }, new object[4] { mfgReceiptComponent.rmnJobID, mfgReceiptComponent.rmnJobAssemblyID, mfgReceiptComponent.rmnJobMaterialID, mfgReceiptComponent.rmnJobMaterialComponentID })))
			{
				errorsList.Add($"rmnJobMaterialComponentID [{mfgReceiptComponent.rmnJobMaterialComponentID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mfgReceiptComponent.rmnReverseMfgReceiptID) && !(await base.ERPMfgReceiptComponentRepository.DoesRecordExistInTableUsingKeys("MfgReceipts", new object[1] { "rmmMfgReceiptID" }, new object[1] { mfgReceiptComponent.rmnReverseMfgReceiptID })))
			{
				errorsList.Add("rmnReverseMfgReceiptID [" + mfgReceiptComponent.rmnReverseMfgReceiptID + "] not found.");
			}
			if (mfgReceiptComponent.rmnReverseMfgReceiptCompID > 0 && !(await base.ERPMfgReceiptComponentRepository.DoesRecordExistInTableUsingKeys("MfgReceiptComponents", new object[2] { "rmnMfgReceiptID", "rmnMfgReceiptComponentID" }, new object[2] { mfgReceiptComponent.rmnReverseMfgReceiptID, mfgReceiptComponent.rmnReverseMfgReceiptCompID })))
			{
				errorsList.Add($"rmnReverseMfgReceiptCompID [{mfgReceiptComponent.rmnReverseMfgReceiptCompID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPMfgReceiptComponentDto>>> Process_GetAllMfgReceiptComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPMfgReceiptComponentDto> allMfgReceiptComponentsDto = new List<ERPMfgReceiptComponentDto>();
		ERPResponseMessageDto<IList<ERPMfgReceiptComponentDto>> result;
		try
		{
			IERPMfgReceiptComponentRepository iERPMfgReceiptComponentRepository = (base.ERPMfgReceiptComponentRepository = new ERPMfgReceiptComponentRepository(base.ApiClientContext));
			using (iERPMfgReceiptComponentRepository)
			{
				foreach (ERPMfgReceiptComponentInformationDto item2 in await base.ERPMfgReceiptComponentRepository.GetAllMfgReceiptComponents(pageSize, pageNumber, filter, orderBy))
				{
					ERPMfgReceiptComponentDto item = new ERPMfgReceiptComponentDto
					{
						rmnAdditionalQuantity = item2.rmnAdditionalQuantity,
						rmnCreatedBy = item2.rmnCreatedBy,
						rmnCreatedDate = item2.rmnCreatedDate,
						rmnDescription = item2.rmnDescription,
						rmnUniqueID = item2.rmnUniqueID,
						rmnExtendedCost = item2.rmnExtendedCost,
						rmnInvParentQuantity = item2.rmnInvParentQuantity,
						rmnInvReceiptQuantity = item2.rmnInvReceiptQuantity,
						rmnPosted = item2.rmnPosted,
						rmnReceivedComplete = item2.rmnReceivedComplete,
						rmnReversed = item2.rmnReversed,
						rmnJobAssemblyID = item2.rmnJobAssemblyID,
						rmnJobID = item2.rmnJobID,
						rmnJobMaterialComponentID = item2.rmnJobMaterialComponentID,
						rmnJobMaterialID = item2.rmnJobMaterialID,
						rmnJobMatParentQuantity = item2.rmnJobMatParentQuantity,
						rmnJobMatReceiptQuantity = item2.rmnJobMatReceiptQuantity,
						rmnMfgReceiptID = item2.rmnMfgReceiptID,
						rmnPartBinID = item2.rmnPartBinID,
						rmnPartID = item2.rmnPartID,
						rmnPartRevisionID = item2.rmnPartRevisionID,
						rmnPartWarehouseLocationID = item2.rmnPartWarehouseLocationID,
						rmnQuantityPerParent = item2.rmnQuantityPerParent,
						rmnReverseMfgReceiptCompID = item2.rmnReverseMfgReceiptCompID,
						rmnReverseMfgReceiptID = item2.rmnReverseMfgReceiptID,
						rmnRowVersion = item2.rmnRowVersion,
						rmnMfgReceiptComponentID = item2.rmnMfgReceiptComponentID,
						rmnUnitCost = item2.rmnUnitCost,
						rmnUnitOfMeasure = item2.rmnUnitOfMeasure,
						rmnWeight = item2.rmnWeight,
						CustomFields = item2.CustomFields
					};
					allMfgReceiptComponentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all MfgReceiptComponents]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPMfgReceiptComponentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allMfgReceiptComponentsDto,
				RecordCount = allMfgReceiptComponentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMfgReceiptComponentDto>> Process_GetMfgReceiptComponent(Guid mfgReceiptComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPMfgReceiptComponentDto mfgReceiptComponentDto = null;
		ERPResponseMessageDto<ERPMfgReceiptComponentDto> result;
		try
		{
			IERPMfgReceiptComponentRepository iERPMfgReceiptComponentRepository = (base.ERPMfgReceiptComponentRepository = new ERPMfgReceiptComponentRepository(base.ApiClientContext));
			using (iERPMfgReceiptComponentRepository)
			{
				ERPMfgReceiptComponentInformationDto eRPMfgReceiptComponentInformationDto = await base.ERPMfgReceiptComponentRepository.GetMfgReceiptComponent(mfgReceiptComponentId);
				mfgReceiptComponentDto = new ERPMfgReceiptComponentDto
				{
					rmnAdditionalQuantity = eRPMfgReceiptComponentInformationDto.rmnAdditionalQuantity,
					rmnCreatedBy = eRPMfgReceiptComponentInformationDto.rmnCreatedBy,
					rmnCreatedDate = eRPMfgReceiptComponentInformationDto.rmnCreatedDate,
					rmnDescription = eRPMfgReceiptComponentInformationDto.rmnDescription,
					rmnUniqueID = eRPMfgReceiptComponentInformationDto.rmnUniqueID,
					rmnExtendedCost = eRPMfgReceiptComponentInformationDto.rmnExtendedCost,
					rmnInvParentQuantity = eRPMfgReceiptComponentInformationDto.rmnInvParentQuantity,
					rmnInvReceiptQuantity = eRPMfgReceiptComponentInformationDto.rmnInvReceiptQuantity,
					rmnPosted = eRPMfgReceiptComponentInformationDto.rmnPosted,
					rmnReceivedComplete = eRPMfgReceiptComponentInformationDto.rmnReceivedComplete,
					rmnReversed = eRPMfgReceiptComponentInformationDto.rmnReversed,
					rmnJobAssemblyID = eRPMfgReceiptComponentInformationDto.rmnJobAssemblyID,
					rmnJobID = eRPMfgReceiptComponentInformationDto.rmnJobID,
					rmnJobMaterialComponentID = eRPMfgReceiptComponentInformationDto.rmnJobMaterialComponentID,
					rmnJobMaterialID = eRPMfgReceiptComponentInformationDto.rmnJobMaterialID,
					rmnJobMatParentQuantity = eRPMfgReceiptComponentInformationDto.rmnJobMatParentQuantity,
					rmnJobMatReceiptQuantity = eRPMfgReceiptComponentInformationDto.rmnJobMatReceiptQuantity,
					rmnMfgReceiptID = eRPMfgReceiptComponentInformationDto.rmnMfgReceiptID,
					rmnPartBinID = eRPMfgReceiptComponentInformationDto.rmnPartBinID,
					rmnPartID = eRPMfgReceiptComponentInformationDto.rmnPartID,
					rmnPartRevisionID = eRPMfgReceiptComponentInformationDto.rmnPartRevisionID,
					rmnPartWarehouseLocationID = eRPMfgReceiptComponentInformationDto.rmnPartWarehouseLocationID,
					rmnQuantityPerParent = eRPMfgReceiptComponentInformationDto.rmnQuantityPerParent,
					rmnReverseMfgReceiptCompID = eRPMfgReceiptComponentInformationDto.rmnReverseMfgReceiptCompID,
					rmnReverseMfgReceiptID = eRPMfgReceiptComponentInformationDto.rmnReverseMfgReceiptID,
					rmnRowVersion = eRPMfgReceiptComponentInformationDto.rmnRowVersion,
					rmnMfgReceiptComponentID = eRPMfgReceiptComponentInformationDto.rmnMfgReceiptComponentID,
					rmnUnitCost = eRPMfgReceiptComponentInformationDto.rmnUnitCost,
					rmnUnitOfMeasure = eRPMfgReceiptComponentInformationDto.rmnUnitOfMeasure,
					rmnWeight = eRPMfgReceiptComponentInformationDto.rmnWeight,
					CustomFields = eRPMfgReceiptComponentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the MfgReceiptComponents []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMfgReceiptComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = mfgReceiptComponentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMfgReceiptComponentDto>> Process_PutMfgReceiptComponent(ERPMfgReceiptComponentDto mfgReceiptComponent)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPMfgReceiptComponentDto createdObject = null;
		ERPResponseMessageDto<ERPMfgReceiptComponentDto> result;
		try
		{
			IERPMfgReceiptComponentRepository iERPMfgReceiptComponentRepository = (base.ERPMfgReceiptComponentRepository = new ERPMfgReceiptComponentRepository(base.ApiClientContext));
			using (iERPMfgReceiptComponentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPMfgReceiptComponentRepository.SaveMfgReceiptComponent(mfgReceiptComponent);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPMfgReceiptComponentInformationDto eRPMfgReceiptComponentInformationDto = await base.ERPMfgReceiptComponentRepository.GetMfgReceiptComponent(mfgReceiptComponent.rmnUniqueID);
					createdObject = new ERPMfgReceiptComponentDto
					{
						rmnAdditionalQuantity = eRPMfgReceiptComponentInformationDto.rmnAdditionalQuantity,
						rmnCreatedBy = eRPMfgReceiptComponentInformationDto.rmnCreatedBy,
						rmnCreatedDate = eRPMfgReceiptComponentInformationDto.rmnCreatedDate,
						rmnDescription = eRPMfgReceiptComponentInformationDto.rmnDescription,
						rmnUniqueID = eRPMfgReceiptComponentInformationDto.rmnUniqueID,
						rmnExtendedCost = eRPMfgReceiptComponentInformationDto.rmnExtendedCost,
						rmnInvParentQuantity = eRPMfgReceiptComponentInformationDto.rmnInvParentQuantity,
						rmnInvReceiptQuantity = eRPMfgReceiptComponentInformationDto.rmnInvReceiptQuantity,
						rmnPosted = eRPMfgReceiptComponentInformationDto.rmnPosted,
						rmnReceivedComplete = eRPMfgReceiptComponentInformationDto.rmnReceivedComplete,
						rmnReversed = eRPMfgReceiptComponentInformationDto.rmnReversed,
						rmnJobAssemblyID = eRPMfgReceiptComponentInformationDto.rmnJobAssemblyID,
						rmnJobID = eRPMfgReceiptComponentInformationDto.rmnJobID,
						rmnJobMaterialComponentID = eRPMfgReceiptComponentInformationDto.rmnJobMaterialComponentID,
						rmnJobMaterialID = eRPMfgReceiptComponentInformationDto.rmnJobMaterialID,
						rmnJobMatParentQuantity = eRPMfgReceiptComponentInformationDto.rmnJobMatParentQuantity,
						rmnJobMatReceiptQuantity = eRPMfgReceiptComponentInformationDto.rmnJobMatReceiptQuantity,
						rmnMfgReceiptID = eRPMfgReceiptComponentInformationDto.rmnMfgReceiptID,
						rmnPartBinID = eRPMfgReceiptComponentInformationDto.rmnPartBinID,
						rmnPartID = eRPMfgReceiptComponentInformationDto.rmnPartID,
						rmnPartRevisionID = eRPMfgReceiptComponentInformationDto.rmnPartRevisionID,
						rmnPartWarehouseLocationID = eRPMfgReceiptComponentInformationDto.rmnPartWarehouseLocationID,
						rmnQuantityPerParent = eRPMfgReceiptComponentInformationDto.rmnQuantityPerParent,
						rmnReverseMfgReceiptCompID = eRPMfgReceiptComponentInformationDto.rmnReverseMfgReceiptCompID,
						rmnReverseMfgReceiptID = eRPMfgReceiptComponentInformationDto.rmnReverseMfgReceiptID,
						rmnRowVersion = eRPMfgReceiptComponentInformationDto.rmnRowVersion,
						rmnMfgReceiptComponentID = eRPMfgReceiptComponentInformationDto.rmnMfgReceiptComponentID,
						rmnUnitCost = eRPMfgReceiptComponentInformationDto.rmnUnitCost,
						rmnUnitOfMeasure = eRPMfgReceiptComponentInformationDto.rmnUnitOfMeasure,
						rmnWeight = eRPMfgReceiptComponentInformationDto.rmnWeight,
						CustomFields = eRPMfgReceiptComponentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing MfgReceiptComponent [{mfgReceiptComponent.rmnUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMfgReceiptComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteMfgReceiptComponent(Guid mfgReceiptComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMfgReceiptComponentRepository iERPMfgReceiptComponentRepository = (base.ERPMfgReceiptComponentRepository = new ERPMfgReceiptComponentRepository(base.ApiClientContext));
		using (iERPMfgReceiptComponentRepository)
		{
			if (!(await base.ERPMfgReceiptComponentRepository.DoesMfgReceiptComponentExist(mfgReceiptComponentId)))
			{
				base.ErrorsList.Add($"MfgReceiptComponent [{mfgReceiptComponentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPMfgReceiptComponentInformationDto eRPMfgReceiptComponentInformationDto = await base.ERPMfgReceiptComponentRepository.GetMfgReceiptComponent(mfgReceiptComponentId);
				string text = await base.ERPMfgReceiptComponentRepository.WhereUsed("MfgReceiptComponents", new object[2] { eRPMfgReceiptComponentInformationDto.rmnMfgReceiptID, eRPMfgReceiptComponentInformationDto.rmnMfgReceiptComponentID }, new object[2] { "rmnMfgReceiptID", "rmnMfgReceiptComponentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("MfgReceiptComponent cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPMfgReceiptComponentDto>> Process_DeleteMfgReceiptComponent(Guid mfgReceiptComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPMfgReceiptComponentDto> result;
		try
		{
			IERPMfgReceiptComponentRepository iERPMfgReceiptComponentRepository = (base.ERPMfgReceiptComponentRepository = new ERPMfgReceiptComponentRepository(base.ApiClientContext));
			using (iERPMfgReceiptComponentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPMfgReceiptComponentRepository.DeleteRowFromTable("MfgReceiptComponents", "rmn", mfgReceiptComponentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of MfgReceiptComponent [{mfgReceiptComponentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMfgReceiptComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPMfgReceiptComponentDto()
			};
		}
		return result;
	}
}
