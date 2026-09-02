using System;
using System.Threading.Tasks;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPPartReviewRepository : APIBaseRepository, IERPPartReviewRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartReviewRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartReviewExist(Guid partReviewId)
	{
		InitializeParameterLists();
		base.filterList.Add("wgrUniqueID|C", partReviewId);
		base.selectList.Add("wgrUniqueID");
		return Task.FromResult(GetAsObject("PartReviews", base.filterList, base.selectList, null, null) != null);
	}
}
