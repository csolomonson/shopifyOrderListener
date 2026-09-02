using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.Utilities;
using M1.Core;

namespace M1.API.Repositories.Core;

public class InvoiceRepository : APIBaseRepository, IInvoiceRepository, IAPIBaseRepository, IDisposable
{
	private readonly string ARINVOICELINES_GET_INVOICE_LIST_FORSALESORDER = "SELECT DISTINCT ARInvoiceLines.arlARInvoiceID,ARInvoices.arpPostedToGL,ARInvoices.arpEDITransferred \r\n                                                                      FROM ARInvoiceLines INNER JOIN ARInvoices ON \r\n                                                                      ARInvoiceLines.arlARInvoiceID = ARInvoices.arpARInvoiceID \r\n                                                                      WHERE ((ARInvoiceLines.arlSalesOrderID = @p1) AND (ARInvoiceLines.arlInvoiceQuantity > 0))";

	public InvoiceRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public InvoiceRepository(M1Database database)
	{
		base.M1database = database;
	}

	public Task<bool> DoesInvoiceExists(string invoiceId)
	{
		InitializeParameterLists();
		base.filterList.Add("arpARInvoiceID|C", invoiceId);
		base.selectList.Add("arpARInvoiceID");
		return Task.FromResult(GetAsObject("ARInvoices", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> IsInvoicePostedToGL(string invoiceId)
	{
		InitializeParameterLists();
		base.filterList.Add("arpARInvoiceID|C", invoiceId);
		base.filterList.Add("arpPostedToGL|C", true);
		base.selectList.Add("arpARInvoiceID");
		return Task.FromResult(GetAsObject("ARInvoices", base.filterList, base.selectList, null, null) != null);
	}

	public Task<IDictionary<string, ARInvoiceDto>> GetInvoicesListForSalesOrder(string salesOrderId)
	{
		Dictionary<string, ARInvoiceDto> dictionary = new Dictionary<string, ARInvoiceDto>();
		ARInvoiceDto aRInvoiceDto = null;
		InitializeParameterLists();
		base.filterList.Add("@p1", salesOrderId);
		using (DataTable dataTable = GetAsDataTable(ARINVOICELINES_GET_INVOICE_LIST_FORSALESORDER, base.filterList, null))
		{
			foreach (DataRow row in dataTable.Rows)
			{
				aRInvoiceDto = new ARInvoiceDto
				{
					ARInvoiceID = row["arlARInvoiceID"].ToString().Trim(),
					PostedToGL = Convert.ToBoolean(row["arpPostedToGL"] ?? ((object)false)),
					EDITransferred = Convert.ToBoolean(row["arpEDITransferred"] ?? ((object)false))
				};
				dictionary.Add(row["arlARInvoiceID"].ToString(), aRInvoiceDto);
			}
		}
		return Task.FromResult((IDictionary<string, ARInvoiceDto>)dictionary);
	}

	public new void Dispose()
	{
		base.Dispose(disposing: true);
	}
}
