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

public class ERPAPInvoiceMemoRepository : APIBaseRepository, IERPAPInvoiceMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPAPInvoiceMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAPInvoiceMemoExist(Guid aPInvoiceMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("apiUniqueID|C", aPInvoiceMemoId);
		base.selectList.Add("apiUniqueID");
		return Task.FromResult(GetAsObject("APInvoiceMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAPInvoiceMemoInformationDto>> GetAllAPInvoiceMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAPInvoiceMemoInformationDto> collection = new List<ERPAPInvoiceMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[12]
		{
			"apiApInvoiceID", "apiCreatedBy", "apiCreatedDate", "apiUniqueID", "apiLongDescriptionRtf", "apiLongDescriptionText", "apiMemoDate", "apiRowVersion", "apiApInvoiceMemoID", "apiShortDescription",
			"apiShowInApInvoices", "apiShowInApPayments"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("APInvoiceMemos");
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
		using (DataTable dataTable = GetAsDataTable("APInvoiceMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAPInvoiceMemoInformationDto eRPAPInvoiceMemoInformationDto = new ERPAPInvoiceMemoInformationDto();
				eRPAPInvoiceMemoInformationDto.apiApInvoiceID = dataTable.Rows[i].Field<string>("apiApInvoiceID");
				eRPAPInvoiceMemoInformationDto.apiCreatedBy = dataTable.Rows[i].Field<string>("apiCreatedBy");
				eRPAPInvoiceMemoInformationDto.apiCreatedDate = dataTable.Rows[i].Field<DateTime?>("apiCreatedDate");
				eRPAPInvoiceMemoInformationDto.apiUniqueID = dataTable.Rows[i].Field<Guid>("apiUniqueID");
				eRPAPInvoiceMemoInformationDto.apiLongDescriptionRtf = dataTable.Rows[i].Field<string>("apiLongDescriptionRtf");
				eRPAPInvoiceMemoInformationDto.apiLongDescriptionText = dataTable.Rows[i].Field<string>("apiLongDescriptionText");
				eRPAPInvoiceMemoInformationDto.apiMemoDate = dataTable.Rows[i].Field<DateTime?>("apiMemoDate");
				eRPAPInvoiceMemoInformationDto.apiRowVersion = dataTable.Rows[i].Field<byte[]>("apiRowVersion");
				eRPAPInvoiceMemoInformationDto.apiApInvoiceMemoID = dataTable.Rows[i].Field<short>("apiApInvoiceMemoID");
				eRPAPInvoiceMemoInformationDto.apiShortDescription = dataTable.Rows[i].Field<string>("apiShortDescription");
				eRPAPInvoiceMemoInformationDto.apiShowInApInvoices = dataTable.Rows[i].Field<bool>("apiShowInApInvoices");
				eRPAPInvoiceMemoInformationDto.apiShowInApPayments = dataTable.Rows[i].Field<bool>("apiShowInApPayments");
				eRPAPInvoiceMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAPInvoiceMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAPInvoiceMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAPInvoiceMemoInformationDto> GetAPInvoiceMemo(Guid aPInvoiceMemoId)
	{
		ERPAPInvoiceMemoInformationDto eRPAPInvoiceMemoInformationDto = new ERPAPInvoiceMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[12]
		{
			"apiApInvoiceID", "apiCreatedBy", "apiCreatedDate", "apiUniqueID", "apiLongDescriptionRtf", "apiLongDescriptionText", "apiMemoDate", "apiRowVersion", "apiApInvoiceMemoID", "apiShortDescription",
			"apiShowInApInvoices", "apiShowInApPayments"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("apiUniqueID|C", aPInvoiceMemoId);
		AddCustomFieldsToSelectList("APInvoiceMemos");
		using (DataTable dataTable = GetAsDataTable("APInvoiceMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAPInvoiceMemoInformationDto);
			}
			eRPAPInvoiceMemoInformationDto.apiApInvoiceID = dataTable.Rows[0].Field<string>("apiApInvoiceID");
			eRPAPInvoiceMemoInformationDto.apiCreatedBy = dataTable.Rows[0].Field<string>("apiCreatedBy");
			eRPAPInvoiceMemoInformationDto.apiCreatedDate = dataTable.Rows[0].Field<DateTime?>("apiCreatedDate");
			eRPAPInvoiceMemoInformationDto.apiUniqueID = dataTable.Rows[0].Field<Guid>("apiUniqueID");
			eRPAPInvoiceMemoInformationDto.apiLongDescriptionRtf = dataTable.Rows[0].Field<string>("apiLongDescriptionRtf");
			eRPAPInvoiceMemoInformationDto.apiLongDescriptionText = dataTable.Rows[0].Field<string>("apiLongDescriptionText");
			eRPAPInvoiceMemoInformationDto.apiMemoDate = dataTable.Rows[0].Field<DateTime?>("apiMemoDate");
			eRPAPInvoiceMemoInformationDto.apiRowVersion = dataTable.Rows[0].Field<byte[]>("apiRowVersion");
			eRPAPInvoiceMemoInformationDto.apiApInvoiceMemoID = dataTable.Rows[0].Field<short>("apiApInvoiceMemoID");
			eRPAPInvoiceMemoInformationDto.apiShortDescription = dataTable.Rows[0].Field<string>("apiShortDescription");
			eRPAPInvoiceMemoInformationDto.apiShowInApInvoices = dataTable.Rows[0].Field<bool>("apiShowInApInvoices");
			eRPAPInvoiceMemoInformationDto.apiShowInApPayments = dataTable.Rows[0].Field<bool>("apiShowInApPayments");
			eRPAPInvoiceMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAPInvoiceMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAPInvoiceMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SaveAPInvoiceMemo(ERPAPInvoiceMemoDto aPInvoiceMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM APInvoiceMemos WHERE apiUniqueID = " + M1Util.ConvertToLinq(aPInvoiceMemo.apiUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["apiApInvoiceID"] = aPInvoiceMemo.apiApInvoiceID.ToUpper();
				dataRow["apiApInvoiceMemoID"] = aPInvoiceMemo.apiApInvoiceMemoID;
				aPInvoiceMemo.apiUniqueID = ((aPInvoiceMemo.apiUniqueID == Guid.Empty) ? Guid.NewGuid() : aPInvoiceMemo.apiUniqueID);
				dataRow["apiUniqueID"] = aPInvoiceMemo.apiUniqueID;
				dataRow["apiCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["apiCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The APInvoiceMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (aPInvoiceMemo.apiRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the APInvoiceMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["apiRowVersion"], aPInvoiceMemo.apiRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the APInvoiceMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the APInvoiceMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["apiLongDescriptionRtf"] = aPInvoiceMemo.apiLongDescriptionRtf ?? dataRow["apiLongDescriptionRtf"];
			dataRow["apiLongDescriptionText"] = aPInvoiceMemo.apiLongDescriptionText ?? dataRow["apiLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? apiMemoDate = aPInvoiceMemo.apiMemoDate;
			dataRow2["apiMemoDate"] = (apiMemoDate.HasValue ? ((object)apiMemoDate.GetValueOrDefault()) : dataRow["apiMemoDate"]);
			dataRow["apiShortDescription"] = aPInvoiceMemo.apiShortDescription;
			dataRow["apiShowInApInvoices"] = aPInvoiceMemo.apiShowInApInvoices;
			dataRow["apiShowInApPayments"] = aPInvoiceMemo.apiShowInApPayments;
			if (aPInvoiceMemo.CustomFields != null && aPInvoiceMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in aPInvoiceMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the APInvoiceMemo [{aPInvoiceMemo.apiUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the APInvoiceMemo [{aPInvoiceMemo.apiUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
