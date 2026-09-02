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

public class ERPCallRepository : APIBaseRepository, IERPCallRepository, IAPIBaseRepository, IDisposable
{
	public ERPCallRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesCallExist(Guid callId)
	{
		InitializeParameterLists();
		base.filterList.Add("kbpUniqueID|C", callId);
		base.selectList.Add("kbpUniqueID");
		return Task.FromResult(GetAsObject("Calls", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPCallInformationDto>> GetAllCalls(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPCallInformationDto> collection = new List<ERPCallInformationDto>();
		InitializeParameterLists();
		string[] array = new string[66]
		{
			"kbpAcceptedDate", "kbpApInvoiceID", "kbpArInvoiceContactID", "kbpArInvoiceID", "kbpArInvoiceLocationID", "kbpArInvoiceOrganizationID", "kbpAssignedDate", "kbpAssignedToEmployeeID", "kbpCallTypeID", "kbpClosedByEmployeeID",
			"kbpClosedDate", "kbpCallID", "kbpContactID", "kbpContactMethodID", "kbpCreatedBy", "kbpCreatedDate", "kbpCurrencyRateID", "kbpDmrClaimID", "kbpDueDate", "kbpUniqueID",
			"kbpExchangeRate", "kbpExtraTime", "kbpBillable", "kbpCreatedFromMobile", "kbpCustomRate", "kbpFieldServiceCall", "kbpFieldServiceJobCreated", "kbpInbound", "kbpInternalOnly", "kbpInvoicedComplete",
			"kbpPublished", "kbpJobID", "kbpLeadID", "kbpLocationID", "kbpLongDescriptionRtf", "kbpLongDescriptionText", "kbpMethodPartID", "kbpMethodRevisionID", "kbpOpenedByEmployeeID", "kbpOpenedDate",
			"kbpOrganizationID", "kbpOrgPartID", "kbpPartGroupID", "kbpPartID", "kbpPartRevisionID", "kbpPartShortDescription", "kbpPhoneNumber", "kbpPriorityID", "kbpProjectAreaID", "kbpProjectID",
			"kbpPurchaseOrderID", "kbpQuoteID", "kbpReasonID", "kbpReceiptID", "kbpRfqID", "kbpRmaClaimID", "kbpRowVersion", "kbpSalesOrderID", "kbpSerialNumberID", "kbpShipmentID",
			"kbpShortDescription", "kbpStatus", "kbpSubTotalTime", "kbpTemplateFile", "kbpTimeSpent", "kbpTotalTime"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Calls");
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
		using (DataTable dataTable = GetAsDataTable("Calls", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPCallInformationDto eRPCallInformationDto = new ERPCallInformationDto();
				eRPCallInformationDto.kbpAcceptedDate = dataTable.Rows[i].Field<DateTime?>("kbpAcceptedDate");
				eRPCallInformationDto.kbpApInvoiceID = dataTable.Rows[i].Field<string>("kbpApInvoiceID");
				eRPCallInformationDto.kbpArInvoiceContactID = dataTable.Rows[i].Field<string>("kbpArInvoiceContactID");
				eRPCallInformationDto.kbpArInvoiceID = dataTable.Rows[i].Field<string>("kbpArInvoiceID");
				eRPCallInformationDto.kbpArInvoiceLocationID = dataTable.Rows[i].Field<string>("kbpArInvoiceLocationID");
				eRPCallInformationDto.kbpArInvoiceOrganizationID = dataTable.Rows[i].Field<string>("kbpArInvoiceOrganizationID");
				eRPCallInformationDto.kbpAssignedDate = dataTable.Rows[i].Field<DateTime?>("kbpAssignedDate");
				eRPCallInformationDto.kbpAssignedToEmployeeID = dataTable.Rows[i].Field<string>("kbpAssignedToEmployeeID");
				eRPCallInformationDto.kbpCallTypeID = dataTable.Rows[i].Field<string>("kbpCallTypeID");
				eRPCallInformationDto.kbpClosedByEmployeeID = dataTable.Rows[i].Field<string>("kbpClosedByEmployeeID");
				eRPCallInformationDto.kbpClosedDate = dataTable.Rows[i].Field<DateTime?>("kbpClosedDate");
				eRPCallInformationDto.kbpCallID = dataTable.Rows[i].Field<string>("kbpCallID");
				eRPCallInformationDto.kbpContactID = dataTable.Rows[i].Field<string>("kbpContactID");
				eRPCallInformationDto.kbpContactMethodID = dataTable.Rows[i].Field<string>("kbpContactMethodID");
				eRPCallInformationDto.kbpCreatedBy = dataTable.Rows[i].Field<string>("kbpCreatedBy");
				eRPCallInformationDto.kbpCreatedDate = dataTable.Rows[i].Field<DateTime?>("kbpCreatedDate");
				eRPCallInformationDto.kbpCurrencyRateID = dataTable.Rows[i].Field<string>("kbpCurrencyRateID");
				eRPCallInformationDto.kbpDmrClaimID = dataTable.Rows[i].Field<string>("kbpDmrClaimID");
				eRPCallInformationDto.kbpDueDate = dataTable.Rows[i].Field<DateTime?>("kbpDueDate");
				eRPCallInformationDto.kbpUniqueID = dataTable.Rows[i].Field<Guid>("kbpUniqueID");
				eRPCallInformationDto.kbpExchangeRate = dataTable.Rows[i].Field<decimal>("kbpExchangeRate");
				eRPCallInformationDto.kbpExtraTime = dataTable.Rows[i].Field<decimal>("kbpExtraTime");
				eRPCallInformationDto.kbpBillable = dataTable.Rows[i].Field<bool>("kbpBillable");
				eRPCallInformationDto.kbpCreatedFromMobile = dataTable.Rows[i].Field<bool>("kbpCreatedFromMobile");
				eRPCallInformationDto.kbpCustomRate = dataTable.Rows[i].Field<bool>("kbpCustomRate");
				eRPCallInformationDto.kbpFieldServiceCall = dataTable.Rows[i].Field<bool>("kbpFieldServiceCall");
				eRPCallInformationDto.kbpFieldServiceJobCreated = dataTable.Rows[i].Field<bool>("kbpFieldServiceJobCreated");
				eRPCallInformationDto.kbpInbound = dataTable.Rows[i].Field<bool>("kbpInbound");
				eRPCallInformationDto.kbpInternalOnly = dataTable.Rows[i].Field<bool>("kbpInternalOnly");
				eRPCallInformationDto.kbpInvoicedComplete = dataTable.Rows[i].Field<bool>("kbpInvoicedComplete");
				eRPCallInformationDto.kbpPublished = dataTable.Rows[i].Field<bool>("kbpPublished");
				eRPCallInformationDto.kbpJobID = dataTable.Rows[i].Field<string>("kbpJobID");
				eRPCallInformationDto.kbpLeadID = dataTable.Rows[i].Field<string>("kbpLeadID");
				eRPCallInformationDto.kbpLocationID = dataTable.Rows[i].Field<string>("kbpLocationID");
				eRPCallInformationDto.kbpLongDescriptionRtf = dataTable.Rows[i].Field<string>("kbpLongDescriptionRtf");
				eRPCallInformationDto.kbpLongDescriptionText = dataTable.Rows[i].Field<string>("kbpLongDescriptionText");
				eRPCallInformationDto.kbpMethodPartID = dataTable.Rows[i].Field<string>("kbpMethodPartID");
				eRPCallInformationDto.kbpMethodRevisionID = dataTable.Rows[i].Field<string>("kbpMethodRevisionID");
				eRPCallInformationDto.kbpOpenedByEmployeeID = dataTable.Rows[i].Field<string>("kbpOpenedByEmployeeID");
				eRPCallInformationDto.kbpOpenedDate = dataTable.Rows[i].Field<DateTime?>("kbpOpenedDate");
				eRPCallInformationDto.kbpOrganizationID = dataTable.Rows[i].Field<string>("kbpOrganizationID");
				eRPCallInformationDto.kbpOrgPartID = dataTable.Rows[i].Field<string>("kbpOrgPartID");
				eRPCallInformationDto.kbpPartGroupID = dataTable.Rows[i].Field<string>("kbpPartGroupID");
				eRPCallInformationDto.kbpPartID = dataTable.Rows[i].Field<string>("kbpPartID");
				eRPCallInformationDto.kbpPartRevisionID = dataTable.Rows[i].Field<string>("kbpPartRevisionID");
				eRPCallInformationDto.kbpPartShortDescription = dataTable.Rows[i].Field<string>("kbpPartShortDescription");
				eRPCallInformationDto.kbpPhoneNumber = dataTable.Rows[i].Field<string>("kbpPhoneNumber");
				eRPCallInformationDto.kbpPriorityID = dataTable.Rows[i].Field<byte>("kbpPriorityID");
				eRPCallInformationDto.kbpProjectAreaID = dataTable.Rows[i].Field<string>("kbpProjectAreaID");
				eRPCallInformationDto.kbpProjectID = dataTable.Rows[i].Field<string>("kbpProjectID");
				eRPCallInformationDto.kbpPurchaseOrderID = dataTable.Rows[i].Field<string>("kbpPurchaseOrderID");
				eRPCallInformationDto.kbpQuoteID = dataTable.Rows[i].Field<string>("kbpQuoteID");
				eRPCallInformationDto.kbpReasonID = dataTable.Rows[i].Field<string>("kbpReasonID");
				eRPCallInformationDto.kbpReceiptID = dataTable.Rows[i].Field<string>("kbpReceiptID");
				eRPCallInformationDto.kbpRfqID = dataTable.Rows[i].Field<string>("kbpRfqID");
				eRPCallInformationDto.kbpRmaClaimID = dataTable.Rows[i].Field<string>("kbpRmaClaimID");
				eRPCallInformationDto.kbpRowVersion = dataTable.Rows[i].Field<byte[]>("kbpRowVersion");
				eRPCallInformationDto.kbpSalesOrderID = dataTable.Rows[i].Field<string>("kbpSalesOrderID");
				eRPCallInformationDto.kbpSerialNumberID = dataTable.Rows[i].Field<string>("kbpSerialNumberID");
				eRPCallInformationDto.kbpShipmentID = dataTable.Rows[i].Field<string>("kbpShipmentID");
				eRPCallInformationDto.kbpShortDescription = dataTable.Rows[i].Field<string>("kbpShortDescription");
				eRPCallInformationDto.kbpStatus = dataTable.Rows[i].Field<string>("kbpStatus");
				eRPCallInformationDto.kbpSubTotalTime = dataTable.Rows[i].Field<decimal>("kbpSubTotalTime");
				eRPCallInformationDto.kbpTemplateFile = dataTable.Rows[i].Field<string>("kbpTemplateFile");
				eRPCallInformationDto.kbpTimeSpent = dataTable.Rows[i].Field<decimal>("kbpTimeSpent");
				eRPCallInformationDto.kbpTotalTime = dataTable.Rows[i].Field<decimal>("kbpTotalTime");
				eRPCallInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPCallInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPCallInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPCallInformationDto> GetCall(Guid callId)
	{
		ERPCallInformationDto eRPCallInformationDto = new ERPCallInformationDto();
		InitializeParameterLists();
		string[] collection = new string[66]
		{
			"kbpAcceptedDate", "kbpApInvoiceID", "kbpArInvoiceContactID", "kbpArInvoiceID", "kbpArInvoiceLocationID", "kbpArInvoiceOrganizationID", "kbpAssignedDate", "kbpAssignedToEmployeeID", "kbpCallTypeID", "kbpClosedByEmployeeID",
			"kbpClosedDate", "kbpCallID", "kbpContactID", "kbpContactMethodID", "kbpCreatedBy", "kbpCreatedDate", "kbpCurrencyRateID", "kbpDmrClaimID", "kbpDueDate", "kbpUniqueID",
			"kbpExchangeRate", "kbpExtraTime", "kbpBillable", "kbpCreatedFromMobile", "kbpCustomRate", "kbpFieldServiceCall", "kbpFieldServiceJobCreated", "kbpInbound", "kbpInternalOnly", "kbpInvoicedComplete",
			"kbpPublished", "kbpJobID", "kbpLeadID", "kbpLocationID", "kbpLongDescriptionRtf", "kbpLongDescriptionText", "kbpMethodPartID", "kbpMethodRevisionID", "kbpOpenedByEmployeeID", "kbpOpenedDate",
			"kbpOrganizationID", "kbpOrgPartID", "kbpPartGroupID", "kbpPartID", "kbpPartRevisionID", "kbpPartShortDescription", "kbpPhoneNumber", "kbpPriorityID", "kbpProjectAreaID", "kbpProjectID",
			"kbpPurchaseOrderID", "kbpQuoteID", "kbpReasonID", "kbpReceiptID", "kbpRfqID", "kbpRmaClaimID", "kbpRowVersion", "kbpSalesOrderID", "kbpSerialNumberID", "kbpShipmentID",
			"kbpShortDescription", "kbpStatus", "kbpSubTotalTime", "kbpTemplateFile", "kbpTimeSpent", "kbpTotalTime"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("kbpUniqueID|C", callId);
		AddCustomFieldsToSelectList("Calls");
		using (DataTable dataTable = GetAsDataTable("Calls", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPCallInformationDto);
			}
			eRPCallInformationDto.kbpAcceptedDate = dataTable.Rows[0].Field<DateTime?>("kbpAcceptedDate");
			eRPCallInformationDto.kbpApInvoiceID = dataTable.Rows[0].Field<string>("kbpApInvoiceID");
			eRPCallInformationDto.kbpArInvoiceContactID = dataTable.Rows[0].Field<string>("kbpArInvoiceContactID");
			eRPCallInformationDto.kbpArInvoiceID = dataTable.Rows[0].Field<string>("kbpArInvoiceID");
			eRPCallInformationDto.kbpArInvoiceLocationID = dataTable.Rows[0].Field<string>("kbpArInvoiceLocationID");
			eRPCallInformationDto.kbpArInvoiceOrganizationID = dataTable.Rows[0].Field<string>("kbpArInvoiceOrganizationID");
			eRPCallInformationDto.kbpAssignedDate = dataTable.Rows[0].Field<DateTime?>("kbpAssignedDate");
			eRPCallInformationDto.kbpAssignedToEmployeeID = dataTable.Rows[0].Field<string>("kbpAssignedToEmployeeID");
			eRPCallInformationDto.kbpCallTypeID = dataTable.Rows[0].Field<string>("kbpCallTypeID");
			eRPCallInformationDto.kbpClosedByEmployeeID = dataTable.Rows[0].Field<string>("kbpClosedByEmployeeID");
			eRPCallInformationDto.kbpClosedDate = dataTable.Rows[0].Field<DateTime?>("kbpClosedDate");
			eRPCallInformationDto.kbpCallID = dataTable.Rows[0].Field<string>("kbpCallID");
			eRPCallInformationDto.kbpContactID = dataTable.Rows[0].Field<string>("kbpContactID");
			eRPCallInformationDto.kbpContactMethodID = dataTable.Rows[0].Field<string>("kbpContactMethodID");
			eRPCallInformationDto.kbpCreatedBy = dataTable.Rows[0].Field<string>("kbpCreatedBy");
			eRPCallInformationDto.kbpCreatedDate = dataTable.Rows[0].Field<DateTime?>("kbpCreatedDate");
			eRPCallInformationDto.kbpCurrencyRateID = dataTable.Rows[0].Field<string>("kbpCurrencyRateID");
			eRPCallInformationDto.kbpDmrClaimID = dataTable.Rows[0].Field<string>("kbpDmrClaimID");
			eRPCallInformationDto.kbpDueDate = dataTable.Rows[0].Field<DateTime?>("kbpDueDate");
			eRPCallInformationDto.kbpUniqueID = dataTable.Rows[0].Field<Guid>("kbpUniqueID");
			eRPCallInformationDto.kbpExchangeRate = dataTable.Rows[0].Field<decimal>("kbpExchangeRate");
			eRPCallInformationDto.kbpExtraTime = dataTable.Rows[0].Field<decimal>("kbpExtraTime");
			eRPCallInformationDto.kbpBillable = dataTable.Rows[0].Field<bool>("kbpBillable");
			eRPCallInformationDto.kbpCreatedFromMobile = dataTable.Rows[0].Field<bool>("kbpCreatedFromMobile");
			eRPCallInformationDto.kbpCustomRate = dataTable.Rows[0].Field<bool>("kbpCustomRate");
			eRPCallInformationDto.kbpFieldServiceCall = dataTable.Rows[0].Field<bool>("kbpFieldServiceCall");
			eRPCallInformationDto.kbpFieldServiceJobCreated = dataTable.Rows[0].Field<bool>("kbpFieldServiceJobCreated");
			eRPCallInformationDto.kbpInbound = dataTable.Rows[0].Field<bool>("kbpInbound");
			eRPCallInformationDto.kbpInternalOnly = dataTable.Rows[0].Field<bool>("kbpInternalOnly");
			eRPCallInformationDto.kbpInvoicedComplete = dataTable.Rows[0].Field<bool>("kbpInvoicedComplete");
			eRPCallInformationDto.kbpPublished = dataTable.Rows[0].Field<bool>("kbpPublished");
			eRPCallInformationDto.kbpJobID = dataTable.Rows[0].Field<string>("kbpJobID");
			eRPCallInformationDto.kbpLeadID = dataTable.Rows[0].Field<string>("kbpLeadID");
			eRPCallInformationDto.kbpLocationID = dataTable.Rows[0].Field<string>("kbpLocationID");
			eRPCallInformationDto.kbpLongDescriptionRtf = dataTable.Rows[0].Field<string>("kbpLongDescriptionRtf");
			eRPCallInformationDto.kbpLongDescriptionText = dataTable.Rows[0].Field<string>("kbpLongDescriptionText");
			eRPCallInformationDto.kbpMethodPartID = dataTable.Rows[0].Field<string>("kbpMethodPartID");
			eRPCallInformationDto.kbpMethodRevisionID = dataTable.Rows[0].Field<string>("kbpMethodRevisionID");
			eRPCallInformationDto.kbpOpenedByEmployeeID = dataTable.Rows[0].Field<string>("kbpOpenedByEmployeeID");
			eRPCallInformationDto.kbpOpenedDate = dataTable.Rows[0].Field<DateTime?>("kbpOpenedDate");
			eRPCallInformationDto.kbpOrganizationID = dataTable.Rows[0].Field<string>("kbpOrganizationID");
			eRPCallInformationDto.kbpOrgPartID = dataTable.Rows[0].Field<string>("kbpOrgPartID");
			eRPCallInformationDto.kbpPartGroupID = dataTable.Rows[0].Field<string>("kbpPartGroupID");
			eRPCallInformationDto.kbpPartID = dataTable.Rows[0].Field<string>("kbpPartID");
			eRPCallInformationDto.kbpPartRevisionID = dataTable.Rows[0].Field<string>("kbpPartRevisionID");
			eRPCallInformationDto.kbpPartShortDescription = dataTable.Rows[0].Field<string>("kbpPartShortDescription");
			eRPCallInformationDto.kbpPhoneNumber = dataTable.Rows[0].Field<string>("kbpPhoneNumber");
			eRPCallInformationDto.kbpPriorityID = dataTable.Rows[0].Field<byte>("kbpPriorityID");
			eRPCallInformationDto.kbpProjectAreaID = dataTable.Rows[0].Field<string>("kbpProjectAreaID");
			eRPCallInformationDto.kbpProjectID = dataTable.Rows[0].Field<string>("kbpProjectID");
			eRPCallInformationDto.kbpPurchaseOrderID = dataTable.Rows[0].Field<string>("kbpPurchaseOrderID");
			eRPCallInformationDto.kbpQuoteID = dataTable.Rows[0].Field<string>("kbpQuoteID");
			eRPCallInformationDto.kbpReasonID = dataTable.Rows[0].Field<string>("kbpReasonID");
			eRPCallInformationDto.kbpReceiptID = dataTable.Rows[0].Field<string>("kbpReceiptID");
			eRPCallInformationDto.kbpRfqID = dataTable.Rows[0].Field<string>("kbpRfqID");
			eRPCallInformationDto.kbpRmaClaimID = dataTable.Rows[0].Field<string>("kbpRmaClaimID");
			eRPCallInformationDto.kbpRowVersion = dataTable.Rows[0].Field<byte[]>("kbpRowVersion");
			eRPCallInformationDto.kbpSalesOrderID = dataTable.Rows[0].Field<string>("kbpSalesOrderID");
			eRPCallInformationDto.kbpSerialNumberID = dataTable.Rows[0].Field<string>("kbpSerialNumberID");
			eRPCallInformationDto.kbpShipmentID = dataTable.Rows[0].Field<string>("kbpShipmentID");
			eRPCallInformationDto.kbpShortDescription = dataTable.Rows[0].Field<string>("kbpShortDescription");
			eRPCallInformationDto.kbpStatus = dataTable.Rows[0].Field<string>("kbpStatus");
			eRPCallInformationDto.kbpSubTotalTime = dataTable.Rows[0].Field<decimal>("kbpSubTotalTime");
			eRPCallInformationDto.kbpTemplateFile = dataTable.Rows[0].Field<string>("kbpTemplateFile");
			eRPCallInformationDto.kbpTimeSpent = dataTable.Rows[0].Field<decimal>("kbpTimeSpent");
			eRPCallInformationDto.kbpTotalTime = dataTable.Rows[0].Field<decimal>("kbpTotalTime");
			eRPCallInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPCallInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPCallInformationDto);
	}

	public Task<APIValidationInfoDto> SaveCall(ERPCallDto call)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Calls WHERE kbpUniqueID = " + M1Util.ConvertToLinq(call.kbpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["kbpCallID"] = call.kbpCallID.ToUpper();
				call.kbpUniqueID = ((call.kbpUniqueID == Guid.Empty) ? Guid.NewGuid() : call.kbpUniqueID);
				dataRow["kbpUniqueID"] = call.kbpUniqueID;
				dataRow["kbpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["kbpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Call could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (call.kbpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Call is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["kbpRowVersion"], call.kbpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Call has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Call again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? kbpAcceptedDate = call.kbpAcceptedDate;
			dataRow2["kbpAcceptedDate"] = (kbpAcceptedDate.HasValue ? ((object)kbpAcceptedDate.GetValueOrDefault()) : dataRow["kbpAcceptedDate"]);
			dataRow["kbpApInvoiceID"] = call.kbpApInvoiceID;
			dataRow["kbpArInvoiceContactID"] = call.kbpArInvoiceContactID;
			dataRow["kbpArInvoiceID"] = call.kbpArInvoiceID;
			dataRow["kbpArInvoiceLocationID"] = call.kbpArInvoiceLocationID;
			dataRow["kbpArInvoiceOrganizationID"] = call.kbpArInvoiceOrganizationID;
			DataRow dataRow3 = dataRow;
			kbpAcceptedDate = call.kbpAssignedDate;
			dataRow3["kbpAssignedDate"] = (kbpAcceptedDate.HasValue ? ((object)kbpAcceptedDate.GetValueOrDefault()) : dataRow["kbpAssignedDate"]);
			dataRow["kbpAssignedToEmployeeID"] = call.kbpAssignedToEmployeeID;
			dataRow["kbpCallTypeID"] = call.kbpCallTypeID;
			dataRow["kbpClosedByEmployeeID"] = call.kbpClosedByEmployeeID;
			DataRow dataRow4 = dataRow;
			kbpAcceptedDate = call.kbpClosedDate;
			dataRow4["kbpClosedDate"] = (kbpAcceptedDate.HasValue ? ((object)kbpAcceptedDate.GetValueOrDefault()) : dataRow["kbpClosedDate"]);
			dataRow["kbpContactID"] = call.kbpContactID;
			dataRow["kbpContactMethodID"] = call.kbpContactMethodID;
			dataRow["kbpCurrencyRateID"] = call.kbpCurrencyRateID;
			dataRow["kbpDmrClaimID"] = call.kbpDmrClaimID;
			DataRow dataRow5 = dataRow;
			kbpAcceptedDate = call.kbpDueDate;
			dataRow5["kbpDueDate"] = (kbpAcceptedDate.HasValue ? ((object)kbpAcceptedDate.GetValueOrDefault()) : dataRow["kbpDueDate"]);
			dataRow["kbpExchangeRate"] = call.kbpExchangeRate;
			dataRow["kbpExtraTime"] = call.kbpExtraTime;
			dataRow["kbpBillable"] = call.kbpBillable;
			dataRow["kbpCreatedFromMobile"] = call.kbpCreatedFromMobile;
			dataRow["kbpCustomRate"] = call.kbpCustomRate;
			dataRow["kbpFieldServiceCall"] = call.kbpFieldServiceCall;
			dataRow["kbpFieldServiceJobCreated"] = call.kbpFieldServiceJobCreated;
			dataRow["kbpInbound"] = call.kbpInbound;
			dataRow["kbpInternalOnly"] = call.kbpInternalOnly;
			dataRow["kbpInvoicedComplete"] = call.kbpInvoicedComplete;
			dataRow["kbpPublished"] = call.kbpPublished;
			dataRow["kbpJobID"] = call.kbpJobID;
			dataRow["kbpLeadID"] = call.kbpLeadID;
			dataRow["kbpLocationID"] = call.kbpLocationID;
			dataRow["kbpLongDescriptionRtf"] = call.kbpLongDescriptionRtf ?? dataRow["kbpLongDescriptionRtf"];
			dataRow["kbpLongDescriptionText"] = call.kbpLongDescriptionText ?? dataRow["kbpLongDescriptionText"];
			dataRow["kbpMethodPartID"] = call.kbpMethodPartID;
			dataRow["kbpMethodRevisionID"] = call.kbpMethodRevisionID;
			dataRow["kbpOpenedByEmployeeID"] = call.kbpOpenedByEmployeeID;
			DataRow dataRow6 = dataRow;
			kbpAcceptedDate = call.kbpOpenedDate;
			dataRow6["kbpOpenedDate"] = (kbpAcceptedDate.HasValue ? ((object)kbpAcceptedDate.GetValueOrDefault()) : dataRow["kbpOpenedDate"]);
			dataRow["kbpOrganizationID"] = call.kbpOrganizationID;
			dataRow["kbpOrgPartID"] = call.kbpOrgPartID;
			dataRow["kbpPartGroupID"] = call.kbpPartGroupID;
			dataRow["kbpPartID"] = call.kbpPartID;
			dataRow["kbpPartRevisionID"] = call.kbpPartRevisionID;
			dataRow["kbpPartShortDescription"] = call.kbpPartShortDescription;
			dataRow["kbpPhoneNumber"] = call.kbpPhoneNumber;
			dataRow["kbpPriorityID"] = call.kbpPriorityID;
			dataRow["kbpProjectAreaID"] = call.kbpProjectAreaID;
			dataRow["kbpProjectID"] = call.kbpProjectID;
			dataRow["kbpPurchaseOrderID"] = call.kbpPurchaseOrderID;
			dataRow["kbpQuoteID"] = call.kbpQuoteID;
			dataRow["kbpReasonID"] = call.kbpReasonID;
			dataRow["kbpReceiptID"] = call.kbpReceiptID;
			dataRow["kbpRfqID"] = call.kbpRfqID;
			dataRow["kbpRmaClaimID"] = call.kbpRmaClaimID;
			dataRow["kbpSalesOrderID"] = call.kbpSalesOrderID;
			dataRow["kbpSerialNumberID"] = call.kbpSerialNumberID;
			dataRow["kbpShipmentID"] = call.kbpShipmentID;
			dataRow["kbpShortDescription"] = call.kbpShortDescription;
			dataRow["kbpStatus"] = call.kbpStatus;
			dataRow["kbpSubTotalTime"] = call.kbpSubTotalTime;
			dataRow["kbpTemplateFile"] = call.kbpTemplateFile;
			dataRow["kbpTimeSpent"] = call.kbpTimeSpent;
			dataRow["kbpTotalTime"] = call.kbpTotalTime;
			if (call.CustomFields != null && call.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in call.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Call [{call.kbpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Call [{call.kbpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
