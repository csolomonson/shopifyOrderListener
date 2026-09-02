using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.BOM.Inventory;
using M1.API.DTOs.Core;
using M1.API.Models.BOM.Inventory;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM.Inventory;

[RoutePrefix("api/BOM/Part")]
public class BOMPartBinDetailController : BOMBaseController
{
	/// <summary>
	/// Returns part and part bin details for a given M1 part id or GUID. Do not pass part id if it has special characters (other than Aa-Zz0-9.-) pass GUID instead
	/// </summary>
	/// <param name="partId">The M1 Part Id or GUID of the part as a string</param>
	/// <returns>BOMPartBinDetailDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMPartBinDetailDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("GET")]
	[Route("GetPartBinDetails/{partId}")]
	public async Task<IHttpActionResult> GetPartBinDetailsAsync([FromUri(Name = "partId")] string partId)
	{
		using (bomPartBinDetail = new BOMPartBinDetailModel())
		{
			return await RunApiMethod(base.Request, bomPartBinDetail, () => bomPartBinDetail.ValidateRequest_GetPartId(partId).Result, () => bomPartBinDetail.Process_PostPartBinDetail(bomPartBinDetail.PartKeyDictionary["impPartID"].ToString()), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Returns all existing part bin details with pagination. 
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page</param>
	/// <returns>BOMPartBinDetailDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMPartBinDetailDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("GET")]
	[Route("GetAllPartBinDetails/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllPartBinDetailsAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		using (bomPartBinDetail = new BOMPartBinDetailModel())
		{
			return await RunApiMethod(base.Request, bomPartBinDetail, () => bomPartBinDetail.APIValidationIsTrueFunction(), () => bomPartBinDetail.Process_GetAllPartBinDetails(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
	}
}
