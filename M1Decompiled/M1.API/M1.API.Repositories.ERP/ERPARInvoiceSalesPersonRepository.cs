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

public class ERPARInvoiceSalesPersonRepository : APIBaseRepository, IERPARInvoiceSalesPersonRepository, IAPIBaseRepository, IDisposable
{
	public ERPARInvoiceSalesPersonRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesARInvoiceSalesPersonExist(Guid aRInvoiceSalesPersonId)
	{
		InitializeParameterLists();
		base.filterList.Add("arjUniqueID|C", aRInvoiceSalesPersonId);
		base.selectList.Add("arjUniqueID");
		return Task.FromResult(GetAsObject("ARInvoiceSalesPeople", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPARInvoiceSalesPersonInformationDto>> GetAllARInvoiceSalesPeople(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPARInvoiceSalesPersonInformationDto> collection = new List<ERPARInvoiceSalesPersonInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"arjAmount", "arjArInvoiceID", "arjCreatedBy", "arjCreatedDate", "arjUniqueID", "arjPostedToGl", "arjPercent", "arjRate", "arjRowVersion", "arjSalesEmployeeID",
			"arjSequenceID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ARInvoiceSalesPeople");
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
		using (DataTable dataTable = GetAsDataTable("ARInvoiceSalesPeople", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPARInvoiceSalesPersonInformationDto eRPARInvoiceSalesPersonInformationDto = new ERPARInvoiceSalesPersonInformationDto();
				eRPARInvoiceSalesPersonInformationDto.arjAmount = dataTable.Rows[i].Field<decimal>("arjAmount");
				eRPARInvoiceSalesPersonInformationDto.arjArInvoiceID = dataTable.Rows[i].Field<string>("arjArInvoiceID");
				eRPARInvoiceSalesPersonInformationDto.arjCreatedBy = dataTable.Rows[i].Field<string>("arjCreatedBy");
				eRPARInvoiceSalesPersonInformationDto.arjCreatedDate = dataTable.Rows[i].Field<DateTime?>("arjCreatedDate");
				eRPARInvoiceSalesPersonInformationDto.arjUniqueID = dataTable.Rows[i].Field<Guid>("arjUniqueID");
				eRPARInvoiceSalesPersonInformationDto.arjPostedToGl = dataTable.Rows[i].Field<bool>("arjPostedToGl");
				eRPARInvoiceSalesPersonInformationDto.arjPercent = dataTable.Rows[i].Field<decimal>("arjPercent");
				eRPARInvoiceSalesPersonInformationDto.arjRate = dataTable.Rows[i].Field<decimal>("arjRate");
				eRPARInvoiceSalesPersonInformationDto.arjRowVersion = dataTable.Rows[i].Field<byte[]>("arjRowVersion");
				eRPARInvoiceSalesPersonInformationDto.arjSalesEmployeeID = dataTable.Rows[i].Field<string>("arjSalesEmployeeID");
				eRPARInvoiceSalesPersonInformationDto.arjSequenceID = dataTable.Rows[i].Field<short>("arjSequenceID");
				eRPARInvoiceSalesPersonInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPARInvoiceSalesPersonInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPARInvoiceSalesPersonInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPARInvoiceSalesPersonInformationDto> GetARInvoiceSalesPerson(Guid aRInvoiceSalesPersonId)
	{
		ERPARInvoiceSalesPersonInformationDto eRPARInvoiceSalesPersonInformationDto = new ERPARInvoiceSalesPersonInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"arjAmount", "arjArInvoiceID", "arjCreatedBy", "arjCreatedDate", "arjUniqueID", "arjPostedToGl", "arjPercent", "arjRate", "arjRowVersion", "arjSalesEmployeeID",
			"arjSequenceID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("arjUniqueID|C", aRInvoiceSalesPersonId);
		AddCustomFieldsToSelectList("ARInvoiceSalesPeople");
		using (DataTable dataTable = GetAsDataTable("ARInvoiceSalesPeople", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPARInvoiceSalesPersonInformationDto);
			}
			eRPARInvoiceSalesPersonInformationDto.arjAmount = dataTable.Rows[0].Field<decimal>("arjAmount");
			eRPARInvoiceSalesPersonInformationDto.arjArInvoiceID = dataTable.Rows[0].Field<string>("arjArInvoiceID");
			eRPARInvoiceSalesPersonInformationDto.arjCreatedBy = dataTable.Rows[0].Field<string>("arjCreatedBy");
			eRPARInvoiceSalesPersonInformationDto.arjCreatedDate = dataTable.Rows[0].Field<DateTime?>("arjCreatedDate");
			eRPARInvoiceSalesPersonInformationDto.arjUniqueID = dataTable.Rows[0].Field<Guid>("arjUniqueID");
			eRPARInvoiceSalesPersonInformationDto.arjPostedToGl = dataTable.Rows[0].Field<bool>("arjPostedToGl");
			eRPARInvoiceSalesPersonInformationDto.arjPercent = dataTable.Rows[0].Field<decimal>("arjPercent");
			eRPARInvoiceSalesPersonInformationDto.arjRate = dataTable.Rows[0].Field<decimal>("arjRate");
			eRPARInvoiceSalesPersonInformationDto.arjRowVersion = dataTable.Rows[0].Field<byte[]>("arjRowVersion");
			eRPARInvoiceSalesPersonInformationDto.arjSalesEmployeeID = dataTable.Rows[0].Field<string>("arjSalesEmployeeID");
			eRPARInvoiceSalesPersonInformationDto.arjSequenceID = dataTable.Rows[0].Field<short>("arjSequenceID");
			eRPARInvoiceSalesPersonInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPARInvoiceSalesPersonInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPARInvoiceSalesPersonInformationDto);
	}

	public Task<APIValidationInfoDto> SaveARInvoiceSalesPerson(ERPARInvoiceSalesPersonDto aRInvoiceSalesPerson)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ARInvoiceSalesPeople WHERE arjUniqueID = " + M1Util.ConvertToLinq(aRInvoiceSalesPerson.arjUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["arjArInvoiceID"] = aRInvoiceSalesPerson.arjArInvoiceID.ToUpper();
				dataRow["arjSequenceID"] = aRInvoiceSalesPerson.arjSequenceID;
				aRInvoiceSalesPerson.arjUniqueID = ((aRInvoiceSalesPerson.arjUniqueID == Guid.Empty) ? Guid.NewGuid() : aRInvoiceSalesPerson.arjUniqueID);
				dataRow["arjUniqueID"] = aRInvoiceSalesPerson.arjUniqueID;
				dataRow["arjCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["arjCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ARInvoiceSalesPerson could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (aRInvoiceSalesPerson.arjRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ARInvoiceSalesPerson is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["arjRowVersion"], aRInvoiceSalesPerson.arjRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ARInvoiceSalesPerson has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ARInvoiceSalesPerson again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["arjAmount"] = aRInvoiceSalesPerson.arjAmount;
			dataRow["arjPostedToGl"] = aRInvoiceSalesPerson.arjPostedToGl;
			dataRow["arjPercent"] = aRInvoiceSalesPerson.arjPercent;
			dataRow["arjRate"] = aRInvoiceSalesPerson.arjRate;
			dataRow["arjSalesEmployeeID"] = aRInvoiceSalesPerson.arjSalesEmployeeID;
			if (aRInvoiceSalesPerson.CustomFields != null && aRInvoiceSalesPerson.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in aRInvoiceSalesPerson.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ARInvoiceSalesPerson [{aRInvoiceSalesPerson.arjUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ARInvoiceSalesPerson [{aRInvoiceSalesPerson.arjUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
