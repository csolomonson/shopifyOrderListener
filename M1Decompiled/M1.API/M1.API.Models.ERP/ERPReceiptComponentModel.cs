using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPReceiptComponentModel : ERPBaseModel, IERPReceiptComponentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllReceiptComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPReceiptComponentRepository iERPReceiptComponentRepository = (base.ERPReceiptComponentRepository = new ERPReceiptComponentRepository(base.ApiClientContext));
		using (iERPReceiptComponentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPReceiptComponentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPReceiptComponentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPReceiptComponentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPReceiptComponentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetReceiptComponent(Guid receiptComponentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPReceiptComponentRepository iERPReceiptComponentRepository = (base.ERPReceiptComponentRepository = new ERPReceiptComponentRepository(base.ApiClientContext));
		using (iERPReceiptComponentRepository)
		{
			if (!(await base.ERPReceiptComponentRepository.DoesReceiptComponentExist(receiptComponentId)))
			{
				errorsList.Add($"ReceiptComponent [{receiptComponentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutReceiptComponent(ERPReceiptComponentDto receiptComponent)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPReceiptComponentRepository iERPReceiptComponentRepository = (base.ERPReceiptComponentRepository = new ERPReceiptComponentRepository(base.ApiClientContext));
		using (iERPReceiptComponentRepository)
		{
			if (!string.IsNullOrWhiteSpace(receiptComponent.rmoReceiptID) && !(await base.ERPReceiptComponentRepository.DoesRecordExistInTableUsingKeys("Receipts", new object[1] { "RMPRECEIPTID" }, new object[1] { receiptComponent.rmoReceiptID })))
			{
				errorsList.Add("rmoReceiptID [" + receiptComponent.rmoReceiptID + "] not found.");
			}
			if (receiptComponent.rmoReceiptLineID > 0 && !(await base.ERPReceiptComponentRepository.DoesRecordExistInTableUsingKeys("ReceiptLines", new object[2] { "RMLRECEIPTID", "RMLRECEIPTLINEID" }, new object[2] { receiptComponent.rmoReceiptID, receiptComponent.rmoReceiptLineID })))
			{
				errorsList.Add($"rmoReceiptLineID [{receiptComponent.rmoReceiptLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptComponent.rmoPartID) && !(await base.ERPReceiptComponentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { receiptComponent.rmoPartID })))
			{
				errorsList.Add("rmoPartID [" + receiptComponent.rmoPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptComponent.rmoPartRevisionID) && !(await base.ERPReceiptComponentRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { receiptComponent.rmoPartID, receiptComponent.rmoPartRevisionID })))
			{
				errorsList.Add("rmoPartRevisionID [" + receiptComponent.rmoPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptComponent.rmoPartWarehouseLocationID) && !(await base.ERPReceiptComponentRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { receiptComponent.rmoPartID, receiptComponent.rmoPartRevisionID, receiptComponent.rmoPartWarehouseLocationID })))
			{
				errorsList.Add("rmoPartWarehouseLocationID [" + receiptComponent.rmoPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptComponent.rmoPartBinID) && !(await base.ERPReceiptComponentRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { receiptComponent.rmoPartID, receiptComponent.rmoPartRevisionID, receiptComponent.rmoPartWarehouseLocationID, receiptComponent.rmoPartBinID })))
			{
				errorsList.Add("rmoPartBinID [" + receiptComponent.rmoPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptComponent.rmoPurchaseOrderID) && !(await base.ERPReceiptComponentRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { receiptComponent.rmoPurchaseOrderID })))
			{
				errorsList.Add("rmoPurchaseOrderID [" + receiptComponent.rmoPurchaseOrderID + "] not found.");
			}
			if (receiptComponent.rmoPurchaseOrderLineID > 0 && !(await base.ERPReceiptComponentRepository.DoesRecordExistInTableUsingKeys("PurchaseOrderLines", new object[2] { "PMLPURCHASEORDERID", "PMLPURCHASEORDERLINEID" }, new object[2] { receiptComponent.rmoPurchaseOrderID, receiptComponent.rmoPurchaseOrderLineID })))
			{
				errorsList.Add($"rmoPurchaseOrderLineID [{receiptComponent.rmoPurchaseOrderLineID}] not found.");
			}
			if (receiptComponent.rmoPurchaseOrderComponentID > 0 && !(await base.ERPReceiptComponentRepository.DoesRecordExistInTableUsingKeys("PurchaseOrderComponents", new object[3] { "PMOPURCHASEORDERID", "PMOPURCHASEORDERLINEID", "PMOPURCHASEORDERCOMPONENTID" }, new object[3] { receiptComponent.rmoPurchaseOrderID, receiptComponent.rmoPurchaseOrderLineID, receiptComponent.rmoPurchaseOrderComponentID })))
			{
				errorsList.Add($"rmoPurchaseOrderComponentID [{receiptComponent.rmoPurchaseOrderComponentID}] not found.");
			}
			if (receiptComponent.rmoJobAssemblyID > 0 && !(await base.ERPReceiptComponentRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { receiptComponent.rmoJobID, receiptComponent.rmoJobAssemblyID })))
			{
				errorsList.Add($"rmoJobAssemblyID [{receiptComponent.rmoJobAssemblyID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptComponent.rmoJobID) && !(await base.ERPReceiptComponentRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { receiptComponent.rmoJobID })))
			{
				errorsList.Add("rmoJobID [" + receiptComponent.rmoJobID + "] not found.");
			}
			if (receiptComponent.rmoJobMaterialComponentID > 0 && !(await base.ERPReceiptComponentRepository.DoesRecordExistInTableUsingKeys("JobMaterialComponents", new object[4] { "JMTJOBID", "JMTJOBASSEMBLYID", "JMTJOBMATERIALID", "JMTJOBMATERIALCOMPONENTID" }, new object[4] { receiptComponent.rmoJobID, receiptComponent.rmoJobAssemblyID, receiptComponent.rmoJobMaterialID, receiptComponent.rmoJobMaterialComponentID })))
			{
				errorsList.Add($"rmoJobMaterialComponentID [{receiptComponent.rmoJobMaterialComponentID}] not found.");
			}
			if (receiptComponent.rmoJobMaterialID > 0 && !(await base.ERPReceiptComponentRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { receiptComponent.rmoJobID, receiptComponent.rmoJobAssemblyID, receiptComponent.rmoJobMaterialID })))
			{
				errorsList.Add($"rmoJobMaterialID [{receiptComponent.rmoJobMaterialID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(receiptComponent.rmoReverseReceiptID) && !(await base.ERPReceiptComponentRepository.DoesRecordExistInTableUsingKeys("Receipts", new object[1] { "RMPRECEIPTID" }, new object[1] { receiptComponent.rmoReverseReceiptID })))
			{
				errorsList.Add("rmoReverseReceiptID [" + receiptComponent.rmoReverseReceiptID + "] not found.");
			}
			if (receiptComponent.rmoReverseReceiptLineID > 0 && !(await base.ERPReceiptComponentRepository.DoesRecordExistInTableUsingKeys("ReceiptLines", new object[2] { "RMLRECEIPTID", "RMLRECEIPTLINEID" }, new object[2] { receiptComponent.rmoReverseReceiptID, receiptComponent.rmoReverseReceiptLineID })))
			{
				errorsList.Add($"rmoReverseReceiptLineID [{receiptComponent.rmoReverseReceiptLineID}] not found.");
			}
			if (receiptComponent.rmoReverseReceiptComponentID > 0 && !(await base.ERPReceiptComponentRepository.DoesRecordExistInTableUsingKeys("ReceiptComponents", new object[3] { "RMORECEIPTID", "RMORECEIPTLINEID", "RMORECEIPTCOMPONENTID" }, new object[3] { receiptComponent.rmoReverseReceiptID, receiptComponent.rmoReverseReceiptLineID, receiptComponent.rmoReverseReceiptComponentID })))
			{
				errorsList.Add($"rmoReverseReceiptComponentID [{receiptComponent.rmoReverseReceiptComponentID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPReceiptComponentDto>>> Process_GetAllReceiptComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPReceiptComponentDto> allReceiptComponentsDto = new List<ERPReceiptComponentDto>();
		ERPResponseMessageDto<IList<ERPReceiptComponentDto>> result;
		try
		{
			IERPReceiptComponentRepository iERPReceiptComponentRepository = (base.ERPReceiptComponentRepository = new ERPReceiptComponentRepository(base.ApiClientContext));
			using (iERPReceiptComponentRepository)
			{
				foreach (ERPReceiptComponentInformationDto item2 in await base.ERPReceiptComponentRepository.GetAllReceiptComponents(pageSize, pageNumber, filter, orderBy))
				{
					ERPReceiptComponentDto item = new ERPReceiptComponentDto
					{
						rmoAdditionalQuantity = item2.rmoAdditionalQuantity,
						rmoConversionFactor = item2.rmoConversionFactor,
						rmoCreatedBy = item2.rmoCreatedBy,
						rmoCreatedDate = item2.rmoCreatedDate,
						rmoDescription = item2.rmoDescription,
						rmoUniqueID = item2.rmoUniqueID,
						rmoExtendedCostBase = item2.rmoExtendedCostBase,
						rmoExtendedCostForeign = item2.rmoExtendedCostForeign,
						rmoInspParentQuantity = item2.rmoInspParentQuantity,
						rmoInventoryUnitCost = item2.rmoInventoryUnitCost,
						rmoInventoryUnitCostForeign = item2.rmoInventoryUnitCostForeign,
						rmoInvParentQuantity = item2.rmoInvParentQuantity,
						rmoInvQuantityReceived = item2.rmoInvQuantityReceived,
						rmoClosed = item2.rmoClosed,
						rmoInspectionComplete = item2.rmoInspectionComplete,
						rmoJobReceivedComplete = item2.rmoJobReceivedComplete,
						rmoPostedToGl = item2.rmoPostedToGl,
						rmoReceivedComplete = item2.rmoReceivedComplete,
						rmoReversed = item2.rmoReversed,
						rmoJobAssemblyID = item2.rmoJobAssemblyID,
						rmoJobID = item2.rmoJobID,
						rmoJobMaterialComponentID = item2.rmoJobMaterialComponentID,
						rmoJobMaterialID = item2.rmoJobMaterialID,
						rmoJobParentQuantity = item2.rmoJobParentQuantity,
						rmoJobQuantityReceived = item2.rmoJobQuantityReceived,
						rmoPartBinID = item2.rmoPartBinID,
						rmoPartID = item2.rmoPartID,
						rmoPartRevisionID = item2.rmoPartRevisionID,
						rmoPartWarehouseLocationID = item2.rmoPartWarehouseLocationID,
						rmoPurchaseOrderComponentID = item2.rmoPurchaseOrderComponentID,
						rmoPurchaseOrderID = item2.rmoPurchaseOrderID,
						rmoPurchaseOrderLineID = item2.rmoPurchaseOrderLineID,
						rmoPurchaseUnitCost = item2.rmoPurchaseUnitCost,
						rmoPurchaseUnitCostForeign = item2.rmoPurchaseUnitCostForeign,
						rmoQuantityPerParent = item2.rmoQuantityPerParent,
						rmoQuantityToInspect = item2.rmoQuantityToInspect,
						rmoReceiptID = item2.rmoReceiptID,
						rmoReceiptLineID = item2.rmoReceiptLineID,
						rmoReverseReceiptComponentID = item2.rmoReverseReceiptComponentID,
						rmoReverseReceiptID = item2.rmoReverseReceiptID,
						rmoReverseReceiptLineID = item2.rmoReverseReceiptLineID,
						rmoRowVersion = item2.rmoRowVersion,
						rmoReceiptComponentID = item2.rmoReceiptComponentID,
						rmoUnitOfMeasure = item2.rmoUnitOfMeasure,
						rmoWeight = item2.rmoWeight,
						CustomFields = item2.CustomFields
					};
					allReceiptComponentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ReceiptComponents]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPReceiptComponentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allReceiptComponentsDto,
				RecordCount = allReceiptComponentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPReceiptComponentDto>> Process_GetReceiptComponent(Guid receiptComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPReceiptComponentDto receiptComponentDto = null;
		ERPResponseMessageDto<ERPReceiptComponentDto> result;
		try
		{
			IERPReceiptComponentRepository iERPReceiptComponentRepository = (base.ERPReceiptComponentRepository = new ERPReceiptComponentRepository(base.ApiClientContext));
			using (iERPReceiptComponentRepository)
			{
				ERPReceiptComponentInformationDto eRPReceiptComponentInformationDto = await base.ERPReceiptComponentRepository.GetReceiptComponent(receiptComponentId);
				receiptComponentDto = new ERPReceiptComponentDto
				{
					rmoAdditionalQuantity = eRPReceiptComponentInformationDto.rmoAdditionalQuantity,
					rmoConversionFactor = eRPReceiptComponentInformationDto.rmoConversionFactor,
					rmoCreatedBy = eRPReceiptComponentInformationDto.rmoCreatedBy,
					rmoCreatedDate = eRPReceiptComponentInformationDto.rmoCreatedDate,
					rmoDescription = eRPReceiptComponentInformationDto.rmoDescription,
					rmoUniqueID = eRPReceiptComponentInformationDto.rmoUniqueID,
					rmoExtendedCostBase = eRPReceiptComponentInformationDto.rmoExtendedCostBase,
					rmoExtendedCostForeign = eRPReceiptComponentInformationDto.rmoExtendedCostForeign,
					rmoInspParentQuantity = eRPReceiptComponentInformationDto.rmoInspParentQuantity,
					rmoInventoryUnitCost = eRPReceiptComponentInformationDto.rmoInventoryUnitCost,
					rmoInventoryUnitCostForeign = eRPReceiptComponentInformationDto.rmoInventoryUnitCostForeign,
					rmoInvParentQuantity = eRPReceiptComponentInformationDto.rmoInvParentQuantity,
					rmoInvQuantityReceived = eRPReceiptComponentInformationDto.rmoInvQuantityReceived,
					rmoClosed = eRPReceiptComponentInformationDto.rmoClosed,
					rmoInspectionComplete = eRPReceiptComponentInformationDto.rmoInspectionComplete,
					rmoJobReceivedComplete = eRPReceiptComponentInformationDto.rmoJobReceivedComplete,
					rmoPostedToGl = eRPReceiptComponentInformationDto.rmoPostedToGl,
					rmoReceivedComplete = eRPReceiptComponentInformationDto.rmoReceivedComplete,
					rmoReversed = eRPReceiptComponentInformationDto.rmoReversed,
					rmoJobAssemblyID = eRPReceiptComponentInformationDto.rmoJobAssemblyID,
					rmoJobID = eRPReceiptComponentInformationDto.rmoJobID,
					rmoJobMaterialComponentID = eRPReceiptComponentInformationDto.rmoJobMaterialComponentID,
					rmoJobMaterialID = eRPReceiptComponentInformationDto.rmoJobMaterialID,
					rmoJobParentQuantity = eRPReceiptComponentInformationDto.rmoJobParentQuantity,
					rmoJobQuantityReceived = eRPReceiptComponentInformationDto.rmoJobQuantityReceived,
					rmoPartBinID = eRPReceiptComponentInformationDto.rmoPartBinID,
					rmoPartID = eRPReceiptComponentInformationDto.rmoPartID,
					rmoPartRevisionID = eRPReceiptComponentInformationDto.rmoPartRevisionID,
					rmoPartWarehouseLocationID = eRPReceiptComponentInformationDto.rmoPartWarehouseLocationID,
					rmoPurchaseOrderComponentID = eRPReceiptComponentInformationDto.rmoPurchaseOrderComponentID,
					rmoPurchaseOrderID = eRPReceiptComponentInformationDto.rmoPurchaseOrderID,
					rmoPurchaseOrderLineID = eRPReceiptComponentInformationDto.rmoPurchaseOrderLineID,
					rmoPurchaseUnitCost = eRPReceiptComponentInformationDto.rmoPurchaseUnitCost,
					rmoPurchaseUnitCostForeign = eRPReceiptComponentInformationDto.rmoPurchaseUnitCostForeign,
					rmoQuantityPerParent = eRPReceiptComponentInformationDto.rmoQuantityPerParent,
					rmoQuantityToInspect = eRPReceiptComponentInformationDto.rmoQuantityToInspect,
					rmoReceiptID = eRPReceiptComponentInformationDto.rmoReceiptID,
					rmoReceiptLineID = eRPReceiptComponentInformationDto.rmoReceiptLineID,
					rmoReverseReceiptComponentID = eRPReceiptComponentInformationDto.rmoReverseReceiptComponentID,
					rmoReverseReceiptID = eRPReceiptComponentInformationDto.rmoReverseReceiptID,
					rmoReverseReceiptLineID = eRPReceiptComponentInformationDto.rmoReverseReceiptLineID,
					rmoRowVersion = eRPReceiptComponentInformationDto.rmoRowVersion,
					rmoReceiptComponentID = eRPReceiptComponentInformationDto.rmoReceiptComponentID,
					rmoUnitOfMeasure = eRPReceiptComponentInformationDto.rmoUnitOfMeasure,
					rmoWeight = eRPReceiptComponentInformationDto.rmoWeight,
					CustomFields = eRPReceiptComponentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ReceiptComponents []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPReceiptComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = receiptComponentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPReceiptComponentDto>> Process_PutReceiptComponent(ERPReceiptComponentDto receiptComponent)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPReceiptComponentDto createdObject = null;
		ERPResponseMessageDto<ERPReceiptComponentDto> result;
		try
		{
			IERPReceiptComponentRepository iERPReceiptComponentRepository = (base.ERPReceiptComponentRepository = new ERPReceiptComponentRepository(base.ApiClientContext));
			using (iERPReceiptComponentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPReceiptComponentRepository.SaveReceiptComponent(receiptComponent);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPReceiptComponentInformationDto eRPReceiptComponentInformationDto = await base.ERPReceiptComponentRepository.GetReceiptComponent(receiptComponent.rmoUniqueID);
					createdObject = new ERPReceiptComponentDto
					{
						rmoAdditionalQuantity = eRPReceiptComponentInformationDto.rmoAdditionalQuantity,
						rmoConversionFactor = eRPReceiptComponentInformationDto.rmoConversionFactor,
						rmoCreatedBy = eRPReceiptComponentInformationDto.rmoCreatedBy,
						rmoCreatedDate = eRPReceiptComponentInformationDto.rmoCreatedDate,
						rmoDescription = eRPReceiptComponentInformationDto.rmoDescription,
						rmoUniqueID = eRPReceiptComponentInformationDto.rmoUniqueID,
						rmoExtendedCostBase = eRPReceiptComponentInformationDto.rmoExtendedCostBase,
						rmoExtendedCostForeign = eRPReceiptComponentInformationDto.rmoExtendedCostForeign,
						rmoInspParentQuantity = eRPReceiptComponentInformationDto.rmoInspParentQuantity,
						rmoInventoryUnitCost = eRPReceiptComponentInformationDto.rmoInventoryUnitCost,
						rmoInventoryUnitCostForeign = eRPReceiptComponentInformationDto.rmoInventoryUnitCostForeign,
						rmoInvParentQuantity = eRPReceiptComponentInformationDto.rmoInvParentQuantity,
						rmoInvQuantityReceived = eRPReceiptComponentInformationDto.rmoInvQuantityReceived,
						rmoClosed = eRPReceiptComponentInformationDto.rmoClosed,
						rmoInspectionComplete = eRPReceiptComponentInformationDto.rmoInspectionComplete,
						rmoJobReceivedComplete = eRPReceiptComponentInformationDto.rmoJobReceivedComplete,
						rmoPostedToGl = eRPReceiptComponentInformationDto.rmoPostedToGl,
						rmoReceivedComplete = eRPReceiptComponentInformationDto.rmoReceivedComplete,
						rmoReversed = eRPReceiptComponentInformationDto.rmoReversed,
						rmoJobAssemblyID = eRPReceiptComponentInformationDto.rmoJobAssemblyID,
						rmoJobID = eRPReceiptComponentInformationDto.rmoJobID,
						rmoJobMaterialComponentID = eRPReceiptComponentInformationDto.rmoJobMaterialComponentID,
						rmoJobMaterialID = eRPReceiptComponentInformationDto.rmoJobMaterialID,
						rmoJobParentQuantity = eRPReceiptComponentInformationDto.rmoJobParentQuantity,
						rmoJobQuantityReceived = eRPReceiptComponentInformationDto.rmoJobQuantityReceived,
						rmoPartBinID = eRPReceiptComponentInformationDto.rmoPartBinID,
						rmoPartID = eRPReceiptComponentInformationDto.rmoPartID,
						rmoPartRevisionID = eRPReceiptComponentInformationDto.rmoPartRevisionID,
						rmoPartWarehouseLocationID = eRPReceiptComponentInformationDto.rmoPartWarehouseLocationID,
						rmoPurchaseOrderComponentID = eRPReceiptComponentInformationDto.rmoPurchaseOrderComponentID,
						rmoPurchaseOrderID = eRPReceiptComponentInformationDto.rmoPurchaseOrderID,
						rmoPurchaseOrderLineID = eRPReceiptComponentInformationDto.rmoPurchaseOrderLineID,
						rmoPurchaseUnitCost = eRPReceiptComponentInformationDto.rmoPurchaseUnitCost,
						rmoPurchaseUnitCostForeign = eRPReceiptComponentInformationDto.rmoPurchaseUnitCostForeign,
						rmoQuantityPerParent = eRPReceiptComponentInformationDto.rmoQuantityPerParent,
						rmoQuantityToInspect = eRPReceiptComponentInformationDto.rmoQuantityToInspect,
						rmoReceiptID = eRPReceiptComponentInformationDto.rmoReceiptID,
						rmoReceiptLineID = eRPReceiptComponentInformationDto.rmoReceiptLineID,
						rmoReverseReceiptComponentID = eRPReceiptComponentInformationDto.rmoReverseReceiptComponentID,
						rmoReverseReceiptID = eRPReceiptComponentInformationDto.rmoReverseReceiptID,
						rmoReverseReceiptLineID = eRPReceiptComponentInformationDto.rmoReverseReceiptLineID,
						rmoRowVersion = eRPReceiptComponentInformationDto.rmoRowVersion,
						rmoReceiptComponentID = eRPReceiptComponentInformationDto.rmoReceiptComponentID,
						rmoUnitOfMeasure = eRPReceiptComponentInformationDto.rmoUnitOfMeasure,
						rmoWeight = eRPReceiptComponentInformationDto.rmoWeight,
						CustomFields = eRPReceiptComponentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ReceiptComponent [{receiptComponent.rmoUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPReceiptComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteReceiptComponent(Guid receiptComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPReceiptComponentRepository iERPReceiptComponentRepository = (base.ERPReceiptComponentRepository = new ERPReceiptComponentRepository(base.ApiClientContext));
		using (iERPReceiptComponentRepository)
		{
			if (!(await base.ERPReceiptComponentRepository.DoesReceiptComponentExist(receiptComponentId)))
			{
				base.ErrorsList.Add($"ReceiptComponent [{receiptComponentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPReceiptComponentInformationDto eRPReceiptComponentInformationDto = await base.ERPReceiptComponentRepository.GetReceiptComponent(receiptComponentId);
				string text = await base.ERPReceiptComponentRepository.WhereUsed("ReceiptComponents", new object[3] { eRPReceiptComponentInformationDto.rmoReceiptID, eRPReceiptComponentInformationDto.rmoReceiptLineID, eRPReceiptComponentInformationDto.rmoReceiptComponentID }, new object[3] { "rmoReceiptID", "rmoReceiptLineID", "rmoReceiptComponentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ReceiptComponent cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPReceiptComponentDto>> Process_DeleteReceiptComponent(Guid receiptComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPReceiptComponentDto> result;
		try
		{
			IERPReceiptComponentRepository iERPReceiptComponentRepository = (base.ERPReceiptComponentRepository = new ERPReceiptComponentRepository(base.ApiClientContext));
			using (iERPReceiptComponentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPReceiptComponentRepository.DeleteRowFromTable("ReceiptComponents", "rmo", receiptComponentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ReceiptComponent [{receiptComponentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPReceiptComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPReceiptComponentDto()
			};
		}
		return result;
	}
}
