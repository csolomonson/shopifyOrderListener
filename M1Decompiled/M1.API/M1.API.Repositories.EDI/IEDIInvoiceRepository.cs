using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using M1.API.Repositories.Core;

namespace M1.API.Repositories.EDI;

public interface IEDIInvoiceRepository : IInvoiceRepository, IAPIBaseRepository, IDisposable
{
	Task<bool> DoesNonEDISalesordersExist_ForInvoice(string invoiceId);

	Task<IList<string>> GetInvoices_PendingEDITransfer_ForCustomer(string customerOrganizationID);

	Task<IList<string>> GetInvoices_PendingEDITransfer_ForAllCustomers();

	Task<bool> UpdateEdiFlag(IDictionary<string, bool> invoiceDictionary, SqlTransaction sqlTransaction);

	Task<DataTable> GetInvoiceHeaderInfo(string invoiceId);

	Task<DataTable> GetInvoiceLineInfo(string invoiceId);

	Task<ShipmentInfo> GetShipmentDate(string shipmentId);

	Task<string> GetSalesOrderLineReleaseNo(string salesOrderID, short salesOrderLineID);

	Task<bool> IsEdiInvoice(string invoiceId);
}
