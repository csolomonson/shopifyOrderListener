using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;

namespace M1.API.Models.BOM;

public interface IBOMPartModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	IDictionary<string, object> PartKeyDictionary { get; set; }

	Task<APIValidationInfoDto> ValidateRequest_GetPartId(string partId);

	Task<APIValidationInfoDto> ValidateRequest_ProcessPartRevision(CTMBOMPartRevisionDto partRevision);

	Task<APIValidationInfoDto> ValidateRequest_ProcessPartAssembly(BOMPartAssemblyDto partAssembly);

	Task<APIValidationInfoDto> ValidateRequest_DeletePartAssembly(string methodId, string methodRevisionId, int methodAssemblyId);

	Task<BOMResponseMessageDto<CTMBOMPartRevisionDto>> Process_GetPartRevision(string partId);

	Task<BOMResponseMessageDto<CTMBOMPartRevisionDto>> Process_PostPartRevision(CTMBOMPartRevisionDto partRevision);

	Task<BOMResponseMessageDto<IList<BOMPartMethodAssemblyDto>>> Process_GetPartAssembly(string methodId, string methodRevisionId, int methodAssemblyId);

	Task<BOMResponseMessageDto<BOMPartAssemblyDto>> Process_PostPartAssembly(BOMPartAssemblyDto partAssembly);

	Task<BOMResponseMessageDto<BOMPartAssemblyDto>> Process_DeletePartAssembly(string methodId, string methodRevisionId, int methodAssemblyId);

	Task<APIValidationInfoDto> ValidateRequest_ProcessPartOperation(BOMPartOperationDto partOperation);

	Task<BOMResponseMessageDto<BOMPartOperationDto>> Process_PostPartOperation(BOMPartOperationDto partOperation);

	Task<APIValidationInfoDto> ValidateRequest_DeletePartOperation(string methodId, string methodRevisionId, int methodAssemblyId, int methodOperationId);

	Task<BOMResponseMessageDto<BOMPartOperationDto>> Process_DeletePartOperation(string methodId, string methodRevisionId, int methodAssemblyId, int methodOperationId);

	Task<BOMResponseMessageDto<BOMPartMaterialDto>> Process_PostPartMaterial(BOMPartMaterialDto partMaterial);

	Task<APIValidationInfoDto> ValidateRequest_ProcessPartMaterial(BOMPartMaterialDto partMaterial);

	Task<APIValidationInfoDto> ValidateRequest_DeletePartMaterial(string methodId, string methodRevisionId, int methodAssemblyId, int methodMaterialId);

	Task<APIValidationInfoDto> ValidateRequest_GetPartMaterial(string methodId, string methodRevisionId, int methodAssemblyId);

	Task<BOMResponseMessageDto<BOMPartMaterialDto>> Process_DeletePartMaterial(string methodId, string methodRevisionId, int methodAssemblyId, int methodMaterialId);

	Task<BOMResponseMessageDto<IList<BOMPartMaterialDto>>> Process_GetPartMaterial(string methodId, string methodRevisionId, int methodAssemblyId);

	Task<APIValidationInfoDto> ValidateRequest_GetPartMethodBOM(string partId, string partRevisionId, int partAssemblyId);

	Task<BOMResponseMessageDto<CTMBOMPartMethodDto>> Process_GetPartMethodBOM(string partId, string partRevisionId, int partAssemblyId);

	Task<APIValidationInfoDto> ValidateRequest_DeletePartAssembly_Guid(string methodAsmGuid);

	Task<APIValidationInfoDto> ValidateRequest_DeletePartOperation_Guid(string methodOperationGuid);

	Task<APIValidationInfoDto> ValidateRequest_DeletePartMaterial_Guid(string methodMaterialGuid);

	Task<APIValidationInfoDto> ValidateRequest_GetPartMethodBOM_Guid(string partAssemblyGuid);

	Task<APIValidationInfoDto> ValidateRequest_GetPartMethodGUIDs(string partId);

	Task<BOMResponseMessageDto<CTMBOMPartMethodGuidsDto>> Process_GetPartMethodGUIDs(string partId);

	Task<BOMResponseMessageDto<BOMPartDto>> Process_GetParts(string partId);

	Task<BOMResponseMessageDto<IList<BOMPartDto>>> Process_GetAllParts(int pageSize, int pageNumber);

	Task<BOMResponseMessageDto<BOMPartDto>> Process_PostPart(BOMPartDto partDto);

	Task<APIValidationInfoDto> ValidateRequest_ProcessPart(BOMPartDto partDto);
}
