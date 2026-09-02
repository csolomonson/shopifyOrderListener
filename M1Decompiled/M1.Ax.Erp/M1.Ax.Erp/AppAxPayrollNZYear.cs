using System;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("PayrollNZYear")]
[ComVisible(true)]
public class AppAxPayrollNZYear : IDisposable
{
	private IServiceProvider provider;

	public AppAxPayrollNZYear(IServiceProvider parentProvider)
	{
		provider = parentProvider;
	}

	public void ExportIR348(int year, string plant, int line, string fileName)
	{
		new PayrollNZYear().ExportIR348((M1Database)provider.GetService(typeof(M1Database)), year, plant, line, fileName);
	}

	public void GenerateSchedule(M1BindingSource bindingSource)
	{
		new PayrollNZYear().GenerateSchedule(bindingSource);
	}

	public void Dispose()
	{
		provider = null;
	}
}
