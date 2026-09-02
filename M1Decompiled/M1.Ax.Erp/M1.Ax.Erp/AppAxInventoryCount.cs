using System;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("InventoryCount")]
[ComVisible(true)]
public class AppAxInventoryCount : IDisposable
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxInventoryCount(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public bool Generate(object countID)
	{
		return new InventoryCount().Generate(_Database, Convert.ToInt32(countID));
	}

	public bool InventoryCountPeriodCheck(M1BindingSource bindingSource)
	{
		return new InventoryCount().InventoryCountPeriodCheck(bindingSource);
	}

	public void PostInventoryCount(M1BindingSource bindingsource)
	{
		new InventoryCount().PostInventoryCount(bindingsource);
	}

	public string PostInventoryCountCheck(M1BindingSource bindingsource)
	{
		return new InventoryCount().PostInventoryCountCheck(bindingsource);
	}

	public string PostInventoryCountInactiveBinsCheck(M1BindingSource bindingSource)
	{
		return new InventoryCount().PostInventoryCountInactiveBinsCheck(bindingSource);
	}

	public void Dispose()
	{
		provider = null;
	}
}
