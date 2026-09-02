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

public class ERPSerialNumberTransactionRepository : APIBaseRepository, IERPSerialNumberTransactionRepository, IAPIBaseRepository, IDisposable
{
	public ERPSerialNumberTransactionRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSerialNumberTransactionExist(Guid serialNumberTransactionId)
	{
		InitializeParameterLists();
		base.filterList.Add("sntUniqueID|C", serialNumberTransactionId);
		base.selectList.Add("sntUniqueID");
		return Task.FromResult(GetAsObject("SerialNumberTransactions", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSerialNumberTransactionInformationDto>> GetAllSerialNumberTransactions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSerialNumberTransactionInformationDto> collection = new List<ERPSerialNumberTransactionInformationDto>();
		InitializeParameterLists();
		string[] array = new string[46]
		{
			"sntCreatedBy", "sntCreatedDate", "sntDmrShipmentID", "sntDmrShipmentLineID", "sntUniqueID", "sntInspectionID", "sntInspectionLineID", "sntInventoryCountID", "sntInventoryCountLineID", "sntInspect",
			"sntNegativeTransaction", "sntJobAssemblyID", "sntJobID", "sntJobMaterialComponentID", "sntJobMaterialID", "sntJobPartBinID", "sntJobPartID", "sntJobPartRevisionID", "sntJobPartWarehouseLocationID", "sntJobSerialNumberID",
			"sntLandedCostID", "sntOldTransactionType", "sntPartBinID", "sntPartID", "sntPartRevisionID", "sntPartTransactionID", "sntPartWarehouseLocationID", "sntQuantity", "sntReceiptID", "sntReceiptLineID",
			"sntRmaReceiptID", "sntRmaReceiptLineID", "sntRowVersion", "sntSerialNumberTransactionID", "sntSerialNumberID", "sntShipmentID", "sntShipmentLineID", "sntStatus", "sntTableName", "sntTableUniqueID",
			"sntTransactionDate", "sntTransactionType", "sntWarehouseReceiptID", "sntWarehouseReceiptLineID", "sntWarehouseTransferID", "sntWarehouseTransferLineID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("SerialNumberTransactions");
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
		using (DataTable dataTable = GetAsDataTable("SerialNumberTransactions", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSerialNumberTransactionInformationDto eRPSerialNumberTransactionInformationDto = new ERPSerialNumberTransactionInformationDto();
				eRPSerialNumberTransactionInformationDto.sntCreatedBy = dataTable.Rows[i].Field<string>("sntCreatedBy");
				eRPSerialNumberTransactionInformationDto.sntCreatedDate = dataTable.Rows[i].Field<DateTime?>("sntCreatedDate");
				eRPSerialNumberTransactionInformationDto.sntDmrShipmentID = dataTable.Rows[i].Field<string>("sntDmrShipmentID");
				eRPSerialNumberTransactionInformationDto.sntDmrShipmentLineID = dataTable.Rows[i].Field<short>("sntDmrShipmentLineID");
				eRPSerialNumberTransactionInformationDto.sntUniqueID = dataTable.Rows[i].Field<Guid>("sntUniqueID");
				eRPSerialNumberTransactionInformationDto.sntInspectionID = dataTable.Rows[i].Field<string>("sntInspectionID");
				eRPSerialNumberTransactionInformationDto.sntInspectionLineID = dataTable.Rows[i].Field<short>("sntInspectionLineID");
				eRPSerialNumberTransactionInformationDto.sntInventoryCountID = dataTable.Rows[i].Field<int>("sntInventoryCountID");
				eRPSerialNumberTransactionInformationDto.sntInventoryCountLineID = dataTable.Rows[i].Field<int>("sntInventoryCountLineID");
				eRPSerialNumberTransactionInformationDto.sntInspect = dataTable.Rows[i].Field<bool>("sntInspect");
				eRPSerialNumberTransactionInformationDto.sntNegativeTransaction = dataTable.Rows[i].Field<bool>("sntNegativeTransaction");
				eRPSerialNumberTransactionInformationDto.sntJobAssemblyID = dataTable.Rows[i].Field<int>("sntJobAssemblyID");
				eRPSerialNumberTransactionInformationDto.sntJobID = dataTable.Rows[i].Field<string>("sntJobID");
				eRPSerialNumberTransactionInformationDto.sntJobMaterialComponentID = dataTable.Rows[i].Field<int>("sntJobMaterialComponentID");
				eRPSerialNumberTransactionInformationDto.sntJobMaterialID = dataTable.Rows[i].Field<int>("sntJobMaterialID");
				eRPSerialNumberTransactionInformationDto.sntJobPartBinID = dataTable.Rows[i].Field<string>("sntJobPartBinID");
				eRPSerialNumberTransactionInformationDto.sntJobPartID = dataTable.Rows[i].Field<string>("sntJobPartID");
				eRPSerialNumberTransactionInformationDto.sntJobPartRevisionID = dataTable.Rows[i].Field<string>("sntJobPartRevisionID");
				eRPSerialNumberTransactionInformationDto.sntJobPartWarehouseLocationID = dataTable.Rows[i].Field<string>("sntJobPartWarehouseLocationID");
				eRPSerialNumberTransactionInformationDto.sntJobSerialNumberID = dataTable.Rows[i].Field<string>("sntJobSerialNumberID");
				eRPSerialNumberTransactionInformationDto.sntLandedCostID = dataTable.Rows[i].Field<string>("sntLandedCostID");
				eRPSerialNumberTransactionInformationDto.sntOldTransactionType = dataTable.Rows[i].Field<byte>("sntOldTransactionType");
				eRPSerialNumberTransactionInformationDto.sntPartBinID = dataTable.Rows[i].Field<string>("sntPartBinID");
				eRPSerialNumberTransactionInformationDto.sntPartID = dataTable.Rows[i].Field<string>("sntPartID");
				eRPSerialNumberTransactionInformationDto.sntPartRevisionID = dataTable.Rows[i].Field<string>("sntPartRevisionID");
				eRPSerialNumberTransactionInformationDto.sntPartTransactionID = dataTable.Rows[i].Field<int>("sntPartTransactionID");
				eRPSerialNumberTransactionInformationDto.sntPartWarehouseLocationID = dataTable.Rows[i].Field<string>("sntPartWarehouseLocationID");
				eRPSerialNumberTransactionInformationDto.sntQuantity = dataTable.Rows[i].Field<decimal>("sntQuantity");
				eRPSerialNumberTransactionInformationDto.sntReceiptID = dataTable.Rows[i].Field<string>("sntReceiptID");
				eRPSerialNumberTransactionInformationDto.sntReceiptLineID = dataTable.Rows[i].Field<short>("sntReceiptLineID");
				eRPSerialNumberTransactionInformationDto.sntRmaReceiptID = dataTable.Rows[i].Field<string>("sntRmaReceiptID");
				eRPSerialNumberTransactionInformationDto.sntRmaReceiptLineID = dataTable.Rows[i].Field<short>("sntRmaReceiptLineID");
				eRPSerialNumberTransactionInformationDto.sntRowVersion = dataTable.Rows[i].Field<byte[]>("sntRowVersion");
				eRPSerialNumberTransactionInformationDto.sntSerialNumberTransactionID = dataTable.Rows[i].Field<int>("sntSerialNumberTransactionID");
				eRPSerialNumberTransactionInformationDto.sntSerialNumberID = dataTable.Rows[i].Field<string>("sntSerialNumberID");
				eRPSerialNumberTransactionInformationDto.sntShipmentID = dataTable.Rows[i].Field<string>("sntShipmentID");
				eRPSerialNumberTransactionInformationDto.sntShipmentLineID = dataTable.Rows[i].Field<short>("sntShipmentLineID");
				eRPSerialNumberTransactionInformationDto.sntStatus = dataTable.Rows[i].Field<byte>("sntStatus");
				eRPSerialNumberTransactionInformationDto.sntTableName = dataTable.Rows[i].Field<string>("sntTableName");
				eRPSerialNumberTransactionInformationDto.sntTableUniqueID = dataTable.Rows[i].Field<Guid>("sntTableUniqueID");
				eRPSerialNumberTransactionInformationDto.sntTransactionDate = dataTable.Rows[i].Field<DateTime?>("sntTransactionDate");
				eRPSerialNumberTransactionInformationDto.sntTransactionType = dataTable.Rows[i].Field<byte>("sntTransactionType");
				eRPSerialNumberTransactionInformationDto.sntWarehouseReceiptID = dataTable.Rows[i].Field<string>("sntWarehouseReceiptID");
				eRPSerialNumberTransactionInformationDto.sntWarehouseReceiptLineID = dataTable.Rows[i].Field<short>("sntWarehouseReceiptLineID");
				eRPSerialNumberTransactionInformationDto.sntWarehouseTransferID = dataTable.Rows[i].Field<string>("sntWarehouseTransferID");
				eRPSerialNumberTransactionInformationDto.sntWarehouseTransferLineID = dataTable.Rows[i].Field<short>("sntWarehouseTransferLineID");
				eRPSerialNumberTransactionInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSerialNumberTransactionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSerialNumberTransactionInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSerialNumberTransactionInformationDto> GetSerialNumberTransaction(Guid serialNumberTransactionId)
	{
		ERPSerialNumberTransactionInformationDto eRPSerialNumberTransactionInformationDto = new ERPSerialNumberTransactionInformationDto();
		InitializeParameterLists();
		string[] collection = new string[46]
		{
			"sntCreatedBy", "sntCreatedDate", "sntDmrShipmentID", "sntDmrShipmentLineID", "sntUniqueID", "sntInspectionID", "sntInspectionLineID", "sntInventoryCountID", "sntInventoryCountLineID", "sntInspect",
			"sntNegativeTransaction", "sntJobAssemblyID", "sntJobID", "sntJobMaterialComponentID", "sntJobMaterialID", "sntJobPartBinID", "sntJobPartID", "sntJobPartRevisionID", "sntJobPartWarehouseLocationID", "sntJobSerialNumberID",
			"sntLandedCostID", "sntOldTransactionType", "sntPartBinID", "sntPartID", "sntPartRevisionID", "sntPartTransactionID", "sntPartWarehouseLocationID", "sntQuantity", "sntReceiptID", "sntReceiptLineID",
			"sntRmaReceiptID", "sntRmaReceiptLineID", "sntRowVersion", "sntSerialNumberTransactionID", "sntSerialNumberID", "sntShipmentID", "sntShipmentLineID", "sntStatus", "sntTableName", "sntTableUniqueID",
			"sntTransactionDate", "sntTransactionType", "sntWarehouseReceiptID", "sntWarehouseReceiptLineID", "sntWarehouseTransferID", "sntWarehouseTransferLineID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("sntUniqueID|C", serialNumberTransactionId);
		AddCustomFieldsToSelectList("SerialNumberTransactions");
		using (DataTable dataTable = GetAsDataTable("SerialNumberTransactions", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSerialNumberTransactionInformationDto);
			}
			eRPSerialNumberTransactionInformationDto.sntCreatedBy = dataTable.Rows[0].Field<string>("sntCreatedBy");
			eRPSerialNumberTransactionInformationDto.sntCreatedDate = dataTable.Rows[0].Field<DateTime?>("sntCreatedDate");
			eRPSerialNumberTransactionInformationDto.sntDmrShipmentID = dataTable.Rows[0].Field<string>("sntDmrShipmentID");
			eRPSerialNumberTransactionInformationDto.sntDmrShipmentLineID = dataTable.Rows[0].Field<short>("sntDmrShipmentLineID");
			eRPSerialNumberTransactionInformationDto.sntUniqueID = dataTable.Rows[0].Field<Guid>("sntUniqueID");
			eRPSerialNumberTransactionInformationDto.sntInspectionID = dataTable.Rows[0].Field<string>("sntInspectionID");
			eRPSerialNumberTransactionInformationDto.sntInspectionLineID = dataTable.Rows[0].Field<short>("sntInspectionLineID");
			eRPSerialNumberTransactionInformationDto.sntInventoryCountID = dataTable.Rows[0].Field<int>("sntInventoryCountID");
			eRPSerialNumberTransactionInformationDto.sntInventoryCountLineID = dataTable.Rows[0].Field<int>("sntInventoryCountLineID");
			eRPSerialNumberTransactionInformationDto.sntInspect = dataTable.Rows[0].Field<bool>("sntInspect");
			eRPSerialNumberTransactionInformationDto.sntNegativeTransaction = dataTable.Rows[0].Field<bool>("sntNegativeTransaction");
			eRPSerialNumberTransactionInformationDto.sntJobAssemblyID = dataTable.Rows[0].Field<int>("sntJobAssemblyID");
			eRPSerialNumberTransactionInformationDto.sntJobID = dataTable.Rows[0].Field<string>("sntJobID");
			eRPSerialNumberTransactionInformationDto.sntJobMaterialComponentID = dataTable.Rows[0].Field<int>("sntJobMaterialComponentID");
			eRPSerialNumberTransactionInformationDto.sntJobMaterialID = dataTable.Rows[0].Field<int>("sntJobMaterialID");
			eRPSerialNumberTransactionInformationDto.sntJobPartBinID = dataTable.Rows[0].Field<string>("sntJobPartBinID");
			eRPSerialNumberTransactionInformationDto.sntJobPartID = dataTable.Rows[0].Field<string>("sntJobPartID");
			eRPSerialNumberTransactionInformationDto.sntJobPartRevisionID = dataTable.Rows[0].Field<string>("sntJobPartRevisionID");
			eRPSerialNumberTransactionInformationDto.sntJobPartWarehouseLocationID = dataTable.Rows[0].Field<string>("sntJobPartWarehouseLocationID");
			eRPSerialNumberTransactionInformationDto.sntJobSerialNumberID = dataTable.Rows[0].Field<string>("sntJobSerialNumberID");
			eRPSerialNumberTransactionInformationDto.sntLandedCostID = dataTable.Rows[0].Field<string>("sntLandedCostID");
			eRPSerialNumberTransactionInformationDto.sntOldTransactionType = dataTable.Rows[0].Field<byte>("sntOldTransactionType");
			eRPSerialNumberTransactionInformationDto.sntPartBinID = dataTable.Rows[0].Field<string>("sntPartBinID");
			eRPSerialNumberTransactionInformationDto.sntPartID = dataTable.Rows[0].Field<string>("sntPartID");
			eRPSerialNumberTransactionInformationDto.sntPartRevisionID = dataTable.Rows[0].Field<string>("sntPartRevisionID");
			eRPSerialNumberTransactionInformationDto.sntPartTransactionID = dataTable.Rows[0].Field<int>("sntPartTransactionID");
			eRPSerialNumberTransactionInformationDto.sntPartWarehouseLocationID = dataTable.Rows[0].Field<string>("sntPartWarehouseLocationID");
			eRPSerialNumberTransactionInformationDto.sntQuantity = dataTable.Rows[0].Field<decimal>("sntQuantity");
			eRPSerialNumberTransactionInformationDto.sntReceiptID = dataTable.Rows[0].Field<string>("sntReceiptID");
			eRPSerialNumberTransactionInformationDto.sntReceiptLineID = dataTable.Rows[0].Field<short>("sntReceiptLineID");
			eRPSerialNumberTransactionInformationDto.sntRmaReceiptID = dataTable.Rows[0].Field<string>("sntRmaReceiptID");
			eRPSerialNumberTransactionInformationDto.sntRmaReceiptLineID = dataTable.Rows[0].Field<short>("sntRmaReceiptLineID");
			eRPSerialNumberTransactionInformationDto.sntRowVersion = dataTable.Rows[0].Field<byte[]>("sntRowVersion");
			eRPSerialNumberTransactionInformationDto.sntSerialNumberTransactionID = dataTable.Rows[0].Field<int>("sntSerialNumberTransactionID");
			eRPSerialNumberTransactionInformationDto.sntSerialNumberID = dataTable.Rows[0].Field<string>("sntSerialNumberID");
			eRPSerialNumberTransactionInformationDto.sntShipmentID = dataTable.Rows[0].Field<string>("sntShipmentID");
			eRPSerialNumberTransactionInformationDto.sntShipmentLineID = dataTable.Rows[0].Field<short>("sntShipmentLineID");
			eRPSerialNumberTransactionInformationDto.sntStatus = dataTable.Rows[0].Field<byte>("sntStatus");
			eRPSerialNumberTransactionInformationDto.sntTableName = dataTable.Rows[0].Field<string>("sntTableName");
			eRPSerialNumberTransactionInformationDto.sntTableUniqueID = dataTable.Rows[0].Field<Guid>("sntTableUniqueID");
			eRPSerialNumberTransactionInformationDto.sntTransactionDate = dataTable.Rows[0].Field<DateTime?>("sntTransactionDate");
			eRPSerialNumberTransactionInformationDto.sntTransactionType = dataTable.Rows[0].Field<byte>("sntTransactionType");
			eRPSerialNumberTransactionInformationDto.sntWarehouseReceiptID = dataTable.Rows[0].Field<string>("sntWarehouseReceiptID");
			eRPSerialNumberTransactionInformationDto.sntWarehouseReceiptLineID = dataTable.Rows[0].Field<short>("sntWarehouseReceiptLineID");
			eRPSerialNumberTransactionInformationDto.sntWarehouseTransferID = dataTable.Rows[0].Field<string>("sntWarehouseTransferID");
			eRPSerialNumberTransactionInformationDto.sntWarehouseTransferLineID = dataTable.Rows[0].Field<short>("sntWarehouseTransferLineID");
			eRPSerialNumberTransactionInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSerialNumberTransactionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSerialNumberTransactionInformationDto);
	}

	public Task<APIValidationInfoDto> SaveSerialNumberTransaction(ERPSerialNumberTransactionDto serialNumberTransaction)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM SerialNumberTransactions WHERE sntUniqueID = " + M1Util.ConvertToLinq(serialNumberTransaction.sntUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["sntSerialNumberTransactionID"] = serialNumberTransaction.sntSerialNumberTransactionID;
				serialNumberTransaction.sntUniqueID = ((serialNumberTransaction.sntUniqueID == Guid.Empty) ? Guid.NewGuid() : serialNumberTransaction.sntUniqueID);
				dataRow["sntUniqueID"] = serialNumberTransaction.sntUniqueID;
				dataRow["sntCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["sntCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The SerialNumberTransaction could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (serialNumberTransaction.sntRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the SerialNumberTransaction is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["sntRowVersion"], serialNumberTransaction.sntRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the SerialNumberTransaction has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the SerialNumberTransaction again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["sntDmrShipmentID"] = serialNumberTransaction.sntDmrShipmentID;
			dataRow["sntDmrShipmentLineID"] = serialNumberTransaction.sntDmrShipmentLineID;
			dataRow["sntInspectionID"] = serialNumberTransaction.sntInspectionID;
			dataRow["sntInspectionLineID"] = serialNumberTransaction.sntInspectionLineID;
			dataRow["sntInventoryCountID"] = serialNumberTransaction.sntInventoryCountID;
			dataRow["sntInventoryCountLineID"] = serialNumberTransaction.sntInventoryCountLineID;
			dataRow["sntInspect"] = serialNumberTransaction.sntInspect;
			dataRow["sntNegativeTransaction"] = serialNumberTransaction.sntNegativeTransaction;
			dataRow["sntJobAssemblyID"] = serialNumberTransaction.sntJobAssemblyID;
			dataRow["sntJobID"] = serialNumberTransaction.sntJobID;
			dataRow["sntJobMaterialComponentID"] = serialNumberTransaction.sntJobMaterialComponentID;
			dataRow["sntJobMaterialID"] = serialNumberTransaction.sntJobMaterialID;
			dataRow["sntJobPartBinID"] = serialNumberTransaction.sntJobPartBinID;
			dataRow["sntJobPartID"] = serialNumberTransaction.sntJobPartID;
			dataRow["sntJobPartRevisionID"] = serialNumberTransaction.sntJobPartRevisionID;
			dataRow["sntJobPartWarehouseLocationID"] = serialNumberTransaction.sntJobPartWarehouseLocationID;
			dataRow["sntJobSerialNumberID"] = serialNumberTransaction.sntJobSerialNumberID;
			dataRow["sntLandedCostID"] = serialNumberTransaction.sntLandedCostID;
			dataRow["sntOldTransactionType"] = serialNumberTransaction.sntOldTransactionType;
			dataRow["sntPartBinID"] = serialNumberTransaction.sntPartBinID;
			dataRow["sntPartID"] = serialNumberTransaction.sntPartID;
			dataRow["sntPartRevisionID"] = serialNumberTransaction.sntPartRevisionID;
			dataRow["sntPartTransactionID"] = serialNumberTransaction.sntPartTransactionID;
			dataRow["sntPartWarehouseLocationID"] = serialNumberTransaction.sntPartWarehouseLocationID;
			dataRow["sntQuantity"] = serialNumberTransaction.sntQuantity;
			dataRow["sntReceiptID"] = serialNumberTransaction.sntReceiptID;
			dataRow["sntReceiptLineID"] = serialNumberTransaction.sntReceiptLineID;
			dataRow["sntRmaReceiptID"] = serialNumberTransaction.sntRmaReceiptID;
			dataRow["sntRmaReceiptLineID"] = serialNumberTransaction.sntRmaReceiptLineID;
			dataRow["sntSerialNumberID"] = serialNumberTransaction.sntSerialNumberID;
			dataRow["sntShipmentID"] = serialNumberTransaction.sntShipmentID;
			dataRow["sntShipmentLineID"] = serialNumberTransaction.sntShipmentLineID;
			dataRow["sntStatus"] = serialNumberTransaction.sntStatus;
			dataRow["sntTableName"] = serialNumberTransaction.sntTableName;
			dataRow["sntTableUniqueID"] = serialNumberTransaction.sntTableUniqueID;
			DataRow dataRow2 = dataRow;
			DateTime? sntTransactionDate = serialNumberTransaction.sntTransactionDate;
			dataRow2["sntTransactionDate"] = (sntTransactionDate.HasValue ? ((object)sntTransactionDate.GetValueOrDefault()) : dataRow["sntTransactionDate"]);
			dataRow["sntTransactionType"] = serialNumberTransaction.sntTransactionType;
			dataRow["sntWarehouseReceiptID"] = serialNumberTransaction.sntWarehouseReceiptID;
			dataRow["sntWarehouseReceiptLineID"] = serialNumberTransaction.sntWarehouseReceiptLineID;
			dataRow["sntWarehouseTransferID"] = serialNumberTransaction.sntWarehouseTransferID;
			dataRow["sntWarehouseTransferLineID"] = serialNumberTransaction.sntWarehouseTransferLineID;
			if (serialNumberTransaction.CustomFields != null && serialNumberTransaction.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in serialNumberTransaction.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the SerialNumberTransaction [{serialNumberTransaction.sntUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the SerialNumberTransaction [{serialNumberTransaction.sntUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
