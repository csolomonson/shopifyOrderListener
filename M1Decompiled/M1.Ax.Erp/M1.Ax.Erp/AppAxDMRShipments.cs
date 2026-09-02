using System;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("DMRShipments")]
[ComVisible(true)]
public class AppAxDMRShipments : IDisposable
{
	private IServiceProvider provider;

	public AppAxDMRShipments(IServiceProvider parentProvider)
	{
		provider = parentProvider;
	}

	public bool CheckSumOfShipQtyForDelivery(M1BindingSource bindingSource)
	{
		return new DMRShipment().CheckSumOfShipQtyForDelivery(bindingSource);
	}

	public void PostDMRShipment(M1BindingSource bindingsource)
	{
		new DMRShipment().PostDMRShipment(bindingsource);
	}

	public bool DMRShipmentPeriodCheck(M1BindingSource bindingSource)
	{
		return new DMRShipment().DMRShipmentPeriodCheck(bindingSource);
	}

	public bool PostDMRShipmentCheck(M1BindingSource bindingsource)
	{
		return new DMRShipment().PostDMRShipmentCheck(bindingsource);
	}

	public string CheckDMRShipmentForZeroDollarTotals(M1BindingSource bindingSource)
	{
		return new DMRShipment().CheckDMRShipmentForZeroDollarTotals(bindingSource);
	}

	public void Dispose()
	{
		provider = null;
	}
}
