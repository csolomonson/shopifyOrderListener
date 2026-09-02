using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPWarehouseReceiptLineModel : ERPBaseModel, IERPWarehouseReceiptLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseReceiptLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPWarehouseReceiptLineRepository iERPWarehouseReceiptLineRepository = (base.ERPWarehouseReceiptLineRepository = new ERPWarehouseReceiptLineRepository(base.ApiClientContext));
		using (iERPWarehouseReceiptLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPWarehouseReceiptLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPWarehouseReceiptLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPWarehouseReceiptLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPWarehouseReceiptLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetWarehouseReceiptLine(Guid warehouseReceiptLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseReceiptLineRepository iERPWarehouseReceiptLineRepository = (base.ERPWarehouseReceiptLineRepository = new ERPWarehouseReceiptLineRepository(base.ApiClientContext));
		using (iERPWarehouseReceiptLineRepository)
		{
			if (!(await base.ERPWarehouseReceiptLineRepository.DoesWarehouseReceiptLineExist(warehouseReceiptLineId)))
			{
				errorsList.Add($"WarehouseReceiptLine [{warehouseReceiptLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutWarehouseReceiptLine(ERPWarehouseReceiptLineDto warehouseReceiptLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseReceiptLineRepository iERPWarehouseReceiptLineRepository = (base.ERPWarehouseReceiptLineRepository = new ERPWarehouseReceiptLineRepository(base.ApiClientContext));
		using (iERPWarehouseReceiptLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(warehouseReceiptLine.wrlWarehouseReceiptID) && !(await base.ERPWarehouseReceiptLineRepository.DoesRecordExistInTableUsingKeys("WarehouseReceipts", new object[1] { "WRPWAREHOUSERECEIPTID" }, new object[1] { warehouseReceiptLine.wrlWarehouseReceiptID })))
			{
				errorsList.Add("wrlWarehouseReceiptID [" + warehouseReceiptLine.wrlWarehouseReceiptID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseReceiptLine.wrlPartID) && !(await base.ERPWarehouseReceiptLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { warehouseReceiptLine.wrlPartID })))
			{
				errorsList.Add("wrlPartID [" + warehouseReceiptLine.wrlPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseReceiptLine.wrlPartRevisionID) && !(await base.ERPWarehouseReceiptLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { warehouseReceiptLine.wrlPartID, warehouseReceiptLine.wrlPartRevisionID })))
			{
				errorsList.Add("wrlPartRevisionID [" + warehouseReceiptLine.wrlPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseReceiptLine.wrlDestinationWarehouseID) && !(await base.ERPWarehouseReceiptLineRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { warehouseReceiptLine.wrlPartID, warehouseReceiptLine.wrlPartRevisionID, warehouseReceiptLine.wrlDestinationWarehouseID })))
			{
				errorsList.Add("wrlDestinationWarehouseID [" + warehouseReceiptLine.wrlDestinationWarehouseID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseReceiptLine.wrlWarehouseRequisitionID) && !(await base.ERPWarehouseReceiptLineRepository.DoesRecordExistInTableUsingKeys("WarehouseRequisitions", new object[1] { "WQPWAREHOUSEREQUISITIONID" }, new object[1] { warehouseReceiptLine.wrlWarehouseRequisitionID })))
			{
				errorsList.Add("wrlWarehouseRequisitionID [" + warehouseReceiptLine.wrlWarehouseRequisitionID + "] not found.");
			}
			if (warehouseReceiptLine.wrlWarehouseRequisitionLineID > 0 && !(await base.ERPWarehouseReceiptLineRepository.DoesRecordExistInTableUsingKeys("WarehouseRequisitionLines", new object[2] { "WQLWAREHOUSEREQUISITIONID", "WQLWAREHOUSEREQUISITIONLINEID" }, new object[2] { warehouseReceiptLine.wrlWarehouseRequisitionID, warehouseReceiptLine.wrlWarehouseRequisitionLineID })))
			{
				errorsList.Add($"wrlWarehouseRequisitionLineID [{warehouseReceiptLine.wrlWarehouseRequisitionLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseReceiptLine.wrlWarehouseTransferID) && !(await base.ERPWarehouseReceiptLineRepository.DoesRecordExistInTableUsingKeys("WarehouseTransfers", new object[1] { "MWPWAREHOUSETRANSFERID" }, new object[1] { warehouseReceiptLine.wrlWarehouseTransferID })))
			{
				errorsList.Add("wrlWarehouseTransferID [" + warehouseReceiptLine.wrlWarehouseTransferID + "] not found.");
			}
			if (warehouseReceiptLine.wrlWarehouseTransferLineID > 0 && !(await base.ERPWarehouseReceiptLineRepository.DoesRecordExistInTableUsingKeys("WarehouseTransferLines", new object[2] { "MWLWAREHOUSETRANSFERID", "MWLWAREHOUSETRANSFERLINEID" }, new object[2] { warehouseReceiptLine.wrlWarehouseTransferID, warehouseReceiptLine.wrlWarehouseTransferLineID })))
			{
				errorsList.Add($"wrlWarehouseTransferLineID [{warehouseReceiptLine.wrlWarehouseTransferLineID}] not found.");
			}
			if (warehouseReceiptLine.wrlReverseWHReceiptLineID > 0 && !(await base.ERPWarehouseReceiptLineRepository.DoesRecordExistInTableUsingKeys("WarehouseReceiptLines", new object[2] { "WRLWAREHOUSERECEIPTID", "WRLWAREHOUSERECEIPTLINEID" }, new object[2] { warehouseReceiptLine.wrlReverseWHReceiptID, warehouseReceiptLine.wrlReverseWHReceiptLineID })))
			{
				errorsList.Add($"wrlReverseWHReceiptLineID [{warehouseReceiptLine.wrlReverseWHReceiptLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(warehouseReceiptLine.wrlReverseWHReceiptID) && !(await base.ERPWarehouseReceiptLineRepository.DoesRecordExistInTableUsingKeys("WarehouseReceipts", new object[1] { "WRPWAREHOUSERECEIPTID" }, new object[1] { warehouseReceiptLine.wrlReverseWHReceiptID })))
			{
				errorsList.Add("wrlReverseWHReceiptID [" + warehouseReceiptLine.wrlReverseWHReceiptID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPWarehouseReceiptLineDto>>> Process_GetAllWarehouseReceiptLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPWarehouseReceiptLineDto> allWarehouseReceiptLinesDto = new List<ERPWarehouseReceiptLineDto>();
		ERPResponseMessageDto<IList<ERPWarehouseReceiptLineDto>> result;
		try
		{
			IERPWarehouseReceiptLineRepository iERPWarehouseReceiptLineRepository = (base.ERPWarehouseReceiptLineRepository = new ERPWarehouseReceiptLineRepository(base.ApiClientContext));
			using (iERPWarehouseReceiptLineRepository)
			{
				foreach (ERPWarehouseReceiptLineInformationDto item2 in await base.ERPWarehouseReceiptLineRepository.GetAllWarehouseReceiptLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPWarehouseReceiptLineDto item = new ERPWarehouseReceiptLineDto
					{
						wrlCreatedBy = item2.wrlCreatedBy,
						wrlCreatedDate = item2.wrlCreatedDate,
						wrlDestinationPartBinID = item2.wrlDestinationPartBinID,
						wrlDestinationWarehouseID = item2.wrlDestinationWarehouseID,
						wrlUniqueID = item2.wrlUniqueID,
						wrlHeatLot = item2.wrlHeatLot,
						wrlClosed = item2.wrlClosed,
						wrlKitPart = item2.wrlKitPart,
						wrlPosted = item2.wrlPosted,
						wrlReceivedComplete = item2.wrlReceivedComplete,
						wrlReversed = item2.wrlReversed,
						wrlPartDescription = item2.wrlPartDescription,
						wrlPartID = item2.wrlPartID,
						wrlPartRevisionID = item2.wrlPartRevisionID,
						wrlQuantityReceived = item2.wrlQuantityReceived,
						wrlReference = item2.wrlReference,
						wrlReverseWHReceiptID = item2.wrlReverseWHReceiptID,
						wrlReverseWHReceiptLineID = item2.wrlReverseWHReceiptLineID,
						wrlRowVersion = item2.wrlRowVersion,
						wrlWarehouseReceiptLineID = item2.wrlWarehouseReceiptLineID,
						wrlSourcePartBinID = item2.wrlSourcePartBinID,
						wrlSourceTableName = item2.wrlSourceTableName,
						wrlSourceTableUniqueID = item2.wrlSourceTableUniqueID,
						wrlSourceWarehouseID = item2.wrlSourceWarehouseID,
						wrlUnitCost = item2.wrlUnitCost,
						wrlUnitOfMeasure = item2.wrlUnitOfMeasure,
						wrlWarehouseReceiptID = item2.wrlWarehouseReceiptID,
						wrlWarehouseRequisitionID = item2.wrlWarehouseRequisitionID,
						wrlWarehouseRequisitionLineID = item2.wrlWarehouseRequisitionLineID,
						wrlWarehouseTransferID = item2.wrlWarehouseTransferID,
						wrlWarehouseTransferLineID = item2.wrlWarehouseTransferLineID,
						wrlWTOpenQuantity = item2.wrlWTOpenQuantity,
						wrlWTShippedQuantity = item2.wrlWTShippedQuantity,
						CustomFields = item2.CustomFields
					};
					allWarehouseReceiptLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all WarehouseReceiptLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPWarehouseReceiptLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allWarehouseReceiptLinesDto,
				RecordCount = allWarehouseReceiptLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseReceiptLineDto>> Process_GetWarehouseReceiptLine(Guid warehouseReceiptLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPWarehouseReceiptLineDto warehouseReceiptLineDto = null;
		ERPResponseMessageDto<ERPWarehouseReceiptLineDto> result;
		try
		{
			IERPWarehouseReceiptLineRepository iERPWarehouseReceiptLineRepository = (base.ERPWarehouseReceiptLineRepository = new ERPWarehouseReceiptLineRepository(base.ApiClientContext));
			using (iERPWarehouseReceiptLineRepository)
			{
				ERPWarehouseReceiptLineInformationDto eRPWarehouseReceiptLineInformationDto = await base.ERPWarehouseReceiptLineRepository.GetWarehouseReceiptLine(warehouseReceiptLineId);
				warehouseReceiptLineDto = new ERPWarehouseReceiptLineDto
				{
					wrlCreatedBy = eRPWarehouseReceiptLineInformationDto.wrlCreatedBy,
					wrlCreatedDate = eRPWarehouseReceiptLineInformationDto.wrlCreatedDate,
					wrlDestinationPartBinID = eRPWarehouseReceiptLineInformationDto.wrlDestinationPartBinID,
					wrlDestinationWarehouseID = eRPWarehouseReceiptLineInformationDto.wrlDestinationWarehouseID,
					wrlUniqueID = eRPWarehouseReceiptLineInformationDto.wrlUniqueID,
					wrlHeatLot = eRPWarehouseReceiptLineInformationDto.wrlHeatLot,
					wrlClosed = eRPWarehouseReceiptLineInformationDto.wrlClosed,
					wrlKitPart = eRPWarehouseReceiptLineInformationDto.wrlKitPart,
					wrlPosted = eRPWarehouseReceiptLineInformationDto.wrlPosted,
					wrlReceivedComplete = eRPWarehouseReceiptLineInformationDto.wrlReceivedComplete,
					wrlReversed = eRPWarehouseReceiptLineInformationDto.wrlReversed,
					wrlPartDescription = eRPWarehouseReceiptLineInformationDto.wrlPartDescription,
					wrlPartID = eRPWarehouseReceiptLineInformationDto.wrlPartID,
					wrlPartRevisionID = eRPWarehouseReceiptLineInformationDto.wrlPartRevisionID,
					wrlQuantityReceived = eRPWarehouseReceiptLineInformationDto.wrlQuantityReceived,
					wrlReference = eRPWarehouseReceiptLineInformationDto.wrlReference,
					wrlReverseWHReceiptID = eRPWarehouseReceiptLineInformationDto.wrlReverseWHReceiptID,
					wrlReverseWHReceiptLineID = eRPWarehouseReceiptLineInformationDto.wrlReverseWHReceiptLineID,
					wrlRowVersion = eRPWarehouseReceiptLineInformationDto.wrlRowVersion,
					wrlWarehouseReceiptLineID = eRPWarehouseReceiptLineInformationDto.wrlWarehouseReceiptLineID,
					wrlSourcePartBinID = eRPWarehouseReceiptLineInformationDto.wrlSourcePartBinID,
					wrlSourceTableName = eRPWarehouseReceiptLineInformationDto.wrlSourceTableName,
					wrlSourceTableUniqueID = eRPWarehouseReceiptLineInformationDto.wrlSourceTableUniqueID,
					wrlSourceWarehouseID = eRPWarehouseReceiptLineInformationDto.wrlSourceWarehouseID,
					wrlUnitCost = eRPWarehouseReceiptLineInformationDto.wrlUnitCost,
					wrlUnitOfMeasure = eRPWarehouseReceiptLineInformationDto.wrlUnitOfMeasure,
					wrlWarehouseReceiptID = eRPWarehouseReceiptLineInformationDto.wrlWarehouseReceiptID,
					wrlWarehouseRequisitionID = eRPWarehouseReceiptLineInformationDto.wrlWarehouseRequisitionID,
					wrlWarehouseRequisitionLineID = eRPWarehouseReceiptLineInformationDto.wrlWarehouseRequisitionLineID,
					wrlWarehouseTransferID = eRPWarehouseReceiptLineInformationDto.wrlWarehouseTransferID,
					wrlWarehouseTransferLineID = eRPWarehouseReceiptLineInformationDto.wrlWarehouseTransferLineID,
					wrlWTOpenQuantity = eRPWarehouseReceiptLineInformationDto.wrlWTOpenQuantity,
					wrlWTShippedQuantity = eRPWarehouseReceiptLineInformationDto.wrlWTShippedQuantity,
					CustomFields = eRPWarehouseReceiptLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the WarehouseReceiptLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseReceiptLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = warehouseReceiptLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseReceiptLineDto>> Process_PutWarehouseReceiptLine(ERPWarehouseReceiptLineDto warehouseReceiptLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPWarehouseReceiptLineDto createdObject = null;
		ERPResponseMessageDto<ERPWarehouseReceiptLineDto> result;
		try
		{
			IERPWarehouseReceiptLineRepository iERPWarehouseReceiptLineRepository = (base.ERPWarehouseReceiptLineRepository = new ERPWarehouseReceiptLineRepository(base.ApiClientContext));
			using (iERPWarehouseReceiptLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPWarehouseReceiptLineRepository.SaveWarehouseReceiptLine(warehouseReceiptLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPWarehouseReceiptLineInformationDto eRPWarehouseReceiptLineInformationDto = await base.ERPWarehouseReceiptLineRepository.GetWarehouseReceiptLine(warehouseReceiptLine.wrlUniqueID);
					createdObject = new ERPWarehouseReceiptLineDto
					{
						wrlCreatedBy = eRPWarehouseReceiptLineInformationDto.wrlCreatedBy,
						wrlCreatedDate = eRPWarehouseReceiptLineInformationDto.wrlCreatedDate,
						wrlDestinationPartBinID = eRPWarehouseReceiptLineInformationDto.wrlDestinationPartBinID,
						wrlDestinationWarehouseID = eRPWarehouseReceiptLineInformationDto.wrlDestinationWarehouseID,
						wrlUniqueID = eRPWarehouseReceiptLineInformationDto.wrlUniqueID,
						wrlHeatLot = eRPWarehouseReceiptLineInformationDto.wrlHeatLot,
						wrlClosed = eRPWarehouseReceiptLineInformationDto.wrlClosed,
						wrlKitPart = eRPWarehouseReceiptLineInformationDto.wrlKitPart,
						wrlPosted = eRPWarehouseReceiptLineInformationDto.wrlPosted,
						wrlReceivedComplete = eRPWarehouseReceiptLineInformationDto.wrlReceivedComplete,
						wrlReversed = eRPWarehouseReceiptLineInformationDto.wrlReversed,
						wrlPartDescription = eRPWarehouseReceiptLineInformationDto.wrlPartDescription,
						wrlPartID = eRPWarehouseReceiptLineInformationDto.wrlPartID,
						wrlPartRevisionID = eRPWarehouseReceiptLineInformationDto.wrlPartRevisionID,
						wrlQuantityReceived = eRPWarehouseReceiptLineInformationDto.wrlQuantityReceived,
						wrlReference = eRPWarehouseReceiptLineInformationDto.wrlReference,
						wrlReverseWHReceiptID = eRPWarehouseReceiptLineInformationDto.wrlReverseWHReceiptID,
						wrlReverseWHReceiptLineID = eRPWarehouseReceiptLineInformationDto.wrlReverseWHReceiptLineID,
						wrlRowVersion = eRPWarehouseReceiptLineInformationDto.wrlRowVersion,
						wrlWarehouseReceiptLineID = eRPWarehouseReceiptLineInformationDto.wrlWarehouseReceiptLineID,
						wrlSourcePartBinID = eRPWarehouseReceiptLineInformationDto.wrlSourcePartBinID,
						wrlSourceTableName = eRPWarehouseReceiptLineInformationDto.wrlSourceTableName,
						wrlSourceTableUniqueID = eRPWarehouseReceiptLineInformationDto.wrlSourceTableUniqueID,
						wrlSourceWarehouseID = eRPWarehouseReceiptLineInformationDto.wrlSourceWarehouseID,
						wrlUnitCost = eRPWarehouseReceiptLineInformationDto.wrlUnitCost,
						wrlUnitOfMeasure = eRPWarehouseReceiptLineInformationDto.wrlUnitOfMeasure,
						wrlWarehouseReceiptID = eRPWarehouseReceiptLineInformationDto.wrlWarehouseReceiptID,
						wrlWarehouseRequisitionID = eRPWarehouseReceiptLineInformationDto.wrlWarehouseRequisitionID,
						wrlWarehouseRequisitionLineID = eRPWarehouseReceiptLineInformationDto.wrlWarehouseRequisitionLineID,
						wrlWarehouseTransferID = eRPWarehouseReceiptLineInformationDto.wrlWarehouseTransferID,
						wrlWarehouseTransferLineID = eRPWarehouseReceiptLineInformationDto.wrlWarehouseTransferLineID,
						wrlWTOpenQuantity = eRPWarehouseReceiptLineInformationDto.wrlWTOpenQuantity,
						wrlWTShippedQuantity = eRPWarehouseReceiptLineInformationDto.wrlWTShippedQuantity,
						CustomFields = eRPWarehouseReceiptLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing WarehouseReceiptLine [{warehouseReceiptLine.wrlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseReceiptLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseReceiptLine(Guid warehouseReceiptLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWarehouseReceiptLineRepository iERPWarehouseReceiptLineRepository = (base.ERPWarehouseReceiptLineRepository = new ERPWarehouseReceiptLineRepository(base.ApiClientContext));
		using (iERPWarehouseReceiptLineRepository)
		{
			if (!(await base.ERPWarehouseReceiptLineRepository.DoesWarehouseReceiptLineExist(warehouseReceiptLineId)))
			{
				base.ErrorsList.Add($"WarehouseReceiptLine [{warehouseReceiptLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPWarehouseReceiptLineInformationDto eRPWarehouseReceiptLineInformationDto = await base.ERPWarehouseReceiptLineRepository.GetWarehouseReceiptLine(warehouseReceiptLineId);
				string text = await base.ERPWarehouseReceiptLineRepository.WhereUsed("WarehouseReceiptLines", new object[2] { eRPWarehouseReceiptLineInformationDto.wrlWarehouseReceiptID, eRPWarehouseReceiptLineInformationDto.wrlWarehouseReceiptLineID }, new object[2] { "wrlWarehouseReceiptID", "wrlWarehouseReceiptLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("WarehouseReceiptLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPWarehouseReceiptLineDto>> Process_DeleteWarehouseReceiptLine(Guid warehouseReceiptLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPWarehouseReceiptLineDto> result;
		try
		{
			IERPWarehouseReceiptLineRepository iERPWarehouseReceiptLineRepository = (base.ERPWarehouseReceiptLineRepository = new ERPWarehouseReceiptLineRepository(base.ApiClientContext));
			using (iERPWarehouseReceiptLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPWarehouseReceiptLineRepository.DeleteRowFromTable("WarehouseReceiptLines", "wrl", warehouseReceiptLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of WarehouseReceiptLine [{warehouseReceiptLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWarehouseReceiptLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPWarehouseReceiptLineDto()
			};
		}
		return result;
	}
}
