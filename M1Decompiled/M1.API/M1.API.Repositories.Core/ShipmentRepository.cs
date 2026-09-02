using System;
using System.Threading.Tasks;
using M1.API.Utilities;
using M1.Core;

namespace M1.API.Repositories.Core;

public class ShipmentRepository : APIBaseRepository, IShipmentRepository, IAPIBaseRepository, IDisposable
{
	public ShipmentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public ShipmentRepository(M1Database database)
	{
		base.M1database = database;
	}

	public Task<bool> DoesShipmentExists(string shipmentId)
	{
		InitializeParameterLists();
		base.filterList.Add("smpShipmentID|C", shipmentId);
		base.selectList.Add("smpShipmentID");
		return Task.FromResult(GetAsObject("Shipments", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesShippingMethodExists(string methodId)
	{
		InitializeParameterLists();
		base.filterList.Add("xasShippingMethodID|C", methodId);
		base.filterList.Add("xasInactive|C", false);
		base.selectList.Add("xasShippingMethodID");
		return Task.FromResult(GetAsObject("ShippingMethods", base.filterList, base.selectList, null, null) != null);
	}

	public Task<string> GetShipmentCarrier(string shippingMethodId)
	{
		InitializeParameterLists();
		base.filterList.Add("xasShippingMethodID|C", shippingMethodId);
		base.selectList.Add("xasCarrier");
		return Task.FromResult((GetAsObject("ShippingMethods", base.filterList, base.selectList, null, null) ?? string.Empty).ToString());
	}

	public new void Dispose()
	{
		base.Dispose(disposing: true);
	}
}
