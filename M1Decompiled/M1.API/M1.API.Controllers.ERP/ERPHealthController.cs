using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Models.ERP;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.ERP;

[RoutePrefix("api/ERP/Health")]
public class ERPHealthController : ERPBaseController
{
	public IERPHealthModel erpHealthModel;

	/// <summary>
	/// Returns success if connection was established.
	/// </summary>
	/// <returns></returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<ERPCustomTableDto>))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "ERP/HEALTH" })]
	[AcceptVerbs("GET")]
	[Route("")]
	public async Task<IHttpActionResult> GetHealthCheck()
	{
		using (erpHealthModel = new ERPHealthModel())
		{
			return await RunApiMethod(base.Request, erpHealthModel, () => erpHealthModel.APIValidationIsTrueFunction(), () => erpHealthModel.APIProceessIsTrueFunction(), showReturnObject: true, showResponseMessage: false, showRecordCount: false);
		}
	}
}
