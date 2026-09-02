using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.Models.BOM;
using Swashbuckle.Swagger.Annotations;

namespace M1.API.Controllers.BOM;

[RoutePrefix("api/BOM/Part")]
public class BOMPartController : BOMBaseController
{
	/// <summary>
	/// Returns part details for a given M1 part id or GUID. Do not pass part id if it has special characters (other than Aa-Zz0-9.-) pass GUID instead
	/// </summary>
	/// <param name="partId">The M1 Part Id or GUID of the part as a string</param>
	/// <returns>BOMPartRevisionDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMPartDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("GET")]
	[Route("GetParts/{partId}")]
	public async Task<IHttpActionResult> GetPartsByIdAsync([FromUri(Name = "partId")] string partId)
	{
		using (bomPartModel = new BOMPartModel())
		{
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_GetPartId(partId).Result, () => bomPartModel.Process_GetParts(partId), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Returns all existing parts with pagination. 
	/// </summary>
	/// <param name="pageSize">The required size of the pagination. By default 1000.</param>
	/// <param name="pageNumber">The required number of page from pagination. By default 0, which is the first page</param>
	/// <returns>BOMPartRevisionDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMPartDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("GET")]
	[Route("GetAllParts/{pageSize}/{pageNumber}")]
	public async Task<IHttpActionResult> GetAllPartsAsync([FromUri(Name = "pageSize")] int pageSize = 1000, [FromUri(Name = "pageNumber")] int pageNumber = 0)
	{
		using (bomPartModel = new BOMPartModel())
		{
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.APIValidationIsTrueFunction(), () => bomPartModel.Process_GetAllParts(pageSize, pageNumber), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Creates a part with the required attributes.
	/// </summary>
	/// <param name="part">The part object as BOMPartDto with the incoming data.</param>
	/// <returns>APIResponseMessageDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("POST")]
	[Route("PostParts")]
	public async Task<IHttpActionResult> PostPartAsync([FromBody] BOMPartDto part)
	{
		using (bomPartModel = new BOMPartModel())
		{
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_ProcessPart(part).Result, () => bomPartModel.Process_PostPart(part), showReturnObject: false, showResponseMessage: true);
		}
	}

	/// <summary>
	/// Returns part and part revision details for a given M1 part id or GUID. Do not pass part id if it has special characters (other than Aa-Zz0-9.-) pass GUID instead
	/// </summary>
	/// <param name="partId">The M1 Part Id or GUID of the part as a string</param>
	/// <returns>BOMPartRevisionDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMBOMPartRevisionDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("GET")]
	[Route("GetPartRevisions/{partId}")]
	public async Task<IHttpActionResult> GetPartRevisionsAsync([FromUri(Name = "partId")] string partId)
	{
		using (bomPartModel = new BOMPartModel())
		{
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_GetPartId(partId).Result, () => bomPartModel.Process_GetPartRevision(bomPartModel.PartKeyDictionary["impPartID"].ToString()), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Creates a part and relevant part revisions based on input parameter.
	/// </summary>
	/// <param name="partRevision">The part revision object as CTMBOMPartRevisionDto</param>
	/// <returns>APIResponseMessageDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("POST")]
	[Route("PostPartRevisions")]
	public async Task<IHttpActionResult> PostPartRevisionsAsync([FromBody] CTMBOMPartRevisionDto partRevision)
	{
		using (bomPartModel = new BOMPartModel())
		{
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_ProcessPartRevision(partRevision).Result, () => bomPartModel.Process_PostPartRevision(partRevision), showReturnObject: false, showResponseMessage: true);
		}
	}

	/// <summary>
	/// Returns part and part assembly details for a given M1 method id, method revision id and method assembly id. Alternatively it can be used the method assembly Guid.
	/// </summary>
	/// <param name="methodId">The part method id as string</param>
	/// <param name="methodRevisionId">The part method revision id as string. If wants to pass BLANK revision id, use underscore ("_") character instead </param>
	/// <param name="methodAssemblyId">The part method assembly id as integer</param>
	/// <returns>BOMPartRevisionDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMBOMPartRevisionDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("GET")]
	[Route("GetPartAssembly/{methodId}/{methodRevisionId}/{methodAssemblyId}")]
	public async Task<IHttpActionResult> GetPartAssemblyAsync([FromUri(Name = "methodId")] string methodId, [FromUri(Name = "methodRevisionId")] string methodRevisionId, [FromUri(Name = "methodAssemblyId")] int methodAssemblyId)
	{
		using (bomPartModel = new BOMPartModel())
		{
			methodRevisionId = ((methodRevisionId == "_") ? string.Empty : methodRevisionId);
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_DeletePartAssembly(methodId, methodRevisionId, methodAssemblyId).Result, () => bomPartModel.Process_GetPartAssembly(methodId, methodRevisionId, methodAssemblyId), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Returns part and part assembly details for a given M1 method assembly GUID.
	/// </summary>
	/// <returns>BOMPartRevisionDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMBOMPartRevisionDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("GET")]
	[Route("GetPartAssembly/{methodAsmGuid}")]
	public async Task<IHttpActionResult> GetPartAssemblyAsync([FromUri(Name = "methodAsmGuid")] string methodAsmGuid)
	{
		using (bomPartModel = new BOMPartModel())
		{
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_GetPartMethodBOM_Guid(methodAsmGuid).Result, () => bomPartModel.Process_GetPartAssembly(bomPartModel.PartKeyDictionary["imaMethodID"].ToString(), bomPartModel.PartKeyDictionary["imaMethodRevisionID"].ToString(), Convert.ToInt32(bomPartModel.PartKeyDictionary["imaMethodAssemblyID"])), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Creates a part assembly based on input parameter.
	/// </summary>
	/// <param name="partAssembly">The part assembly object as BOMPartAssembly</param>
	/// <returns>APIResponseMessageDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("POST")]
	[Route("PostPartAssembly")]
	public async Task<IHttpActionResult> PostPartAssemblyAsync([FromBody] BOMPartAssemblyDto partAssembly)
	{
		using (bomPartModel = new BOMPartModel())
		{
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_ProcessPartAssembly(partAssembly).Result, () => bomPartModel.Process_PostPartAssembly(partAssembly), showReturnObject: false, showResponseMessage: true);
		}
	}

	/// <summary>
	/// Deletes a part assembly for method id, method revision id and method assembly id. Do not use this method if you have special characters (other than Aa-Zz0-9.-)  in key fields
	/// </summary>
	/// <param name="methodId">The part method id as string</param>
	/// <param name="methodRevisionId">The part method revision id as string. If wants to pass BLANK revision id, use underscore ("_") character instead </param>
	/// <param name="methodAssemblyId">The part method assembly id as integer</param>
	/// <returns>APIResponseMessageDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("DELETE")]
	[Route("DeletePartAssembly/{methodId}/{methodRevisionId}/{methodAssemblyId}")]
	public async Task<IHttpActionResult> DeletePartAssemblyAsync([FromUri(Name = "methodId")] string methodId, [FromUri(Name = "methodRevisionId")] string methodRevisionId, [FromUri(Name = "methodAssemblyId")] int methodAssemblyId)
	{
		using (bomPartModel = new BOMPartModel())
		{
			methodRevisionId = ((methodRevisionId == "_") ? string.Empty : methodRevisionId);
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_DeletePartAssembly(methodId, methodRevisionId, methodAssemblyId).Result, () => bomPartModel.Process_DeletePartAssembly(methodId, methodRevisionId, methodAssemblyId), showReturnObject: false, showResponseMessage: true);
		}
	}

	/// <summary>
	/// Deletes a part assembly for method assembly GUID. Use this method if you have special characters (other than Aa-Zz0-9.-)  in key fields
	/// </summary>
	/// <param name="methodAsmGuid">The methodAsmGuid as a string</param>
	/// <returns></returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("DELETE")]
	[Route("DeletePartAssembly/{methodAsmGuid}")]
	public async Task<IHttpActionResult> DeletePartAssemblyAsync([FromUri(Name = "methodAsmGuid")] string methodAsmGuid)
	{
		using (bomPartModel = new BOMPartModel())
		{
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_DeletePartAssembly_Guid(methodAsmGuid).Result, () => bomPartModel.Process_DeletePartAssembly(bomPartModel.PartKeyDictionary["imaMethodID"].ToString(), bomPartModel.PartKeyDictionary["imaMethodRevisionID"].ToString(), Convert.ToInt32(bomPartModel.PartKeyDictionary["imaMethodAssemblyID"])), showReturnObject: false, showResponseMessage: true);
		}
	}

	/// <summary>
	/// Creates a part operation based on input parameter.
	/// </summary>
	/// <param name="partOperation">The part operation as BOMPartAssembly</param>
	/// <returns>APIResponseMessageDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("POST")]
	[Route("PostPartOperation")]
	public async Task<IHttpActionResult> PostPartOperationAsync([FromBody] BOMPartOperationDto partOperation)
	{
		using (bomPartModel = new BOMPartModel())
		{
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_ProcessPartOperation(partOperation).Result, () => bomPartModel.Process_PostPartOperation(partOperation), showReturnObject: false, showResponseMessage: true);
		}
	}

	/// <summary>
	/// Deletes a part operation for method id, method revision id, method assembly id and method operation id. Do not use this method if you have special characters (other than Aa-Zz0-9.-)  in key fields
	/// </summary>
	/// <param name="methodId">The part method id as string</param>
	/// <param name="methodRevisionId">The part method revision id as string. If wants to pass BLANK revision id, use underscore ("_") character instead</param>
	/// <param name="methodAssemblyId">The part method assembly id as integer</param>
	/// <param name="methodOperationId">The part method operation id as integer</param>
	/// <returns>APIResponseMessageDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("DELETE")]
	[Route("DeletePartOperation/{methodId}/{methodRevisionId}/{methodAssemblyId}/{methodOperationId}")]
	public async Task<IHttpActionResult> DeletePartOperationAsync([FromUri(Name = "methodId")] string methodId, [FromUri(Name = "methodRevisionId")] string methodRevisionId, [FromUri(Name = "methodAssemblyId")] int methodAssemblyId, [FromUri(Name = "methodOperationId")] int methodOperationId)
	{
		using (bomPartModel = new BOMPartModel())
		{
			methodRevisionId = ((methodRevisionId == "_") ? string.Empty : methodRevisionId);
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_DeletePartOperation(methodId, methodRevisionId, methodAssemblyId, methodOperationId).Result, () => bomPartModel.Process_DeletePartOperation(methodId, methodRevisionId, methodAssemblyId, methodOperationId), showReturnObject: false, showResponseMessage: true);
		}
	}

	/// <summary>
	/// Deletes a part operation for method operation GUID. Use this method if you have special characters (other than Aa-Zz0-9.-)  in key fields
	/// </summary>
	/// <param name="methodOperationGuid">The methodOperationGuid as a string</param>
	/// <returns></returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("DELETE")]
	[Route("DeletePartOperation/{methodOperationGuid}")]
	public async Task<IHttpActionResult> DeletePartOperationAsync([FromUri(Name = "methodOperationGuid")] string methodOperationGuid)
	{
		using (bomPartModel = new BOMPartModel())
		{
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_DeletePartOperation_Guid(methodOperationGuid).Result, () => bomPartModel.Process_DeletePartOperation(bomPartModel.PartKeyDictionary["imoMethodID"].ToString(), bomPartModel.PartKeyDictionary["imoMethodRevisionID"].ToString(), Convert.ToInt32(bomPartModel.PartKeyDictionary["imoMethodAssemblyID"].ToString()), Convert.ToInt32(bomPartModel.PartKeyDictionary["imoMethodOperationID"].ToString())), showReturnObject: false, showResponseMessage: true);
		}
	}

	/// <summary>
	/// Returns all part materials for the given combination of identifiers: methodId, revisionId and assemblyId.
	/// </summary>
	/// <param name="methodId">The M1 Part Method Id</param>
	/// <param name="methodRevisionId">The M1 Part Method Revision Id. If wants to pass BLANK revision id, use underscore ("_") character instead.</param>
	/// <param name="methodAssemblyId">The M1 Part Method Assembly Id.</param>
	/// <returns>BOMPartRevisionDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMPartMaterialDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("GET")]
	[Route("GetPartMaterial/{methodId}/{methodRevisionId}/{methodAssemblyId}")]
	public async Task<IHttpActionResult> GetPartMaterialAsync([FromUri(Name = "methodId")] string methodId, [FromUri(Name = "methodRevisionId")] string methodRevisionId, [FromUri(Name = "methodAssemblyId")] int methodAssemblyId)
	{
		using (bomPartModel = new BOMPartModel())
		{
			methodRevisionId = ((methodRevisionId == "_") ? string.Empty : methodRevisionId);
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_GetPartMaterial(methodId, methodRevisionId, methodAssemblyId).Result, () => bomPartModel.Process_GetPartMaterial(methodId, methodRevisionId, methodAssemblyId), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Returns part material for method material GUID. Use this method if you have special characters (other than Aa-Zz0-9.-) in key fields.
	/// </summary>
	/// <param name="methodMaterialGuid">The methodMaterialGuid as a string</param>
	/// <returns>BOMPartRevisionDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<BOMPartMaterialDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("GET")]
	[Route("GetPartMaterial/{methodMaterialGuid}")]
	public async Task<IHttpActionResult> GetPartMaterialAsync([FromUri(Name = "methodMaterialGuid")] string methodMaterialGuid)
	{
		using (bomPartModel = new BOMPartModel())
		{
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_DeletePartMaterial_Guid(methodMaterialGuid).Result, () => bomPartModel.Process_GetPartMaterial(bomPartModel.PartKeyDictionary["immMethodID"].ToString(), bomPartModel.PartKeyDictionary["immMethodRevisionID"].ToString(), Convert.ToInt32(bomPartModel.PartKeyDictionary["immMethodAssemblyID"].ToString())), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Creates a part material based on input parameter.
	/// </summary>
	/// <param name="partMaterial">The part material as BOMPartMaterialDto</param>
	/// <returns>APIResponseMessageDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BOMResponseMessageDto<BOMPartMaterialDto>))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("POST")]
	[Route("PostPartMaterial")]
	public async Task<IHttpActionResult> PostPartMaterialAsync([FromBody] BOMPartMaterialDto partMaterial)
	{
		using (bomPartModel = new BOMPartModel())
		{
			partMaterial.UseDefaultWarehouseAndBin = true;
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_ProcessPartMaterial(partMaterial).Result, () => bomPartModel.Process_PostPartMaterial(partMaterial), showReturnObject: false, showResponseMessage: true);
		}
	}

	/// <summary>
	/// Deletes a part material for method id, method revision id, method assembly id and method material id. Do not use this method if you have special characters (other than Aa-Zz0-9.-) in key fields
	/// </summary>
	/// <param name="methodId">The part method id as string</param>
	/// <param name="methodRevisionId">The part method revision id as string. If wants to pass BLANK revision id, use underscore ("_") character instead</param>
	/// <param name="methodAssemblyId">The part method assembly id as integer</param>
	/// <param name="methodMaterialId">The part method material id as integer</param>      
	/// <returns>APIResponseMessageDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("DELETE")]
	[Route("DeletePartMaterial/{methodId}/{methodRevisionId}/{methodAssemblyId}/{methodMaterialId}")]
	public async Task<IHttpActionResult> DeletePartMaterialAsync([FromUri(Name = "methodId")] string methodId, [FromUri(Name = "methodRevisionId")] string methodRevisionId, [FromUri(Name = "methodAssemblyId")] int methodAssemblyId, [FromUri(Name = "methodMaterialId")] int methodMaterialId)
	{
		using (bomPartModel = new BOMPartModel())
		{
			methodRevisionId = ((methodRevisionId == "_") ? string.Empty : methodRevisionId);
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_DeletePartMaterial(methodId, methodRevisionId, methodAssemblyId, methodMaterialId).Result, () => bomPartModel.Process_DeletePartMaterial(methodId, methodRevisionId, methodAssemblyId, methodMaterialId), showReturnObject: false, showResponseMessage: true);
		}
	}

	/// <summary>
	/// Deletes a part material for method material GUID. Use this method if you have special characters (other than Aa-Zz0-9.-)  in key fields
	/// </summary>
	/// <param name="methodMaterialGuid">The methodMaterialGuid as a string</param>
	/// <returns></returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("DELETE")]
	[Route("DeletePartMaterial/{methodMaterialGuid}")]
	public async Task<IHttpActionResult> DeletePartMaterialAsync([FromUri(Name = "methodMaterialGuid")] string methodMaterialGuid)
	{
		using (bomPartModel = new BOMPartModel())
		{
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_DeletePartMaterial_Guid(methodMaterialGuid).Result, () => bomPartModel.Process_DeletePartMaterial(bomPartModel.PartKeyDictionary["immMethodID"].ToString(), bomPartModel.PartKeyDictionary["immMethodRevisionID"].ToString(), Convert.ToInt32(bomPartModel.PartKeyDictionary["immMethodAssemblyID"].ToString()), Convert.ToInt32(bomPartModel.PartKeyDictionary["immMethodMaterialID"].ToString())), showReturnObject: false, showResponseMessage: true);
		}
	}

	/// <summary>
	/// Returns part method BOM details for a given M1 part,revision and assembly. Do not use this method if you have special characters (other than Aa-Zz0-9.-) in key fields
	/// </summary>
	/// <param name="partId">The M1 Part Id as a string</param>
	/// <param name="partRevisionId">The part revision id as string. If wants to pass BLANK revision id, use underscore ("_") character instead</param>
	/// <param name="partAssemblyId">The part assembly id as integer</param>
	/// <returns>CTMBOMPartMethodDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMBOMPartMethodDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("GET")]
	[Route("GetPartMethodBOM/{partId}/{partRevisionId}/{partAssemblyId}")]
	public async Task<IHttpActionResult> GetPartMethodBomAsync([FromUri(Name = "partId")] string partId, [FromUri(Name = "partRevisionId")] string partRevisionId, [FromUri(Name = "partAssemblyId")] int partAssemblyId)
	{
		using (bomPartModel = new BOMPartModel())
		{
			partRevisionId = ((partRevisionId == "_") ? string.Empty : partRevisionId);
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_GetPartMethodBOM(partId, partRevisionId, partAssemblyId).Result, () => bomPartModel.Process_GetPartMethodBOM(partId, partRevisionId, partAssemblyId), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Returns part method BOM details for a given part assembly guid. Use this method if you have special characters (other than Aa-Zz0-9.-) in part assembly key fields
	/// </summary>
	/// <param name="partAssemblyGuid">The partAssemblyGuid as a string</param>
	/// <returns></returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMBOMPartMethodDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[AcceptVerbs("GET")]
	[Route("GetPartMethodBOM/{partAssemblyGuid}")]
	public async Task<IHttpActionResult> GetPartMethodBomAsync([FromUri(Name = "partAssemblyGuid")] string partAssemblyGuid)
	{
		using (bomPartModel = new BOMPartModel())
		{
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_GetPartMethodBOM_Guid(partAssemblyGuid).Result, () => bomPartModel.Process_GetPartMethodBOM(bomPartModel.PartKeyDictionary["imaMethodID"].ToString(), bomPartModel.PartKeyDictionary["imaMethodRevisionID"].ToString(), Convert.ToInt32(bomPartModel.PartKeyDictionary["imaMethodAssemblyID"])), showReturnObject: true, showResponseMessage: false);
		}
	}

	/// <summary>
	/// Returns part method GUIDs for a given part. Do not use special characters or url reserved characters for parameter values
	/// </summary>
	/// <param name="partId">The partId or section of partId as string</param>
	/// <returns>CTMBOMPartMethodGuidsDto object</returns>
	[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.Unauthorized)]
	[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CTMBOMPartMethodGuidsDto))]
	[SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string))]
	[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(APIResponseMessageDto))]
	[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(APIResponseMessageDto))]
	[SwaggerOperation(null, Tags = new string[] { "BOM/PART" })]
	[HttpGet]
	[Route("GetPartMethodGUIDs/{partId}")]
	public async Task<IHttpActionResult> GetPartMethodGuidsAsync([FromUri] string partId)
	{
		using (bomPartModel = new BOMPartModel())
		{
			return await RunApiMethod(base.Request, bomPartModel, () => bomPartModel.ValidateRequest_GetPartMethodGUIDs(partId).Result, () => bomPartModel.Process_GetPartMethodGUIDs(partId), showReturnObject: true, showResponseMessage: false);
		}
	}
}
