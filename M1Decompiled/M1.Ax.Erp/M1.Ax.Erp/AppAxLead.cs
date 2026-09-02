using System;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("Lead")]
[ComVisible(true)]
public class AppAxLead : IDisposable
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxLead(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public string CreateLead(string customerID, string locationID, string currencyID, string partID, string revisionID, decimal leadQty = 1m)
	{
		return new Lead().CreateLead(_Database, customerID, locationID, currencyID, partID, revisionID);
	}

	public void SetSalesPeople(M1BindingSource bsQuote, string orgID, string locationID)
	{
		new Lead().SetSalesPeople(_Database, bsQuote, orgID, locationID);
	}

	public void Dispose()
	{
		_Database = null;
		provider = null;
	}
}
