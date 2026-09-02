using System;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("Warehouses")]
[ComVisible(true)]
public class AppAxWarehouses
{
	private IServiceProvider _provider;

	private M1Database _database;

	public AppAxWarehouses(IServiceProvider parentProvider)
	{
		_provider = parentProvider;
		_database = _provider.GetService(typeof(M1Database)) as M1Database;
	}

	public void InactivateWarehouseBins(M1BindingSource bindingSource)
	{
		new Warehouses().InactivateWarehouseBins(bindingSource);
	}
}
