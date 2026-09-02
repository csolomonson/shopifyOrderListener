using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using M1.API.App_Start;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.EOD;

[RoutePrefix("api/EODSalesOrder")]
public class EODSalesOrderController : EODBaseController
{
	[AcceptVerbs("GET")]
	[Route("{id}")]
	[SwaggerOperation(null, Tags = new string[] { "EOD" })]
	[ApiExplorerSettings(IgnoreApi = true)]
	public HttpResponseMessage GetOrder([FromUri(Name = "id")] string m1SalesOrderId)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00b4: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Expected O, but got Unknown
		_ = base.Request.GetRequestContext().Principal;
		_ = ClaimsPrincipal.Current.Identity.Name;
		if (ApiClientContext.LoginAuthenticated)
		{
			APIStartup.Logger.InfoFormat("[EDI850SalesOrderREQUEST] - REQUEST_URI: {0}, HOST: {1}", base.Request.RequestUri, base.Request.Headers.Host);
			return new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = (HttpContent)new StringContent("ABC", Encoding.UTF8, "application/xml")
			};
		}
		return new HttpResponseMessage
		{
			StatusCode = HttpStatusCode.Unauthorized,
			Content = (HttpContent)new StringContent(ApiClientContext.LoginErrorOutputString, Encoding.UTF8, "application/xml")
		};
	}
}
