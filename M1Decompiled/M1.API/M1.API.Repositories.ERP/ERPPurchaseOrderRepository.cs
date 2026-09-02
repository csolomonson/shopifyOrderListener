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

public class ERPPurchaseOrderRepository : APIBaseRepository, IERPPurchaseOrderRepository, IAPIBaseRepository, IDisposable
{
	public ERPPurchaseOrderRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPurchaseOrderExist(Guid purchaseOrderId)
	{
		InitializeParameterLists();
		base.filterList.Add("pmpUniqueID|C", purchaseOrderId);
		base.selectList.Add("pmpUniqueID");
		return Task.FromResult(GetAsObject("PurchaseOrders", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPurchaseOrderInformationDto>> GetAllPurchaseOrders(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPurchaseOrderInformationDto> collection = new List<ERPPurchaseOrderInformationDto>();
		InitializeParameterLists();
		string[] array = new string[45]
		{
			"pmpApInvoiceContactID", "pmpApInvoiceLocationID", "pmpApprovalDecisionDate", "pmpApprovalRequestDate", "pmpBuyerEmployeeID", "pmpClosedDate", "pmpPurchaseOrderID", "pmpCreatedBy", "pmpCreatedDate", "pmpCurrencyRateID",
			"pmpDocuments", "pmpDropShipContactID", "pmpDropShipLocationID", "pmpDropShipOrganizationID", "pmpDueDate", "pmpUniqueID", "pmpExchangeRate", "pmpFreeOnBoardDescription", "pmpIntraCompanyPostedDate", "pmpClosed",
			"pmpCustomRate", "pmpIntraCompany", "pmpIntraCompanyPosted", "pmpReadyToPrint", "pmpNextApprovalEmployeeID", "pmpOrderCommentsRTF", "pmpOrderCommentsText", "pmpOrderDate", "pmpOrderSubtotalBase", "pmpOrderSubtotalForeign",
			"pmpOrderTaxAmountBase", "pmpOrderTaxAmountForeign", "pmpOrderTotalBase", "pmpOrderTotalForeign", "pmpPaymentTermID", "pmpPlantDepartmentID", "pmpPlantID", "pmpProjectID", "pmpPurchaseContactID", "pmpPurchaseLocationID",
			"pmpRowVersion", "pmpShippingMethodID", "pmpStandardMessageID", "pmpStatus", "pmpSupplierOrganizationID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PurchaseOrders");
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
		using (DataTable dataTable = GetAsDataTable("PurchaseOrders", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPurchaseOrderInformationDto eRPPurchaseOrderInformationDto = new ERPPurchaseOrderInformationDto();
				eRPPurchaseOrderInformationDto.pmpApInvoiceContactID = dataTable.Rows[i].Field<string>("pmpApInvoiceContactID");
				eRPPurchaseOrderInformationDto.pmpApInvoiceLocationID = dataTable.Rows[i].Field<string>("pmpApInvoiceLocationID");
				eRPPurchaseOrderInformationDto.pmpApprovalDecisionDate = dataTable.Rows[i].Field<DateTime?>("pmpApprovalDecisionDate");
				eRPPurchaseOrderInformationDto.pmpApprovalRequestDate = dataTable.Rows[i].Field<DateTime?>("pmpApprovalRequestDate");
				eRPPurchaseOrderInformationDto.pmpBuyerEmployeeID = dataTable.Rows[i].Field<string>("pmpBuyerEmployeeID");
				eRPPurchaseOrderInformationDto.pmpClosedDate = dataTable.Rows[i].Field<DateTime?>("pmpClosedDate");
				eRPPurchaseOrderInformationDto.pmpPurchaseOrderID = dataTable.Rows[i].Field<string>("pmpPurchaseOrderID");
				eRPPurchaseOrderInformationDto.pmpCreatedBy = dataTable.Rows[i].Field<string>("pmpCreatedBy");
				eRPPurchaseOrderInformationDto.pmpCreatedDate = dataTable.Rows[i].Field<DateTime?>("pmpCreatedDate");
				eRPPurchaseOrderInformationDto.pmpCurrencyRateID = dataTable.Rows[i].Field<string>("pmpCurrencyRateID");
				eRPPurchaseOrderInformationDto.pmpDocuments = dataTable.Rows[i].Field<string>("pmpDocuments");
				eRPPurchaseOrderInformationDto.pmpDropShipContactID = dataTable.Rows[i].Field<string>("pmpDropShipContactID");
				eRPPurchaseOrderInformationDto.pmpDropShipLocationID = dataTable.Rows[i].Field<string>("pmpDropShipLocationID");
				eRPPurchaseOrderInformationDto.pmpDropShipOrganizationID = dataTable.Rows[i].Field<string>("pmpDropShipOrganizationID");
				eRPPurchaseOrderInformationDto.pmpDueDate = dataTable.Rows[i].Field<DateTime?>("pmpDueDate");
				eRPPurchaseOrderInformationDto.pmpUniqueID = dataTable.Rows[i].Field<Guid>("pmpUniqueID");
				eRPPurchaseOrderInformationDto.pmpExchangeRate = dataTable.Rows[i].Field<decimal>("pmpExchangeRate");
				eRPPurchaseOrderInformationDto.pmpFreeOnBoardDescription = dataTable.Rows[i].Field<string>("pmpFreeOnBoardDescription");
				eRPPurchaseOrderInformationDto.pmpIntraCompanyPostedDate = dataTable.Rows[i].Field<DateTime?>("pmpIntraCompanyPostedDate");
				eRPPurchaseOrderInformationDto.pmpClosed = dataTable.Rows[i].Field<bool>("pmpClosed");
				eRPPurchaseOrderInformationDto.pmpCustomRate = dataTable.Rows[i].Field<bool>("pmpCustomRate");
				eRPPurchaseOrderInformationDto.pmpIntraCompany = dataTable.Rows[i].Field<bool>("pmpIntraCompany");
				eRPPurchaseOrderInformationDto.pmpIntraCompanyPosted = dataTable.Rows[i].Field<bool>("pmpIntraCompanyPosted");
				eRPPurchaseOrderInformationDto.pmpReadyToPrint = dataTable.Rows[i].Field<bool>("pmpReadyToPrint");
				eRPPurchaseOrderInformationDto.pmpNextApprovalEmployeeID = dataTable.Rows[i].Field<string>("pmpNextApprovalEmployeeID");
				eRPPurchaseOrderInformationDto.pmpOrderCommentsRTF = dataTable.Rows[i].Field<string>("pmpOrderCommentsRTF");
				eRPPurchaseOrderInformationDto.pmpOrderCommentsText = dataTable.Rows[i].Field<string>("pmpOrderCommentsText");
				eRPPurchaseOrderInformationDto.pmpOrderDate = dataTable.Rows[i].Field<DateTime?>("pmpOrderDate");
				eRPPurchaseOrderInformationDto.pmpOrderSubtotalBase = dataTable.Rows[i].Field<decimal>("pmpOrderSubtotalBase");
				eRPPurchaseOrderInformationDto.pmpOrderSubtotalForeign = dataTable.Rows[i].Field<decimal>("pmpOrderSubtotalForeign");
				eRPPurchaseOrderInformationDto.pmpOrderTaxAmountBase = dataTable.Rows[i].Field<decimal>("pmpOrderTaxAmountBase");
				eRPPurchaseOrderInformationDto.pmpOrderTaxAmountForeign = dataTable.Rows[i].Field<decimal>("pmpOrderTaxAmountForeign");
				eRPPurchaseOrderInformationDto.pmpOrderTotalBase = dataTable.Rows[i].Field<decimal>("pmpOrderTotalBase");
				eRPPurchaseOrderInformationDto.pmpOrderTotalForeign = dataTable.Rows[i].Field<decimal>("pmpOrderTotalForeign");
				eRPPurchaseOrderInformationDto.pmpPaymentTermID = dataTable.Rows[i].Field<string>("pmpPaymentTermID");
				eRPPurchaseOrderInformationDto.pmpPlantDepartmentID = dataTable.Rows[i].Field<string>("pmpPlantDepartmentID");
				eRPPurchaseOrderInformationDto.pmpPlantID = dataTable.Rows[i].Field<string>("pmpPlantID");
				eRPPurchaseOrderInformationDto.pmpProjectID = dataTable.Rows[i].Field<string>("pmpProjectID");
				eRPPurchaseOrderInformationDto.pmpPurchaseContactID = dataTable.Rows[i].Field<string>("pmpPurchaseContactID");
				eRPPurchaseOrderInformationDto.pmpPurchaseLocationID = dataTable.Rows[i].Field<string>("pmpPurchaseLocationID");
				eRPPurchaseOrderInformationDto.pmpRowVersion = dataTable.Rows[i].Field<byte[]>("pmpRowVersion");
				eRPPurchaseOrderInformationDto.pmpShippingMethodID = dataTable.Rows[i].Field<string>("pmpShippingMethodID");
				eRPPurchaseOrderInformationDto.pmpStandardMessageID = dataTable.Rows[i].Field<string>("pmpStandardMessageID");
				eRPPurchaseOrderInformationDto.pmpStatus = dataTable.Rows[i].Field<byte>("pmpStatus");
				eRPPurchaseOrderInformationDto.pmpSupplierOrganizationID = dataTable.Rows[i].Field<string>("pmpSupplierOrganizationID");
				eRPPurchaseOrderInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPurchaseOrderInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPurchaseOrderInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPurchaseOrderInformationDto> GetPurchaseOrder(Guid purchaseOrderId)
	{
		ERPPurchaseOrderInformationDto eRPPurchaseOrderInformationDto = new ERPPurchaseOrderInformationDto();
		InitializeParameterLists();
		string[] collection = new string[45]
		{
			"pmpApInvoiceContactID", "pmpApInvoiceLocationID", "pmpApprovalDecisionDate", "pmpApprovalRequestDate", "pmpBuyerEmployeeID", "pmpClosedDate", "pmpPurchaseOrderID", "pmpCreatedBy", "pmpCreatedDate", "pmpCurrencyRateID",
			"pmpDocuments", "pmpDropShipContactID", "pmpDropShipLocationID", "pmpDropShipOrganizationID", "pmpDueDate", "pmpUniqueID", "pmpExchangeRate", "pmpFreeOnBoardDescription", "pmpIntraCompanyPostedDate", "pmpClosed",
			"pmpCustomRate", "pmpIntraCompany", "pmpIntraCompanyPosted", "pmpReadyToPrint", "pmpNextApprovalEmployeeID", "pmpOrderCommentsRTF", "pmpOrderCommentsText", "pmpOrderDate", "pmpOrderSubtotalBase", "pmpOrderSubtotalForeign",
			"pmpOrderTaxAmountBase", "pmpOrderTaxAmountForeign", "pmpOrderTotalBase", "pmpOrderTotalForeign", "pmpPaymentTermID", "pmpPlantDepartmentID", "pmpPlantID", "pmpProjectID", "pmpPurchaseContactID", "pmpPurchaseLocationID",
			"pmpRowVersion", "pmpShippingMethodID", "pmpStandardMessageID", "pmpStatus", "pmpSupplierOrganizationID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("pmpUniqueID|C", purchaseOrderId);
		AddCustomFieldsToSelectList("PurchaseOrders");
		using (DataTable dataTable = GetAsDataTable("PurchaseOrders", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPurchaseOrderInformationDto);
			}
			eRPPurchaseOrderInformationDto.pmpApInvoiceContactID = dataTable.Rows[0].Field<string>("pmpApInvoiceContactID");
			eRPPurchaseOrderInformationDto.pmpApInvoiceLocationID = dataTable.Rows[0].Field<string>("pmpApInvoiceLocationID");
			eRPPurchaseOrderInformationDto.pmpApprovalDecisionDate = dataTable.Rows[0].Field<DateTime?>("pmpApprovalDecisionDate");
			eRPPurchaseOrderInformationDto.pmpApprovalRequestDate = dataTable.Rows[0].Field<DateTime?>("pmpApprovalRequestDate");
			eRPPurchaseOrderInformationDto.pmpBuyerEmployeeID = dataTable.Rows[0].Field<string>("pmpBuyerEmployeeID");
			eRPPurchaseOrderInformationDto.pmpClosedDate = dataTable.Rows[0].Field<DateTime?>("pmpClosedDate");
			eRPPurchaseOrderInformationDto.pmpPurchaseOrderID = dataTable.Rows[0].Field<string>("pmpPurchaseOrderID");
			eRPPurchaseOrderInformationDto.pmpCreatedBy = dataTable.Rows[0].Field<string>("pmpCreatedBy");
			eRPPurchaseOrderInformationDto.pmpCreatedDate = dataTable.Rows[0].Field<DateTime?>("pmpCreatedDate");
			eRPPurchaseOrderInformationDto.pmpCurrencyRateID = dataTable.Rows[0].Field<string>("pmpCurrencyRateID");
			eRPPurchaseOrderInformationDto.pmpDocuments = dataTable.Rows[0].Field<string>("pmpDocuments");
			eRPPurchaseOrderInformationDto.pmpDropShipContactID = dataTable.Rows[0].Field<string>("pmpDropShipContactID");
			eRPPurchaseOrderInformationDto.pmpDropShipLocationID = dataTable.Rows[0].Field<string>("pmpDropShipLocationID");
			eRPPurchaseOrderInformationDto.pmpDropShipOrganizationID = dataTable.Rows[0].Field<string>("pmpDropShipOrganizationID");
			eRPPurchaseOrderInformationDto.pmpDueDate = dataTable.Rows[0].Field<DateTime?>("pmpDueDate");
			eRPPurchaseOrderInformationDto.pmpUniqueID = dataTable.Rows[0].Field<Guid>("pmpUniqueID");
			eRPPurchaseOrderInformationDto.pmpExchangeRate = dataTable.Rows[0].Field<decimal>("pmpExchangeRate");
			eRPPurchaseOrderInformationDto.pmpFreeOnBoardDescription = dataTable.Rows[0].Field<string>("pmpFreeOnBoardDescription");
			eRPPurchaseOrderInformationDto.pmpIntraCompanyPostedDate = dataTable.Rows[0].Field<DateTime?>("pmpIntraCompanyPostedDate");
			eRPPurchaseOrderInformationDto.pmpClosed = dataTable.Rows[0].Field<bool>("pmpClosed");
			eRPPurchaseOrderInformationDto.pmpCustomRate = dataTable.Rows[0].Field<bool>("pmpCustomRate");
			eRPPurchaseOrderInformationDto.pmpIntraCompany = dataTable.Rows[0].Field<bool>("pmpIntraCompany");
			eRPPurchaseOrderInformationDto.pmpIntraCompanyPosted = dataTable.Rows[0].Field<bool>("pmpIntraCompanyPosted");
			eRPPurchaseOrderInformationDto.pmpReadyToPrint = dataTable.Rows[0].Field<bool>("pmpReadyToPrint");
			eRPPurchaseOrderInformationDto.pmpNextApprovalEmployeeID = dataTable.Rows[0].Field<string>("pmpNextApprovalEmployeeID");
			eRPPurchaseOrderInformationDto.pmpOrderCommentsRTF = dataTable.Rows[0].Field<string>("pmpOrderCommentsRTF");
			eRPPurchaseOrderInformationDto.pmpOrderCommentsText = dataTable.Rows[0].Field<string>("pmpOrderCommentsText");
			eRPPurchaseOrderInformationDto.pmpOrderDate = dataTable.Rows[0].Field<DateTime?>("pmpOrderDate");
			eRPPurchaseOrderInformationDto.pmpOrderSubtotalBase = dataTable.Rows[0].Field<decimal>("pmpOrderSubtotalBase");
			eRPPurchaseOrderInformationDto.pmpOrderSubtotalForeign = dataTable.Rows[0].Field<decimal>("pmpOrderSubtotalForeign");
			eRPPurchaseOrderInformationDto.pmpOrderTaxAmountBase = dataTable.Rows[0].Field<decimal>("pmpOrderTaxAmountBase");
			eRPPurchaseOrderInformationDto.pmpOrderTaxAmountForeign = dataTable.Rows[0].Field<decimal>("pmpOrderTaxAmountForeign");
			eRPPurchaseOrderInformationDto.pmpOrderTotalBase = dataTable.Rows[0].Field<decimal>("pmpOrderTotalBase");
			eRPPurchaseOrderInformationDto.pmpOrderTotalForeign = dataTable.Rows[0].Field<decimal>("pmpOrderTotalForeign");
			eRPPurchaseOrderInformationDto.pmpPaymentTermID = dataTable.Rows[0].Field<string>("pmpPaymentTermID");
			eRPPurchaseOrderInformationDto.pmpPlantDepartmentID = dataTable.Rows[0].Field<string>("pmpPlantDepartmentID");
			eRPPurchaseOrderInformationDto.pmpPlantID = dataTable.Rows[0].Field<string>("pmpPlantID");
			eRPPurchaseOrderInformationDto.pmpProjectID = dataTable.Rows[0].Field<string>("pmpProjectID");
			eRPPurchaseOrderInformationDto.pmpPurchaseContactID = dataTable.Rows[0].Field<string>("pmpPurchaseContactID");
			eRPPurchaseOrderInformationDto.pmpPurchaseLocationID = dataTable.Rows[0].Field<string>("pmpPurchaseLocationID");
			eRPPurchaseOrderInformationDto.pmpRowVersion = dataTable.Rows[0].Field<byte[]>("pmpRowVersion");
			eRPPurchaseOrderInformationDto.pmpShippingMethodID = dataTable.Rows[0].Field<string>("pmpShippingMethodID");
			eRPPurchaseOrderInformationDto.pmpStandardMessageID = dataTable.Rows[0].Field<string>("pmpStandardMessageID");
			eRPPurchaseOrderInformationDto.pmpStatus = dataTable.Rows[0].Field<byte>("pmpStatus");
			eRPPurchaseOrderInformationDto.pmpSupplierOrganizationID = dataTable.Rows[0].Field<string>("pmpSupplierOrganizationID");
			eRPPurchaseOrderInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPurchaseOrderInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPurchaseOrderInformationDto);
	}

	public Task<APIValidationInfoDto> SavePurchaseOrder(ERPPurchaseOrderDto purchaseOrder)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PurchaseOrders WHERE pmpUniqueID = " + M1Util.ConvertToLinq(purchaseOrder.pmpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["pmpPurchaseOrderID"] = purchaseOrder.pmpPurchaseOrderID.ToUpper();
				purchaseOrder.pmpUniqueID = ((purchaseOrder.pmpUniqueID == Guid.Empty) ? Guid.NewGuid() : purchaseOrder.pmpUniqueID);
				dataRow["pmpUniqueID"] = purchaseOrder.pmpUniqueID;
				dataRow["pmpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["pmpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PurchaseOrder could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (purchaseOrder.pmpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PurchaseOrder is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["pmpRowVersion"], purchaseOrder.pmpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PurchaseOrder has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PurchaseOrder again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["pmpApInvoiceContactID"] = purchaseOrder.pmpApInvoiceContactID;
			dataRow["pmpApInvoiceLocationID"] = purchaseOrder.pmpApInvoiceLocationID;
			DataRow dataRow2 = dataRow;
			DateTime? pmpApprovalDecisionDate = purchaseOrder.pmpApprovalDecisionDate;
			dataRow2["pmpApprovalDecisionDate"] = (pmpApprovalDecisionDate.HasValue ? ((object)pmpApprovalDecisionDate.GetValueOrDefault()) : dataRow["pmpApprovalDecisionDate"]);
			DataRow dataRow3 = dataRow;
			pmpApprovalDecisionDate = purchaseOrder.pmpApprovalRequestDate;
			dataRow3["pmpApprovalRequestDate"] = (pmpApprovalDecisionDate.HasValue ? ((object)pmpApprovalDecisionDate.GetValueOrDefault()) : dataRow["pmpApprovalRequestDate"]);
			dataRow["pmpBuyerEmployeeID"] = purchaseOrder.pmpBuyerEmployeeID;
			DataRow dataRow4 = dataRow;
			pmpApprovalDecisionDate = purchaseOrder.pmpClosedDate;
			dataRow4["pmpClosedDate"] = (pmpApprovalDecisionDate.HasValue ? ((object)pmpApprovalDecisionDate.GetValueOrDefault()) : dataRow["pmpClosedDate"]);
			dataRow["pmpCurrencyRateID"] = purchaseOrder.pmpCurrencyRateID;
			dataRow["pmpDocuments"] = purchaseOrder.pmpDocuments ?? dataRow["pmpDocuments"];
			dataRow["pmpDropShipContactID"] = purchaseOrder.pmpDropShipContactID;
			dataRow["pmpDropShipLocationID"] = purchaseOrder.pmpDropShipLocationID;
			dataRow["pmpDropShipOrganizationID"] = purchaseOrder.pmpDropShipOrganizationID;
			DataRow dataRow5 = dataRow;
			pmpApprovalDecisionDate = purchaseOrder.pmpDueDate;
			dataRow5["pmpDueDate"] = (pmpApprovalDecisionDate.HasValue ? ((object)pmpApprovalDecisionDate.GetValueOrDefault()) : dataRow["pmpDueDate"]);
			dataRow["pmpExchangeRate"] = purchaseOrder.pmpExchangeRate;
			dataRow["pmpFreeOnBoardDescription"] = purchaseOrder.pmpFreeOnBoardDescription;
			DataRow dataRow6 = dataRow;
			pmpApprovalDecisionDate = purchaseOrder.pmpIntraCompanyPostedDate;
			dataRow6["pmpIntraCompanyPostedDate"] = (pmpApprovalDecisionDate.HasValue ? ((object)pmpApprovalDecisionDate.GetValueOrDefault()) : dataRow["pmpIntraCompanyPostedDate"]);
			dataRow["pmpClosed"] = purchaseOrder.pmpClosed;
			dataRow["pmpCustomRate"] = purchaseOrder.pmpCustomRate;
			dataRow["pmpIntraCompany"] = purchaseOrder.pmpIntraCompany;
			dataRow["pmpIntraCompanyPosted"] = purchaseOrder.pmpIntraCompanyPosted;
			dataRow["pmpReadyToPrint"] = purchaseOrder.pmpReadyToPrint;
			dataRow["pmpNextApprovalEmployeeID"] = purchaseOrder.pmpNextApprovalEmployeeID;
			dataRow["pmpOrderCommentsRTF"] = purchaseOrder.pmpOrderCommentsRTF ?? dataRow["pmpOrderCommentsRTF"];
			dataRow["pmpOrderCommentsText"] = purchaseOrder.pmpOrderCommentsText ?? dataRow["pmpOrderCommentsText"];
			DataRow dataRow7 = dataRow;
			pmpApprovalDecisionDate = purchaseOrder.pmpOrderDate;
			dataRow7["pmpOrderDate"] = (pmpApprovalDecisionDate.HasValue ? ((object)pmpApprovalDecisionDate.GetValueOrDefault()) : dataRow["pmpOrderDate"]);
			dataRow["pmpOrderSubtotalBase"] = purchaseOrder.pmpOrderSubtotalBase;
			dataRow["pmpOrderSubtotalForeign"] = purchaseOrder.pmpOrderSubtotalForeign;
			dataRow["pmpOrderTaxAmountBase"] = purchaseOrder.pmpOrderTaxAmountBase;
			dataRow["pmpOrderTaxAmountForeign"] = purchaseOrder.pmpOrderTaxAmountForeign;
			dataRow["pmpOrderTotalBase"] = purchaseOrder.pmpOrderTotalBase;
			dataRow["pmpOrderTotalForeign"] = purchaseOrder.pmpOrderTotalForeign;
			dataRow["pmpPaymentTermID"] = purchaseOrder.pmpPaymentTermID;
			dataRow["pmpPlantDepartmentID"] = purchaseOrder.pmpPlantDepartmentID;
			dataRow["pmpPlantID"] = purchaseOrder.pmpPlantID;
			dataRow["pmpProjectID"] = purchaseOrder.pmpProjectID;
			dataRow["pmpPurchaseContactID"] = purchaseOrder.pmpPurchaseContactID;
			dataRow["pmpPurchaseLocationID"] = purchaseOrder.pmpPurchaseLocationID;
			dataRow["pmpShippingMethodID"] = purchaseOrder.pmpShippingMethodID;
			dataRow["pmpStandardMessageID"] = purchaseOrder.pmpStandardMessageID;
			dataRow["pmpStatus"] = purchaseOrder.pmpStatus;
			dataRow["pmpSupplierOrganizationID"] = purchaseOrder.pmpSupplierOrganizationID;
			if (purchaseOrder.CustomFields != null && purchaseOrder.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in purchaseOrder.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PurchaseOrder [{purchaseOrder.pmpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PurchaseOrder [{purchaseOrder.pmpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
