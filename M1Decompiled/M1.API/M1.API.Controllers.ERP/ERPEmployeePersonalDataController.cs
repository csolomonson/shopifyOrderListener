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

[RoutePrefix("api/ERP/EmployeePersonalData")]
public class ERPEmployeePersonalDataController : ERPBaseController
{
	public IERPEmployeePersonalDatumModel erpEmployeePersonalDatumModel;

	/// <summary>
	/// Returns all existing EmployeePersonalData according to the filter, order by and pagination options.
	/// </summary>
	/// <param name="pageSize">Specifies the number of items per page. Defaults to 1000 which is also the maximum.</param>
	/// <param name="pageNumber">Specifies the page number to retrieve. Defaults to 0, which corresponds to the first page.</param>
	/// <param name="filter">Applies a filter to the returned data. The format should be `M1Field[operator]value`.</param>
	/// <param name="orderBy">Specifies the sorting criteria for the returned data. The format should be `M1Field[Asc],M1Field[Desc]`.</param>
	/// <returns>A list of <see cref="T:M1.API.DTOs.ERP.ERPEmployeePersonalDatumDto" /> objects</returns>
	/// <remarks>
	/// When passing the filter parameter, the following operators are supported: [eq, ne, gt, lt]. Multiple filters can be specified as separate query parameters in the format M1Field[operator]value. eg. `impPartID[eq]CLIP`. An invalid filter will return a 400 Bad Request response.
	///
	/// When passing the orderBy parameter, the following operators are supported: [Asc, Desc]. The format must be M1Field[Asc],M1Field[Desc]. eg. `impPartID[Asc],impPartType[Desc]`. An invalid orderBy will return a 400 Bad Request response.
	///
	/// Custom fields are returned in the response as their full field name, they are also filterable and sortable. The format should be `M1CustomField[operator]value`. eg. `uimpCustomField1[eq]12`.
	/// </remarks>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ERPResponseMessageDto<IList<ERPEmployeePersonalDatumDto>>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "ERP/EMPLOYEE" })]
	[AcceptVerbs("GET")]
	[Route("")]
	public async Task<IHttpActionResult> GetAllEmployeePersonalDatumAsync([FromUri] int pageSize = 1000, [FromUri] int pageNumber = 0, [FromUri] string[] filter = null, [FromUri] string orderBy = null)
	{
		using (erpEmployeePersonalDatumModel = new ERPEmployeePersonalDatumModel())
		{
			return await RunApiMethod(base.Request, erpEmployeePersonalDatumModel, () => erpEmployeePersonalDatumModel.ValidateRequest_GetAllEmployeePersonalData(pageSize, pageNumber, filter, orderBy).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult(), () => erpEmployeePersonalDatumModel.Process_GetAllEmployeePersonalData(pageSize, pageNumber, filter, orderBy), showReturnObject: true, showResponseMessage: false, showRecordCount: true);
		}
	}

	/// <summary>
	/// Returns a single EmployeePersonalDatum record for a given Unique Id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a EmployeePersonalDatum based on its identifier. If the record does not exist, a 404 Not Found response will be returned.
	/// </remarks>
	/// <param name="key">The Unique Id of the record to be retrieved</param>
	/// <returns>A single <see cref="T:M1.API.DTOs.ERP.ERPEmployeePersonalDatumDto" /> object).</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ERPResponseMessageDto<ERPEmployeePersonalDatumDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "ERP/EMPLOYEE" })]
	[AcceptVerbs("GET")]
	[Route("{key}")]
	public async Task<IHttpActionResult> GetEmployeePersonalDatumAsync([FromUri(Name = "key")] Guid key)
	{
		using (erpEmployeePersonalDatumModel = new ERPEmployeePersonalDatumModel())
		{
			return await RunApiMethod(base.Request, erpEmployeePersonalDatumModel, () => erpEmployeePersonalDatumModel.ValidateRequest_GetEmployeePersonalDatum(key).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult(), () => erpEmployeePersonalDatumModel.Process_GetEmployeePersonalDatum(key), showReturnObject: true, showResponseMessage: false, showRecordCount: false);
		}
	}

	/// <summary>
	/// Creates or updates a single EmployeePersonalDatum record.
	/// </summary>
	/// <param name="employeePersonalDatum">The fully populated <see cref="T:M1.API.DTOs.ERP.ERPEmployeePersonalDatumDto" /> model.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:System.Web.Http.IHttpActionResult" /> indicating the result of the put operation,
	/// or an appropriate error message depending on the outcome of the request.
	/// </returns>
	/// <remarks>
	/// When a record does not exist, a new record will be created. When a record exists, the record will be updated.
	///
	/// When updating an existing record, all fields must be provided, even if they are not changing. Upon update, the UniqueID, Created By/Date and M1 Keys (eg. cmoOrganizationID) will not be updated.
	///
	/// Custom fields should be sent in the request as their full field name and should be prefixed with `u`. eg. `uimpCustomField1`.
	///
	/// To use this endpoint, you must have an API Key that supports writes.
	/// </remarks>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.Created, Type = typeof(ERPResponseMessageDto<ERPEmployeePersonalDatumDto>))]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ERPResponseMessageDto<ERPEmployeePersonalDatumDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "ERP/EMPLOYEE" })]
	[AcceptVerbs("PUT")]
	[Route("")]
	public async Task<IHttpActionResult> PutEmployeePersonalDatumAsync([FromBody] ERPEmployeePersonalDatumDto employeePersonalDatum)
	{
		using (erpEmployeePersonalDatumModel = new ERPEmployeePersonalDatumModel())
		{
			return await RunApiMethod(base.Request, erpEmployeePersonalDatumModel, () => erpEmployeePersonalDatumModel.ValidateRequest_PutEmployeePersonalDatum(employeePersonalDatum).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult(), () => erpEmployeePersonalDatumModel.Process_PutEmployeePersonalDatum(employeePersonalDatum), showReturnObject: true, showResponseMessage: true, showRecordCount: false);
		}
	}

	/// <summary>
	/// Deletes a single EmployeePersonalDatum record.
	/// </summary>
	/// <param name="key">The Unique Id of the record to be deleted</param>
	/// <returns></returns>
	/// <remarks>
	/// If the record is not found, a 404 Not Found response will be returned.
	///
	/// If the record is successfully deleted, a 200 OK response will be returned.
	///
	/// If the record is being used anywhere in the M1 system, a 400 Bad Request response will be returned.
	///
	/// To use this endpoint, you must have an API Key that supports writes.
	/// </remarks>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ERPResponseMessageDto<ERPEmployeePersonalDatumDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "ERP/EMPLOYEE" })]
	[AcceptVerbs("DELETE")]
	[Route("{key}")]
	public async Task<IHttpActionResult> DeleteEmployeePersonalDatumAsync([FromUri(Name = "key")] Guid key)
	{
		using (erpEmployeePersonalDatumModel = new ERPEmployeePersonalDatumModel())
		{
			return await RunApiMethod(base.Request, erpEmployeePersonalDatumModel, () => erpEmployeePersonalDatumModel.ValidateRequest_DeleteEmployeePersonalDatum(key).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult(), () => erpEmployeePersonalDatumModel.Process_DeleteEmployeePersonalDatum(key), showReturnObject: false, showResponseMessage: true, showRecordCount: false);
		}
	}
}
