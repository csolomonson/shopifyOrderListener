using System;
using System.Data;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("Receipt")]
[ComVisible(true)]
public class AppAxReceipt : IDisposable
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxReceipt(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public void PostReceipt(M1BindingSource bindingsource)
	{
		new Receipts().PostReceipt(bindingsource);
	}

	public bool ReceiptPeriodCheck(M1BindingSource bindingSource)
	{
		return new Receipts().ReceiptPeriodCheck(bindingSource);
	}

	public bool ReceiptPostCheck(M1BindingSource bindingSource)
	{
		return new Receipts().ReceiptPostCheck(bindingSource);
	}

	public bool ExistsNegativeSerialLot(M1BindingSource bindingSource)
	{
		return new Receipts().NegativeSerialLotPartCheck(bindingSource);
	}

	public string GetMessageForNegativeParts(M1BindingSource bindingSource)
	{
		return new Receipts().GetMessageForNegativeParts(bindingSource);
	}

	public decimal GetTotalComponentsCost(M1BindingSource bindingSource, DataRow currentRow)
	{
		return new Receipts().GetTotalComponentsCost(bindingSource, currentRow);
	}

	public string CheckReceiptForZeroDollarTotals(M1BindingSource bindingSource)
	{
		return new Receipts().CheckReceiptForZeroDollarTotals(bindingSource);
	}

	public string VerifyQuantityForInactiveBins(M1BindingSource bindingSource, string receiptID)
	{
		return new Receipts().VerifyQuantityForInactiveBins(bindingSource.Database, receiptID);
	}

	public bool GetMessageForInactivePartBins(M1BindingSource bindingSource, out string msg)
	{
		return new Receipts().GetMessageForInactivePartBins(bindingSource.Database, bindingSource.CurrentAsDataRow, out msg);
	}

	public void Dispose()
	{
		provider = null;
	}
}
