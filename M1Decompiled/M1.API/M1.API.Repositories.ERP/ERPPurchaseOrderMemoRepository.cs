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

public class ERPPurchaseOrderMemoRepository : APIBaseRepository, IERPPurchaseOrderMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPPurchaseOrderMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPurchaseOrderMemoExist(Guid purchaseOrderMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("pmkUniqueID|C", purchaseOrderMemoId);
		base.selectList.Add("pmkUniqueID");
		return Task.FromResult(GetAsObject("PurchaseOrderMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPurchaseOrderMemoInformationDto>> GetAllPurchaseOrderMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPurchaseOrderMemoInformationDto> collection = new List<ERPPurchaseOrderMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[14]
		{
			"pmkCreatedBy", "pmkCreatedDate", "pmkUniqueID", "pmkClosed", "pmkLongDescriptionRtf", "pmkLongDescriptionText", "pmkMemoDate", "pmkPurchaseOrderID", "pmkRowVersion", "pmkPurchaseOrderMemoID",
			"pmkShortDescription", "pmkShowInApInvoices", "pmkShowInPurchaseOrders", "pmkShowInReceipts"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PurchaseOrderMemos");
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
		using (DataTable dataTable = GetAsDataTable("PurchaseOrderMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPurchaseOrderMemoInformationDto eRPPurchaseOrderMemoInformationDto = new ERPPurchaseOrderMemoInformationDto();
				eRPPurchaseOrderMemoInformationDto.pmkCreatedBy = dataTable.Rows[i].Field<string>("pmkCreatedBy");
				eRPPurchaseOrderMemoInformationDto.pmkCreatedDate = dataTable.Rows[i].Field<DateTime?>("pmkCreatedDate");
				eRPPurchaseOrderMemoInformationDto.pmkUniqueID = dataTable.Rows[i].Field<Guid>("pmkUniqueID");
				eRPPurchaseOrderMemoInformationDto.pmkClosed = dataTable.Rows[i].Field<bool>("pmkClosed");
				eRPPurchaseOrderMemoInformationDto.pmkLongDescriptionRtf = dataTable.Rows[i].Field<string>("pmkLongDescriptionRtf");
				eRPPurchaseOrderMemoInformationDto.pmkLongDescriptionText = dataTable.Rows[i].Field<string>("pmkLongDescriptionText");
				eRPPurchaseOrderMemoInformationDto.pmkMemoDate = dataTable.Rows[i].Field<DateTime?>("pmkMemoDate");
				eRPPurchaseOrderMemoInformationDto.pmkPurchaseOrderID = dataTable.Rows[i].Field<string>("pmkPurchaseOrderID");
				eRPPurchaseOrderMemoInformationDto.pmkRowVersion = dataTable.Rows[i].Field<byte[]>("pmkRowVersion");
				eRPPurchaseOrderMemoInformationDto.pmkPurchaseOrderMemoID = dataTable.Rows[i].Field<short>("pmkPurchaseOrderMemoID");
				eRPPurchaseOrderMemoInformationDto.pmkShortDescription = dataTable.Rows[i].Field<string>("pmkShortDescription");
				eRPPurchaseOrderMemoInformationDto.pmkShowInApInvoices = dataTable.Rows[i].Field<bool>("pmkShowInApInvoices");
				eRPPurchaseOrderMemoInformationDto.pmkShowInPurchaseOrders = dataTable.Rows[i].Field<bool>("pmkShowInPurchaseOrders");
				eRPPurchaseOrderMemoInformationDto.pmkShowInReceipts = dataTable.Rows[i].Field<bool>("pmkShowInReceipts");
				eRPPurchaseOrderMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPurchaseOrderMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPurchaseOrderMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPurchaseOrderMemoInformationDto> GetPurchaseOrderMemo(Guid purchaseOrderMemoId)
	{
		ERPPurchaseOrderMemoInformationDto eRPPurchaseOrderMemoInformationDto = new ERPPurchaseOrderMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[14]
		{
			"pmkCreatedBy", "pmkCreatedDate", "pmkUniqueID", "pmkClosed", "pmkLongDescriptionRtf", "pmkLongDescriptionText", "pmkMemoDate", "pmkPurchaseOrderID", "pmkRowVersion", "pmkPurchaseOrderMemoID",
			"pmkShortDescription", "pmkShowInApInvoices", "pmkShowInPurchaseOrders", "pmkShowInReceipts"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("pmkUniqueID|C", purchaseOrderMemoId);
		AddCustomFieldsToSelectList("PurchaseOrderMemos");
		using (DataTable dataTable = GetAsDataTable("PurchaseOrderMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPurchaseOrderMemoInformationDto);
			}
			eRPPurchaseOrderMemoInformationDto.pmkCreatedBy = dataTable.Rows[0].Field<string>("pmkCreatedBy");
			eRPPurchaseOrderMemoInformationDto.pmkCreatedDate = dataTable.Rows[0].Field<DateTime?>("pmkCreatedDate");
			eRPPurchaseOrderMemoInformationDto.pmkUniqueID = dataTable.Rows[0].Field<Guid>("pmkUniqueID");
			eRPPurchaseOrderMemoInformationDto.pmkClosed = dataTable.Rows[0].Field<bool>("pmkClosed");
			eRPPurchaseOrderMemoInformationDto.pmkLongDescriptionRtf = dataTable.Rows[0].Field<string>("pmkLongDescriptionRtf");
			eRPPurchaseOrderMemoInformationDto.pmkLongDescriptionText = dataTable.Rows[0].Field<string>("pmkLongDescriptionText");
			eRPPurchaseOrderMemoInformationDto.pmkMemoDate = dataTable.Rows[0].Field<DateTime?>("pmkMemoDate");
			eRPPurchaseOrderMemoInformationDto.pmkPurchaseOrderID = dataTable.Rows[0].Field<string>("pmkPurchaseOrderID");
			eRPPurchaseOrderMemoInformationDto.pmkRowVersion = dataTable.Rows[0].Field<byte[]>("pmkRowVersion");
			eRPPurchaseOrderMemoInformationDto.pmkPurchaseOrderMemoID = dataTable.Rows[0].Field<short>("pmkPurchaseOrderMemoID");
			eRPPurchaseOrderMemoInformationDto.pmkShortDescription = dataTable.Rows[0].Field<string>("pmkShortDescription");
			eRPPurchaseOrderMemoInformationDto.pmkShowInApInvoices = dataTable.Rows[0].Field<bool>("pmkShowInApInvoices");
			eRPPurchaseOrderMemoInformationDto.pmkShowInPurchaseOrders = dataTable.Rows[0].Field<bool>("pmkShowInPurchaseOrders");
			eRPPurchaseOrderMemoInformationDto.pmkShowInReceipts = dataTable.Rows[0].Field<bool>("pmkShowInReceipts");
			eRPPurchaseOrderMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPurchaseOrderMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPurchaseOrderMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SavePurchaseOrderMemo(ERPPurchaseOrderMemoDto purchaseOrderMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PurchaseOrderMemos WHERE pmkUniqueID = " + M1Util.ConvertToLinq(purchaseOrderMemo.pmkUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["pmkPurchaseOrderID"] = purchaseOrderMemo.pmkPurchaseOrderID.ToUpper();
				dataRow["pmkPurchaseOrderMemoID"] = purchaseOrderMemo.pmkPurchaseOrderMemoID;
				purchaseOrderMemo.pmkUniqueID = ((purchaseOrderMemo.pmkUniqueID == Guid.Empty) ? Guid.NewGuid() : purchaseOrderMemo.pmkUniqueID);
				dataRow["pmkUniqueID"] = purchaseOrderMemo.pmkUniqueID;
				dataRow["pmkCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["pmkCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PurchaseOrderMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (purchaseOrderMemo.pmkRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PurchaseOrderMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["pmkRowVersion"], purchaseOrderMemo.pmkRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PurchaseOrderMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PurchaseOrderMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["pmkClosed"] = purchaseOrderMemo.pmkClosed;
			dataRow["pmkLongDescriptionRtf"] = purchaseOrderMemo.pmkLongDescriptionRtf ?? dataRow["pmkLongDescriptionRtf"];
			dataRow["pmkLongDescriptionText"] = purchaseOrderMemo.pmkLongDescriptionText ?? dataRow["pmkLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? pmkMemoDate = purchaseOrderMemo.pmkMemoDate;
			dataRow2["pmkMemoDate"] = (pmkMemoDate.HasValue ? ((object)pmkMemoDate.GetValueOrDefault()) : dataRow["pmkMemoDate"]);
			dataRow["pmkShortDescription"] = purchaseOrderMemo.pmkShortDescription;
			dataRow["pmkShowInApInvoices"] = purchaseOrderMemo.pmkShowInApInvoices;
			dataRow["pmkShowInPurchaseOrders"] = purchaseOrderMemo.pmkShowInPurchaseOrders;
			dataRow["pmkShowInReceipts"] = purchaseOrderMemo.pmkShowInReceipts;
			if (purchaseOrderMemo.CustomFields != null && purchaseOrderMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in purchaseOrderMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PurchaseOrderMemo [{purchaseOrderMemo.pmkUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PurchaseOrderMemo [{purchaseOrderMemo.pmkUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
