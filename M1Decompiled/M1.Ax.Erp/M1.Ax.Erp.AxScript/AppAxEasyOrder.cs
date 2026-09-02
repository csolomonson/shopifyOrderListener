using System;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp.AxScript;

[AxScript("EasyOrder")]
[ComVisible(true)]
public class AppAxEasyOrder
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxEasyOrder(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public string CreateOfflineFileFromShipment(string shipmentID)
	{
		return new EasyOrder().CreateOfflineFileFromShipment(provider, _Database, shipmentID);
	}

	public string CreateOfflineFileFromSalesOrder(string salesOrderID)
	{
		return new EasyOrder().CreateOfflineFileFromSalesOrder(provider, _Database, salesOrderID);
	}

	public string CreateOfflineFile(object bindingSource)
	{
		return new EasyOrder().CreateOfflineFile(provider, _Database, bindingSource);
	}

	public void UpdateEasyOrderExternalStatus(string salesOrderID, object transaction = null)
	{
		new EasyOrder().UpdateEasyOrderExternalStatus(_Database, salesOrderID, transaction);
	}

	public void UpdateEasyOrderExternalStatusFromShipment(string shipmentID, object transaction = null)
	{
		new EasyOrder().UpdateEasyOrderExternalStatusFromShipment(_Database, shipmentID, transaction);
	}
}
