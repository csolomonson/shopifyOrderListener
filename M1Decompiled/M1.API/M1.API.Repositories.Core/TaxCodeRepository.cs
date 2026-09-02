using System;
using System.Threading.Tasks;
using M1.API.Utilities;
using M1.Core;

namespace M1.API.Repositories.Core;

public class TaxCodeRepository : APIBaseRepository, ITaxCodeRepository, IAPIBaseRepository, IDisposable
{
	public TaxCodeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public TaxCodeRepository(M1Database database)
	{
		base.M1database = database;
	}

	public Task<bool> DoesTaxCodeExistAsync(string taxCodeId)
	{
		InitializeParameterLists();
		base.filterList.Add("xaxTaxCodeID|C", taxCodeId);
		base.selectList.Add("xaxTaxCodeID");
		return Task.FromResult(GetAsObject("TaxCodes", base.filterList, base.selectList, null, null) != null);
	}

	public new void Dispose()
	{
		base.Dispose(disposing: true);
	}
}
