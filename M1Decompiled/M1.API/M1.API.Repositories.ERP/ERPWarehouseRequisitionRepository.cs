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

public class ERPWarehouseRequisitionRepository : APIBaseRepository, IERPWarehouseRequisitionRepository, IAPIBaseRepository, IDisposable
{
	public ERPWarehouseRequisitionRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesWarehouseRequisitionExist(Guid warehouseRequisitionId)
	{
		InitializeParameterLists();
		base.filterList.Add("wqpUniqueID|C", warehouseRequisitionId);
		base.selectList.Add("wqpUniqueID");
		return Task.FromResult(GetAsObject("WarehouseRequisitions", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPWarehouseRequisitionInformationDto>> GetAllWarehouseRequisitions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPWarehouseRequisitionInformationDto> collection = new List<ERPWarehouseRequisitionInformationDto>();
		InitializeParameterLists();
		string[] array = new string[17]
		{
			"wqpClosedDate", "wqpWarehouseRequisitionID", "wqpCreatedBy", "wqpCreatedDate", "wqpDestinationWarehouseID", "wqpUniqueID", "wqpClosed", "wqpReadyToPrint", "wqpRequestedShipDate", "wqpRequisitionCommentsRTF",
			"wqpRequisitionCommentsText", "wqpRequisitionDate", "wqpRowVersion", "wqpShippingMethodID", "wqpShippingPaymentTypeID", "wqpSourceWarehouseID", "wqpStatus"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("WarehouseRequisitions");
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
		using (DataTable dataTable = GetAsDataTable("WarehouseRequisitions", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPWarehouseRequisitionInformationDto eRPWarehouseRequisitionInformationDto = new ERPWarehouseRequisitionInformationDto();
				eRPWarehouseRequisitionInformationDto.wqpClosedDate = dataTable.Rows[i].Field<DateTime?>("wqpClosedDate");
				eRPWarehouseRequisitionInformationDto.wqpWarehouseRequisitionID = dataTable.Rows[i].Field<string>("wqpWarehouseRequisitionID");
				eRPWarehouseRequisitionInformationDto.wqpCreatedBy = dataTable.Rows[i].Field<string>("wqpCreatedBy");
				eRPWarehouseRequisitionInformationDto.wqpCreatedDate = dataTable.Rows[i].Field<DateTime?>("wqpCreatedDate");
				eRPWarehouseRequisitionInformationDto.wqpDestinationWarehouseID = dataTable.Rows[i].Field<string>("wqpDestinationWarehouseID");
				eRPWarehouseRequisitionInformationDto.wqpUniqueID = dataTable.Rows[i].Field<Guid>("wqpUniqueID");
				eRPWarehouseRequisitionInformationDto.wqpClosed = dataTable.Rows[i].Field<bool>("wqpClosed");
				eRPWarehouseRequisitionInformationDto.wqpReadyToPrint = dataTable.Rows[i].Field<bool>("wqpReadyToPrint");
				eRPWarehouseRequisitionInformationDto.wqpRequestedShipDate = dataTable.Rows[i].Field<DateTime?>("wqpRequestedShipDate");
				eRPWarehouseRequisitionInformationDto.wqpRequisitionCommentsRTF = dataTable.Rows[i].Field<string>("wqpRequisitionCommentsRTF");
				eRPWarehouseRequisitionInformationDto.wqpRequisitionCommentsText = dataTable.Rows[i].Field<string>("wqpRequisitionCommentsText");
				eRPWarehouseRequisitionInformationDto.wqpRequisitionDate = dataTable.Rows[i].Field<DateTime?>("wqpRequisitionDate");
				eRPWarehouseRequisitionInformationDto.wqpRowVersion = dataTable.Rows[i].Field<byte[]>("wqpRowVersion");
				eRPWarehouseRequisitionInformationDto.wqpShippingMethodID = dataTable.Rows[i].Field<string>("wqpShippingMethodID");
				eRPWarehouseRequisitionInformationDto.wqpShippingPaymentTypeID = dataTable.Rows[i].Field<string>("wqpShippingPaymentTypeID");
				eRPWarehouseRequisitionInformationDto.wqpSourceWarehouseID = dataTable.Rows[i].Field<string>("wqpSourceWarehouseID");
				eRPWarehouseRequisitionInformationDto.wqpStatus = dataTable.Rows[i].Field<byte>("wqpStatus");
				eRPWarehouseRequisitionInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPWarehouseRequisitionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPWarehouseRequisitionInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPWarehouseRequisitionInformationDto> GetWarehouseRequisition(Guid warehouseRequisitionId)
	{
		ERPWarehouseRequisitionInformationDto eRPWarehouseRequisitionInformationDto = new ERPWarehouseRequisitionInformationDto();
		InitializeParameterLists();
		string[] collection = new string[17]
		{
			"wqpClosedDate", "wqpWarehouseRequisitionID", "wqpCreatedBy", "wqpCreatedDate", "wqpDestinationWarehouseID", "wqpUniqueID", "wqpClosed", "wqpReadyToPrint", "wqpRequestedShipDate", "wqpRequisitionCommentsRTF",
			"wqpRequisitionCommentsText", "wqpRequisitionDate", "wqpRowVersion", "wqpShippingMethodID", "wqpShippingPaymentTypeID", "wqpSourceWarehouseID", "wqpStatus"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("wqpUniqueID|C", warehouseRequisitionId);
		AddCustomFieldsToSelectList("WarehouseRequisitions");
		using (DataTable dataTable = GetAsDataTable("WarehouseRequisitions", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPWarehouseRequisitionInformationDto);
			}
			eRPWarehouseRequisitionInformationDto.wqpClosedDate = dataTable.Rows[0].Field<DateTime?>("wqpClosedDate");
			eRPWarehouseRequisitionInformationDto.wqpWarehouseRequisitionID = dataTable.Rows[0].Field<string>("wqpWarehouseRequisitionID");
			eRPWarehouseRequisitionInformationDto.wqpCreatedBy = dataTable.Rows[0].Field<string>("wqpCreatedBy");
			eRPWarehouseRequisitionInformationDto.wqpCreatedDate = dataTable.Rows[0].Field<DateTime?>("wqpCreatedDate");
			eRPWarehouseRequisitionInformationDto.wqpDestinationWarehouseID = dataTable.Rows[0].Field<string>("wqpDestinationWarehouseID");
			eRPWarehouseRequisitionInformationDto.wqpUniqueID = dataTable.Rows[0].Field<Guid>("wqpUniqueID");
			eRPWarehouseRequisitionInformationDto.wqpClosed = dataTable.Rows[0].Field<bool>("wqpClosed");
			eRPWarehouseRequisitionInformationDto.wqpReadyToPrint = dataTable.Rows[0].Field<bool>("wqpReadyToPrint");
			eRPWarehouseRequisitionInformationDto.wqpRequestedShipDate = dataTable.Rows[0].Field<DateTime?>("wqpRequestedShipDate");
			eRPWarehouseRequisitionInformationDto.wqpRequisitionCommentsRTF = dataTable.Rows[0].Field<string>("wqpRequisitionCommentsRTF");
			eRPWarehouseRequisitionInformationDto.wqpRequisitionCommentsText = dataTable.Rows[0].Field<string>("wqpRequisitionCommentsText");
			eRPWarehouseRequisitionInformationDto.wqpRequisitionDate = dataTable.Rows[0].Field<DateTime?>("wqpRequisitionDate");
			eRPWarehouseRequisitionInformationDto.wqpRowVersion = dataTable.Rows[0].Field<byte[]>("wqpRowVersion");
			eRPWarehouseRequisitionInformationDto.wqpShippingMethodID = dataTable.Rows[0].Field<string>("wqpShippingMethodID");
			eRPWarehouseRequisitionInformationDto.wqpShippingPaymentTypeID = dataTable.Rows[0].Field<string>("wqpShippingPaymentTypeID");
			eRPWarehouseRequisitionInformationDto.wqpSourceWarehouseID = dataTable.Rows[0].Field<string>("wqpSourceWarehouseID");
			eRPWarehouseRequisitionInformationDto.wqpStatus = dataTable.Rows[0].Field<byte>("wqpStatus");
			eRPWarehouseRequisitionInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPWarehouseRequisitionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPWarehouseRequisitionInformationDto);
	}

	public Task<APIValidationInfoDto> SaveWarehouseRequisition(ERPWarehouseRequisitionDto warehouseRequisition)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM WarehouseRequisitions WHERE wqpUniqueID = " + M1Util.ConvertToLinq(warehouseRequisition.wqpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["wqpWarehouseRequisitionID"] = warehouseRequisition.wqpWarehouseRequisitionID.ToUpper();
				warehouseRequisition.wqpUniqueID = ((warehouseRequisition.wqpUniqueID == Guid.Empty) ? Guid.NewGuid() : warehouseRequisition.wqpUniqueID);
				dataRow["wqpUniqueID"] = warehouseRequisition.wqpUniqueID;
				dataRow["wqpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["wqpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The WarehouseRequisition could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (warehouseRequisition.wqpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the WarehouseRequisition is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["wqpRowVersion"], warehouseRequisition.wqpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the WarehouseRequisition has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the WarehouseRequisition again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? wqpClosedDate = warehouseRequisition.wqpClosedDate;
			dataRow2["wqpClosedDate"] = (wqpClosedDate.HasValue ? ((object)wqpClosedDate.GetValueOrDefault()) : dataRow["wqpClosedDate"]);
			dataRow["wqpDestinationWarehouseID"] = warehouseRequisition.wqpDestinationWarehouseID;
			dataRow["wqpClosed"] = warehouseRequisition.wqpClosed;
			dataRow["wqpReadyToPrint"] = warehouseRequisition.wqpReadyToPrint;
			DataRow dataRow3 = dataRow;
			wqpClosedDate = warehouseRequisition.wqpRequestedShipDate;
			dataRow3["wqpRequestedShipDate"] = (wqpClosedDate.HasValue ? ((object)wqpClosedDate.GetValueOrDefault()) : dataRow["wqpRequestedShipDate"]);
			dataRow["wqpRequisitionCommentsRTF"] = warehouseRequisition.wqpRequisitionCommentsRTF ?? dataRow["wqpRequisitionCommentsRTF"];
			dataRow["wqpRequisitionCommentsText"] = warehouseRequisition.wqpRequisitionCommentsText ?? dataRow["wqpRequisitionCommentsText"];
			DataRow dataRow4 = dataRow;
			wqpClosedDate = warehouseRequisition.wqpRequisitionDate;
			dataRow4["wqpRequisitionDate"] = (wqpClosedDate.HasValue ? ((object)wqpClosedDate.GetValueOrDefault()) : dataRow["wqpRequisitionDate"]);
			dataRow["wqpShippingMethodID"] = warehouseRequisition.wqpShippingMethodID;
			dataRow["wqpShippingPaymentTypeID"] = warehouseRequisition.wqpShippingPaymentTypeID;
			dataRow["wqpSourceWarehouseID"] = warehouseRequisition.wqpSourceWarehouseID;
			dataRow["wqpStatus"] = warehouseRequisition.wqpStatus;
			if (warehouseRequisition.CustomFields != null && warehouseRequisition.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in warehouseRequisition.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the WarehouseRequisition [{warehouseRequisition.wqpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the WarehouseRequisition [{warehouseRequisition.wqpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
