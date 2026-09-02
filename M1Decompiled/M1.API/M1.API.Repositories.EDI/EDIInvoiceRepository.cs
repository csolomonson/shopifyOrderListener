using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using M1.API.Repositories.Core;
using M1.API.Utilities;
using M1.Core;

namespace M1.API.Repositories.EDI;

public class EDIInvoiceRepository : InvoiceRepository, IEDIInvoiceRepository, IInvoiceRepository, IAPIBaseRepository, IDisposable
{
	private readonly string ARINVOICELINES_GET_NONEDISALESORDER_COUNT_FORINVOICE = "SELECT DISTINCT COUNT(SalesOrders.ompSalesOrderID) AS OrderCount \r\n                                                            FROM SalesOrders INNER JOIN ARInvoiceLines ON dbo.SalesOrders.ompSalesOrderID = ARInvoiceLines.arlSalesOrderID \r\n                                                            WHERE (ARInvoiceLines.arlARInvoiceID =  @p1) AND (SalesOrders.ompCreatedByEDI = 0) AND (ARInvoiceLines.arlInvoiceQuantity > 0)";

	private readonly string ARINVOICE_HEADER_UPDATE_SINGLEINVOICE_EDIFLAG = "UPDATE ARInvoices SET arpEDITransferred=@p2, arpEDITransferredDate=@p3 \r\n                                                                                        WHERE arpARInvoiceID = @p1";

	private readonly string ARINVOICE_HEADER_SELECT_SINGLEINVOICE = "SELECT ARInvoices.arpARInvoiceID, ARInvoices.arpInvoiceType, ARInvoices.arpCustomerOrganizationID, \r\n                         ARInvoices.arpShipLocationID,\r\n                         ARInvoices.arpInvoiceDate, ARInvoices.arpShippingMethodID, ARInvoices.arpInvoiceTotalForeign, ARInvoices.arpInvoiceTaxAmountForeign,\r\n                         ARInvoices.arpInvoiceSubtotalForeign, ARInvoices.arpCurrencyRateID, ARInvoices.arpExchangeRate, ARInvoices.arpARInvoiceLocationID,\r\n                         ARInvoices.arpShipContactID, ARInvoices.arpARInvoiceContactID, ARInvoices.arpShippingPaymentTypeID, ARInvoices.arpFreightTotalForeign,\r\n                         ARInvoices.arpFreightTaxAmountForeign, ARInvoices.arpFullInvoiceSubtotalForeign, ARInvoices.arpDiscountTotalForeign,\r\n                         ARInvoices.arpShipOrganizationID, ARInvoices.arpPaymentTermID, ARInvoices.arpOrderDate, ARInvoices.arpEDITransferred,\r\n                         ARInvoices.arpInvoiceBalanceForeign,ARInvoices.arpDueDate,ARInvoices.arpFreightAmountForeign,\r\n                         ARInvoices.arpPostedToGL,ARInvoices.arpFreeOnBoardDescription,ARInvoices.arpPlantID,ISNULL(ShippingMethods.xasDescription,'') as xasDescription,ISNULL(PaymentTerms.xatDescription,'') AS xatDescription \r\n                         FROM ARInvoices LEFT OUTER JOIN \r\n                         ShippingMethods ON ARInvoices.arpShippingMethodID = ShippingMethods.xasShippingMethodID LEFT OUTER JOIN \r\n                         PaymentTerms ON ARInvoices.arpPaymentTermID = PaymentTerms.xatPaymentTermID \r\n                         WHERE (ARInvoices.arpARInvoiceID = @p1)";

	private readonly string ARINVOICE_GET_INVOICE_LIST_PENDINGEDITRANSFER_FORALLCUSTOMERS = "SELECT DISTINCT ARInvoices.arpARInvoiceID, ARInvoices.arpCustomerOrganizationID FROM ARInvoices INNER JOIN Organizations ON ARInvoices.arpCustomerOrganizationID = Organizations.cmoOrganizationID WHERE (ARInvoices.arpEDITransferred = 0) AND (ARInvoices.arpPostedToGL =1) AND (Organizations.cmoEDIIntegrated=1) ORDER BY dbo.ARInvoices.arpCustomerOrganizationID";

	private readonly string ARINVOICE_GET_INVOICE_LIST_PENDINGEDITRANSFER_FORCUSTOMER = "SELECT DISTINCT arpARInvoiceID FROM ARInvoices WHERE (arpCustomerOrganizationID=@p1) AND (arpEDITransferred=0 AND arpPostedToGL=1)";

	private readonly string ARINVOICE_CHECK_EDIINVOICE = "SELECT ARInvoices.arpARInvoiceID FROM ARInvoices INNER JOIN Organizations ON ARInvoices.arpCustomerOrganizationID = Organizations.cmoOrganizationID WHERE (ARInvoices.arpARInvoiceID = @p1) AND (ARInvoices.arpPostedToGL =1) AND (Organizations.cmoEDIIntegrated=1)";

	public Task<ShipmentInfo> GetShipmentDate(string shipmentId)
	{
		ShipmentInfo result = new ShipmentInfo();
		InitializeParameterLists();
		base.filterList.Add("smpShipmentID|C", shipmentId);
		base.selectList.Add("smpShipDate, smpTrackingNumber");
		using (DataTable source = GetAsDataTable("Shipments", base.filterList, base.selectList, null, null))
		{
			result = (from row in source.AsEnumerable()
				select new ShipmentInfo
				{
					ShipDate = row.Field<DateTime>("smpShipDate"),
					TrackingNo = row.Field<string>("smpTrackingNumber")
				}).FirstOrDefault();
		}
		return Task.FromResult(result);
	}

	public Task<string> GetSalesOrderLineReleaseNo(string salesOrderID, short salesOrderLineID)
	{
		string result = string.Empty;
		InitializeParameterLists();
		base.filterList.Add("omlSalesOrderID|C", salesOrderID);
		base.filterList.Add("omlSalesOrderLineID", salesOrderLineID);
		base.selectList.Add("omlReleaseNumber");
		using (DataTable dataTable = GetAsDataTable("SalesOrderLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				result = dataTable?.Rows[0]?.Field<string>("omlReleaseNumber") ?? string.Empty;
			}
		}
		return Task.FromResult(result);
	}

	public EDIInvoiceRepository(APIClientContext clientContext)
		: base(clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public EDIInvoiceRepository(M1Database database)
		: base(database)
	{
		base.M1database = database;
	}

	public Task<bool> DoesNonEDISalesordersExist_ForInvoice(string invoiceId)
	{
		int num = 0;
		InitializeParameterLists();
		base.filterList.Add("@p1", invoiceId);
		using (DataTable dataTable = GetAsDataTable(ARINVOICELINES_GET_NONEDISALESORDER_COUNT_FORINVOICE, base.filterList, null))
		{
			num = (dataTable?.Rows?.Count).GetValueOrDefault();
		}
		return Task.FromResult(num > 0);
	}

	public Task<bool> IsEdiInvoice(string invoiceId)
	{
		int num = 0;
		InitializeParameterLists();
		base.filterList.Add("@p1", invoiceId);
		using (DataTable dataTable = GetAsDataTable(ARINVOICE_CHECK_EDIINVOICE, base.filterList, null))
		{
			num = (dataTable?.Rows?.Count).GetValueOrDefault();
		}
		return Task.FromResult(num > 0);
	}

	public Task<IList<string>> GetInvoices_PendingEDITransfer_ForCustomer(string customerOrganizationID)
	{
		List<string> result = new List<string>();
		InitializeParameterLists();
		base.filterList.Add("@p1", customerOrganizationID);
		using (DataTable source = GetAsDataTable(ARINVOICE_GET_INVOICE_LIST_PENDINGEDITRANSFER_FORCUSTOMER, base.filterList, null))
		{
			result = (from x in source.AsEnumerable()
				select x.Field<string>("arpARInvoiceID")).ToList();
		}
		return Task.FromResult((IList<string>)result);
	}

	public Task<IList<string>> GetInvoices_PendingEDITransfer_ForAllCustomers()
	{
		List<string> result = new List<string>();
		InitializeParameterLists();
		using (DataTable source = GetAsDataTable(ARINVOICE_GET_INVOICE_LIST_PENDINGEDITRANSFER_FORALLCUSTOMERS, base.filterList, null))
		{
			result = (from x in source.AsEnumerable()
				select x.Field<string>("arpARInvoiceID")).ToList();
		}
		return Task.FromResult((IList<string>)result);
	}

	public Task<bool> UpdateEdiFlag(IDictionary<string, bool> invoiceDictionary, SqlTransaction sqlTransaction)
	{
		bool result = true;
		SqlCommand sqlCommand = new SqlCommand();
		foreach (KeyValuePair<string, bool> item in invoiceDictionary)
		{
			sqlCommand = new SqlCommand(ARINVOICE_HEADER_UPDATE_SINGLEINVOICE_EDIFLAG);
			sqlCommand.Parameters.AddWithValue("@p1", item.Key);
			sqlCommand.Parameters.AddWithValue("@p2", item.Value);
			if (item.Value)
			{
				sqlCommand.Parameters.AddWithValue("@p3", DateTime.Now);
			}
			else
			{
				sqlCommand.Parameters.AddWithValue("@p3", DBNull.Value);
			}
			result = base.M1database.ExecuteCommand(sqlCommand, sqlTransaction) > 0;
		}
		sqlCommand.Dispose();
		return Task.FromResult(result);
	}

	public Task<DataTable> GetInvoiceLineInfo(string invoiceId)
	{
		InitializeParameterLists();
		base.filterList.Add("arlARInvoiceID|C", invoiceId);
		base.filterList.Add("arlInvoiceQuantity|>", 0);
		base.selectList.Add("arlARInvoiceID, arlPartID, arlUnitOfMeasure, arlPartShortDescription, arlSalesOrderID,\r\n                            arlShipmentID, arlTaxAmountForeign,arlSecondTaxAmountForeign, arlOrgPartID, arlOrderQuantity,arlInvoiceQuantity,\r\n                            arlUnitPriceForeign, arlARInvoiceLineID, arlSalesOrderLineID, arlSalesOrderDeliveryID,\r\n                            arlShipmentLineID, arlFreightAmountForeign,arlFullUnitPriceForeign, arlFullExtendedPriceForeign,\r\n                            arlExtendedDiscountForeign,arlExtendedPriceForeign, arlOrgPartShortDescription,arlCustomerPO");
		return Task.FromResult(GetAsDataTable("ARInvoiceLines", base.filterList, base.selectList, null, null));
	}

	public Task<DataTable> GetInvoiceHeaderInfo(string invoiceId)
	{
		InitializeParameterLists();
		base.filterList.Add("@p1", invoiceId);
		return Task.FromResult(GetAsDataTable(ARINVOICE_HEADER_SELECT_SINGLEINVOICE, base.filterList, null));
	}
}
