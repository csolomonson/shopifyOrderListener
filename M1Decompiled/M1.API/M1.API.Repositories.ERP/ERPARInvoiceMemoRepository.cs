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

public class ERPARInvoiceMemoRepository : APIBaseRepository, IERPARInvoiceMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPARInvoiceMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesARInvoiceMemoExist(Guid aRInvoiceMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("ariUniqueID|C", aRInvoiceMemoId);
		base.selectList.Add("ariUniqueID");
		return Task.FromResult(GetAsObject("ARInvoiceMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPARInvoiceMemoInformationDto>> GetAllARInvoiceMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPARInvoiceMemoInformationDto> collection = new List<ERPARInvoiceMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[12]
		{
			"ariArInvoiceID", "ariCreatedBy", "ariCreatedDate", "ariUniqueID", "ariLongDescriptionRtf", "ariLongDescriptionText", "ariMemoDate", "ariRowVersion", "ariArInvoiceMemoID", "ariShortDescription",
			"ariShowInArInvoices", "ariShowInArPayments"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ARInvoiceMemos");
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
		using (DataTable dataTable = GetAsDataTable("ARInvoiceMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPARInvoiceMemoInformationDto eRPARInvoiceMemoInformationDto = new ERPARInvoiceMemoInformationDto();
				eRPARInvoiceMemoInformationDto.ariArInvoiceID = dataTable.Rows[i].Field<string>("ariArInvoiceID");
				eRPARInvoiceMemoInformationDto.ariCreatedBy = dataTable.Rows[i].Field<string>("ariCreatedBy");
				eRPARInvoiceMemoInformationDto.ariCreatedDate = dataTable.Rows[i].Field<DateTime?>("ariCreatedDate");
				eRPARInvoiceMemoInformationDto.ariUniqueID = dataTable.Rows[i].Field<Guid>("ariUniqueID");
				eRPARInvoiceMemoInformationDto.ariLongDescriptionRtf = dataTable.Rows[i].Field<string>("ariLongDescriptionRtf");
				eRPARInvoiceMemoInformationDto.ariLongDescriptionText = dataTable.Rows[i].Field<string>("ariLongDescriptionText");
				eRPARInvoiceMemoInformationDto.ariMemoDate = dataTable.Rows[i].Field<DateTime?>("ariMemoDate");
				eRPARInvoiceMemoInformationDto.ariRowVersion = dataTable.Rows[i].Field<byte[]>("ariRowVersion");
				eRPARInvoiceMemoInformationDto.ariArInvoiceMemoID = dataTable.Rows[i].Field<short>("ariArInvoiceMemoID");
				eRPARInvoiceMemoInformationDto.ariShortDescription = dataTable.Rows[i].Field<string>("ariShortDescription");
				eRPARInvoiceMemoInformationDto.ariShowInArInvoices = dataTable.Rows[i].Field<bool>("ariShowInArInvoices");
				eRPARInvoiceMemoInformationDto.ariShowInArPayments = dataTable.Rows[i].Field<bool>("ariShowInArPayments");
				eRPARInvoiceMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPARInvoiceMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPARInvoiceMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPARInvoiceMemoInformationDto> GetARInvoiceMemo(Guid aRInvoiceMemoId)
	{
		ERPARInvoiceMemoInformationDto eRPARInvoiceMemoInformationDto = new ERPARInvoiceMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[12]
		{
			"ariArInvoiceID", "ariCreatedBy", "ariCreatedDate", "ariUniqueID", "ariLongDescriptionRtf", "ariLongDescriptionText", "ariMemoDate", "ariRowVersion", "ariArInvoiceMemoID", "ariShortDescription",
			"ariShowInArInvoices", "ariShowInArPayments"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("ariUniqueID|C", aRInvoiceMemoId);
		AddCustomFieldsToSelectList("ARInvoiceMemos");
		using (DataTable dataTable = GetAsDataTable("ARInvoiceMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPARInvoiceMemoInformationDto);
			}
			eRPARInvoiceMemoInformationDto.ariArInvoiceID = dataTable.Rows[0].Field<string>("ariArInvoiceID");
			eRPARInvoiceMemoInformationDto.ariCreatedBy = dataTable.Rows[0].Field<string>("ariCreatedBy");
			eRPARInvoiceMemoInformationDto.ariCreatedDate = dataTable.Rows[0].Field<DateTime?>("ariCreatedDate");
			eRPARInvoiceMemoInformationDto.ariUniqueID = dataTable.Rows[0].Field<Guid>("ariUniqueID");
			eRPARInvoiceMemoInformationDto.ariLongDescriptionRtf = dataTable.Rows[0].Field<string>("ariLongDescriptionRtf");
			eRPARInvoiceMemoInformationDto.ariLongDescriptionText = dataTable.Rows[0].Field<string>("ariLongDescriptionText");
			eRPARInvoiceMemoInformationDto.ariMemoDate = dataTable.Rows[0].Field<DateTime?>("ariMemoDate");
			eRPARInvoiceMemoInformationDto.ariRowVersion = dataTable.Rows[0].Field<byte[]>("ariRowVersion");
			eRPARInvoiceMemoInformationDto.ariArInvoiceMemoID = dataTable.Rows[0].Field<short>("ariArInvoiceMemoID");
			eRPARInvoiceMemoInformationDto.ariShortDescription = dataTable.Rows[0].Field<string>("ariShortDescription");
			eRPARInvoiceMemoInformationDto.ariShowInArInvoices = dataTable.Rows[0].Field<bool>("ariShowInArInvoices");
			eRPARInvoiceMemoInformationDto.ariShowInArPayments = dataTable.Rows[0].Field<bool>("ariShowInArPayments");
			eRPARInvoiceMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPARInvoiceMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPARInvoiceMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SaveARInvoiceMemo(ERPARInvoiceMemoDto aRInvoiceMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ARInvoiceMemos WHERE ariUniqueID = " + M1Util.ConvertToLinq(aRInvoiceMemo.ariUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["ariArInvoiceID"] = aRInvoiceMemo.ariArInvoiceID.ToUpper();
				dataRow["ariArInvoiceMemoID"] = aRInvoiceMemo.ariArInvoiceMemoID;
				aRInvoiceMemo.ariUniqueID = ((aRInvoiceMemo.ariUniqueID == Guid.Empty) ? Guid.NewGuid() : aRInvoiceMemo.ariUniqueID);
				dataRow["ariUniqueID"] = aRInvoiceMemo.ariUniqueID;
				dataRow["ariCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["ariCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ARInvoiceMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (aRInvoiceMemo.ariRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ARInvoiceMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["ariRowVersion"], aRInvoiceMemo.ariRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ARInvoiceMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ARInvoiceMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["ariLongDescriptionRtf"] = aRInvoiceMemo.ariLongDescriptionRtf ?? dataRow["ariLongDescriptionRtf"];
			dataRow["ariLongDescriptionText"] = aRInvoiceMemo.ariLongDescriptionText ?? dataRow["ariLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? ariMemoDate = aRInvoiceMemo.ariMemoDate;
			dataRow2["ariMemoDate"] = (ariMemoDate.HasValue ? ((object)ariMemoDate.GetValueOrDefault()) : dataRow["ariMemoDate"]);
			dataRow["ariShortDescription"] = aRInvoiceMemo.ariShortDescription;
			dataRow["ariShowInArInvoices"] = aRInvoiceMemo.ariShowInArInvoices;
			dataRow["ariShowInArPayments"] = aRInvoiceMemo.ariShowInArPayments;
			if (aRInvoiceMemo.CustomFields != null && aRInvoiceMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in aRInvoiceMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ARInvoiceMemo [{aRInvoiceMemo.ariUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ARInvoiceMemo [{aRInvoiceMemo.ariUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
