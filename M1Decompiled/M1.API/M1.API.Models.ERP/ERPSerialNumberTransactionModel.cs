using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSerialNumberTransactionModel : ERPBaseModel, IERPSerialNumberTransactionModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSerialNumberTransactions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSerialNumberTransactionRepository iERPSerialNumberTransactionRepository = (base.ERPSerialNumberTransactionRepository = new ERPSerialNumberTransactionRepository(base.ApiClientContext));
		using (iERPSerialNumberTransactionRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSerialNumberTransactionRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSerialNumberTransactionRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSerialNumberTransactionRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSerialNumberTransactionRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSerialNumberTransaction(Guid serialNumberTransactionId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSerialNumberTransactionRepository iERPSerialNumberTransactionRepository = (base.ERPSerialNumberTransactionRepository = new ERPSerialNumberTransactionRepository(base.ApiClientContext));
		using (iERPSerialNumberTransactionRepository)
		{
			if (!(await base.ERPSerialNumberTransactionRepository.DoesSerialNumberTransactionExist(serialNumberTransactionId)))
			{
				errorsList.Add($"SerialNumberTransaction [{serialNumberTransactionId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutSerialNumberTransaction(ERPSerialNumberTransactionDto serialNumberTransaction)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSerialNumberTransactionRepository iERPSerialNumberTransactionRepository = (base.ERPSerialNumberTransactionRepository = new ERPSerialNumberTransactionRepository(base.ApiClientContext));
		using (iERPSerialNumberTransactionRepository)
		{
			if (!string.IsNullOrWhiteSpace(serialNumberTransaction.sntPartID) && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { serialNumberTransaction.sntPartID })))
			{
				errorsList.Add("sntPartID [" + serialNumberTransaction.sntPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberTransaction.sntPartRevisionID) && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { serialNumberTransaction.sntPartID, serialNumberTransaction.sntPartRevisionID })))
			{
				errorsList.Add("sntPartRevisionID [" + serialNumberTransaction.sntPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberTransaction.sntSerialNumberID) && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("SerialNumbers", new object[3] { "IMSPARTID", "IMSPARTREVISIONID", "IMSSERIALNUMBERID" }, new object[3] { serialNumberTransaction.sntPartID, serialNumberTransaction.sntPartRevisionID, serialNumberTransaction.sntSerialNumberID })))
			{
				errorsList.Add("sntSerialNumberID [" + serialNumberTransaction.sntSerialNumberID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberTransaction.sntPartWarehouseLocationID) && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { serialNumberTransaction.sntPartID, serialNumberTransaction.sntPartRevisionID, serialNumberTransaction.sntPartWarehouseLocationID })))
			{
				errorsList.Add("sntPartWarehouseLocationID [" + serialNumberTransaction.sntPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberTransaction.sntPartBinID) && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { serialNumberTransaction.sntPartID, serialNumberTransaction.sntPartRevisionID, serialNumberTransaction.sntPartWarehouseLocationID, serialNumberTransaction.sntPartBinID })))
			{
				errorsList.Add("sntPartBinID [" + serialNumberTransaction.sntPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberTransaction.sntJobID) && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { serialNumberTransaction.sntJobID })))
			{
				errorsList.Add("sntJobID [" + serialNumberTransaction.sntJobID + "] not found.");
			}
			if (serialNumberTransaction.sntJobAssemblyID > 0 && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { serialNumberTransaction.sntJobID, serialNumberTransaction.sntJobAssemblyID })))
			{
				errorsList.Add($"sntJobAssemblyID [{serialNumberTransaction.sntJobAssemblyID}] not found.");
			}
			if (serialNumberTransaction.sntJobMaterialID > 0 && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { serialNumberTransaction.sntJobID, serialNumberTransaction.sntJobAssemblyID, serialNumberTransaction.sntJobMaterialID })))
			{
				errorsList.Add($"sntJobMaterialID [{serialNumberTransaction.sntJobMaterialID}] not found.");
			}
			if (serialNumberTransaction.sntPartTransactionID > 0 && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("PartTransactions", new object[1] { "IMTPARTTRANSACTIONID" }, new object[1] { serialNumberTransaction.sntPartTransactionID })))
			{
				errorsList.Add($"sntPartTransactionID [{serialNumberTransaction.sntPartTransactionID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberTransaction.sntReceiptID) && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("Receipts", new object[1] { "RMPRECEIPTID" }, new object[1] { serialNumberTransaction.sntReceiptID })))
			{
				errorsList.Add("sntReceiptID [" + serialNumberTransaction.sntReceiptID + "] not found.");
			}
			if (serialNumberTransaction.sntReceiptLineID > 0 && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("ReceiptLines", new object[2] { "RMLRECEIPTID", "RMLRECEIPTLINEID" }, new object[2] { serialNumberTransaction.sntReceiptID, serialNumberTransaction.sntReceiptLineID })))
			{
				errorsList.Add($"sntReceiptLineID [{serialNumberTransaction.sntReceiptLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberTransaction.sntShipmentID) && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("Shipments", new object[1] { "SMPSHIPMENTID" }, new object[1] { serialNumberTransaction.sntShipmentID })))
			{
				errorsList.Add("sntShipmentID [" + serialNumberTransaction.sntShipmentID + "] not found.");
			}
			if (serialNumberTransaction.sntShipmentLineID > 0 && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("ShipmentLines", new object[2] { "SMLSHIPMENTID", "SMLSHIPMENTLINEID" }, new object[2] { serialNumberTransaction.sntShipmentID, serialNumberTransaction.sntShipmentLineID })))
			{
				errorsList.Add($"sntShipmentLineID [{serialNumberTransaction.sntShipmentLineID}] not found.");
			}
			if (serialNumberTransaction.sntInventoryCountID > 0 && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("InventoryCounts", new object[1] { "IMNINVENTORYCOUNTID" }, new object[1] { serialNumberTransaction.sntInventoryCountID })))
			{
				errorsList.Add($"sntInventoryCountID [{serialNumberTransaction.sntInventoryCountID}] not found.");
			}
			if (serialNumberTransaction.sntInventoryCountLineID > 0 && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("InventoryCountLines", new object[2] { "IMQINVENTORYCOUNTID", "IMQINVENTORYCOUNTLINEID" }, new object[2] { serialNumberTransaction.sntInventoryCountID, serialNumberTransaction.sntInventoryCountLineID })))
			{
				errorsList.Add($"sntInventoryCountLineID [{serialNumberTransaction.sntInventoryCountLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberTransaction.sntDmrShipmentID) && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("DMRShipments", new object[1] { "DSPDMRSHIPMENTID" }, new object[1] { serialNumberTransaction.sntDmrShipmentID })))
			{
				errorsList.Add("sntDmrShipmentID [" + serialNumberTransaction.sntDmrShipmentID + "] not found.");
			}
			if (serialNumberTransaction.sntDmrShipmentLineID > 0 && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("DMRShipmentLines", new object[2] { "DSLDMRSHIPMENTID", "DSLDMRSHIPMENTLINEID" }, new object[2] { serialNumberTransaction.sntDmrShipmentID, serialNumberTransaction.sntDmrShipmentLineID })))
			{
				errorsList.Add($"sntDmrShipmentLineID [{serialNumberTransaction.sntDmrShipmentLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberTransaction.sntRmaReceiptID) && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("RMAReceipts", new object[1] { "RRPRMARECEIPTID" }, new object[1] { serialNumberTransaction.sntRmaReceiptID })))
			{
				errorsList.Add("sntRmaReceiptID [" + serialNumberTransaction.sntRmaReceiptID + "] not found.");
			}
			if (serialNumberTransaction.sntRmaReceiptLineID > 0 && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("RMAReceiptLines", new object[2] { "RRLRMARECEIPTID", "RRLRMARECEIPTLINEID" }, new object[2] { serialNumberTransaction.sntRmaReceiptID, serialNumberTransaction.sntRmaReceiptLineID })))
			{
				errorsList.Add($"sntRmaReceiptLineID [{serialNumberTransaction.sntRmaReceiptLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberTransaction.sntWarehouseTransferID) && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("WarehouseTransfers", new object[1] { "MWPWAREHOUSETRANSFERID" }, new object[1] { serialNumberTransaction.sntWarehouseTransferID })))
			{
				errorsList.Add("sntWarehouseTransferID [" + serialNumberTransaction.sntWarehouseTransferID + "] not found.");
			}
			if (serialNumberTransaction.sntWarehouseTransferLineID > 0 && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("WarehouseTransferLines", new object[2] { "MWLWAREHOUSETRANSFERID", "MWLWAREHOUSETRANSFERLINEID" }, new object[2] { serialNumberTransaction.sntWarehouseTransferID, serialNumberTransaction.sntWarehouseTransferLineID })))
			{
				errorsList.Add($"sntWarehouseTransferLineID [{serialNumberTransaction.sntWarehouseTransferLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberTransaction.sntWarehouseReceiptID) && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("WarehouseReceipts", new object[1] { "WRPWAREHOUSERECEIPTID" }, new object[1] { serialNumberTransaction.sntWarehouseReceiptID })))
			{
				errorsList.Add("sntWarehouseReceiptID [" + serialNumberTransaction.sntWarehouseReceiptID + "] not found.");
			}
			if (serialNumberTransaction.sntWarehouseReceiptLineID > 0 && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("WarehouseReceiptLines", new object[2] { "WRLWAREHOUSERECEIPTID", "WRLWAREHOUSERECEIPTLINEID" }, new object[2] { serialNumberTransaction.sntWarehouseReceiptID, serialNumberTransaction.sntWarehouseReceiptLineID })))
			{
				errorsList.Add($"sntWarehouseReceiptLineID [{serialNumberTransaction.sntWarehouseReceiptLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberTransaction.sntInspectionID) && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("Inspections", new object[1] { "QAPINSPECTIONID" }, new object[1] { serialNumberTransaction.sntInspectionID })))
			{
				errorsList.Add("sntInspectionID [" + serialNumberTransaction.sntInspectionID + "] not found.");
			}
			if (serialNumberTransaction.sntInspectionLineID > 0 && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("InspectionLines", new object[2] { "QALINSPECTIONID", "QALINSPECTIONLINEID" }, new object[2] { serialNumberTransaction.sntInspectionID, serialNumberTransaction.sntInspectionLineID })))
			{
				errorsList.Add($"sntInspectionLineID [{serialNumberTransaction.sntInspectionLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberTransaction.sntLandedCostID) && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("LandedCosts", new object[1] { "RMCLANDEDCOSTID" }, new object[1] { serialNumberTransaction.sntLandedCostID })))
			{
				errorsList.Add("sntLandedCostID [" + serialNumberTransaction.sntLandedCostID + "] not found.");
			}
			if (serialNumberTransaction.sntJobMaterialComponentID > 0 && !(await base.ERPSerialNumberTransactionRepository.DoesRecordExistInTableUsingKeys("JobMaterialComponents", new object[4] { "JMTJOBID", "JMTJOBASSEMBLYID", "JMTJOBMATERIALID", "JMTJOBMATERIALCOMPONENTID" }, new object[4] { serialNumberTransaction.sntJobID, serialNumberTransaction.sntJobAssemblyID, serialNumberTransaction.sntJobMaterialID, serialNumberTransaction.sntJobMaterialComponentID })))
			{
				errorsList.Add($"sntJobMaterialComponentID [{serialNumberTransaction.sntJobMaterialComponentID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSerialNumberTransactionDto>>> Process_GetAllSerialNumberTransactions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSerialNumberTransactionDto> allSerialNumberTransactionsDto = new List<ERPSerialNumberTransactionDto>();
		ERPResponseMessageDto<IList<ERPSerialNumberTransactionDto>> result;
		try
		{
			IERPSerialNumberTransactionRepository iERPSerialNumberTransactionRepository = (base.ERPSerialNumberTransactionRepository = new ERPSerialNumberTransactionRepository(base.ApiClientContext));
			using (iERPSerialNumberTransactionRepository)
			{
				foreach (ERPSerialNumberTransactionInformationDto item2 in await base.ERPSerialNumberTransactionRepository.GetAllSerialNumberTransactions(pageSize, pageNumber, filter, orderBy))
				{
					ERPSerialNumberTransactionDto item = new ERPSerialNumberTransactionDto
					{
						sntCreatedBy = item2.sntCreatedBy,
						sntCreatedDate = item2.sntCreatedDate,
						sntDmrShipmentID = item2.sntDmrShipmentID,
						sntDmrShipmentLineID = item2.sntDmrShipmentLineID,
						sntUniqueID = item2.sntUniqueID,
						sntInspectionID = item2.sntInspectionID,
						sntInspectionLineID = item2.sntInspectionLineID,
						sntInventoryCountID = item2.sntInventoryCountID,
						sntInventoryCountLineID = item2.sntInventoryCountLineID,
						sntInspect = item2.sntInspect,
						sntNegativeTransaction = item2.sntNegativeTransaction,
						sntJobAssemblyID = item2.sntJobAssemblyID,
						sntJobID = item2.sntJobID,
						sntJobMaterialComponentID = item2.sntJobMaterialComponentID,
						sntJobMaterialID = item2.sntJobMaterialID,
						sntJobPartBinID = item2.sntJobPartBinID,
						sntJobPartID = item2.sntJobPartID,
						sntJobPartRevisionID = item2.sntJobPartRevisionID,
						sntJobPartWarehouseLocationID = item2.sntJobPartWarehouseLocationID,
						sntJobSerialNumberID = item2.sntJobSerialNumberID,
						sntLandedCostID = item2.sntLandedCostID,
						sntOldTransactionType = item2.sntOldTransactionType,
						sntPartBinID = item2.sntPartBinID,
						sntPartID = item2.sntPartID,
						sntPartRevisionID = item2.sntPartRevisionID,
						sntPartTransactionID = item2.sntPartTransactionID,
						sntPartWarehouseLocationID = item2.sntPartWarehouseLocationID,
						sntQuantity = item2.sntQuantity,
						sntReceiptID = item2.sntReceiptID,
						sntReceiptLineID = item2.sntReceiptLineID,
						sntRmaReceiptID = item2.sntRmaReceiptID,
						sntRmaReceiptLineID = item2.sntRmaReceiptLineID,
						sntRowVersion = item2.sntRowVersion,
						sntSerialNumberTransactionID = item2.sntSerialNumberTransactionID,
						sntSerialNumberID = item2.sntSerialNumberID,
						sntShipmentID = item2.sntShipmentID,
						sntShipmentLineID = item2.sntShipmentLineID,
						sntStatus = item2.sntStatus,
						sntTableName = item2.sntTableName,
						sntTableUniqueID = item2.sntTableUniqueID,
						sntTransactionDate = item2.sntTransactionDate,
						sntTransactionType = item2.sntTransactionType,
						sntWarehouseReceiptID = item2.sntWarehouseReceiptID,
						sntWarehouseReceiptLineID = item2.sntWarehouseReceiptLineID,
						sntWarehouseTransferID = item2.sntWarehouseTransferID,
						sntWarehouseTransferLineID = item2.sntWarehouseTransferLineID,
						CustomFields = item2.CustomFields
					};
					allSerialNumberTransactionsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SerialNumberTransactions]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSerialNumberTransactionDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSerialNumberTransactionsDto,
				RecordCount = allSerialNumberTransactionsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSerialNumberTransactionDto>> Process_GetSerialNumberTransaction(Guid serialNumberTransactionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSerialNumberTransactionDto serialNumberTransactionDto = null;
		ERPResponseMessageDto<ERPSerialNumberTransactionDto> result;
		try
		{
			IERPSerialNumberTransactionRepository iERPSerialNumberTransactionRepository = (base.ERPSerialNumberTransactionRepository = new ERPSerialNumberTransactionRepository(base.ApiClientContext));
			using (iERPSerialNumberTransactionRepository)
			{
				ERPSerialNumberTransactionInformationDto eRPSerialNumberTransactionInformationDto = await base.ERPSerialNumberTransactionRepository.GetSerialNumberTransaction(serialNumberTransactionId);
				serialNumberTransactionDto = new ERPSerialNumberTransactionDto
				{
					sntCreatedBy = eRPSerialNumberTransactionInformationDto.sntCreatedBy,
					sntCreatedDate = eRPSerialNumberTransactionInformationDto.sntCreatedDate,
					sntDmrShipmentID = eRPSerialNumberTransactionInformationDto.sntDmrShipmentID,
					sntDmrShipmentLineID = eRPSerialNumberTransactionInformationDto.sntDmrShipmentLineID,
					sntUniqueID = eRPSerialNumberTransactionInformationDto.sntUniqueID,
					sntInspectionID = eRPSerialNumberTransactionInformationDto.sntInspectionID,
					sntInspectionLineID = eRPSerialNumberTransactionInformationDto.sntInspectionLineID,
					sntInventoryCountID = eRPSerialNumberTransactionInformationDto.sntInventoryCountID,
					sntInventoryCountLineID = eRPSerialNumberTransactionInformationDto.sntInventoryCountLineID,
					sntInspect = eRPSerialNumberTransactionInformationDto.sntInspect,
					sntNegativeTransaction = eRPSerialNumberTransactionInformationDto.sntNegativeTransaction,
					sntJobAssemblyID = eRPSerialNumberTransactionInformationDto.sntJobAssemblyID,
					sntJobID = eRPSerialNumberTransactionInformationDto.sntJobID,
					sntJobMaterialComponentID = eRPSerialNumberTransactionInformationDto.sntJobMaterialComponentID,
					sntJobMaterialID = eRPSerialNumberTransactionInformationDto.sntJobMaterialID,
					sntJobPartBinID = eRPSerialNumberTransactionInformationDto.sntJobPartBinID,
					sntJobPartID = eRPSerialNumberTransactionInformationDto.sntJobPartID,
					sntJobPartRevisionID = eRPSerialNumberTransactionInformationDto.sntJobPartRevisionID,
					sntJobPartWarehouseLocationID = eRPSerialNumberTransactionInformationDto.sntJobPartWarehouseLocationID,
					sntJobSerialNumberID = eRPSerialNumberTransactionInformationDto.sntJobSerialNumberID,
					sntLandedCostID = eRPSerialNumberTransactionInformationDto.sntLandedCostID,
					sntOldTransactionType = eRPSerialNumberTransactionInformationDto.sntOldTransactionType,
					sntPartBinID = eRPSerialNumberTransactionInformationDto.sntPartBinID,
					sntPartID = eRPSerialNumberTransactionInformationDto.sntPartID,
					sntPartRevisionID = eRPSerialNumberTransactionInformationDto.sntPartRevisionID,
					sntPartTransactionID = eRPSerialNumberTransactionInformationDto.sntPartTransactionID,
					sntPartWarehouseLocationID = eRPSerialNumberTransactionInformationDto.sntPartWarehouseLocationID,
					sntQuantity = eRPSerialNumberTransactionInformationDto.sntQuantity,
					sntReceiptID = eRPSerialNumberTransactionInformationDto.sntReceiptID,
					sntReceiptLineID = eRPSerialNumberTransactionInformationDto.sntReceiptLineID,
					sntRmaReceiptID = eRPSerialNumberTransactionInformationDto.sntRmaReceiptID,
					sntRmaReceiptLineID = eRPSerialNumberTransactionInformationDto.sntRmaReceiptLineID,
					sntRowVersion = eRPSerialNumberTransactionInformationDto.sntRowVersion,
					sntSerialNumberTransactionID = eRPSerialNumberTransactionInformationDto.sntSerialNumberTransactionID,
					sntSerialNumberID = eRPSerialNumberTransactionInformationDto.sntSerialNumberID,
					sntShipmentID = eRPSerialNumberTransactionInformationDto.sntShipmentID,
					sntShipmentLineID = eRPSerialNumberTransactionInformationDto.sntShipmentLineID,
					sntStatus = eRPSerialNumberTransactionInformationDto.sntStatus,
					sntTableName = eRPSerialNumberTransactionInformationDto.sntTableName,
					sntTableUniqueID = eRPSerialNumberTransactionInformationDto.sntTableUniqueID,
					sntTransactionDate = eRPSerialNumberTransactionInformationDto.sntTransactionDate,
					sntTransactionType = eRPSerialNumberTransactionInformationDto.sntTransactionType,
					sntWarehouseReceiptID = eRPSerialNumberTransactionInformationDto.sntWarehouseReceiptID,
					sntWarehouseReceiptLineID = eRPSerialNumberTransactionInformationDto.sntWarehouseReceiptLineID,
					sntWarehouseTransferID = eRPSerialNumberTransactionInformationDto.sntWarehouseTransferID,
					sntWarehouseTransferLineID = eRPSerialNumberTransactionInformationDto.sntWarehouseTransferLineID,
					CustomFields = eRPSerialNumberTransactionInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SerialNumberTransactions []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSerialNumberTransactionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = serialNumberTransactionDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSerialNumberTransactionDto>> Process_PutSerialNumberTransaction(ERPSerialNumberTransactionDto serialNumberTransaction)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPSerialNumberTransactionDto createdObject = null;
		ERPResponseMessageDto<ERPSerialNumberTransactionDto> result;
		try
		{
			IERPSerialNumberTransactionRepository iERPSerialNumberTransactionRepository = (base.ERPSerialNumberTransactionRepository = new ERPSerialNumberTransactionRepository(base.ApiClientContext));
			using (iERPSerialNumberTransactionRepository)
			{
				APIValidationInfoDto postResult = await base.ERPSerialNumberTransactionRepository.SaveSerialNumberTransaction(serialNumberTransaction);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPSerialNumberTransactionInformationDto eRPSerialNumberTransactionInformationDto = await base.ERPSerialNumberTransactionRepository.GetSerialNumberTransaction(serialNumberTransaction.sntUniqueID);
					createdObject = new ERPSerialNumberTransactionDto
					{
						sntCreatedBy = eRPSerialNumberTransactionInformationDto.sntCreatedBy,
						sntCreatedDate = eRPSerialNumberTransactionInformationDto.sntCreatedDate,
						sntDmrShipmentID = eRPSerialNumberTransactionInformationDto.sntDmrShipmentID,
						sntDmrShipmentLineID = eRPSerialNumberTransactionInformationDto.sntDmrShipmentLineID,
						sntUniqueID = eRPSerialNumberTransactionInformationDto.sntUniqueID,
						sntInspectionID = eRPSerialNumberTransactionInformationDto.sntInspectionID,
						sntInspectionLineID = eRPSerialNumberTransactionInformationDto.sntInspectionLineID,
						sntInventoryCountID = eRPSerialNumberTransactionInformationDto.sntInventoryCountID,
						sntInventoryCountLineID = eRPSerialNumberTransactionInformationDto.sntInventoryCountLineID,
						sntInspect = eRPSerialNumberTransactionInformationDto.sntInspect,
						sntNegativeTransaction = eRPSerialNumberTransactionInformationDto.sntNegativeTransaction,
						sntJobAssemblyID = eRPSerialNumberTransactionInformationDto.sntJobAssemblyID,
						sntJobID = eRPSerialNumberTransactionInformationDto.sntJobID,
						sntJobMaterialComponentID = eRPSerialNumberTransactionInformationDto.sntJobMaterialComponentID,
						sntJobMaterialID = eRPSerialNumberTransactionInformationDto.sntJobMaterialID,
						sntJobPartBinID = eRPSerialNumberTransactionInformationDto.sntJobPartBinID,
						sntJobPartID = eRPSerialNumberTransactionInformationDto.sntJobPartID,
						sntJobPartRevisionID = eRPSerialNumberTransactionInformationDto.sntJobPartRevisionID,
						sntJobPartWarehouseLocationID = eRPSerialNumberTransactionInformationDto.sntJobPartWarehouseLocationID,
						sntJobSerialNumberID = eRPSerialNumberTransactionInformationDto.sntJobSerialNumberID,
						sntLandedCostID = eRPSerialNumberTransactionInformationDto.sntLandedCostID,
						sntOldTransactionType = eRPSerialNumberTransactionInformationDto.sntOldTransactionType,
						sntPartBinID = eRPSerialNumberTransactionInformationDto.sntPartBinID,
						sntPartID = eRPSerialNumberTransactionInformationDto.sntPartID,
						sntPartRevisionID = eRPSerialNumberTransactionInformationDto.sntPartRevisionID,
						sntPartTransactionID = eRPSerialNumberTransactionInformationDto.sntPartTransactionID,
						sntPartWarehouseLocationID = eRPSerialNumberTransactionInformationDto.sntPartWarehouseLocationID,
						sntQuantity = eRPSerialNumberTransactionInformationDto.sntQuantity,
						sntReceiptID = eRPSerialNumberTransactionInformationDto.sntReceiptID,
						sntReceiptLineID = eRPSerialNumberTransactionInformationDto.sntReceiptLineID,
						sntRmaReceiptID = eRPSerialNumberTransactionInformationDto.sntRmaReceiptID,
						sntRmaReceiptLineID = eRPSerialNumberTransactionInformationDto.sntRmaReceiptLineID,
						sntRowVersion = eRPSerialNumberTransactionInformationDto.sntRowVersion,
						sntSerialNumberTransactionID = eRPSerialNumberTransactionInformationDto.sntSerialNumberTransactionID,
						sntSerialNumberID = eRPSerialNumberTransactionInformationDto.sntSerialNumberID,
						sntShipmentID = eRPSerialNumberTransactionInformationDto.sntShipmentID,
						sntShipmentLineID = eRPSerialNumberTransactionInformationDto.sntShipmentLineID,
						sntStatus = eRPSerialNumberTransactionInformationDto.sntStatus,
						sntTableName = eRPSerialNumberTransactionInformationDto.sntTableName,
						sntTableUniqueID = eRPSerialNumberTransactionInformationDto.sntTableUniqueID,
						sntTransactionDate = eRPSerialNumberTransactionInformationDto.sntTransactionDate,
						sntTransactionType = eRPSerialNumberTransactionInformationDto.sntTransactionType,
						sntWarehouseReceiptID = eRPSerialNumberTransactionInformationDto.sntWarehouseReceiptID,
						sntWarehouseReceiptLineID = eRPSerialNumberTransactionInformationDto.sntWarehouseReceiptLineID,
						sntWarehouseTransferID = eRPSerialNumberTransactionInformationDto.sntWarehouseTransferID,
						sntWarehouseTransferLineID = eRPSerialNumberTransactionInformationDto.sntWarehouseTransferLineID,
						CustomFields = eRPSerialNumberTransactionInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing SerialNumberTransaction [{serialNumberTransaction.sntUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSerialNumberTransactionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteSerialNumberTransaction(Guid serialNumberTransactionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSerialNumberTransactionRepository iERPSerialNumberTransactionRepository = (base.ERPSerialNumberTransactionRepository = new ERPSerialNumberTransactionRepository(base.ApiClientContext));
		using (iERPSerialNumberTransactionRepository)
		{
			if (!(await base.ERPSerialNumberTransactionRepository.DoesSerialNumberTransactionExist(serialNumberTransactionId)))
			{
				base.ErrorsList.Add($"SerialNumberTransaction [{serialNumberTransactionId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPSerialNumberTransactionInformationDto eRPSerialNumberTransactionInformationDto = await base.ERPSerialNumberTransactionRepository.GetSerialNumberTransaction(serialNumberTransactionId);
				string text = await base.ERPSerialNumberTransactionRepository.WhereUsed("SerialNumberTransactions", new object[1] { eRPSerialNumberTransactionInformationDto.sntSerialNumberTransactionID }, new object[1] { "sntSerialNumberTransactionID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("SerialNumberTransaction cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPSerialNumberTransactionDto>> Process_DeleteSerialNumberTransaction(Guid serialNumberTransactionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPSerialNumberTransactionDto> result;
		try
		{
			IERPSerialNumberTransactionRepository iERPSerialNumberTransactionRepository = (base.ERPSerialNumberTransactionRepository = new ERPSerialNumberTransactionRepository(base.ApiClientContext));
			using (iERPSerialNumberTransactionRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPSerialNumberTransactionRepository.DeleteRowFromTable("SerialNumberTransactions", "snt", serialNumberTransactionId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of SerialNumberTransaction [{serialNumberTransactionId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSerialNumberTransactionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPSerialNumberTransactionDto()
			};
		}
		return result;
	}
}
