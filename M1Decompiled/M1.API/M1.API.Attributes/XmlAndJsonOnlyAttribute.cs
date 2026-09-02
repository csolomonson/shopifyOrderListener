using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace M1.API.Attributes;

public class XmlAndJsonOnlyAttribute : ActionFilterAttribute
{
	public override void OnActionExecuting(HttpActionContext actionContext)
	{
		base.OnActionExecuting(actionContext);
		actionContext.ControllerContext.Configuration.Formatters.Clear();
		actionContext.ControllerContext.Configuration.Formatters.Add(new XmlMediaTypeFormatter());
		actionContext.ControllerContext.Configuration.Formatters.Add(new JsonMediaTypeFormatter());
	}
}
