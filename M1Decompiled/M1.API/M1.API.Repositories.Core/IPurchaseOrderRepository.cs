using System;
using System.Threading.Tasks;

namespace M1.API.Repositories.Core;

public interface IPurchaseOrderRepository : IAPIBaseRepository, IDisposable
{
	Task<bool> DoesPurchaseOrderExists(string purchaseOrderId);
}
