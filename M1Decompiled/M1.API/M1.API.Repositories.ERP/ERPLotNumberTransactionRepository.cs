using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Utilities;
using M1.Extensions;

namespace M1.API.Repositories.ERP;

public class ERPLotNumberTransactionRepository : APIBaseRepository, IERPLotNumberTransactionRepository, IAPIBaseRepository, IDisposable
{
	public ERPLotNumberTransactionRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesLotNumberTransactionExist(Guid lotNumberTransactionId)
	{
		InitializeParameterLists();
		base.filterList.Add("abtUniqueID|C", lotNumberTransactionId);
		base.selectList.Add("abtUniqueID");
		return Task.FromResult(GetAsObject("LotNumberTransactions", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPLotNumberTransactionInformationDto>> GetAllLotNumberTransactions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPLotNumberTransactionInformationDto> collection = new List<ERPLotNumberTransactionInformationDto>();
		InitializeParameterLists();
		string[] array = new string[44]
		{
			"abtCreatedBy", "abtCreatedDate", "abtDmrShipmentID", "abtDmrShipmentLineID", "abtUniqueID", "abtInspectionID", "abtInspectionLineID", "abtInventoryCountID", "abtInventoryCountLineID", "abtInProgress",
			"abtInspect", "abtNegativeTransaction", "abtNonInventoryTransaction", "abtJobAssemblyID", "abtJobID", "abtJobMaterialComponentID", "abtJobMaterialID", "abtLandedCostID", "abtLotNumberID", "abtOldTransactionType",
			"abtPartBinID", "abtPartID", "abtPartRevisionID", "abtPartTransactionID", "abtPartWarehouseLocationID", "abtQuantity", "abtQuantityToInspect", "abtReceiptID", "abtReceiptLineID", "abtRmaReceiptID",
			"abtRmaReceiptLineID", "abtRowVersion", "abtLotNumberTransactionID", "abtShipmentID", "abtShipmentLineID", "abtStatus", "abtTableName", "abtTableUniqueID", "abtTransactionDate", "abtTransactionType",
			"abtWarehouseReceiptID", "abtWarehouseReceiptLineID", "abtWarehouseTransferID", "abtWarehouseTransferLineID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("LotNumberTransactions");
		List<string> list = new List<string>();
		string[] fields = ((base.selectList.Count != array.Count()) ? base.selectList.ToArray() : array);
		if (orderBy != null && orderBy.Length > 0)
		{
			ParseAndAddOrderByFields(orderBy, list, fields);
		}
		if (list.Count == 0)
		{
			list = new List<string> { "1" };
		}
		if (filter != null && filter.Length != 0)
		{
			ParseAndAddFilter(filter, base.filterList, fields);
		}
		using (DataTable dataTable = GetAsDataTable("LotNumberTransactions", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPLotNumberTransactionInformationDto eRPLotNumberTransactionInformationDto = new ERPLotNumberTransactionInformationDto();
				eRPLotNumberTransactionInformationDto.abtCreatedBy = dataTable.Rows[i].Field<string>("abtCreatedBy");
				eRPLotNumberTransactionInformationDto.abtCreatedDate = dataTable.Rows[i].Field<DateTime?>("abtCreatedDate");
				eRPLotNumberTransactionInformationDto.abtDmrShipmentID = dataTable.Rows[i].Field<string>("abtDmrShipmentID");
				eRPLotNumberTransactionInformationDto.abtDmrShipmentLineID = dataTable.Rows[i].Field<short>("abtDmrShipmentLineID");
				eRPLotNumberTransactionInformationDto.abtUniqueID = dataTable.Rows[i].Field<Guid>("abtUniqueID");
				eRPLotNumberTransactionInformationDto.abtInspectionID = dataTable.Rows[i].Field<string>("abtInspectionID");
				eRPLotNumberTransactionInformationDto.abtInspectionLineID = dataTable.Rows[i].Field<short>("abtInspectionLineID");
				eRPLotNumberTransactionInformationDto.abtInventoryCountID = dataTable.Rows[i].Field<int>("abtInventoryCountID");
				eRPLotNumberTransactionInformationDto.abtInventoryCountLineID = dataTable.Rows[i].Field<int>("abtInventoryCountLineID");
				eRPLotNumberTransactionInformationDto.abtInProgress = dataTable.Rows[i].Field<bool>("abtInProgress");
				eRPLotNumberTransactionInformationDto.abtInspect = dataTable.Rows[i].Field<bool>("abtInspect");
				eRPLotNumberTransactionInformationDto.abtNegativeTransaction = dataTable.Rows[i].Field<bool>("abtNegativeTransaction");
				eRPLotNumberTransactionInformationDto.abtNonInventoryTransaction = dataTable.Rows[i].Field<bool>("abtNonInventoryTransaction");
				eRPLotNumberTransactionInformationDto.abtJobAssemblyID = dataTable.Rows[i].Field<int>("abtJobAssemblyID");
				eRPLotNumberTransactionInformationDto.abtJobID = dataTable.Rows[i].Field<string>("abtJobID");
				eRPLotNumberTransactionInformationDto.abtJobMaterialComponentID = dataTable.Rows[i].Field<int>("abtJobMaterialComponentID");
				eRPLotNumberTransactionInformationDto.abtJobMaterialID = dataTable.Rows[i].Field<int>("abtJobMaterialID");
				eRPLotNumberTransactionInformationDto.abtLandedCostID = dataTable.Rows[i].Field<string>("abtLandedCostID");
				eRPLotNumberTransactionInformationDto.abtLotNumberID = dataTable.Rows[i].Field<string>("abtLotNumberID");
				eRPLotNumberTransactionInformationDto.abtOldTransactionType = dataTable.Rows[i].Field<byte>("abtOldTransactionType");
				eRPLotNumberTransactionInformationDto.abtPartBinID = dataTable.Rows[i].Field<string>("abtPartBinID");
				eRPLotNumberTransactionInformationDto.abtPartID = dataTable.Rows[i].Field<string>("abtPartID");
				eRPLotNumberTransactionInformationDto.abtPartRevisionID = dataTable.Rows[i].Field<string>("abtPartRevisionID");
				eRPLotNumberTransactionInformationDto.abtPartTransactionID = dataTable.Rows[i].Field<int>("abtPartTransactionID");
				eRPLotNumberTransactionInformationDto.abtPartWarehouseLocationID = dataTable.Rows[i].Field<string>("abtPartWarehouseLocationID");
				eRPLotNumberTransactionInformationDto.abtQuantity = dataTable.Rows[i].Field<decimal>("abtQuantity");
				eRPLotNumberTransactionInformationDto.abtQuantityToInspect = dataTable.Rows[i].Field<decimal>("abtQuantityToInspect");
				eRPLotNumberTransactionInformationDto.abtReceiptID = dataTable.Rows[i].Field<string>("abtReceiptID");
				eRPLotNumberTransactionInformationDto.abtReceiptLineID = dataTable.Rows[i].Field<short>("abtReceiptLineID");
				eRPLotNumberTransactionInformationDto.abtRmaReceiptID = dataTable.Rows[i].Field<string>("abtRmaReceiptID");
				eRPLotNumberTransactionInformationDto.abtRmaReceiptLineID = dataTable.Rows[i].Field<short>("abtRmaReceiptLineID");
				eRPLotNumberTransactionInformationDto.abtRowVersion = dataTable.Rows[i].Field<byte[]>("abtRowVersion");
				eRPLotNumberTransactionInformationDto.abtLotNumberTransactionID = dataTable.Rows[i].Field<int>("abtLotNumberTransactionID");
				eRPLotNumberTransactionInformationDto.abtShipmentID = dataTable.Rows[i].Field<string>("abtShipmentID");
				eRPLotNumberTransactionInformationDto.abtShipmentLineID = dataTable.Rows[i].Field<short>("abtShipmentLineID");
				eRPLotNumberTransactionInformationDto.abtStatus = dataTable.Rows[i].Field<byte>("abtStatus");
				eRPLotNumberTransactionInformationDto.abtTableName = dataTable.Rows[i].Field<string>("abtTableName");
				eRPLotNumberTransactionInformationDto.abtTableUniqueID = dataTable.Rows[i].Field<Guid>("abtTableUniqueID");
				eRPLotNumberTransactionInformationDto.abtTransactionDate = dataTable.Rows[i].Field<DateTime?>("abtTransactionDate");
				eRPLotNumberTransactionInformationDto.abtTransactionType = dataTable.Rows[i].Field<byte>("abtTransactionType");
				eRPLotNumberTransactionInformationDto.abtWarehouseReceiptID = dataTable.Rows[i].Field<string>("abtWarehouseReceiptID");
				eRPLotNumberTransactionInformationDto.abtWarehouseReceiptLineID = dataTable.Rows[i].Field<short>("abtWarehouseReceiptLineID");
				eRPLotNumberTransactionInformationDto.abtWarehouseTransferID = dataTable.Rows[i].Field<string>("abtWarehouseTransferID");
				eRPLotNumberTransactionInformationDto.abtWarehouseTransferLineID = dataTable.Rows[i].Field<short>("abtWarehouseTransferLineID");
				eRPLotNumberTransactionInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPLotNumberTransactionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPLotNumberTransactionInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPLotNumberTransactionInformationDto> GetLotNumberTransaction(Guid lotNumberTransactionId)
	{
		ERPLotNumberTransactionInformationDto eRPLotNumberTransactionInformationDto = new ERPLotNumberTransactionInformationDto();
		InitializeParameterLists();
		string[] collection = new string[44]
		{
			"abtCreatedBy", "abtCreatedDate", "abtDmrShipmentID", "abtDmrShipmentLineID", "abtUniqueID", "abtInspectionID", "abtInspectionLineID", "abtInventoryCountID", "abtInventoryCountLineID", "abtInProgress",
			"abtInspect", "abtNegativeTransaction", "abtNonInventoryTransaction", "abtJobAssemblyID", "abtJobID", "abtJobMaterialComponentID", "abtJobMaterialID", "abtLandedCostID", "abtLotNumberID", "abtOldTransactionType",
			"abtPartBinID", "abtPartID", "abtPartRevisionID", "abtPartTransactionID", "abtPartWarehouseLocationID", "abtQuantity", "abtQuantityToInspect", "abtReceiptID", "abtReceiptLineID", "abtRmaReceiptID",
			"abtRmaReceiptLineID", "abtRowVersion", "abtLotNumberTransactionID", "abtShipmentID", "abtShipmentLineID", "abtStatus", "abtTableName", "abtTableUniqueID", "abtTransactionDate", "abtTransactionType",
			"abtWarehouseReceiptID", "abtWarehouseReceiptLineID", "abtWarehouseTransferID", "abtWarehouseTransferLineID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("abtUniqueID|C", lotNumberTransactionId);
		AddCustomFieldsToSelectList("LotNumberTransactions");
		using (DataTable dataTable = GetAsDataTable("LotNumberTransactions", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPLotNumberTransactionInformationDto);
			}
			eRPLotNumberTransactionInformationDto.abtCreatedBy = dataTable.Rows[0].Field<string>("abtCreatedBy");
			eRPLotNumberTransactionInformationDto.abtCreatedDate = dataTable.Rows[0].Field<DateTime?>("abtCreatedDate");
			eRPLotNumberTransactionInformationDto.abtDmrShipmentID = dataTable.Rows[0].Field<string>("abtDmrShipmentID");
			eRPLotNumberTransactionInformationDto.abtDmrShipmentLineID = dataTable.Rows[0].Field<short>("abtDmrShipmentLineID");
			eRPLotNumberTransactionInformationDto.abtUniqueID = dataTable.Rows[0].Field<Guid>("abtUniqueID");
			eRPLotNumberTransactionInformationDto.abtInspectionID = dataTable.Rows[0].Field<string>("abtInspectionID");
			eRPLotNumberTransactionInformationDto.abtInspectionLineID = dataTable.Rows[0].Field<short>("abtInspectionLineID");
			eRPLotNumberTransactionInformationDto.abtInventoryCountID = dataTable.Rows[0].Field<int>("abtInventoryCountID");
			eRPLotNumberTransactionInformationDto.abtInventoryCountLineID = dataTable.Rows[0].Field<int>("abtInventoryCountLineID");
			eRPLotNumberTransactionInformationDto.abtInProgress = dataTable.Rows[0].Field<bool>("abtInProgress");
			eRPLotNumberTransactionInformationDto.abtInspect = dataTable.Rows[0].Field<bool>("abtInspect");
			eRPLotNumberTransactionInformationDto.abtNegativeTransaction = dataTable.Rows[0].Field<bool>("abtNegativeTransaction");
			eRPLotNumberTransactionInformationDto.abtNonInventoryTransaction = dataTable.Rows[0].Field<bool>("abtNonInventoryTransaction");
			eRPLotNumberTransactionInformationDto.abtJobAssemblyID = dataTable.Rows[0].Field<int>("abtJobAssemblyID");
			eRPLotNumberTransactionInformationDto.abtJobID = dataTable.Rows[0].Field<string>("abtJobID");
			eRPLotNumberTransactionInformationDto.abtJobMaterialComponentID = dataTable.Rows[0].Field<int>("abtJobMaterialComponentID");
			eRPLotNumberTransactionInformationDto.abtJobMaterialID = dataTable.Rows[0].Field<int>("abtJobMaterialID");
			eRPLotNumberTransactionInformationDto.abtLandedCostID = dataTable.Rows[0].Field<string>("abtLandedCostID");
			eRPLotNumberTransactionInformationDto.abtLotNumberID = dataTable.Rows[0].Field<string>("abtLotNumberID");
			eRPLotNumberTransactionInformationDto.abtOldTransactionType = dataTable.Rows[0].Field<byte>("abtOldTransactionType");
			eRPLotNumberTransactionInformationDto.abtPartBinID = dataTable.Rows[0].Field<string>("abtPartBinID");
			eRPLotNumberTransactionInformationDto.abtPartID = dataTable.Rows[0].Field<string>("abtPartID");
			eRPLotNumberTransactionInformationDto.abtPartRevisionID = dataTable.Rows[0].Field<string>("abtPartRevisionID");
			eRPLotNumberTransactionInformationDto.abtPartTransactionID = dataTable.Rows[0].Field<int>("abtPartTransactionID");
			eRPLotNumberTransactionInformationDto.abtPartWarehouseLocationID = dataTable.Rows[0].Field<string>("abtPartWarehouseLocationID");
			eRPLotNumberTransactionInformationDto.abtQuantity = dataTable.Rows[0].Field<decimal>("abtQuantity");
			eRPLotNumberTransactionInformationDto.abtQuantityToInspect = dataTable.Rows[0].Field<decimal>("abtQuantityToInspect");
			eRPLotNumberTransactionInformationDto.abtReceiptID = dataTable.Rows[0].Field<string>("abtReceiptID");
			eRPLotNumberTransactionInformationDto.abtReceiptLineID = dataTable.Rows[0].Field<short>("abtReceiptLineID");
			eRPLotNumberTransactionInformationDto.abtRmaReceiptID = dataTable.Rows[0].Field<string>("abtRmaReceiptID");
			eRPLotNumberTransactionInformationDto.abtRmaReceiptLineID = dataTable.Rows[0].Field<short>("abtRmaReceiptLineID");
			eRPLotNumberTransactionInformationDto.abtRowVersion = dataTable.Rows[0].Field<byte[]>("abtRowVersion");
			eRPLotNumberTransactionInformationDto.abtLotNumberTransactionID = dataTable.Rows[0].Field<int>("abtLotNumberTransactionID");
			eRPLotNumberTransactionInformationDto.abtShipmentID = dataTable.Rows[0].Field<string>("abtShipmentID");
			eRPLotNumberTransactionInformationDto.abtShipmentLineID = dataTable.Rows[0].Field<short>("abtShipmentLineID");
			eRPLotNumberTransactionInformationDto.abtStatus = dataTable.Rows[0].Field<byte>("abtStatus");
			eRPLotNumberTransactionInformationDto.abtTableName = dataTable.Rows[0].Field<string>("abtTableName");
			eRPLotNumberTransactionInformationDto.abtTableUniqueID = dataTable.Rows[0].Field<Guid>("abtTableUniqueID");
			eRPLotNumberTransactionInformationDto.abtTransactionDate = dataTable.Rows[0].Field<DateTime?>("abtTransactionDate");
			eRPLotNumberTransactionInformationDto.abtTransactionType = dataTable.Rows[0].Field<byte>("abtTransactionType");
			eRPLotNumberTransactionInformationDto.abtWarehouseReceiptID = dataTable.Rows[0].Field<string>("abtWarehouseReceiptID");
			eRPLotNumberTransactionInformationDto.abtWarehouseReceiptLineID = dataTable.Rows[0].Field<short>("abtWarehouseReceiptLineID");
			eRPLotNumberTransactionInformationDto.abtWarehouseTransferID = dataTable.Rows[0].Field<string>("abtWarehouseTransferID");
			eRPLotNumberTransactionInformationDto.abtWarehouseTransferLineID = dataTable.Rows[0].Field<short>("abtWarehouseTransferLineID");
			eRPLotNumberTransactionInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPLotNumberTransactionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPLotNumberTransactionInformationDto);
	}

	public Task<APIValidationInfoDto> SaveLotNumberTransaction(ERPLotNumberTransactionDto lotNumberTransaction)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM LotNumberTransactions WHERE abtUniqueID = " + M1Util.ConvertToLinq(lotNumberTransaction.abtUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["abtLotNumberTransactionID"] = lotNumberTransaction.abtLotNumberTransactionID;
				lotNumberTransaction.abtUniqueID = ((lotNumberTransaction.abtUniqueID == Guid.Empty) ? Guid.NewGuid() : lotNumberTransaction.abtUniqueID);
				dataRow["abtUniqueID"] = lotNumberTransaction.abtUniqueID;
				dataRow["abtCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["abtCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The LotNumberTransaction could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (lotNumberTransaction.abtRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the LotNumberTransaction is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["abtRowVersion"], lotNumberTransaction.abtRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the LotNumberTransaction has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the LotNumberTransaction again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["abtDmrShipmentID"] = lotNumberTransaction.abtDmrShipmentID;
			dataRow["abtDmrShipmentLineID"] = lotNumberTransaction.abtDmrShipmentLineID;
			dataRow["abtInspectionID"] = lotNumberTransaction.abtInspectionID;
			dataRow["abtInspectionLineID"] = lotNumberTransaction.abtInspectionLineID;
			dataRow["abtInventoryCountID"] = lotNumberTransaction.abtInventoryCountID;
			dataRow["abtInventoryCountLineID"] = lotNumberTransaction.abtInventoryCountLineID;
			dataRow["abtInProgress"] = lotNumberTransaction.abtInProgress;
			dataRow["abtInspect"] = lotNumberTransaction.abtInspect;
			dataRow["abtNegativeTransaction"] = lotNumberTransaction.abtNegativeTransaction;
			dataRow["abtNonInventoryTransaction"] = lotNumberTransaction.abtNonInventoryTransaction;
			dataRow["abtJobAssemblyID"] = lotNumberTransaction.abtJobAssemblyID;
			dataRow["abtJobID"] = lotNumberTransaction.abtJobID;
			dataRow["abtJobMaterialComponentID"] = lotNumberTransaction.abtJobMaterialComponentID;
			dataRow["abtJobMaterialID"] = lotNumberTransaction.abtJobMaterialID;
			dataRow["abtLandedCostID"] = lotNumberTransaction.abtLandedCostID;
			dataRow["abtLotNumberID"] = lotNumberTransaction.abtLotNumberID;
			dataRow["abtOldTransactionType"] = lotNumberTransaction.abtOldTransactionType;
			dataRow["abtPartBinID"] = lotNumberTransaction.abtPartBinID;
			dataRow["abtPartID"] = lotNumberTransaction.abtPartID;
			dataRow["abtPartRevisionID"] = lotNumberTransaction.abtPartRevisionID;
			dataRow["abtPartTransactionID"] = lotNumberTransaction.abtPartTransactionID;
			dataRow["abtPartWarehouseLocationID"] = lotNumberTransaction.abtPartWarehouseLocationID;
			dataRow["abtQuantity"] = lotNumberTransaction.abtQuantity;
			dataRow["abtQuantityToInspect"] = lotNumberTransaction.abtQuantityToInspect;
			dataRow["abtReceiptID"] = lotNumberTransaction.abtReceiptID;
			dataRow["abtReceiptLineID"] = lotNumberTransaction.abtReceiptLineID;
			dataRow["abtRmaReceiptID"] = lotNumberTransaction.abtRmaReceiptID;
			dataRow["abtRmaReceiptLineID"] = lotNumberTransaction.abtRmaReceiptLineID;
			dataRow["abtShipmentID"] = lotNumberTransaction.abtShipmentID;
			dataRow["abtShipmentLineID"] = lotNumberTransaction.abtShipmentLineID;
			dataRow["abtStatus"] = lotNumberTransaction.abtStatus;
			dataRow["abtTableName"] = lotNumberTransaction.abtTableName;
			dataRow["abtTableUniqueID"] = lotNumberTransaction.abtTableUniqueID;
			DataRow dataRow2 = dataRow;
			DateTime? abtTransactionDate = lotNumberTransaction.abtTransactionDate;
			dataRow2["abtTransactionDate"] = (abtTransactionDate.HasValue ? ((object)abtTransactionDate.GetValueOrDefault()) : dataRow["abtTransactionDate"]);
			dataRow["abtTransactionType"] = lotNumberTransaction.abtTransactionType;
			dataRow["abtWarehouseReceiptID"] = lotNumberTransaction.abtWarehouseReceiptID;
			dataRow["abtWarehouseReceiptLineID"] = lotNumberTransaction.abtWarehouseReceiptLineID;
			dataRow["abtWarehouseTransferID"] = lotNumberTransaction.abtWarehouseTransferID;
			dataRow["abtWarehouseTransferLineID"] = lotNumberTransaction.abtWarehouseTransferLineID;
			if (lotNumberTransaction.CustomFields != null && lotNumberTransaction.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in lotNumberTransaction.CustomFields)
				{
					if (dataTable.Columns.Contains(customField.Key))
					{
						dataRow[customField.Key] = customField.Value;
					}
				}
			}
			dataRow.EndEdit();
			if (flag)
			{
				dataTable.Rows.Add(dataRow);
			}
			if (base.M1database.UpdateData(dataTable, adapter))
			{
				if (flag)
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Created;
				}
				else
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.OK;
				}
			}
			else
			{
				aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.InternalServerError;
			}
		}
		catch (SqlException ex)
		{
			SqlErrorResult httpStatusCodeForSqlException = SqlExceptionMapper.GetHttpStatusCodeForSqlException(ex);
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the LotNumberTransaction [{lotNumberTransaction.abtUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the LotNumberTransaction [{lotNumberTransaction.abtUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
