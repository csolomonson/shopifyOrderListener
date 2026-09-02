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

public class ERPOrganizationMemoRepository : APIBaseRepository, IERPOrganizationMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPOrganizationMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesOrganizationMemoExist(Guid organizationMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmmUniqueID|C", organizationMemoId);
		base.selectList.Add("cmmUniqueID");
		return Task.FromResult(GetAsObject("OrganizationMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPOrganizationMemoInformationDto>> GetAllOrganizationMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPOrganizationMemoInformationDto> collection = new List<ERPOrganizationMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[30]
		{
			"cmmContactID", "cmmCreatedBy", "cmmCreatedDate", "cmmUniqueID", "cmmLocationID", "cmmLongDescriptionRtf", "cmmLongDescriptionText", "cmmMemoDate", "cmmOrganizationID", "cmmRowVersion",
			"cmmOrganizationMemoID", "cmmShortDescription", "cmmShowInApInvoices", "cmmShowInApPayments", "cmmShowInArInvoices", "cmmShowInArPayments", "cmmShowInCalls", "cmmShowInDmrClaims", "cmmShowInDmrShipments", "cmmShowInLeads",
			"cmmShowInOrganizations", "cmmShowInPriceAndAvailability", "cmmShowInPurchaseOrders", "cmmShowInQuotes", "cmmShowInReceipts", "cmmShowInRfqs", "cmmShowInRmaClaims", "cmmShowInRmaReceipts", "cmmShowInSalesOrders", "cmmShowInShipments"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("OrganizationMemos");
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
		using (DataTable dataTable = GetAsDataTable("OrganizationMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPOrganizationMemoInformationDto eRPOrganizationMemoInformationDto = new ERPOrganizationMemoInformationDto();
				eRPOrganizationMemoInformationDto.cmmContactID = dataTable.Rows[i].Field<string>("cmmContactID");
				eRPOrganizationMemoInformationDto.cmmCreatedBy = dataTable.Rows[i].Field<string>("cmmCreatedBy");
				eRPOrganizationMemoInformationDto.cmmCreatedDate = dataTable.Rows[i].Field<DateTime?>("cmmCreatedDate");
				eRPOrganizationMemoInformationDto.cmmUniqueID = dataTable.Rows[i].Field<Guid>("cmmUniqueID");
				eRPOrganizationMemoInformationDto.cmmLocationID = dataTable.Rows[i].Field<string>("cmmLocationID");
				eRPOrganizationMemoInformationDto.cmmLongDescriptionRtf = dataTable.Rows[i].Field<string>("cmmLongDescriptionRtf");
				eRPOrganizationMemoInformationDto.cmmLongDescriptionText = dataTable.Rows[i].Field<string>("cmmLongDescriptionText");
				eRPOrganizationMemoInformationDto.cmmMemoDate = dataTable.Rows[i].Field<DateTime?>("cmmMemoDate");
				eRPOrganizationMemoInformationDto.cmmOrganizationID = dataTable.Rows[i].Field<string>("cmmOrganizationID");
				eRPOrganizationMemoInformationDto.cmmRowVersion = dataTable.Rows[i].Field<byte[]>("cmmRowVersion");
				eRPOrganizationMemoInformationDto.cmmOrganizationMemoID = dataTable.Rows[i].Field<short>("cmmOrganizationMemoID");
				eRPOrganizationMemoInformationDto.cmmShortDescription = dataTable.Rows[i].Field<string>("cmmShortDescription");
				eRPOrganizationMemoInformationDto.cmmShowInApInvoices = dataTable.Rows[i].Field<bool>("cmmShowInApInvoices");
				eRPOrganizationMemoInformationDto.cmmShowInApPayments = dataTable.Rows[i].Field<bool>("cmmShowInApPayments");
				eRPOrganizationMemoInformationDto.cmmShowInArInvoices = dataTable.Rows[i].Field<bool>("cmmShowInArInvoices");
				eRPOrganizationMemoInformationDto.cmmShowInArPayments = dataTable.Rows[i].Field<bool>("cmmShowInArPayments");
				eRPOrganizationMemoInformationDto.cmmShowInCalls = dataTable.Rows[i].Field<bool>("cmmShowInCalls");
				eRPOrganizationMemoInformationDto.cmmShowInDmrClaims = dataTable.Rows[i].Field<bool>("cmmShowInDmrClaims");
				eRPOrganizationMemoInformationDto.cmmShowInDmrShipments = dataTable.Rows[i].Field<bool>("cmmShowInDmrShipments");
				eRPOrganizationMemoInformationDto.cmmShowInLeads = dataTable.Rows[i].Field<bool>("cmmShowInLeads");
				eRPOrganizationMemoInformationDto.cmmShowInOrganizations = dataTable.Rows[i].Field<bool>("cmmShowInOrganizations");
				eRPOrganizationMemoInformationDto.cmmShowInPriceAndAvailability = dataTable.Rows[i].Field<bool>("cmmShowInPriceAndAvailability");
				eRPOrganizationMemoInformationDto.cmmShowInPurchaseOrders = dataTable.Rows[i].Field<bool>("cmmShowInPurchaseOrders");
				eRPOrganizationMemoInformationDto.cmmShowInQuotes = dataTable.Rows[i].Field<bool>("cmmShowInQuotes");
				eRPOrganizationMemoInformationDto.cmmShowInReceipts = dataTable.Rows[i].Field<bool>("cmmShowInReceipts");
				eRPOrganizationMemoInformationDto.cmmShowInRfqs = dataTable.Rows[i].Field<bool>("cmmShowInRfqs");
				eRPOrganizationMemoInformationDto.cmmShowInRmaClaims = dataTable.Rows[i].Field<bool>("cmmShowInRmaClaims");
				eRPOrganizationMemoInformationDto.cmmShowInRmaReceipts = dataTable.Rows[i].Field<bool>("cmmShowInRmaReceipts");
				eRPOrganizationMemoInformationDto.cmmShowInSalesOrders = dataTable.Rows[i].Field<bool>("cmmShowInSalesOrders");
				eRPOrganizationMemoInformationDto.cmmShowInShipments = dataTable.Rows[i].Field<bool>("cmmShowInShipments");
				eRPOrganizationMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPOrganizationMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPOrganizationMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPOrganizationMemoInformationDto> GetOrganizationMemo(Guid organizationMemoId)
	{
		ERPOrganizationMemoInformationDto eRPOrganizationMemoInformationDto = new ERPOrganizationMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[30]
		{
			"cmmContactID", "cmmCreatedBy", "cmmCreatedDate", "cmmUniqueID", "cmmLocationID", "cmmLongDescriptionRtf", "cmmLongDescriptionText", "cmmMemoDate", "cmmOrganizationID", "cmmRowVersion",
			"cmmOrganizationMemoID", "cmmShortDescription", "cmmShowInApInvoices", "cmmShowInApPayments", "cmmShowInArInvoices", "cmmShowInArPayments", "cmmShowInCalls", "cmmShowInDmrClaims", "cmmShowInDmrShipments", "cmmShowInLeads",
			"cmmShowInOrganizations", "cmmShowInPriceAndAvailability", "cmmShowInPurchaseOrders", "cmmShowInQuotes", "cmmShowInReceipts", "cmmShowInRfqs", "cmmShowInRmaClaims", "cmmShowInRmaReceipts", "cmmShowInSalesOrders", "cmmShowInShipments"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("cmmUniqueID|C", organizationMemoId);
		AddCustomFieldsToSelectList("OrganizationMemos");
		using (DataTable dataTable = GetAsDataTable("OrganizationMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPOrganizationMemoInformationDto);
			}
			eRPOrganizationMemoInformationDto.cmmContactID = dataTable.Rows[0].Field<string>("cmmContactID");
			eRPOrganizationMemoInformationDto.cmmCreatedBy = dataTable.Rows[0].Field<string>("cmmCreatedBy");
			eRPOrganizationMemoInformationDto.cmmCreatedDate = dataTable.Rows[0].Field<DateTime?>("cmmCreatedDate");
			eRPOrganizationMemoInformationDto.cmmUniqueID = dataTable.Rows[0].Field<Guid>("cmmUniqueID");
			eRPOrganizationMemoInformationDto.cmmLocationID = dataTable.Rows[0].Field<string>("cmmLocationID");
			eRPOrganizationMemoInformationDto.cmmLongDescriptionRtf = dataTable.Rows[0].Field<string>("cmmLongDescriptionRtf");
			eRPOrganizationMemoInformationDto.cmmLongDescriptionText = dataTable.Rows[0].Field<string>("cmmLongDescriptionText");
			eRPOrganizationMemoInformationDto.cmmMemoDate = dataTable.Rows[0].Field<DateTime?>("cmmMemoDate");
			eRPOrganizationMemoInformationDto.cmmOrganizationID = dataTable.Rows[0].Field<string>("cmmOrganizationID");
			eRPOrganizationMemoInformationDto.cmmRowVersion = dataTable.Rows[0].Field<byte[]>("cmmRowVersion");
			eRPOrganizationMemoInformationDto.cmmOrganizationMemoID = dataTable.Rows[0].Field<short>("cmmOrganizationMemoID");
			eRPOrganizationMemoInformationDto.cmmShortDescription = dataTable.Rows[0].Field<string>("cmmShortDescription");
			eRPOrganizationMemoInformationDto.cmmShowInApInvoices = dataTable.Rows[0].Field<bool>("cmmShowInApInvoices");
			eRPOrganizationMemoInformationDto.cmmShowInApPayments = dataTable.Rows[0].Field<bool>("cmmShowInApPayments");
			eRPOrganizationMemoInformationDto.cmmShowInArInvoices = dataTable.Rows[0].Field<bool>("cmmShowInArInvoices");
			eRPOrganizationMemoInformationDto.cmmShowInArPayments = dataTable.Rows[0].Field<bool>("cmmShowInArPayments");
			eRPOrganizationMemoInformationDto.cmmShowInCalls = dataTable.Rows[0].Field<bool>("cmmShowInCalls");
			eRPOrganizationMemoInformationDto.cmmShowInDmrClaims = dataTable.Rows[0].Field<bool>("cmmShowInDmrClaims");
			eRPOrganizationMemoInformationDto.cmmShowInDmrShipments = dataTable.Rows[0].Field<bool>("cmmShowInDmrShipments");
			eRPOrganizationMemoInformationDto.cmmShowInLeads = dataTable.Rows[0].Field<bool>("cmmShowInLeads");
			eRPOrganizationMemoInformationDto.cmmShowInOrganizations = dataTable.Rows[0].Field<bool>("cmmShowInOrganizations");
			eRPOrganizationMemoInformationDto.cmmShowInPriceAndAvailability = dataTable.Rows[0].Field<bool>("cmmShowInPriceAndAvailability");
			eRPOrganizationMemoInformationDto.cmmShowInPurchaseOrders = dataTable.Rows[0].Field<bool>("cmmShowInPurchaseOrders");
			eRPOrganizationMemoInformationDto.cmmShowInQuotes = dataTable.Rows[0].Field<bool>("cmmShowInQuotes");
			eRPOrganizationMemoInformationDto.cmmShowInReceipts = dataTable.Rows[0].Field<bool>("cmmShowInReceipts");
			eRPOrganizationMemoInformationDto.cmmShowInRfqs = dataTable.Rows[0].Field<bool>("cmmShowInRfqs");
			eRPOrganizationMemoInformationDto.cmmShowInRmaClaims = dataTable.Rows[0].Field<bool>("cmmShowInRmaClaims");
			eRPOrganizationMemoInformationDto.cmmShowInRmaReceipts = dataTable.Rows[0].Field<bool>("cmmShowInRmaReceipts");
			eRPOrganizationMemoInformationDto.cmmShowInSalesOrders = dataTable.Rows[0].Field<bool>("cmmShowInSalesOrders");
			eRPOrganizationMemoInformationDto.cmmShowInShipments = dataTable.Rows[0].Field<bool>("cmmShowInShipments");
			eRPOrganizationMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPOrganizationMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPOrganizationMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SaveOrganizationMemo(ERPOrganizationMemoDto organizationMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM OrganizationMemos WHERE cmmUniqueID = " + M1Util.ConvertToLinq(organizationMemo.cmmUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cmmOrganizationID"] = organizationMemo.cmmOrganizationID.ToUpper();
				dataRow["cmmLocationID"] = organizationMemo.cmmLocationID.ToUpper();
				dataRow["cmmContactID"] = organizationMemo.cmmContactID.ToUpper();
				dataRow["cmmOrganizationMemoID"] = organizationMemo.cmmOrganizationMemoID;
				organizationMemo.cmmUniqueID = ((organizationMemo.cmmUniqueID == Guid.Empty) ? Guid.NewGuid() : organizationMemo.cmmUniqueID);
				dataRow["cmmUniqueID"] = organizationMemo.cmmUniqueID;
				dataRow["cmmCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cmmCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The OrganizationMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (organizationMemo.cmmRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the OrganizationMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cmmRowVersion"], organizationMemo.cmmRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the OrganizationMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the OrganizationMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cmmLongDescriptionRtf"] = organizationMemo.cmmLongDescriptionRtf ?? dataRow["cmmLongDescriptionRtf"];
			dataRow["cmmLongDescriptionText"] = organizationMemo.cmmLongDescriptionText ?? dataRow["cmmLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? cmmMemoDate = organizationMemo.cmmMemoDate;
			dataRow2["cmmMemoDate"] = (cmmMemoDate.HasValue ? ((object)cmmMemoDate.GetValueOrDefault()) : dataRow["cmmMemoDate"]);
			dataRow["cmmShortDescription"] = organizationMemo.cmmShortDescription;
			dataRow["cmmShowInApInvoices"] = organizationMemo.cmmShowInApInvoices;
			dataRow["cmmShowInApPayments"] = organizationMemo.cmmShowInApPayments;
			dataRow["cmmShowInArInvoices"] = organizationMemo.cmmShowInArInvoices;
			dataRow["cmmShowInArPayments"] = organizationMemo.cmmShowInArPayments;
			dataRow["cmmShowInCalls"] = organizationMemo.cmmShowInCalls;
			dataRow["cmmShowInDmrClaims"] = organizationMemo.cmmShowInDmrClaims;
			dataRow["cmmShowInDmrShipments"] = organizationMemo.cmmShowInDmrShipments;
			dataRow["cmmShowInLeads"] = organizationMemo.cmmShowInLeads;
			dataRow["cmmShowInOrganizations"] = organizationMemo.cmmShowInOrganizations;
			dataRow["cmmShowInPriceAndAvailability"] = organizationMemo.cmmShowInPriceAndAvailability;
			dataRow["cmmShowInPurchaseOrders"] = organizationMemo.cmmShowInPurchaseOrders;
			dataRow["cmmShowInQuotes"] = organizationMemo.cmmShowInQuotes;
			dataRow["cmmShowInReceipts"] = organizationMemo.cmmShowInReceipts;
			dataRow["cmmShowInRfqs"] = organizationMemo.cmmShowInRfqs;
			dataRow["cmmShowInRmaClaims"] = organizationMemo.cmmShowInRmaClaims;
			dataRow["cmmShowInRmaReceipts"] = organizationMemo.cmmShowInRmaReceipts;
			dataRow["cmmShowInSalesOrders"] = organizationMemo.cmmShowInSalesOrders;
			dataRow["cmmShowInShipments"] = organizationMemo.cmmShowInShipments;
			if (organizationMemo.CustomFields != null && organizationMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in organizationMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the OrganizationMemo [{organizationMemo.cmmUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the OrganizationMemo [{organizationMemo.cmmUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
