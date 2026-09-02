using System;
using System.Data;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("Shipments")]
[ComVisible(true)]
public class AppAxShipments : IDisposable
{
	private IServiceProvider provider;

	public AppAxShipments(IServiceProvider parentProvider)
	{
		provider = parentProvider;
	}

	public string GetTrackingLink(string ShipMethodID, string TrackingNumber, string ReferenceNumber, object FromDate)
	{
		DateTime? fromDate = null;
		if (FromDate != null && FromDate != DBNull.Value)
		{
			fromDate = Convert.ToDateTime(FromDate);
		}
		return new Shipments().GetTrackingLink((M1Database)provider.GetService(typeof(M1Database)), ShipMethodID, TrackingNumber, ReferenceNumber, fromDate);
	}

	public bool CheckSumOfShipQtyForDelivery(M1BindingSource bindingSource)
	{
		return new Shipments().CheckSumOfShipQtyForDelivery(bindingSource);
	}

	public void Dispose()
	{
		provider = null;
	}

	public void GetShippingPackageRates(M1BindingSource bindingSource)
	{
		new Shipments().GetShippingPackageRates(bindingSource);
	}

	public void GetShippingPackageShipment(M1BindingSource bindingSource)
	{
		new Shipments().GetShippingPackageShipment(bindingSource);
	}

	public void GetShipmentTrackingInfo(M1BindingSource bindingSource)
	{
		new Shipments().GetShipmentTrackingInfo(bindingSource);
	}

	public void PrintShippingLabel(M1BindingSource bindingSource)
	{
		new Shipments().PrintLabel(bindingSource);
	}

	public void PrintAllShippingLabels(M1BindingSource bindingSource)
	{
		new Shipments().PrintAllLabels(bindingSource);
	}

	public void PrintCODLabel(M1BindingSource bindingSource)
	{
		new Shipments().PrintCODLabel(bindingSource);
	}

	public void DeletePackage(M1BindingSource bindingSource)
	{
		new Shipments().DeletePackage(bindingSource);
	}

	public void PostShipment(M1BindingSource bindingsource)
	{
		new Shipments().PostShipment(bindingsource);
	}

	public bool ShipmentPeriodCheck(M1BindingSource bindingSource)
	{
		return new Shipments().ShipmentPeriodCheck(bindingSource);
	}

	public string PostShipmentCheck(M1BindingSource bindingsource)
	{
		return new Shipments().PostShipmentCheck(bindingsource);
	}

	public string QtyShippedExceedsQtyOnSalesOrder(M1BindingSource bindingSource, DataRow row)
	{
		return new Shipments().QtyShippedExceedsQtyOnSalesOrder(bindingSource, row);
	}

	public string CheckShipmentForFutureAdjustmentTransactions(M1BindingSource bindingsource)
	{
		return new Shipments().CheckShipmentForFutureAdjustmentTransactions(bindingsource);
	}

	public string CheckShipmentForZeroDollarTotals(M1BindingSource bindingSource)
	{
		return new Shipments().CheckShipmentForZeroDollarTotals(bindingSource);
	}

	public bool CheckEDIModuleIsPurchased(M1BindingSource bindingSource)
	{
		return bindingSource.DataDictionary.ProductCode.IsCustomModulePurchased(12);
	}

	public bool ShipmentLinesHasInactiveBinsGoingNegative(M1BindingSource bindingSource, string shipmentId)
	{
		return new Shipments().ShipmentLinesHasInactiveBinsGoingNegative(bindingSource.Database, shipmentId);
	}
}
