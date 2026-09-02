using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Inventory;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom.Inventory;

namespace M1.API.Models.BOM.Inventory;

public interface IBOMPartBinDetailModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	IDictionary<string, object> PartKeyDictionary { get; set; }

	Task<APIValidationInfoDto> ValidateRequest_GetPartId(string partId);

	Task<BOMResponseMessageDto<CTMBOMPartBinDetailDto>> Process_PostPartBinDetail(string partId);

	Task<BOMResponseMessageDto<IList<BOMPartBinDetailDto>>> Process_GetAllPartBinDetails(int? pageSize, int? pageNumber);

	Task<BOMResponseMessageDto<BOMPartBinDetailDto>> Process_GetPartBinDetail(Guid uniqueId);
}
