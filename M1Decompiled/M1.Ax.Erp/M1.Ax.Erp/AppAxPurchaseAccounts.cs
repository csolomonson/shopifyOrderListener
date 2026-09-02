using System;
using System.Data;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("PurchaseAccounts")]
[ComVisible(true)]
public class AppAxPurchaseAccounts
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxPurchaseAccounts(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public bool RefreshPurchaseOrderLineAccounts(M1BindingSource m1BindingSource, DataRow row)
	{
		return new PurchaseAccounts().RefreshPurchaseOrderLineAccounts(m1BindingSource, row);
	}

	public void RecalculateExpenseAmounts(M1BindingSource bindingSource)
	{
		new PurchaseAccounts().RecalculateExpenseAmounts(bindingSource);
	}

	public bool AllowPurchaseExpenseAccounts(string purchaseType, string partID)
	{
		return new PurchaseAccounts().AllowPurchaseExpenseAccounts(_Database, purchaseType, partID);
	}
}
