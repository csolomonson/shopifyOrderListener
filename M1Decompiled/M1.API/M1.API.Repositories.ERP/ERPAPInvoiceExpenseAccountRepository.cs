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

public class ERPAPInvoiceExpenseAccountRepository : APIBaseRepository, IERPAPInvoiceExpenseAccountRepository, IAPIBaseRepository, IDisposable
{
	public ERPAPInvoiceExpenseAccountRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAPInvoiceExpenseAccountExist(Guid aPInvoiceExpenseAccountId)
	{
		InitializeParameterLists();
		base.filterList.Add("apxUniqueID|C", aPInvoiceExpenseAccountId);
		base.selectList.Add("apxUniqueID");
		return Task.FromResult(GetAsObject("APInvoiceExpenseAccounts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAPInvoiceExpenseAccountInformationDto>> GetAllAPInvoiceExpenseAccounts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAPInvoiceExpenseAccountInformationDto> collection = new List<ERPAPInvoiceExpenseAccountInformationDto>();
		InitializeParameterLists();
		string[] array = new string[13]
		{
			"apxAmount", "apxApInvoiceID", "apxApInvoiceLineID", "apxCreatedBy", "apxCreatedDate", "apxUniqueID", "apxExpenseGlAccountID", "apxPostedToGl", "apxPercent", "apxRowVersion",
			"apxApInvoiceExpenseAccountID", "apxSourceTableName", "apxSourceTableUniqueID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("APInvoiceExpenseAccounts");
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
		using (DataTable dataTable = GetAsDataTable("APInvoiceExpenseAccounts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAPInvoiceExpenseAccountInformationDto eRPAPInvoiceExpenseAccountInformationDto = new ERPAPInvoiceExpenseAccountInformationDto();
				eRPAPInvoiceExpenseAccountInformationDto.apxAmount = dataTable.Rows[i].Field<decimal>("apxAmount");
				eRPAPInvoiceExpenseAccountInformationDto.apxApInvoiceID = dataTable.Rows[i].Field<string>("apxApInvoiceID");
				eRPAPInvoiceExpenseAccountInformationDto.apxApInvoiceLineID = dataTable.Rows[i].Field<short>("apxApInvoiceLineID");
				eRPAPInvoiceExpenseAccountInformationDto.apxCreatedBy = dataTable.Rows[i].Field<string>("apxCreatedBy");
				eRPAPInvoiceExpenseAccountInformationDto.apxCreatedDate = dataTable.Rows[i].Field<DateTime?>("apxCreatedDate");
				eRPAPInvoiceExpenseAccountInformationDto.apxUniqueID = dataTable.Rows[i].Field<Guid>("apxUniqueID");
				eRPAPInvoiceExpenseAccountInformationDto.apxExpenseGlAccountID = dataTable.Rows[i].Field<string>("apxExpenseGlAccountID");
				eRPAPInvoiceExpenseAccountInformationDto.apxPostedToGl = dataTable.Rows[i].Field<bool>("apxPostedToGl");
				eRPAPInvoiceExpenseAccountInformationDto.apxPercent = dataTable.Rows[i].Field<decimal>("apxPercent");
				eRPAPInvoiceExpenseAccountInformationDto.apxRowVersion = dataTable.Rows[i].Field<byte[]>("apxRowVersion");
				eRPAPInvoiceExpenseAccountInformationDto.apxApInvoiceExpenseAccountID = dataTable.Rows[i].Field<short>("apxApInvoiceExpenseAccountID");
				eRPAPInvoiceExpenseAccountInformationDto.apxSourceTableName = dataTable.Rows[i].Field<string>("apxSourceTableName");
				eRPAPInvoiceExpenseAccountInformationDto.apxSourceTableUniqueID = dataTable.Rows[i].Field<Guid>("apxSourceTableUniqueID");
				eRPAPInvoiceExpenseAccountInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAPInvoiceExpenseAccountInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAPInvoiceExpenseAccountInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAPInvoiceExpenseAccountInformationDto> GetAPInvoiceExpenseAccount(Guid aPInvoiceExpenseAccountId)
	{
		ERPAPInvoiceExpenseAccountInformationDto eRPAPInvoiceExpenseAccountInformationDto = new ERPAPInvoiceExpenseAccountInformationDto();
		InitializeParameterLists();
		string[] collection = new string[13]
		{
			"apxAmount", "apxApInvoiceID", "apxApInvoiceLineID", "apxCreatedBy", "apxCreatedDate", "apxUniqueID", "apxExpenseGlAccountID", "apxPostedToGl", "apxPercent", "apxRowVersion",
			"apxApInvoiceExpenseAccountID", "apxSourceTableName", "apxSourceTableUniqueID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("apxUniqueID|C", aPInvoiceExpenseAccountId);
		AddCustomFieldsToSelectList("APInvoiceExpenseAccounts");
		using (DataTable dataTable = GetAsDataTable("APInvoiceExpenseAccounts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAPInvoiceExpenseAccountInformationDto);
			}
			eRPAPInvoiceExpenseAccountInformationDto.apxAmount = dataTable.Rows[0].Field<decimal>("apxAmount");
			eRPAPInvoiceExpenseAccountInformationDto.apxApInvoiceID = dataTable.Rows[0].Field<string>("apxApInvoiceID");
			eRPAPInvoiceExpenseAccountInformationDto.apxApInvoiceLineID = dataTable.Rows[0].Field<short>("apxApInvoiceLineID");
			eRPAPInvoiceExpenseAccountInformationDto.apxCreatedBy = dataTable.Rows[0].Field<string>("apxCreatedBy");
			eRPAPInvoiceExpenseAccountInformationDto.apxCreatedDate = dataTable.Rows[0].Field<DateTime?>("apxCreatedDate");
			eRPAPInvoiceExpenseAccountInformationDto.apxUniqueID = dataTable.Rows[0].Field<Guid>("apxUniqueID");
			eRPAPInvoiceExpenseAccountInformationDto.apxExpenseGlAccountID = dataTable.Rows[0].Field<string>("apxExpenseGlAccountID");
			eRPAPInvoiceExpenseAccountInformationDto.apxPostedToGl = dataTable.Rows[0].Field<bool>("apxPostedToGl");
			eRPAPInvoiceExpenseAccountInformationDto.apxPercent = dataTable.Rows[0].Field<decimal>("apxPercent");
			eRPAPInvoiceExpenseAccountInformationDto.apxRowVersion = dataTable.Rows[0].Field<byte[]>("apxRowVersion");
			eRPAPInvoiceExpenseAccountInformationDto.apxApInvoiceExpenseAccountID = dataTable.Rows[0].Field<short>("apxApInvoiceExpenseAccountID");
			eRPAPInvoiceExpenseAccountInformationDto.apxSourceTableName = dataTable.Rows[0].Field<string>("apxSourceTableName");
			eRPAPInvoiceExpenseAccountInformationDto.apxSourceTableUniqueID = dataTable.Rows[0].Field<Guid>("apxSourceTableUniqueID");
			eRPAPInvoiceExpenseAccountInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAPInvoiceExpenseAccountInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAPInvoiceExpenseAccountInformationDto);
	}

	public Task<APIValidationInfoDto> SaveAPInvoiceExpenseAccount(ERPAPInvoiceExpenseAccountDto aPInvoiceExpenseAccount)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM APInvoiceExpenseAccounts WHERE apxUniqueID = " + M1Util.ConvertToLinq(aPInvoiceExpenseAccount.apxUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["apxApInvoiceID"] = aPInvoiceExpenseAccount.apxApInvoiceID.ToUpper();
				dataRow["apxApInvoiceLineID"] = aPInvoiceExpenseAccount.apxApInvoiceLineID;
				dataRow["apxApInvoiceExpenseAccountID"] = aPInvoiceExpenseAccount.apxApInvoiceExpenseAccountID;
				aPInvoiceExpenseAccount.apxUniqueID = ((aPInvoiceExpenseAccount.apxUniqueID == Guid.Empty) ? Guid.NewGuid() : aPInvoiceExpenseAccount.apxUniqueID);
				dataRow["apxUniqueID"] = aPInvoiceExpenseAccount.apxUniqueID;
				dataRow["apxCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["apxCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The APInvoiceExpenseAccount could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (aPInvoiceExpenseAccount.apxRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the APInvoiceExpenseAccount is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["apxRowVersion"], aPInvoiceExpenseAccount.apxRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the APInvoiceExpenseAccount has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the APInvoiceExpenseAccount again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["apxAmount"] = aPInvoiceExpenseAccount.apxAmount;
			dataRow["apxExpenseGlAccountID"] = aPInvoiceExpenseAccount.apxExpenseGlAccountID;
			dataRow["apxPostedToGl"] = aPInvoiceExpenseAccount.apxPostedToGl;
			dataRow["apxPercent"] = aPInvoiceExpenseAccount.apxPercent;
			dataRow["apxSourceTableName"] = aPInvoiceExpenseAccount.apxSourceTableName;
			dataRow["apxSourceTableUniqueID"] = aPInvoiceExpenseAccount.apxSourceTableUniqueID;
			if (aPInvoiceExpenseAccount.CustomFields != null && aPInvoiceExpenseAccount.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in aPInvoiceExpenseAccount.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the APInvoiceExpenseAccount [{aPInvoiceExpenseAccount.apxUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the APInvoiceExpenseAccount [{aPInvoiceExpenseAccount.apxUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
