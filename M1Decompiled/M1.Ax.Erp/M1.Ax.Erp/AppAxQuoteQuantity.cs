using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("QuoteQuantity")]
[ComVisible(true)]
public class AppAxQuoteQuantity : IDisposable
{
	private IServiceProvider provider;

	private M1Database database;

	private QuoteQuantity quoteFunc;

	public AppAxQuoteQuantity(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	private QuoteQuantity getRef()
	{
		if (quoteFunc == null)
		{
			quoteFunc = new QuoteQuantity();
		}
		return quoteFunc;
	}

	public List<QuoteAssemblyTotals> CalculateUsingCurrentQty(object row, object transaction)
	{
		return getRef().CalculateUsingCurrentQty(database, (DataRow)row, (SqlTransaction)transaction);
	}

	public decimal SetPriceForQuantity(object bindingSource, object row)
	{
		return getRef().SetPriceForQuantity((M1BindingSource)bindingSource, (DataRow)row);
	}

	public decimal GetQuoteCommissionRate(object bindingSource)
	{
		return getRef().GetQuoteCommissionRate((M1BindingSource)bindingSource);
	}

	public void Dispose()
	{
		quoteFunc = null;
		database = null;
		provider = null;
	}
}
