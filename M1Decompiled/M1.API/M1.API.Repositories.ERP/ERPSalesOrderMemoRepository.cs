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

public class ERPSalesOrderMemoRepository : APIBaseRepository, IERPSalesOrderMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPSalesOrderMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSalesOrderMemoExist(Guid salesOrderMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("omkUniqueID|C", salesOrderMemoId);
		base.selectList.Add("omkUniqueID");
		return Task.FromResult(GetAsObject("SalesOrderMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSalesOrderMemoInformationDto>> GetAllSalesOrderMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSalesOrderMemoInformationDto> collection = new List<ERPSalesOrderMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[14]
		{
			"omkCreatedBy", "omkCreatedDate", "omkUniqueID", "omkClosed", "omkLongDescriptionRtf", "omkLongDescriptionText", "omkMemoDate", "omkRowVersion", "omkSalesOrderID", "omkSalesOrderMemoID",
			"omkShortDescription", "omkShowInArInvoices", "omkShowInSalesOrders", "omkShowInShipments"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("SalesOrderMemos");
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
		using (DataTable dataTable = GetAsDataTable("SalesOrderMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSalesOrderMemoInformationDto eRPSalesOrderMemoInformationDto = new ERPSalesOrderMemoInformationDto();
				eRPSalesOrderMemoInformationDto.omkCreatedBy = dataTable.Rows[i].Field<string>("omkCreatedBy");
				eRPSalesOrderMemoInformationDto.omkCreatedDate = dataTable.Rows[i].Field<DateTime?>("omkCreatedDate");
				eRPSalesOrderMemoInformationDto.omkUniqueID = dataTable.Rows[i].Field<Guid>("omkUniqueID");
				eRPSalesOrderMemoInformationDto.omkClosed = dataTable.Rows[i].Field<bool>("omkClosed");
				eRPSalesOrderMemoInformationDto.omkLongDescriptionRtf = dataTable.Rows[i].Field<string>("omkLongDescriptionRtf");
				eRPSalesOrderMemoInformationDto.omkLongDescriptionText = dataTable.Rows[i].Field<string>("omkLongDescriptionText");
				eRPSalesOrderMemoInformationDto.omkMemoDate = dataTable.Rows[i].Field<DateTime?>("omkMemoDate");
				eRPSalesOrderMemoInformationDto.omkRowVersion = dataTable.Rows[i].Field<byte[]>("omkRowVersion");
				eRPSalesOrderMemoInformationDto.omkSalesOrderID = dataTable.Rows[i].Field<string>("omkSalesOrderID");
				eRPSalesOrderMemoInformationDto.omkSalesOrderMemoID = dataTable.Rows[i].Field<short>("omkSalesOrderMemoID");
				eRPSalesOrderMemoInformationDto.omkShortDescription = dataTable.Rows[i].Field<string>("omkShortDescription");
				eRPSalesOrderMemoInformationDto.omkShowInArInvoices = dataTable.Rows[i].Field<bool>("omkShowInArInvoices");
				eRPSalesOrderMemoInformationDto.omkShowInSalesOrders = dataTable.Rows[i].Field<bool>("omkShowInSalesOrders");
				eRPSalesOrderMemoInformationDto.omkShowInShipments = dataTable.Rows[i].Field<bool>("omkShowInShipments");
				eRPSalesOrderMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSalesOrderMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSalesOrderMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSalesOrderMemoInformationDto> GetSalesOrderMemo(Guid salesOrderMemoId)
	{
		ERPSalesOrderMemoInformationDto eRPSalesOrderMemoInformationDto = new ERPSalesOrderMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[14]
		{
			"omkCreatedBy", "omkCreatedDate", "omkUniqueID", "omkClosed", "omkLongDescriptionRtf", "omkLongDescriptionText", "omkMemoDate", "omkRowVersion", "omkSalesOrderID", "omkSalesOrderMemoID",
			"omkShortDescription", "omkShowInArInvoices", "omkShowInSalesOrders", "omkShowInShipments"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("omkUniqueID|C", salesOrderMemoId);
		AddCustomFieldsToSelectList("SalesOrderMemos");
		using (DataTable dataTable = GetAsDataTable("SalesOrderMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSalesOrderMemoInformationDto);
			}
			eRPSalesOrderMemoInformationDto.omkCreatedBy = dataTable.Rows[0].Field<string>("omkCreatedBy");
			eRPSalesOrderMemoInformationDto.omkCreatedDate = dataTable.Rows[0].Field<DateTime?>("omkCreatedDate");
			eRPSalesOrderMemoInformationDto.omkUniqueID = dataTable.Rows[0].Field<Guid>("omkUniqueID");
			eRPSalesOrderMemoInformationDto.omkClosed = dataTable.Rows[0].Field<bool>("omkClosed");
			eRPSalesOrderMemoInformationDto.omkLongDescriptionRtf = dataTable.Rows[0].Field<string>("omkLongDescriptionRtf");
			eRPSalesOrderMemoInformationDto.omkLongDescriptionText = dataTable.Rows[0].Field<string>("omkLongDescriptionText");
			eRPSalesOrderMemoInformationDto.omkMemoDate = dataTable.Rows[0].Field<DateTime?>("omkMemoDate");
			eRPSalesOrderMemoInformationDto.omkRowVersion = dataTable.Rows[0].Field<byte[]>("omkRowVersion");
			eRPSalesOrderMemoInformationDto.omkSalesOrderID = dataTable.Rows[0].Field<string>("omkSalesOrderID");
			eRPSalesOrderMemoInformationDto.omkSalesOrderMemoID = dataTable.Rows[0].Field<short>("omkSalesOrderMemoID");
			eRPSalesOrderMemoInformationDto.omkShortDescription = dataTable.Rows[0].Field<string>("omkShortDescription");
			eRPSalesOrderMemoInformationDto.omkShowInArInvoices = dataTable.Rows[0].Field<bool>("omkShowInArInvoices");
			eRPSalesOrderMemoInformationDto.omkShowInSalesOrders = dataTable.Rows[0].Field<bool>("omkShowInSalesOrders");
			eRPSalesOrderMemoInformationDto.omkShowInShipments = dataTable.Rows[0].Field<bool>("omkShowInShipments");
			eRPSalesOrderMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSalesOrderMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSalesOrderMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SaveSalesOrderMemo(ERPSalesOrderMemoDto salesOrderMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM SalesOrderMemos WHERE omkUniqueID = " + M1Util.ConvertToLinq(salesOrderMemo.omkUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["omkSalesOrderID"] = salesOrderMemo.omkSalesOrderID.ToUpper();
				dataRow["omkSalesOrderMemoID"] = salesOrderMemo.omkSalesOrderMemoID;
				salesOrderMemo.omkUniqueID = ((salesOrderMemo.omkUniqueID == Guid.Empty) ? Guid.NewGuid() : salesOrderMemo.omkUniqueID);
				dataRow["omkUniqueID"] = salesOrderMemo.omkUniqueID;
				dataRow["omkCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["omkCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The SalesOrderMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (salesOrderMemo.omkRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the SalesOrderMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["omkRowVersion"], salesOrderMemo.omkRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the SalesOrderMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the SalesOrderMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["omkClosed"] = salesOrderMemo.omkClosed;
			dataRow["omkLongDescriptionRtf"] = salesOrderMemo.omkLongDescriptionRtf ?? dataRow["omkLongDescriptionRtf"];
			dataRow["omkLongDescriptionText"] = salesOrderMemo.omkLongDescriptionText ?? dataRow["omkLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? omkMemoDate = salesOrderMemo.omkMemoDate;
			dataRow2["omkMemoDate"] = (omkMemoDate.HasValue ? ((object)omkMemoDate.GetValueOrDefault()) : dataRow["omkMemoDate"]);
			dataRow["omkShortDescription"] = salesOrderMemo.omkShortDescription;
			dataRow["omkShowInArInvoices"] = salesOrderMemo.omkShowInArInvoices;
			dataRow["omkShowInSalesOrders"] = salesOrderMemo.omkShowInSalesOrders;
			dataRow["omkShowInShipments"] = salesOrderMemo.omkShowInShipments;
			if (salesOrderMemo.CustomFields != null && salesOrderMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in salesOrderMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the SalesOrderMemo [{salesOrderMemo.omkUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the SalesOrderMemo [{salesOrderMemo.omkUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
