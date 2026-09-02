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

public class ERPWarehouseReceiptLineRepository : APIBaseRepository, IERPWarehouseReceiptLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPWarehouseReceiptLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesWarehouseReceiptLineExist(Guid warehouseReceiptLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("wrlUniqueID|C", warehouseReceiptLineId);
		base.selectList.Add("wrlUniqueID");
		return Task.FromResult(GetAsObject("WarehouseReceiptLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPWarehouseReceiptLineInformationDto>> GetAllWarehouseReceiptLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPWarehouseReceiptLineInformationDto> collection = new List<ERPWarehouseReceiptLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[33]
		{
			"wrlCreatedBy", "wrlCreatedDate", "wrlDestinationPartBinID", "wrlDestinationWarehouseID", "wrlUniqueID", "wrlHeatLot", "wrlClosed", "wrlKitPart", "wrlPosted", "wrlReceivedComplete",
			"wrlReversed", "wrlPartDescription", "wrlPartID", "wrlPartRevisionID", "wrlQuantityReceived", "wrlReference", "wrlReverseWHReceiptID", "wrlReverseWHReceiptLineID", "wrlRowVersion", "wrlWarehouseReceiptLineID",
			"wrlSourcePartBinID", "wrlSourceTableName", "wrlSourceTableUniqueID", "wrlSourceWarehouseID", "wrlUnitCost", "wrlUnitOfMeasure", "wrlWarehouseReceiptID", "wrlWarehouseRequisitionID", "wrlWarehouseRequisitionLineID", "wrlWarehouseTransferID",
			"wrlWarehouseTransferLineID", "wrlWTOpenQuantity", "wrlWTShippedQuantity"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("WarehouseReceiptLines");
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
		using (DataTable dataTable = GetAsDataTable("WarehouseReceiptLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPWarehouseReceiptLineInformationDto eRPWarehouseReceiptLineInformationDto = new ERPWarehouseReceiptLineInformationDto();
				eRPWarehouseReceiptLineInformationDto.wrlCreatedBy = dataTable.Rows[i].Field<string>("wrlCreatedBy");
				eRPWarehouseReceiptLineInformationDto.wrlCreatedDate = dataTable.Rows[i].Field<DateTime?>("wrlCreatedDate");
				eRPWarehouseReceiptLineInformationDto.wrlDestinationPartBinID = dataTable.Rows[i].Field<string>("wrlDestinationPartBinID");
				eRPWarehouseReceiptLineInformationDto.wrlDestinationWarehouseID = dataTable.Rows[i].Field<string>("wrlDestinationWarehouseID");
				eRPWarehouseReceiptLineInformationDto.wrlUniqueID = dataTable.Rows[i].Field<Guid>("wrlUniqueID");
				eRPWarehouseReceiptLineInformationDto.wrlHeatLot = dataTable.Rows[i].Field<string>("wrlHeatLot");
				eRPWarehouseReceiptLineInformationDto.wrlClosed = dataTable.Rows[i].Field<bool>("wrlClosed");
				eRPWarehouseReceiptLineInformationDto.wrlKitPart = dataTable.Rows[i].Field<bool>("wrlKitPart");
				eRPWarehouseReceiptLineInformationDto.wrlPosted = dataTable.Rows[i].Field<bool>("wrlPosted");
				eRPWarehouseReceiptLineInformationDto.wrlReceivedComplete = dataTable.Rows[i].Field<bool>("wrlReceivedComplete");
				eRPWarehouseReceiptLineInformationDto.wrlReversed = dataTable.Rows[i].Field<bool>("wrlReversed");
				eRPWarehouseReceiptLineInformationDto.wrlPartDescription = dataTable.Rows[i].Field<string>("wrlPartDescription");
				eRPWarehouseReceiptLineInformationDto.wrlPartID = dataTable.Rows[i].Field<string>("wrlPartID");
				eRPWarehouseReceiptLineInformationDto.wrlPartRevisionID = dataTable.Rows[i].Field<string>("wrlPartRevisionID");
				eRPWarehouseReceiptLineInformationDto.wrlQuantityReceived = dataTable.Rows[i].Field<decimal>("wrlQuantityReceived");
				eRPWarehouseReceiptLineInformationDto.wrlReference = dataTable.Rows[i].Field<string>("wrlReference");
				eRPWarehouseReceiptLineInformationDto.wrlReverseWHReceiptID = dataTable.Rows[i].Field<string>("wrlReverseWHReceiptID");
				eRPWarehouseReceiptLineInformationDto.wrlReverseWHReceiptLineID = dataTable.Rows[i].Field<short>("wrlReverseWHReceiptLineID");
				eRPWarehouseReceiptLineInformationDto.wrlRowVersion = dataTable.Rows[i].Field<byte[]>("wrlRowVersion");
				eRPWarehouseReceiptLineInformationDto.wrlWarehouseReceiptLineID = dataTable.Rows[i].Field<short>("wrlWarehouseReceiptLineID");
				eRPWarehouseReceiptLineInformationDto.wrlSourcePartBinID = dataTable.Rows[i].Field<string>("wrlSourcePartBinID");
				eRPWarehouseReceiptLineInformationDto.wrlSourceTableName = dataTable.Rows[i].Field<string>("wrlSourceTableName");
				eRPWarehouseReceiptLineInformationDto.wrlSourceTableUniqueID = dataTable.Rows[i].Field<Guid>("wrlSourceTableUniqueID");
				eRPWarehouseReceiptLineInformationDto.wrlSourceWarehouseID = dataTable.Rows[i].Field<string>("wrlSourceWarehouseID");
				eRPWarehouseReceiptLineInformationDto.wrlUnitCost = dataTable.Rows[i].Field<decimal>("wrlUnitCost");
				eRPWarehouseReceiptLineInformationDto.wrlUnitOfMeasure = dataTable.Rows[i].Field<string>("wrlUnitOfMeasure");
				eRPWarehouseReceiptLineInformationDto.wrlWarehouseReceiptID = dataTable.Rows[i].Field<string>("wrlWarehouseReceiptID");
				eRPWarehouseReceiptLineInformationDto.wrlWarehouseRequisitionID = dataTable.Rows[i].Field<string>("wrlWarehouseRequisitionID");
				eRPWarehouseReceiptLineInformationDto.wrlWarehouseRequisitionLineID = dataTable.Rows[i].Field<short>("wrlWarehouseRequisitionLineID");
				eRPWarehouseReceiptLineInformationDto.wrlWarehouseTransferID = dataTable.Rows[i].Field<string>("wrlWarehouseTransferID");
				eRPWarehouseReceiptLineInformationDto.wrlWarehouseTransferLineID = dataTable.Rows[i].Field<short>("wrlWarehouseTransferLineID");
				eRPWarehouseReceiptLineInformationDto.wrlWTOpenQuantity = dataTable.Rows[i].Field<decimal>("wrlWTOpenQuantity");
				eRPWarehouseReceiptLineInformationDto.wrlWTShippedQuantity = dataTable.Rows[i].Field<decimal>("wrlWTShippedQuantity");
				eRPWarehouseReceiptLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPWarehouseReceiptLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPWarehouseReceiptLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPWarehouseReceiptLineInformationDto> GetWarehouseReceiptLine(Guid warehouseReceiptLineId)
	{
		ERPWarehouseReceiptLineInformationDto eRPWarehouseReceiptLineInformationDto = new ERPWarehouseReceiptLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[33]
		{
			"wrlCreatedBy", "wrlCreatedDate", "wrlDestinationPartBinID", "wrlDestinationWarehouseID", "wrlUniqueID", "wrlHeatLot", "wrlClosed", "wrlKitPart", "wrlPosted", "wrlReceivedComplete",
			"wrlReversed", "wrlPartDescription", "wrlPartID", "wrlPartRevisionID", "wrlQuantityReceived", "wrlReference", "wrlReverseWHReceiptID", "wrlReverseWHReceiptLineID", "wrlRowVersion", "wrlWarehouseReceiptLineID",
			"wrlSourcePartBinID", "wrlSourceTableName", "wrlSourceTableUniqueID", "wrlSourceWarehouseID", "wrlUnitCost", "wrlUnitOfMeasure", "wrlWarehouseReceiptID", "wrlWarehouseRequisitionID", "wrlWarehouseRequisitionLineID", "wrlWarehouseTransferID",
			"wrlWarehouseTransferLineID", "wrlWTOpenQuantity", "wrlWTShippedQuantity"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("wrlUniqueID|C", warehouseReceiptLineId);
		AddCustomFieldsToSelectList("WarehouseReceiptLines");
		using (DataTable dataTable = GetAsDataTable("WarehouseReceiptLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPWarehouseReceiptLineInformationDto);
			}
			eRPWarehouseReceiptLineInformationDto.wrlCreatedBy = dataTable.Rows[0].Field<string>("wrlCreatedBy");
			eRPWarehouseReceiptLineInformationDto.wrlCreatedDate = dataTable.Rows[0].Field<DateTime?>("wrlCreatedDate");
			eRPWarehouseReceiptLineInformationDto.wrlDestinationPartBinID = dataTable.Rows[0].Field<string>("wrlDestinationPartBinID");
			eRPWarehouseReceiptLineInformationDto.wrlDestinationWarehouseID = dataTable.Rows[0].Field<string>("wrlDestinationWarehouseID");
			eRPWarehouseReceiptLineInformationDto.wrlUniqueID = dataTable.Rows[0].Field<Guid>("wrlUniqueID");
			eRPWarehouseReceiptLineInformationDto.wrlHeatLot = dataTable.Rows[0].Field<string>("wrlHeatLot");
			eRPWarehouseReceiptLineInformationDto.wrlClosed = dataTable.Rows[0].Field<bool>("wrlClosed");
			eRPWarehouseReceiptLineInformationDto.wrlKitPart = dataTable.Rows[0].Field<bool>("wrlKitPart");
			eRPWarehouseReceiptLineInformationDto.wrlPosted = dataTable.Rows[0].Field<bool>("wrlPosted");
			eRPWarehouseReceiptLineInformationDto.wrlReceivedComplete = dataTable.Rows[0].Field<bool>("wrlReceivedComplete");
			eRPWarehouseReceiptLineInformationDto.wrlReversed = dataTable.Rows[0].Field<bool>("wrlReversed");
			eRPWarehouseReceiptLineInformationDto.wrlPartDescription = dataTable.Rows[0].Field<string>("wrlPartDescription");
			eRPWarehouseReceiptLineInformationDto.wrlPartID = dataTable.Rows[0].Field<string>("wrlPartID");
			eRPWarehouseReceiptLineInformationDto.wrlPartRevisionID = dataTable.Rows[0].Field<string>("wrlPartRevisionID");
			eRPWarehouseReceiptLineInformationDto.wrlQuantityReceived = dataTable.Rows[0].Field<decimal>("wrlQuantityReceived");
			eRPWarehouseReceiptLineInformationDto.wrlReference = dataTable.Rows[0].Field<string>("wrlReference");
			eRPWarehouseReceiptLineInformationDto.wrlReverseWHReceiptID = dataTable.Rows[0].Field<string>("wrlReverseWHReceiptID");
			eRPWarehouseReceiptLineInformationDto.wrlReverseWHReceiptLineID = dataTable.Rows[0].Field<short>("wrlReverseWHReceiptLineID");
			eRPWarehouseReceiptLineInformationDto.wrlRowVersion = dataTable.Rows[0].Field<byte[]>("wrlRowVersion");
			eRPWarehouseReceiptLineInformationDto.wrlWarehouseReceiptLineID = dataTable.Rows[0].Field<short>("wrlWarehouseReceiptLineID");
			eRPWarehouseReceiptLineInformationDto.wrlSourcePartBinID = dataTable.Rows[0].Field<string>("wrlSourcePartBinID");
			eRPWarehouseReceiptLineInformationDto.wrlSourceTableName = dataTable.Rows[0].Field<string>("wrlSourceTableName");
			eRPWarehouseReceiptLineInformationDto.wrlSourceTableUniqueID = dataTable.Rows[0].Field<Guid>("wrlSourceTableUniqueID");
			eRPWarehouseReceiptLineInformationDto.wrlSourceWarehouseID = dataTable.Rows[0].Field<string>("wrlSourceWarehouseID");
			eRPWarehouseReceiptLineInformationDto.wrlUnitCost = dataTable.Rows[0].Field<decimal>("wrlUnitCost");
			eRPWarehouseReceiptLineInformationDto.wrlUnitOfMeasure = dataTable.Rows[0].Field<string>("wrlUnitOfMeasure");
			eRPWarehouseReceiptLineInformationDto.wrlWarehouseReceiptID = dataTable.Rows[0].Field<string>("wrlWarehouseReceiptID");
			eRPWarehouseReceiptLineInformationDto.wrlWarehouseRequisitionID = dataTable.Rows[0].Field<string>("wrlWarehouseRequisitionID");
			eRPWarehouseReceiptLineInformationDto.wrlWarehouseRequisitionLineID = dataTable.Rows[0].Field<short>("wrlWarehouseRequisitionLineID");
			eRPWarehouseReceiptLineInformationDto.wrlWarehouseTransferID = dataTable.Rows[0].Field<string>("wrlWarehouseTransferID");
			eRPWarehouseReceiptLineInformationDto.wrlWarehouseTransferLineID = dataTable.Rows[0].Field<short>("wrlWarehouseTransferLineID");
			eRPWarehouseReceiptLineInformationDto.wrlWTOpenQuantity = dataTable.Rows[0].Field<decimal>("wrlWTOpenQuantity");
			eRPWarehouseReceiptLineInformationDto.wrlWTShippedQuantity = dataTable.Rows[0].Field<decimal>("wrlWTShippedQuantity");
			eRPWarehouseReceiptLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPWarehouseReceiptLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPWarehouseReceiptLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveWarehouseReceiptLine(ERPWarehouseReceiptLineDto warehouseReceiptLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM WarehouseReceiptLines WHERE wrlUniqueID = " + M1Util.ConvertToLinq(warehouseReceiptLine.wrlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["wrlWarehouseReceiptID"] = warehouseReceiptLine.wrlWarehouseReceiptID.ToUpper();
				dataRow["wrlWarehouseReceiptLineID"] = warehouseReceiptLine.wrlWarehouseReceiptLineID;
				warehouseReceiptLine.wrlUniqueID = ((warehouseReceiptLine.wrlUniqueID == Guid.Empty) ? Guid.NewGuid() : warehouseReceiptLine.wrlUniqueID);
				dataRow["wrlUniqueID"] = warehouseReceiptLine.wrlUniqueID;
				dataRow["wrlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["wrlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The WarehouseReceiptLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (warehouseReceiptLine.wrlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the WarehouseReceiptLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["wrlRowVersion"], warehouseReceiptLine.wrlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the WarehouseReceiptLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the WarehouseReceiptLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["wrlDestinationPartBinID"] = warehouseReceiptLine.wrlDestinationPartBinID;
			dataRow["wrlDestinationWarehouseID"] = warehouseReceiptLine.wrlDestinationWarehouseID;
			dataRow["wrlHeatLot"] = warehouseReceiptLine.wrlHeatLot;
			dataRow["wrlClosed"] = warehouseReceiptLine.wrlClosed;
			dataRow["wrlKitPart"] = warehouseReceiptLine.wrlKitPart;
			dataRow["wrlPosted"] = warehouseReceiptLine.wrlPosted;
			dataRow["wrlReceivedComplete"] = warehouseReceiptLine.wrlReceivedComplete;
			dataRow["wrlReversed"] = warehouseReceiptLine.wrlReversed;
			dataRow["wrlPartDescription"] = warehouseReceiptLine.wrlPartDescription;
			dataRow["wrlPartID"] = warehouseReceiptLine.wrlPartID;
			dataRow["wrlPartRevisionID"] = warehouseReceiptLine.wrlPartRevisionID;
			dataRow["wrlQuantityReceived"] = warehouseReceiptLine.wrlQuantityReceived;
			dataRow["wrlReference"] = warehouseReceiptLine.wrlReference;
			dataRow["wrlReverseWHReceiptID"] = warehouseReceiptLine.wrlReverseWHReceiptID;
			dataRow["wrlReverseWHReceiptLineID"] = warehouseReceiptLine.wrlReverseWHReceiptLineID;
			dataRow["wrlSourcePartBinID"] = warehouseReceiptLine.wrlSourcePartBinID;
			dataRow["wrlSourceTableName"] = warehouseReceiptLine.wrlSourceTableName;
			dataRow["wrlSourceTableUniqueID"] = warehouseReceiptLine.wrlSourceTableUniqueID;
			dataRow["wrlSourceWarehouseID"] = warehouseReceiptLine.wrlSourceWarehouseID;
			dataRow["wrlUnitCost"] = warehouseReceiptLine.wrlUnitCost;
			dataRow["wrlUnitOfMeasure"] = warehouseReceiptLine.wrlUnitOfMeasure;
			dataRow["wrlWarehouseRequisitionID"] = warehouseReceiptLine.wrlWarehouseRequisitionID;
			dataRow["wrlWarehouseRequisitionLineID"] = warehouseReceiptLine.wrlWarehouseRequisitionLineID;
			dataRow["wrlWarehouseTransferID"] = warehouseReceiptLine.wrlWarehouseTransferID;
			dataRow["wrlWarehouseTransferLineID"] = warehouseReceiptLine.wrlWarehouseTransferLineID;
			dataRow["wrlWTOpenQuantity"] = warehouseReceiptLine.wrlWTOpenQuantity;
			dataRow["wrlWTShippedQuantity"] = warehouseReceiptLine.wrlWTShippedQuantity;
			if (warehouseReceiptLine.CustomFields != null && warehouseReceiptLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in warehouseReceiptLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the WarehouseReceiptLine [{warehouseReceiptLine.wrlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the WarehouseReceiptLine [{warehouseReceiptLine.wrlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
