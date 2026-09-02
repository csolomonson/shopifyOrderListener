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

public class ERPWarehouseReceiptRepository : APIBaseRepository, IERPWarehouseReceiptRepository, IAPIBaseRepository, IDisposable
{
	public ERPWarehouseReceiptRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesWarehouseReceiptExist(Guid warehouseReceiptId)
	{
		InitializeParameterLists();
		base.filterList.Add("wrpUniqueID|C", warehouseReceiptId);
		base.selectList.Add("wrpUniqueID");
		return Task.FromResult(GetAsObject("WarehouseReceipts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPWarehouseReceiptInformationDto>> GetAllWarehouseReceipts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPWarehouseReceiptInformationDto> collection = new List<ERPWarehouseReceiptInformationDto>();
		InitializeParameterLists();
		string[] array = new string[17]
		{
			"wrpClosedDate", "wrpWarehouseReceiptID", "wrpCreatedBy", "wrpCreatedDate", "wrpDestinationWarehouseID", "wrpUniqueID", "wrpFreightCharge", "wrpClosed", "wrpPosted", "wrpReversalEntry",
			"wrpReversed", "wrpPostedDate", "wrpReceiptDate", "wrpRowVersion", "wrpShippingMethodID", "wrpShippingPaymentTypeID", "wrpSourceWarehouseID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("WarehouseReceipts");
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
		using (DataTable dataTable = GetAsDataTable("WarehouseReceipts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPWarehouseReceiptInformationDto eRPWarehouseReceiptInformationDto = new ERPWarehouseReceiptInformationDto();
				eRPWarehouseReceiptInformationDto.wrpClosedDate = dataTable.Rows[i].Field<DateTime?>("wrpClosedDate");
				eRPWarehouseReceiptInformationDto.wrpWarehouseReceiptID = dataTable.Rows[i].Field<string>("wrpWarehouseReceiptID");
				eRPWarehouseReceiptInformationDto.wrpCreatedBy = dataTable.Rows[i].Field<string>("wrpCreatedBy");
				eRPWarehouseReceiptInformationDto.wrpCreatedDate = dataTable.Rows[i].Field<DateTime?>("wrpCreatedDate");
				eRPWarehouseReceiptInformationDto.wrpDestinationWarehouseID = dataTable.Rows[i].Field<string>("wrpDestinationWarehouseID");
				eRPWarehouseReceiptInformationDto.wrpUniqueID = dataTable.Rows[i].Field<Guid>("wrpUniqueID");
				eRPWarehouseReceiptInformationDto.wrpFreightCharge = dataTable.Rows[i].Field<decimal>("wrpFreightCharge");
				eRPWarehouseReceiptInformationDto.wrpClosed = dataTable.Rows[i].Field<bool>("wrpClosed");
				eRPWarehouseReceiptInformationDto.wrpPosted = dataTable.Rows[i].Field<bool>("wrpPosted");
				eRPWarehouseReceiptInformationDto.wrpReversalEntry = dataTable.Rows[i].Field<bool>("wrpReversalEntry");
				eRPWarehouseReceiptInformationDto.wrpReversed = dataTable.Rows[i].Field<bool>("wrpReversed");
				eRPWarehouseReceiptInformationDto.wrpPostedDate = dataTable.Rows[i].Field<DateTime?>("wrpPostedDate");
				eRPWarehouseReceiptInformationDto.wrpReceiptDate = dataTable.Rows[i].Field<DateTime?>("wrpReceiptDate");
				eRPWarehouseReceiptInformationDto.wrpRowVersion = dataTable.Rows[i].Field<byte[]>("wrpRowVersion");
				eRPWarehouseReceiptInformationDto.wrpShippingMethodID = dataTable.Rows[i].Field<string>("wrpShippingMethodID");
				eRPWarehouseReceiptInformationDto.wrpShippingPaymentTypeID = dataTable.Rows[i].Field<string>("wrpShippingPaymentTypeID");
				eRPWarehouseReceiptInformationDto.wrpSourceWarehouseID = dataTable.Rows[i].Field<string>("wrpSourceWarehouseID");
				eRPWarehouseReceiptInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPWarehouseReceiptInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPWarehouseReceiptInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPWarehouseReceiptInformationDto> GetWarehouseReceipt(Guid warehouseReceiptId)
	{
		ERPWarehouseReceiptInformationDto eRPWarehouseReceiptInformationDto = new ERPWarehouseReceiptInformationDto();
		InitializeParameterLists();
		string[] collection = new string[17]
		{
			"wrpClosedDate", "wrpWarehouseReceiptID", "wrpCreatedBy", "wrpCreatedDate", "wrpDestinationWarehouseID", "wrpUniqueID", "wrpFreightCharge", "wrpClosed", "wrpPosted", "wrpReversalEntry",
			"wrpReversed", "wrpPostedDate", "wrpReceiptDate", "wrpRowVersion", "wrpShippingMethodID", "wrpShippingPaymentTypeID", "wrpSourceWarehouseID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("wrpUniqueID|C", warehouseReceiptId);
		AddCustomFieldsToSelectList("WarehouseReceipts");
		using (DataTable dataTable = GetAsDataTable("WarehouseReceipts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPWarehouseReceiptInformationDto);
			}
			eRPWarehouseReceiptInformationDto.wrpClosedDate = dataTable.Rows[0].Field<DateTime?>("wrpClosedDate");
			eRPWarehouseReceiptInformationDto.wrpWarehouseReceiptID = dataTable.Rows[0].Field<string>("wrpWarehouseReceiptID");
			eRPWarehouseReceiptInformationDto.wrpCreatedBy = dataTable.Rows[0].Field<string>("wrpCreatedBy");
			eRPWarehouseReceiptInformationDto.wrpCreatedDate = dataTable.Rows[0].Field<DateTime?>("wrpCreatedDate");
			eRPWarehouseReceiptInformationDto.wrpDestinationWarehouseID = dataTable.Rows[0].Field<string>("wrpDestinationWarehouseID");
			eRPWarehouseReceiptInformationDto.wrpUniqueID = dataTable.Rows[0].Field<Guid>("wrpUniqueID");
			eRPWarehouseReceiptInformationDto.wrpFreightCharge = dataTable.Rows[0].Field<decimal>("wrpFreightCharge");
			eRPWarehouseReceiptInformationDto.wrpClosed = dataTable.Rows[0].Field<bool>("wrpClosed");
			eRPWarehouseReceiptInformationDto.wrpPosted = dataTable.Rows[0].Field<bool>("wrpPosted");
			eRPWarehouseReceiptInformationDto.wrpReversalEntry = dataTable.Rows[0].Field<bool>("wrpReversalEntry");
			eRPWarehouseReceiptInformationDto.wrpReversed = dataTable.Rows[0].Field<bool>("wrpReversed");
			eRPWarehouseReceiptInformationDto.wrpPostedDate = dataTable.Rows[0].Field<DateTime?>("wrpPostedDate");
			eRPWarehouseReceiptInformationDto.wrpReceiptDate = dataTable.Rows[0].Field<DateTime?>("wrpReceiptDate");
			eRPWarehouseReceiptInformationDto.wrpRowVersion = dataTable.Rows[0].Field<byte[]>("wrpRowVersion");
			eRPWarehouseReceiptInformationDto.wrpShippingMethodID = dataTable.Rows[0].Field<string>("wrpShippingMethodID");
			eRPWarehouseReceiptInformationDto.wrpShippingPaymentTypeID = dataTable.Rows[0].Field<string>("wrpShippingPaymentTypeID");
			eRPWarehouseReceiptInformationDto.wrpSourceWarehouseID = dataTable.Rows[0].Field<string>("wrpSourceWarehouseID");
			eRPWarehouseReceiptInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPWarehouseReceiptInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPWarehouseReceiptInformationDto);
	}

	public Task<APIValidationInfoDto> SaveWarehouseReceipt(ERPWarehouseReceiptDto warehouseReceipt)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM WarehouseReceipts WHERE wrpUniqueID = " + M1Util.ConvertToLinq(warehouseReceipt.wrpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["wrpWarehouseReceiptID"] = warehouseReceipt.wrpWarehouseReceiptID.ToUpper();
				warehouseReceipt.wrpUniqueID = ((warehouseReceipt.wrpUniqueID == Guid.Empty) ? Guid.NewGuid() : warehouseReceipt.wrpUniqueID);
				dataRow["wrpUniqueID"] = warehouseReceipt.wrpUniqueID;
				dataRow["wrpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["wrpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The WarehouseReceipt could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (warehouseReceipt.wrpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the WarehouseReceipt is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["wrpRowVersion"], warehouseReceipt.wrpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the WarehouseReceipt has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the WarehouseReceipt again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? wrpClosedDate = warehouseReceipt.wrpClosedDate;
			dataRow2["wrpClosedDate"] = (wrpClosedDate.HasValue ? ((object)wrpClosedDate.GetValueOrDefault()) : dataRow["wrpClosedDate"]);
			dataRow["wrpDestinationWarehouseID"] = warehouseReceipt.wrpDestinationWarehouseID;
			dataRow["wrpFreightCharge"] = warehouseReceipt.wrpFreightCharge;
			dataRow["wrpClosed"] = warehouseReceipt.wrpClosed;
			dataRow["wrpPosted"] = warehouseReceipt.wrpPosted;
			dataRow["wrpReversalEntry"] = warehouseReceipt.wrpReversalEntry;
			dataRow["wrpReversed"] = warehouseReceipt.wrpReversed;
			DataRow dataRow3 = dataRow;
			wrpClosedDate = warehouseReceipt.wrpPostedDate;
			dataRow3["wrpPostedDate"] = (wrpClosedDate.HasValue ? ((object)wrpClosedDate.GetValueOrDefault()) : dataRow["wrpPostedDate"]);
			DataRow dataRow4 = dataRow;
			wrpClosedDate = warehouseReceipt.wrpReceiptDate;
			dataRow4["wrpReceiptDate"] = (wrpClosedDate.HasValue ? ((object)wrpClosedDate.GetValueOrDefault()) : dataRow["wrpReceiptDate"]);
			dataRow["wrpShippingMethodID"] = warehouseReceipt.wrpShippingMethodID;
			dataRow["wrpShippingPaymentTypeID"] = warehouseReceipt.wrpShippingPaymentTypeID;
			dataRow["wrpSourceWarehouseID"] = warehouseReceipt.wrpSourceWarehouseID;
			if (warehouseReceipt.CustomFields != null && warehouseReceipt.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in warehouseReceipt.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the WarehouseReceipt [{warehouseReceipt.wrpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the WarehouseReceipt [{warehouseReceipt.wrpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
