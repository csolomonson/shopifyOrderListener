using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Repositories.Core;
using M1.API.Utilities;

namespace M1.API.Models.Core;

public class APICoreModel : APIBaseModel, IAPICoreModel, IAPIBaseModel, IDisposable
{
	public IPartRepository PartRepository { get; set; }

	public ICoreRepository CoreRepository { get; set; }

	public APIValidationInfoDto APIValidationIsTrueFunction()
	{
		return new APIValidationInfoDto();
	}

	public APICoreModel(APIClientContext clientContext)
	{
		base.ApiClientContext = clientContext;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
	}

	public APICoreModel()
	{
		base.ApiClientContext = null;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
	}

	public async Task<BOMResponseMessageDto<CTMPartClassesDto>> Process_GetPartClassesAll()
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		CTMPartClassesDto returnObject = new CTMPartClassesDto();
		BOMResponseMessageDto<CTMPartClassesDto> result;
		try
		{
			IPartRepository partRepository = (PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				returnObject = PartRepository.GetAllPartClasses().Result;
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing part classes.");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<CTMPartClassesDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = returnObject
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<CTMPartGroupsDto>> Process_GetPartGroupsAll()
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		CTMPartGroupsDto returnObject = new CTMPartGroupsDto();
		BOMResponseMessageDto<CTMPartGroupsDto> result;
		try
		{
			IPartRepository partRepository = (PartRepository = new PartRepository(base.ApiClientContext));
			using (partRepository)
			{
				returnObject = PartRepository.GetAllPartGroups().Result;
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing part groups.");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<CTMPartGroupsDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = returnObject
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<CTMProcessDto>> Process_GetProcessesAll()
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		CTMProcessDto returnObject = new CTMProcessDto();
		BOMResponseMessageDto<CTMProcessDto> result;
		try
		{
			ICoreRepository coreRepository = (CoreRepository = new CoreRepository(base.ApiClientContext));
			using (coreRepository)
			{
				returnObject = CoreRepository.GetAllProcesses().Result;
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing processes.");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<CTMProcessDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = returnObject
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<CTMWorkCenterDto>> Process_GetWorkCentersAll()
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		CTMWorkCenterDto returnObject = new CTMWorkCenterDto();
		BOMResponseMessageDto<CTMWorkCenterDto> result;
		try
		{
			ICoreRepository coreRepository = (CoreRepository = new CoreRepository(base.ApiClientContext));
			using (coreRepository)
			{
				returnObject = CoreRepository.GetAllWorkCenters().Result;
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing workcenters.");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<CTMWorkCenterDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = returnObject
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<CTMWarehousesDto>> Process_GetWarehousesAll()
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		CTMWarehousesDto returnObject = new CTMWarehousesDto();
		BOMResponseMessageDto<CTMWarehousesDto> result;
		try
		{
			ICoreRepository coreRepository = (CoreRepository = new CoreRepository(base.ApiClientContext));
			using (coreRepository)
			{
				returnObject = CoreRepository.GetAllWarehouses().Result;
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing warehouses.");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<CTMWarehousesDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = returnObject
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<CTMWarehouseBinsDto>> Process_GetWarehouseBinsAll()
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		CTMWarehouseBinsDto returnObject = new CTMWarehouseBinsDto();
		BOMResponseMessageDto<CTMWarehouseBinsDto> result;
		try
		{
			ICoreRepository coreRepository = (CoreRepository = new CoreRepository(base.ApiClientContext));
			using (coreRepository)
			{
				returnObject = CoreRepository.GetAllWarehouseBins().Result;
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing warehouse bins.");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<CTMWarehouseBinsDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = returnObject
			};
		}
		return result;
	}
}
