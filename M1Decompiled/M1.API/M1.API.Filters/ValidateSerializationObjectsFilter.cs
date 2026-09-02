using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace M1.API.Filters;

public class ValidateSerializationObjectsFilter : ActionFilterAttribute
{
	public override void OnActionExecuting(HttpActionContext actionContext)
	{
		if (!actionContext.ModelState.IsValid)
		{
			actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
		}
	}
}
