using System.Threading.Tasks;
using M1.API.Utilities;
using M1.Core;

namespace M1.API.Repositories.Core;

public class ShippingMethodRepository : APIBaseRepository, IShippingMethodRepository
{
	public ShippingMethodRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public ShippingMethodRepository(M1Database database)
	{
		base.M1database = database;
	}

	public Task<bool> DoesShippingMethodExistsAsync(string shippingMethodId)
	{
		InitializeParameterLists();
		base.filterList.Add("xasShippingMethodID|C", shippingMethodId);
		base.selectList.Add("xasShippingMethodID");
		return Task.FromResult(GetAsObject("ShippingMethods", base.filterList, base.selectList, null, null) != null);
	}

	public new void Dispose()
	{
		base.Dispose(disposing: true);
	}
}
