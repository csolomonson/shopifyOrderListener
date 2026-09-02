using System;
using System.Runtime.InteropServices;
using M1.Ax.Erp.Financials.Avalara;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp.AxScript;

[AxScript("AvalaraTax")]
[ComVisible(true)]
public class AppAxAvalaraTax : IDisposable
{
	private IServiceProvider provider;

	private M1Database _Database;

	private M1User _User;

	public AppAxAvalaraTax(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
		_User = provider.GetService(typeof(M1User)) as M1User;
	}

	public string PingInterface()
	{
		return new AvalaraTaxFunctions(_Database, _User).PingInterface();
	}

	public string ValidateAddress(M1BindingSource bindingSource)
	{
		return new AvalaraTax().ValidateAddress(bindingSource);
	}

	public string GetTax(M1BindingSource bindingSource, bool postToAvalara)
	{
		return new AvalaraTax().GetTax(bindingSource, postToAvalara);
	}

	public string PostTax(M1BindingSource bindingSource)
	{
		return new AvalaraTax().PostTax(bindingSource);
	}

	public string GetARPaymentTax(M1BindingSource bindingSource)
	{
		return new AvalaraTax().GetARPaymentTax(bindingSource);
	}

	public string PostPaymentTax(M1BindingSource bindingSource)
	{
		return new AvalaraTax().PostPaymentTax(bindingSource);
	}

	public string CancelTax(M1BindingSource bindingSource, string table, string recordID)
	{
		return new AvalaraTax().CancelTax(bindingSource, table, recordID);
	}

	public string CancelPaymentTax(M1BindingSource bindingSource, int sessionID)
	{
		return new AvalaraTax().CancelPaymentTax(bindingSource, sessionID);
	}

	public int CheckLastSuccessfulTransaction(M1BindingSource bindingSource)
	{
		return new AvalaraTax().CheckLastSuccessfulTransaction(bindingSource);
	}

	public void Dispose()
	{
		provider = null;
	}
}
