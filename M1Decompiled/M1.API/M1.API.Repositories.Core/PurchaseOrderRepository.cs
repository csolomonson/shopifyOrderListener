using System;
using System.Threading.Tasks;
using M1.API.Utilities;

namespace M1.API.Repositories.Core;

public class PurchaseOrderRepository : APIBaseRepository, IPurchaseOrderRepository, IAPIBaseRepository, IDisposable
{
	public PurchaseOrderRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public Task<bool> DoesPurchaseOrderExists(string purchaseOrderId)
	{
		InitializeParameterLists();
		base.filterList.Add("pmpPurchaseOrderID|C", purchaseOrderId);
		base.selectList.Add("pmpPurchaseOrderID");
		return Task.FromResult(GetAsObject("PurchaseOrders", base.filterList, base.selectList, null, null) != null);
	}

	public new void Dispose()
	{
		base.Dispose(disposing: true);
	}
}
