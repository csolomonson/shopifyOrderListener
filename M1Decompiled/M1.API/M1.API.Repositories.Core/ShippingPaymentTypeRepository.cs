using System.Threading.Tasks;
using M1.API.Utilities;
using M1.Core;

namespace M1.API.Repositories.Core;

public class ShippingPaymentTypeRepository : APIBaseRepository, IShippingPaymentTypeRepository
{
	public ShippingPaymentTypeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public ShippingPaymentTypeRepository(M1Database database)
	{
		base.M1database = database;
	}

	public Task<bool> DoesShippingPaymentTypeExistsAsync(string shippingPaymentTypeCode)
	{
		InitializeParameterLists();
		base.filterList.Add("xayShippingPaymentTypeID|C", shippingPaymentTypeCode);
		base.selectList.Add("xayShippingPaymentTypeID");
		return Task.FromResult(GetAsObject("ShippingPaymentTypes", base.filterList, base.selectList, null, null) != null);
	}

	public new void Dispose()
	{
		base.Dispose(disposing: true);
	}
}
