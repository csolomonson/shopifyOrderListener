using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("AR")]
[ComVisible(true)]
public class AppAxAR
{
	private IServiceProvider provider;

	private M1Database _Database;

	public bool DisableTaxFields => _Database.Props("FN").Field<bool>("xafARDisableTaxFields");

	public AppAxAR(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public object GetARPaymentInfo(object sessionID)
	{
		return new AR().GetARPaymentInfo(_Database, Convert.ToInt32(sessionID));
	}

	public int GetPaymentHeaderCount(object sessionID)
	{
		return new AR().GetPaymentHeaderCount(_Database, Convert.ToInt32(sessionID));
	}

	public void SetARInvoiceAccounts(DataRow invoiceRow)
	{
		new AR().SetARInvoiceAccounts(_Database, invoiceRow);
	}

	public void SetARRecurringInvoiceAccounts(DataRow invoiceRow)
	{
		new AR().SetARRecurringInvoiceAccounts(_Database, invoiceRow);
	}

	public string PostInvoiceCheck(string invoiceID)
	{
		return new AR().PostInvoiceCheck(_Database, invoiceID);
	}

	public string PostInvoice(M1BindingSource bindingSource, bool fromPOS = false, bool forceNoMsg = false)
	{
		return new AR().PostInvoice(bindingSource, fromPOS, forceNoMsg);
	}

	public void RefreshTaxSubtotal(M1BindingSource bsInvoice, SqlTransaction transaction)
	{
		new AR().RefreshTaxSubtotal(_Database, bsInvoice, transaction);
	}

	public bool ARInvoicePostedCheck(object transaction, string arInvoiceID)
	{
		if (!string.IsNullOrWhiteSpace(arInvoiceID))
		{
			if (transaction == DBNull.Value)
			{
				transaction = null;
			}
			return new AR().ARInvoicePostedCheck(_Database, (SqlTransaction)transaction, arInvoiceID);
		}
		return false;
	}
}
