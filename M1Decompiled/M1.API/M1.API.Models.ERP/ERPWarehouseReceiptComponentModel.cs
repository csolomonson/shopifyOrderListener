using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPWarehouseReceiptComponentModel : ERPBaseModel, IERPWarehouseReceiptComponentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseReceiptComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPWarehouseReceiptComponentRepository iERPWarehouseReceiptComponentRepository = (base.ERPWarehouseReceiptComponentRepository = new ERPWarehouseReceiptComponentRepository(base.ApiClientContext));
		using (iERPWarehouseReceiptComponentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPWarehouseReceiptComponentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPWarehouseReceiptComponentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPWarehouseReceiptComponentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPWarehouseReceiptComponentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetWarehouseReceiptComponent(Guid warehouseReceiptComponentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseReceiptComponentRepository iERPWarehouseReceiptComponentRepository = (base.ERPWarehouseReceiptComponentRepository = new ERPWarehouseReceiptComponentRepository(base.ApiClientContext));
		using (iERPWarehouseReceiptComponentRepository)
		{
			if (!(await base.ERPWarehouseReceiptComponentRepository.DoesWarehouseReceiptComponentExist(warehouseReceiptComponentId)))
			{
				errorsList.Add($"WarehouseReceiptComponent [{warehouseReceiptComponentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutWarehouseReceiptComponent(ERPWarehouseReceiptComponentDto warehouseReceiptComponent)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseReceiptComponentRepository iERPWarehouseReceiptComponentRepository = (base.ERPWarehouseReceiptComponentRepository = new ERPWarehouseReceiptComponentRepository(base.ApiClientContext));
		using (iERPWarehouseReceiptComponentRepository)
		{
			if (!string.IsNullOrWhiteSpace(warehouseReceiptComponent.wroWarehouseReceiptID) && !(await base.ERPWarehouseReceiptComponentRepository.DoesRecordExistInTableUsingKeys("WarehouseReceipts", new object[1] { "WRPWAREHOUSERECEIPTID" }, new object[1] { warehouseReceiptComponent.wroWarehouseReceiptID })))
			{
				errorsList.Add("wroWarehouseReceiptID [" + warehouseReceiptComponent.wroWarehouseReceiptID + "] not found.");
			}
			if (warehouseReceiptComponent.wroWarehouseReceiptLineID > 0 && !(await base.ERPWarehouseReceiptComponentRepository.DoesRecordExistInTableUsingKeys("WarehouseReceiptLines", new object[2] { "WRLWAREHOUSERECEIPTID", "WRLWAREHOUSERECEIPTLINEID" }, new object[2] { warehouseReceiptComponent.wroWarehouseReceiptID, warehouseReceiptComponent.wroWarehouseReceiptLineID })))
			{
				errorsList.Add($"wroWarehouseReceiptLineID [{warehouseReceiptComponent.wroWarehouseReceiptLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseReceiptComponent.wroPartID) && !(await base.ERPWarehouseReceiptComponentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { warehouseReceiptComponent.wroPartID })))
			{
				errorsList.Add("wroPartID [" + warehouseReceiptComponent.wroPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseReceiptComponent.wroPartRevisionID) && !(await base.ERPWarehouseReceiptComponentRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { warehouseReceiptComponent.wroPartID, warehouseReceiptComponent.wroPartRevisionID })))
			{
				errorsList.Add("wroPartRevisionID [" + warehouseReceiptComponent.wroPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseReceiptComponent.wroSourcePartBinID) && !(await base.ERPWarehouseReceiptComponentRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { warehouseReceiptComponent.wroPartID, warehouseReceiptComponent.wroPartRevisionID, warehouseReceiptComponent.wroSourceWarehouseID, warehouseReceiptComponent.wroSourcePartBinID })))
			{
				errorsList.Add("wroSourcePartBinID [" + warehouseReceiptComponent.wroSourcePartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseReceiptComponent.wroWarehouseTransferID) && !(await base.ERPWarehouseReceiptComponentRepository.DoesRecordExistInTableUsingKeys("WarehouseTransfers", new object[1] { "MWPWAREHOUSETRANSFERID" }, new object[1] { warehouseReceiptComponent.wroWarehouseTransferID })))
			{
				errorsList.Add("wroWarehouseTransferID [" + warehouseReceiptComponent.wroWarehouseTransferID + "] not found.");
			}
			if (warehouseReceiptComponent.wroWarehouseTransferLineID > 0 && !(await base.ERPWarehouseReceiptComponentRepository.DoesRecordExistInTableUsingKeys("WarehouseTransferLines", new object[2] { "MWLWAREHOUSETRANSFERID", "MWLWAREHOUSETRANSFERLINEID" }, new object[2] { warehouseReceiptComponent.wroWarehouseTransferID, warehouseReceiptComponent.wroWarehouseTransferLineID })))
			{
				errorsList.Add($"wroWarehouseTransferLineID [{warehouseReceiptComponent.wroWarehouseTransferLineID}] not found.");
			}
			if (warehouseReceiptComponent.wroWarehouseTransComponentID > 0 && !(await base.ERPWarehouseReceiptComponentRepository.DoesRecordExistInTableUsingKeys("WarehouseTransferComponents", new object[3] { "MWOWAREHOUSETRANSFERID", "MWOWAREHOUSETRANSFERLINEID", "MWOWAREHOUSETRANSCOMPONENTID" }, new object[3] { warehouseReceiptComponent.wroWarehouseTransferID, warehouseReceiptComponent.wroWarehouseTransferLineID, warehouseReceiptComponent.wroWarehouseTransComponentID })))
			{
				errorsList.Add($"wroWarehouseTransComponentID [{warehouseReceiptComponent.wroWarehouseTransComponentID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseReceiptComponent.wroWarehouseRequisitionID) && !(await base.ERPWarehouseReceiptComponentRepository.DoesRecordExistInTableUsingKeys("WarehouseRequisitions", new object[1] { "WQPWAREHOUSEREQUISITIONID" }, new object[1] { warehouseReceiptComponent.wroWarehouseRequisitionID })))
			{
				errorsList.Add("wroWarehouseRequisitionID [" + warehouseReceiptComponent.wroWarehouseRequisitionID + "] not found.");
			}
			if (warehouseReceiptComponent.wroWarehouseRequisitionLineID > 0 && !(await base.ERPWarehouseReceiptComponentRepository.DoesRecordExistInTableUsingKeys("WarehouseRequisitionLines", new object[2] { "WQLWAREHOUSEREQUISITIONID", "WQLWAREHOUSEREQUISITIONLINEID" }, new object[2] { warehouseReceiptComponent.wroWarehouseRequisitionID, warehouseReceiptComponent.wroWarehouseRequisitionLineID })))
			{
				errorsList.Add($"wroWarehouseRequisitionLineID [{warehouseReceiptComponent.wroWarehouseRequisitionLineID}] not found.");
			}
			if (warehouseReceiptComponent.wroWarehouseReqComponentID > 0 && !(await base.ERPWarehouseReceiptComponentRepository.DoesRecordExistInTableUsingKeys("WarehouseRequisitionComponents", new object[3] { "WQOWAREHOUSEREQUISITIONID", "WQOWAREHOUSEREQUISITIONLINEID", "WQOWAREHOUSEREQCOMPONENTID" }, new object[3] { warehouseReceiptComponent.wroWarehouseRequisitionID, warehouseReceiptComponent.wroWarehouseRequisitionLineID, warehouseReceiptComponent.wroWarehouseReqComponentID })))
			{
				errorsList.Add($"wroWarehouseReqComponentID [{warehouseReceiptComponent.wroWarehouseReqComponentID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPWarehouseReceiptComponentDto>>> Process_GetAllWarehouseReceiptComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPWarehouseReceiptComponentDto> allWarehouseReceiptComponentsDto = new List<ERPWarehouseReceiptComponentDto>();
		ERPResponseMessageDto<IList<ERPWarehouseReceiptComponentDto>> result;
		try
		{
			IERPWarehouseReceiptComponentRepository iERPWarehouseReceiptComponentRepository = (base.ERPWarehouseReceiptComponentRepository = new ERPWarehouseReceiptComponentRepository(base.ApiClientContext));
			using (iERPWarehouseReceiptComponentRepository)
			{
				foreach (ERPWarehouseReceiptComponentInformationDto item2 in await base.ERPWarehouseReceiptComponentRepository.GetAllWarehouseReceiptComponents(pageSize, pageNumber, filter, orderBy))
				{
					ERPWarehouseReceiptComponentDto item = new ERPWarehouseReceiptComponentDto
					{
						wroAdditionalQuantity = item2.wroAdditionalQuantity,
						wroCreatedBy = item2.wroCreatedBy,
						wroCreatedDate = item2.wroCreatedDate,
						wroDescription = item2.wroDescription,
						wroDestinationPartBinID = item2.wroDestinationPartBinID,
						wroDestinationWarehouseID = item2.wroDestinationWarehouseID,
						wroUniqueID = item2.wroUniqueID,
						wroClosed = item2.wroClosed,
						wroPosted = item2.wroPosted,
						wroReceivedComplete = item2.wroReceivedComplete,
						wroReversed = item2.wroReversed,
						wroParentQuantity = item2.wroParentQuantity,
						wroPartID = item2.wroPartID,
						wroPartRevisionID = item2.wroPartRevisionID,
						wroQuantityPerParent = item2.wroQuantityPerParent,
						wroQuantityReceived = item2.wroQuantityReceived,
						wroReverseWHReceiptCompID = item2.wroReverseWHReceiptCompID,
						wroReverseWHReceiptID = item2.wroReverseWHReceiptID,
						wroReverseWHReceiptLineID = item2.wroReverseWHReceiptLineID,
						wroRowVersion = item2.wroRowVersion,
						wroWarehouseReceiptComponentID = item2.wroWarehouseReceiptComponentID,
						wroSourcePartBinID = item2.wroSourcePartBinID,
						wroSourceTableName = item2.wroSourceTableName,
						wroSourceTableUniqueID = item2.wroSourceTableUniqueID,
						wroSourceWarehouseID = item2.wroSourceWarehouseID,
						wroUnitOfMeasure = item2.wroUnitOfMeasure,
						wroWarehouseReceiptID = item2.wroWarehouseReceiptID,
						wroWarehouseReceiptLineID = item2.wroWarehouseReceiptLineID,
						wroWarehouseReqComponentID = item2.wroWarehouseReqComponentID,
						wroWarehouseRequisitionID = item2.wroWarehouseRequisitionID,
						wroWarehouseRequisitionLineID = item2.wroWarehouseRequisitionLineID,
						wroWarehouseTransComponentID = item2.wroWarehouseTransComponentID,
						wroWarehouseTransferID = item2.wroWarehouseTransferID,
						wroWarehouseTransferLineID = item2.wroWarehouseTransferLineID,
						wroWeight = item2.wroWeight,
						CustomFields = item2.CustomFields
					};
					allWarehouseReceiptComponentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all WarehouseReceiptComponents]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPWarehouseReceiptComponentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allWarehouseReceiptComponentsDto,
				RecordCount = allWarehouseReceiptComponentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseReceiptComponentDto>> Process_GetWarehouseReceiptComponent(Guid warehouseReceiptComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPWarehouseReceiptComponentDto warehouseReceiptComponentDto = null;
		ERPResponseMessageDto<ERPWarehouseReceiptComponentDto> result;
		try
		{
			IERPWarehouseReceiptComponentRepository iERPWarehouseReceiptComponentRepository = (base.ERPWarehouseReceiptComponentRepository = new ERPWarehouseReceiptComponentRepository(base.ApiClientContext));
			using (iERPWarehouseReceiptComponentRepository)
			{
				ERPWarehouseReceiptComponentInformationDto eRPWarehouseReceiptComponentInformationDto = await base.ERPWarehouseReceiptComponentRepository.GetWarehouseReceiptComponent(warehouseReceiptComponentId);
				warehouseReceiptComponentDto = new ERPWarehouseReceiptComponentDto
				{
					wroAdditionalQuantity = eRPWarehouseReceiptComponentInformationDto.wroAdditionalQuantity,
					wroCreatedBy = eRPWarehouseReceiptComponentInformationDto.wroCreatedBy,
					wroCreatedDate = eRPWarehouseReceiptComponentInformationDto.wroCreatedDate,
					wroDescription = eRPWarehouseReceiptComponentInformationDto.wroDescription,
					wroDestinationPartBinID = eRPWarehouseReceiptComponentInformationDto.wroDestinationPartBinID,
					wroDestinationWarehouseID = eRPWarehouseReceiptComponentInformationDto.wroDestinationWarehouseID,
					wroUniqueID = eRPWarehouseReceiptComponentInformationDto.wroUniqueID,
					wroClosed = eRPWarehouseReceiptComponentInformationDto.wroClosed,
					wroPosted = eRPWarehouseReceiptComponentInformationDto.wroPosted,
					wroReceivedComplete = eRPWarehouseReceiptComponentInformationDto.wroReceivedComplete,
					wroReversed = eRPWarehouseReceiptComponentInformationDto.wroReversed,
					wroParentQuantity = eRPWarehouseReceiptComponentInformationDto.wroParentQuantity,
					wroPartID = eRPWarehouseReceiptComponentInformationDto.wroPartID,
					wroPartRevisionID = eRPWarehouseReceiptComponentInformationDto.wroPartRevisionID,
					wroQuantityPerParent = eRPWarehouseReceiptComponentInformationDto.wroQuantityPerParent,
					wroQuantityReceived = eRPWarehouseReceiptComponentInformationDto.wroQuantityReceived,
					wroReverseWHReceiptCompID = eRPWarehouseReceiptComponentInformationDto.wroReverseWHReceiptCompID,
					wroReverseWHReceiptID = eRPWarehouseReceiptComponentInformationDto.wroReverseWHReceiptID,
					wroReverseWHReceiptLineID = eRPWarehouseReceiptComponentInformationDto.wroReverseWHReceiptLineID,
					wroRowVersion = eRPWarehouseReceiptComponentInformationDto.wroRowVersion,
					wroWarehouseReceiptComponentID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseReceiptComponentID,
					wroSourcePartBinID = eRPWarehouseReceiptComponentInformationDto.wroSourcePartBinID,
					wroSourceTableName = eRPWarehouseReceiptComponentInformationDto.wroSourceTableName,
					wroSourceTableUniqueID = eRPWarehouseReceiptComponentInformationDto.wroSourceTableUniqueID,
					wroSourceWarehouseID = eRPWarehouseReceiptComponentInformationDto.wroSourceWarehouseID,
					wroUnitOfMeasure = eRPWarehouseReceiptComponentInformationDto.wroUnitOfMeasure,
					wroWarehouseReceiptID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseReceiptID,
					wroWarehouseReceiptLineID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseReceiptLineID,
					wroWarehouseReqComponentID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseReqComponentID,
					wroWarehouseRequisitionID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseRequisitionID,
					wroWarehouseRequisitionLineID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseRequisitionLineID,
					wroWarehouseTransComponentID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseTransComponentID,
					wroWarehouseTransferID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseTransferID,
					wroWarehouseTransferLineID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseTransferLineID,
					wroWeight = eRPWarehouseReceiptComponentInformationDto.wroWeight,
					CustomFields = eRPWarehouseReceiptComponentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the WarehouseReceiptComponents []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseReceiptComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = warehouseReceiptComponentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseReceiptComponentDto>> Process_PutWarehouseReceiptComponent(ERPWarehouseReceiptComponentDto warehouseReceiptComponent)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPWarehouseReceiptComponentDto createdObject = null;
		ERPResponseMessageDto<ERPWarehouseReceiptComponentDto> result;
		try
		{
			IERPWarehouseReceiptComponentRepository iERPWarehouseReceiptComponentRepository = (base.ERPWarehouseReceiptComponentRepository = new ERPWarehouseReceiptComponentRepository(base.ApiClientContext));
			using (iERPWarehouseReceiptComponentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPWarehouseReceiptComponentRepository.SaveWarehouseReceiptComponent(warehouseReceiptComponent);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPWarehouseReceiptComponentInformationDto eRPWarehouseReceiptComponentInformationDto = await base.ERPWarehouseReceiptComponentRepository.GetWarehouseReceiptComponent(warehouseReceiptComponent.wroUniqueID);
					createdObject = new ERPWarehouseReceiptComponentDto
					{
						wroAdditionalQuantity = eRPWarehouseReceiptComponentInformationDto.wroAdditionalQuantity,
						wroCreatedBy = eRPWarehouseReceiptComponentInformationDto.wroCreatedBy,
						wroCreatedDate = eRPWarehouseReceiptComponentInformationDto.wroCreatedDate,
						wroDescription = eRPWarehouseReceiptComponentInformationDto.wroDescription,
						wroDestinationPartBinID = eRPWarehouseReceiptComponentInformationDto.wroDestinationPartBinID,
						wroDestinationWarehouseID = eRPWarehouseReceiptComponentInformationDto.wroDestinationWarehouseID,
						wroUniqueID = eRPWarehouseReceiptComponentInformationDto.wroUniqueID,
						wroClosed = eRPWarehouseReceiptComponentInformationDto.wroClosed,
						wroPosted = eRPWarehouseReceiptComponentInformationDto.wroPosted,
						wroReceivedComplete = eRPWarehouseReceiptComponentInformationDto.wroReceivedComplete,
						wroReversed = eRPWarehouseReceiptComponentInformationDto.wroReversed,
						wroParentQuantity = eRPWarehouseReceiptComponentInformationDto.wroParentQuantity,
						wroPartID = eRPWarehouseReceiptComponentInformationDto.wroPartID,
						wroPartRevisionID = eRPWarehouseReceiptComponentInformationDto.wroPartRevisionID,
						wroQuantityPerParent = eRPWarehouseReceiptComponentInformationDto.wroQuantityPerParent,
						wroQuantityReceived = eRPWarehouseReceiptComponentInformationDto.wroQuantityReceived,
						wroReverseWHReceiptCompID = eRPWarehouseReceiptComponentInformationDto.wroReverseWHReceiptCompID,
						wroReverseWHReceiptID = eRPWarehouseReceiptComponentInformationDto.wroReverseWHReceiptID,
						wroReverseWHReceiptLineID = eRPWarehouseReceiptComponentInformationDto.wroReverseWHReceiptLineID,
						wroRowVersion = eRPWarehouseReceiptComponentInformationDto.wroRowVersion,
						wroWarehouseReceiptComponentID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseReceiptComponentID,
						wroSourcePartBinID = eRPWarehouseReceiptComponentInformationDto.wroSourcePartBinID,
						wroSourceTableName = eRPWarehouseReceiptComponentInformationDto.wroSourceTableName,
						wroSourceTableUniqueID = eRPWarehouseReceiptComponentInformationDto.wroSourceTableUniqueID,
						wroSourceWarehouseID = eRPWarehouseReceiptComponentInformationDto.wroSourceWarehouseID,
						wroUnitOfMeasure = eRPWarehouseReceiptComponentInformationDto.wroUnitOfMeasure,
						wroWarehouseReceiptID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseReceiptID,
						wroWarehouseReceiptLineID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseReceiptLineID,
						wroWarehouseReqComponentID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseReqComponentID,
						wroWarehouseRequisitionID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseRequisitionID,
						wroWarehouseRequisitionLineID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseRequisitionLineID,
						wroWarehouseTransComponentID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseTransComponentID,
						wroWarehouseTransferID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseTransferID,
						wroWarehouseTransferLineID = eRPWarehouseReceiptComponentInformationDto.wroWarehouseTransferLineID,
						wroWeight = eRPWarehouseReceiptComponentInformationDto.wroWeight,
						CustomFields = eRPWarehouseReceiptComponentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing WarehouseReceiptComponent [{warehouseReceiptComponent.wroUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseReceiptComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseReceiptComponent(Guid warehouseReceiptComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseReceiptComponentRepository iERPWarehouseReceiptComponentRepository = (base.ERPWarehouseReceiptComponentRepository = new ERPWarehouseReceiptComponentRepository(base.ApiClientContext));
		using (iERPWarehouseReceiptComponentRepository)
		{
			if (!(await base.ERPWarehouseReceiptComponentRepository.DoesWarehouseReceiptComponentExist(warehouseReceiptComponentId)))
			{
				base.ErrorsList.Add($"WarehouseReceiptComponent [{warehouseReceiptComponentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPWarehouseReceiptComponentInformationDto eRPWarehouseReceiptComponentInformationDto = await base.ERPWarehouseReceiptComponentRepository.GetWarehouseReceiptComponent(warehouseReceiptComponentId);
				string text = await base.ERPWarehouseReceiptComponentRepository.WhereUsed("WarehouseReceiptComponents", new object[3] { eRPWarehouseReceiptComponentInformationDto.wroWarehouseReceiptID, eRPWarehouseReceiptComponentInformationDto.wroWarehouseReceiptLineID, eRPWarehouseReceiptComponentInformationDto.wroWarehouseReceiptComponentID }, new object[3] { "wroWarehouseReceiptID", "wroWarehouseReceiptLineID", "wroWarehouseReceiptComponentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("WarehouseReceiptComponent cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseReceiptComponentDto>> Process_DeleteWarehouseReceiptComponent(Guid warehouseReceiptComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPWarehouseReceiptComponentDto> result;
		try
		{
			IERPWarehouseReceiptComponentRepository iERPWarehouseReceiptComponentRepository = (base.ERPWarehouseReceiptComponentRepository = new ERPWarehouseReceiptComponentRepository(base.ApiClientContext));
			using (iERPWarehouseReceiptComponentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPWarehouseReceiptComponentRepository.DeleteRowFromTable("WarehouseReceiptComponents", "wro", warehouseReceiptComponentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of WarehouseReceiptComponent [{warehouseReceiptComponentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseReceiptComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPWarehouseReceiptComponentDto()
			};
		}
		return result;
	}
}
