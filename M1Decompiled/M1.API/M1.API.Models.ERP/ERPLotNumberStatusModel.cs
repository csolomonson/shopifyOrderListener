using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPLotNumberStatusModel : ERPBaseModel, IERPLotNumberStatusModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllLotNumberStatuses(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPLotNumberStatusRepository iERPLotNumberStatusRepository = (base.ERPLotNumberStatusRepository = new ERPLotNumberStatusRepository(base.ApiClientContext));
		using (iERPLotNumberStatusRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPLotNumberStatusRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPLotNumberStatusRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPLotNumberStatusRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPLotNumberStatusRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetLotNumberStatus(Guid lotNumberStatusId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLotNumberStatusRepository iERPLotNumberStatusRepository = (base.ERPLotNumberStatusRepository = new ERPLotNumberStatusRepository(base.ApiClientContext));
		using (iERPLotNumberStatusRepository)
		{
			if (!(await base.ERPLotNumberStatusRepository.DoesLotNumberStatusExist(lotNumberStatusId)))
			{
				errorsList.Add($"LotNumberStatus [{lotNumberStatusId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutLotNumberStatus(ERPLotNumberStatusDto lotNumberStatus)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLotNumberStatusRepository iERPLotNumberStatusRepository = (base.ERPLotNumberStatusRepository = new ERPLotNumberStatusRepository(base.ApiClientContext));
		using (iERPLotNumberStatusRepository)
		{
			if (!string.IsNullOrWhiteSpace(lotNumberStatus.absPartID) && !(await base.ERPLotNumberStatusRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { lotNumberStatus.absPartID })))
			{
				errorsList.Add("absPartID [" + lotNumberStatus.absPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberStatus.absPartRevisionID) && !(await base.ERPLotNumberStatusRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { lotNumberStatus.absPartID, lotNumberStatus.absPartRevisionID })))
			{
				errorsList.Add("absPartRevisionID [" + lotNumberStatus.absPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberStatus.absLotNumberID) && !(await base.ERPLotNumberStatusRepository.DoesRecordExistInTableUsingKeys("LotNumbers", new object[3] { "ABLPARTID", "ABLPARTREVISIONID", "ABLLOTNUMBERID" }, new object[3] { lotNumberStatus.absPartID, lotNumberStatus.absPartRevisionID, lotNumberStatus.absLotNumberID })))
			{
				errorsList.Add("absLotNumberID [" + lotNumberStatus.absLotNumberID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberStatus.absPartWarehouseLocationID) && !(await base.ERPLotNumberStatusRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { lotNumberStatus.absPartID, lotNumberStatus.absPartRevisionID, lotNumberStatus.absPartWarehouseLocationID })))
			{
				errorsList.Add("absPartWarehouseLocationID [" + lotNumberStatus.absPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lotNumberStatus.absPartBinID) && !(await base.ERPLotNumberStatusRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { lotNumberStatus.absPartID, lotNumberStatus.absPartRevisionID, lotNumberStatus.absPartWarehouseLocationID, lotNumberStatus.absPartBinID })))
			{
				errorsList.Add("absPartBinID [" + lotNumberStatus.absPartBinID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPLotNumberStatusDto>>> Process_GetAllLotNumberStatuses(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPLotNumberStatusDto> allLotNumberStatusesDto = new List<ERPLotNumberStatusDto>();
		ERPResponseMessageDto<IList<ERPLotNumberStatusDto>> result;
		try
		{
			IERPLotNumberStatusRepository iERPLotNumberStatusRepository = (base.ERPLotNumberStatusRepository = new ERPLotNumberStatusRepository(base.ApiClientContext));
			using (iERPLotNumberStatusRepository)
			{
				foreach (ERPLotNumberStatusInformationDto item2 in await base.ERPLotNumberStatusRepository.GetAllLotNumberStatuses(pageSize, pageNumber, filter, orderBy))
				{
					ERPLotNumberStatusDto item = new ERPLotNumberStatusDto
					{
						absCreatedBy = item2.absCreatedBy,
						absCreatedDate = item2.absCreatedDate,
						absUniqueID = item2.absUniqueID,
						absLotNumberID = item2.absLotNumberID,
						absPartBinID = item2.absPartBinID,
						absPartID = item2.absPartID,
						absPartRevisionID = item2.absPartRevisionID,
						absPartWarehouseLocationID = item2.absPartWarehouseLocationID,
						absQuantity = item2.absQuantity,
						absRowVersion = item2.absRowVersion,
						absStatus = item2.absStatus,
						CustomFields = item2.CustomFields
					};
					allLotNumberStatusesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all LotNumberStatuses]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPLotNumberStatusDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allLotNumberStatusesDto,
				RecordCount = allLotNumberStatusesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLotNumberStatusDto>> Process_GetLotNumberStatus(Guid lotNumberStatusId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPLotNumberStatusDto lotNumberStatusDto = null;
		ERPResponseMessageDto<ERPLotNumberStatusDto> result;
		try
		{
			IERPLotNumberStatusRepository iERPLotNumberStatusRepository = (base.ERPLotNumberStatusRepository = new ERPLotNumberStatusRepository(base.ApiClientContext));
			using (iERPLotNumberStatusRepository)
			{
				ERPLotNumberStatusInformationDto eRPLotNumberStatusInformationDto = await base.ERPLotNumberStatusRepository.GetLotNumberStatus(lotNumberStatusId);
				lotNumberStatusDto = new ERPLotNumberStatusDto
				{
					absCreatedBy = eRPLotNumberStatusInformationDto.absCreatedBy,
					absCreatedDate = eRPLotNumberStatusInformationDto.absCreatedDate,
					absUniqueID = eRPLotNumberStatusInformationDto.absUniqueID,
					absLotNumberID = eRPLotNumberStatusInformationDto.absLotNumberID,
					absPartBinID = eRPLotNumberStatusInformationDto.absPartBinID,
					absPartID = eRPLotNumberStatusInformationDto.absPartID,
					absPartRevisionID = eRPLotNumberStatusInformationDto.absPartRevisionID,
					absPartWarehouseLocationID = eRPLotNumberStatusInformationDto.absPartWarehouseLocationID,
					absQuantity = eRPLotNumberStatusInformationDto.absQuantity,
					absRowVersion = eRPLotNumberStatusInformationDto.absRowVersion,
					absStatus = eRPLotNumberStatusInformationDto.absStatus,
					CustomFields = eRPLotNumberStatusInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the LotNumberStatuses []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLotNumberStatusDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = lotNumberStatusDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLotNumberStatusDto>> Process_PutLotNumberStatus(ERPLotNumberStatusDto lotNumberStatus)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPLotNumberStatusDto createdObject = null;
		ERPResponseMessageDto<ERPLotNumberStatusDto> result;
		try
		{
			IERPLotNumberStatusRepository iERPLotNumberStatusRepository = (base.ERPLotNumberStatusRepository = new ERPLotNumberStatusRepository(base.ApiClientContext));
			using (iERPLotNumberStatusRepository)
			{
				APIValidationInfoDto postResult = await base.ERPLotNumberStatusRepository.SaveLotNumberStatus(lotNumberStatus);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPLotNumberStatusInformationDto eRPLotNumberStatusInformationDto = await base.ERPLotNumberStatusRepository.GetLotNumberStatus(lotNumberStatus.absUniqueID);
					createdObject = new ERPLotNumberStatusDto
					{
						absCreatedBy = eRPLotNumberStatusInformationDto.absCreatedBy,
						absCreatedDate = eRPLotNumberStatusInformationDto.absCreatedDate,
						absUniqueID = eRPLotNumberStatusInformationDto.absUniqueID,
						absLotNumberID = eRPLotNumberStatusInformationDto.absLotNumberID,
						absPartBinID = eRPLotNumberStatusInformationDto.absPartBinID,
						absPartID = eRPLotNumberStatusInformationDto.absPartID,
						absPartRevisionID = eRPLotNumberStatusInformationDto.absPartRevisionID,
						absPartWarehouseLocationID = eRPLotNumberStatusInformationDto.absPartWarehouseLocationID,
						absQuantity = eRPLotNumberStatusInformationDto.absQuantity,
						absRowVersion = eRPLotNumberStatusInformationDto.absRowVersion,
						absStatus = eRPLotNumberStatusInformationDto.absStatus,
						CustomFields = eRPLotNumberStatusInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing LotNumberStatus [{lotNumberStatus.absUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLotNumberStatusDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteLotNumberStatus(Guid lotNumberStatusId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLotNumberStatusRepository iERPLotNumberStatusRepository = (base.ERPLotNumberStatusRepository = new ERPLotNumberStatusRepository(base.ApiClientContext));
		using (iERPLotNumberStatusRepository)
		{
			if (!(await base.ERPLotNumberStatusRepository.DoesLotNumberStatusExist(lotNumberStatusId)))
			{
				base.ErrorsList.Add($"LotNumberStatus [{lotNumberStatusId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPLotNumberStatusInformationDto eRPLotNumberStatusInformationDto = await base.ERPLotNumberStatusRepository.GetLotNumberStatus(lotNumberStatusId);
				string text = await base.ERPLotNumberStatusRepository.WhereUsed("LotNumberStatuses", new object[6] { eRPLotNumberStatusInformationDto.absPartID, eRPLotNumberStatusInformationDto.absPartRevisionID, eRPLotNumberStatusInformationDto.absLotNumberID, eRPLotNumberStatusInformationDto.absPartWarehouseLocationID, eRPLotNumberStatusInformationDto.absPartBinID, eRPLotNumberStatusInformationDto.absStatus }, new object[6] { "absPartID", "absPartRevisionID", "absLotNumberID", "absPartWarehouseLocationID", "absPartBinID", "absStatus" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("LotNumberStatus cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPLotNumberStatusDto>> Process_DeleteLotNumberStatus(Guid lotNumberStatusId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPLotNumberStatusDto> result;
		try
		{
			IERPLotNumberStatusRepository iERPLotNumberStatusRepository = (base.ERPLotNumberStatusRepository = new ERPLotNumberStatusRepository(base.ApiClientContext));
			using (iERPLotNumberStatusRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPLotNumberStatusRepository.DeleteRowFromTable("LotNumberStatuses", "abs", lotNumberStatusId);
				((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
				IList<string> errorsList = base.ErrorsList;
				if (errorsList != null && errorsList.Count > 0)
				{
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of LotNumberStatus [{lotNumberStatusId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLotNumberStatusDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPLotNumberStatusDto()
			};
		}
		return result;
	}
}
