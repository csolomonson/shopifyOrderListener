using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("RMAReceipt")]
[ComVisible(true)]
public class AppAxRMAReceipt : IDisposable
{
	private IServiceProvider provider;

	private readonly M1Database _Database;

	public AppAxRMAReceipt(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public void PostRMAReceipt(M1BindingSource bindingsource)
	{
		new RMAReceipt().PostRMAReceipt(bindingsource);
	}

	public bool RMAReceiptPeriodCheck(M1BindingSource bindingSource)
	{
		return new RMAReceipt().RMAReceiptPeriodCheck(bindingSource);
	}

	public bool RMAReceiptPostCheck(M1BindingSource bindingSource)
	{
		return new RMAReceipt().RMAReceiptPostCheck(bindingSource);
	}

	public bool GetMessageForInactivePartBins(M1BindingSource bindingSource, out string msg)
	{
		return new RMAReceipt().GetMessageForInactivePartBins(bindingSource.Database, bindingSource.CurrentAsDataRow, out msg);
	}

	public decimal GetTotalComponentsCost(M1BindingSource bindingSource, DataRow currentRow)
	{
		return new RMAReceipt().GetTotalComponentsCost(bindingSource, currentRow);
	}

	public PartCost GetPartCostObjectForRmaReceipt(SqlTransaction transaction, DataRow currentDataRow)
	{
		return new RMAReceipt().GetPartCostObjectForRmaReceipt(_Database, transaction, currentDataRow);
	}

	public string CheckRMAReceiptForZeroDollarTotals(M1BindingSource bindingSource)
	{
		return new RMAReceipt().CheckRMAReceiptForZeroDollarTotals(bindingSource);
	}

	public string VerifyQuantityForInactiveBins(M1BindingSource bindingSource, string receiptID)
	{
		return new RMAReceipt().VerifyQuantityForInactiveBins(bindingSource.Database, receiptID);
	}

	public void Dispose()
	{
		provider = null;
	}
}
