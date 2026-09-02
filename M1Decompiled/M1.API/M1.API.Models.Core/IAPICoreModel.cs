using System;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Repositories.Core;

namespace M1.API.Models.Core;

public interface IAPICoreModel : IAPIBaseModel, IDisposable
{
	IPartRepository PartRepository { get; set; }

	ICoreRepository CoreRepository { get; set; }

	APIValidationInfoDto APIValidationIsTrueFunction();

	Task<BOMResponseMessageDto<CTMPartClassesDto>> Process_GetPartClassesAll();

	Task<BOMResponseMessageDto<CTMPartGroupsDto>> Process_GetPartGroupsAll();

	Task<BOMResponseMessageDto<CTMProcessDto>> Process_GetProcessesAll();

	Task<BOMResponseMessageDto<CTMWorkCenterDto>> Process_GetWorkCentersAll();

	Task<BOMResponseMessageDto<CTMWarehousesDto>> Process_GetWarehousesAll();

	Task<BOMResponseMessageDto<CTMWarehouseBinsDto>> Process_GetWarehouseBinsAll();
}
