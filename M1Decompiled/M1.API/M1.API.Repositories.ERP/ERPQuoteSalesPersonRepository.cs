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

public class ERPQuoteSalesPersonRepository : APIBaseRepository, IERPQuoteSalesPersonRepository, IAPIBaseRepository, IDisposable
{
	public ERPQuoteSalesPersonRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesQuoteSalesPersonExist(Guid quoteSalesPersonId)
	{
		InitializeParameterLists();
		base.filterList.Add("qmjUniqueID|C", quoteSalesPersonId);
		base.selectList.Add("qmjUniqueID");
		return Task.FromResult(GetAsObject("QuoteSalesPeople", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPQuoteSalesPersonInformationDto>> GetAllQuoteSalesPeople(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPQuoteSalesPersonInformationDto> collection = new List<ERPQuoteSalesPersonInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "qmjCreatedBy", "qmjCreatedDate", "qmjUniqueID", "qmjClosed", "qmjCreatedFromMobile", "qmjPercent", "qmjQuoteID", "qmjRowVersion", "qmjSalesEmployeeID", "qmjSequenceID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("QuoteSalesPeople");
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
		using (DataTable dataTable = GetAsDataTable("QuoteSalesPeople", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPQuoteSalesPersonInformationDto eRPQuoteSalesPersonInformationDto = new ERPQuoteSalesPersonInformationDto();
				eRPQuoteSalesPersonInformationDto.qmjCreatedBy = dataTable.Rows[i].Field<string>("qmjCreatedBy");
				eRPQuoteSalesPersonInformationDto.qmjCreatedDate = dataTable.Rows[i].Field<DateTime?>("qmjCreatedDate");
				eRPQuoteSalesPersonInformationDto.qmjUniqueID = dataTable.Rows[i].Field<Guid>("qmjUniqueID");
				eRPQuoteSalesPersonInformationDto.qmjClosed = dataTable.Rows[i].Field<bool>("qmjClosed");
				eRPQuoteSalesPersonInformationDto.qmjCreatedFromMobile = dataTable.Rows[i].Field<bool>("qmjCreatedFromMobile");
				eRPQuoteSalesPersonInformationDto.qmjPercent = dataTable.Rows[i].Field<decimal>("qmjPercent");
				eRPQuoteSalesPersonInformationDto.qmjQuoteID = dataTable.Rows[i].Field<string>("qmjQuoteID");
				eRPQuoteSalesPersonInformationDto.qmjRowVersion = dataTable.Rows[i].Field<byte[]>("qmjRowVersion");
				eRPQuoteSalesPersonInformationDto.qmjSalesEmployeeID = dataTable.Rows[i].Field<string>("qmjSalesEmployeeID");
				eRPQuoteSalesPersonInformationDto.qmjSequenceID = dataTable.Rows[i].Field<short>("qmjSequenceID");
				eRPQuoteSalesPersonInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPQuoteSalesPersonInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPQuoteSalesPersonInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPQuoteSalesPersonInformationDto> GetQuoteSalesPerson(Guid quoteSalesPersonId)
	{
		ERPQuoteSalesPersonInformationDto eRPQuoteSalesPersonInformationDto = new ERPQuoteSalesPersonInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "qmjCreatedBy", "qmjCreatedDate", "qmjUniqueID", "qmjClosed", "qmjCreatedFromMobile", "qmjPercent", "qmjQuoteID", "qmjRowVersion", "qmjSalesEmployeeID", "qmjSequenceID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("qmjUniqueID|C", quoteSalesPersonId);
		AddCustomFieldsToSelectList("QuoteSalesPeople");
		using (DataTable dataTable = GetAsDataTable("QuoteSalesPeople", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPQuoteSalesPersonInformationDto);
			}
			eRPQuoteSalesPersonInformationDto.qmjCreatedBy = dataTable.Rows[0].Field<string>("qmjCreatedBy");
			eRPQuoteSalesPersonInformationDto.qmjCreatedDate = dataTable.Rows[0].Field<DateTime?>("qmjCreatedDate");
			eRPQuoteSalesPersonInformationDto.qmjUniqueID = dataTable.Rows[0].Field<Guid>("qmjUniqueID");
			eRPQuoteSalesPersonInformationDto.qmjClosed = dataTable.Rows[0].Field<bool>("qmjClosed");
			eRPQuoteSalesPersonInformationDto.qmjCreatedFromMobile = dataTable.Rows[0].Field<bool>("qmjCreatedFromMobile");
			eRPQuoteSalesPersonInformationDto.qmjPercent = dataTable.Rows[0].Field<decimal>("qmjPercent");
			eRPQuoteSalesPersonInformationDto.qmjQuoteID = dataTable.Rows[0].Field<string>("qmjQuoteID");
			eRPQuoteSalesPersonInformationDto.qmjRowVersion = dataTable.Rows[0].Field<byte[]>("qmjRowVersion");
			eRPQuoteSalesPersonInformationDto.qmjSalesEmployeeID = dataTable.Rows[0].Field<string>("qmjSalesEmployeeID");
			eRPQuoteSalesPersonInformationDto.qmjSequenceID = dataTable.Rows[0].Field<short>("qmjSequenceID");
			eRPQuoteSalesPersonInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPQuoteSalesPersonInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPQuoteSalesPersonInformationDto);
	}

	public Task<APIValidationInfoDto> SaveQuoteSalesPerson(ERPQuoteSalesPersonDto quoteSalesPerson)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM QuoteSalesPeople WHERE qmjUniqueID = " + M1Util.ConvertToLinq(quoteSalesPerson.qmjUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qmjQuoteID"] = quoteSalesPerson.qmjQuoteID.ToUpper();
				dataRow["qmjSequenceID"] = quoteSalesPerson.qmjSequenceID;
				quoteSalesPerson.qmjUniqueID = ((quoteSalesPerson.qmjUniqueID == Guid.Empty) ? Guid.NewGuid() : quoteSalesPerson.qmjUniqueID);
				dataRow["qmjUniqueID"] = quoteSalesPerson.qmjUniqueID;
				dataRow["qmjCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qmjCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The QuoteSalesPerson could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (quoteSalesPerson.qmjRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the QuoteSalesPerson is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["qmjRowVersion"], quoteSalesPerson.qmjRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the QuoteSalesPerson has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the QuoteSalesPerson again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qmjClosed"] = quoteSalesPerson.qmjClosed;
			dataRow["qmjCreatedFromMobile"] = quoteSalesPerson.qmjCreatedFromMobile;
			dataRow["qmjPercent"] = quoteSalesPerson.qmjPercent;
			dataRow["qmjSalesEmployeeID"] = quoteSalesPerson.qmjSalesEmployeeID;
			if (quoteSalesPerson.CustomFields != null && quoteSalesPerson.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in quoteSalesPerson.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the QuoteSalesPerson [{quoteSalesPerson.qmjUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the QuoteSalesPerson [{quoteSalesPerson.qmjUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
