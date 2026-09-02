using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Models.ERP;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.ERP;

[RoutePrefix("api/ERP/AssetTypeMethods")]
public class ERPAssetTypeMethodsController : ERPBaseController
{
	public IERPAssetTypeMethodModel erpAssetTypeMethodModel;

	/// <summary>
	/// Returns all existing AssetTypeMethods according to the filter, order by and pagination options.
	/// </summary>
	/// <param name="pageSize">Specifies the number of items per page. Defaults to 1000 which is also the maximum.</param>
	/// <param name="pageNumber">Specifies the page number to retrieve. Defaults to 0, which corresponds to the first page.</param>
	/// <param name="filter">Applies a filter to the returned data. The format should be `M1Field[operator]value`.</param>
	/// <param name="orderBy">Specifies the sorting criteria for the returned data. The format should be `M1Field[Asc],M1Field[Desc]`.</param>
	/// <returns>A list of <see cref="T:M1.API.DTOs.ERP.ERPAssetTypeMethodDto" /> objects</returns>
	/// <remarks>
	/// When passing the filter parameter, the following operators are supported: [eq, ne, gt, lt]. Multiple filters can be specified as separate query parameters in the format M1Field[operator]value. eg. `impPartID[eq]CLIP`. An invalid filter will return a 400 Bad Request response.
	///
	/// When passing the orderBy parameter, the following operators are supported: [Asc, Desc]. The format must be M1Field[Asc],M1Field[Desc]. eg. `impPartID[Asc],impPartType[Desc]`. An invalid orderBy will return a 400 Bad Request response.
	///
	/// Custom fields are returned in the response as their full field name, they are also filterable and sortable. The format should be `M1CustomField[operator]value`. eg. `uimpCustomField1[eq]12`.
	/// </remarks>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ERPResponseMessageDto<IList<ERPAssetTypeMethodDto>>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "ERP/FINANCIAL" })]
	[AcceptVerbs("GET")]
	[Route("")]
	public async Task<IHttpActionResult> GetAllAssetTypeMethodAsync([FromUri] int pageSize = 1000, [FromUri] int pageNumber = 0, [FromUri] string[] filter = null, [FromUri] string orderBy = null)
	{
		using (erpAssetTypeMethodModel = new ERPAssetTypeMethodModel())
		{
			return await RunApiMethod(base.Request, erpAssetTypeMethodModel, () => erpAssetTypeMethodModel.ValidateRequest_GetAllAssetTypeMethods(pageSize, pageNumber, filter, orderBy).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult(), () => erpAssetTypeMethodModel.Process_GetAllAssetTypeMethods(pageSize, pageNumber, filter, orderBy), showReturnObject: true, showResponseMessage: false, showRecordCount: true);
		}
	}

	/// <summary>
	/// Returns a single AssetTypeMethod record for a given Unique Id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a AssetTypeMethod based on its identifier. If the record does not exist, a 404 Not Found response will be returned.
	/// </remarks>
	/// <param name="key">The Unique Id of the record to be retrieved</param>
	/// <returns>A single <see cref="T:M1.API.DTOs.ERP.ERPAssetTypeMethodDto" /> object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ERPResponseMessageDto<ERPAssetTypeMethodDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "ERP/FINANCIAL" })]
	[AcceptVerbs("GET")]
	[Route("{key}")]
	public async Task<IHttpActionResult> GetAssetTypeMethodAsync([FromUri(Name = "key")] Guid key)
	{
		using (erpAssetTypeMethodModel = new ERPAssetTypeMethodModel())
		{
			return await RunApiMethod(base.Request, erpAssetTypeMethodModel, () => erpAssetTypeMethodModel.ValidateRequest_GetAssetTypeMethod(key).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult(), () => erpAssetTypeMethodModel.Process_GetAssetTypeMethod(key), showReturnObject: true, showResponseMessage: false, showRecordCount: false);
		}
	}
}
