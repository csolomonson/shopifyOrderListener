using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPurchaseOrderComponentModel : ERPBaseModel, IERPPurchaseOrderComponentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPurchaseOrderComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPurchaseOrderComponentRepository iERPPurchaseOrderComponentRepository = (base.ERPPurchaseOrderComponentRepository = new ERPPurchaseOrderComponentRepository(base.ApiClientContext));
		using (iERPPurchaseOrderComponentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPurchaseOrderComponentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPurchaseOrderComponentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPurchaseOrderComponentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPurchaseOrderComponentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPurchaseOrderComponent(Guid purchaseOrderComponentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderComponentRepository iERPPurchaseOrderComponentRepository = (base.ERPPurchaseOrderComponentRepository = new ERPPurchaseOrderComponentRepository(base.ApiClientContext));
		using (iERPPurchaseOrderComponentRepository)
		{
			if (!(await base.ERPPurchaseOrderComponentRepository.DoesPurchaseOrderComponentExist(purchaseOrderComponentId)))
			{
				errorsList.Add($"PurchaseOrderComponent [{purchaseOrderComponentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPurchaseOrderComponent(ERPPurchaseOrderComponentDto purchaseOrderComponent)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderComponentRepository iERPPurchaseOrderComponentRepository = (base.ERPPurchaseOrderComponentRepository = new ERPPurchaseOrderComponentRepository(base.ApiClientContext));
		using (iERPPurchaseOrderComponentRepository)
		{
			if (!string.IsNullOrWhiteSpace(purchaseOrderComponent.pmoPurchaseOrderID) && !(await base.ERPPurchaseOrderComponentRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { purchaseOrderComponent.pmoPurchaseOrderID })))
			{
				errorsList.Add("pmoPurchaseOrderID [" + purchaseOrderComponent.pmoPurchaseOrderID + "] not found.");
			}
			if (purchaseOrderComponent.pmoPurchaseOrderLineID > 0 && !(await base.ERPPurchaseOrderComponentRepository.DoesRecordExistInTableUsingKeys("PurchaseOrderLines", new object[2] { "PMLPURCHASEORDERID", "PMLPURCHASEORDERLINEID" }, new object[2] { purchaseOrderComponent.pmoPurchaseOrderID, purchaseOrderComponent.pmoPurchaseOrderLineID })))
			{
				errorsList.Add($"pmoPurchaseOrderLineID [{purchaseOrderComponent.pmoPurchaseOrderLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderComponent.pmoPartID) && !(await base.ERPPurchaseOrderComponentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { purchaseOrderComponent.pmoPartID })))
			{
				errorsList.Add("pmoPartID [" + purchaseOrderComponent.pmoPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderComponent.pmoPartRevisionID) && !(await base.ERPPurchaseOrderComponentRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { purchaseOrderComponent.pmoPartID, purchaseOrderComponent.pmoPartRevisionID })))
			{
				errorsList.Add("pmoPartRevisionID [" + purchaseOrderComponent.pmoPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderComponent.pmoPartWarehouseLocationID) && !(await base.ERPPurchaseOrderComponentRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { purchaseOrderComponent.pmoPartID, purchaseOrderComponent.pmoPartRevisionID, purchaseOrderComponent.pmoPartWarehouseLocationID })))
			{
				errorsList.Add("pmoPartWarehouseLocationID [" + purchaseOrderComponent.pmoPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderComponent.pmoPartBinID) && !(await base.ERPPurchaseOrderComponentRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { purchaseOrderComponent.pmoPartID, purchaseOrderComponent.pmoPartRevisionID, purchaseOrderComponent.pmoPartWarehouseLocationID, purchaseOrderComponent.pmoPartBinID })))
			{
				errorsList.Add("pmoPartBinID [" + purchaseOrderComponent.pmoPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderComponent.pmoJobID) && !(await base.ERPPurchaseOrderComponentRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { purchaseOrderComponent.pmoJobID })))
			{
				errorsList.Add("pmoJobID [" + purchaseOrderComponent.pmoJobID + "] not found.");
			}
			if (purchaseOrderComponent.pmoJobMaterialID > 0 && !(await base.ERPPurchaseOrderComponentRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { purchaseOrderComponent.pmoJobID, purchaseOrderComponent.pmoJobAssemblyID, purchaseOrderComponent.pmoJobMaterialID })))
			{
				errorsList.Add($"pmoJobMaterialID [{purchaseOrderComponent.pmoJobMaterialID}] not found.");
			}
			if (purchaseOrderComponent.pmoJobAssemblyID > 0 && !(await base.ERPPurchaseOrderComponentRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { purchaseOrderComponent.pmoJobID, purchaseOrderComponent.pmoJobAssemblyID })))
			{
				errorsList.Add($"pmoJobAssemblyID [{purchaseOrderComponent.pmoJobAssemblyID}] not found.");
			}
			if (purchaseOrderComponent.pmoJobMaterialComponentID > 0 && !(await base.ERPPurchaseOrderComponentRepository.DoesRecordExistInTableUsingKeys("JobMaterialComponents", new object[4] { "JMTJOBID", "JMTJOBASSEMBLYID", "JMTJOBMATERIALID", "JMTJOBMATERIALCOMPONENTID" }, new object[4] { purchaseOrderComponent.pmoJobID, purchaseOrderComponent.pmoJobAssemblyID, purchaseOrderComponent.pmoJobMaterialID, purchaseOrderComponent.pmoJobMaterialComponentID })))
			{
				errorsList.Add($"pmoJobMaterialComponentID [{purchaseOrderComponent.pmoJobMaterialComponentID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPurchaseOrderComponentDto>>> Process_GetAllPurchaseOrderComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPurchaseOrderComponentDto> allPurchaseOrderComponentsDto = new List<ERPPurchaseOrderComponentDto>();
		ERPResponseMessageDto<IList<ERPPurchaseOrderComponentDto>> result;
		try
		{
			IERPPurchaseOrderComponentRepository iERPPurchaseOrderComponentRepository = (base.ERPPurchaseOrderComponentRepository = new ERPPurchaseOrderComponentRepository(base.ApiClientContext));
			using (iERPPurchaseOrderComponentRepository)
			{
				foreach (ERPPurchaseOrderComponentInformationDto item2 in await base.ERPPurchaseOrderComponentRepository.GetAllPurchaseOrderComponents(pageSize, pageNumber, filter, orderBy))
				{
					ERPPurchaseOrderComponentDto item = new ERPPurchaseOrderComponentDto
					{
						pmoAdditionalQuantity = item2.pmoAdditionalQuantity,
						pmoCreatedBy = item2.pmoCreatedBy,
						pmoCreatedDate = item2.pmoCreatedDate,
						pmoDeliveryQuantity = item2.pmoDeliveryQuantity,
						pmoDescription = item2.pmoDescription,
						pmoUniqueID = item2.pmoUniqueID,
						pmoExtendedCostBase = item2.pmoExtendedCostBase,
						pmoExtendedCostForeign = item2.pmoExtendedCostForeign,
						pmoClosed = item2.pmoClosed,
						pmoIntraCompanyPosted = item2.pmoIntraCompanyPosted,
						pmoReceivedComplete = item2.pmoReceivedComplete,
						pmoJobAssemblyID = item2.pmoJobAssemblyID,
						pmoJobID = item2.pmoJobID,
						pmoJobMaterialComponentID = item2.pmoJobMaterialComponentID,
						pmoJobMaterialID = item2.pmoJobMaterialID,
						pmoParentQuantity = item2.pmoParentQuantity,
						pmoPartBinID = item2.pmoPartBinID,
						pmoPartID = item2.pmoPartID,
						pmoPartRevisionID = item2.pmoPartRevisionID,
						pmoPartWarehouseLocationID = item2.pmoPartWarehouseLocationID,
						pmoPurchaseOrderID = item2.pmoPurchaseOrderID,
						pmoPurchaseOrderLineID = item2.pmoPurchaseOrderLineID,
						pmoPurchaseUnitCost = item2.pmoPurchaseUnitCost,
						pmoPurchaseUnitCostForeign = item2.pmoPurchaseUnitCostForeign,
						pmoQuantityPerParent = item2.pmoQuantityPerParent,
						pmoQuantityReceived = item2.pmoQuantityReceived,
						pmoRowVersion = item2.pmoRowVersion,
						pmoPurchaseOrderComponentID = item2.pmoPurchaseOrderComponentID,
						pmoUnitOfMeasure = item2.pmoUnitOfMeasure,
						pmoWeight = item2.pmoWeight,
						CustomFields = item2.CustomFields
					};
					allPurchaseOrderComponentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PurchaseOrderComponents]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPurchaseOrderComponentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPurchaseOrderComponentsDto,
				RecordCount = allPurchaseOrderComponentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderComponentDto>> Process_GetPurchaseOrderComponent(Guid purchaseOrderComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPurchaseOrderComponentDto purchaseOrderComponentDto = null;
		ERPResponseMessageDto<ERPPurchaseOrderComponentDto> result;
		try
		{
			IERPPurchaseOrderComponentRepository iERPPurchaseOrderComponentRepository = (base.ERPPurchaseOrderComponentRepository = new ERPPurchaseOrderComponentRepository(base.ApiClientContext));
			using (iERPPurchaseOrderComponentRepository)
			{
				ERPPurchaseOrderComponentInformationDto eRPPurchaseOrderComponentInformationDto = await base.ERPPurchaseOrderComponentRepository.GetPurchaseOrderComponent(purchaseOrderComponentId);
				purchaseOrderComponentDto = new ERPPurchaseOrderComponentDto
				{
					pmoAdditionalQuantity = eRPPurchaseOrderComponentInformationDto.pmoAdditionalQuantity,
					pmoCreatedBy = eRPPurchaseOrderComponentInformationDto.pmoCreatedBy,
					pmoCreatedDate = eRPPurchaseOrderComponentInformationDto.pmoCreatedDate,
					pmoDeliveryQuantity = eRPPurchaseOrderComponentInformationDto.pmoDeliveryQuantity,
					pmoDescription = eRPPurchaseOrderComponentInformationDto.pmoDescription,
					pmoUniqueID = eRPPurchaseOrderComponentInformationDto.pmoUniqueID,
					pmoExtendedCostBase = eRPPurchaseOrderComponentInformationDto.pmoExtendedCostBase,
					pmoExtendedCostForeign = eRPPurchaseOrderComponentInformationDto.pmoExtendedCostForeign,
					pmoClosed = eRPPurchaseOrderComponentInformationDto.pmoClosed,
					pmoIntraCompanyPosted = eRPPurchaseOrderComponentInformationDto.pmoIntraCompanyPosted,
					pmoReceivedComplete = eRPPurchaseOrderComponentInformationDto.pmoReceivedComplete,
					pmoJobAssemblyID = eRPPurchaseOrderComponentInformationDto.pmoJobAssemblyID,
					pmoJobID = eRPPurchaseOrderComponentInformationDto.pmoJobID,
					pmoJobMaterialComponentID = eRPPurchaseOrderComponentInformationDto.pmoJobMaterialComponentID,
					pmoJobMaterialID = eRPPurchaseOrderComponentInformationDto.pmoJobMaterialID,
					pmoParentQuantity = eRPPurchaseOrderComponentInformationDto.pmoParentQuantity,
					pmoPartBinID = eRPPurchaseOrderComponentInformationDto.pmoPartBinID,
					pmoPartID = eRPPurchaseOrderComponentInformationDto.pmoPartID,
					pmoPartRevisionID = eRPPurchaseOrderComponentInformationDto.pmoPartRevisionID,
					pmoPartWarehouseLocationID = eRPPurchaseOrderComponentInformationDto.pmoPartWarehouseLocationID,
					pmoPurchaseOrderID = eRPPurchaseOrderComponentInformationDto.pmoPurchaseOrderID,
					pmoPurchaseOrderLineID = eRPPurchaseOrderComponentInformationDto.pmoPurchaseOrderLineID,
					pmoPurchaseUnitCost = eRPPurchaseOrderComponentInformationDto.pmoPurchaseUnitCost,
					pmoPurchaseUnitCostForeign = eRPPurchaseOrderComponentInformationDto.pmoPurchaseUnitCostForeign,
					pmoQuantityPerParent = eRPPurchaseOrderComponentInformationDto.pmoQuantityPerParent,
					pmoQuantityReceived = eRPPurchaseOrderComponentInformationDto.pmoQuantityReceived,
					pmoRowVersion = eRPPurchaseOrderComponentInformationDto.pmoRowVersion,
					pmoPurchaseOrderComponentID = eRPPurchaseOrderComponentInformationDto.pmoPurchaseOrderComponentID,
					pmoUnitOfMeasure = eRPPurchaseOrderComponentInformationDto.pmoUnitOfMeasure,
					pmoWeight = eRPPurchaseOrderComponentInformationDto.pmoWeight,
					CustomFields = eRPPurchaseOrderComponentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PurchaseOrderComponents []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = purchaseOrderComponentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderComponentDto>> Process_PutPurchaseOrderComponent(ERPPurchaseOrderComponentDto purchaseOrderComponent)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPurchaseOrderComponentDto createdObject = null;
		ERPResponseMessageDto<ERPPurchaseOrderComponentDto> result;
		try
		{
			IERPPurchaseOrderComponentRepository iERPPurchaseOrderComponentRepository = (base.ERPPurchaseOrderComponentRepository = new ERPPurchaseOrderComponentRepository(base.ApiClientContext));
			using (iERPPurchaseOrderComponentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPurchaseOrderComponentRepository.SavePurchaseOrderComponent(purchaseOrderComponent);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPurchaseOrderComponentInformationDto eRPPurchaseOrderComponentInformationDto = await base.ERPPurchaseOrderComponentRepository.GetPurchaseOrderComponent(purchaseOrderComponent.pmoUniqueID);
					createdObject = new ERPPurchaseOrderComponentDto
					{
						pmoAdditionalQuantity = eRPPurchaseOrderComponentInformationDto.pmoAdditionalQuantity,
						pmoCreatedBy = eRPPurchaseOrderComponentInformationDto.pmoCreatedBy,
						pmoCreatedDate = eRPPurchaseOrderComponentInformationDto.pmoCreatedDate,
						pmoDeliveryQuantity = eRPPurchaseOrderComponentInformationDto.pmoDeliveryQuantity,
						pmoDescription = eRPPurchaseOrderComponentInformationDto.pmoDescription,
						pmoUniqueID = eRPPurchaseOrderComponentInformationDto.pmoUniqueID,
						pmoExtendedCostBase = eRPPurchaseOrderComponentInformationDto.pmoExtendedCostBase,
						pmoExtendedCostForeign = eRPPurchaseOrderComponentInformationDto.pmoExtendedCostForeign,
						pmoClosed = eRPPurchaseOrderComponentInformationDto.pmoClosed,
						pmoIntraCompanyPosted = eRPPurchaseOrderComponentInformationDto.pmoIntraCompanyPosted,
						pmoReceivedComplete = eRPPurchaseOrderComponentInformationDto.pmoReceivedComplete,
						pmoJobAssemblyID = eRPPurchaseOrderComponentInformationDto.pmoJobAssemblyID,
						pmoJobID = eRPPurchaseOrderComponentInformationDto.pmoJobID,
						pmoJobMaterialComponentID = eRPPurchaseOrderComponentInformationDto.pmoJobMaterialComponentID,
						pmoJobMaterialID = eRPPurchaseOrderComponentInformationDto.pmoJobMaterialID,
						pmoParentQuantity = eRPPurchaseOrderComponentInformationDto.pmoParentQuantity,
						pmoPartBinID = eRPPurchaseOrderComponentInformationDto.pmoPartBinID,
						pmoPartID = eRPPurchaseOrderComponentInformationDto.pmoPartID,
						pmoPartRevisionID = eRPPurchaseOrderComponentInformationDto.pmoPartRevisionID,
						pmoPartWarehouseLocationID = eRPPurchaseOrderComponentInformationDto.pmoPartWarehouseLocationID,
						pmoPurchaseOrderID = eRPPurchaseOrderComponentInformationDto.pmoPurchaseOrderID,
						pmoPurchaseOrderLineID = eRPPurchaseOrderComponentInformationDto.pmoPurchaseOrderLineID,
						pmoPurchaseUnitCost = eRPPurchaseOrderComponentInformationDto.pmoPurchaseUnitCost,
						pmoPurchaseUnitCostForeign = eRPPurchaseOrderComponentInformationDto.pmoPurchaseUnitCostForeign,
						pmoQuantityPerParent = eRPPurchaseOrderComponentInformationDto.pmoQuantityPerParent,
						pmoQuantityReceived = eRPPurchaseOrderComponentInformationDto.pmoQuantityReceived,
						pmoRowVersion = eRPPurchaseOrderComponentInformationDto.pmoRowVersion,
						pmoPurchaseOrderComponentID = eRPPurchaseOrderComponentInformationDto.pmoPurchaseOrderComponentID,
						pmoUnitOfMeasure = eRPPurchaseOrderComponentInformationDto.pmoUnitOfMeasure,
						pmoWeight = eRPPurchaseOrderComponentInformationDto.pmoWeight,
						CustomFields = eRPPurchaseOrderComponentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PurchaseOrderComponent [{purchaseOrderComponent.pmoUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePurchaseOrderComponent(Guid purchaseOrderComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderComponentRepository iERPPurchaseOrderComponentRepository = (base.ERPPurchaseOrderComponentRepository = new ERPPurchaseOrderComponentRepository(base.ApiClientContext));
		using (iERPPurchaseOrderComponentRepository)
		{
			if (!(await base.ERPPurchaseOrderComponentRepository.DoesPurchaseOrderComponentExist(purchaseOrderComponentId)))
			{
				base.ErrorsList.Add($"PurchaseOrderComponent [{purchaseOrderComponentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPurchaseOrderComponentInformationDto eRPPurchaseOrderComponentInformationDto = await base.ERPPurchaseOrderComponentRepository.GetPurchaseOrderComponent(purchaseOrderComponentId);
				string text = await base.ERPPurchaseOrderComponentRepository.WhereUsed("PurchaseOrderComponents", new object[3] { eRPPurchaseOrderComponentInformationDto.pmoPurchaseOrderID, eRPPurchaseOrderComponentInformationDto.pmoPurchaseOrderLineID, eRPPurchaseOrderComponentInformationDto.pmoPurchaseOrderComponentID }, new object[3] { "pmoPurchaseOrderID", "pmoPurchaseOrderLineID", "pmoPurchaseOrderComponentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PurchaseOrderComponent cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderComponentDto>> Process_DeletePurchaseOrderComponent(Guid purchaseOrderComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPurchaseOrderComponentDto> result;
		try
		{
			IERPPurchaseOrderComponentRepository iERPPurchaseOrderComponentRepository = (base.ERPPurchaseOrderComponentRepository = new ERPPurchaseOrderComponentRepository(base.ApiClientContext));
			using (iERPPurchaseOrderComponentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPurchaseOrderComponentRepository.DeleteRowFromTable("PurchaseOrderComponents", "pmo", purchaseOrderComponentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PurchaseOrderComponent [{purchaseOrderComponentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPurchaseOrderComponentDto()
			};
		}
		return result;
	}
}
