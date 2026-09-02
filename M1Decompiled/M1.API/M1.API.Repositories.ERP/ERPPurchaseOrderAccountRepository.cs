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

public class ERPPurchaseOrderAccountRepository : APIBaseRepository, IERPPurchaseOrderAccountRepository, IAPIBaseRepository, IDisposable
{
	public ERPPurchaseOrderAccountRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPurchaseOrderAccountExist(Guid purchaseOrderAccountId)
	{
		InitializeParameterLists();
		base.filterList.Add("pmxUniqueID|C", purchaseOrderAccountId);
		base.selectList.Add("pmxUniqueID");
		return Task.FromResult(GetAsObject("PurchaseOrderAccounts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPurchaseOrderAccountInformationDto>> GetAllPurchaseOrderAccounts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPurchaseOrderAccountInformationDto> collection = new List<ERPPurchaseOrderAccountInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"pmxAmount", "pmxCreatedBy", "pmxCreatedDate", "pmxUniqueID", "pmxExpenseGlAccountID", "pmxClosed", "pmxPercent", "pmxPurchaseOrderID", "pmxPurchaseOrderLineID", "pmxRowVersion",
			"pmxPurchaseOrderAccountID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PurchaseOrderAccounts");
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
		using (DataTable dataTable = GetAsDataTable("PurchaseOrderAccounts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPurchaseOrderAccountInformationDto eRPPurchaseOrderAccountInformationDto = new ERPPurchaseOrderAccountInformationDto();
				eRPPurchaseOrderAccountInformationDto.pmxAmount = dataTable.Rows[i].Field<decimal>("pmxAmount");
				eRPPurchaseOrderAccountInformationDto.pmxCreatedBy = dataTable.Rows[i].Field<string>("pmxCreatedBy");
				eRPPurchaseOrderAccountInformationDto.pmxCreatedDate = dataTable.Rows[i].Field<DateTime?>("pmxCreatedDate");
				eRPPurchaseOrderAccountInformationDto.pmxUniqueID = dataTable.Rows[i].Field<Guid>("pmxUniqueID");
				eRPPurchaseOrderAccountInformationDto.pmxExpenseGlAccountID = dataTable.Rows[i].Field<string>("pmxExpenseGlAccountID");
				eRPPurchaseOrderAccountInformationDto.pmxClosed = dataTable.Rows[i].Field<bool>("pmxClosed");
				eRPPurchaseOrderAccountInformationDto.pmxPercent = dataTable.Rows[i].Field<decimal>("pmxPercent");
				eRPPurchaseOrderAccountInformationDto.pmxPurchaseOrderID = dataTable.Rows[i].Field<string>("pmxPurchaseOrderID");
				eRPPurchaseOrderAccountInformationDto.pmxPurchaseOrderLineID = dataTable.Rows[i].Field<short>("pmxPurchaseOrderLineID");
				eRPPurchaseOrderAccountInformationDto.pmxRowVersion = dataTable.Rows[i].Field<byte[]>("pmxRowVersion");
				eRPPurchaseOrderAccountInformationDto.pmxPurchaseOrderAccountID = dataTable.Rows[i].Field<short>("pmxPurchaseOrderAccountID");
				eRPPurchaseOrderAccountInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPurchaseOrderAccountInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPurchaseOrderAccountInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPurchaseOrderAccountInformationDto> GetPurchaseOrderAccount(Guid purchaseOrderAccountId)
	{
		ERPPurchaseOrderAccountInformationDto eRPPurchaseOrderAccountInformationDto = new ERPPurchaseOrderAccountInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"pmxAmount", "pmxCreatedBy", "pmxCreatedDate", "pmxUniqueID", "pmxExpenseGlAccountID", "pmxClosed", "pmxPercent", "pmxPurchaseOrderID", "pmxPurchaseOrderLineID", "pmxRowVersion",
			"pmxPurchaseOrderAccountID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("pmxUniqueID|C", purchaseOrderAccountId);
		AddCustomFieldsToSelectList("PurchaseOrderAccounts");
		using (DataTable dataTable = GetAsDataTable("PurchaseOrderAccounts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPurchaseOrderAccountInformationDto);
			}
			eRPPurchaseOrderAccountInformationDto.pmxAmount = dataTable.Rows[0].Field<decimal>("pmxAmount");
			eRPPurchaseOrderAccountInformationDto.pmxCreatedBy = dataTable.Rows[0].Field<string>("pmxCreatedBy");
			eRPPurchaseOrderAccountInformationDto.pmxCreatedDate = dataTable.Rows[0].Field<DateTime?>("pmxCreatedDate");
			eRPPurchaseOrderAccountInformationDto.pmxUniqueID = dataTable.Rows[0].Field<Guid>("pmxUniqueID");
			eRPPurchaseOrderAccountInformationDto.pmxExpenseGlAccountID = dataTable.Rows[0].Field<string>("pmxExpenseGlAccountID");
			eRPPurchaseOrderAccountInformationDto.pmxClosed = dataTable.Rows[0].Field<bool>("pmxClosed");
			eRPPurchaseOrderAccountInformationDto.pmxPercent = dataTable.Rows[0].Field<decimal>("pmxPercent");
			eRPPurchaseOrderAccountInformationDto.pmxPurchaseOrderID = dataTable.Rows[0].Field<string>("pmxPurchaseOrderID");
			eRPPurchaseOrderAccountInformationDto.pmxPurchaseOrderLineID = dataTable.Rows[0].Field<short>("pmxPurchaseOrderLineID");
			eRPPurchaseOrderAccountInformationDto.pmxRowVersion = dataTable.Rows[0].Field<byte[]>("pmxRowVersion");
			eRPPurchaseOrderAccountInformationDto.pmxPurchaseOrderAccountID = dataTable.Rows[0].Field<short>("pmxPurchaseOrderAccountID");
			eRPPurchaseOrderAccountInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPurchaseOrderAccountInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPurchaseOrderAccountInformationDto);
	}

	public Task<APIValidationInfoDto> SavePurchaseOrderAccount(ERPPurchaseOrderAccountDto purchaseOrderAccount)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PurchaseOrderAccounts WHERE pmxUniqueID = " + M1Util.ConvertToLinq(purchaseOrderAccount.pmxUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["pmxPurchaseOrderID"] = purchaseOrderAccount.pmxPurchaseOrderID.ToUpper();
				dataRow["pmxPurchaseOrderLineID"] = purchaseOrderAccount.pmxPurchaseOrderLineID;
				dataRow["pmxPurchaseOrderAccountID"] = purchaseOrderAccount.pmxPurchaseOrderAccountID;
				purchaseOrderAccount.pmxUniqueID = ((purchaseOrderAccount.pmxUniqueID == Guid.Empty) ? Guid.NewGuid() : purchaseOrderAccount.pmxUniqueID);
				dataRow["pmxUniqueID"] = purchaseOrderAccount.pmxUniqueID;
				dataRow["pmxCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["pmxCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PurchaseOrderAccount could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (purchaseOrderAccount.pmxRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PurchaseOrderAccount is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["pmxRowVersion"], purchaseOrderAccount.pmxRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PurchaseOrderAccount has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PurchaseOrderAccount again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["pmxAmount"] = purchaseOrderAccount.pmxAmount;
			dataRow["pmxExpenseGlAccountID"] = purchaseOrderAccount.pmxExpenseGlAccountID;
			dataRow["pmxClosed"] = purchaseOrderAccount.pmxClosed;
			dataRow["pmxPercent"] = purchaseOrderAccount.pmxPercent;
			if (purchaseOrderAccount.CustomFields != null && purchaseOrderAccount.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in purchaseOrderAccount.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PurchaseOrderAccount [{purchaseOrderAccount.pmxUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PurchaseOrderAccount [{purchaseOrderAccount.pmxUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
