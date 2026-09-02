using System.Web.Http;
using M1.API.Models.ERP;

namespace M1.API.Controllers.ERP;

[RoutePrefix("api/ERP/PartReviews")]
public class ERPPartReviewsController : ERPBaseController
{
	public IERPPartReviewModel erpPartReviewModel;
}
