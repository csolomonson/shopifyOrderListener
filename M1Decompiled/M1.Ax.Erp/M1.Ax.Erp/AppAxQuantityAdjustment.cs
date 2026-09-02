using System;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;
using M1.ServiceCore.AxScript;

namespace M1.Ax.Erp;

[AxScript("QuantityAdjustment")]
[ComVisible(true)]
public class AppAxQuantityAdjustment : IWebAxQuantityAdjustment, IDisposable
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxQuantityAdjustment(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public void PostQuantityAdjustment(M1BindingSource bindingsource)
	{
		new QuantityAdjustment().PostQuantityAdjustment(bindingsource);
	}

	public bool QuantityAdjustmentPeriodCheck(M1BindingSource bindingSource)
	{
		return new QuantityAdjustment().QuantityAdjustmentPeriodCheck(bindingSource);
	}

	public string PostQuantityAdjustmentCheck(M1BindingSource bindingsource)
	{
		return new QuantityAdjustment().PostQuantityAdjustmentCheck(bindingsource);
	}

	public void OpenBinTransfer()
	{
		new QuantityAdjustment().OpenBinTransfer(_Database);
	}

	public void Dispose()
	{
		provider = null;
	}
}
