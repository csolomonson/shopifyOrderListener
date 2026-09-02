using System;
using System.Threading.Tasks;

namespace M1.API.Repositories.Core;

public interface IShipmentRepository : IAPIBaseRepository, IDisposable
{
	Task<bool> DoesShipmentExists(string shipmentId);

	Task<bool> DoesShippingMethodExists(string methodId);

	Task<string> GetShipmentCarrier(string shippingMethodId);
}
