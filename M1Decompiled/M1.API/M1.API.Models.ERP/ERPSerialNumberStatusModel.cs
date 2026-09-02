using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSerialNumberStatusModel : ERPBaseModel, IERPSerialNumberStatusModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSerialNumberStatuses(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSerialNumberStatusRepository iERPSerialNumberStatusRepository = (base.ERPSerialNumberStatusRepository = new ERPSerialNumberStatusRepository(base.ApiClientContext));
		using (iERPSerialNumberStatusRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSerialNumberStatusRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSerialNumberStatusRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSerialNumberStatusRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSerialNumberStatusRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSerialNumberStatus(Guid serialNumberStatusId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSerialNumberStatusRepository iERPSerialNumberStatusRepository = (base.ERPSerialNumberStatusRepository = new ERPSerialNumberStatusRepository(base.ApiClientContext));
		using (iERPSerialNumberStatusRepository)
		{
			if (!(await base.ERPSerialNumberStatusRepository.DoesSerialNumberStatusExist(serialNumberStatusId)))
			{
				errorsList.Add($"SerialNumberStatus [{serialNumberStatusId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutSerialNumberStatus(ERPSerialNumberStatusDto serialNumberStatus)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSerialNumberStatusRepository iERPSerialNumberStatusRepository = (base.ERPSerialNumberStatusRepository = new ERPSerialNumberStatusRepository(base.ApiClientContext));
		using (iERPSerialNumberStatusRepository)
		{
			if (!string.IsNullOrWhiteSpace(serialNumberStatus.snsPartID) && !(await base.ERPSerialNumberStatusRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { serialNumberStatus.snsPartID })))
			{
				errorsList.Add("snsPartID [" + serialNumberStatus.snsPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberStatus.snsPartRevisionID) && !(await base.ERPSerialNumberStatusRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { serialNumberStatus.snsPartID, serialNumberStatus.snsPartRevisionID })))
			{
				errorsList.Add("snsPartRevisionID [" + serialNumberStatus.snsPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberStatus.snsSerialNumberID) && !(await base.ERPSerialNumberStatusRepository.DoesRecordExistInTableUsingKeys("SerialNumbers", new object[3] { "IMSPARTID", "IMSPARTREVISIONID", "IMSSERIALNUMBERID" }, new object[3] { serialNumberStatus.snsPartID, serialNumberStatus.snsPartRevisionID, serialNumberStatus.snsSerialNumberID })))
			{
				errorsList.Add("snsSerialNumberID [" + serialNumberStatus.snsSerialNumberID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberStatus.snsPartWarehouseLocationID) && !(await base.ERPSerialNumberStatusRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { serialNumberStatus.snsPartID, serialNumberStatus.snsPartRevisionID, serialNumberStatus.snsPartWarehouseLocationID })))
			{
				errorsList.Add("snsPartWarehouseLocationID [" + serialNumberStatus.snsPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serialNumberStatus.snsPartBinID) && !(await base.ERPSerialNumberStatusRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { serialNumberStatus.snsPartID, serialNumberStatus.snsPartRevisionID, serialNumberStatus.snsPartWarehouseLocationID, serialNumberStatus.snsPartBinID })))
			{
				errorsList.Add("snsPartBinID [" + serialNumberStatus.snsPartBinID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSerialNumberStatusDto>>> Process_GetAllSerialNumberStatuses(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSerialNumberStatusDto> allSerialNumberStatusesDto = new List<ERPSerialNumberStatusDto>();
		ERPResponseMessageDto<IList<ERPSerialNumberStatusDto>> result;
		try
		{
			IERPSerialNumberStatusRepository iERPSerialNumberStatusRepository = (base.ERPSerialNumberStatusRepository = new ERPSerialNumberStatusRepository(base.ApiClientContext));
			using (iERPSerialNumberStatusRepository)
			{
				foreach (ERPSerialNumberStatusInformationDto item2 in await base.ERPSerialNumberStatusRepository.GetAllSerialNumberStatuses(pageSize, pageNumber, filter, orderBy))
				{
					ERPSerialNumberStatusDto item = new ERPSerialNumberStatusDto
					{
						snsCreatedBy = item2.snsCreatedBy,
						snsCreatedDate = item2.snsCreatedDate,
						snsUniqueID = item2.snsUniqueID,
						snsPartBinID = item2.snsPartBinID,
						snsPartID = item2.snsPartID,
						snsPartRevisionID = item2.snsPartRevisionID,
						snsPartWarehouseLocationID = item2.snsPartWarehouseLocationID,
						snsQuantity = item2.snsQuantity,
						snsRowVersion = item2.snsRowVersion,
						snsSerialNumberID = item2.snsSerialNumberID,
						snsStatus = item2.snsStatus,
						CustomFields = item2.CustomFields
					};
					allSerialNumberStatusesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SerialNumberStatuses]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSerialNumberStatusDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSerialNumberStatusesDto,
				RecordCount = allSerialNumberStatusesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSerialNumberStatusDto>> Process_GetSerialNumberStatus(Guid serialNumberStatusId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSerialNumberStatusDto serialNumberStatusDto = null;
		ERPResponseMessageDto<ERPSerialNumberStatusDto> result;
		try
		{
			IERPSerialNumberStatusRepository iERPSerialNumberStatusRepository = (base.ERPSerialNumberStatusRepository = new ERPSerialNumberStatusRepository(base.ApiClientContext));
			using (iERPSerialNumberStatusRepository)
			{
				ERPSerialNumberStatusInformationDto eRPSerialNumberStatusInformationDto = await base.ERPSerialNumberStatusRepository.GetSerialNumberStatus(serialNumberStatusId);
				serialNumberStatusDto = new ERPSerialNumberStatusDto
				{
					snsCreatedBy = eRPSerialNumberStatusInformationDto.snsCreatedBy,
					snsCreatedDate = eRPSerialNumberStatusInformationDto.snsCreatedDate,
					snsUniqueID = eRPSerialNumberStatusInformationDto.snsUniqueID,
					snsPartBinID = eRPSerialNumberStatusInformationDto.snsPartBinID,
					snsPartID = eRPSerialNumberStatusInformationDto.snsPartID,
					snsPartRevisionID = eRPSerialNumberStatusInformationDto.snsPartRevisionID,
					snsPartWarehouseLocationID = eRPSerialNumberStatusInformationDto.snsPartWarehouseLocationID,
					snsQuantity = eRPSerialNumberStatusInformationDto.snsQuantity,
					snsRowVersion = eRPSerialNumberStatusInformationDto.snsRowVersion,
					snsSerialNumberID = eRPSerialNumberStatusInformationDto.snsSerialNumberID,
					snsStatus = eRPSerialNumberStatusInformationDto.snsStatus,
					CustomFields = eRPSerialNumberStatusInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SerialNumberStatuses []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSerialNumberStatusDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = serialNumberStatusDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSerialNumberStatusDto>> Process_PutSerialNumberStatus(ERPSerialNumberStatusDto serialNumberStatus)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPSerialNumberStatusDto createdObject = null;
		ERPResponseMessageDto<ERPSerialNumberStatusDto> result;
		try
		{
			IERPSerialNumberStatusRepository iERPSerialNumberStatusRepository = (base.ERPSerialNumberStatusRepository = new ERPSerialNumberStatusRepository(base.ApiClientContext));
			using (iERPSerialNumberStatusRepository)
			{
				APIValidationInfoDto postResult = await base.ERPSerialNumberStatusRepository.SaveSerialNumberStatus(serialNumberStatus);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPSerialNumberStatusInformationDto eRPSerialNumberStatusInformationDto = await base.ERPSerialNumberStatusRepository.GetSerialNumberStatus(serialNumberStatus.snsUniqueID);
					createdObject = new ERPSerialNumberStatusDto
					{
						snsCreatedBy = eRPSerialNumberStatusInformationDto.snsCreatedBy,
						snsCreatedDate = eRPSerialNumberStatusInformationDto.snsCreatedDate,
						snsUniqueID = eRPSerialNumberStatusInformationDto.snsUniqueID,
						snsPartBinID = eRPSerialNumberStatusInformationDto.snsPartBinID,
						snsPartID = eRPSerialNumberStatusInformationDto.snsPartID,
						snsPartRevisionID = eRPSerialNumberStatusInformationDto.snsPartRevisionID,
						snsPartWarehouseLocationID = eRPSerialNumberStatusInformationDto.snsPartWarehouseLocationID,
						snsQuantity = eRPSerialNumberStatusInformationDto.snsQuantity,
						snsRowVersion = eRPSerialNumberStatusInformationDto.snsRowVersion,
						snsSerialNumberID = eRPSerialNumberStatusInformationDto.snsSerialNumberID,
						snsStatus = eRPSerialNumberStatusInformationDto.snsStatus,
						CustomFields = eRPSerialNumberStatusInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing SerialNumberStatus [{serialNumberStatus.snsUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSerialNumberStatusDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteSerialNumberStatus(Guid serialNumberStatusId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSerialNumberStatusRepository iERPSerialNumberStatusRepository = (base.ERPSerialNumberStatusRepository = new ERPSerialNumberStatusRepository(base.ApiClientContext));
		using (iERPSerialNumberStatusRepository)
		{
			if (!(await base.ERPSerialNumberStatusRepository.DoesSerialNumberStatusExist(serialNumberStatusId)))
			{
				base.ErrorsList.Add($"SerialNumberStatus [{serialNumberStatusId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPSerialNumberStatusInformationDto eRPSerialNumberStatusInformationDto = await base.ERPSerialNumberStatusRepository.GetSerialNumberStatus(serialNumberStatusId);
				string text = await base.ERPSerialNumberStatusRepository.WhereUsed("SerialNumberStatuses", new object[6] { eRPSerialNumberStatusInformationDto.snsPartID, eRPSerialNumberStatusInformationDto.snsPartRevisionID, eRPSerialNumberStatusInformationDto.snsSerialNumberID, eRPSerialNumberStatusInformationDto.snsPartWarehouseLocationID, eRPSerialNumberStatusInformationDto.snsPartBinID, eRPSerialNumberStatusInformationDto.snsStatus }, new object[6] { "snsPartID", "snsPartRevisionID", "snsSerialNumberID", "snsPartWarehouseLocationID", "snsPartBinID", "snsStatus" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("SerialNumberStatus cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPSerialNumberStatusDto>> Process_DeleteSerialNumberStatus(Guid serialNumberStatusId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPSerialNumberStatusDto> result;
		try
		{
			IERPSerialNumberStatusRepository iERPSerialNumberStatusRepository = (base.ERPSerialNumberStatusRepository = new ERPSerialNumberStatusRepository(base.ApiClientContext));
			using (iERPSerialNumberStatusRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPSerialNumberStatusRepository.DeleteRowFromTable("SerialNumberStatuses", "sns", serialNumberStatusId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of SerialNumberStatus [{serialNumberStatusId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSerialNumberStatusDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPSerialNumberStatusDto()
			};
		}
		return result;
	}
}
