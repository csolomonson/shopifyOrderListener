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

public class ERPWarehouseTransferRepository : APIBaseRepository, IERPWarehouseTransferRepository, IAPIBaseRepository, IDisposable
{
	public ERPWarehouseTransferRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesWarehouseTransferExist(Guid warehouseTransferId)
	{
		InitializeParameterLists();
		base.filterList.Add("mwpUniqueID|C", warehouseTransferId);
		base.selectList.Add("mwpUniqueID");
		return Task.FromResult(GetAsObject("WarehouseTransfers", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPWarehouseTransferInformationDto>> GetAllWarehouseTransfers(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPWarehouseTransferInformationDto> collection = new List<ERPWarehouseTransferInformationDto>();
		InitializeParameterLists();
		string[] array = new string[23]
		{
			"mwpClosedDate", "mwpWarehouseTransferID", "mwpCreatedBy", "mwpCreatedDate", "mwpDestinationWarehouseID", "mwpUniqueID", "mwpFreightCharge", "mwpClosed", "mwpPosted", "mwpPrintLabels",
			"mwpPrintPacker", "mwpReversalEntry", "mwpReversed", "mwpNumberOfLabels", "mwpPostedDate", "mwpRowVersion", "mwpShipDate", "mwpShippingCommentsRTF", "mwpShippingCommentsText", "mwpShippingMethodID",
			"mwpShippingPaymentTypeID", "mwpSourceWarehouseID", "mwpTrackingNumber"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("WarehouseTransfers");
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
		using (DataTable dataTable = GetAsDataTable("WarehouseTransfers", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPWarehouseTransferInformationDto eRPWarehouseTransferInformationDto = new ERPWarehouseTransferInformationDto();
				eRPWarehouseTransferInformationDto.mwpClosedDate = dataTable.Rows[i].Field<DateTime?>("mwpClosedDate");
				eRPWarehouseTransferInformationDto.mwpWarehouseTransferID = dataTable.Rows[i].Field<string>("mwpWarehouseTransferID");
				eRPWarehouseTransferInformationDto.mwpCreatedBy = dataTable.Rows[i].Field<string>("mwpCreatedBy");
				eRPWarehouseTransferInformationDto.mwpCreatedDate = dataTable.Rows[i].Field<DateTime?>("mwpCreatedDate");
				eRPWarehouseTransferInformationDto.mwpDestinationWarehouseID = dataTable.Rows[i].Field<string>("mwpDestinationWarehouseID");
				eRPWarehouseTransferInformationDto.mwpUniqueID = dataTable.Rows[i].Field<Guid>("mwpUniqueID");
				eRPWarehouseTransferInformationDto.mwpFreightCharge = dataTable.Rows[i].Field<decimal>("mwpFreightCharge");
				eRPWarehouseTransferInformationDto.mwpClosed = dataTable.Rows[i].Field<bool>("mwpClosed");
				eRPWarehouseTransferInformationDto.mwpPosted = dataTable.Rows[i].Field<bool>("mwpPosted");
				eRPWarehouseTransferInformationDto.mwpPrintLabels = dataTable.Rows[i].Field<bool>("mwpPrintLabels");
				eRPWarehouseTransferInformationDto.mwpPrintPacker = dataTable.Rows[i].Field<bool>("mwpPrintPacker");
				eRPWarehouseTransferInformationDto.mwpReversalEntry = dataTable.Rows[i].Field<bool>("mwpReversalEntry");
				eRPWarehouseTransferInformationDto.mwpReversed = dataTable.Rows[i].Field<bool>("mwpReversed");
				eRPWarehouseTransferInformationDto.mwpNumberOfLabels = dataTable.Rows[i].Field<short>("mwpNumberOfLabels");
				eRPWarehouseTransferInformationDto.mwpPostedDate = dataTable.Rows[i].Field<DateTime?>("mwpPostedDate");
				eRPWarehouseTransferInformationDto.mwpRowVersion = dataTable.Rows[i].Field<byte[]>("mwpRowVersion");
				eRPWarehouseTransferInformationDto.mwpShipDate = dataTable.Rows[i].Field<DateTime?>("mwpShipDate");
				eRPWarehouseTransferInformationDto.mwpShippingCommentsRTF = dataTable.Rows[i].Field<string>("mwpShippingCommentsRTF");
				eRPWarehouseTransferInformationDto.mwpShippingCommentsText = dataTable.Rows[i].Field<string>("mwpShippingCommentsText");
				eRPWarehouseTransferInformationDto.mwpShippingMethodID = dataTable.Rows[i].Field<string>("mwpShippingMethodID");
				eRPWarehouseTransferInformationDto.mwpShippingPaymentTypeID = dataTable.Rows[i].Field<string>("mwpShippingPaymentTypeID");
				eRPWarehouseTransferInformationDto.mwpSourceWarehouseID = dataTable.Rows[i].Field<string>("mwpSourceWarehouseID");
				eRPWarehouseTransferInformationDto.mwpTrackingNumber = dataTable.Rows[i].Field<string>("mwpTrackingNumber");
				eRPWarehouseTransferInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPWarehouseTransferInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPWarehouseTransferInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPWarehouseTransferInformationDto> GetWarehouseTransfer(Guid warehouseTransferId)
	{
		ERPWarehouseTransferInformationDto eRPWarehouseTransferInformationDto = new ERPWarehouseTransferInformationDto();
		InitializeParameterLists();
		string[] collection = new string[23]
		{
			"mwpClosedDate", "mwpWarehouseTransferID", "mwpCreatedBy", "mwpCreatedDate", "mwpDestinationWarehouseID", "mwpUniqueID", "mwpFreightCharge", "mwpClosed", "mwpPosted", "mwpPrintLabels",
			"mwpPrintPacker", "mwpReversalEntry", "mwpReversed", "mwpNumberOfLabels", "mwpPostedDate", "mwpRowVersion", "mwpShipDate", "mwpShippingCommentsRTF", "mwpShippingCommentsText", "mwpShippingMethodID",
			"mwpShippingPaymentTypeID", "mwpSourceWarehouseID", "mwpTrackingNumber"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("mwpUniqueID|C", warehouseTransferId);
		AddCustomFieldsToSelectList("WarehouseTransfers");
		using (DataTable dataTable = GetAsDataTable("WarehouseTransfers", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPWarehouseTransferInformationDto);
			}
			eRPWarehouseTransferInformationDto.mwpClosedDate = dataTable.Rows[0].Field<DateTime?>("mwpClosedDate");
			eRPWarehouseTransferInformationDto.mwpWarehouseTransferID = dataTable.Rows[0].Field<string>("mwpWarehouseTransferID");
			eRPWarehouseTransferInformationDto.mwpCreatedBy = dataTable.Rows[0].Field<string>("mwpCreatedBy");
			eRPWarehouseTransferInformationDto.mwpCreatedDate = dataTable.Rows[0].Field<DateTime?>("mwpCreatedDate");
			eRPWarehouseTransferInformationDto.mwpDestinationWarehouseID = dataTable.Rows[0].Field<string>("mwpDestinationWarehouseID");
			eRPWarehouseTransferInformationDto.mwpUniqueID = dataTable.Rows[0].Field<Guid>("mwpUniqueID");
			eRPWarehouseTransferInformationDto.mwpFreightCharge = dataTable.Rows[0].Field<decimal>("mwpFreightCharge");
			eRPWarehouseTransferInformationDto.mwpClosed = dataTable.Rows[0].Field<bool>("mwpClosed");
			eRPWarehouseTransferInformationDto.mwpPosted = dataTable.Rows[0].Field<bool>("mwpPosted");
			eRPWarehouseTransferInformationDto.mwpPrintLabels = dataTable.Rows[0].Field<bool>("mwpPrintLabels");
			eRPWarehouseTransferInformationDto.mwpPrintPacker = dataTable.Rows[0].Field<bool>("mwpPrintPacker");
			eRPWarehouseTransferInformationDto.mwpReversalEntry = dataTable.Rows[0].Field<bool>("mwpReversalEntry");
			eRPWarehouseTransferInformationDto.mwpReversed = dataTable.Rows[0].Field<bool>("mwpReversed");
			eRPWarehouseTransferInformationDto.mwpNumberOfLabels = dataTable.Rows[0].Field<short>("mwpNumberOfLabels");
			eRPWarehouseTransferInformationDto.mwpPostedDate = dataTable.Rows[0].Field<DateTime?>("mwpPostedDate");
			eRPWarehouseTransferInformationDto.mwpRowVersion = dataTable.Rows[0].Field<byte[]>("mwpRowVersion");
			eRPWarehouseTransferInformationDto.mwpShipDate = dataTable.Rows[0].Field<DateTime?>("mwpShipDate");
			eRPWarehouseTransferInformationDto.mwpShippingCommentsRTF = dataTable.Rows[0].Field<string>("mwpShippingCommentsRTF");
			eRPWarehouseTransferInformationDto.mwpShippingCommentsText = dataTable.Rows[0].Field<string>("mwpShippingCommentsText");
			eRPWarehouseTransferInformationDto.mwpShippingMethodID = dataTable.Rows[0].Field<string>("mwpShippingMethodID");
			eRPWarehouseTransferInformationDto.mwpShippingPaymentTypeID = dataTable.Rows[0].Field<string>("mwpShippingPaymentTypeID");
			eRPWarehouseTransferInformationDto.mwpSourceWarehouseID = dataTable.Rows[0].Field<string>("mwpSourceWarehouseID");
			eRPWarehouseTransferInformationDto.mwpTrackingNumber = dataTable.Rows[0].Field<string>("mwpTrackingNumber");
			eRPWarehouseTransferInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPWarehouseTransferInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPWarehouseTransferInformationDto);
	}

	public Task<APIValidationInfoDto> SaveWarehouseTransfer(ERPWarehouseTransferDto warehouseTransfer)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM WarehouseTransfers WHERE mwpUniqueID = " + M1Util.ConvertToLinq(warehouseTransfer.mwpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["mwpWarehouseTransferID"] = warehouseTransfer.mwpWarehouseTransferID.ToUpper();
				warehouseTransfer.mwpUniqueID = ((warehouseTransfer.mwpUniqueID == Guid.Empty) ? Guid.NewGuid() : warehouseTransfer.mwpUniqueID);
				dataRow["mwpUniqueID"] = warehouseTransfer.mwpUniqueID;
				dataRow["mwpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["mwpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The WarehouseTransfer could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (warehouseTransfer.mwpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the WarehouseTransfer is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["mwpRowVersion"], warehouseTransfer.mwpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the WarehouseTransfer has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the WarehouseTransfer again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? mwpClosedDate = warehouseTransfer.mwpClosedDate;
			dataRow2["mwpClosedDate"] = (mwpClosedDate.HasValue ? ((object)mwpClosedDate.GetValueOrDefault()) : dataRow["mwpClosedDate"]);
			dataRow["mwpDestinationWarehouseID"] = warehouseTransfer.mwpDestinationWarehouseID;
			dataRow["mwpFreightCharge"] = warehouseTransfer.mwpFreightCharge;
			dataRow["mwpClosed"] = warehouseTransfer.mwpClosed;
			dataRow["mwpPosted"] = warehouseTransfer.mwpPosted;
			dataRow["mwpPrintLabels"] = warehouseTransfer.mwpPrintLabels;
			dataRow["mwpPrintPacker"] = warehouseTransfer.mwpPrintPacker;
			dataRow["mwpReversalEntry"] = warehouseTransfer.mwpReversalEntry;
			dataRow["mwpReversed"] = warehouseTransfer.mwpReversed;
			dataRow["mwpNumberOfLabels"] = warehouseTransfer.mwpNumberOfLabels;
			DataRow dataRow3 = dataRow;
			mwpClosedDate = warehouseTransfer.mwpPostedDate;
			dataRow3["mwpPostedDate"] = (mwpClosedDate.HasValue ? ((object)mwpClosedDate.GetValueOrDefault()) : dataRow["mwpPostedDate"]);
			DataRow dataRow4 = dataRow;
			mwpClosedDate = warehouseTransfer.mwpShipDate;
			dataRow4["mwpShipDate"] = (mwpClosedDate.HasValue ? ((object)mwpClosedDate.GetValueOrDefault()) : dataRow["mwpShipDate"]);
			dataRow["mwpShippingCommentsRTF"] = warehouseTransfer.mwpShippingCommentsRTF ?? dataRow["mwpShippingCommentsRTF"];
			dataRow["mwpShippingCommentsText"] = warehouseTransfer.mwpShippingCommentsText ?? dataRow["mwpShippingCommentsText"];
			dataRow["mwpShippingMethodID"] = warehouseTransfer.mwpShippingMethodID;
			dataRow["mwpShippingPaymentTypeID"] = warehouseTransfer.mwpShippingPaymentTypeID;
			dataRow["mwpSourceWarehouseID"] = warehouseTransfer.mwpSourceWarehouseID;
			dataRow["mwpTrackingNumber"] = warehouseTransfer.mwpTrackingNumber;
			if (warehouseTransfer.CustomFields != null && warehouseTransfer.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in warehouseTransfer.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the WarehouseTransfer [{warehouseTransfer.mwpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the WarehouseTransfer [{warehouseTransfer.mwpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
