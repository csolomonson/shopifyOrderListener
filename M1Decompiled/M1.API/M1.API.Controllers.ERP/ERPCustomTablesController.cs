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

[RoutePrefix("api/ERP/CustomTables")]
public class ERPCustomTablesController : ERPBaseController
{
	public IERPCustomTableModel erpCustomTableModel;

	/// <summary>
	/// Returns all existing records from the specified M1 custom table with pagination.
	/// </summary>
	/// <param name="tableName">The M1 Custom table name.</param>
	/// <param name="pageSize">Specifies the number of items per page. Defaults to 1000 which is also the maximum.</param>
	/// <param name="pageNumber">Specifies the page number to retrieve. Defaults to 0, which corresponds to the first page.</param>
	/// <param name="filter">Applies a filter to the returned data. The format should be `M1UserDefinedCustomField[operator]value`.</param>
	/// <param name="orderBy">Specifies the sorting criteria for the returned data. The format should be `M1UserDefinedCustomField[Asc],M1UserDefinedCustomField[Desc]`.</param>
	/// <remarks>
	/// When passing the filter parameter, the following operators are supported: [eq, ne, gt, lt]. Multiple filters can be specified as separate query parameters in the format M1UserDefinedCustomField[operator]value. eg. `uctCustomTableFieldID[eq]TEST`. An invalid filter will return a 400 Bad Request response.
	///
	/// When passing the orderBy parameter, the following operators are supported: [Asc, Desc]. The format must be M1UserDefinedCustomField[Asc],M1UserDefinedCustomField[Desc]. eg. `uctCustomTableFieldID[Asc],uctCustomTableField2ID[Desc]`. An invalid orderBy will return a 400 Bad Request response.
	///
	/// The M1 custom table passed must exist in the M1 Data Dictionary. If the table does not exist, a 404 Not Found response will be returned.
	///
	/// The M1 custom table must have a field in the format of `uctUniqueID' of type unique identifier (guid). If the table does not have a unique identifier field, a 400 Bad Request response will be returned.
	///
	/// The M1 custom table must have a prefix defined in the M1 Data Dictionary. If the table does not have a prefix, a 400 Bad Request response will be returned.
	/// </remarks>
	/// <returns>A single <see cref="T:M1.API.DTOs.ERP.ERPCustomTableDto" /> object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ERPResponseMessageDto<IList<ERPCustomTableDto>>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "ERP/CUSTOM" })]
	[AcceptVerbs("GET")]
	[Route("{tableName}")]
	public async Task<IHttpActionResult> GetAllCustomTableAsync([FromUri(Name = "tableName")] string tableName, [FromUri] int pageSize = 1000, [FromUri] int pageNumber = 0, [FromUri] string[] filter = null, [FromUri] string orderBy = null)
	{
		using (erpCustomTableModel = new ERPCustomTableModel())
		{
			return await RunApiMethod(base.Request, erpCustomTableModel, () => erpCustomTableModel.ValidateRequest_GetAllCustomTableRecords(tableName, pageSize, pageNumber, filter, orderBy).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult(), () => erpCustomTableModel.Process_GetAllCustomTableRecords(tableName, pageSize, pageNumber, filter, orderBy), showReturnObject: true, showResponseMessage: false, showRecordCount: true);
		}
	}

	/// <summary>
	/// Returns a single CustomTable record for a given CustomTable Unique Id.
	/// </summary>
	/// <remarks>
	/// This endpoint retrieves a CustomTable record based on its identifier. If the record does not exist, a 404 Not Found response will be returned.
	///
	/// The M1 custom table passed must exist in the M1 Data Dictionary. If the table does not exist, a 404 Not Found response will be returned.
	///
	/// The M1 custom table must have a field in the format of `uctUniqueID' of type unique identifier (guid). If the table does not have a unique identifier field, a 400 Bad Request response will be returned.
	///
	/// The M1 custom table must have a prefix defined in the M1 Data Dictionary. If the table does not have a prefix, a 400 Bad Request response will be returned.
	/// </remarks>
	/// <param name="tableName">The M1 custom table name</param>
	/// <param name="key">The Unique Id of the record to be retrieved</param>
	/// <returns>A single <see cref="T:M1.API.DTOs.ERP.ERPCustomTableDto" /> object.</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ERPResponseMessageDto<ERPCustomTableDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "ERP/CUSTOM" })]
	[AcceptVerbs("GET")]
	[Route("{tableName}/{key}")]
	public async Task<IHttpActionResult> GetCustomTableAsync([FromUri(Name = "tableName")] string tableName, [FromUri(Name = "key")] Guid key)
	{
		using (erpCustomTableModel = new ERPCustomTableModel())
		{
			return await RunApiMethod(base.Request, erpCustomTableModel, () => erpCustomTableModel.ValidateRequest_GetCustomTableRecord(tableName, key).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult(), () => erpCustomTableModel.Process_GetCustomTableRecord(tableName, key), showReturnObject: true, showResponseMessage: false, showRecordCount: false);
		}
	}

	/// <summary>
	/// Creates or updates a single CustomTable record.
	/// </summary>
	/// <param name="tableName">The custom table name</param>
	/// <param name="customTable">The fully populated CustomTable model.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:System.Web.Http.IHttpActionResult" /> indicating the result of the put operation,
	/// or an appropriate error message depending on the outcome of the request.
	/// </returns>
	/// <remarks>
	/// When a record does not exist, a new record will be created. When a record exists, the record will be updated.
	///
	/// When updating an existing record, all fields must be provided, even if they are not changing.
	///
	/// The M1 custom table passed must exist in the M1 Data Dictionary. If the table does not exist, a 404 Not Found response will be returned.
	///
	/// The M1 custom table must have a field in the format of `uctUniqueID' of type unique identifier (guid). If the table does not have a unique identifier field, a 400 Bad Request response will be returned.
	///
	/// The M1 custom table must have a prefix defined in the M1 Data Dictionary. If the table does not have a prefix, a 400 Bad Request response will be returned.
	/// </remarks>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.Created, Type = typeof(ERPResponseMessageDto<ERPCustomTableDto>))]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ERPResponseMessageDto<ERPCustomTableDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "ERP/CUSTOM" })]
	[AcceptVerbs("PUT")]
	[Route("{tableName}")]
	public async Task<IHttpActionResult> PutCustomTableAsync([FromUri(Name = "tableName")] string tableName, [FromBody] ERPCustomTableDto customTable)
	{
		using (erpCustomTableModel = new ERPCustomTableModel())
		{
			return await RunApiMethod(base.Request, erpCustomTableModel, () => erpCustomTableModel.ValidateRequest_PutCustomTableRecord(tableName, customTable).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult(), () => erpCustomTableModel.Process_PutCustomTableRecord(tableName, customTable), showReturnObject: true, showResponseMessage: true, showRecordCount: false);
		}
	}

	/// <summary>
	/// Deletes a single CustomTable record.
	/// </summary>
	/// <param name="tableName">The custom table name</param>
	/// <param name="key">The Unique Id of the record to be retrieved</param>
	/// <returns></returns>
	/// <remarks>
	/// If the CustomTable record is not found, a 404 Not Found response will be returned.
	///
	/// If the CustomTable record is successfully deleted, a 200 OK response will be returned.
	///
	/// The M1 custom table passed must exist in the M1 Data Dictionary. If the table does not exist, a 404 Not Found response will be returned.
	///
	/// The M1 custom table must have a field in the format of `uctUniqueID' of type unique identifier (guid). If the table does not have a unique identifier field, a 400 Bad Request response will be returned.
	///
	/// The M1 custom table must have a prefix defined in the M1 Data Dictionary. If the table does not have a prefix, a 400 Bad Request response will be returned.
	///
	/// If the CustomTable UniqueID is being used anywhere in the M1 system as a foreign key (ie. DDRelations), a 400 Bad Request response will be returned.
	/// </remarks>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ERPResponseMessageDto<ERPCustomTableDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "ERP/CUSTOM" })]
	[AcceptVerbs("DELETE")]
	[Route("{tableName}/{key}")]
	public async Task<IHttpActionResult> DeleteCustomTableAsync([FromUri(Name = "tableName")] string tableName, [FromUri(Name = "key")] Guid key)
	{
		using (erpCustomTableModel = new ERPCustomTableModel())
		{
			return await RunApiMethod(base.Request, erpCustomTableModel, () => erpCustomTableModel.ValidateRequest_DeleteCustomTableRecord(tableName, key).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult(), () => erpCustomTableModel.Process_DeleteCustomTableRecord(tableName, key), showReturnObject: false, showResponseMessage: true, showRecordCount: false);
		}
	}
}
