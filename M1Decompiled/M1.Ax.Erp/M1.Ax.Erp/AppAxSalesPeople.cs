using System;
using System.Data;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("SalesPeople")]
[ComVisible(true)]
public class AppAxSalesPeople : IDisposable
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxSalesPeople(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public bool AddEmployee(M1BindingSource bindingSource, string employeeID, decimal percent)
	{
		return new SalesPeople().AddEmployee(bindingSource, employeeID, percent);
	}

	public void ClearEmployees(M1BindingSource bindingSource, DataRow parentRow)
	{
		new SalesPeople().ClearEmployees(bindingSource, parentRow);
	}

	public void Dispose()
	{
		_Database = null;
		provider = null;
	}
}
