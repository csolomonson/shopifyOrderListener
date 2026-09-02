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

public class ERPQuoteMemoRepository : APIBaseRepository, IERPQuoteMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPQuoteMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesQuoteMemoExist(Guid quoteMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("qmkUniqueID|C", quoteMemoId);
		base.selectList.Add("qmkUniqueID");
		return Task.FromResult(GetAsObject("QuoteMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPQuoteMemoInformationDto>> GetAllQuoteMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPQuoteMemoInformationDto> collection = new List<ERPQuoteMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[13]
		{
			"qmkCreatedBy", "qmkCreatedDate", "qmkUniqueID", "qmkClosed", "qmkLongDescriptionRtf", "qmkLongDescriptionText", "qmkMemoDate", "qmkQuoteID", "qmkRowVersion", "qmkQuoteMemoID",
			"qmkShortDescription", "qmkShowInQuotes", "qmkShowInSalesOrders"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("QuoteMemos");
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
		using (DataTable dataTable = GetAsDataTable("QuoteMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPQuoteMemoInformationDto eRPQuoteMemoInformationDto = new ERPQuoteMemoInformationDto();
				eRPQuoteMemoInformationDto.qmkCreatedBy = dataTable.Rows[i].Field<string>("qmkCreatedBy");
				eRPQuoteMemoInformationDto.qmkCreatedDate = dataTable.Rows[i].Field<DateTime?>("qmkCreatedDate");
				eRPQuoteMemoInformationDto.qmkUniqueID = dataTable.Rows[i].Field<Guid>("qmkUniqueID");
				eRPQuoteMemoInformationDto.qmkClosed = dataTable.Rows[i].Field<bool>("qmkClosed");
				eRPQuoteMemoInformationDto.qmkLongDescriptionRtf = dataTable.Rows[i].Field<string>("qmkLongDescriptionRtf");
				eRPQuoteMemoInformationDto.qmkLongDescriptionText = dataTable.Rows[i].Field<string>("qmkLongDescriptionText");
				eRPQuoteMemoInformationDto.qmkMemoDate = dataTable.Rows[i].Field<DateTime?>("qmkMemoDate");
				eRPQuoteMemoInformationDto.qmkQuoteID = dataTable.Rows[i].Field<string>("qmkQuoteID");
				eRPQuoteMemoInformationDto.qmkRowVersion = dataTable.Rows[i].Field<byte[]>("qmkRowVersion");
				eRPQuoteMemoInformationDto.qmkQuoteMemoID = dataTable.Rows[i].Field<short>("qmkQuoteMemoID");
				eRPQuoteMemoInformationDto.qmkShortDescription = dataTable.Rows[i].Field<string>("qmkShortDescription");
				eRPQuoteMemoInformationDto.qmkShowInQuotes = dataTable.Rows[i].Field<bool>("qmkShowInQuotes");
				eRPQuoteMemoInformationDto.qmkShowInSalesOrders = dataTable.Rows[i].Field<bool>("qmkShowInSalesOrders");
				eRPQuoteMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPQuoteMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPQuoteMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPQuoteMemoInformationDto> GetQuoteMemo(Guid quoteMemoId)
	{
		ERPQuoteMemoInformationDto eRPQuoteMemoInformationDto = new ERPQuoteMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[13]
		{
			"qmkCreatedBy", "qmkCreatedDate", "qmkUniqueID", "qmkClosed", "qmkLongDescriptionRtf", "qmkLongDescriptionText", "qmkMemoDate", "qmkQuoteID", "qmkRowVersion", "qmkQuoteMemoID",
			"qmkShortDescription", "qmkShowInQuotes", "qmkShowInSalesOrders"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("qmkUniqueID|C", quoteMemoId);
		AddCustomFieldsToSelectList("QuoteMemos");
		using (DataTable dataTable = GetAsDataTable("QuoteMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPQuoteMemoInformationDto);
			}
			eRPQuoteMemoInformationDto.qmkCreatedBy = dataTable.Rows[0].Field<string>("qmkCreatedBy");
			eRPQuoteMemoInformationDto.qmkCreatedDate = dataTable.Rows[0].Field<DateTime?>("qmkCreatedDate");
			eRPQuoteMemoInformationDto.qmkUniqueID = dataTable.Rows[0].Field<Guid>("qmkUniqueID");
			eRPQuoteMemoInformationDto.qmkClosed = dataTable.Rows[0].Field<bool>("qmkClosed");
			eRPQuoteMemoInformationDto.qmkLongDescriptionRtf = dataTable.Rows[0].Field<string>("qmkLongDescriptionRtf");
			eRPQuoteMemoInformationDto.qmkLongDescriptionText = dataTable.Rows[0].Field<string>("qmkLongDescriptionText");
			eRPQuoteMemoInformationDto.qmkMemoDate = dataTable.Rows[0].Field<DateTime?>("qmkMemoDate");
			eRPQuoteMemoInformationDto.qmkQuoteID = dataTable.Rows[0].Field<string>("qmkQuoteID");
			eRPQuoteMemoInformationDto.qmkRowVersion = dataTable.Rows[0].Field<byte[]>("qmkRowVersion");
			eRPQuoteMemoInformationDto.qmkQuoteMemoID = dataTable.Rows[0].Field<short>("qmkQuoteMemoID");
			eRPQuoteMemoInformationDto.qmkShortDescription = dataTable.Rows[0].Field<string>("qmkShortDescription");
			eRPQuoteMemoInformationDto.qmkShowInQuotes = dataTable.Rows[0].Field<bool>("qmkShowInQuotes");
			eRPQuoteMemoInformationDto.qmkShowInSalesOrders = dataTable.Rows[0].Field<bool>("qmkShowInSalesOrders");
			eRPQuoteMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPQuoteMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPQuoteMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SaveQuoteMemo(ERPQuoteMemoDto quoteMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM QuoteMemos WHERE qmkUniqueID = " + M1Util.ConvertToLinq(quoteMemo.qmkUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qmkQuoteID"] = quoteMemo.qmkQuoteID.ToUpper();
				dataRow["qmkQuoteMemoID"] = quoteMemo.qmkQuoteMemoID;
				quoteMemo.qmkUniqueID = ((quoteMemo.qmkUniqueID == Guid.Empty) ? Guid.NewGuid() : quoteMemo.qmkUniqueID);
				dataRow["qmkUniqueID"] = quoteMemo.qmkUniqueID;
				dataRow["qmkCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qmkCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The QuoteMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (quoteMemo.qmkRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the QuoteMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["qmkRowVersion"], quoteMemo.qmkRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the QuoteMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the QuoteMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qmkClosed"] = quoteMemo.qmkClosed;
			dataRow["qmkLongDescriptionRtf"] = quoteMemo.qmkLongDescriptionRtf ?? dataRow["qmkLongDescriptionRtf"];
			dataRow["qmkLongDescriptionText"] = quoteMemo.qmkLongDescriptionText ?? dataRow["qmkLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? qmkMemoDate = quoteMemo.qmkMemoDate;
			dataRow2["qmkMemoDate"] = (qmkMemoDate.HasValue ? ((object)qmkMemoDate.GetValueOrDefault()) : dataRow["qmkMemoDate"]);
			dataRow["qmkShortDescription"] = quoteMemo.qmkShortDescription;
			dataRow["qmkShowInQuotes"] = quoteMemo.qmkShowInQuotes;
			dataRow["qmkShowInSalesOrders"] = quoteMemo.qmkShowInSalesOrders;
			if (quoteMemo.CustomFields != null && quoteMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in quoteMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the QuoteMemo [{quoteMemo.qmkUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the QuoteMemo [{quoteMemo.qmkUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
