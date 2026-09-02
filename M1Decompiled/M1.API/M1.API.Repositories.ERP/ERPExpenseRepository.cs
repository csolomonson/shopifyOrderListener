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

public class ERPExpenseRepository : APIBaseRepository, IERPExpenseRepository, IAPIBaseRepository, IDisposable
{
	public ERPExpenseRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesExpenseExist(Guid expenseId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmxUniqueID|C", expenseId);
		base.selectList.Add("lmxUniqueID");
		return Task.FromResult(GetAsObject("Expenses", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPExpenseInformationDto>> GetAllExpenses(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPExpenseInformationDto> collection = new List<ERPExpenseInformationDto>();
		InitializeParameterLists();
		string[] array = new string[6] { "lmxExpenseID", "lmxCreatedBy", "lmxCreatedDate", "lmxDescription", "lmxUniqueID", "lmxRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Expenses");
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
		using (DataTable dataTable = GetAsDataTable("Expenses", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPExpenseInformationDto eRPExpenseInformationDto = new ERPExpenseInformationDto();
				eRPExpenseInformationDto.lmxExpenseID = dataTable.Rows[i].Field<string>("lmxExpenseID");
				eRPExpenseInformationDto.lmxCreatedBy = dataTable.Rows[i].Field<string>("lmxCreatedBy");
				eRPExpenseInformationDto.lmxCreatedDate = dataTable.Rows[i].Field<DateTime?>("lmxCreatedDate");
				eRPExpenseInformationDto.lmxDescription = dataTable.Rows[i].Field<string>("lmxDescription");
				eRPExpenseInformationDto.lmxUniqueID = dataTable.Rows[i].Field<Guid>("lmxUniqueID");
				eRPExpenseInformationDto.lmxRowVersion = dataTable.Rows[i].Field<byte[]>("lmxRowVersion");
				eRPExpenseInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPExpenseInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPExpenseInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPExpenseInformationDto> GetExpense(Guid expenseId)
	{
		ERPExpenseInformationDto eRPExpenseInformationDto = new ERPExpenseInformationDto();
		InitializeParameterLists();
		string[] collection = new string[6] { "lmxExpenseID", "lmxCreatedBy", "lmxCreatedDate", "lmxDescription", "lmxUniqueID", "lmxRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("lmxUniqueID|C", expenseId);
		AddCustomFieldsToSelectList("Expenses");
		using (DataTable dataTable = GetAsDataTable("Expenses", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPExpenseInformationDto);
			}
			eRPExpenseInformationDto.lmxExpenseID = dataTable.Rows[0].Field<string>("lmxExpenseID");
			eRPExpenseInformationDto.lmxCreatedBy = dataTable.Rows[0].Field<string>("lmxCreatedBy");
			eRPExpenseInformationDto.lmxCreatedDate = dataTable.Rows[0].Field<DateTime?>("lmxCreatedDate");
			eRPExpenseInformationDto.lmxDescription = dataTable.Rows[0].Field<string>("lmxDescription");
			eRPExpenseInformationDto.lmxUniqueID = dataTable.Rows[0].Field<Guid>("lmxUniqueID");
			eRPExpenseInformationDto.lmxRowVersion = dataTable.Rows[0].Field<byte[]>("lmxRowVersion");
			eRPExpenseInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPExpenseInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPExpenseInformationDto);
	}

	public Task<APIValidationInfoDto> SaveExpense(ERPExpenseDto expense)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Expenses WHERE lmxUniqueID = " + M1Util.ConvertToLinq(expense.lmxUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["lmxExpenseID"] = expense.lmxExpenseID.ToUpper();
				expense.lmxUniqueID = ((expense.lmxUniqueID == Guid.Empty) ? Guid.NewGuid() : expense.lmxUniqueID);
				dataRow["lmxUniqueID"] = expense.lmxUniqueID;
				dataRow["lmxCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["lmxCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Expense could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (expense.lmxRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Expense is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["lmxRowVersion"], expense.lmxRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Expense has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Expense again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["lmxDescription"] = expense.lmxDescription;
			if (expense.CustomFields != null && expense.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in expense.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Expense [{expense.lmxUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Expense [{expense.lmxUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
