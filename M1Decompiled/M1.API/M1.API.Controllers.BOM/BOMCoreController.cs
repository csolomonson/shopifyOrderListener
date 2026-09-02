using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Models.Core;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM;

[RoutePrefix("api/BOM/Core")]
public class BOMCoreController : BOMBaseController
{
	/// <summary>
	/// Returns list of all active part classes
	/// </summary>
	/// <param name="pageno">The page number if wants to get a specific page as integer.</param>
	/// <param name="pagesize">The total numbers of items should be on a page as integer.</param>
	/// <returns>BOMPartClassesDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMPartClassesDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/CORE" })]
	[AcceptVerbs("GET")]
	[Route("GetPartClassesAll/{pageNo?}/{pageSize?}")]
	public async Task<IHttpActionResult> GetPartClassesAllAsync([FromUri(Name = "pageNo")] int pageno = 0, [FromUri(Name = "pageSize")] int pagesize = 20)
	{
		using (apiCoreModel = new APICoreModel())
		{
			return await RunApiMethod(base.Request, apiCoreModel, () => apiCoreModel.APIValidationIsTrueFunction(), () => apiCoreModel.Process_GetPartClassesAll(), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Returns list of all active part groups
	/// </summary>
	/// <param name="pageno">The page number if wants to get a specific page as integer.</param>
	/// <param name="pagesize">The total numbers of items should be on a page as integer.</param>
	/// <returns>BOMPartGroupsDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMPartGroupsDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/CORE" })]
	[AcceptVerbs("GET")]
	[Route("GetPartGroupsAll/{pageNo?}/{pageSize?}")]
	public async Task<IHttpActionResult> GetPartGroupsAllAsync([FromUri(Name = "pageNo")] int pageno = 0, [FromUri(Name = "pageSize")] int pagesize = 20)
	{
		using (apiCoreModel = new APICoreModel())
		{
			return await RunApiMethod(base.Request, apiCoreModel, () => apiCoreModel.APIValidationIsTrueFunction(), () => apiCoreModel.Process_GetPartGroupsAll(), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Returns list of all active processes
	/// </summary>
	/// <param name="pageno">The page number if wants to get a specific page as integer.</param>
	/// <param name="pagesize">The total numbers of items should be on a page as integer.</param>
	/// <returns></returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMProcessDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/CORE" })]
	[AcceptVerbs("GET")]
	[Route("GetProcessesAll/{pageNo?}/{pageSize?}")]
	public async Task<IHttpActionResult> GetProcessesAllAsync([FromUri(Name = "pageNo")] int pageno = 0, [FromUri(Name = "pageSize")] int pagesize = 20)
	{
		using (apiCoreModel = new APICoreModel())
		{
			return await RunApiMethod(base.Request, apiCoreModel, () => apiCoreModel.APIValidationIsTrueFunction(), () => apiCoreModel.Process_GetProcessesAll(), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Returns list of all active workcenters
	/// </summary>
	/// <param name="pageno">The page number if wants to get a specific page as integer.</param>
	/// <param name="pagesize">The total numbers of items should be on a page as integer.</param>
	/// <returns></returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMWorkCenterDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/CORE" })]
	[AcceptVerbs("GET")]
	[Route("GetWorkCentersAll/{pageNo?}/{pageSize?}")]
	public async Task<IHttpActionResult> GetWorkCentersAllAsync([FromUri(Name = "pageNo")] int pageno = 0, [FromUri(Name = "pageSize")] int pagesize = 20)
	{
		using (apiCoreModel = new APICoreModel())
		{
			return await RunApiMethod(base.Request, apiCoreModel, () => apiCoreModel.APIValidationIsTrueFunction(), () => apiCoreModel.Process_GetWorkCentersAll(), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Returns list of all active warehouses
	/// </summary>
	/// <param name="pageno">The page number if wants to get a specific page as integer.</param>
	/// <param name="pagesize">The total numbers of items should be on a page as integer.</param>
	/// <returns></returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMWarehousesDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/CORE" })]
	[AcceptVerbs("GET")]
	[Route("GetWarehousesAll/{pageNo?}/{pageSize?}")]
	public async Task<IHttpActionResult> GetWarehousesAllAsync([FromUri(Name = "pageNo")] int pageno = 0, [FromUri(Name = "pageSize")] int pagesize = 20)
	{
		using (apiCoreModel = new APICoreModel())
		{
			return await RunApiMethod(base.Request, apiCoreModel, () => apiCoreModel.APIValidationIsTrueFunction(), () => apiCoreModel.Process_GetWarehousesAll(), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Returns list of all active warehouse bins
	/// </summary>
	/// <param name="pageno">The page number if wants to get a specific page as integer.</param>
	/// <param name="pagesize">The total numbers of items should be on a page as integer.</param>
	/// <returns></returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMWarehouseBinsDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/CORE" })]
	[AcceptVerbs("GET")]
	[Route("GetWarehouseBinsAll/{pageNo?}/{pageSize?}")]
	public async Task<IHttpActionResult> GetWarehouseBinsAllAsync([FromUri(Name = "pageNo")] int pageno = 0, [FromUri(Name = "pageSize")] int pagesize = 20)
	{
		using (apiCoreModel = new APICoreModel())
		{
			return await RunApiMethod(base.Request, apiCoreModel, () => apiCoreModel.APIValidationIsTrueFunction(), () => apiCoreModel.Process_GetWarehouseBinsAll(), showReturnObject: true, showResponseMessage: false);
		}
	}
}
