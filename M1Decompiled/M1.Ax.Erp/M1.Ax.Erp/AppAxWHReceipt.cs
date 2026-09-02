using System;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("WHReceipt")]
[ComVisible(true)]
public class AppAxWHReceipt
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxWHReceipt(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public bool WHReceiptPeriodCheck(M1BindingSource bindingSource)
	{
		return new WHReceipt().WHReceiptPeriodCheck(bindingSource);
	}

	public bool PostWHReceiptCheck(M1BindingSource bindingsource)
	{
		return new WHReceipt().PostWHReceiptCheck(bindingsource);
	}

	public void PostWHReceipt(M1BindingSource bindingsource)
	{
		new WHReceipt().PostWHReceipt(bindingsource);
	}
}
