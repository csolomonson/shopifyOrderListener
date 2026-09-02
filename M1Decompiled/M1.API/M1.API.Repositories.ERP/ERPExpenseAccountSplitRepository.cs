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

public class ERPExpenseAccountSplitRepository : APIBaseRepository, IERPExpenseAccountSplitRepository, IAPIBaseRepository, IDisposable
{
	public ERPExpenseAccountSplitRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesExpenseAccountSplitExist(Guid expenseAccountSplitId)
	{
		InitializeParameterLists();
		base.filterList.Add("xazUniqueID|C", expenseAccountSplitId);
		base.selectList.Add("xazUniqueID");
		return Task.FromResult(GetAsObject("ExpenseAccountSplits", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPExpenseAccountSplitInformationDto>> GetAllExpenseAccountSplits(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPExpenseAccountSplitInformationDto> collection = new List<ERPExpenseAccountSplitInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"xazExpenseAccountSplitID", "xazCreatedBy", "xazCreatedDate", "xazExpenseGlAccountID", "xazLandedCostCategoryID", "xazPartID", "xazPartRevisionID", "xazPercent", "xazRowVersion", "xazSequence",
			"xazSupplierOrganizationID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ExpenseAccountSplits");
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
		using (DataTable dataTable = GetAsDataTable("ExpenseAccountSplits", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPExpenseAccountSplitInformationDto eRPExpenseAccountSplitInformationDto = new ERPExpenseAccountSplitInformationDto();
				eRPExpenseAccountSplitInformationDto.xazExpenseAccountSplitID = dataTable.Rows[i].Field<Guid>("xazExpenseAccountSplitID");
				eRPExpenseAccountSplitInformationDto.xazCreatedBy = dataTable.Rows[i].Field<string>("xazCreatedBy");
				eRPExpenseAccountSplitInformationDto.xazCreatedDate = dataTable.Rows[i].Field<DateTime?>("xazCreatedDate");
				eRPExpenseAccountSplitInformationDto.xazExpenseGlAccountID = dataTable.Rows[i].Field<string>("xazExpenseGlAccountID");
				eRPExpenseAccountSplitInformationDto.xazLandedCostCategoryID = dataTable.Rows[i].Field<string>("xazLandedCostCategoryID");
				eRPExpenseAccountSplitInformationDto.xazPartID = dataTable.Rows[i].Field<string>("xazPartID");
				eRPExpenseAccountSplitInformationDto.xazPartRevisionID = dataTable.Rows[i].Field<string>("xazPartRevisionID");
				eRPExpenseAccountSplitInformationDto.xazPercent = dataTable.Rows[i].Field<decimal>("xazPercent");
				eRPExpenseAccountSplitInformationDto.xazRowVersion = dataTable.Rows[i].Field<byte[]>("xazRowVersion");
				eRPExpenseAccountSplitInformationDto.xazSequence = dataTable.Rows[i].Field<short>("xazSequence");
				eRPExpenseAccountSplitInformationDto.xazSupplierOrganizationID = dataTable.Rows[i].Field<string>("xazSupplierOrganizationID");
				eRPExpenseAccountSplitInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPExpenseAccountSplitInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPExpenseAccountSplitInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPExpenseAccountSplitInformationDto> GetExpenseAccountSplit(Guid expenseAccountSplitId)
	{
		ERPExpenseAccountSplitInformationDto eRPExpenseAccountSplitInformationDto = new ERPExpenseAccountSplitInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"xazExpenseAccountSplitID", "xazCreatedBy", "xazCreatedDate", "xazExpenseGlAccountID", "xazLandedCostCategoryID", "xazPartID", "xazPartRevisionID", "xazPercent", "xazRowVersion", "xazSequence",
			"xazSupplierOrganizationID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xazUniqueID|C", expenseAccountSplitId);
		AddCustomFieldsToSelectList("ExpenseAccountSplits");
		using (DataTable dataTable = GetAsDataTable("ExpenseAccountSplits", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPExpenseAccountSplitInformationDto);
			}
			eRPExpenseAccountSplitInformationDto.xazExpenseAccountSplitID = dataTable.Rows[0].Field<Guid>("xazExpenseAccountSplitID");
			eRPExpenseAccountSplitInformationDto.xazCreatedBy = dataTable.Rows[0].Field<string>("xazCreatedBy");
			eRPExpenseAccountSplitInformationDto.xazCreatedDate = dataTable.Rows[0].Field<DateTime?>("xazCreatedDate");
			eRPExpenseAccountSplitInformationDto.xazExpenseGlAccountID = dataTable.Rows[0].Field<string>("xazExpenseGlAccountID");
			eRPExpenseAccountSplitInformationDto.xazLandedCostCategoryID = dataTable.Rows[0].Field<string>("xazLandedCostCategoryID");
			eRPExpenseAccountSplitInformationDto.xazPartID = dataTable.Rows[0].Field<string>("xazPartID");
			eRPExpenseAccountSplitInformationDto.xazPartRevisionID = dataTable.Rows[0].Field<string>("xazPartRevisionID");
			eRPExpenseAccountSplitInformationDto.xazPercent = dataTable.Rows[0].Field<decimal>("xazPercent");
			eRPExpenseAccountSplitInformationDto.xazRowVersion = dataTable.Rows[0].Field<byte[]>("xazRowVersion");
			eRPExpenseAccountSplitInformationDto.xazSequence = dataTable.Rows[0].Field<short>("xazSequence");
			eRPExpenseAccountSplitInformationDto.xazSupplierOrganizationID = dataTable.Rows[0].Field<string>("xazSupplierOrganizationID");
			eRPExpenseAccountSplitInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPExpenseAccountSplitInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPExpenseAccountSplitInformationDto);
	}

	public Task<APIValidationInfoDto> SaveExpenseAccountSplit(ERPExpenseAccountSplitDto expenseAccountSplit)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ExpenseAccountSplits WHERE xazUniqueID = " + M1Util.ConvertToLinq(expenseAccountSplit.xazExpenseAccountSplitID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xazExpenseAccountSplitID"] = expenseAccountSplit.xazExpenseAccountSplitID;
				dataRow["xazCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xazCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ExpenseAccountSplit could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (expenseAccountSplit.xazRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ExpenseAccountSplit is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xazRowVersion"], expenseAccountSplit.xazRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ExpenseAccountSplit has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ExpenseAccountSplit again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xazExpenseGlAccountID"] = expenseAccountSplit.xazExpenseGlAccountID;
			dataRow["xazLandedCostCategoryID"] = expenseAccountSplit.xazLandedCostCategoryID;
			dataRow["xazPartID"] = expenseAccountSplit.xazPartID;
			dataRow["xazPartRevisionID"] = expenseAccountSplit.xazPartRevisionID;
			dataRow["xazPercent"] = expenseAccountSplit.xazPercent;
			dataRow["xazSequence"] = expenseAccountSplit.xazSequence;
			dataRow["xazSupplierOrganizationID"] = expenseAccountSplit.xazSupplierOrganizationID;
			if (expenseAccountSplit.CustomFields != null && expenseAccountSplit.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in expenseAccountSplit.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ExpenseAccountSplit [{expenseAccountSplit.xazExpenseAccountSplitID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ExpenseAccountSplit [{expenseAccountSplit.xazExpenseAccountSplitID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
