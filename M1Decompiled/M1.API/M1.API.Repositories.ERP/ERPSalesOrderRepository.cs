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

public class ERPSalesOrderRepository : APIBaseRepository, IERPSalesOrderRepository, IAPIBaseRepository, IDisposable
{
	public ERPSalesOrderRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSalesOrderExist(Guid salesOrderId)
	{
		InitializeParameterLists();
		base.filterList.Add("ompUniqueID|C", salesOrderId);
		base.selectList.Add("ompUniqueID");
		return Task.FromResult(GetAsObject("SalesOrders", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSalesOrderInformationDto>> GetAllSalesOrders(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSalesOrderInformationDto> collection = new List<ERPSalesOrderInformationDto>();
		InitializeParameterLists();
		string[] array = new string[81]
		{
			"ompApprovalDecisionDate", "ompApprovalRequestDate", "ompArInvoiceContactID", "ompArInvoiceLocationID", "ompCallID", "ompClosedDate", "ompSalesOrderID", "ompCreatedBy", "ompCreatedDate", "ompCurrencyRateID",
			"ompCustomerOrganizationID", "ompCustomerPo", "ompDepositAmountBase", "ompDepositAmountForeign", "ompDepositPercent", "ompDiscountTotalBase", "ompDiscountTotalForeign", "ompUniqueID", "ompExchangeRate", "ompFedEx3rdPartyLocationID",
			"ompFedEx3rdPartyOrganizationID", "ompFedExAccountNumber", "ompFedExBillingOption", "ompFreeOnBoardDescription", "ompFreightAmountBase", "ompFreightAmountForeign", "ompFreightSubtotalBase", "ompFreightSubtotalForeign", "ompFreightTaxAmountBase", "ompFreightTaxAmountForeign",
			"ompFreightTaxCodeID", "ompFreightTotalBase", "ompFreightTotalForeign", "ompFullOrderSubtotalBase", "ompFullOrderSubtotalForeign", "ompAvalaraTaxCalculated", "ompClosed", "ompCreatedByEdi", "ompCustomRate", "ompDeposit",
			"ompDepositCreated", "ompReadyToPrint", "ompNextApprovalEmployeeID", "ompOrderCommentsRTF", "ompOrderCommentsText", "ompOrderDate", "ompOrderSubtotalBase", "ompOrderSubTotalForeign", "ompOrderTaxAmountBase", "ompOrderTaxAmountForeign",
			"ompOrderTotalBase", "ompOrderTotalForeign", "ompPaymentTermID", "ompPlantDepartmentID", "ompPlantID", "ompProjectID", "ompQuoteContactID", "ompQuoteLocationID", "ompRequestedShipDate", "ompResellerContactID",
			"ompResellerLocationID", "ompResellerOrganizationID", "ompRowVersion", "ompSecondFreightTaxAmtBase", "ompSecondFreightTaxAmtForeign", "ompSecondFreightTaxCodeID", "ompShipContactID", "ompShipLocationID", "ompShipOrganizationID", "ompShippingMethodID",
			"ompShippingPaymentTypeID", "ompSplitPercentTotal", "ompStandardMessageID", "ompStatus", "ompTaxSubtotalBase", "ompTaxSubtotalForeign", "ompTotalOrderWeight", "ompUps3rdPartyLocationID", "ompUps3rdPartyOrganizationID", "ompUpsAccountNumber",
			"ompUpsBillingOption"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("SalesOrders");
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
		using (DataTable dataTable = GetAsDataTable("SalesOrders", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSalesOrderInformationDto eRPSalesOrderInformationDto = new ERPSalesOrderInformationDto();
				eRPSalesOrderInformationDto.ompApprovalDecisionDate = dataTable.Rows[i].Field<DateTime?>("ompApprovalDecisionDate");
				eRPSalesOrderInformationDto.ompApprovalRequestDate = dataTable.Rows[i].Field<DateTime?>("ompApprovalRequestDate");
				eRPSalesOrderInformationDto.ompArInvoiceContactID = dataTable.Rows[i].Field<string>("ompArInvoiceContactID");
				eRPSalesOrderInformationDto.ompArInvoiceLocationID = dataTable.Rows[i].Field<string>("ompArInvoiceLocationID");
				eRPSalesOrderInformationDto.ompCallID = dataTable.Rows[i].Field<string>("ompCallID");
				eRPSalesOrderInformationDto.ompClosedDate = dataTable.Rows[i].Field<DateTime?>("ompClosedDate");
				eRPSalesOrderInformationDto.ompSalesOrderID = dataTable.Rows[i].Field<string>("ompSalesOrderID");
				eRPSalesOrderInformationDto.ompCreatedBy = dataTable.Rows[i].Field<string>("ompCreatedBy");
				eRPSalesOrderInformationDto.ompCreatedDate = dataTable.Rows[i].Field<DateTime?>("ompCreatedDate");
				eRPSalesOrderInformationDto.ompCurrencyRateID = dataTable.Rows[i].Field<string>("ompCurrencyRateID");
				eRPSalesOrderInformationDto.ompCustomerOrganizationID = dataTable.Rows[i].Field<string>("ompCustomerOrganizationID");
				eRPSalesOrderInformationDto.ompCustomerPo = dataTable.Rows[i].Field<string>("ompCustomerPo");
				eRPSalesOrderInformationDto.ompDepositAmountBase = dataTable.Rows[i].Field<decimal>("ompDepositAmountBase");
				eRPSalesOrderInformationDto.ompDepositAmountForeign = dataTable.Rows[i].Field<decimal>("ompDepositAmountForeign");
				eRPSalesOrderInformationDto.ompDepositPercent = dataTable.Rows[i].Field<decimal>("ompDepositPercent");
				eRPSalesOrderInformationDto.ompDiscountTotalBase = dataTable.Rows[i].Field<decimal>("ompDiscountTotalBase");
				eRPSalesOrderInformationDto.ompDiscountTotalForeign = dataTable.Rows[i].Field<decimal>("ompDiscountTotalForeign");
				eRPSalesOrderInformationDto.ompUniqueID = dataTable.Rows[i].Field<Guid>("ompUniqueID");
				eRPSalesOrderInformationDto.ompExchangeRate = dataTable.Rows[i].Field<decimal>("ompExchangeRate");
				eRPSalesOrderInformationDto.ompFedEx3rdPartyLocationID = dataTable.Rows[i].Field<string>("ompFedEx3rdPartyLocationID");
				eRPSalesOrderInformationDto.ompFedEx3rdPartyOrganizationID = dataTable.Rows[i].Field<string>("ompFedEx3rdPartyOrganizationID");
				eRPSalesOrderInformationDto.ompFedExAccountNumber = dataTable.Rows[i].Field<string>("ompFedExAccountNumber");
				eRPSalesOrderInformationDto.ompFedExBillingOption = dataTable.Rows[i].Field<string>("ompFedExBillingOption");
				eRPSalesOrderInformationDto.ompFreeOnBoardDescription = dataTable.Rows[i].Field<string>("ompFreeOnBoardDescription");
				eRPSalesOrderInformationDto.ompFreightAmountBase = dataTable.Rows[i].Field<decimal>("ompFreightAmountBase");
				eRPSalesOrderInformationDto.ompFreightAmountForeign = dataTable.Rows[i].Field<decimal>("ompFreightAmountForeign");
				eRPSalesOrderInformationDto.ompFreightSubtotalBase = dataTable.Rows[i].Field<decimal>("ompFreightSubtotalBase");
				eRPSalesOrderInformationDto.ompFreightSubtotalForeign = dataTable.Rows[i].Field<decimal>("ompFreightSubtotalForeign");
				eRPSalesOrderInformationDto.ompFreightTaxAmountBase = dataTable.Rows[i].Field<decimal>("ompFreightTaxAmountBase");
				eRPSalesOrderInformationDto.ompFreightTaxAmountForeign = dataTable.Rows[i].Field<decimal>("ompFreightTaxAmountForeign");
				eRPSalesOrderInformationDto.ompFreightTaxCodeID = dataTable.Rows[i].Field<string>("ompFreightTaxCodeID");
				eRPSalesOrderInformationDto.ompFreightTotalBase = dataTable.Rows[i].Field<decimal>("ompFreightTotalBase");
				eRPSalesOrderInformationDto.ompFreightTotalForeign = dataTable.Rows[i].Field<decimal>("ompFreightTotalForeign");
				eRPSalesOrderInformationDto.ompFullOrderSubtotalBase = dataTable.Rows[i].Field<decimal>("ompFullOrderSubtotalBase");
				eRPSalesOrderInformationDto.ompFullOrderSubtotalForeign = dataTable.Rows[i].Field<decimal>("ompFullOrderSubtotalForeign");
				eRPSalesOrderInformationDto.ompAvalaraTaxCalculated = dataTable.Rows[i].Field<bool>("ompAvalaraTaxCalculated");
				eRPSalesOrderInformationDto.ompClosed = dataTable.Rows[i].Field<bool>("ompClosed");
				eRPSalesOrderInformationDto.ompCreatedByEdi = dataTable.Rows[i].Field<bool>("ompCreatedByEdi");
				eRPSalesOrderInformationDto.ompCustomRate = dataTable.Rows[i].Field<bool>("ompCustomRate");
				eRPSalesOrderInformationDto.ompDeposit = dataTable.Rows[i].Field<bool>("ompDeposit");
				eRPSalesOrderInformationDto.ompDepositCreated = dataTable.Rows[i].Field<bool>("ompDepositCreated");
				eRPSalesOrderInformationDto.ompReadyToPrint = dataTable.Rows[i].Field<bool>("ompReadyToPrint");
				eRPSalesOrderInformationDto.ompNextApprovalEmployeeID = dataTable.Rows[i].Field<string>("ompNextApprovalEmployeeID");
				eRPSalesOrderInformationDto.ompOrderCommentsRTF = dataTable.Rows[i].Field<string>("ompOrderCommentsRTF");
				eRPSalesOrderInformationDto.ompOrderCommentsText = dataTable.Rows[i].Field<string>("ompOrderCommentsText");
				eRPSalesOrderInformationDto.ompOrderDate = dataTable.Rows[i].Field<DateTime?>("ompOrderDate");
				eRPSalesOrderInformationDto.ompOrderSubtotalBase = dataTable.Rows[i].Field<decimal>("ompOrderSubtotalBase");
				eRPSalesOrderInformationDto.ompOrderSubTotalForeign = dataTable.Rows[i].Field<decimal>("ompOrderSubTotalForeign");
				eRPSalesOrderInformationDto.ompOrderTaxAmountBase = dataTable.Rows[i].Field<decimal>("ompOrderTaxAmountBase");
				eRPSalesOrderInformationDto.ompOrderTaxAmountForeign = dataTable.Rows[i].Field<decimal>("ompOrderTaxAmountForeign");
				eRPSalesOrderInformationDto.ompOrderTotalBase = dataTable.Rows[i].Field<decimal>("ompOrderTotalBase");
				eRPSalesOrderInformationDto.ompOrderTotalForeign = dataTable.Rows[i].Field<decimal>("ompOrderTotalForeign");
				eRPSalesOrderInformationDto.ompPaymentTermID = dataTable.Rows[i].Field<string>("ompPaymentTermID");
				eRPSalesOrderInformationDto.ompPlantDepartmentID = dataTable.Rows[i].Field<string>("ompPlantDepartmentID");
				eRPSalesOrderInformationDto.ompPlantID = dataTable.Rows[i].Field<string>("ompPlantID");
				eRPSalesOrderInformationDto.ompProjectID = dataTable.Rows[i].Field<string>("ompProjectID");
				eRPSalesOrderInformationDto.ompQuoteContactID = dataTable.Rows[i].Field<string>("ompQuoteContactID");
				eRPSalesOrderInformationDto.ompQuoteLocationID = dataTable.Rows[i].Field<string>("ompQuoteLocationID");
				eRPSalesOrderInformationDto.ompRequestedShipDate = dataTable.Rows[i].Field<DateTime?>("ompRequestedShipDate");
				eRPSalesOrderInformationDto.ompResellerContactID = dataTable.Rows[i].Field<string>("ompResellerContactID");
				eRPSalesOrderInformationDto.ompResellerLocationID = dataTable.Rows[i].Field<string>("ompResellerLocationID");
				eRPSalesOrderInformationDto.ompResellerOrganizationID = dataTable.Rows[i].Field<string>("ompResellerOrganizationID");
				eRPSalesOrderInformationDto.ompRowVersion = dataTable.Rows[i].Field<byte[]>("ompRowVersion");
				eRPSalesOrderInformationDto.ompSecondFreightTaxAmtBase = dataTable.Rows[i].Field<decimal>("ompSecondFreightTaxAmtBase");
				eRPSalesOrderInformationDto.ompSecondFreightTaxAmtForeign = dataTable.Rows[i].Field<decimal>("ompSecondFreightTaxAmtForeign");
				eRPSalesOrderInformationDto.ompSecondFreightTaxCodeID = dataTable.Rows[i].Field<string>("ompSecondFreightTaxCodeID");
				eRPSalesOrderInformationDto.ompShipContactID = dataTable.Rows[i].Field<string>("ompShipContactID");
				eRPSalesOrderInformationDto.ompShipLocationID = dataTable.Rows[i].Field<string>("ompShipLocationID");
				eRPSalesOrderInformationDto.ompShipOrganizationID = dataTable.Rows[i].Field<string>("ompShipOrganizationID");
				eRPSalesOrderInformationDto.ompShippingMethodID = dataTable.Rows[i].Field<string>("ompShippingMethodID");
				eRPSalesOrderInformationDto.ompShippingPaymentTypeID = dataTable.Rows[i].Field<string>("ompShippingPaymentTypeID");
				eRPSalesOrderInformationDto.ompSplitPercentTotal = dataTable.Rows[i].Field<decimal>("ompSplitPercentTotal");
				eRPSalesOrderInformationDto.ompStandardMessageID = dataTable.Rows[i].Field<string>("ompStandardMessageID");
				eRPSalesOrderInformationDto.ompStatus = dataTable.Rows[i].Field<byte>("ompStatus");
				eRPSalesOrderInformationDto.ompTaxSubtotalBase = dataTable.Rows[i].Field<decimal>("ompTaxSubtotalBase");
				eRPSalesOrderInformationDto.ompTaxSubtotalForeign = dataTable.Rows[i].Field<decimal>("ompTaxSubtotalForeign");
				eRPSalesOrderInformationDto.ompTotalOrderWeight = dataTable.Rows[i].Field<decimal>("ompTotalOrderWeight");
				eRPSalesOrderInformationDto.ompUps3rdPartyLocationID = dataTable.Rows[i].Field<string>("ompUps3rdPartyLocationID");
				eRPSalesOrderInformationDto.ompUps3rdPartyOrganizationID = dataTable.Rows[i].Field<string>("ompUps3rdPartyOrganizationID");
				eRPSalesOrderInformationDto.ompUpsAccountNumber = dataTable.Rows[i].Field<string>("ompUpsAccountNumber");
				eRPSalesOrderInformationDto.ompUpsBillingOption = dataTable.Rows[i].Field<string>("ompUpsBillingOption");
				eRPSalesOrderInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSalesOrderInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSalesOrderInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSalesOrderInformationDto> GetSalesOrder(Guid salesOrderId)
	{
		ERPSalesOrderInformationDto eRPSalesOrderInformationDto = new ERPSalesOrderInformationDto();
		InitializeParameterLists();
		string[] collection = new string[81]
		{
			"ompApprovalDecisionDate", "ompApprovalRequestDate", "ompArInvoiceContactID", "ompArInvoiceLocationID", "ompCallID", "ompClosedDate", "ompSalesOrderID", "ompCreatedBy", "ompCreatedDate", "ompCurrencyRateID",
			"ompCustomerOrganizationID", "ompCustomerPo", "ompDepositAmountBase", "ompDepositAmountForeign", "ompDepositPercent", "ompDiscountTotalBase", "ompDiscountTotalForeign", "ompUniqueID", "ompExchangeRate", "ompFedEx3rdPartyLocationID",
			"ompFedEx3rdPartyOrganizationID", "ompFedExAccountNumber", "ompFedExBillingOption", "ompFreeOnBoardDescription", "ompFreightAmountBase", "ompFreightAmountForeign", "ompFreightSubtotalBase", "ompFreightSubtotalForeign", "ompFreightTaxAmountBase", "ompFreightTaxAmountForeign",
			"ompFreightTaxCodeID", "ompFreightTotalBase", "ompFreightTotalForeign", "ompFullOrderSubtotalBase", "ompFullOrderSubtotalForeign", "ompAvalaraTaxCalculated", "ompClosed", "ompCreatedByEdi", "ompCustomRate", "ompDeposit",
			"ompDepositCreated", "ompReadyToPrint", "ompNextApprovalEmployeeID", "ompOrderCommentsRTF", "ompOrderCommentsText", "ompOrderDate", "ompOrderSubtotalBase", "ompOrderSubTotalForeign", "ompOrderTaxAmountBase", "ompOrderTaxAmountForeign",
			"ompOrderTotalBase", "ompOrderTotalForeign", "ompPaymentTermID", "ompPlantDepartmentID", "ompPlantID", "ompProjectID", "ompQuoteContactID", "ompQuoteLocationID", "ompRequestedShipDate", "ompResellerContactID",
			"ompResellerLocationID", "ompResellerOrganizationID", "ompRowVersion", "ompSecondFreightTaxAmtBase", "ompSecondFreightTaxAmtForeign", "ompSecondFreightTaxCodeID", "ompShipContactID", "ompShipLocationID", "ompShipOrganizationID", "ompShippingMethodID",
			"ompShippingPaymentTypeID", "ompSplitPercentTotal", "ompStandardMessageID", "ompStatus", "ompTaxSubtotalBase", "ompTaxSubtotalForeign", "ompTotalOrderWeight", "ompUps3rdPartyLocationID", "ompUps3rdPartyOrganizationID", "ompUpsAccountNumber",
			"ompUpsBillingOption"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("ompUniqueID|C", salesOrderId);
		AddCustomFieldsToSelectList("SalesOrders");
		using (DataTable dataTable = GetAsDataTable("SalesOrders", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSalesOrderInformationDto);
			}
			eRPSalesOrderInformationDto.ompApprovalDecisionDate = dataTable.Rows[0].Field<DateTime?>("ompApprovalDecisionDate");
			eRPSalesOrderInformationDto.ompApprovalRequestDate = dataTable.Rows[0].Field<DateTime?>("ompApprovalRequestDate");
			eRPSalesOrderInformationDto.ompArInvoiceContactID = dataTable.Rows[0].Field<string>("ompArInvoiceContactID");
			eRPSalesOrderInformationDto.ompArInvoiceLocationID = dataTable.Rows[0].Field<string>("ompArInvoiceLocationID");
			eRPSalesOrderInformationDto.ompCallID = dataTable.Rows[0].Field<string>("ompCallID");
			eRPSalesOrderInformationDto.ompClosedDate = dataTable.Rows[0].Field<DateTime?>("ompClosedDate");
			eRPSalesOrderInformationDto.ompSalesOrderID = dataTable.Rows[0].Field<string>("ompSalesOrderID");
			eRPSalesOrderInformationDto.ompCreatedBy = dataTable.Rows[0].Field<string>("ompCreatedBy");
			eRPSalesOrderInformationDto.ompCreatedDate = dataTable.Rows[0].Field<DateTime?>("ompCreatedDate");
			eRPSalesOrderInformationDto.ompCurrencyRateID = dataTable.Rows[0].Field<string>("ompCurrencyRateID");
			eRPSalesOrderInformationDto.ompCustomerOrganizationID = dataTable.Rows[0].Field<string>("ompCustomerOrganizationID");
			eRPSalesOrderInformationDto.ompCustomerPo = dataTable.Rows[0].Field<string>("ompCustomerPo");
			eRPSalesOrderInformationDto.ompDepositAmountBase = dataTable.Rows[0].Field<decimal>("ompDepositAmountBase");
			eRPSalesOrderInformationDto.ompDepositAmountForeign = dataTable.Rows[0].Field<decimal>("ompDepositAmountForeign");
			eRPSalesOrderInformationDto.ompDepositPercent = dataTable.Rows[0].Field<decimal>("ompDepositPercent");
			eRPSalesOrderInformationDto.ompDiscountTotalBase = dataTable.Rows[0].Field<decimal>("ompDiscountTotalBase");
			eRPSalesOrderInformationDto.ompDiscountTotalForeign = dataTable.Rows[0].Field<decimal>("ompDiscountTotalForeign");
			eRPSalesOrderInformationDto.ompUniqueID = dataTable.Rows[0].Field<Guid>("ompUniqueID");
			eRPSalesOrderInformationDto.ompExchangeRate = dataTable.Rows[0].Field<decimal>("ompExchangeRate");
			eRPSalesOrderInformationDto.ompFedEx3rdPartyLocationID = dataTable.Rows[0].Field<string>("ompFedEx3rdPartyLocationID");
			eRPSalesOrderInformationDto.ompFedEx3rdPartyOrganizationID = dataTable.Rows[0].Field<string>("ompFedEx3rdPartyOrganizationID");
			eRPSalesOrderInformationDto.ompFedExAccountNumber = dataTable.Rows[0].Field<string>("ompFedExAccountNumber");
			eRPSalesOrderInformationDto.ompFedExBillingOption = dataTable.Rows[0].Field<string>("ompFedExBillingOption");
			eRPSalesOrderInformationDto.ompFreeOnBoardDescription = dataTable.Rows[0].Field<string>("ompFreeOnBoardDescription");
			eRPSalesOrderInformationDto.ompFreightAmountBase = dataTable.Rows[0].Field<decimal>("ompFreightAmountBase");
			eRPSalesOrderInformationDto.ompFreightAmountForeign = dataTable.Rows[0].Field<decimal>("ompFreightAmountForeign");
			eRPSalesOrderInformationDto.ompFreightSubtotalBase = dataTable.Rows[0].Field<decimal>("ompFreightSubtotalBase");
			eRPSalesOrderInformationDto.ompFreightSubtotalForeign = dataTable.Rows[0].Field<decimal>("ompFreightSubtotalForeign");
			eRPSalesOrderInformationDto.ompFreightTaxAmountBase = dataTable.Rows[0].Field<decimal>("ompFreightTaxAmountBase");
			eRPSalesOrderInformationDto.ompFreightTaxAmountForeign = dataTable.Rows[0].Field<decimal>("ompFreightTaxAmountForeign");
			eRPSalesOrderInformationDto.ompFreightTaxCodeID = dataTable.Rows[0].Field<string>("ompFreightTaxCodeID");
			eRPSalesOrderInformationDto.ompFreightTotalBase = dataTable.Rows[0].Field<decimal>("ompFreightTotalBase");
			eRPSalesOrderInformationDto.ompFreightTotalForeign = dataTable.Rows[0].Field<decimal>("ompFreightTotalForeign");
			eRPSalesOrderInformationDto.ompFullOrderSubtotalBase = dataTable.Rows[0].Field<decimal>("ompFullOrderSubtotalBase");
			eRPSalesOrderInformationDto.ompFullOrderSubtotalForeign = dataTable.Rows[0].Field<decimal>("ompFullOrderSubtotalForeign");
			eRPSalesOrderInformationDto.ompAvalaraTaxCalculated = dataTable.Rows[0].Field<bool>("ompAvalaraTaxCalculated");
			eRPSalesOrderInformationDto.ompClosed = dataTable.Rows[0].Field<bool>("ompClosed");
			eRPSalesOrderInformationDto.ompCreatedByEdi = dataTable.Rows[0].Field<bool>("ompCreatedByEdi");
			eRPSalesOrderInformationDto.ompCustomRate = dataTable.Rows[0].Field<bool>("ompCustomRate");
			eRPSalesOrderInformationDto.ompDeposit = dataTable.Rows[0].Field<bool>("ompDeposit");
			eRPSalesOrderInformationDto.ompDepositCreated = dataTable.Rows[0].Field<bool>("ompDepositCreated");
			eRPSalesOrderInformationDto.ompReadyToPrint = dataTable.Rows[0].Field<bool>("ompReadyToPrint");
			eRPSalesOrderInformationDto.ompNextApprovalEmployeeID = dataTable.Rows[0].Field<string>("ompNextApprovalEmployeeID");
			eRPSalesOrderInformationDto.ompOrderCommentsRTF = dataTable.Rows[0].Field<string>("ompOrderCommentsRTF");
			eRPSalesOrderInformationDto.ompOrderCommentsText = dataTable.Rows[0].Field<string>("ompOrderCommentsText");
			eRPSalesOrderInformationDto.ompOrderDate = dataTable.Rows[0].Field<DateTime?>("ompOrderDate");
			eRPSalesOrderInformationDto.ompOrderSubtotalBase = dataTable.Rows[0].Field<decimal>("ompOrderSubtotalBase");
			eRPSalesOrderInformationDto.ompOrderSubTotalForeign = dataTable.Rows[0].Field<decimal>("ompOrderSubTotalForeign");
			eRPSalesOrderInformationDto.ompOrderTaxAmountBase = dataTable.Rows[0].Field<decimal>("ompOrderTaxAmountBase");
			eRPSalesOrderInformationDto.ompOrderTaxAmountForeign = dataTable.Rows[0].Field<decimal>("ompOrderTaxAmountForeign");
			eRPSalesOrderInformationDto.ompOrderTotalBase = dataTable.Rows[0].Field<decimal>("ompOrderTotalBase");
			eRPSalesOrderInformationDto.ompOrderTotalForeign = dataTable.Rows[0].Field<decimal>("ompOrderTotalForeign");
			eRPSalesOrderInformationDto.ompPaymentTermID = dataTable.Rows[0].Field<string>("ompPaymentTermID");
			eRPSalesOrderInformationDto.ompPlantDepartmentID = dataTable.Rows[0].Field<string>("ompPlantDepartmentID");
			eRPSalesOrderInformationDto.ompPlantID = dataTable.Rows[0].Field<string>("ompPlantID");
			eRPSalesOrderInformationDto.ompProjectID = dataTable.Rows[0].Field<string>("ompProjectID");
			eRPSalesOrderInformationDto.ompQuoteContactID = dataTable.Rows[0].Field<string>("ompQuoteContactID");
			eRPSalesOrderInformationDto.ompQuoteLocationID = dataTable.Rows[0].Field<string>("ompQuoteLocationID");
			eRPSalesOrderInformationDto.ompRequestedShipDate = dataTable.Rows[0].Field<DateTime?>("ompRequestedShipDate");
			eRPSalesOrderInformationDto.ompResellerContactID = dataTable.Rows[0].Field<string>("ompResellerContactID");
			eRPSalesOrderInformationDto.ompResellerLocationID = dataTable.Rows[0].Field<string>("ompResellerLocationID");
			eRPSalesOrderInformationDto.ompResellerOrganizationID = dataTable.Rows[0].Field<string>("ompResellerOrganizationID");
			eRPSalesOrderInformationDto.ompRowVersion = dataTable.Rows[0].Field<byte[]>("ompRowVersion");
			eRPSalesOrderInformationDto.ompSecondFreightTaxAmtBase = dataTable.Rows[0].Field<decimal>("ompSecondFreightTaxAmtBase");
			eRPSalesOrderInformationDto.ompSecondFreightTaxAmtForeign = dataTable.Rows[0].Field<decimal>("ompSecondFreightTaxAmtForeign");
			eRPSalesOrderInformationDto.ompSecondFreightTaxCodeID = dataTable.Rows[0].Field<string>("ompSecondFreightTaxCodeID");
			eRPSalesOrderInformationDto.ompShipContactID = dataTable.Rows[0].Field<string>("ompShipContactID");
			eRPSalesOrderInformationDto.ompShipLocationID = dataTable.Rows[0].Field<string>("ompShipLocationID");
			eRPSalesOrderInformationDto.ompShipOrganizationID = dataTable.Rows[0].Field<string>("ompShipOrganizationID");
			eRPSalesOrderInformationDto.ompShippingMethodID = dataTable.Rows[0].Field<string>("ompShippingMethodID");
			eRPSalesOrderInformationDto.ompShippingPaymentTypeID = dataTable.Rows[0].Field<string>("ompShippingPaymentTypeID");
			eRPSalesOrderInformationDto.ompSplitPercentTotal = dataTable.Rows[0].Field<decimal>("ompSplitPercentTotal");
			eRPSalesOrderInformationDto.ompStandardMessageID = dataTable.Rows[0].Field<string>("ompStandardMessageID");
			eRPSalesOrderInformationDto.ompStatus = dataTable.Rows[0].Field<byte>("ompStatus");
			eRPSalesOrderInformationDto.ompTaxSubtotalBase = dataTable.Rows[0].Field<decimal>("ompTaxSubtotalBase");
			eRPSalesOrderInformationDto.ompTaxSubtotalForeign = dataTable.Rows[0].Field<decimal>("ompTaxSubtotalForeign");
			eRPSalesOrderInformationDto.ompTotalOrderWeight = dataTable.Rows[0].Field<decimal>("ompTotalOrderWeight");
			eRPSalesOrderInformationDto.ompUps3rdPartyLocationID = dataTable.Rows[0].Field<string>("ompUps3rdPartyLocationID");
			eRPSalesOrderInformationDto.ompUps3rdPartyOrganizationID = dataTable.Rows[0].Field<string>("ompUps3rdPartyOrganizationID");
			eRPSalesOrderInformationDto.ompUpsAccountNumber = dataTable.Rows[0].Field<string>("ompUpsAccountNumber");
			eRPSalesOrderInformationDto.ompUpsBillingOption = dataTable.Rows[0].Field<string>("ompUpsBillingOption");
			eRPSalesOrderInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSalesOrderInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSalesOrderInformationDto);
	}

	public Task<APIValidationInfoDto> SaveSalesOrder(ERPSalesOrderDto salesOrder)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM SalesOrders WHERE ompUniqueID = " + M1Util.ConvertToLinq(salesOrder.ompUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["ompSalesOrderID"] = salesOrder.ompSalesOrderID.ToUpper();
				salesOrder.ompUniqueID = ((salesOrder.ompUniqueID == Guid.Empty) ? Guid.NewGuid() : salesOrder.ompUniqueID);
				dataRow["ompUniqueID"] = salesOrder.ompUniqueID;
				dataRow["ompCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["ompCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The SalesOrder could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (salesOrder.ompRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the SalesOrder is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["ompRowVersion"], salesOrder.ompRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the SalesOrder has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the SalesOrder again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? ompApprovalDecisionDate = salesOrder.ompApprovalDecisionDate;
			dataRow2["ompApprovalDecisionDate"] = (ompApprovalDecisionDate.HasValue ? ((object)ompApprovalDecisionDate.GetValueOrDefault()) : dataRow["ompApprovalDecisionDate"]);
			DataRow dataRow3 = dataRow;
			ompApprovalDecisionDate = salesOrder.ompApprovalRequestDate;
			dataRow3["ompApprovalRequestDate"] = (ompApprovalDecisionDate.HasValue ? ((object)ompApprovalDecisionDate.GetValueOrDefault()) : dataRow["ompApprovalRequestDate"]);
			dataRow["ompArInvoiceContactID"] = salesOrder.ompArInvoiceContactID;
			dataRow["ompArInvoiceLocationID"] = salesOrder.ompArInvoiceLocationID;
			dataRow["ompCallID"] = salesOrder.ompCallID;
			DataRow dataRow4 = dataRow;
			ompApprovalDecisionDate = salesOrder.ompClosedDate;
			dataRow4["ompClosedDate"] = (ompApprovalDecisionDate.HasValue ? ((object)ompApprovalDecisionDate.GetValueOrDefault()) : dataRow["ompClosedDate"]);
			dataRow["ompCurrencyRateID"] = salesOrder.ompCurrencyRateID;
			dataRow["ompCustomerOrganizationID"] = salesOrder.ompCustomerOrganizationID;
			dataRow["ompCustomerPo"] = salesOrder.ompCustomerPo;
			dataRow["ompDepositAmountBase"] = salesOrder.ompDepositAmountBase;
			dataRow["ompDepositAmountForeign"] = salesOrder.ompDepositAmountForeign;
			dataRow["ompDepositPercent"] = salesOrder.ompDepositPercent;
			dataRow["ompDiscountTotalBase"] = salesOrder.ompDiscountTotalBase;
			dataRow["ompDiscountTotalForeign"] = salesOrder.ompDiscountTotalForeign;
			dataRow["ompExchangeRate"] = salesOrder.ompExchangeRate;
			dataRow["ompFedEx3rdPartyLocationID"] = salesOrder.ompFedEx3rdPartyLocationID;
			dataRow["ompFedEx3rdPartyOrganizationID"] = salesOrder.ompFedEx3rdPartyOrganizationID;
			dataRow["ompFedExAccountNumber"] = salesOrder.ompFedExAccountNumber;
			dataRow["ompFedExBillingOption"] = salesOrder.ompFedExBillingOption;
			dataRow["ompFreeOnBoardDescription"] = salesOrder.ompFreeOnBoardDescription;
			dataRow["ompFreightAmountBase"] = salesOrder.ompFreightAmountBase;
			dataRow["ompFreightAmountForeign"] = salesOrder.ompFreightAmountForeign;
			dataRow["ompFreightSubtotalBase"] = salesOrder.ompFreightSubtotalBase;
			dataRow["ompFreightSubtotalForeign"] = salesOrder.ompFreightSubtotalForeign;
			dataRow["ompFreightTaxAmountBase"] = salesOrder.ompFreightTaxAmountBase;
			dataRow["ompFreightTaxAmountForeign"] = salesOrder.ompFreightTaxAmountForeign;
			dataRow["ompFreightTaxCodeID"] = salesOrder.ompFreightTaxCodeID;
			dataRow["ompFreightTotalBase"] = salesOrder.ompFreightTotalBase;
			dataRow["ompFreightTotalForeign"] = salesOrder.ompFreightTotalForeign;
			dataRow["ompFullOrderSubtotalBase"] = salesOrder.ompFullOrderSubtotalBase;
			dataRow["ompFullOrderSubtotalForeign"] = salesOrder.ompFullOrderSubtotalForeign;
			dataRow["ompAvalaraTaxCalculated"] = salesOrder.ompAvalaraTaxCalculated;
			dataRow["ompClosed"] = salesOrder.ompClosed;
			dataRow["ompCreatedByEdi"] = salesOrder.ompCreatedByEdi;
			dataRow["ompCustomRate"] = salesOrder.ompCustomRate;
			dataRow["ompDeposit"] = salesOrder.ompDeposit;
			dataRow["ompDepositCreated"] = salesOrder.ompDepositCreated;
			dataRow["ompReadyToPrint"] = salesOrder.ompReadyToPrint;
			dataRow["ompNextApprovalEmployeeID"] = salesOrder.ompNextApprovalEmployeeID;
			dataRow["ompOrderCommentsRTF"] = salesOrder.ompOrderCommentsRTF ?? dataRow["ompOrderCommentsRTF"];
			dataRow["ompOrderCommentsText"] = salesOrder.ompOrderCommentsText ?? dataRow["ompOrderCommentsText"];
			DataRow dataRow5 = dataRow;
			ompApprovalDecisionDate = salesOrder.ompOrderDate;
			dataRow5["ompOrderDate"] = (ompApprovalDecisionDate.HasValue ? ((object)ompApprovalDecisionDate.GetValueOrDefault()) : dataRow["ompOrderDate"]);
			dataRow["ompOrderSubtotalBase"] = salesOrder.ompOrderSubtotalBase;
			dataRow["ompOrderSubTotalForeign"] = salesOrder.ompOrderSubTotalForeign;
			dataRow["ompOrderTaxAmountBase"] = salesOrder.ompOrderTaxAmountBase;
			dataRow["ompOrderTaxAmountForeign"] = salesOrder.ompOrderTaxAmountForeign;
			dataRow["ompOrderTotalBase"] = salesOrder.ompOrderTotalBase;
			dataRow["ompOrderTotalForeign"] = salesOrder.ompOrderTotalForeign;
			dataRow["ompPaymentTermID"] = salesOrder.ompPaymentTermID;
			dataRow["ompPlantDepartmentID"] = salesOrder.ompPlantDepartmentID;
			dataRow["ompPlantID"] = salesOrder.ompPlantID;
			dataRow["ompProjectID"] = salesOrder.ompProjectID;
			dataRow["ompQuoteContactID"] = salesOrder.ompQuoteContactID;
			dataRow["ompQuoteLocationID"] = salesOrder.ompQuoteLocationID;
			DataRow dataRow6 = dataRow;
			ompApprovalDecisionDate = salesOrder.ompRequestedShipDate;
			dataRow6["ompRequestedShipDate"] = (ompApprovalDecisionDate.HasValue ? ((object)ompApprovalDecisionDate.GetValueOrDefault()) : dataRow["ompRequestedShipDate"]);
			dataRow["ompResellerContactID"] = salesOrder.ompResellerContactID;
			dataRow["ompResellerLocationID"] = salesOrder.ompResellerLocationID;
			dataRow["ompResellerOrganizationID"] = salesOrder.ompResellerOrganizationID;
			dataRow["ompSecondFreightTaxAmtBase"] = salesOrder.ompSecondFreightTaxAmtBase;
			dataRow["ompSecondFreightTaxAmtForeign"] = salesOrder.ompSecondFreightTaxAmtForeign;
			dataRow["ompSecondFreightTaxCodeID"] = salesOrder.ompSecondFreightTaxCodeID;
			dataRow["ompShipContactID"] = salesOrder.ompShipContactID;
			dataRow["ompShipLocationID"] = salesOrder.ompShipLocationID;
			dataRow["ompShipOrganizationID"] = salesOrder.ompShipOrganizationID;
			dataRow["ompShippingMethodID"] = salesOrder.ompShippingMethodID;
			dataRow["ompShippingPaymentTypeID"] = salesOrder.ompShippingPaymentTypeID;
			dataRow["ompSplitPercentTotal"] = salesOrder.ompSplitPercentTotal;
			dataRow["ompStandardMessageID"] = salesOrder.ompStandardMessageID;
			dataRow["ompStatus"] = salesOrder.ompStatus;
			dataRow["ompTaxSubtotalBase"] = salesOrder.ompTaxSubtotalBase;
			dataRow["ompTaxSubtotalForeign"] = salesOrder.ompTaxSubtotalForeign;
			dataRow["ompTotalOrderWeight"] = salesOrder.ompTotalOrderWeight;
			dataRow["ompUps3rdPartyLocationID"] = salesOrder.ompUps3rdPartyLocationID;
			dataRow["ompUps3rdPartyOrganizationID"] = salesOrder.ompUps3rdPartyOrganizationID;
			dataRow["ompUpsAccountNumber"] = salesOrder.ompUpsAccountNumber;
			dataRow["ompUpsBillingOption"] = salesOrder.ompUpsBillingOption;
			if (salesOrder.CustomFields != null && salesOrder.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in salesOrder.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the SalesOrder [{salesOrder.ompUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the SalesOrder [{salesOrder.ompUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
