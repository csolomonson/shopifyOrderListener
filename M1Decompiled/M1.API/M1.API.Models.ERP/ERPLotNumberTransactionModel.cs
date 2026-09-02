using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPLotNumberTransactionModel : ERPBaseModel, IERPLotNumberTransactionModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllLotNumberTransactions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPLotNumberTransactionRepository iERPLotNumberTransactionRepository = (base.ERPLotNumberTransactionRepository = new ERPLotNumberTransactionRepository(base.ApiClientContext));
		using (iERPLotNumberTransactionRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPLotNumberTransactionRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPLotNumberTransactionRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPLotNumberTransactionRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPLotNumberTransactionRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetLotNumberTransaction(Guid lotNumberTransactionId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLotNumberTransactionRepository iERPLotNumberTransactionRepository = (base.ERPLotNumberTransactionRepository = new ERPLotNumberTransactionRepository(base.ApiClientContext));
		using (iERPLotNumberTransactionRepository)
		{
			if (!(await base.ERPLotNumberTransactionRepository.DoesLotNumberTransactionExist(lotNumberTransactionId)))
			{
				errorsList.Add($"LotNumberTransaction [{lotNumberTransactionId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutLotNumberTransaction(ERPLotNumberTransactionDto lotNumberTransaction)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLotNumberTransactionRepository iERPLotNumberTransactionRepository = (base.ERPLotNumberTransactionRepository = new ERPLotNumberTransactionRepository(base.ApiClientContext));
		using (iERPLotNumberTransactionRepository)
		{
			if (!string.IsNullOrWhiteSpace(lotNumberTransaction.abtPartID) && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { lotNumberTransaction.abtPartID })))
			{
				errorsList.Add("abtPartID [" + lotNumberTransaction.abtPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberTransaction.abtPartRevisionID) && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { lotNumberTransaction.abtPartID, lotNumberTransaction.abtPartRevisionID })))
			{
				errorsList.Add("abtPartRevisionID [" + lotNumberTransaction.abtPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberTransaction.abtPartWarehouseLocationID) && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { lotNumberTransaction.abtPartID, lotNumberTransaction.abtPartRevisionID, lotNumberTransaction.abtPartWarehouseLocationID })))
			{
				errorsList.Add("abtPartWarehouseLocationID [" + lotNumberTransaction.abtPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberTransaction.abtPartBinID) && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { lotNumberTransaction.abtPartID, lotNumberTransaction.abtPartRevisionID, lotNumberTransaction.abtPartWarehouseLocationID, lotNumberTransaction.abtPartBinID })))
			{
				errorsList.Add("abtPartBinID [" + lotNumberTransaction.abtPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberTransaction.abtLotNumberID) && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("LotNumbers", new object[3] { "ABLPARTID", "ABLPARTREVISIONID", "ABLLOTNUMBERID" }, new object[3] { lotNumberTransaction.abtPartID, lotNumberTransaction.abtPartRevisionID, lotNumberTransaction.abtLotNumberID })))
			{
				errorsList.Add("abtLotNumberID [" + lotNumberTransaction.abtLotNumberID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberTransaction.abtJobID) && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { lotNumberTransaction.abtJobID })))
			{
				errorsList.Add("abtJobID [" + lotNumberTransaction.abtJobID + "] not found.");
			}
			if (lotNumberTransaction.abtJobAssemblyID > 0 && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { lotNumberTransaction.abtJobID, lotNumberTransaction.abtJobAssemblyID })))
			{
				errorsList.Add($"abtJobAssemblyID [{lotNumberTransaction.abtJobAssemblyID}] not found.");
			}
			if (lotNumberTransaction.abtJobMaterialID > 0 && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { lotNumberTransaction.abtJobID, lotNumberTransaction.abtJobAssemblyID, lotNumberTransaction.abtJobMaterialID })))
			{
				errorsList.Add($"abtJobMaterialID [{lotNumberTransaction.abtJobMaterialID}] not found.");
			}
			if (lotNumberTransaction.abtPartTransactionID > 0 && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("PartTransactions", new object[1] { "IMTPARTTRANSACTIONID" }, new object[1] { lotNumberTransaction.abtPartTransactionID })))
			{
				errorsList.Add($"abtPartTransactionID [{lotNumberTransaction.abtPartTransactionID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberTransaction.abtReceiptID) && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("Receipts", new object[1] { "RMPRECEIPTID" }, new object[1] { lotNumberTransaction.abtReceiptID })))
			{
				errorsList.Add("abtReceiptID [" + lotNumberTransaction.abtReceiptID + "] not found.");
			}
			if (lotNumberTransaction.abtReceiptLineID > 0 && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("ReceiptLines", new object[2] { "RMLRECEIPTID", "RMLRECEIPTLINEID" }, new object[2] { lotNumberTransaction.abtReceiptID, lotNumberTransaction.abtReceiptLineID })))
			{
				errorsList.Add($"abtReceiptLineID [{lotNumberTransaction.abtReceiptLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberTransaction.abtShipmentID) && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("Shipments", new object[1] { "SMPSHIPMENTID" }, new object[1] { lotNumberTransaction.abtShipmentID })))
			{
				errorsList.Add("abtShipmentID [" + lotNumberTransaction.abtShipmentID + "] not found.");
			}
			if (lotNumberTransaction.abtShipmentLineID > 0 && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("ShipmentLines", new object[2] { "SMLSHIPMENTID", "SMLSHIPMENTLINEID" }, new object[2] { lotNumberTransaction.abtShipmentID, lotNumberTransaction.abtShipmentLineID })))
			{
				errorsList.Add($"abtShipmentLineID [{lotNumberTransaction.abtShipmentLineID}] not found.");
			}
			if (lotNumberTransaction.abtInventoryCountID > 0 && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("InventoryCounts", new object[1] { "IMNINVENTORYCOUNTID" }, new object[1] { lotNumberTransaction.abtInventoryCountID })))
			{
				errorsList.Add($"abtInventoryCountID [{lotNumberTransaction.abtInventoryCountID}] not found.");
			}
			if (lotNumberTransaction.abtInventoryCountLineID > 0 && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("InventoryCountLines", new object[2] { "IMQINVENTORYCOUNTID", "IMQINVENTORYCOUNTLINEID" }, new object[2] { lotNumberTransaction.abtInventoryCountID, lotNumberTransaction.abtInventoryCountLineID })))
			{
				errorsList.Add($"abtInventoryCountLineID [{lotNumberTransaction.abtInventoryCountLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberTransaction.abtDmrShipmentID) && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("DMRShipments", new object[1] { "DSPDMRSHIPMENTID" }, new object[1] { lotNumberTransaction.abtDmrShipmentID })))
			{
				errorsList.Add("abtDmrShipmentID [" + lotNumberTransaction.abtDmrShipmentID + "] not found.");
			}
			if (lotNumberTransaction.abtDmrShipmentLineID > 0 && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("DMRShipmentLines", new object[2] { "DSLDMRSHIPMENTID", "DSLDMRSHIPMENTLINEID" }, new object[2] { lotNumberTransaction.abtDmrShipmentID, lotNumberTransaction.abtDmrShipmentLineID })))
			{
				errorsList.Add($"abtDmrShipmentLineID [{lotNumberTransaction.abtDmrShipmentLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberTransaction.abtRmaReceiptID) && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("RMAReceipts", new object[1] { "RRPRMARECEIPTID" }, new object[1] { lotNumberTransaction.abtRmaReceiptID })))
			{
				errorsList.Add("abtRmaReceiptID [" + lotNumberTransaction.abtRmaReceiptID + "] not found.");
			}
			if (lotNumberTransaction.abtRmaReceiptLineID > 0 && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("RMAReceiptLines", new object[2] { "RRLRMARECEIPTID", "RRLRMARECEIPTLINEID" }, new object[2] { lotNumberTransaction.abtRmaReceiptID, lotNumberTransaction.abtRmaReceiptLineID })))
			{
				errorsList.Add($"abtRmaReceiptLineID [{lotNumberTransaction.abtRmaReceiptLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberTransaction.abtWarehouseTransferID) && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("WarehouseTransfers", new object[1] { "MWPWAREHOUSETRANSFERID" }, new object[1] { lotNumberTransaction.abtWarehouseTransferID })))
			{
				errorsList.Add("abtWarehouseTransferID [" + lotNumberTransaction.abtWarehouseTransferID + "] not found.");
			}
			if (lotNumberTransaction.abtWarehouseTransferLineID > 0 && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("WarehouseTransferLines", new object[2] { "MWLWAREHOUSETRANSFERID", "MWLWAREHOUSETRANSFERLINEID" }, new object[2] { lotNumberTransaction.abtWarehouseTransferID, lotNumberTransaction.abtWarehouseTransferLineID })))
			{
				errorsList.Add($"abtWarehouseTransferLineID [{lotNumberTransaction.abtWarehouseTransferLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberTransaction.abtWarehouseReceiptID) && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("WarehouseReceipts", new object[1] { "WRPWAREHOUSERECEIPTID" }, new object[1] { lotNumberTransaction.abtWarehouseReceiptID })))
			{
				errorsList.Add("abtWarehouseReceiptID [" + lotNumberTransaction.abtWarehouseReceiptID + "] not found.");
			}
			if (lotNumberTransaction.abtWarehouseReceiptLineID > 0 && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("WarehouseReceiptLines", new object[2] { "WRLWAREHOUSERECEIPTID", "WRLWAREHOUSERECEIPTLINEID" }, new object[2] { lotNumberTransaction.abtWarehouseReceiptID, lotNumberTransaction.abtWarehouseReceiptLineID })))
			{
				errorsList.Add($"abtWarehouseReceiptLineID [{lotNumberTransaction.abtWarehouseReceiptLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberTransaction.abtInspectionID) && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("Inspections", new object[1] { "QAPINSPECTIONID" }, new object[1] { lotNumberTransaction.abtInspectionID })))
			{
				errorsList.Add("abtInspectionID [" + lotNumberTransaction.abtInspectionID + "] not found.");
			}
			if (lotNumberTransaction.abtInspectionLineID > 0 && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("InspectionLines", new object[2] { "QALINSPECTIONID", "QALINSPECTIONLINEID" }, new object[2] { lotNumberTransaction.abtInspectionID, lotNumberTransaction.abtInspectionLineID })))
			{
				errorsList.Add($"abtInspectionLineID [{lotNumberTransaction.abtInspectionLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberTransaction.abtLandedCostID) && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("LandedCosts", new object[1] { "RMCLANDEDCOSTID" }, new object[1] { lotNumberTransaction.abtLandedCostID })))
			{
				errorsList.Add("abtLandedCostID [" + lotNumberTransaction.abtLandedCostID + "] not found.");
			}
			if (lotNumberTransaction.abtJobMaterialComponentID > 0 && !(await base.ERPLotNumberTransactionRepository.DoesRecordExistInTableUsingKeys("JobMaterialComponents", new object[4] { "JMTJOBID", "JMTJOBASSEMBLYID", "JMTJOBMATERIALID", "JMTJOBMATERIALCOMPONENTID" }, new object[4] { lotNumberTransaction.abtJobID, lotNumberTransaction.abtJobAssemblyID, lotNumberTransaction.abtJobMaterialID, lotNumberTransaction.abtJobMaterialComponentID })))
			{
				errorsList.Add($"abtJobMaterialComponentID [{lotNumberTransaction.abtJobMaterialComponentID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPLotNumberTransactionDto>>> Process_GetAllLotNumberTransactions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPLotNumberTransactionDto> allLotNumberTransactionsDto = new List<ERPLotNumberTransactionDto>();
		ERPResponseMessageDto<IList<ERPLotNumberTransactionDto>> result;
		try
		{
			IERPLotNumberTransactionRepository iERPLotNumberTransactionRepository = (base.ERPLotNumberTransactionRepository = new ERPLotNumberTransactionRepository(base.ApiClientContext));
			using (iERPLotNumberTransactionRepository)
			{
				foreach (ERPLotNumberTransactionInformationDto item2 in await base.ERPLotNumberTransactionRepository.GetAllLotNumberTransactions(pageSize, pageNumber, filter, orderBy))
				{
					ERPLotNumberTransactionDto item = new ERPLotNumberTransactionDto
					{
						abtCreatedBy = item2.abtCreatedBy,
						abtCreatedDate = item2.abtCreatedDate,
						abtDmrShipmentID = item2.abtDmrShipmentID,
						abtDmrShipmentLineID = item2.abtDmrShipmentLineID,
						abtUniqueID = item2.abtUniqueID,
						abtInspectionID = item2.abtInspectionID,
						abtInspectionLineID = item2.abtInspectionLineID,
						abtInventoryCountID = item2.abtInventoryCountID,
						abtInventoryCountLineID = item2.abtInventoryCountLineID,
						abtInProgress = item2.abtInProgress,
						abtInspect = item2.abtInspect,
						abtNegativeTransaction = item2.abtNegativeTransaction,
						abtNonInventoryTransaction = item2.abtNonInventoryTransaction,
						abtJobAssemblyID = item2.abtJobAssemblyID,
						abtJobID = item2.abtJobID,
						abtJobMaterialComponentID = item2.abtJobMaterialComponentID,
						abtJobMaterialID = item2.abtJobMaterialID,
						abtLandedCostID = item2.abtLandedCostID,
						abtLotNumberID = item2.abtLotNumberID,
						abtOldTransactionType = item2.abtOldTransactionType,
						abtPartBinID = item2.abtPartBinID,
						abtPartID = item2.abtPartID,
						abtPartRevisionID = item2.abtPartRevisionID,
						abtPartTransactionID = item2.abtPartTransactionID,
						abtPartWarehouseLocationID = item2.abtPartWarehouseLocationID,
						abtQuantity = item2.abtQuantity,
						abtQuantityToInspect = item2.abtQuantityToInspect,
						abtReceiptID = item2.abtReceiptID,
						abtReceiptLineID = item2.abtReceiptLineID,
						abtRmaReceiptID = item2.abtRmaReceiptID,
						abtRmaReceiptLineID = item2.abtRmaReceiptLineID,
						abtRowVersion = item2.abtRowVersion,
						abtLotNumberTransactionID = item2.abtLotNumberTransactionID,
						abtShipmentID = item2.abtShipmentID,
						abtShipmentLineID = item2.abtShipmentLineID,
						abtStatus = item2.abtStatus,
						abtTableName = item2.abtTableName,
						abtTableUniqueID = item2.abtTableUniqueID,
						abtTransactionDate = item2.abtTransactionDate,
						abtTransactionType = item2.abtTransactionType,
						abtWarehouseReceiptID = item2.abtWarehouseReceiptID,
						abtWarehouseReceiptLineID = item2.abtWarehouseReceiptLineID,
						abtWarehouseTransferID = item2.abtWarehouseTransferID,
						abtWarehouseTransferLineID = item2.abtWarehouseTransferLineID,
						CustomFields = item2.CustomFields
					};
					allLotNumberTransactionsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all LotNumberTransactions]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPLotNumberTransactionDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allLotNumberTransactionsDto,
				RecordCount = allLotNumberTransactionsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLotNumberTransactionDto>> Process_GetLotNumberTransaction(Guid lotNumberTransactionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPLotNumberTransactionDto lotNumberTransactionDto = null;
		ERPResponseMessageDto<ERPLotNumberTransactionDto> result;
		try
		{
			IERPLotNumberTransactionRepository iERPLotNumberTransactionRepository = (base.ERPLotNumberTransactionRepository = new ERPLotNumberTransactionRepository(base.ApiClientContext));
			using (iERPLotNumberTransactionRepository)
			{
				ERPLotNumberTransactionInformationDto eRPLotNumberTransactionInformationDto = await base.ERPLotNumberTransactionRepository.GetLotNumberTransaction(lotNumberTransactionId);
				lotNumberTransactionDto = new ERPLotNumberTransactionDto
				{
					abtCreatedBy = eRPLotNumberTransactionInformationDto.abtCreatedBy,
					abtCreatedDate = eRPLotNumberTransactionInformationDto.abtCreatedDate,
					abtDmrShipmentID = eRPLotNumberTransactionInformationDto.abtDmrShipmentID,
					abtDmrShipmentLineID = eRPLotNumberTransactionInformationDto.abtDmrShipmentLineID,
					abtUniqueID = eRPLotNumberTransactionInformationDto.abtUniqueID,
					abtInspectionID = eRPLotNumberTransactionInformationDto.abtInspectionID,
					abtInspectionLineID = eRPLotNumberTransactionInformationDto.abtInspectionLineID,
					abtInventoryCountID = eRPLotNumberTransactionInformationDto.abtInventoryCountID,
					abtInventoryCountLineID = eRPLotNumberTransactionInformationDto.abtInventoryCountLineID,
					abtInProgress = eRPLotNumberTransactionInformationDto.abtInProgress,
					abtInspect = eRPLotNumberTransactionInformationDto.abtInspect,
					abtNegativeTransaction = eRPLotNumberTransactionInformationDto.abtNegativeTransaction,
					abtNonInventoryTransaction = eRPLotNumberTransactionInformationDto.abtNonInventoryTransaction,
					abtJobAssemblyID = eRPLotNumberTransactionInformationDto.abtJobAssemblyID,
					abtJobID = eRPLotNumberTransactionInformationDto.abtJobID,
					abtJobMaterialComponentID = eRPLotNumberTransactionInformationDto.abtJobMaterialComponentID,
					abtJobMaterialID = eRPLotNumberTransactionInformationDto.abtJobMaterialID,
					abtLandedCostID = eRPLotNumberTransactionInformationDto.abtLandedCostID,
					abtLotNumberID = eRPLotNumberTransactionInformationDto.abtLotNumberID,
					abtOldTransactionType = eRPLotNumberTransactionInformationDto.abtOldTransactionType,
					abtPartBinID = eRPLotNumberTransactionInformationDto.abtPartBinID,
					abtPartID = eRPLotNumberTransactionInformationDto.abtPartID,
					abtPartRevisionID = eRPLotNumberTransactionInformationDto.abtPartRevisionID,
					abtPartTransactionID = eRPLotNumberTransactionInformationDto.abtPartTransactionID,
					abtPartWarehouseLocationID = eRPLotNumberTransactionInformationDto.abtPartWarehouseLocationID,
					abtQuantity = eRPLotNumberTransactionInformationDto.abtQuantity,
					abtQuantityToInspect = eRPLotNumberTransactionInformationDto.abtQuantityToInspect,
					abtReceiptID = eRPLotNumberTransactionInformationDto.abtReceiptID,
					abtReceiptLineID = eRPLotNumberTransactionInformationDto.abtReceiptLineID,
					abtRmaReceiptID = eRPLotNumberTransactionInformationDto.abtRmaReceiptID,
					abtRmaReceiptLineID = eRPLotNumberTransactionInformationDto.abtRmaReceiptLineID,
					abtRowVersion = eRPLotNumberTransactionInformationDto.abtRowVersion,
					abtLotNumberTransactionID = eRPLotNumberTransactionInformationDto.abtLotNumberTransactionID,
					abtShipmentID = eRPLotNumberTransactionInformationDto.abtShipmentID,
					abtShipmentLineID = eRPLotNumberTransactionInformationDto.abtShipmentLineID,
					abtStatus = eRPLotNumberTransactionInformationDto.abtStatus,
					abtTableName = eRPLotNumberTransactionInformationDto.abtTableName,
					abtTableUniqueID = eRPLotNumberTransactionInformationDto.abtTableUniqueID,
					abtTransactionDate = eRPLotNumberTransactionInformationDto.abtTransactionDate,
					abtTransactionType = eRPLotNumberTransactionInformationDto.abtTransactionType,
					abtWarehouseReceiptID = eRPLotNumberTransactionInformationDto.abtWarehouseReceiptID,
					abtWarehouseReceiptLineID = eRPLotNumberTransactionInformationDto.abtWarehouseReceiptLineID,
					abtWarehouseTransferID = eRPLotNumberTransactionInformationDto.abtWarehouseTransferID,
					abtWarehouseTransferLineID = eRPLotNumberTransactionInformationDto.abtWarehouseTransferLineID,
					CustomFields = eRPLotNumberTransactionInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the LotNumberTransactions []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLotNumberTransactionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = lotNumberTransactionDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLotNumberTransactionDto>> Process_PutLotNumberTransaction(ERPLotNumberTransactionDto lotNumberTransaction)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPLotNumberTransactionDto createdObject = null;
		ERPResponseMessageDto<ERPLotNumberTransactionDto> result;
		try
		{
			IERPLotNumberTransactionRepository iERPLotNumberTransactionRepository = (base.ERPLotNumberTransactionRepository = new ERPLotNumberTransactionRepository(base.ApiClientContext));
			using (iERPLotNumberTransactionRepository)
			{
				APIValidationInfoDto postResult = await base.ERPLotNumberTransactionRepository.SaveLotNumberTransaction(lotNumberTransaction);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPLotNumberTransactionInformationDto eRPLotNumberTransactionInformationDto = await base.ERPLotNumberTransactionRepository.GetLotNumberTransaction(lotNumberTransaction.abtUniqueID);
					createdObject = new ERPLotNumberTransactionDto
					{
						abtCreatedBy = eRPLotNumberTransactionInformationDto.abtCreatedBy,
						abtCreatedDate = eRPLotNumberTransactionInformationDto.abtCreatedDate,
						abtDmrShipmentID = eRPLotNumberTransactionInformationDto.abtDmrShipmentID,
						abtDmrShipmentLineID = eRPLotNumberTransactionInformationDto.abtDmrShipmentLineID,
						abtUniqueID = eRPLotNumberTransactionInformationDto.abtUniqueID,
						abtInspectionID = eRPLotNumberTransactionInformationDto.abtInspectionID,
						abtInspectionLineID = eRPLotNumberTransactionInformationDto.abtInspectionLineID,
						abtInventoryCountID = eRPLotNumberTransactionInformationDto.abtInventoryCountID,
						abtInventoryCountLineID = eRPLotNumberTransactionInformationDto.abtInventoryCountLineID,
						abtInProgress = eRPLotNumberTransactionInformationDto.abtInProgress,
						abtInspect = eRPLotNumberTransactionInformationDto.abtInspect,
						abtNegativeTransaction = eRPLotNumberTransactionInformationDto.abtNegativeTransaction,
						abtNonInventoryTransaction = eRPLotNumberTransactionInformationDto.abtNonInventoryTransaction,
						abtJobAssemblyID = eRPLotNumberTransactionInformationDto.abtJobAssemblyID,
						abtJobID = eRPLotNumberTransactionInformationDto.abtJobID,
						abtJobMaterialComponentID = eRPLotNumberTransactionInformationDto.abtJobMaterialComponentID,
						abtJobMaterialID = eRPLotNumberTransactionInformationDto.abtJobMaterialID,
						abtLandedCostID = eRPLotNumberTransactionInformationDto.abtLandedCostID,
						abtLotNumberID = eRPLotNumberTransactionInformationDto.abtLotNumberID,
						abtOldTransactionType = eRPLotNumberTransactionInformationDto.abtOldTransactionType,
						abtPartBinID = eRPLotNumberTransactionInformationDto.abtPartBinID,
						abtPartID = eRPLotNumberTransactionInformationDto.abtPartID,
						abtPartRevisionID = eRPLotNumberTransactionInformationDto.abtPartRevisionID,
						abtPartTransactionID = eRPLotNumberTransactionInformationDto.abtPartTransactionID,
						abtPartWarehouseLocationID = eRPLotNumberTransactionInformationDto.abtPartWarehouseLocationID,
						abtQuantity = eRPLotNumberTransactionInformationDto.abtQuantity,
						abtQuantityToInspect = eRPLotNumberTransactionInformationDto.abtQuantityToInspect,
						abtReceiptID = eRPLotNumberTransactionInformationDto.abtReceiptID,
						abtReceiptLineID = eRPLotNumberTransactionInformationDto.abtReceiptLineID,
						abtRmaReceiptID = eRPLotNumberTransactionInformationDto.abtRmaReceiptID,
						abtRmaReceiptLineID = eRPLotNumberTransactionInformationDto.abtRmaReceiptLineID,
						abtRowVersion = eRPLotNumberTransactionInformationDto.abtRowVersion,
						abtLotNumberTransactionID = eRPLotNumberTransactionInformationDto.abtLotNumberTransactionID,
						abtShipmentID = eRPLotNumberTransactionInformationDto.abtShipmentID,
						abtShipmentLineID = eRPLotNumberTransactionInformationDto.abtShipmentLineID,
						abtStatus = eRPLotNumberTransactionInformationDto.abtStatus,
						abtTableName = eRPLotNumberTransactionInformationDto.abtTableName,
						abtTableUniqueID = eRPLotNumberTransactionInformationDto.abtTableUniqueID,
						abtTransactionDate = eRPLotNumberTransactionInformationDto.abtTransactionDate,
						abtTransactionType = eRPLotNumberTransactionInformationDto.abtTransactionType,
						abtWarehouseReceiptID = eRPLotNumberTransactionInformationDto.abtWarehouseReceiptID,
						abtWarehouseReceiptLineID = eRPLotNumberTransactionInformationDto.abtWarehouseReceiptLineID,
						abtWarehouseTransferID = eRPLotNumberTransactionInformationDto.abtWarehouseTransferID,
						abtWarehouseTransferLineID = eRPLotNumberTransactionInformationDto.abtWarehouseTransferLineID,
						CustomFields = eRPLotNumberTransactionInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing LotNumberTransaction [{lotNumberTransaction.abtUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLotNumberTransactionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteLotNumberTransaction(Guid lotNumberTransactionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLotNumberTransactionRepository iERPLotNumberTransactionRepository = (base.ERPLotNumberTransactionRepository = new ERPLotNumberTransactionRepository(base.ApiClientContext));
		using (iERPLotNumberTransactionRepository)
		{
			if (!(await base.ERPLotNumberTransactionRepository.DoesLotNumberTransactionExist(lotNumberTransactionId)))
			{
				base.ErrorsList.Add($"LotNumberTransaction [{lotNumberTransactionId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPLotNumberTransactionInformationDto eRPLotNumberTransactionInformationDto = await base.ERPLotNumberTransactionRepository.GetLotNumberTransaction(lotNumberTransactionId);
				string text = await base.ERPLotNumberTransactionRepository.WhereUsed("LotNumberTransactions", new object[1] { eRPLotNumberTransactionInformationDto.abtLotNumberTransactionID }, new object[1] { "abtLotNumberTransactionID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("LotNumberTransaction cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPLotNumberTransactionDto>> Process_DeleteLotNumberTransaction(Guid lotNumberTransactionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPLotNumberTransactionDto> result;
		try
		{
			IERPLotNumberTransactionRepository iERPLotNumberTransactionRepository = (base.ERPLotNumberTransactionRepository = new ERPLotNumberTransactionRepository(base.ApiClientContext));
			using (iERPLotNumberTransactionRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPLotNumberTransactionRepository.DeleteRowFromTable("LotNumberTransactions", "abt", lotNumberTransactionId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of LotNumberTransaction [{lotNumberTransactionId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLotNumberTransactionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPLotNumberTransactionDto()
			};
		}
		return result;
	}
}
